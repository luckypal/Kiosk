// Decompiled with JetBrains decompiler
// Type: Kiosk.ControlDisplay
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Kiosk
{
  internal static class ControlDisplay
  {
    public const int ENUM_CURRENT_SETTINGS = -1;
    public const int ENUM_REGISTRY_SETTINGS = -2;
    public const int CDS_UPDATEREGISTRY = 1;
    public const int CDS_TEST = 2;
    public const int DISP_CHANGE_SUCCESSFUL = 0;
    public const int DISP_CHANGE_RESTART = 1;
    public const int DISP_CHANGE_FAILED = -1;

    public static void SetDisplayMax()
    {
      ControlDisplay.Resolution(-1, -1);
      Process process = new Process();
      try
      {
        process.StartInfo.WorkingDirectory = "c:\\windows\\system32";
        process.StartInfo.FileName = "DisplaySwitch.exe";
        process.StartInfo.Arguments = "/clone";
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
        process.Start();
      }
      catch
      {
      }
      ControlDisplay.Resolution(-1, -1);
    }

    public static void SetDisplay(int _mx, int _my)
    {
      ControlDisplay.Resolution(_mx, _my);
      Process process = new Process();
      try
      {
        process.StartInfo.WorkingDirectory = "c:\\windows\\system32";
        process.StartInfo.FileName = "DisplaySwitch.exe";
        process.StartInfo.Arguments = "/clone";
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
        process.Start();
      }
      catch
      {
      }
      ControlDisplay.Resolution(_mx, _my);
    }

    [DllImport("user32.dll")]
    public static extern int EnumDisplaySettings(
      string deviceName,
      int modeNum,
      ref ControlDisplay.DEVMODE devMode);

    [DllImport("user32.dll")]
    public static extern int ChangeDisplaySettings(ref ControlDisplay.DEVMODE devMode, int flags);

    [DllImport("user32.dll")]
    public static extern bool EnumDisplayDevices(
      string lpDevice,
      int iDevNum,
      ref ControlDisplay.DISPLAY_DEVICE lpDisplayDevice,
      int dwFlags);

    [DllImport("user32.dll")]
    public static extern int ChangeDisplaySettingsEx(
      string lpszDeviceName,
      [In] ref ControlDisplay.DEVMODE lpDevMode,
      IntPtr hwnd,
      int dwFlags,
      IntPtr lParam);

    public static void Resolution(int a, int b)
    {
      Screen primaryScreen = Screen.PrimaryScreen;
      int num1 = a;
      int num2 = b;
      if (a == -1 || b == -1)
      {
        ControlDisplay.DEVMODE devMode = new ControlDisplay.DEVMODE();
        int modeNum = 0;
        int num3 = 1024;
        int num4 = 768;
        for (; ControlDisplay.EnumDisplaySettings((string) null, modeNum, ref devMode) > 0; ++modeNum)
        {
          if (devMode.dmPelsWidth > num3)
          {
            num3 = devMode.dmPelsWidth;
            num4 = 768;
          }
          if (devMode.dmPelsWidth == num3 && devMode.dmPelsHeight > num4)
            num4 = devMode.dmPelsHeight;
        }
        num1 = num3;
        num2 = num4;
      }
      ControlDisplay.DEVMODE devMode1 = new ControlDisplay.DEVMODE()
      {
        dmDeviceName = new string(new char[32]),
        dmFormName = new string(new char[32])
      };
      devMode1.dmSize = (short) Marshal.SizeOf((object) devMode1);
      if (0 == ControlDisplay.EnumDisplaySettings((string) null, -1, ref devMode1))
        return;
      devMode1.dmPelsWidth = num1;
      devMode1.dmPelsHeight = num2;
      int num5;
      if (ControlDisplay.ChangeDisplaySettings(ref devMode1, 2) == -1)
      {
        devMode1.dmPelsWidth = 1024;
        devMode1.dmPelsHeight = 768;
        num5 = ControlDisplay.ChangeDisplaySettings(ref devMode1, 2);
        num5 = ControlDisplay.ChangeDisplaySettings(ref devMode1, 1);
      }
      else
      {
        switch (ControlDisplay.ChangeDisplaySettings(ref devMode1, 1))
        {
          case 0:
          case 1:
            break;
          default:
            devMode1.dmPelsWidth = 1024;
            devMode1.dmPelsHeight = 768;
            num5 = ControlDisplay.ChangeDisplaySettings(ref devMode1, 2);
            num5 = ControlDisplay.ChangeDisplaySettings(ref devMode1, 1);
            goto case 0;
        }
      }
    }

    public static DisplayApi.DEVMODE NewDevMode()
    {
      DisplayApi.DEVMODE devmode = new DisplayApi.DEVMODE()
      {
        dmDeviceName = new string(new char[31]),
        dmFormName = new string(new char[31])
      };
      devmode.dmSize = (ushort) Marshal.SizeOf((object) devmode);
      return devmode;
    }

    public static unsafe void SwapMonitor(int _1, int _2)
    {
      DisplayApi.DisplaySetting_Results displaySettingResults = DisplayApi.DisplaySetting_Results.DISP_CHANGE_SUCCESSFUL;
      DisplayApi.DISPLAY_DEVICE lpDisplayDevice1 = new DisplayApi.DISPLAY_DEVICE();
      lpDisplayDevice1.cb = Marshal.SizeOf((object) lpDisplayDevice1);
      DisplayApi.User_32.EnumDisplayDevices((string) null, _2, ref lpDisplayDevice1, 0);
      string deviceName1 = lpDisplayDevice1.DeviceName;
      DisplayApi.DISPLAY_DEVICE lpDisplayDevice2 = new DisplayApi.DISPLAY_DEVICE();
      lpDisplayDevice2.cb = Marshal.SizeOf((object) lpDisplayDevice2);
      DisplayApi.User_32.EnumDisplayDevices((string) null, _1, ref lpDisplayDevice2, 0);
      string deviceName2 = lpDisplayDevice2.DeviceName;
      DisplayApi.DEVMODE devMode1 = ControlDisplay.NewDevMode();
      DisplayApi.User_32.EnumDisplaySettings(deviceName1, -2, ref devMode1);
      DisplayApi.DEVMODE lpDevMode1 = ControlDisplay.NewDevMode();
      lpDevMode1.dmFields = DisplayApi.DEVMODE_Flags.DM_POSITION;
      lpDevMode1.dmPosition.x = (int) devMode1.dmPelsWidth;
      lpDevMode1.dmPosition.y = 0;
      displaySettingResults = (DisplayApi.DisplaySetting_Results) DisplayApi.User_32.ChangeDisplaySettingsEx(deviceName2, ref lpDevMode1, (IntPtr) ((void*) null), 268435457, IntPtr.Zero);
      DisplayApi.DEVMODE devMode2 = ControlDisplay.NewDevMode();
      DisplayApi.User_32.EnumDisplaySettings(deviceName1, -2, ref devMode2);
      DisplayApi.DEVMODE lpDevMode2 = ControlDisplay.NewDevMode();
      lpDevMode2.dmFields = DisplayApi.DEVMODE_Flags.DM_POSITION;
      lpDevMode2.dmPosition.x = 0;
      lpDevMode2.dmPosition.y = 0;
      displaySettingResults = (DisplayApi.DisplaySetting_Results) DisplayApi.User_32.ChangeDisplaySettingsEx(deviceName1, ref lpDevMode2, (IntPtr) ((void*) null), 268435473, IntPtr.Zero);
      DisplayApi.DEVMODE lpDevMode3 = ControlDisplay.NewDevMode();
      displaySettingResults = (DisplayApi.DisplaySetting_Results) DisplayApi.User_32.ChangeDisplaySettingsEx(deviceName2, ref lpDevMode3, (IntPtr) ((void*) null), 1, (IntPtr) ((void*) null));
      DisplayApi.DEVMODE lpDevMode4 = ControlDisplay.NewDevMode();
      displaySettingResults = (DisplayApi.DisplaySetting_Results) DisplayApi.User_32.ChangeDisplaySettingsEx(deviceName1, ref lpDevMode4, (IntPtr) ((void*) null), 17, IntPtr.Zero);
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
      public short dmSpecVersion;
      public short dmDriverVersion;
      public short dmSize;
      public short dmDriverExtra;
      public int dmFields;
      public short dmOrientation;
      public short dmPaperSize;
      public short dmPaperLength;
      public short dmPaperWidth;
      public short dmScale;
      public short dmCopies;
      public short dmDefaultSource;
      public short dmPrintQuality;
      public short dmColor;
      public short dmDuplex;
      public short dmYResolution;
      public short dmTTOption;
      public short dmCollate;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
      public string dmFormName;
      public short dmLogPixels;
      public short dmBitsPerPel;
      public int dmPelsWidth;
      public int dmPelsHeight;
      public int dmDisplayFlags;
      public int dmDisplayFrequency;
      public int dmICMMethod;
      public int dmICMIntent;
      public int dmMediaType;
      public int dmDitherType;
      public int dmReserved1;
      public int dmReserved2;
      public int dmPanningWidth;
      public int dmPanningHeight;
    }

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

    public struct DISPLAY_DEVICE
    {
      [MarshalAs(UnmanagedType.U4)]
      public int cb;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
      public string DeviceName;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
      public string DeviceString;
      [MarshalAs(UnmanagedType.U4)]
      public ControlDisplay.Display_Device_Stateflags StateFlags;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
      public string DeviceID;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
      public string DeviceKey;
    }
  }
}
