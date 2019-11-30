using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using wyUpdate.Common;

namespace wyDay.Controls
{
	internal class AutoUpdaterInfo
	{
		public DateTime LastCheckedForUpdate;

		public UpdateStepOn UpdateStepOn;

		public AutoUpdaterStatus AutoUpdaterStatus;

		public string UpdateVersion;

		public string ChangesInLatestVersion;

		public bool ChangesIsRTF;

		public string ErrorTitle;

		public string ErrorMessage;

		private readonly string autoUpdateID;

		private readonly string[] filenames = new string[2];

		public string AutoUpdateID
		{
			get
			{
				if (string.IsNullOrEmpty(autoUpdateID))
				{
					return Path.GetFileName(VersionTools.SelfLocation);
				}
				return autoUpdateID;
			}
		}

		public AutoUpdaterInfo(string auID, string oldAUTempFolder)
		{
			autoUpdateID = auID;
			filenames[0] = GetFilename();
			if (oldAUTempFolder != null && !SystemFolders.IsDirInDir(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), oldAUTempFolder))
			{
				filenames[1] = Path.Combine(oldAUTempFolder, "..\\..\\" + AutoUpdateID + ".autoupdate");
				if (!File.Exists(filenames[1]))
				{
					filenames[1] = null;
				}
			}
			bool flag = false;
			int num = 0;
			bool flag2;
			while (true)
			{
				try
				{
					if (filenames[1] != null && !flag)
					{
						Load(filenames[1]);
					}
					else
					{
						Load(filenames[0]);
					}
					flag2 = false;
				}
				catch (IOException e)
				{
					int hRForException = Marshal.GetHRForException(e);
					if ((hRForException & 0xFFFF) == 32)
					{
						Thread.Sleep(500);
						if (num != 20)
						{
							num++;
							continue;
						}
					}
					flag2 = true;
					if (!flag)
					{
						flag = true;
						continue;
					}
				}
				catch
				{
					flag2 = true;
					if (!flag)
					{
						flag = true;
						continue;
					}
				}
				break;
			}
			if (flag2)
			{
				LastCheckedForUpdate = DateTime.MinValue;
				UpdateStepOn = UpdateStepOn.Nothing;
			}
		}

		private string GetFilename()
		{
			string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "wyUpdate AU");
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
				File.SetAttributes(text, FileAttributes.Hidden | FileAttributes.System);
			}
			return Path.Combine(text, AutoUpdateID + ".autoupdate");
		}

		public void Save()
		{
			int num = 0;
			while (true)
			{
				try
				{
					Save(filenames[0]);
					if (filenames[1] != null)
					{
						Save(filenames[1]);
					}
					return;
				}
				catch (IOException e)
				{
					int hRForException = Marshal.GetHRForException(e);
					if ((hRForException & 0xFFFF) != 32)
					{
						throw;
					}
					Thread.Sleep(500);
					if (num == 20)
					{
						throw;
					}
					num++;
				}
			}
		}

		private void Save(string filename)
		{
			using (FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write))
			{
				WriteFiles.WriteHeader(fileStream, "AUIF");
				UpdateStepOn = UpdateStepOn.Nothing;
				WriteFiles.WriteDateTime(fileStream, 1, LastCheckedForUpdate);
				WriteFiles.WriteInt(fileStream, 2, (int)UpdateStepOn);
				WriteFiles.WriteInt(fileStream, 3, (int)AutoUpdaterStatus);
				if (!string.IsNullOrEmpty(UpdateVersion))
				{
					WriteFiles.WriteString(fileStream, 4, UpdateVersion);
				}
				if (!string.IsNullOrEmpty(ChangesInLatestVersion))
				{
					WriteFiles.WriteString(fileStream, 5, ChangesInLatestVersion);
					WriteFiles.WriteBool(fileStream, 6, ChangesIsRTF);
				}
				if (!string.IsNullOrEmpty(ErrorTitle))
				{
					WriteFiles.WriteString(fileStream, 7, ErrorTitle);
				}
				if (!string.IsNullOrEmpty(ErrorMessage))
				{
					WriteFiles.WriteString(fileStream, 8, ErrorMessage);
				}
				fileStream.WriteByte(byte.MaxValue);
			}
		}

		private void Load(string filename)
		{
			using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
			{
				if (!ReadFiles.IsHeaderValid(fileStream, "AUIF"))
				{
					fileStream.Close();
					throw new Exception("Auto update state file ID is wrong.");
				}
				byte b = (byte)fileStream.ReadByte();
				while (!ReadFiles.ReachedEndByte(fileStream, b, byte.MaxValue))
				{
					switch (b)
					{
					case 1:
						LastCheckedForUpdate = ReadFiles.ReadDateTime(fileStream);
						break;
					case 2:
						UpdateStepOn = (UpdateStepOn)ReadFiles.ReadInt(fileStream);
						break;
					case 3:
						AutoUpdaterStatus = (AutoUpdaterStatus)ReadFiles.ReadInt(fileStream);
						break;
					case 4:
						UpdateVersion = ReadFiles.ReadString(fileStream);
						break;
					case 5:
						ChangesInLatestVersion = ReadFiles.ReadString(fileStream);
						break;
					case 6:
						ChangesIsRTF = ReadFiles.ReadBool(fileStream);
						break;
					case 7:
						ErrorTitle = ReadFiles.ReadString(fileStream);
						break;
					case 8:
						ErrorMessage = ReadFiles.ReadString(fileStream);
						break;
					default:
						ReadFiles.SkipField(fileStream, b);
						break;
					}
					b = (byte)fileStream.ReadByte();
				}
			}
		}

		public void ClearSuccessError()
		{
			AutoUpdaterStatus = AutoUpdaterStatus.Nothing;
			UpdateVersion = null;
			ChangesInLatestVersion = null;
			ChangesIsRTF = false;
			ErrorTitle = null;
			ErrorMessage = null;
		}
	}
}
