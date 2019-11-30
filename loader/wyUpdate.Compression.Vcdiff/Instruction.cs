namespace wyUpdate.Compression.Vcdiff
{
	internal struct Instruction
	{
		private readonly InstructionType type;

		private readonly byte size;

		private readonly byte mode;

		internal InstructionType Type => type;

		internal byte Size => size;

		internal byte Mode => mode;

		internal Instruction(InstructionType type, byte size, byte mode)
		{
			this.type = type;
			this.size = size;
			this.mode = mode;
		}
	}
}
