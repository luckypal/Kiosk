using GLib.Config.Local;
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace GLib.Config
{
	public class XmlConfig
	{
		public string loc;

		public bool unique;

		public string error;

		public string AppName;

		public string CfgFile;

		public string CfgFileFull;

		public string Path;

		public string DataPath;

		private static byte[] bytes = Encoding.ASCII.GetBytes("2kAWAFxg");

		public XmlLocalize Localize;

		public bool Seguridad;

		public LogFile _log;

		public string XML_File;

		public XmlConfig()
		{
			_XmlConfig(_seg: false);
		}

		public XmlConfig(bool _seg)
		{
			_XmlConfig(_seg);
		}

		public void _XmlConfig(bool _seg)
		{
			Seguridad = _seg;
			loc = "esES";
			unique = true;
			error = "";
			Path = Environment.CurrentDirectory + "\\";
			DataPath = Path + "data\\";
			AppName = Assembly.GetExecutingAssembly().GetName().Name;
			CfgFile = AppName + ".options";
			Localize = null;
			XML_File = "<?xml version=\"1.0\" encoding=\"us-ascii\"?>\r\n<config/>\r\n";
		}

		public bool Save()
		{
			bool result = config_save(CfgFile);
			try
			{
				Util.Load_Text(CfgFileFull, out XML_File);
			}
			catch (Exception)
			{
			}
			return result;
		}

		public bool Modify()
		{
			return config_modify(CfgFile);
		}

		public bool Load(int _modo)
		{
			if (_modo == 1 && File.Exists(CfgFileFull + ".tmp") && config_load(CfgFile + ".tmp", 0))
			{
				if (File.Exists(CfgFileFull))
				{
					try
					{
						File.Delete(CfgFileFull);
					}
					catch
					{
					}
				}
				try
				{
					File.Copy(CfgFileFull + ".tmp", CfgFileFull);
				}
				catch
				{
				}
				Save();
			}
			bool flag = config_load(CfgFile, 0);
			if (!flag)
			{
				Save();
			}
			Localize = new XmlLocalize(DataPath, AppName + ".locs");
			if (Localize != null)
			{
				Localize.Localize = loc;
			}
			return flag;
		}

		public void Reload_Localizacion()
		{
			Localize = new XmlLocalize(DataPath, AppName + ".locs");
			if (Localize != null)
			{
				Localize.Localize = loc;
			}
		}

		public bool Set_AppName(string _s)
		{
			AppName = _s;
			CfgFile = AppName + ".options";
			return true;
		}

		public bool Set_CfgFile(string _s)
		{
			CfgFile = _s + ".options";
			return true;
		}

		public bool Set_DataPath(string _s)
		{
			DataPath = _s;
			return true;
		}

		public virtual void save(ref XmlTextWriter writer)
		{
		}

		public virtual void load(ref XmlTextReader reader)
		{
		}

		public virtual void modify(ref XmlDocument docum)
		{
		}

		public bool config_save(string _cfg)
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + _cfg;
			string text2 = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + _cfg + ".tmp";
			if (_log != null)
			{
				_log.Log("Save config (" + _cfg + ")", LogFile.LogLevel.Debug);
			}
			if (File.Exists(text2))
			{
				try
				{
					File.Delete(text2);
				}
				catch (Exception ex)
				{
					error = ex.Message;
				}
			}
			XmlTextWriter writer = new XmlTextWriter(text2, Encoding.ASCII);
			writer.Formatting = Formatting.Indented;
			writer.WriteStartDocument();
			try
			{
				writer.WriteStartElement("Config".ToLower());
				writer.WriteStartElement("Localize".ToLower());
				writer.WriteAttributeString("Language".ToLower(), loc);
				writer.WriteEndElement();
				save(ref writer);
				writer.WriteEndElement();
			}
			catch (Exception ex)
			{
				error = ex.Message;
				writer.Flush();
				writer.Close();
				try
				{
					File.Delete(text2);
				}
				catch
				{
				}
				return false;
			}
			writer.Flush();
			writer.Close();
			if (File.Exists(text))
			{
				try
				{
					File.Delete(text);
				}
				catch (Exception ex)
				{
					error = ex.Message;
				}
			}
			try
			{
				File.Copy(text2, text);
			}
			catch
			{
			}
			try
			{
				File.Delete(text2);
			}
			catch
			{
			}
			if (Seguridad)
			{
				if (_log != null)
				{
					_log.Log("Save OK HASH config (" + _cfg + ")", LogFile.LogLevel.Debug);
				}
				return HashFile_Save(text);
			}
			if (_log != null)
			{
				_log.Log("Save OK config (" + _cfg + ")", LogFile.LogLevel.Debug);
			}
			return true;
		}

		public bool config_load(string _cfg, int _modo)
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + _cfg;
			XmlTextReader reader = null;
			if (_log != null)
			{
				_log.Log("Load config (" + _cfg + ")", LogFile.LogLevel.Debug);
			}
			if (Seguridad && _modo == 0 && !HashFile_Load(text))
			{
				if (_log != null)
				{
					_log.Log("Hash error config (" + _cfg + ")", LogFile.LogLevel.Debug);
				}
				return false;
			}
			try
			{
				reader = ((_modo != 0) ? new XmlTextReader(new StringReader(_cfg)) : new XmlTextReader(text));
				reader.Read();
				reader.Close();
			}
			catch (Exception ex)
			{
				if (_log != null)
				{
					_log.Log("XML error config (" + ex.Message + ")", LogFile.LogLevel.Debug);
				}
				reader?.Close();
				return false;
			}
			try
			{
				reader = ((_modo != 0) ? new XmlTextReader(new StringReader(_cfg)) : new XmlTextReader(text));
			}
			catch (Exception ex)
			{
				error = ex.Message;
				if (_log != null)
				{
					_log.Log("XML error config (" + ex.Message + ")", LogFile.LogLevel.Debug);
				}
				reader.Close();
				return false;
			}
			try
			{
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						if (reader.Name.ToLower() == "localize".ToLower() && reader.HasAttributes)
						{
							for (int i = 0; i < reader.AttributeCount; i++)
							{
								reader.MoveToAttribute(i);
								if (reader.Name.ToLower() == "language".ToLower())
								{
									try
									{
										loc = reader.Value.ToString();
									}
									catch
									{
									}
								}
							}
						}
						load(ref reader);
					}
				}
				reader.Close();
			}
			catch (Exception ex)
			{
				error = ex.Message;
				if (_log != null)
				{
					_log.Log("XML error config (" + ex.Message + ")", LogFile.LogLevel.Debug);
				}
				reader?.Close();
				return false;
			}
			if (_log != null)
			{
				_log.Log("Load OK config (" + _cfg + ")", LogFile.LogLevel.Debug);
			}
			return true;
		}

		public bool config_modify(string _cfg)
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + _cfg;
			string text2 = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + _cfg + ".tmp";
			try
			{
				File.Delete(text2);
			}
			catch
			{
			}
			try
			{
				File.Copy(text, text2);
			}
			catch
			{
			}
			XmlDocument xmlDocument = null;
			try
			{
				xmlDocument = new XmlDocument();
				xmlDocument.Load(text2);
			}
			catch (Exception)
			{
				try
				{
					File.Delete(text2);
				}
				catch
				{
				}
				return false;
			}
			try
			{
				modify(ref xmlDocument);
			}
			catch (Exception ex2)
			{
				error = ex2.Message;
				try
				{
					File.Delete(text2);
				}
				catch
				{
				}
				return false;
			}
			try
			{
				xmlDocument.Save(text2);
			}
			catch (Exception ex2)
			{
				error = ex2.Message;
				try
				{
					File.Delete(text2);
				}
				catch
				{
				}
				return false;
			}
			try
			{
				File.Delete(text);
			}
			catch
			{
			}
			try
			{
				File.Copy(text2, text);
			}
			catch
			{
			}
			try
			{
				File.Delete(text2);
			}
			catch
			{
			}
			return HashFile_Save(text);
		}

		private static bool HashFile_Load(string _file)
		{
			if (!File.Exists(_file))
			{
				return false;
			}
			string path = _file + ".ver";
			if (!File.Exists(path))
			{
				return false;
			}
			HashAlgorithm hashAlgorithm = new SHA1CryptoServiceProvider();
			byte[] array = null;
			using (FileStream inputStream = File.Open(_file, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				array = hashAlgorithm.ComputeHash(inputStream);
			}
			for (int i = 0; i < array.Length; i++)
			{
				array[i] ^= 123;
			}
			byte[] array2 = new byte[array.Length];
			using (FileStream inputStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				inputStream.Read(array2, 0, array.Length);
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array2[i] != array[i])
				{
					return false;
				}
			}
			return true;
		}

		public static bool WriteElement(ref XmlDocument _xdoc, string _key, string _attrib, string _val)
		{
			XmlNode xmlNode = _xdoc.SelectSingleNode(_key.ToLower());
			if (xmlNode != null)
			{
				xmlNode.Attributes[_attrib.ToLower()].Value = _val;
			}
			return true;
		}

		private static bool HashFile_Save(string _file)
		{
			if (!File.Exists(_file))
			{
				return false;
			}
			string path = _file + ".ver";
			HashAlgorithm hashAlgorithm = new SHA1CryptoServiceProvider();
			byte[] array = null;
			using (FileStream inputStream = File.Open(_file, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				array = hashAlgorithm.ComputeHash(inputStream);
			}
			for (int i = 0; i < array.Length; i++)
			{
				array[i] ^= 123;
			}
			FileStream fileStream = File.OpenWrite(path);
			fileStream.Write(array, 0, array.Length);
			return true;
		}

		public static string Z()
		{
			Random random = new Random();
			byte[] array = new byte[8];
			random.NextBytes(array);
			return Convert.ToBase64String(array);
		}

		public static string X2Y(string dades)
		{
			UTF8Encoding uTF8Encoding = new UTF8Encoding();
			byte[] buffer = uTF8Encoding.GetBytes(dades);
			SHA1CryptoServiceProvider sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();
			byte[] array = sHA1CryptoServiceProvider.ComputeHash(buffer);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] < 16)
				{
					stringBuilder.Append("0");
				}
				stringBuilder.Append(array[i].ToString("x"));
			}
			return stringBuilder.ToString().ToUpper();
		}

		public static string Encrypt(string originalString)
		{
			if (string.IsNullOrEmpty(originalString))
			{
				throw new ArgumentNullException("Error Encrypt.");
			}
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			MemoryStream memoryStream = new MemoryStream();
			if (dESCryptoServiceProvider.ValidKeySize(bytes.Length))
			{
				return "0";
			}
			CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);
			StreamWriter streamWriter = new StreamWriter(cryptoStream);
			streamWriter.Write(originalString);
			streamWriter.Flush();
			cryptoStream.FlushFinalBlock();
			streamWriter.Flush();
			return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length, Base64FormattingOptions.None);
		}

		public static string Decrypt(string cryptedString)
		{
			if (string.IsNullOrEmpty(cryptedString))
			{
				throw new ArgumentNullException("Error Decrypt.");
			}
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			MemoryStream stream = new MemoryStream(Convert.FromBase64String(cryptedString));
			CryptoStream stream2 = new CryptoStream(stream, dESCryptoServiceProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
			StreamReader streamReader = new StreamReader(stream2);
			return streamReader.ReadToEnd();
		}

		public static string EncodeDate(DateTime date)
		{
			int month = date.Month;
			int day = date.Day;
			int hour = date.Hour;
			int minute = date.Minute;
			int second = date.Second;
			int millisecond = date.Millisecond;
			return date.Year + ((month > 9) ? string.Concat(month) : ("0" + month)) + ((day > 9) ? string.Concat(day) : ("0" + day)) + ((hour > 9) ? string.Concat(hour) : ("0" + hour)) + ((minute > 9) ? string.Concat(minute) : ("0" + minute)) + ((second > 9) ? string.Concat(second) : ("0" + second));
		}

		public static long EncodeDateInt(DateTime date)
		{
			return date.Year * 10000000000L + date.Month * 100000000 + date.Day * 1000000 + date.Hour * 10000 + date.Minute * 100 + date.Second;
		}

		public static int EncodeDateDifInt(long d1, long d2)
		{
			long num = d1 % 100;
			long num2 = d2 % 100;
			long num3 = d1 / 100 % 100;
			long num4 = d2 / 100 % 100;
			long num5 = d1 / 10000 % 100;
			long num6 = d2 / 10000 % 100;
			long num7 = d1 / 1000000 % 100;
			long num8 = d2 / 1000000 % 100;
			long num9 = num7 * 24 * 60 * 60 + num5 * 60 * 60 + num3 * 60 + num;
			long num10 = num8 * 24 * 60 * 60 + num6 * 60 * 60 + num4 * 60 + num2;
			return (int)(num9 - num10);
		}
	}
}
