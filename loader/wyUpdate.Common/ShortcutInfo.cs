using System;
using System.IO;

namespace wyUpdate.Common
{
	public class ShortcutInfo : ICloneable
	{
		public string Path;

		public string WorkingDirectory;

		public string Arguments;

		public string Description;

		public string IconPath;

		public int IconIndex;

		public WindowStyle WindowStyle = WindowStyle.ShowNormal;

		public string RelativeOuputPath;

		public void SaveToStream(Stream fs, bool saveRelativePath)
		{
			fs.WriteByte(141);
			if (!string.IsNullOrEmpty(Path))
			{
				WriteFiles.WriteDeprecatedString(fs, 1, Path);
			}
			if (!string.IsNullOrEmpty(WorkingDirectory))
			{
				WriteFiles.WriteDeprecatedString(fs, 2, WorkingDirectory);
			}
			if (!string.IsNullOrEmpty(Arguments))
			{
				WriteFiles.WriteDeprecatedString(fs, 3, Arguments);
			}
			if (!string.IsNullOrEmpty(Description))
			{
				WriteFiles.WriteDeprecatedString(fs, 4, Description);
			}
			if (!string.IsNullOrEmpty(IconPath))
			{
				WriteFiles.WriteDeprecatedString(fs, 5, IconPath);
			}
			WriteFiles.WriteInt(fs, 6, IconIndex);
			WriteFiles.WriteInt(fs, 7, (int)WindowStyle);
			if (saveRelativePath)
			{
				WriteFiles.WriteDeprecatedString(fs, 8, RelativeOuputPath);
			}
			fs.WriteByte(154);
		}

		public static ShortcutInfo LoadFromStream(Stream fs)
		{
			ShortcutInfo shortcutInfo = new ShortcutInfo();
			byte b = (byte)fs.ReadByte();
			while (!ReadFiles.ReachedEndByte(fs, b, 154))
			{
				switch (b)
				{
				case 1:
					shortcutInfo.Path = ReadFiles.ReadDeprecatedString(fs);
					break;
				case 2:
					shortcutInfo.WorkingDirectory = ReadFiles.ReadDeprecatedString(fs);
					break;
				case 3:
					shortcutInfo.Arguments = ReadFiles.ReadDeprecatedString(fs);
					break;
				case 4:
					shortcutInfo.Description = ReadFiles.ReadDeprecatedString(fs);
					break;
				case 5:
					shortcutInfo.IconPath = ReadFiles.ReadDeprecatedString(fs);
					break;
				case 6:
					shortcutInfo.IconIndex = ReadFiles.ReadInt(fs);
					break;
				case 7:
					shortcutInfo.WindowStyle = (WindowStyle)ReadFiles.ReadInt(fs);
					break;
				case 8:
					shortcutInfo.RelativeOuputPath = ReadFiles.ReadDeprecatedString(fs);
					break;
				default:
					ReadFiles.SkipField(fs, b);
					break;
				}
				b = (byte)fs.ReadByte();
			}
			return shortcutInfo;
		}

		public object Clone()
		{
			ShortcutInfo shortcutInfo = new ShortcutInfo();
			shortcutInfo.Path = Path;
			shortcutInfo.WorkingDirectory = WorkingDirectory;
			shortcutInfo.Arguments = Arguments;
			shortcutInfo.Description = Description;
			shortcutInfo.IconPath = IconPath;
			shortcutInfo.IconIndex = IconIndex;
			shortcutInfo.WindowStyle = WindowStyle;
			shortcutInfo.RelativeOuputPath = RelativeOuputPath;
			return shortcutInfo;
		}
	}
}
