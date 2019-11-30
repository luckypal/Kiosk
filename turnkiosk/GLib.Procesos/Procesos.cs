using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace GLib.Procesos
{
	public class Procesos
	{
		public static long EncodeTick(DateTime date)
		{
			return (long)new TimeSpan(date.Ticks).TotalMilliseconds;
		}

		public static string Encrypt(string originalString, ref byte[] bytes)
		{
			if (string.IsNullOrEmpty(originalString))
			{
				throw new ArgumentNullException("Error Encrypt.");
			}
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			if (dESCryptoServiceProvider.ValidKeySize(bytes.Length))
			{
				throw new ArgumentNullException("Encrypt: Key not valid.");
			}
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);
			StreamWriter streamWriter = new StreamWriter(cryptoStream);
			streamWriter.Write(originalString);
			streamWriter.Flush();
			cryptoStream.FlushFinalBlock();
			streamWriter.Flush();
			return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length, Base64FormattingOptions.None);
		}

		public static string Decrypt(string cryptedString, ref byte[] bytes)
		{
			if (string.IsNullOrEmpty(cryptedString))
			{
				throw new ArgumentNullException("Error Decrypt.");
			}
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			if (dESCryptoServiceProvider.ValidKeySize(bytes.Length))
			{
				throw new ArgumentNullException("Encrypt: Key not valid.");
			}
			MemoryStream stream = new MemoryStream(Convert.FromBase64String(cryptedString));
			CryptoStream stream2 = new CryptoStream(stream, dESCryptoServiceProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
			StreamReader streamReader = new StreamReader(stream2);
			return streamReader.ReadToEnd();
		}

		public static long EncodeTickNow()
		{
			TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks);
			return (long)timeSpan.TotalMilliseconds;
		}

		public static string Token_Read_Value(string ch, int pos)
		{
			bool flag = false;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			string result = "";
			do
			{
				num3 = ch.IndexOf("#", num2);
				if (num3 != -1)
				{
					result = ch.Substring(num2, num3 - num2);
					num2 = num3 + 1;
					if (num == pos)
					{
						flag = true;
					}
					num++;
				}
				else
				{
					flag = true;
				}
			}
			while (!flag);
			return result;
		}

		public static string Column_Token_Read_Value(string ch, int col, int pos)
		{
			bool flag = false;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			string text = "";
			do
			{
				num2 = ch.IndexOf("|", num2);
				if (num2 != -1)
				{
					num3 = ch.IndexOf("|", num2 + 1);
					if (num3 == -1)
					{
						text = ch.Substring(num2 + 1, ch.Length - 1 - num2);
						flag = true;
					}
					else
					{
						text = ch.Substring(num2 + 1, num3 - num2);
					}
					if (num == col)
					{
						return Token_Read_Value(text, pos);
					}
					num++;
					num2 = num3;
				}
				else
				{
					flag = true;
				}
			}
			while (!flag);
			return "";
		}

		public static bool FindAndKillProcess(string name)
		{
			name = name.ToLower();
			Process[] processes = Process.GetProcesses();
			foreach (Process process in processes)
			{
				if (process.ProcessName.ToLower().StartsWith(name))
				{
					process.Kill();
					return true;
				}
			}
			return false;
		}

		public static int FindProcess(string name)
		{
			int num = 0;
			name = name.ToLower();
			Process[] processes = Process.GetProcesses();
			foreach (Process process in processes)
			{
				if (process.ProcessName.ToLower().StartsWith(name))
				{
					num++;
				}
			}
			return num;
		}

		public static bool Read_Int64(string _v, out long r)
		{
			r = 0L;
			if (!string.IsNullOrEmpty(_v))
			{
				try
				{
					r = Convert.ToInt64(_v);
				}
				catch
				{
					return false;
				}
			}
			return true;
		}
	}
}
