using System.IO;

namespace wyUpdate.Compression.Vcdiff
{
	internal static class IOHelper
	{
		internal static byte[] CheckedReadBytes(Stream stream, int size)
		{
			byte[] array = new byte[size];
			int num;
			for (int i = 0; i < size; i += num)
			{
				num = stream.Read(array, i, size - i);
				if (num == 0)
				{
					throw new EndOfStreamException(string.Format("End of stream reached with {0} byte{1} left to read.", size - i, (size - i == 1) ? "s" : ""));
				}
			}
			return array;
		}

		internal static byte CheckedReadByte(Stream stream)
		{
			int num = stream.ReadByte();
			if (num == -1)
			{
				throw new IOException("Expected to be able to read a byte.");
			}
			return (byte)num;
		}

		internal static int ReadBigEndian7BitEncodedInt(Stream stream)
		{
			int num = 0;
			for (int i = 0; i < 5; i++)
			{
				int num2 = stream.ReadByte();
				if (num2 == -1)
				{
					throw new EndOfStreamException();
				}
				num = ((num << 7) | (num2 & 0x7F));
				if ((num2 & 0x80) == 0)
				{
					return num;
				}
			}
			throw new IOException("Invalid 7-bit encoded integer in stream.");
		}
	}
}
