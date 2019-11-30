using System;

namespace wyUpdate.Common
{
	[Flags]
	public enum InstallingTo
	{
		BaseDir = 0x1,
		SysDirx86 = 0x2,
		CommonDesktop = 0x4,
		CommonStartMenu = 0x8,
		CommonAppData = 0x10,
		SysDirx64 = 0x20,
		WindowsRoot = 0x40,
		CommonFilesx86 = 0x80,
		CommonFilesx64 = 0x100,
		ServiceOrCOMReg = 0x200,
		NonCurrentUserReg = 0x400
	}
}
