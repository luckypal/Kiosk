using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace wyUpdate
{
	internal static class CmdLineToArgvW
	{
		[DllImport("shell32.dll", SetLastError = true)]
		private static extern IntPtr CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);

		[DllImport("kernel32.dll")]
		private static extern IntPtr LocalFree(IntPtr hMem);

		public static string[] SplitArgs(string unsplitArgumentLine)
		{
			int pNumArgs;
			IntPtr intPtr = CommandLineToArgvW(unsplitArgumentLine, out pNumArgs);
			if (intPtr == IntPtr.Zero)
			{
				throw new ArgumentException("Unable to split argument.", new Win32Exception());
			}
			try
			{
				string[] array = new string[pNumArgs];
				for (int i = 0; i < pNumArgs; i++)
				{
					array[i] = Marshal.PtrToStringUni(Marshal.ReadIntPtr(intPtr, i * IntPtr.Size));
				}
				return array;
			}
			finally
			{
				LocalFree(intPtr);
			}
		}
	}
}
