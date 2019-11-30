// Decompiled with JetBrains decompiler
// Type: GLib.LogFile
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace GLib
{
  public class LogFile
  {
    private string Filename;
    private LogFile.LogType Type;
    private LogFile.LogLevel Level;
    private LogFile.LogLevel DefaultLevel;

    public LogFile(string filename)
      : this(filename, false, LogFile.LogType.TXT, LogFile.LogLevel.All, "")
    {
    }

    public LogFile(string filename, bool append)
      : this(filename, append, LogFile.LogType.TXT, LogFile.LogLevel.All, "")
    {
    }

    public LogFile(string filename, bool append, LogFile.LogType type)
      : this(filename, append, type, LogFile.LogLevel.All, "")
    {
    }

    public LogFile(string filename, bool append, LogFile.LogType type, LogFile.LogLevel level)
      : this(filename, append, type, level, "")
    {
    }

    public LogFile(
      string filename,
      bool append,
      LogFile.LogType type,
      LogFile.LogLevel level,
      string title)
    {
    }

    public void WriteFooter()
    {
    }

    public void LogRaw(string text)
    {
    }

    public void Log(string text)
    {
    }

    public void Log(string text, LogFile.LogLevel level)
    {
    }

    public LogFile.LogType CurrentLogType
    {
      get
      {
        return this.Type;
      }
    }

    public LogFile.LogLevel CurrentLogLevel
    {
      get
      {
        return this.Level;
      }
      set
      {
        this.Level = value;
      }
    }

    public LogFile.LogLevel DefaultLogLevel
    {
      get
      {
        return this.DefaultLevel;
      }
      set
      {
        this.DefaultLevel = value;
      }
    }

    public Version Version
    {
      get
      {
        return Assembly.GetExecutingAssembly().GetName().Version;
      }
    }

    public static string LogFile_Timestamp(string _file)
    {
      DateTime now = DateTime.Now;
      return _file + string.Format("_{0}_{1}_{2}_{3}_{4}_{5}", (object) now.Year.ToString().PadLeft(4, '0'), (object) now.Month.ToString().PadLeft(2, '0'), (object) now.Day.ToString().PadLeft(2, '0'), (object) now.Hour.ToString().PadLeft(2, '0'), (object) now.Minute.ToString().PadLeft(2, '0'), (object) now.Second.ToString().PadLeft(2, '0'));
    }

    private void WriteLine(string text, bool append)
    {
      try
      {
        StreamWriter streamWriter = new StreamWriter(this.Filename, append, Encoding.UTF8);
        if (text != "")
          streamWriter.WriteLine(text);
        streamWriter.Flush();
        streamWriter.Close();
      }
      catch
      {
        throw;
      }
    }

    public enum LogType
    {
      TXT,
      XHTML_Plain,
    }

    [Flags]
    public enum LogLevel
    {
      Debug = 1,
      Info = 2,
      Warn = 4,
      Error = 8,
      Credits = 16, // 0x00000010
      Server = 32, // 0x00000020
      Users = 64, // 0x00000040
      All = 255, // 0x000000FF
    }
  }
}
