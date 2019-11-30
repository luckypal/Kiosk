using System.IO;
using wyUpdate.Common;

namespace wyUpdate
{
	public class UninstallFileInfo
	{
		public string Path;

		public bool DeleteFile;

		public bool UnNGENFile;

		public CPUVersion CPUVersion;

		public FrameworkVersion FrameworkVersion;

		public COMRegistration RegisterCOMDll;

		public static UninstallFileInfo Read(Stream fs)
		{
			UninstallFileInfo uninstallFileInfo = new UninstallFileInfo();
			byte b = (byte)fs.ReadByte();
			while (!ReadFiles.ReachedEndByte(fs, b, 154))
			{
				switch (b)
				{
				case 1:
					uninstallFileInfo.Path = ReadFiles.ReadDeprecatedString(fs);
					break;
				case 2:
					uninstallFileInfo.DeleteFile = ReadFiles.ReadBool(fs);
					break;
				case 3:
					uninstallFileInfo.UnNGENFile = ReadFiles.ReadBool(fs);
					break;
				case 4:
					uninstallFileInfo.CPUVersion = (CPUVersion)ReadFiles.ReadInt(fs);
					break;
				case 5:
					uninstallFileInfo.FrameworkVersion = (FrameworkVersion)ReadFiles.ReadInt(fs);
					break;
				case 6:
					uninstallFileInfo.RegisterCOMDll = (COMRegistration)ReadFiles.ReadInt(fs);
					break;
				default:
					ReadFiles.SkipField(fs, b);
					break;
				}
				b = (byte)fs.ReadByte();
			}
			return uninstallFileInfo;
		}

		public void Write(Stream fs, bool comFiles)
		{
			if (comFiles)
			{
				fs.WriteByte(139);
			}
			else
			{
				fs.WriteByte(138);
			}
			WriteFiles.WriteDeprecatedString(fs, 1, Path);
			if (DeleteFile)
			{
				WriteFiles.WriteBool(fs, 2, val: true);
			}
			if (UnNGENFile)
			{
				WriteFiles.WriteBool(fs, 3, val: true);
				WriteFiles.WriteInt(fs, 4, (int)CPUVersion);
				WriteFiles.WriteInt(fs, 5, (int)FrameworkVersion);
			}
			if (RegisterCOMDll != 0)
			{
				WriteFiles.WriteInt(fs, 6, (int)RegisterCOMDll);
			}
			fs.WriteByte(154);
		}
	}
}
