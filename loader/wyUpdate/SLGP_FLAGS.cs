using System;

namespace wyUpdate
{
	[Flags]
	public enum SLGP_FLAGS
	{
		SLGP_SHORTPATH = 0x1,
		SLGP_UNCPRIORITY = 0x2,
		SLGP_RAWPATH = 0x4
	}
}
