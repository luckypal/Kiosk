using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public static class SystemMenu
{
	[DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
	private static extern IntPtr GetSystemMenu(IntPtr WindowHandle, int bReset);

	[DllImport("user32.dll", SetLastError = true)]
	private static extern int EnableMenuItem(IntPtr menu, int ideEnableItem, int enable);

	public static void DisableCloseButton(Form form)
	{
		IntPtr systemMenu = GetSystemMenu(form.Handle, 0);
		EnableMenuItem(systemMenu, 61536, 1);
	}

	public static void EnableCloseButton(Form form)
	{
		IntPtr systemMenu = GetSystemMenu(form.Handle, 0);
		EnableMenuItem(systemMenu, 61536, 0);
	}
}
