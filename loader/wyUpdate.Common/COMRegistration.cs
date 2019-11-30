using System;

namespace wyUpdate.Common
{
	[Flags]
	public enum COMRegistration
	{
		None = 0x0,
		IsNETAssembly = 0x1,
		Register = 0x2,
		UnRegister = 0x4,
		PreviouslyRegistered = 0x8
	}
}
