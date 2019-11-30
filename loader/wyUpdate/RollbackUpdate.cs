using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using System.Text;
using wyUpdate.Common;

namespace wyUpdate
{
	internal class RollbackUpdate
	{
		public static void RollbackFiles(string m_TempDirectory, string m_ProgramDirectory)
		{
			string path = Path.Combine(m_TempDirectory, "backup");
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			try
			{
				ReadRollbackFiles(Path.Combine(path, "fileList.bak"), list, list2, list3);
			}
			catch
			{
				return;
			}
			foreach (string item in list)
			{
				try
				{
					File.SetAttributes(item, FileAttributes.Normal);
					File.Delete(item);
				}
				catch
				{
				}
			}
			foreach (string item2 in list2)
			{
				try
				{
					Directory.Delete(item2, recursive: true);
				}
				catch
				{
				}
			}
			foreach (string item3 in list3)
			{
				try
				{
					Directory.CreateDirectory(item3);
				}
				catch
				{
				}
			}
			string[] array = new string[13]
			{
				Path.Combine(path, "base"),
				Path.Combine(path, "system"),
				Path.Combine(path, "64system"),
				Path.Combine(path, "root"),
				Path.Combine(path, "appdata"),
				Path.Combine(path, "lappdata"),
				Path.Combine(path, "comappdata"),
				Path.Combine(path, "comdesktop"),
				Path.Combine(path, "comstartmenu"),
				Path.Combine(path, "cp86"),
				Path.Combine(path, "cp64"),
				Path.Combine(path, "curdesk"),
				Path.Combine(path, "curstart")
			};
			string[] array2 = new string[13]
			{
				m_ProgramDirectory,
				SystemFolders.GetSystem32x86(),
				SystemFolders.GetSystem32x64(),
				SystemFolders.GetRootDrive(),
				SystemFolders.GetCurrentUserAppData(),
				SystemFolders.GetCurrentUserLocalAppData(),
				SystemFolders.GetCommonAppData(),
				SystemFolders.GetCommonDesktop(),
				SystemFolders.GetCommonProgramsStartMenu(),
				SystemFolders.GetCommonProgramFilesx86(),
				SystemFolders.GetCommonProgramFilesx64(),
				SystemFolders.GetCurrentUserDesktop(),
				SystemFolders.GetCurrentUserProgramsStartMenu()
			};
			for (int i = 0; i < array.Length; i++)
			{
				if (Directory.Exists(array[i]) && array2[i] != null)
				{
					RestoreFiles(array2[i], array[i]);
				}
			}
		}

		public static void RestoreFiles(string destDir, string backupDir)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(backupDir);
			FileInfo[] files = directoryInfo.GetFiles("*");
			FileInfo[] array = files;
			foreach (FileInfo fileInfo in array)
			{
				try
				{
					string text = Path.Combine(destDir, fileInfo.Name);
					FileAttributes attributes = fileInfo.Attributes;
					bool flag = (attributes & FileAttributes.Hidden) != 0 || (attributes & FileAttributes.ReadOnly) != 0 || (attributes & FileAttributes.System) != 0;
					if (flag)
					{
						File.SetAttributes(text, FileAttributes.Normal);
					}
					File.Copy(fileInfo.FullName, text, overwrite: true);
					if (flag)
					{
						File.SetAttributes(text, attributes);
					}
				}
				catch
				{
				}
			}
			DirectoryInfo[] directories = directoryInfo.GetDirectories("*");
			DirectoryInfo[] array2 = directories;
			foreach (DirectoryInfo directoryInfo2 in array2)
			{
				RestoreFiles(Path.Combine(destDir, directoryInfo2.Name), directoryInfo2.FullName);
			}
		}

		public static void RollbackRegistry(string m_TempDirectory)
		{
			List<RegChange> list = new List<RegChange>();
			try
			{
				ReadRollbackRegistry(Path.Combine(m_TempDirectory, "backup\\regList.bak"), list);
			}
			catch
			{
				return;
			}
			foreach (RegChange item in list)
			{
				try
				{
					item.ExecuteOperation();
				}
				catch
				{
				}
			}
		}

		public static void RollbackUnregedCOM(string tempDir)
		{
			List<UninstallFileInfo> list = new List<UninstallFileInfo>();
			try
			{
				ReadRollbackCOM(Path.Combine(tempDir, "backup\\unreggedComList.bak"), list);
			}
			catch
			{
				return;
			}
			foreach (UninstallFileInfo item in list)
			{
				try
				{
					InstallUpdate.RegisterDllServer(item.Path, Uninstall: false);
				}
				catch
				{
				}
			}
		}

		public static void RollbackRegedCOM(string tempDir)
		{
			List<UninstallFileInfo> list = new List<UninstallFileInfo>();
			try
			{
				ReadRollbackCOM(Path.Combine(tempDir, "backup\\reggedComList.bak"), list);
			}
			catch
			{
				return;
			}
			foreach (UninstallFileInfo item in list)
			{
				try
				{
					InstallUpdate.RegisterDllServer(item.Path, Uninstall: true);
				}
				catch
				{
				}
			}
		}

		public static void RollbackStoppedServices(string tempDir)
		{
			List<string> list = new List<string>();
			try
			{
				ReadRollbackServices(Path.Combine(tempDir, "backup\\stoppedServices.bak"), list, addUnique: false);
			}
			catch
			{
				return;
			}
			foreach (string item in list)
			{
				try
				{
					using (ServiceController serviceController = new ServiceController(item))
					{
						serviceController.Start();
						serviceController.WaitForStatus(ServiceControllerStatus.Running);
					}
				}
				catch
				{
				}
			}
			try
			{
				Directory.Delete(tempDir, recursive: true);
			}
			catch
			{
			}
		}

		public static void RollbackStartedServices(string tempDir)
		{
			List<string> list = new List<string>();
			try
			{
				ReadRollbackServices(Path.Combine(tempDir, "backup\\startedServices.bak"), list, addUnique: false);
			}
			catch
			{
				return;
			}
			foreach (string item in list)
			{
				try
				{
					using (ServiceController serviceController = new ServiceController(item))
					{
						serviceController.Stop();
						serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
					}
				}
				catch
				{
				}
			}
		}

		public static void WriteRollbackRegistry(string fileName, List<RegChange> rollbackRegistry)
		{
			if (rollbackRegistry.Count != 0)
			{
				using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
				{
					WriteFiles.WriteHeader(fileStream, "IURURV1");
					WriteFiles.WriteInt(fileStream, 1, rollbackRegistry.Count);
					foreach (RegChange item in rollbackRegistry)
					{
						item.WriteToStream(fileStream, embedBinaryData: true);
					}
					fileStream.WriteByte(byte.MaxValue);
				}
			}
		}

		public static void ReadRollbackRegistry(string fileName, List<RegChange> rollbackRegistry)
		{
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				if (!ReadFiles.IsHeaderValid(fileStream, "IURURV1"))
				{
					throw new Exception("Identifier incorrect");
				}
				byte b = (byte)fileStream.ReadByte();
				while (!ReadFiles.ReachedEndByte(fileStream, b, byte.MaxValue))
				{
					switch (b)
					{
					case 1:
						rollbackRegistry.Capacity = ReadFiles.ReadInt(fileStream);
						break;
					case 142:
						rollbackRegistry.Add(RegChange.ReadFromStream(fileStream));
						break;
					default:
						ReadFiles.SkipField(fileStream, b);
						break;
					}
					b = (byte)fileStream.ReadByte();
				}
			}
		}

		public static void WriteRollbackServices(string fileName, List<string> rollbackList)
		{
			if (rollbackList.Count != 0)
			{
				using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
				{
					fileStream.Write(Encoding.UTF8.GetBytes("IURUSV1"), 0, 7);
					foreach (string rollback in rollbackList)
					{
						WriteFiles.WriteString(fileStream, 1, rollback);
					}
					fileStream.WriteByte(byte.MaxValue);
				}
			}
		}

		public static void ReadRollbackServices(string fileName, List<string> rollbackList, bool addUnique)
		{
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				if (!ReadFiles.IsHeaderValid(fileStream, "IURUSV1"))
				{
					throw new Exception("Identifier incorrect");
				}
				byte b = (byte)fileStream.ReadByte();
				while (!ReadFiles.ReachedEndByte(fileStream, b, byte.MaxValue))
				{
					byte b2 = b;
					if (b2 == 1)
					{
						if (addUnique)
						{
							ClientFile.AddUniqueString(ReadFiles.ReadString(fileStream), rollbackList);
						}
						else
						{
							rollbackList.Add(ReadFiles.ReadString(fileStream));
						}
					}
					else
					{
						ReadFiles.SkipField(fileStream, b);
					}
					b = (byte)fileStream.ReadByte();
				}
			}
		}

		public static void WriteRollbackFiles(string fileName, List<FileFolder> rollbackList)
		{
			if (rollbackList.Count != 0)
			{
				using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
				{
					fileStream.Write(Encoding.UTF8.GetBytes("IURUFV1"), 0, 7);
					foreach (FileFolder rollback in rollbackList)
					{
						if (rollback.isFolder)
						{
							if (rollback.deleteFolder)
							{
								WriteFiles.WriteString(fileStream, 4, rollback.Path);
							}
							else
							{
								WriteFiles.WriteString(fileStream, 6, rollback.Path);
							}
						}
						else
						{
							WriteFiles.WriteString(fileStream, 2, rollback.Path);
						}
					}
					fileStream.WriteByte(byte.MaxValue);
				}
			}
		}

		public static void ReadRollbackFiles(string fileName, List<string> rollbackFiles, List<string> rollbackFolders, List<string> createFolders)
		{
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				if (!ReadFiles.IsHeaderValid(fileStream, "IURUFV1"))
				{
					throw new Exception("Identifier incorrect");
				}
				byte b = (byte)fileStream.ReadByte();
				while (!ReadFiles.ReachedEndByte(fileStream, b, byte.MaxValue))
				{
					switch (b)
					{
					case 2:
						rollbackFiles.Add(ReadFiles.ReadString(fileStream));
						break;
					case 4:
						rollbackFolders.Add(ReadFiles.ReadString(fileStream));
						break;
					case 6:
						if (createFolders != null)
						{
							createFolders.Add(ReadFiles.ReadString(fileStream));
						}
						else
						{
							ReadFiles.SkipField(fileStream, b);
						}
						break;
					default:
						ReadFiles.SkipField(fileStream, b);
						break;
					}
					b = (byte)fileStream.ReadByte();
				}
			}
		}

		public static void WriteRollbackCOM(string fileName, List<UninstallFileInfo> rollbackList)
		{
			if (rollbackList.Count != 0)
			{
				using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
				{
					fileStream.Write(Encoding.UTF8.GetBytes("IURUCV1"), 0, 7);
					foreach (UninstallFileInfo rollback in rollbackList)
					{
						fileStream.WriteByte(139);
						WriteFiles.WriteString(fileStream, 1, rollback.Path);
						WriteFiles.WriteInt(fileStream, 2, (int)rollback.RegisterCOMDll);
						fileStream.WriteByte(155);
					}
					fileStream.WriteByte(byte.MaxValue);
				}
			}
		}

		public static void ReadRollbackCOM(string fileName, List<UninstallFileInfo> rollbackList)
		{
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				if (!ReadFiles.IsHeaderValid(fileStream, "IURUCV1"))
				{
					throw new Exception("Identifier incorrect");
				}
				UninstallFileInfo uninstallFileInfo = new UninstallFileInfo();
				byte b = (byte)fileStream.ReadByte();
				while (!ReadFiles.ReachedEndByte(fileStream, b, byte.MaxValue))
				{
					switch (b)
					{
					case 1:
						uninstallFileInfo.Path = ReadFiles.ReadString(fileStream);
						break;
					case 2:
						uninstallFileInfo.RegisterCOMDll = (COMRegistration)ReadFiles.ReadInt(fileStream);
						break;
					case 155:
						rollbackList.Add(uninstallFileInfo);
						uninstallFileInfo = new UninstallFileInfo();
						break;
					default:
						ReadFiles.SkipField(fileStream, b);
						break;
					}
					b = (byte)fileStream.ReadByte();
				}
			}
		}

		public static void WriteUninstallFile(string tempDir, string uninstallDataFile, List<UpdateFile> updateDetailsFiles)
		{
			string text = Path.Combine(tempDir, "backup\\regList.bak");
			string text2 = Path.Combine(tempDir, "backup\\fileList.bak");
			string text3 = Path.Combine(tempDir, "backup\\reggedComList.bak");
			string text4 = Path.Combine(tempDir, "backup\\startedServices.bak");
			List<UninstallFileInfo> list = new List<UninstallFileInfo>();
			List<string> list2 = new List<string>();
			List<RegChange> list3 = new List<RegChange>();
			List<UninstallFileInfo> list4 = new List<UninstallFileInfo>();
			List<string> list5 = new List<string>();
			try
			{
				if (File.Exists(uninstallDataFile))
				{
					ReadUninstallFile(uninstallDataFile, list, list2, list3, list4, list5);
				}
			}
			catch
			{
			}
			if (File.Exists(text3))
			{
				try
				{
					ReadRollbackCOM(text3, list4);
				}
				catch
				{
				}
			}
			if (File.Exists(text2))
			{
				List<string> list6 = new List<string>();
				try
				{
					ReadRollbackFiles(text2, list6, list2, null);
				}
				catch
				{
				}
				foreach (string item in list6)
				{
					list.Add(new UninstallFileInfo
					{
						Path = item,
						DeleteFile = true
					});
				}
			}
			foreach (UpdateFile updateDetailsFile in updateDetailsFiles)
			{
				if (updateDetailsFile.IsNETAssembly)
				{
					bool flag = true;
					for (int i = 0; i < list.Count; i++)
					{
						if (updateDetailsFile.Filename == list[i].Path)
						{
							if (!list[i].UnNGENFile)
							{
								list[i].UnNGENFile = true;
							}
							flag = false;
							break;
						}
					}
					if (flag)
					{
						list.Add(new UninstallFileInfo
						{
							Path = updateDetailsFile.Filename,
							UnNGENFile = true,
							CPUVersion = updateDetailsFile.CPUVersion
						});
					}
				}
			}
			if (File.Exists(text))
			{
				try
				{
					ReadRollbackRegistry(text, list3);
					for (int j = 0; j < list3.Count; j++)
					{
						if (list3[j].RegOperation != RegOperations.RemoveKey && list3[j].RegOperation != RegOperations.RemoveValue)
						{
							list3.RemoveAt(j);
							j--;
						}
					}
				}
				catch
				{
				}
			}
			if (File.Exists(text4))
			{
				try
				{
					ReadRollbackServices(text4, list5, addUnique: true);
				}
				catch
				{
				}
			}
			if (list.Count != 0 || list2.Count != 0 || list3.Count != 0)
			{
				using (FileStream fileStream = new FileStream(uninstallDataFile, FileMode.Create, FileAccess.Write))
				{
					WriteFiles.WriteHeader(fileStream, "IUUFRV1");
					foreach (UninstallFileInfo item2 in list4)
					{
						item2.Write(fileStream, comFiles: true);
					}
					foreach (UninstallFileInfo item3 in list)
					{
						item3.Write(fileStream, comFiles: false);
					}
					foreach (string item4 in list2)
					{
						WriteFiles.WriteDeprecatedString(fileStream, 16, item4);
					}
					foreach (string item5 in list5)
					{
						WriteFiles.WriteString(fileStream, 17, item5);
					}
					foreach (RegChange item6 in list3)
					{
						item6.WriteToStream(fileStream, embedBinaryData: true);
					}
					fileStream.WriteByte(byte.MaxValue);
				}
			}
		}

		public static void ReadUninstallData(string clientFile, List<UninstallFileInfo> uninstallFiles, List<string> uninstallFolders, List<RegChange> uninstallRegistry, List<UninstallFileInfo> comDllsToUnreg, List<string> servicesToStop)
		{
			try
			{
				using (ZipFile zipFile = ZipFile.Read(clientFile))
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						zipFile["uninstall.dat"].Extract(memoryStream);
						LoadUninstallData(memoryStream, uninstallFiles, uninstallFolders, uninstallRegistry, comDllsToUnreg, servicesToStop);
					}
				}
			}
			catch
			{
			}
		}

		private static void ReadUninstallFile(string uninstallFile, List<UninstallFileInfo> uninstallFiles, List<string> uninstallFolders, List<RegChange> uninstallRegistry, List<UninstallFileInfo> comDllsToUnreg, List<string> servicesToStop)
		{
			using (FileStream ms = new FileStream(uninstallFile, FileMode.Open, FileAccess.Read))
			{
				LoadUninstallData(ms, uninstallFiles, uninstallFolders, uninstallRegistry, comDllsToUnreg, servicesToStop);
			}
		}

		private static void LoadUninstallData(Stream ms, List<UninstallFileInfo> uninstallFiles, List<string> uninstallFolders, List<RegChange> uninstallRegistry, List<UninstallFileInfo> comDllsToUnreg, List<string> servicesToStop)
		{
			ms.Position = 0L;
			if (!ReadFiles.IsHeaderValid(ms, "IUUFRV1"))
			{
				ms.Close();
				throw new Exception("The uninstall file does not have the correct identifier - this is usually caused by file corruption.");
			}
			byte b = (byte)ms.ReadByte();
			while (!ReadFiles.ReachedEndByte(ms, b, byte.MaxValue))
			{
				switch (b)
				{
				case 138:
					uninstallFiles.Add(UninstallFileInfo.Read(ms));
					break;
				case 139:
					comDllsToUnreg.Add(UninstallFileInfo.Read(ms));
					break;
				case 16:
					uninstallFolders.Add(ReadFiles.ReadDeprecatedString(ms));
					break;
				case 17:
					servicesToStop.Add(ReadFiles.ReadString(ms));
					break;
				case 142:
					uninstallRegistry.Add(RegChange.ReadFromStream(ms));
					break;
				default:
					ReadFiles.SkipField(ms, b);
					break;
				}
				b = (byte)ms.ReadByte();
			}
		}
	}
}
