using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using wyUpdate;

public static class LimitedProcess
{
	private struct STARTUPINFO
	{
		public int cb;

		public string lpReserved;

		public string lpDesktop;

		public string lpTitle;

		public uint dwX;

		public uint dwY;

		public uint dwXSize;

		public uint dwYSize;

		public uint dwXCountChars;

		public uint dwYCountChars;

		public uint dwFillAttribute;

		public uint dwFlags;

		public short wShowWindow;

		public short cbReserved2;

		public IntPtr lpReserved2;

		public IntPtr hStdInput;

		public IntPtr hStdOutput;

		public IntPtr hStdError;
	}

	private struct PROCESS_INFORMATION
	{
		public IntPtr hProcess;

		public IntPtr hThread;

		public uint dwProcessId;

		public uint dwThreadId;
	}

	private const uint INFINITE = uint.MaxValue;

	private const uint WAIT_ABANDONED = 128u;

	private const uint WAIT_OBJECT_0 = 0u;

	private const uint WAIT_TIMEOUT = 258u;

	private const uint WAIT_FAILED = uint.MaxValue;

	private const short SW_HIDE = 0;

	private const short SW_MAXIMIZE = 3;

	private const short SW_MINIMIZE = 6;

	private const uint STARTF_USESHOWWINDOW = 1u;

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern IntPtr GetShellWindow();

	[DllImport("user32.dll", SetLastError = true)]
	private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

	[DllImport("advapi32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);

	[DllImport("kernel32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool CloseHandle(IntPtr hObject);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	private static extern bool CreateProcessWithTokenW(IntPtr hToken, uint dwLogonFlags, string lpApplicationName, string lpCommandLine, uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, ref STARTUPINFO lpStartupInfo, out PROCESS_INFORMATION lpProcessInformation);

	[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern bool DuplicateTokenEx(IntPtr hExistingToken, uint dwDesiredAccess, IntPtr lpTokenAttributes, int ImpersonationLevel, int TokenType, out IntPtr phNewToken);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

	[DllImport("kernel32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool GetExitCodeProcess(IntPtr hProcess, out uint lpExitCode);

	public static uint Start(string filename, string arguments = null, bool fallback = true, bool waitForExit = false, ProcessWindowStyle windowStyle = ProcessWindowStyle.Normal)
	{
		bool flag = false;
		uint lpExitCode = 0u;
		string str = null;
		int num = 0;
		if (VistaTools.AtLeastVista() && VistaTools.IsUserAnAdmin())
		{
			if (!File.Exists(filename))
			{
				throw new Exception("The system cannot find the file specified");
			}
			IntPtr shellWindow = GetShellWindow();
			int lpdwProcessId = 0;
			if (shellWindow == IntPtr.Zero)
			{
				if (!fallback)
				{
					throw new Exception("Unable to locate shell window; you might be using a custom shell");
				}
			}
			else
			{
				GetWindowThreadProcessId(shellWindow, out lpdwProcessId);
			}
			if (lpdwProcessId != 0)
			{
				IntPtr intPtr = OpenProcess(1024, bInheritHandle: false, lpdwProcessId);
				if (intPtr != IntPtr.Zero)
				{
					if (OpenProcessToken(intPtr, 2u, out IntPtr TokenHandle))
					{
						if (DuplicateTokenEx(TokenHandle, 395u, IntPtr.Zero, 2, 1, out IntPtr phNewToken))
						{
							STARTUPINFO lpStartupInfo = default(STARTUPINFO);
							if (windowStyle != 0)
							{
								lpStartupInfo.dwFlags = 1u;
								switch (windowStyle)
								{
								case ProcessWindowStyle.Hidden:
									lpStartupInfo.wShowWindow = 0;
									break;
								case ProcessWindowStyle.Maximized:
									lpStartupInfo.wShowWindow = 3;
									break;
								case ProcessWindowStyle.Minimized:
									lpStartupInfo.wShowWindow = 6;
									break;
								}
							}
							lpStartupInfo.cb = Marshal.SizeOf((object)lpStartupInfo);
							if (filename.EndsWith(".exe"))
							{
								arguments = ((!string.IsNullOrEmpty(arguments)) ? ("\"" + filename + "\" " + arguments) : ("\"" + filename + "\""));
							}
							else
							{
								string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.System);
								arguments = ((!string.IsNullOrEmpty(arguments)) ? ("\"" + folderPath + "\\cmd.exe\" /c \"\"" + filename + "\" " + arguments + "\"") : ("\"" + folderPath + "\\cmd.exe\" /c \"" + filename + "\""));
								filename = folderPath + "\\cmd.exe";
							}
							flag = CreateProcessWithTokenW(phNewToken, 0u, filename, arguments, 0u, IntPtr.Zero, Path.GetDirectoryName(filename), ref lpStartupInfo, out PROCESS_INFORMATION lpProcessInformation);
							if (flag)
							{
								if (waitForExit)
								{
									if (WaitForSingleObject(lpProcessInformation.hProcess, uint.MaxValue) == uint.MaxValue)
									{
										num = Marshal.GetLastWin32Error();
										str = "WaitForSingleObject() failed with the error code " + num + ".";
									}
									else if (!GetExitCodeProcess(lpProcessInformation.hProcess, out lpExitCode))
									{
										num = Marshal.GetLastWin32Error();
										str = "GetExitCodeProcess() failed with the error code " + num + ".";
									}
								}
								CloseHandle(lpProcessInformation.hProcess);
								CloseHandle(lpProcessInformation.hThread);
							}
							else if (!fallback)
							{
								num = Marshal.GetLastWin32Error();
								str = "CreateProcessWithTokenW() failed with the error code " + num + ".";
							}
							CloseHandle(phNewToken);
						}
						else if (!fallback)
						{
							num = Marshal.GetLastWin32Error();
							str = "DuplicateTokenEx() on the desktop shell process failed with the error code " + num + ".";
						}
						CloseHandle(TokenHandle);
					}
					else if (!fallback)
					{
						num = Marshal.GetLastWin32Error();
						str = "OpenProcessToken() on the desktop shell process failed with the error code " + num + ".";
					}
					CloseHandle(intPtr);
				}
				else if (!fallback)
				{
					num = Marshal.GetLastWin32Error();
					str = "OpenProcess() on the desktop shell process failed with the error code " + num + ".";
				}
			}
			else if (!fallback)
			{
				str = "Unable to get the window thread process ID of the desktop shell process.";
			}
		}
		if (!flag)
		{
			if (!fallback)
			{
				throw new ExternalException("Failed to start as a limited process. " + str, num);
			}
			Process.Start(filename, arguments);
		}
		return lpExitCode;
	}
}
