using System;
using System.Runtime.InteropServices;

namespace wyDay.Controls
{
	public static class Windows7Taskbar
	{
		private static ITaskbarList3 _taskbarList;

		private static readonly OperatingSystem osInfo = Environment.OSVersion;

		internal static ITaskbarList3 TaskbarList
		{
			get
			{
				if (_taskbarList == null)
				{
					lock (typeof(Windows7Taskbar))
					{
						if (_taskbarList == null)
						{
							_taskbarList = (ITaskbarList3)new CTaskbarList();
							_taskbarList.HrInit();
						}
					}
				}
				return _taskbarList;
			}
		}

		internal static bool Windows7OrGreater
		{
			get
			{
				if (osInfo.Version.Major != 6 || osInfo.Version.Minor < 1)
				{
					return osInfo.Version.Major > 6;
				}
				return true;
			}
		}

		public static void SetProgressState(IntPtr hwnd, ThumbnailProgressState state)
		{
			if (Windows7OrGreater)
			{
				TaskbarList.SetProgressState(hwnd, state);
			}
		}

		public static void SetProgressValue(IntPtr hwnd, ulong current, ulong maximum)
		{
			if (Windows7OrGreater)
			{
				TaskbarList.SetProgressValue(hwnd, current, maximum);
			}
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		internal static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
	}
}
