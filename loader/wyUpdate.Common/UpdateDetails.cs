using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace wyUpdate.Common
{
	public class UpdateDetails
	{
		public List<RegChange> RegistryModifications = new List<RegChange>();

		public List<UpdateFile> UpdateFiles = new List<UpdateFile>();

		public List<ShortcutInfo> ShortcutInfos = new List<ShortcutInfo>();

		public List<string> PreviousCommonDesktopShortcuts = new List<string>();

		public List<string> PreviousCommonSMenuShortcuts = new List<string>();

		public List<string> PreviousCUserDesktopShortcuts = new List<string>();

		public List<string> PreviousCUserSMenuShortcuts = new List<string>();

		public List<string> FoldersToDelete = new List<string>();

		public List<string> ServicesToStop = new List<string>();

		public List<StartService> ServicesToStart = new List<StartService>();

		public static UpdateDetails Load(string fileName)
		{
			UpdateDetails updateDetails = new UpdateDetails();
			try
			{
				using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
				{
					if (!ReadFiles.IsHeaderValid(fileStream, "IUUDFV2"))
					{
						throw new ArgumentException("Incorrect file identifier.");
					}
					UpdateFile updateFile = new UpdateFile();
					int num = 0;
					byte b = (byte)fileStream.ReadByte();
					while (!ReadFiles.ReachedEndByte(fileStream, b, byte.MaxValue))
					{
						switch (b)
						{
						case 32:
							updateDetails.RegistryModifications = new List<RegChange>(ReadFiles.ReadInt(fileStream));
							break;
						case 33:
							updateDetails.UpdateFiles = new List<UpdateFile>(ReadFiles.ReadInt(fileStream));
							break;
						case 142:
							updateDetails.RegistryModifications.Add(RegChange.ReadFromStream(fileStream));
							break;
						case 48:
							updateDetails.PreviousCommonDesktopShortcuts.Add(ReadFiles.ReadDeprecatedString(fileStream));
							break;
						case 49:
							updateDetails.PreviousCommonSMenuShortcuts.Add(ReadFiles.ReadDeprecatedString(fileStream));
							break;
						case 54:
							updateDetails.PreviousCUserDesktopShortcuts.Add(ReadFiles.ReadString(fileStream));
							break;
						case 55:
							updateDetails.PreviousCUserSMenuShortcuts.Add(ReadFiles.ReadString(fileStream));
							break;
						case 50:
							updateDetails.ServicesToStop.Add(ReadFiles.ReadString(fileStream));
							break;
						case 51:
							updateDetails.ServicesToStart.Add(new StartService(ReadFiles.ReadString(fileStream)));
							break;
						case 52:
							updateDetails.ServicesToStart[updateDetails.ServicesToStart.Count - 1].Arguments = new string[ReadFiles.ReadInt(fileStream)];
							num = 0;
							break;
						case 53:
							updateDetails.ServicesToStart[updateDetails.ServicesToStart.Count - 1].Arguments[num] = ReadFiles.ReadString(fileStream);
							num++;
							break;
						case 64:
							updateFile.RelativePath = ReadFiles.ReadDeprecatedString(fileStream);
							break;
						case 65:
							updateFile.Execute = ReadFiles.ReadBool(fileStream);
							break;
						case 66:
							updateFile.ExBeforeUpdate = ReadFiles.ReadBool(fileStream);
							break;
						case 67:
							updateFile.CommandLineArgs = ReadFiles.ReadDeprecatedString(fileStream);
							break;
						case 68:
							updateFile.IsNETAssembly = ReadFiles.ReadBool(fileStream);
							break;
						case 69:
							updateFile.WaitForExecution = ReadFiles.ReadBool(fileStream);
							break;
						case 143:
							updateFile.RollbackOnNonZeroRet = true;
							break;
						case 77:
							if (updateFile.RetExceptions == null)
							{
								updateFile.RetExceptions = new List<int>();
							}
							updateFile.RetExceptions.Add(ReadFiles.ReadInt(fileStream));
							break;
						case 70:
							updateFile.DeleteFile = ReadFiles.ReadBool(fileStream);
							break;
						case 71:
							updateFile.DeltaPatchRelativePath = ReadFiles.ReadDeprecatedString(fileStream);
							break;
						case 72:
							updateFile.NewFileAdler32 = ReadFiles.ReadLong(fileStream);
							break;
						case 73:
							updateFile.CPUVersion = (CPUVersion)ReadFiles.ReadInt(fileStream);
							break;
						case 74:
							updateFile.ProcessWindowStyle = (ProcessWindowStyle)ReadFiles.ReadInt(fileStream);
							break;
						case 78:
							updateFile.ElevationType = (ElevationType)ReadFiles.ReadInt(fileStream);
							break;
						case 75:
							updateFile.FrameworkVersion = (FrameworkVersion)ReadFiles.ReadInt(fileStream);
							break;
						case 76:
							updateFile.RegisterCOMDll = (COMRegistration)ReadFiles.ReadInt(fileStream);
							break;
						case 155:
							updateDetails.UpdateFiles.Add(updateFile);
							updateFile = new UpdateFile();
							break;
						case 141:
							updateDetails.ShortcutInfos.Add(ShortcutInfo.LoadFromStream(fileStream));
							break;
						case 96:
							updateDetails.FoldersToDelete.Add(ReadFiles.ReadDeprecatedString(fileStream));
							break;
						default:
							ReadFiles.SkipField(fileStream, b);
							break;
						}
						b = (byte)fileStream.ReadByte();
					}
					return updateDetails;
				}
			}
			catch (Exception ex)
			{
				throw new Exception("The update details file failed to open.\n\nFull details: " + ex.Message);
			}
		}
	}
}
