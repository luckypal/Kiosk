using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace wyUpdate
{
	public class VistaTools
	{
		public static bool AtLeastVista()
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				return Environment.OSVersion.Version.Major > 5;
			}
			return false;
		}

		[DllImport("shell32.dll", CharSet = CharSet.Unicode, EntryPoint = "#680")]
		public static extern bool IsUserAnAdmin();

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		public static void SetButtonShield(Button btn, bool showShield)
		{
			SendMessage(new HandleRef(btn, btn.Handle), 5644u, IntPtr.Zero, showShield ? new IntPtr(1) : IntPtr.Zero);
		}
	}
}
