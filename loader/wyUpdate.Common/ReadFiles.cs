using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace wyUpdate.Common
{
	public static class ReadFiles
	{
		public static Image ReadImage(Stream fs)
		{
			byte[] array = new byte[4];
			Image image = null;
			fs.Position += 4L;
			fs.Read(array, 0, 4);
			byte[] array2 = new byte[BitConverter.ToInt32(array, 0)];
			ReadWholeArray(fs, array2);
			try
			{
				using (MemoryStream stream = new MemoryStream(array2, 0, array2.Length))
				{
					image = Image.FromStream(stream, useEmbeddedColorManagement: true);
				}
			}
			catch (Exception)
			{
				return null;
			}
			Bitmap bitmap = new Bitmap(image, image.Size);
			Bitmap bitmap2 = new Bitmap(bitmap.Width, bitmap.Height, bitmap.PixelFormat);
			Rectangle rect = new Rectangle(new Point(0, 0), bitmap.Size);
			BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
			BitmapData bitmapData2 = bitmap2.LockBits(rect, ImageLockMode.ReadWrite, bitmap2.PixelFormat);
			int num = BitmapArraySize(bitmap.Width, bitmap.Height, bitmap.PixelFormat);
			array2 = new byte[num];
			Marshal.Copy(bitmapData.Scan0, array2, 0, num);
			Marshal.Copy(array2, 0, bitmapData2.Scan0, num);
			bitmap.UnlockBits(bitmapData);
			bitmap2.UnlockBits(bitmapData);
			bitmap.Dispose();
			return bitmap2;
		}

		private static int BitmapArraySize(int width, int height, PixelFormat pixFormat)
		{
			switch (pixFormat)
			{
			case PixelFormat.Max:
			case PixelFormat.Extended:
			case PixelFormat.Format64bppPArgb:
			case PixelFormat.Format64bppArgb:
				return width * height * 8;
			case PixelFormat.Format48bppRgb:
				return width * height * 6;
			case PixelFormat.Indexed:
			case PixelFormat.Format32bppRgb:
			case PixelFormat.Alpha:
			case PixelFormat.PAlpha:
			case PixelFormat.Format32bppPArgb:
			case PixelFormat.Canonical:
			case PixelFormat.Format32bppArgb:
				return width * height * 4;
			case PixelFormat.Gdi:
			case PixelFormat.Format24bppRgb:
				return width * height * 3;
			case PixelFormat.Format16bppRgb555:
			case PixelFormat.Format16bppRgb565:
			case PixelFormat.Format16bppArgb1555:
			case PixelFormat.Format16bppGrayScale:
				return width * height * 2;
			case PixelFormat.Format8bppIndexed:
				return width * height;
			case PixelFormat.Format4bppIndexed:
				return width * height / 2;
			case PixelFormat.Format1bppIndexed:
				return width * height / 8;
			default:
				return -1;
			}
		}

		public static DateTime ReadDateTime(Stream fs)
		{
			fs.Position += 4L;
			byte[] array = new byte[4];
			ReadWholeArray(fs, array);
			int year = BitConverter.ToInt32(array, 0);
			ReadWholeArray(fs, array);
			int month = BitConverter.ToInt32(array, 0);
			ReadWholeArray(fs, array);
			int day = BitConverter.ToInt32(array, 0);
			ReadWholeArray(fs, array);
			int hour = BitConverter.ToInt32(array, 0);
			ReadWholeArray(fs, array);
			int minute = BitConverter.ToInt32(array, 0);
			ReadWholeArray(fs, array);
			int second = BitConverter.ToInt32(array, 0);
			return new DateTime(year, month, day, hour, minute, second);
		}

		public static string ReadString(Stream fs)
		{
			byte[] array = new byte[4];
			ReadWholeArray(fs, array);
			byte[] array2 = new byte[BitConverter.ToInt32(array, 0)];
			ReadWholeArray(fs, array2);
			return Encoding.UTF8.GetString(array2);
		}

		public static string ReadDeprecatedString(Stream fs)
		{
			int num = ReadInt(fs);
			byte[] array = new byte[num];
			ReadWholeArray(fs, array);
			return Encoding.UTF8.GetString(array);
		}

		public static byte[] ReadByteArray(Stream fs)
		{
			byte[] array = new byte[4];
			ReadWholeArray(fs, array);
			byte[] array2 = new byte[BitConverter.ToInt32(array, 0)];
			ReadWholeArray(fs, array2);
			return array2;
		}

		public static int ReadInt(Stream fs)
		{
			byte[] array = new byte[4];
			fs.Position += 4L;
			ReadWholeArray(fs, array);
			return BitConverter.ToInt32(array, 0);
		}

		public static bool ReadBool(Stream fs)
		{
			return ReadInt(fs) == 1;
		}

		public static long ReadLong(Stream fs)
		{
			byte[] array = new byte[8];
			fs.Position += 4L;
			ReadWholeArray(fs, array);
			return BitConverter.ToInt64(array, 0);
		}

		public static short ReadShort(Stream fs)
		{
			byte[] array = new byte[2];
			fs.Position += 4L;
			ReadWholeArray(fs, array);
			return BitConverter.ToInt16(array, 0);
		}

		public static bool IsHeaderValid(Stream fs, string HeaderShouldBe)
		{
			byte[] array = new byte[HeaderShouldBe.Length];
			fs.Read(array, 0, array.Length);
			string @string = Encoding.UTF8.GetString(array);
			return @string == HeaderShouldBe;
		}

		public static bool ReachedEndByte(Stream fs, byte endByte, byte readValue)
		{
			if (endByte == readValue)
			{
				return true;
			}
			if (fs.Length == fs.Position)
			{
				throw new Exception("Premature end of file.");
			}
			return false;
		}

		public static void SkipField(Stream fs, byte flag)
		{
			if (flag < 128 || flag > 159)
			{
				byte[] array = new byte[4];
				fs.Read(array, 0, 4);
				fs.Position += BitConverter.ToInt32(array, 0);
			}
		}

		public static void ReadWholeArray(Stream stream, byte[] data)
		{
			int num = 0;
			int num2 = data.Length;
			while (true)
			{
				if (num2 > 0)
				{
					int num3 = stream.Read(data, num, num2);
					if (num3 <= 0)
					{
						break;
					}
					num2 -= num3;
					num += num3;
					continue;
				}
				return;
			}
			throw new EndOfStreamException(string.Format(CultureInfo.CurrentCulture, "End of stream reached with {0} bytes left to read", new object[1]
			{
				num2
			}));
		}
	}
}
