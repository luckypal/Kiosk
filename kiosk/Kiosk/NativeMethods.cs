using System;
using System.Runtime.InteropServices;

namespace Kiosk
{
	internal static class NativeMethods
	{
		internal enum DeviceCap
		{
			DRIVERVERSION = 0,
			TECHNOLOGY = 2,
			HORZSIZE = 4,
			VERTSIZE = 6,
			HORZRES = 8,
			VERTRES = 10,
			BITSPIXEL = 12,
			PLANES = 14,
			NUMBRUSHES = 0x10,
			NUMPENS = 18,
			NUMMARKERS = 20,
			NUMFONTS = 22,
			NUMCOLORS = 24,
			PDEVICESIZE = 26,
			CURVECAPS = 28,
			LINECAPS = 30,
			POLYGONALCAPS = 0x20,
			TEXTCAPS = 34,
			CLIPCAPS = 36,
			RASTERCAPS = 38,
			ASPECTX = 40,
			ASPECTY = 42,
			ASPECTXY = 44,
			SHADEBLENDCAPS = 45,
			LOGPIXELSX = 88,
			LOGPIXELSY = 90,
			SIZEPALETTE = 104,
			NUMRESERVED = 106,
			COLORRES = 108,
			PHYSICALWIDTH = 110,
			PHYSICALHEIGHT = 111,
			PHYSICALOFFSETX = 112,
			PHYSICALOFFSETY = 113,
			SCALINGFACTORX = 114,
			SCALINGFACTORY = 115,
			VREFRESH = 116,
			DESKTOPVERTRES = 117,
			DESKTOPHORZRES = 118,
			BLTALIGNMENT = 119
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal class DOCINFO
		{
			public int cbSize = 20;

			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpszDocName;

			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpszOutput;

			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpszDatatype;

			public int fwType;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal class DOC_INFO_1
		{
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pDocName;

			[MarshalAs(UnmanagedType.LPWStr)]
			public string pOutputFile;

			[MarshalAs(UnmanagedType.LPWStr)]
			public string pDataType;
		}

		[DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
		internal static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);

		[DllImport("gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool DeleteDC(IntPtr hdc);

		[DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "OpenPrinterW", ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPWStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

		[DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool ClosePrinter(IntPtr hPrinter);

		[DllImport("gdi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int StartDoc(IntPtr hdc, DOCINFO lpdi);

		[DllImport("gdi32.dll")]
		internal static extern int EndDoc(IntPtr hdc);

		[DllImport("gdi32.dll")]
		internal static extern int GetDeviceCaps(IntPtr hdc, DeviceCap capindex);

		[DllImport("gdi32.dll")]
		internal static extern int StartPage(IntPtr hdc);

		[DllImport("gdi32.dll")]
		internal static extern int EndPage(IntPtr hdc);

		[DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "StartDocPrinterW", ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool StartDocPrinter(IntPtr hPrinter, int level, [In] [MarshalAs(UnmanagedType.LPStruct)] DOC_INFO_1 di);

		[DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool EndDocPrinter(IntPtr hPrinter);

		[DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool StartPagePrinter(IntPtr hPrinter);

		[DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool EndPagePrinter(IntPtr hPrinter);

		[DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);
	}
}
