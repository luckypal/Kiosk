// Decompiled with JetBrains decompiler
// Type: GLib.Util
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System.IO;
using System.Security.Cryptography;

namespace GLib
{
  public class Util
  {
    public static bool HashFile(string _file, bool _create)
    {
      if (!File.Exists(_file))
        return false;
      string path = _file + ".ver";
      HashAlgorithm hashAlgorithm = (HashAlgorithm) new SHA1CryptoServiceProvider();
      byte[] buffer1 = (byte[]) null;
      using (FileStream fileStream = File.Open(_file, FileMode.Open, FileAccess.Read, FileShare.Read))
        buffer1 = hashAlgorithm.ComputeHash((Stream) fileStream);
      for (int index = 0; index < buffer1.Length; ++index)
        buffer1[index] ^= (byte) 123;
      if (_create)
      {
        FileStream fileStream = File.OpenWrite(path);
        fileStream.Write(buffer1, 0, buffer1.Length);
        fileStream.Close();
        return true;
      }
      byte[] buffer2 = new byte[buffer1.Length];
      using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
        fileStream.Read(buffer2, 0, buffer1.Length);
      for (int index = 0; index < buffer1.Length; ++index)
      {
        if ((int) buffer2[index] != (int) buffer1[index])
          return false;
      }
      return true;
    }

    public static bool Load_Text(string _file, out string _text)
    {
      _text = "";
      if (!File.Exists(_file))
        return false;
      try
      {
        _text = File.ReadAllText(_file);
      }
      catch
      {
        return false;
      }
      return true;
    }

    public static bool Save_Text(string _file, string _text)
    {
      if (File.Exists(_file))
      {
        try
        {
          File.Delete(_file);
        }
        catch
        {
          return false;
        }
      }
      try
      {
        File.WriteAllText(_file, _text);
      }
      catch
      {
        return false;
      }
      return true;
    }
  }
}
