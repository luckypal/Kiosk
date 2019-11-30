using System;

namespace wyUpdate.Compression.Vcdiff
{
	internal sealed class CodeTable
	{
		internal static CodeTable Default = BuildDefaultCodeTable();

		private readonly Instruction[,] entries = new Instruction[256, 2];

		internal Instruction this[int i, int j] => entries[i, j];

		internal CodeTable(byte[] bytes)
		{
			for (int i = 0; i < 256; i++)
			{
				entries[i, 0] = new Instruction((InstructionType)bytes[i], bytes[i + 512], bytes[i + 1024]);
				entries[i, 1] = new Instruction((InstructionType)bytes[i + 256], bytes[i + 768], bytes[i + 1280]);
			}
		}

		internal CodeTable(Instruction[,] entries)
		{
			if (entries == null)
			{
				throw new ArgumentNullException("entries");
			}
			if (entries.Rank != 2)
			{
				throw new ArgumentException("Array must be rectangular.", "entries");
			}
			if (entries.GetLength(0) != 256)
			{
				throw new ArgumentException("Array must have outer length 256.", "entries");
			}
			if (entries.GetLength(1) != 2)
			{
				throw new ArgumentException("Array must have inner length 256.", "entries");
			}
			Array.Copy(entries, 0, this.entries, 0, 512);
		}

		private static CodeTable BuildDefaultCodeTable()
		{
			Instruction[,] array = new Instruction[256, 2];
			array[0, 0] = new Instruction(InstructionType.Run, 0, 0);
			for (byte b = 0; b < 18; b = (byte)(b + 1))
			{
				array[b + 1, 0] = new Instruction(InstructionType.Add, b, 0);
			}
			int num = 19;
			for (byte b2 = 0; b2 < 9; b2 = (byte)(b2 + 1))
			{
				array[num++, 0] = new Instruction(InstructionType.Copy, 0, b2);
				for (byte b3 = 4; b3 < 19; b3 = (byte)(b3 + 1))
				{
					array[num++, 0] = new Instruction(InstructionType.Copy, b3, b2);
				}
			}
			for (byte b4 = 0; b4 < 6; b4 = (byte)(b4 + 1))
			{
				for (byte b5 = 1; b5 < 5; b5 = (byte)(b5 + 1))
				{
					for (byte b6 = 4; b6 < 7; b6 = (byte)(b6 + 1))
					{
						array[num, 0] = new Instruction(InstructionType.Add, b5, 0);
						array[num++, 1] = new Instruction(InstructionType.Copy, b6, b4);
					}
				}
			}
			for (byte b7 = 6; b7 < 9; b7 = (byte)(b7 + 1))
			{
				for (byte b8 = 1; b8 < 5; b8 = (byte)(b8 + 1))
				{
					array[num, 0] = new Instruction(InstructionType.Add, b8, 0);
					array[num++, 1] = new Instruction(InstructionType.Copy, 4, b7);
				}
			}
			for (byte b9 = 0; b9 < 9; b9 = (byte)(b9 + 1))
			{
				array[num, 0] = new Instruction(InstructionType.Copy, 4, b9);
				array[num++, 1] = new Instruction(InstructionType.Add, 1, 0);
			}
			return new CodeTable(array);
		}

		internal byte[] GetBytes()
		{
			byte[] array = new byte[1536];
			for (int i = 0; i < 256; i++)
			{
				array[i] = (byte)entries[i, 0].Type;
				array[i + 256] = (byte)entries[i, 1].Type;
				array[i + 512] = entries[i, 0].Size;
				array[i + 768] = entries[i, 1].Size;
				array[i + 1024] = entries[i, 0].Mode;
				array[i + 1280] = entries[i, 1].Mode;
			}
			return array;
		}
	}
}
