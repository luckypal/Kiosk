using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Kiosk
{
	internal static class ControlDisplay
	{
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
			ENUM_CURRENT_SETTINGS = -1,
			ENUM_REGISTRY_SETTINGS = -2
		}

		public enum Display_Device_Stateflags
		{
			DISPLAY_DEVICE_ATTACHED_TO_DESKTOP = 1,
			DISPLAY_DEVICE_MIRRORING_DRIVER = 8,
			DISPLAY_DEVICE_MODESPRUNED = 0x8000000,
			DISPLAY_DEVICE_MULTI_DRIVER = 2,
			DISPLAY_DEVICE_PRIMARY_DEVICE = 4,
			DISPLAY_DEVICE_VGA_COMPATIBLE = 0x10
		}

		public enum DeviceFlags
		{
			CDS_FULLSCREEN = 4,
			CDS_GLOBAL = 8,
			CDS_NORESET = 0x10000000,
			CDS_RESET = 0x40000000,
			CDS_SET_PRIMARY = 0x10,
			CDS_TEST = 2,
			CDS_UPDATEREGISTRY = 1,
			CDS_VIDEOPARAMETERS = 0x20
		}

		public enum DEVMODE_Flags
		{
			DM_BITSPERPEL = 0x40000,
			DM_DISPLAYFLAGS = 0x200000,
			DM_DISPLAYFREQUENCY = 0x400000,
			DM_PELSHEIGHT = 0x100000,
			DM_PELSWIDTH = 0x80000,
			DM_POSITION = 0x20
		}

		public enum DisplaySetting_Results
		{
			DISP_CHANGE_BADFLAGS = -4,
			DISP_CHANGE_BADMODE = -2,
			DISP_CHANGE_BADPARAM = -5,
			DISP_CHANGE_FAILED = -1,
			DISP_CHANGE_NOTUPDATED = -3,
			DISP_CHANGE_RESTART = 1,
			DISP_CHANGE_SUCCESSFUL = 0
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
			public Display_Device_Stateflags StateFlags;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string DeviceID;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string DeviceKey;
		}

		public const int ENUM_CURRENT_SETTINGS = -1;

		public const int ENUM_REGISTRY_SETTINGS = -2;

		public const int CDS_UPDATEREGISTRY = 1;

		public const int CDS_TEST = 2;

		public const int DISP_CHANGE_SUCCESSFUL = 0;

		public const int DISP_CHANGE_RESTART = 1;

		public const int DISP_CHANGE_FAILED = -1;

		public static void SetDisplayMax()
		{
			Resolution(-1, -1);
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
			Resolution(-1, -1);
		}

		public static void SetDisplay(int _mx, int _my)
		{
			Resolution(_mx, _my);
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
			Resolution(_mx, _my);
		}

		[DllImport("user32.dll")]
		public static extern int EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);

		[DllImport("user32.dll")]
		public static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags);

		[DllImport("user32.dll")]
		public static extern bool EnumDisplayDevices(string lpDevice, int iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, int dwFlags);

		[DllImport("user32.dll")]
		public static extern int ChangeDisplaySettingsEx(string lpszDeviceName, [In] ref DEVMODE lpDevMode, IntPtr hwnd, int dwFlags, IntPtr lParam);

		public static void Resolution(int a, int b)
		{
			Screen primaryScreen = Screen.PrimaryScreen;
			int dmPelsWidth = a;
			int dmPelsHeight = b;
			if (a == -1 || b == -1)
			{
				DEVMODE devMode = default(DEVMODE);
				int i = 0;
				int num = 1024;
				int num2 = 768;
				for (; EnumDisplaySettings(null, i, ref devMode) > 0; i++)
				{
					if (devMode.dmPelsWidth > num)
					{
						num = devMode.dmPelsWidth;
						num2 = 768;
					}
					if (devMode.dmPelsWidth == num && devMode.dmPelsHeight > num2)
					{
						num2 = devMode.dmPelsHeight;
					}
				}
				dmPelsWidth = num;
				dmPelsHeight = num2;
			}
			DEVMODE devMode2 = default(DEVMODE);
			devMode2.dmDeviceName = new string(new char[32]);
			devMode2.dmFormName = new string(new char[32]);
			devMode2.dmSize = (short)Marshal.SizeOf((object)devMode2);
			if (0 == EnumDisplaySettings(null, -1, ref devMode2))
			{
				return;
			}
			devMode2.dmPelsWidth = dmPelsWidth;
			devMode2.dmPelsHeight = dmPelsHeight;
			int num3 = ChangeDisplaySettings(ref devMode2, 2);
			if (num3 == -1)
			{
				devMode2.dmPelsWidth = 1024;
				devMode2.dmPelsHeight = 768;
				num3 = ChangeDisplaySettings(ref devMode2, 2);
				num3 = ChangeDisplaySettings(ref devMode2, 1);
				return;
			}
			switch (ChangeDisplaySettings(ref devMode2, 1))
			{
			case 0:
				return;
			case 1:
				return;
			}
			devMode2.dmPelsWidth = 1024;
			devMode2.dmPelsHeight = 768;
			num3 = ChangeDisplaySettings(ref devMode2, 2);
			num3 = ChangeDisplaySettings(ref devMode2, 1);
		}

		public static DisplayApi.DEVMODE NewDevMode()
		{
			DisplayApi.DEVMODE dEVMODE = default(DisplayApi.DEVMODE);
			dEVMODE.dmDeviceName = new string(new char[31]);
			dEVMODE.dmFormName = new string(new char[31]);
			dEVMODE.dmSize = (ushort)Marshal.SizeOf((object)dEVMODE);
			return dEVMODE;
		}

		public static void SwapMonitor(int _1, int _2)
		{
			DisplayApi.DisplaySetting_Results displaySetting_Results = DisplayApi.DisplaySetting_Results.DISP_CHANGE_SUCCESSFUL;
			DisplayApi.DISPLAY_DEVICE lpDisplayDevice = default(DisplayApi.DISPLAY_DEVICE);
			lpDisplayDevice.cb = Marshal.SizeOf((object)lpDisplayDevice);
			int iDevNum = _2;
			DisplayApi.User_32.EnumDisplayDevices(null, iDevNum, ref lpDisplayDevice, 0);
			string deviceName = lpDisplayDevice.DeviceName;
			DisplayApi.DISPLAY_DEVICE lpDisplayDevice2 = default(DisplayApi.DISPLAY_DEVICE);
			lpDisplayDevice2.cb = Marshal.SizeOf((object)lpDisplayDevice2);
			iDevNum = _1;
			DisplayApi.User_32.EnumDisplayDevices(null, iDevNum, ref lpDisplayDevice2, 0);
			string deviceName2 = lpDisplayDevice2.DeviceName;
			DisplayApi.DEVMODE devMode = NewDevMode();
			DisplayApi.User_32.EnumDisplaySettings(deviceName, -2, ref devMode);
			DisplayApi.DEVMODE lpDevMode = NewDevMode();
			lpDevMode.dmFields = DisplayApi.DEVMODE_Flags.DM_POSITION;
			lpDevMode.dmPosition.x = (int)devMode.dmPelsWidth;
			lpDevMode.dmPosition.y = 0;
			displaySetting_Results = (DisplayApi.DisplaySetting_Results)DisplayApi.User_32.ChangeDisplaySettingsEx(deviceName2, ref lpDevMode, (IntPtr)null, 268435457, IntPtr.Zero);
			DisplayApi.DEVMODE devMode2 = NewDevMode();
			DisplayApi.User_32.EnumDisplaySettings(deviceName, -2, ref devMode2);
			DisplayApi.DEVMODE lpDevMode2 = NewDevMode();
			lpDevMode2.dmFields = DisplayApi.DEVMODE_Flags.DM_POSITION;
			lpDevMode2.dmPosition.x = 0;
			lpDevMode2.dmPosition.y = 0;
			displaySetting_Results = (DisplayApi.DisplaySetting_Results)DisplayApi.User_32.ChangeDisplaySettingsEx(deviceName, ref lpDevMode2, (IntPtr)null, 268435473, IntPtr.Zero);
			DisplayApi.DEVMODE lpDevMode3 = NewDevMode();
			displaySetting_Results = (DisplayApi.DisplaySetting_Results)DisplayApi.User_32.ChangeDisplaySettingsEx(deviceName2, ref lpDevMode3, (IntPtr)null, 1, (IntPtr)null);
			DisplayApi.DEVMODE lpDevMode4 = NewDevMode();
			displaySetting_Results = (DisplayApi.DisplaySetting_Results)DisplayApi.User_32.ChangeDisplaySettingsEx(deviceName, ref lpDevMode4, (IntPtr)null, 17, IntPtr.Zero);
		}
	}
}
