// Decompiled with JetBrains decompiler
// Type: GLib.Config.XmlConfig
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

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
    private static byte[] bytes = Encoding.ASCII.GetBytes("2kAWAFxg");
    public string loc;
    public bool unique;
    public string error;
    public string AppName;
    public string CfgFile;
    public string CfgFileFull;
    public string Path;
    public string DataPath;
    public XmlLocalize Localize;
    public bool Seguridad;
    public LogFile _log;
    public string XML_File;

    public XmlConfig()
    {
      this._XmlConfig(false);
    }

    public XmlConfig(bool _seg)
    {
      this._XmlConfig(_seg);
    }

    public void _XmlConfig(bool _seg)
    {
      this.Seguridad = _seg;
      this.loc = "esES";
      this.unique = true;
      this.error = "";
      this.Path = Environment.CurrentDirectory + "\\";
      this.DataPath = this.Path + "data\\";
      this.AppName = Assembly.GetExecutingAssembly().GetName().Name;
      this.CfgFile = this.AppName + ".options";
      this.Localize = (XmlLocalize) null;
      this.XML_File = "<?xml version=\"1.0\" encoding=\"us-ascii\"?>\r\n<config/>\r\n";
    }

    public bool Save()
    {
      bool flag = this.config_save(this.CfgFile);
      try
      {
        Util.Load_Text(this.CfgFileFull, out this.XML_File);
      }
      catch (Exception ex)
      {
      }
      return flag;
    }

    public bool Modify()
    {
      return this.config_modify(this.CfgFile);
    }

    public bool Load(int _modo)
    {
      if (_modo == 1 && File.Exists(this.CfgFileFull + ".tmp") && this.config_load(this.CfgFile + ".tmp", 0))
      {
        if (File.Exists(this.CfgFileFull))
        {
          try
          {
            File.Delete(this.CfgFileFull);
          }
          catch
          {
          }
        }
        try
        {
          File.Copy(this.CfgFileFull + ".tmp", this.CfgFileFull);
        }
        catch
        {
        }
        this.Save();
      }
      bool flag = this.config_load(this.CfgFile, 0);
      if (!flag)
        this.Save();
      this.Localize = new XmlLocalize(this.DataPath, this.AppName + ".locs");
      if (this.Localize != null)
        this.Localize.Localize = this.loc;
      return flag;
    }

    public void Reload_Localizacion()
    {
      this.Localize = new XmlLocalize(this.DataPath, this.AppName + ".locs");
      if (this.Localize == null)
        return;
      this.Localize.Localize = this.loc;
    }

    public bool Set_AppName(string _s)
    {
      this.AppName = _s;
      this.CfgFile = this.AppName + ".options";
      return true;
    }

    public bool Set_CfgFile(string _s)
    {
      this.CfgFile = _s + ".options";
      return true;
    }

    public bool Set_DataPath(string _s)
    {
      this.DataPath = _s;
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
      string str1 = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + _cfg;
      string str2 = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + _cfg + ".tmp";
      if (this._log != null)
        this._log.Log("Save config (" + _cfg + ")", LogFile.LogLevel.Debug);
      if (File.Exists(str2))
      {
        try
        {
          File.Delete(str2);
        }
        catch (Exception ex)
        {
          this.error = ex.Message;
        }
      }
      XmlTextWriter writer = new XmlTextWriter(str2, Encoding.ASCII);
      writer.Formatting = Formatting.Indented;
      writer.WriteStartDocument();
      try
      {
        writer.WriteStartElement("Config".ToLower());
        writer.WriteStartElement("Localize".ToLower());
        writer.WriteAttributeString("Language".ToLower(), this.loc);
        writer.WriteEndElement();
        this.save(ref writer);
        writer.WriteEndElement();
      }
      catch (Exception ex)
      {
        this.error = ex.Message;
        writer.Flush();
        writer.Close();
        try
        {
          File.Delete(str2);
        }
        catch
        {
        }
        return false;
      }
      writer.Flush();
      writer.Close();
      if (File.Exists(str1))
      {
        try
        {
          File.Delete(str1);
        }
        catch (Exception ex)
        {
          this.error = ex.Message;
        }
      }
      try
      {
        File.Copy(str2, str1);
      }
      catch
      {
      }
      try
      {
        File.Delete(str2);
      }
      catch
      {
      }
      if (this.Seguridad)
      {
        if (this._log != null)
          this._log.Log("Save OK HASH config (" + _cfg + ")", LogFile.LogLevel.Debug);
        return XmlConfig.HashFile_Save(str1);
      }
      if (this._log != null)
        this._log.Log("Save OK config (" + _cfg + ")", LogFile.LogLevel.Debug);
      return true;
    }

    public bool config_load(string _cfg, int _modo)
    {
      string str = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + _cfg;
      XmlTextReader xmlTextReader = (XmlTextReader) null;
      if (this._log != null)
        this._log.Log("Load config (" + _cfg + ")", LogFile.LogLevel.Debug);
      if (this.Seguridad && _modo == 0)
      {
        if (!XmlConfig.HashFile_Load(str))
        {
          if (this._log != null)
            this._log.Log("Hash error config (" + _cfg + ")", LogFile.LogLevel.Debug);
          return false;
        }
      }
      try
      {
        xmlTextReader = _modo != 0 ? new XmlTextReader((TextReader) new StringReader(_cfg)) : new XmlTextReader(str);
        xmlTextReader.Read();
        xmlTextReader.Close();
      }
      catch (Exception ex)
      {
        if (this._log != null)
          this._log.Log("XML error config (" + ex.Message + ")", LogFile.LogLevel.Debug);
        xmlTextReader?.Close();
        return false;
      }
      XmlTextReader reader;
      try
      {
        reader = _modo != 0 ? new XmlTextReader((TextReader) new StringReader(_cfg)) : new XmlTextReader(str);
      }
      catch (Exception ex)
      {
        this.error = ex.Message;
        if (this._log != null)
          this._log.Log("XML error config (" + ex.Message + ")", LogFile.LogLevel.Debug);
        xmlTextReader.Close();
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
              for (int i = 0; i < reader.AttributeCount; ++i)
              {
                reader.MoveToAttribute(i);
                if (reader.Name.ToLower() == "language".ToLower())
                {
                  try
                  {
                    this.loc = reader.Value.ToString();
                  }
                  catch
                  {
                  }
                }
              }
            }
            this.load(ref reader);
          }
        }
        reader.Close();
      }
      catch (Exception ex)
      {
        this.error = ex.Message;
        if (this._log != null)
          this._log.Log("XML error config (" + ex.Message + ")", LogFile.LogLevel.Debug);
        reader?.Close();
        return false;
      }
      if (this._log != null)
        this._log.Log("Load OK config (" + _cfg + ")", LogFile.LogLevel.Debug);
      return true;
    }

    public bool config_modify(string _cfg)
    {
      string str1 = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + _cfg;
      string str2 = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + _cfg + ".tmp";
      try
      {
        File.Delete(str2);
      }
      catch
      {
      }
      try
      {
        File.Copy(str1, str2);
      }
      catch
      {
      }
      XmlDocument docum;
      try
      {
        docum = new XmlDocument();
        docum.Load(str2);
      }
      catch (Exception ex)
      {
        try
        {
          File.Delete(str2);
        }
        catch
        {
        }
        return false;
      }
      try
      {
        this.modify(ref docum);
      }
      catch (Exception ex)
      {
        this.error = ex.Message;
        try
        {
          File.Delete(str2);
        }
        catch
        {
        }
        return false;
      }
      try
      {
        docum.Save(str2);
      }
      catch (Exception ex)
      {
        this.error = ex.Message;
        try
        {
          File.Delete(str2);
        }
        catch
        {
        }
        return false;
      }
      try
      {
        File.Delete(str1);
      }
      catch
      {
      }
      try
      {
        File.Copy(str2, str1);
      }
      catch
      {
      }
      try
      {
        File.Delete(str2);
      }
      catch
      {
      }
      return XmlConfig.HashFile_Save(str1);
    }

    private static bool HashFile_Load(string _file)
    {
      if (!File.Exists(_file))
        return false;
      string path = _file + ".ver";
      if (!File.Exists(path))
        return false;
      HashAlgorithm hashAlgorithm = (HashAlgorithm) new SHA1CryptoServiceProvider();
      byte[] numArray = (byte[]) null;
      using (FileStream fileStream = File.Open(_file, FileMode.Open, FileAccess.Read, FileShare.Read))
        numArray = hashAlgorithm.ComputeHash((Stream) fileStream);
      for (int index = 0; index < numArray.Length; ++index)
        numArray[index] ^= (byte) 123;
      byte[] buffer = new byte[numArray.Length];
      using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
        fileStream.Read(buffer, 0, numArray.Length);
      for (int index = 0; index < numArray.Length; ++index)
      {
        if ((int) buffer[index] != (int) numArray[index])
          return false;
      }
      return true;
    }

    public static bool WriteElement(
      ref XmlDocument _xdoc,
      string _key,
      string _attrib,
      string _val)
    {
      XmlNode xmlNode = _xdoc.SelectSingleNode(_key.ToLower());
      if (xmlNode != null)
        xmlNode.Attributes[_attrib.ToLower()].Value = _val;
      return true;
    }

    private static bool HashFile_Save(string _file)
    {
      if (!File.Exists(_file))
        return false;
      string path = _file + ".ver";
      HashAlgorithm hashAlgorithm = (HashAlgorithm) new SHA1CryptoServiceProvider();
      byte[] buffer = (byte[]) null;
      using (FileStream fileStream = File.Open(_file, FileMode.Open, FileAccess.Read, FileShare.Read))
        buffer = hashAlgorithm.ComputeHash((Stream) fileStream);
      for (int index = 0; index < buffer.Length; ++index)
        buffer[index] ^= (byte) 123;
      File.OpenWrite(path).Write(buffer, 0, buffer.Length);
      return true;
    }

    public static string Z()
    {
      Random random = new Random();
      byte[] numArray = new byte[8];
      random.NextBytes(numArray);
      return Convert.ToBase64String(numArray);
    }

    public static string X2Y(string dades)
    {
      byte[] hash = new SHA1CryptoServiceProvider().ComputeHash(new UTF8Encoding().GetBytes(dades));
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < hash.Length; ++index)
      {
        if (hash[index] < (byte) 16)
          stringBuilder.Append("0");
        stringBuilder.Append(hash[index].ToString("x"));
      }
      return stringBuilder.ToString().ToUpper();
    }

    public static string Encrypt(string originalString)
    {
      if (string.IsNullOrEmpty(originalString))
        throw new ArgumentNullException("Error Encrypt.");
      DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
      MemoryStream memoryStream = new MemoryStream();
      if (cryptoServiceProvider.ValidKeySize(XmlConfig.bytes.Length))
        return "0";
      CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, cryptoServiceProvider.CreateEncryptor(XmlConfig.bytes, XmlConfig.bytes), CryptoStreamMode.Write);
      StreamWriter streamWriter = new StreamWriter((Stream) cryptoStream);
      streamWriter.Write(originalString);
      streamWriter.Flush();
      cryptoStream.FlushFinalBlock();
      streamWriter.Flush();
      return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int) memoryStream.Length, Base64FormattingOptions.None);
    }

    public static string Decrypt(string cryptedString)
    {
      if (string.IsNullOrEmpty(cryptedString))
        throw new ArgumentNullException("Error Decrypt.");
      DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
      return new StreamReader((Stream) new CryptoStream((Stream) new MemoryStream(Convert.FromBase64String(cryptedString)), cryptoServiceProvider.CreateDecryptor(XmlConfig.bytes, XmlConfig.bytes), CryptoStreamMode.Read)).ReadToEnd();
    }

    public static string EncodeDate(DateTime date)
    {
      int month = date.Month;
      int day = date.Day;
      int hour = date.Hour;
      int minute = date.Minute;
      int second = date.Second;
      int millisecond = date.Millisecond;
      return date.Year.ToString() + (month > 9 ? (object) string.Concat((object) month) : (object) ("0" + (object) month)) + (day > 9 ? (object) string.Concat((object) day) : (object) ("0" + (object) day)) + (hour > 9 ? (object) string.Concat((object) hour) : (object) ("0" + (object) hour)) + (minute > 9 ? (object) string.Concat((object) minute) : (object) ("0" + (object) minute)) + (second > 9 ? (object) string.Concat((object) second) : (object) ("0" + (object) second));
    }

    public static long EncodeDateInt(DateTime date)
    {
      return (long) date.Year * 10000000000L + (long) (date.Month * 100000000) + (long) (date.Day * 1000000) + (long) (date.Hour * 10000) + (long) (date.Minute * 100) + (long) date.Second;
    }

    public static int EncodeDateDifInt(long d1, long d2)
    {
      long num1 = d1 % 100L;
      long num2 = d2 % 100L;
      long num3 = d1 / 100L % 100L;
      long num4 = d2 / 100L % 100L;
      long num5 = d1 / 10000L % 100L;
      long num6 = d2 / 10000L % 100L;
      long num7 = d1 / 1000000L % 100L;
      long num8 = d2 / 1000000L % 100L;
      return (int) (num7 * 24L * 60L * 60L + num5 * 60L * 60L + num3 * 60L + num1 - (num8 * 24L * 60L * 60L + num6 * 60L * 60L + num4 * 60L + num2));
    }
  }
}
