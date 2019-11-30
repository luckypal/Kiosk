namespace wyUpdate.Compression.Vcdiff
{
	internal enum InstructionType : byte
	{
		NoOp,
		Add,
		Run,
		Copy
	}
}
