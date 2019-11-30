using System;

namespace wyUpdate
{
	[Flags]
	public enum SLR_FLAGS
	{
		SLR_NO_UI = 0x1,
		SLR_ANY_MATCH = 0x2,
		SLR_UPDATE = 0x4,
		SLR_NOUPDATE = 0x8,
		SLR_NOSEARCH = 0x10,
		SLR_NOTRACK = 0x20,
		SLR_NOLINKINFO = 0x40,
		SLR_INVOKE_MSI = 0x80
	}
}
