// Decompiled with JetBrains decompiler
// Type: Kiosk.NativeMethods
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.Runtime.InteropServices;

namespace Kiosk
{
  internal static class NativeMethods
  {
    [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
    internal static extern IntPtr CreateDC(
      string lpszDriver,
      string lpszDevice,
      string lpszOutput,
      IntPtr lpInitData);

    [DllImport("gdi32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool DeleteDC(IntPtr hdc);

    [DllImport("winspool.drv", EntryPoint = "OpenPrinterW", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPWStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

    [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool ClosePrinter(IntPtr hPrinter);

    [DllImport("gdi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int StartDoc(IntPtr hdc, NativeMethods.DOCINFO lpdi);

    [DllImport("gdi32.dll")]
    internal static extern int EndDoc(IntPtr hdc);

    [DllImport("gdi32.dll")]
    internal static extern int GetDeviceCaps(IntPtr hdc, NativeMethods.DeviceCap capindex);

    [DllImport("gdi32.dll")]
    internal static extern int StartPage(IntPtr hdc);

    [DllImport("gdi32.dll")]
    internal static extern int EndPage(IntPtr hdc);

    [DllImport("winspool.drv", EntryPoint = "StartDocPrinterW", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool StartDocPrinter(
      IntPtr hPrinter,
      int level,
      [MarshalAs(UnmanagedType.LPStruct), In] NativeMethods.DOC_INFO_1 di);

    [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool EndDocPrinter(IntPtr hPrinter);

    [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool StartPagePrinter(IntPtr hPrinter);

    [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool EndPagePrinter(IntPtr hPrinter);

    [DllImport("winspool.drv", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool WritePrinter(
      IntPtr hPrinter,
      IntPtr pBytes,
      int dwCount,
      out int dwWritten);

    internal enum DeviceCap
    {
      DRIVERVERSION = 0,
      TECHNOLOGY = 2,
      HORZSIZE = 4,
      VERTSIZE = 6,
      HORZRES = 8,
      VERTRES = 10, // 0x0000000A
      BITSPIXEL = 12, // 0x0000000C
      PLANES = 14, // 0x0000000E
      NUMBRUSHES = 16, // 0x00000010
      NUMPENS = 18, // 0x00000012
      NUMMARKERS = 20, // 0x00000014
      NUMFONTS = 22, // 0x00000016
      NUMCOLORS = 24, // 0x00000018
      PDEVICESIZE = 26, // 0x0000001A
      CURVECAPS = 28, // 0x0000001C
      LINECAPS = 30, // 0x0000001E
      POLYGONALCAPS = 32, // 0x00000020
      TEXTCAPS = 34, // 0x00000022
      CLIPCAPS = 36, // 0x00000024
      RASTERCAPS = 38, // 0x00000026
      ASPECTX = 40, // 0x00000028
      ASPECTY = 42, // 0x0000002A
      ASPECTXY = 44, // 0x0000002C
      SHADEBLENDCAPS = 45, // 0x0000002D
      LOGPIXELSX = 88, // 0x00000058
      LOGPIXELSY = 90, // 0x0000005A
      SIZEPALETTE = 104, // 0x00000068
      NUMRESERVED = 106, // 0x0000006A
      COLORRES = 108, // 0x0000006C
      PHYSICALWIDTH = 110, // 0x0000006E
      PHYSICALHEIGHT = 111, // 0x0000006F
      PHYSICALOFFSETX = 112, // 0x00000070
      PHYSICALOFFSETY = 113, // 0x00000071
      SCALINGFACTORX = 114, // 0x00000072
      SCALINGFACTORY = 115, // 0x00000073
      VREFRESH = 116, // 0x00000074
      DESKTOPVERTRES = 117, // 0x00000075
      DESKTOPHORZRES = 118, // 0x00000076
      BLTALIGNMENT = 119, // 0x00000077
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
  }
}
