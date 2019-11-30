using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace wyUpdate
{
	internal static class Program
	{
		[STAThread]
		public static string Freeze_Timestamp()
		{
			return $"{DateTime.Now.Ticks / 10000000}";
		}

		public static int Freeze_Check_Timestamp()
		{
			string path = "c:\\kiosk\\unfreezeboot.tmp";
			if (!File.Exists(path))
			{
				return 0;
			}
			try
			{
				string value = File.ReadAllText(path);
				File.Delete(path);
				Application.DoEvents();
				long num = Convert.ToInt64(value);
				num += 120;
				long num2 = Convert.ToInt64(Freeze_Timestamp());
				if (num2 <= num)
				{
					return 1;
				}
			}
			catch
			{
			}
			return 0;
		}

		public static void UnLock_Windows()
		{
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Keyboard Layout", writable: true);
				registryKey.DeleteValue("Scancode Map");
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", writable: true);
				registryKey2.SetValue("NoWinKeys", 0, RegistryValueKind.DWord);
				registryKey2.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey3 = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", writable: true);
				registryKey3.SetValue("NoWinKeys", 0, RegistryValueKind.DWord);
				registryKey3.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey4 = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", writable: true);
				registryKey4.SetValue("NoViewContextMenu", 0, RegistryValueKind.DWord);
				registryKey4.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey5 = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", writable: true);
				registryKey5.SetValue("NoViewContextMenu", 0, RegistryValueKind.DWord);
				registryKey5.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey6 = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", writable: true);
				registryKey6.SetValue("DisableTaskMgr", 0, RegistryValueKind.DWord);
				registryKey6.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey7 = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", writable: true);
				registryKey7.SetValue("NoDesktop", 0, RegistryValueKind.DWord);
				registryKey7.Close();
			}
			catch
			{
			}
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.System);
			string path = folderPath + "\\kbfmgr.exe";
			if (!File.Exists(path))
			{
				try
				{
					using (Process process = Process.Start("kbfmgr.exe", "/disable"))
					{
						process.WaitForExit();
					}
				}
				catch
				{
				}
			}
		}

		private static void Reset()
		{
			Process.Start("shutdown.exe", "/r /t 4");
			while (true)
			{
				Application.DoEvents();
			}
		}

		public static int Freeze_Check()
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
			string text = folderPath + "\\Toolwiz Time Freeze 2014\\ToolwizTimeFreeze.exe";
			if (File.Exists(text))
			{
				Process.Start(text, "/check /usepass=kiosksave");
				Thread.Sleep(2000);
				try
				{
					RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Toolwiz\\TimefreezeNew");
					if (registryKey != null)
					{
						object value = registryKey.GetValue("CURRENT_PROTECT_MODE");
						if (value != null)
						{
							int num = Convert.ToInt32(value);
							if (num == 1)
							{
								return 1;
							}
						}
					}
				}
				catch
				{
				}
				return 0;
			}
			return -1;
		}

		private static int Main(string[] args)
		{
			Application.EnableVisualStyles();
			bool onlyinstall = false;
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			if (commandLineArgs.Length > 1)
			{
				string a = commandLineArgs[1].ToLower();
				if (a == "/noloader".ToLower())
				{
					onlyinstall = true;
				}
			}
			int num = 0;
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\WinLogon", writable: true);
				string text = registryKey.GetValue("Shell").ToString();
				registryKey.Close();
				if (!text.ToLower().Contains("loader.exe".ToLower()))
				{
					num = 1;
				}
			}
			catch
			{
			}
			if (num == 1)
			{
				onlyinstall = true;
			}
			frmMain frmMain = new frmMain(args, onlyinstall);
			if (frmMain.IsDisposed)
			{
				return 0;
			}
			StringBuilder stringBuilder = new StringBuilder("Local\\wyUpdate-" + frmMain.update.GUID);
			if (frmMain.IsAdmin)
			{
				stringBuilder.Append('a');
			}
			if (frmMain.SelfUpdateState == SelfUpdateState.FullUpdate)
			{
				stringBuilder.Append('s');
			}
			if (frmMain.IsNewSelf)
			{
				stringBuilder.Append('n');
			}
			Mutex mutex = new Mutex(initiallyOwned: true, stringBuilder.ToString());
			if (mutex.WaitOne(TimeSpan.Zero, exitContext: true))
			{
				Application.Run(frmMain);
				mutex.ReleaseMutex();
			}
			else
			{
				FocusOtherProcess();
			}
			return frmMain.ReturnCode;
		}

		[DllImport("user32")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32")]
		private static extern int ShowWindow(IntPtr hWnd, int swCommand);

		[DllImport("user32")]
		private static extern bool IsIconic(IntPtr hWnd);

		public static void FocusOtherProcess()
		{
			Process currentProcess = Process.GetCurrentProcess();
			string name = Assembly.GetExecutingAssembly().GetName().Name;
			Process[] processesByName = Process.GetProcessesByName(name);
			int num = 0;
			Process process;
			while (true)
			{
				if (num < processesByName.Length)
				{
					process = processesByName[num];
					if (currentProcess.Id != process.Id && process.MainModule != null && currentProcess.MainModule != null && currentProcess.MainModule.FileName == process.MainModule.FileName)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			IntPtr mainWindowHandle = process.MainWindowHandle;
			if (IsIconic(mainWindowHandle))
			{
				ShowWindow(mainWindowHandle, 9);
			}
			SetForegroundWindow(mainWindowHandle);
		}
	}
}
