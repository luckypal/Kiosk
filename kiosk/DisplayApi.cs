// Decompiled with JetBrains decompiler
// Type: DisplayApi
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.Runtime.InteropServices;

public class DisplayApi
{
  public const int CCHDEVICENAME = 32;
  public const int CCHFORMNAME = 32;

  public enum DEVMODE_SETTINGS
  {
    ENUM_REGISTRY_SETTINGS = -2,
    ENUM_CURRENT_SETTINGS = -1,
  }

  public enum Display_Device_Stateflags
  {
    DISPLAY_DEVICE_ATTACHED_TO_DESKTOP = 1,
    DISPLAY_DEVICE_MULTI_DRIVER = 2,
    DISPLAY_DEVICE_PRIMARY_DEVICE = 4,
    DISPLAY_DEVICE_MIRRORING_DRIVER = 8,
    DISPLAY_DEVICE_VGA_COMPATIBLE = 16, // 0x00000010
    DISPLAY_DEVICE_MODESPRUNED = 134217728, // 0x08000000
  }

  public enum DeviceFlags
  {
    CDS_UPDATEREGISTRY = 1,
    CDS_TEST = 2,
    CDS_FULLSCREEN = 4,
    CDS_GLOBAL = 8,
    CDS_SET_PRIMARY = 16, // 0x00000010
    CDS_VIDEOPARAMETERS = 32, // 0x00000020
    CDS_NORESET = 268435456, // 0x10000000
    CDS_RESET = 1073741824, // 0x40000000
  }

  public enum DEVMODE_Flags
  {
    DM_POSITION = 32, // 0x00000020
    DM_BITSPERPEL = 262144, // 0x00040000
    DM_PELSWIDTH = 524288, // 0x00080000
    DM_PELSHEIGHT = 1048576, // 0x00100000
    DM_DISPLAYFLAGS = 2097152, // 0x00200000
    DM_DISPLAYFREQUENCY = 4194304, // 0x00400000
  }

  public enum DisplaySetting_Results
  {
    DISP_CHANGE_BADPARAM = -5,
    DISP_CHANGE_BADFLAGS = -4,
    DISP_CHANGE_NOTUPDATED = -3,
    DISP_CHANGE_BADMODE = -2,
    DISP_CHANGE_FAILED = -1,
    DISP_CHANGE_SUCCESSFUL = 0,
    DISP_CHANGE_RESTART = 1,
  }

  public struct POINTL
  {
    [MarshalAs(UnmanagedType.I4)]
    public int x;
    [MarshalAs(UnmanagedType.I4)]
    public int y;
  }

  public struct DEVMODE
  {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string dmDeviceName;
    [MarshalAs(UnmanagedType.U2)]
    public ushort dmSpecVersion;
    [MarshalAs(UnmanagedType.U2)]
    public ushort dmDriverVersion;
    [MarshalAs(UnmanagedType.U2)]
    public ushort dmSize;
    [MarshalAs(UnmanagedType.U2)]
    public ushort dmDriverExtra;
    [MarshalAs(UnmanagedType.U4)]
    public DisplayApi.DEVMODE_Flags dmFields;
    public DisplayApi.POINTL dmPosition;
    [MarshalAs(UnmanagedType.U4)]
    public uint dmDisplayOrientation;
    [MarshalAs(UnmanagedType.U4)]
    public uint dmDisplayFixedOutput;
    [MarshalAs(UnmanagedType.I2)]
    public short dmColor;
    [MarshalAs(UnmanagedType.I2)]
    public short dmDuplex;
    [MarshalAs(UnmanagedType.I2)]
    public short dmYResolution;
    [MarshalAs(UnmanagedType.I2)]
    public short dmTTOption;
    [MarshalAs(UnmanagedType.I2)]
    public short dmCollate;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string dmFormName;
    [MarshalAs(UnmanagedType.U2)]
    public ushort dmLogPixels;
    [MarshalAs(UnmanagedType.U4)]
    public uint dmBitsPerPel;
    [MarshalAs(UnmanagedType.U4)]
    public uint dmPelsWidth;
    [MarshalAs(UnmanagedType.U4)]
    public uint dmPelsHeight;
    [MarshalAs(UnmanagedType.U4)]
    public uint dmDisplayFlags;
    [MarshalAs(UnmanagedType.U4)]
    public uint dmDisplayFrequency;
    [MarshalAs(UnmanagedType.U4)]
    public uint dmICMMethod;
    [MarshalAs(UnmanagedType.U4)]
    public uint dmICMIntent;
    [MarshalAs(UnmanagedType.U4)]
    public uint dmMediaType;
    [MarshalAs(UnmanagedType.U4)]
    public uint dmDitherType;
    [MarshalAs(UnmanagedType.U4)]
    public uint dmReserved1;
    [MarshalAs(UnmanagedType.U4)]
    public uint dmReserved2;
    [MarshalAs(UnmanagedType.U4)]
    public uint dmPanningWidth;
    [MarshalAs(UnmanagedType.U4)]
    public uint dmPanningHeight;
  }

  public struct DISPLAY_DEVICE
  {
    [MarshalAs(UnmanagedType.U4)]
    public int cb;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string DeviceName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string DeviceString;
    [MarshalAs(UnmanagedType.U4)]
    public DisplayApi.Display_Device_Stateflags StateFlags;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string DeviceID;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string DeviceKey;
  }

  public class User_32
  {
    [DllImport("user32.dll")]
    public static extern int ChangeDisplaySettings(ref DisplayApi.DEVMODE devMode, int flags);

    [DllImport("user32.dll")]
    public static extern int ChangeDisplaySettingsEx(
      string lpszDeviceName,
      [In] ref DisplayApi.DEVMODE lpDevMode,
      IntPtr hwnd,
      int dwFlags,
      IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern bool EnumDisplayDevices(
      string lpDevice,
      int iDevNum,
      ref DisplayApi.DISPLAY_DEVICE lpDisplayDevice,
      int dwFlags);

    [DllImport("user32.dll")]
    public static extern int EnumDisplaySettings(
      string deviceName,
      int modeNum,
      ref DisplayApi.DEVMODE devMode);
  }
}
