using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace wyUpdate.Common
{
	public static class SystemFolders
	{
		public const int FILE_ATTRIBUTE_DIRECTORY = 16;

		public const int FILE_ATTRIBUTE_FILE = 0;

		public const int MAX_PATH = 260;

		private static string m_CommonAppData;

		private static string m_CurrentAppData;

		private static string m_CurrentLocalAppData;

		private static string m_CommonDesktop;

		private static string m_CurrentDesktop;

		private static string m_CommonDocuments;

		private static string m_CommonProgramsStartMenu;

		private static string m_CurrentProgramsStartMenu;

		private static string m_CommonStartup;

		private static string m_System32x86;

		private static string m_System32x64;

		private static string m_RootDrive;

		private static string m_CommonProgramFilesx86;

		private static string m_CommonProgramFilesx64;

		private static string m_UserProfile;

		private static bool? is32on64;

		[DllImport("shell32.dll")]
		private static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken, uint dwFlags, StringBuilder pszPath);

		public static string GetCommonAppData()
		{
			if (m_CommonAppData == null)
			{
				StringBuilder stringBuilder = new StringBuilder(256);
				SHGetFolderPath(IntPtr.Zero, 35, IntPtr.Zero, 0u, stringBuilder);
				m_CommonAppData = stringBuilder.ToString();
			}
			return m_CommonAppData;
		}

		public static string GetCurrentUserAppData()
		{
			return m_CurrentAppData ?? (m_CurrentAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
		}

		public static string GetCurrentUserLocalAppData()
		{
			return m_CurrentLocalAppData ?? (m_CurrentLocalAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
		}

		public static string GetCommonDesktop()
		{
			if (m_CommonDesktop == null)
			{
				StringBuilder stringBuilder = new StringBuilder(256);
				SHGetFolderPath(IntPtr.Zero, 25, IntPtr.Zero, 0u, stringBuilder);
				m_CommonDesktop = stringBuilder.ToString();
			}
			return m_CommonDesktop;
		}

		public static string GetCurrentUserDesktop()
		{
			return m_CurrentDesktop ?? (m_CurrentDesktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
		}

		public static string GetCommonDocuments()
		{
			if (m_CommonDocuments == null)
			{
				StringBuilder stringBuilder = new StringBuilder(256);
				SHGetFolderPath(IntPtr.Zero, 46, IntPtr.Zero, 0u, stringBuilder);
				m_CommonDocuments = stringBuilder.ToString();
			}
			return m_CommonDocuments;
		}

		public static string GetCommonProgramsStartMenu()
		{
			if (m_CommonProgramsStartMenu == null)
			{
				StringBuilder stringBuilder = new StringBuilder(256);
				SHGetFolderPath(IntPtr.Zero, 23, IntPtr.Zero, 0u, stringBuilder);
				m_CommonProgramsStartMenu = stringBuilder.ToString();
			}
			return m_CommonProgramsStartMenu;
		}

		public static string GetCurrentUserProgramsStartMenu()
		{
			return m_CurrentProgramsStartMenu ?? (m_CurrentProgramsStartMenu = Environment.GetFolderPath(Environment.SpecialFolder.Programs));
		}

		public static string GetCommonProgramFilesx86()
		{
			if (m_CommonProgramFilesx86 == null)
			{
				StringBuilder stringBuilder = new StringBuilder(256);
				SHGetFolderPath(IntPtr.Zero, Is64Bit() ? 44 : 43, IntPtr.Zero, 0u, stringBuilder);
				m_CommonProgramFilesx86 = stringBuilder.ToString();
			}
			return m_CommonProgramFilesx86;
		}

		public static string GetCommonProgramFilesx64()
		{
			if (m_CommonProgramFilesx64 == null)
			{
				if (!Is64Bit())
				{
					return null;
				}
				StringBuilder stringBuilder = new StringBuilder(256);
				SHGetFolderPath(IntPtr.Zero, 43, IntPtr.Zero, 0u, stringBuilder);
				m_CommonProgramFilesx64 = stringBuilder.ToString();
			}
			return m_CommonProgramFilesx64;
		}

		public static string GetCommonStartup()
		{
			if (m_CommonStartup == null)
			{
				StringBuilder stringBuilder = new StringBuilder(256);
				SHGetFolderPath(IntPtr.Zero, 24, IntPtr.Zero, 0u, stringBuilder);
				m_CommonStartup = stringBuilder.ToString();
			}
			return m_CommonStartup;
		}

		public static string GetRootDrive()
		{
			return m_RootDrive ?? (m_RootDrive = Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, 3));
		}

		public static string GetUserProfile()
		{
			if (m_UserProfile == null)
			{
				StringBuilder stringBuilder = new StringBuilder(256);
				SHGetFolderPath(IntPtr.Zero, 40, IntPtr.Zero, 0u, stringBuilder);
				m_UserProfile = stringBuilder.ToString();
				if (string.IsNullOrEmpty(m_UserProfile))
				{
					m_UserProfile = Environment.GetEnvironmentVariable("userprofile");
				}
			}
			return m_UserProfile;
		}

		public static string GetSystem32x86()
		{
			if (m_System32x86 == null)
			{
				if (Is64Bit())
				{
					StringBuilder stringBuilder = new StringBuilder(256);
					SHGetFolderPath(IntPtr.Zero, 41, IntPtr.Zero, 0u, stringBuilder);
					m_System32x86 = stringBuilder.ToString();
				}
				else
				{
					m_System32x86 = Environment.GetFolderPath(Environment.SpecialFolder.System);
				}
			}
			return m_System32x86;
		}

		public static string GetSystem32x64()
		{
			if (m_System32x64 == null)
			{
				if (!Is64Bit())
				{
					return null;
				}
				m_System32x64 = Environment.GetFolderPath(Environment.SpecialFolder.System);
			}
			return m_System32x64;
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool IsWow64Process([In] IntPtr hProcess, out bool lpSystemInfo);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		private static extern UIntPtr GetProcAddress(IntPtr hModule, string procName);

		public static bool Is64Bit()
		{
			if (IntPtr.Size != 8)
			{
				if (IntPtr.Size == 4)
				{
					return Is32BitProcessOn64BitProcessor();
				}
				return false;
			}
			return true;
		}

		private static bool Is32BitProcessOn64BitProcessor()
		{
			if (!is32on64.HasValue)
			{
				UIntPtr procAddress = GetProcAddress(GetModuleHandle("kernel32.dll"), "IsWow64Process");
				if (procAddress == UIntPtr.Zero)
				{
					is32on64 = false;
				}
				else
				{
					IsWow64Process(Process.GetCurrentProcess().Handle, out bool lpSystemInfo);
					is32on64 = lpSystemInfo;
				}
			}
			return is32on64.Value;
		}

		[DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
		public static extern bool PathRelativePathTo([Out] StringBuilder pszPath, [In] string pszFrom, [In] uint dwAttrFrom, [In] string pszTo, [In] uint dwAttrTo);

		public static bool IsFileInDirectory(string dir, string file)
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			if (PathRelativePathTo(stringBuilder, dir, 16u, file, 0u) && stringBuilder.Length >= 2)
			{
				return stringBuilder.ToString().Substring(0, 2) == ".\\";
			}
			return false;
		}

		public static bool IsDirInDir(string dir, string checkDir)
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			if (PathRelativePathTo(stringBuilder, dir, 16u, checkDir, 16u))
			{
				if (stringBuilder.Length != 1)
				{
					if (stringBuilder.Length >= 2)
					{
						return stringBuilder.ToString().Substring(0, 2) == ".\\";
					}
					return false;
				}
				return true;
			}
			return false;
		}
	}
}
