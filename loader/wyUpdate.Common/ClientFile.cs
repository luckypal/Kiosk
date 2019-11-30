using Ionic.Zip;
using Ionic.Zlib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;

namespace wyUpdate.Common
{
	public class ClientFile
	{
		public Hashtable Languages = new Hashtable();

		public string InstalledVersion;

		public UpdateOn CurrentlyUpdating = UpdateOn.DownloadingUpdate;

		public List<string> ServerFileSites = new List<string>(1);

		public List<string> ClientServerSites = new List<string>(1);

		public string CompanyName;

		public string ProductName;

		private string m_GUID;

		public ImageAlign HeaderImageAlign;

		public string HeaderTextColorName;

		public int HeaderTextIndent = -1;

		public bool HideHeaderDivider;

		public Image TopImage;

		public Image SideImage;

		public string TopImageFilename;

		public string SideImageFilename;

		public bool CloseOnSuccess;

		public string CustomWyUpdateTitle;

		public string PublicSignKey;

		public string UpdatePassword;

		public string GUID
		{
			get
			{
				if (string.IsNullOrEmpty(m_GUID))
				{
					char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
					if (ProductName != null && ProductName.IndexOfAny(invalidFileNameChars) != -1)
					{
						List<char> list = new List<char>(invalidFileNameChars);
						StringBuilder stringBuilder = new StringBuilder(ProductName.Length - 1);
						for (int i = 0; i < ProductName.Length; i++)
						{
							if (list.IndexOf(ProductName[i]) == -1)
							{
								stringBuilder.Append(ProductName[i]);
							}
						}
						return stringBuilder.ToString();
					}
					return ProductName;
				}
				return m_GUID;
			}
			set
			{
				m_GUID = value;
			}
		}

		public void OpenObsoleteClientFile(string fileName)
		{
			FileStream fileStream = null;
			try
			{
				fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
			}
			catch (Exception ex)
			{
				fileStream?.Close();
				throw new ArgumentException("The client data file (client.wyc) failed to open.\n\nFull details:\n\n" + ex.Message);
			}
			if (!ReadFiles.IsHeaderValid(fileStream, "IUCDFV2"))
			{
				fileStream.Close();
				throw new ArgumentException("The client file does not have the correct identifier - this is usually caused by file corruption. \n\nA possible solution is to replace the following file by reinstalling:\n\n" + fileName);
			}
			byte b = (byte)fileStream.ReadByte();
			while (!ReadFiles.ReachedEndByte(fileStream, b, byte.MaxValue))
			{
				switch (b)
				{
				case 1:
					CompanyName = ReadFiles.ReadDeprecatedString(fileStream);
					break;
				case 2:
					ProductName = ReadFiles.ReadDeprecatedString(fileStream);
					break;
				case 3:
					InstalledVersion = ReadFiles.ReadDeprecatedString(fileStream);
					break;
				case 4:
					AddUniqueString(ReadFiles.ReadDeprecatedString(fileStream), ServerFileSites);
					break;
				case 9:
					AddUniqueString(ReadFiles.ReadDeprecatedString(fileStream), ClientServerSites);
					break;
				case 17:
					try
					{
						HeaderImageAlign = (ImageAlign)Enum.Parse(typeof(ImageAlign), ReadFiles.ReadDeprecatedString(fileStream));
					}
					catch
					{
					}
					break;
				case 18:
					HeaderTextIndent = ReadFiles.ReadInt(fileStream);
					break;
				case 19:
					HeaderTextColorName = ReadFiles.ReadDeprecatedString(fileStream);
					break;
				case 6:
					TopImage = ReadFiles.ReadImage(fileStream);
					break;
				case 7:
					SideImage = ReadFiles.ReadImage(fileStream);
					break;
				default:
					ReadFiles.SkipField(fileStream, b);
					break;
				}
				b = (byte)fileStream.ReadByte();
			}
			fileStream.Close();
		}

		private void LoadClientData(Stream ms, string updatePathVar, string customUrlArgs)
		{
			ms.Position = 0L;
			if (!ReadFiles.IsHeaderValid(ms, "IUCDFV2"))
			{
				ms.Close();
				throw new Exception("The client file does not have the correct identifier - this is usually caused by file corruption.");
			}
			LanguageCulture languageCulture = null;
			byte b = (byte)ms.ReadByte();
			while (!ReadFiles.ReachedEndByte(ms, b, byte.MaxValue))
			{
				switch (b)
				{
				case 1:
					CompanyName = ReadFiles.ReadDeprecatedString(ms);
					break;
				case 2:
					ProductName = ReadFiles.ReadDeprecatedString(ms);
					break;
				case 10:
					m_GUID = ReadFiles.ReadString(ms);
					break;
				case 3:
					InstalledVersion = ReadFiles.ReadDeprecatedString(ms);
					break;
				case 4:
				{
					string text = ReadFiles.ReadDeprecatedString(ms);
					if (updatePathVar != null)
					{
						text = text.Replace("%updatepath%", updatePathVar);
					}
					if (customUrlArgs != null)
					{
						text = text.Replace("%urlargs%", customUrlArgs);
					}
					AddUniqueString(text, ServerFileSites);
					break;
				}
				case 9:
				{
					string text = ReadFiles.ReadDeprecatedString(ms);
					if (updatePathVar != null)
					{
						text = text.Replace("%updatepath%", updatePathVar);
					}
					if (customUrlArgs != null)
					{
						text = text.Replace("%urlargs%", customUrlArgs);
					}
					AddUniqueString(text, ClientServerSites);
					break;
				}
				case 17:
					try
					{
						HeaderImageAlign = (ImageAlign)Enum.Parse(typeof(ImageAlign), ReadFiles.ReadDeprecatedString(ms));
					}
					catch
					{
					}
					break;
				case 18:
					HeaderTextIndent = ReadFiles.ReadInt(ms);
					break;
				case 19:
					HeaderTextColorName = ReadFiles.ReadDeprecatedString(ms);
					break;
				case 20:
					TopImageFilename = ReadFiles.ReadDeprecatedString(ms);
					break;
				case 21:
					SideImageFilename = ReadFiles.ReadDeprecatedString(ms);
					break;
				case 24:
					languageCulture = new LanguageCulture(ReadFiles.ReadDeprecatedString(ms));
					Languages.Add(languageCulture.Culture, languageCulture);
					break;
				case 22:
					if (languageCulture != null)
					{
						languageCulture.Filename = ReadFiles.ReadDeprecatedString(ms);
					}
					else
					{
						Languages.Add(string.Empty, new LanguageCulture(null)
						{
							Filename = ReadFiles.ReadDeprecatedString(ms)
						});
					}
					break;
				case 23:
					HideHeaderDivider = ReadFiles.ReadBool(ms);
					break;
				case 25:
					CloseOnSuccess = ReadFiles.ReadBool(ms);
					break;
				case 26:
					CustomWyUpdateTitle = ReadFiles.ReadString(ms);
					break;
				case 27:
					PublicSignKey = ReadFiles.ReadString(ms);
					break;
				case 28:
					UpdatePassword = ReadFiles.ReadString(ms);
					break;
				default:
					ReadFiles.SkipField(ms, b);
					break;
				}
				b = (byte)ms.ReadByte();
			}
			ms.Close();
		}

		public void LoadClientData(string filename)
		{
			try
			{
				using (Stream ms = new FileStream(filename, FileMode.Open, FileAccess.Read))
				{
					LoadClientData(ms, null, null);
				}
			}
			catch
			{
			}
		}

		public void OpenClientFile(string m_Filename, ClientLanguage lang, string forcedCulture, string updatePathVar, string customUrlArgs)
		{
			using (ZipFile zipFile = ZipFile.Read(m_Filename))
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					zipFile["iuclient.iuc"].Extract(memoryStream);
					LoadClientData(memoryStream, updatePathVar, customUrlArgs);
				}
				if (!string.IsNullOrEmpty(TopImageFilename))
				{
					using (MemoryStream stream = new MemoryStream())
					{
						zipFile[TopImageFilename].Extract(stream);
						TopImage = Image.FromStream(stream, useEmbeddedColorManagement: true);
					}
				}
				if (!string.IsNullOrEmpty(SideImageFilename))
				{
					using (MemoryStream stream2 = new MemoryStream())
					{
						zipFile[SideImageFilename].Extract(stream2);
						SideImage = Image.FromStream(stream2, useEmbeddedColorManagement: true);
					}
				}
				if (Languages.Count == 1 && Languages.Contains(string.Empty))
				{
					using (MemoryStream memoryStream2 = new MemoryStream())
					{
						zipFile[((LanguageCulture)Languages[string.Empty]).Filename].Extract(memoryStream2);
						lang.Open(memoryStream2);
					}
				}
				else if (Languages.Count > 0)
				{
					LanguageCulture languageCulture = null;
					if (!string.IsNullOrEmpty(forcedCulture))
					{
						languageCulture = (LanguageCulture)Languages[forcedCulture];
					}
					if (languageCulture == null)
					{
						languageCulture = (LanguageCulture)Languages[CultureInfo.CurrentUICulture.Name];
					}
					if (languageCulture == null)
					{
						languageCulture = (LanguageCulture)Languages["en-US"];
					}
					if (languageCulture == null)
					{
						{
							IEnumerator enumerator = Languages.Values.GetEnumerator();
							try
							{
								if (enumerator.MoveNext())
								{
									LanguageCulture languageCulture2 = (LanguageCulture)enumerator.Current;
									languageCulture = languageCulture2;
								}
							}
							finally
							{
								IDisposable disposable = enumerator as IDisposable;
								if (disposable != null)
								{
									disposable.Dispose();
								}
							}
						}
					}
					if (languageCulture != null && !string.IsNullOrEmpty(languageCulture.Filename))
					{
						using (MemoryStream memoryStream3 = new MemoryStream())
						{
							zipFile[languageCulture.Filename].Extract(memoryStream3);
							lang.Open(memoryStream3);
						}
					}
				}
			}
		}

		public void SaveClientFile(List<UpdateFile> files, string outputFilename)
		{
			try
			{
				if (File.Exists(outputFilename))
				{
					File.Delete(outputFilename);
				}
				using (ZipFile zipFile = new ZipFile(outputFilename))
				{
					zipFile.AlternateEncoding = Encoding.UTF8;
					zipFile.AlternateEncodingUsage = ZipOption.AsNecessary;
					zipFile.CompressionLevel = CompressionLevel.Level7;
					for (int i = 0; i < files.Count; i++)
					{
						ZipEntry zipEntry = zipFile.AddFile(files[i].Filename, "");
						zipEntry.FileName = files[i].RelativePath;
						zipEntry.LastModified = File.GetLastWriteTime(files[i].Filename);
					}
					using (Stream stream = SaveClientFile())
					{
						ZipEntry zipEntry = zipFile.AddEntry("iuclient.iuc", stream);
						zipEntry.LastModified = DateTime.Now;
						zipFile.Save();
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(Path.GetFileName(outputFilename) + ". \r\n\r\n" + ex.Message);
			}
		}

		private Stream SaveClientFile()
		{
			MemoryStream memoryStream = new MemoryStream();
			try
			{
				WriteFiles.WriteHeader(memoryStream, "IUCDFV2");
				WriteFiles.WriteDeprecatedString(memoryStream, 1, CompanyName);
				WriteFiles.WriteDeprecatedString(memoryStream, 2, ProductName);
				if (m_GUID != null)
				{
					WriteFiles.WriteString(memoryStream, 10, m_GUID);
				}
				WriteFiles.WriteDeprecatedString(memoryStream, 3, InstalledVersion);
				foreach (string serverFileSite in ServerFileSites)
				{
					WriteFiles.WriteDeprecatedString(memoryStream, 4, serverFileSite);
				}
				foreach (string clientServerSite in ClientServerSites)
				{
					WriteFiles.WriteDeprecatedString(memoryStream, 9, clientServerSite);
				}
				WriteFiles.WriteDeprecatedString(memoryStream, 17, HeaderImageAlign.ToString());
				WriteFiles.WriteInt(memoryStream, 18, HeaderTextIndent);
				if (!string.IsNullOrEmpty(HeaderTextColorName))
				{
					WriteFiles.WriteDeprecatedString(memoryStream, 19, HeaderTextColorName);
				}
				if (!string.IsNullOrEmpty(TopImageFilename))
				{
					WriteFiles.WriteDeprecatedString(memoryStream, 20, TopImageFilename);
				}
				if (!string.IsNullOrEmpty(SideImageFilename))
				{
					WriteFiles.WriteDeprecatedString(memoryStream, 21, SideImageFilename);
				}
				foreach (DictionaryEntry language in Languages)
				{
					LanguageCulture languageCulture = (LanguageCulture)language.Value;
					WriteFiles.WriteDeprecatedString(memoryStream, 24, languageCulture.Culture);
					if (!string.IsNullOrEmpty(languageCulture.Filename))
					{
						WriteFiles.WriteDeprecatedString(memoryStream, 22, languageCulture.Filename);
					}
				}
				if (HideHeaderDivider)
				{
					WriteFiles.WriteBool(memoryStream, 23, val: true);
				}
				if (CloseOnSuccess)
				{
					WriteFiles.WriteBool(memoryStream, 25, val: true);
				}
				if (!string.IsNullOrEmpty(CustomWyUpdateTitle))
				{
					WriteFiles.WriteString(memoryStream, 26, CustomWyUpdateTitle);
				}
				if (!string.IsNullOrEmpty(PublicSignKey))
				{
					WriteFiles.WriteString(memoryStream, 27, PublicSignKey);
				}
				if (!string.IsNullOrEmpty(UpdatePassword))
				{
					WriteFiles.WriteString(memoryStream, 28, UpdatePassword);
				}
				memoryStream.WriteByte(byte.MaxValue);
				memoryStream.Position = 0L;
				return memoryStream;
			}
			catch (Exception)
			{
				memoryStream.Dispose();
				throw;
			}
		}

		public static void AddUniqueString(string newString, List<string> list)
		{
			foreach (string item in list)
			{
				if (string.Equals(newString, item, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
			}
			list.Add(newString);
		}
	}
}
