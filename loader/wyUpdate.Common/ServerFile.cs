using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace wyUpdate.Common
{
	public class ServerFile
	{
		public string NewVersion;

		public List<VersionChoice> VersionChoices = new List<VersionChoice>();

		public string MinClientVersion;

		public string NoUpdateToLatestLinkText;

		public string NoUpdateToLatestLinkURL;

		private bool? catchAllExists;

		public bool CatchAllUpdateExists
		{
			get
			{
				if (!catchAllExists.HasValue)
				{
					catchAllExists = (VersionChoices.Count > 0 && VersionTools.Compare(VersionChoices[VersionChoices.Count - 1].Version, NewVersion) == 0);
				}
				return catchAllExists.Value;
			}
		}

		public static ServerFile Load(string fileName, string updatePathVar, string customUrlArgs)
		{
			ServerFile serverFile = new ServerFile();
			byte[] array = new byte[7];
			Stream stream = null;
			try
			{
				stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
				stream.Read(array, 0, 7);
			}
			catch (Exception)
			{
				stream?.Close();
				throw;
			}
			if (array[0] == 80 && array[1] == 75 && array[2] == 3 && array[3] == 4)
			{
				stream.Close();
				using (ZipFile zipFile = ZipFile.Read(fileName))
				{
					stream = new MemoryStream();
					zipFile["0"].Extract(stream);
				}
				stream.Position = 0L;
				stream.Read(array, 0, 7);
			}
			string @string = Encoding.UTF8.GetString(array);
			if (@string != "IUSDFV2")
			{
				stream.Close();
				throw new Exception("The downloaded server file does not have the correct identifier. This is usually caused by file corruption.");
			}
			serverFile.VersionChoices.Add(new VersionChoice());
			byte b = (byte)stream.ReadByte();
			while (!ReadFiles.ReachedEndByte(stream, b, byte.MaxValue))
			{
				switch (b)
				{
				case 1:
					serverFile.NewVersion = ReadFiles.ReadDeprecatedString(stream);
					break;
				case 7:
					serverFile.MinClientVersion = ReadFiles.ReadDeprecatedString(stream);
					break;
				case 11:
					if (serverFile.VersionChoices.Count > 1 || serverFile.VersionChoices[0].Version != null)
					{
						serverFile.VersionChoices.Add(new VersionChoice());
					}
					serverFile.VersionChoices[serverFile.VersionChoices.Count - 1].Version = ReadFiles.ReadDeprecatedString(stream);
					break;
				case 3:
				{
					string text = ReadFiles.ReadDeprecatedString(stream);
					if (updatePathVar != null)
					{
						text = text.Replace("%updatepath%", updatePathVar);
					}
					text = text.Replace("%urlargs%", customUrlArgs ?? string.Empty);
					ClientFile.AddUniqueString(text, serverFile.VersionChoices[serverFile.VersionChoices.Count - 1].FileSites);
					break;
				}
				case 128:
					serverFile.VersionChoices[serverFile.VersionChoices.Count - 1].RTFChanges = true;
					break;
				case 4:
					serverFile.VersionChoices[serverFile.VersionChoices.Count - 1].Changes = ReadFiles.ReadDeprecatedString(stream);
					break;
				case 9:
					serverFile.VersionChoices[serverFile.VersionChoices.Count - 1].FileSize = ReadFiles.ReadLong(stream);
					break;
				case 8:
					serverFile.VersionChoices[serverFile.VersionChoices.Count - 1].Adler32 = ReadFiles.ReadLong(stream);
					break;
				case 20:
					serverFile.VersionChoices[serverFile.VersionChoices.Count - 1].SignedSHA1Hash = ReadFiles.ReadByteArray(stream);
					break;
				case 10:
					serverFile.VersionChoices[serverFile.VersionChoices.Count - 1].InstallingTo = (InstallingTo)ReadFiles.ReadInt(stream);
					break;
				case 142:
					if (RegChange.ReadFromStream(stream).RegBasekey != RegBasekeys.HKEY_CURRENT_USER)
					{
						serverFile.VersionChoices[serverFile.VersionChoices.Count - 1].InstallingTo |= InstallingTo.NonCurrentUserReg;
					}
					break;
				case 32:
					serverFile.NoUpdateToLatestLinkText = ReadFiles.ReadDeprecatedString(stream);
					break;
				case 33:
					serverFile.NoUpdateToLatestLinkURL = ReadFiles.ReadDeprecatedString(stream);
					break;
				case 15:
					stream.Position += 4L;
					break;
				default:
					ReadFiles.SkipField(stream, b);
					break;
				}
				b = (byte)stream.ReadByte();
			}
			stream.Close();
			return serverFile;
		}

		public VersionChoice GetVersionChoice(string installedVersion)
		{
			VersionChoice versionChoice = null;
			for (int i = 0; i < VersionChoices.Count; i++)
			{
				if (VersionTools.Compare(VersionChoices[i].Version, installedVersion) == 0)
				{
					versionChoice = VersionChoices[i];
					break;
				}
			}
			if (versionChoice == null && CatchAllUpdateExists)
			{
				versionChoice = VersionChoices[VersionChoices.Count - 1];
			}
			if (versionChoice == null)
			{
				throw new NoUpdatePathToNewestException();
			}
			return versionChoice;
		}
	}
}
