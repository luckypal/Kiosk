using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace GLib
{
	public class LogFile
	{
		public enum LogType
		{
			TXT,
			XHTML_Plain
		}

		[Flags]
		public enum LogLevel
		{
			Debug = 0x1,
			Info = 0x2,
			Warn = 0x4,
			Error = 0x8,
			Credits = 0x10,
			Server = 0x20,
			Users = 0x40,
			All = 0xFF
		}

		private string Filename;

		private LogType Type;

		private LogLevel Level;

		private LogLevel DefaultLevel;

		public LogType CurrentLogType => Type;

		public LogLevel CurrentLogLevel
		{
			get
			{
				return Level;
			}
			set
			{
				Level = value;
			}
		}

		public LogLevel DefaultLogLevel
		{
			get
			{
				return DefaultLevel;
			}
			set
			{
				DefaultLevel = value;
			}
		}

		public Version Version => Assembly.GetExecutingAssembly().GetName().Version;

		public LogFile(string filename)
			: this(filename, append: false, LogType.TXT, LogLevel.All, "")
		{
		}

		public LogFile(string filename, bool append)
			: this(filename, append, LogType.TXT, LogLevel.All, "")
		{
		}

		public LogFile(string filename, bool append, LogType type)
			: this(filename, append, type, LogLevel.All, "")
		{
		}

		public LogFile(string filename, bool append, LogType type, LogLevel level)
			: this(filename, append, type, level, "")
		{
		}

		public LogFile(string filename, bool append, LogType type, LogLevel level, string title)
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

		public void Log(string text, LogLevel level)
		{
		}

		public static string LogFile_Timestamp(string _file)
		{
			DateTime now = DateTime.Now;
			return _file + $"_{now.Year.ToString().PadLeft(4, '0')}_{now.Month.ToString().PadLeft(2, '0')}_{now.Day.ToString().PadLeft(2, '0')}_{now.Hour.ToString().PadLeft(2, '0')}_{now.Minute.ToString().PadLeft(2, '0')}_{now.Second.ToString().PadLeft(2, '0')}";
		}

		private void WriteLine(string text, bool append)
		{
			try
			{
				StreamWriter streamWriter = new StreamWriter(Filename, append, Encoding.UTF8);
				if (text != "")
				{
					streamWriter.WriteLine(text);
				}
				streamWriter.Flush();
				streamWriter.Close();
			}
			catch
			{
				throw;
			}
		}
	}
}
