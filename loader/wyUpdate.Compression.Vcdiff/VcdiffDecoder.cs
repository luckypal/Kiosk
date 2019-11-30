using System;
using System.IO;
using wyUpdate.Common;

namespace wyUpdate.Compression.Vcdiff
{
	public sealed class VcdiffDecoder
	{
		private Stream original;

		private Stream delta;

		private Stream output;

		private Adler32 adler = new Adler32();

		private CodeTable codeTable = CodeTable.Default;

		private AddressCache cache = new AddressCache(4, 3);

		private VcdiffDecoder(Stream original, Stream delta, Stream output)
		{
			this.original = original;
			this.delta = delta;
			this.output = output;
		}

		public static void Decode(Stream original, Stream delta, Stream output, long adler)
		{
			if (original != null && (!original.CanRead || !original.CanSeek))
			{
				throw new ArgumentException("Must be able to read and seek in original stream", "original");
			}
			if (delta == null)
			{
				throw new ArgumentNullException("delta");
			}
			if (!delta.CanRead)
			{
				throw new ArgumentException("Unable to read from delta stream");
			}
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (!output.CanWrite || !output.CanRead || !output.CanSeek)
			{
				throw new ArgumentException("Must be able to read, write and seek in output stream", "output");
			}
			VcdiffDecoder vcdiffDecoder = new VcdiffDecoder(original, delta, output);
			vcdiffDecoder.Decode();
			if (adler != 0 && adler != vcdiffDecoder.adler.Value)
			{
				throw new Exception();
			}
		}

		private void Decode()
		{
			ReadHeader();
			while (DecodeWindow())
			{
			}
		}

		private void ReadHeader()
		{
			byte[] array = IOHelper.CheckedReadBytes(delta, 4);
			if (array[0] != 214 || array[1] != 195 || array[2] != 196)
			{
				throw new VcdiffFormatException("Invalid VCDIFF header in delta stream");
			}
			if (array[3] != 0)
			{
				throw new VcdiffFormatException("VcdiffDecoder can only read delta streams of version 0");
			}
			byte b = IOHelper.CheckedReadByte(delta);
			if ((b & 1) != 0)
			{
				throw new VcdiffFormatException("VcdiffDecoder does not handle delta stream using secondary compressors");
			}
			bool flag = (b & 2) != 0;
			bool flag2 = (b & 4) != 0;
			if ((b & 0xF8) != 0)
			{
				throw new VcdiffFormatException("Invalid header indicator - bits 3-7 not all zero.");
			}
			if (flag)
			{
				ReadCodeTable();
			}
			if (flag2)
			{
				int size = IOHelper.ReadBigEndian7BitEncodedInt(delta);
				IOHelper.CheckedReadBytes(delta, size);
			}
		}

		private void ReadCodeTable()
		{
			int size = IOHelper.ReadBigEndian7BitEncodedInt(delta) - 2;
			int nearSize = IOHelper.CheckedReadByte(delta);
			int sameSize = IOHelper.CheckedReadByte(delta);
			byte[] buffer = IOHelper.CheckedReadBytes(delta, size);
			byte[] bytes = CodeTable.Default.GetBytes();
			MemoryStream memoryStream = new MemoryStream(bytes, writable: false);
			MemoryStream memoryStream2 = new MemoryStream(buffer, writable: false);
			byte[] array = new byte[1536];
			MemoryStream memoryStream3 = new MemoryStream(array, writable: true);
			Decode(memoryStream, memoryStream2, memoryStream3, 0L);
			if (memoryStream3.Position != 1536)
			{
				throw new VcdiffFormatException("Compressed code table was incorrect size");
			}
			codeTable = new CodeTable(array);
			cache = new AddressCache(nearSize, sameSize);
		}

		private bool DecodeWindow()
		{
			int num = delta.ReadByte();
			if (num == -1)
			{
				return false;
			}
			int num2 = -1;
			bool flag = (num & 4) == 4;
			num &= 0xFB;
			Stream stream;
			switch (num & 3)
			{
			case 0:
				stream = null;
				break;
			case 1:
				if (original == null)
				{
					throw new VcdiffFormatException("Source stream requested by delta but not provided by caller.");
				}
				stream = original;
				break;
			case 2:
				stream = output;
				num2 = (int)output.Position;
				break;
			case 3:
				throw new VcdiffFormatException("Invalid window indicator - bits 0 and 1 both set.");
			default:
				throw new VcdiffFormatException("Invalid window indicator - bits 3-7 not all zero.");
			}
			byte[] array = null;
			int num3 = 0;
			if (stream != null)
			{
				num3 = IOHelper.ReadBigEndian7BitEncodedInt(delta);
				int num4 = IOHelper.ReadBigEndian7BitEncodedInt(delta);
				stream.Position = num4;
				array = IOHelper.CheckedReadBytes(stream, num3);
				if (num2 != -1)
				{
					stream.Position = num2;
				}
			}
			IOHelper.ReadBigEndian7BitEncodedInt(delta);
			int num5 = IOHelper.ReadBigEndian7BitEncodedInt(delta);
			byte[] array2 = new byte[num5];
			MemoryStream memoryStream = new MemoryStream(array2, writable: true);
			if (IOHelper.CheckedReadByte(delta) != 0)
			{
				throw new VcdiffFormatException("VcdiffDecoder is unable to handle compressed delta sections.");
			}
			int size = IOHelper.ReadBigEndian7BitEncodedInt(delta);
			int size2 = IOHelper.ReadBigEndian7BitEncodedInt(delta);
			int size3 = IOHelper.ReadBigEndian7BitEncodedInt(delta);
			if (flag)
			{
				IOHelper.CheckedReadBytes(delta, 4);
			}
			byte[] array3 = IOHelper.CheckedReadBytes(delta, size);
			byte[] buffer = IOHelper.CheckedReadBytes(delta, size2);
			byte[] addresses = IOHelper.CheckedReadBytes(delta, size3);
			int num6 = 0;
			MemoryStream memoryStream2 = new MemoryStream(buffer, writable: false);
			cache.Reset(addresses);
			while (true)
			{
				int num7 = memoryStream2.ReadByte();
				if (num7 == -1)
				{
					break;
				}
				for (int i = 0; i < 2; i++)
				{
					Instruction instruction = codeTable[num7, i];
					int num8 = instruction.Size;
					if (num8 == 0 && instruction.Type != 0)
					{
						num8 = IOHelper.ReadBigEndian7BitEncodedInt(memoryStream2);
					}
					switch (instruction.Type)
					{
					case InstructionType.Add:
						memoryStream.Write(array3, num6, num8);
						num6 += num8;
						break;
					case InstructionType.Copy:
					{
						int num9 = cache.DecodeAddress((int)memoryStream.Position + num3, instruction.Mode);
						if (array != null && num9 < array.Length)
						{
							memoryStream.Write(array, num9, num8);
							break;
						}
						num9 -= num3;
						if (num9 + num8 < memoryStream.Position)
						{
							memoryStream.Write(array2, num9, num8);
							break;
						}
						for (int k = 0; k < num8; k++)
						{
							memoryStream.WriteByte(array2[num9++]);
						}
						break;
					}
					case InstructionType.Run:
					{
						byte value = array3[num6++];
						for (int j = 0; j < num8; j++)
						{
							memoryStream.WriteByte(value);
						}
						break;
					}
					default:
						throw new VcdiffFormatException("Invalid instruction type found.");
					case InstructionType.NoOp:
						break;
					}
				}
			}
			output.Write(array2, 0, num5);
			adler.Update(array2, 0, num5);
			return true;
		}
	}
}
