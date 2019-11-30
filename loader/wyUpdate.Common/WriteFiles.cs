using System;
using System.IO;
using System.Text;

namespace wyUpdate.Common
{
	public static class WriteFiles
	{
		public static void WriteInt(Stream fs, byte flag, int num)
		{
			fs.WriteByte(flag);
			fs.Write(BitConverter.GetBytes(4), 0, 4);
			fs.Write(BitConverter.GetBytes(num), 0, 4);
		}

		public static void WriteBool(Stream fs, byte flag, bool val)
		{
			WriteInt(fs, flag, val ? 1 : 0);
		}

		public static void WriteLong(Stream fs, byte flag, long val)
		{
			fs.WriteByte(flag);
			fs.Write(BitConverter.GetBytes(8), 0, 4);
			fs.Write(BitConverter.GetBytes(val), 0, 8);
		}

		public static void WriteShort(Stream fs, byte flag, short val)
		{
			fs.WriteByte(flag);
			fs.Write(BitConverter.GetBytes(2), 0, 4);
			fs.Write(BitConverter.GetBytes(val), 0, 2);
		}

		public static void WriteDateTime(Stream fs, byte flag, DateTime dt)
		{
			fs.WriteByte(flag);
			fs.Write(BitConverter.GetBytes(24), 0, 4);
			fs.Write(BitConverter.GetBytes(dt.Year), 0, 4);
			fs.Write(BitConverter.GetBytes(dt.Month), 0, 4);
			fs.Write(BitConverter.GetBytes(dt.Day), 0, 4);
			fs.Write(BitConverter.GetBytes(dt.Hour), 0, 4);
			fs.Write(BitConverter.GetBytes(dt.Minute), 0, 4);
			fs.Write(BitConverter.GetBytes(dt.Second), 0, 4);
		}

		public static void WriteString(Stream fs, byte flag, string text)
		{
			if (text == null)
			{
				text = string.Empty;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			fs.WriteByte(flag);
			fs.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
			fs.Write(bytes, 0, bytes.Length);
		}

		public static void WriteDeprecatedString(Stream fs, byte flag, string text)
		{
			if (text == null)
			{
				text = string.Empty;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			byte[] bytes2 = BitConverter.GetBytes(bytes.Length);
			fs.WriteByte(flag);
			fs.Write(BitConverter.GetBytes(bytes.Length + 4), 0, 4);
			fs.Write(bytes2, 0, 4);
			fs.Write(bytes, 0, bytes.Length);
		}

		public static void WriteByteArray(Stream fs, byte flag, byte[] arr)
		{
			byte[] bytes = BitConverter.GetBytes(arr.Length);
			fs.WriteByte(flag);
			fs.Write(bytes, 0, 4);
			fs.Write(arr, 0, arr.Length);
		}

		public static void WriteHeader(Stream fs, string Header)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(Header);
			fs.Write(bytes, 0, bytes.Length);
		}
	}
}
