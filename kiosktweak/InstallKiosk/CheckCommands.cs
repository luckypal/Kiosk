using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;

namespace InstallKiosk
{
	public class CheckCommands
	{
		public string Last_error;

		public static string Check_Chrome()
		{
			string empty = string.Empty;
			RegistryKey localMachine = Registry.LocalMachine;
			RegistryKey registryKey = localMachine.OpenSubKey("Software\\" + empty + "Google\\Update\\Clients");
			if (registryKey == null)
			{
				localMachine = Registry.CurrentUser;
				registryKey = localMachine.OpenSubKey("Software\\" + empty + "Google\\Update\\Clients");
			}
			if (registryKey == null)
			{
				localMachine = Registry.LocalMachine;
				registryKey = localMachine.OpenSubKey("Software\\Google\\Update\\Clients");
			}
			if (registryKey == null)
			{
				localMachine = Registry.CurrentUser;
				registryKey = localMachine.OpenSubKey("Software\\Google\\Update\\Clients");
			}
			if (registryKey != null)
			{
				string[] subKeyNames = registryKey.GetSubKeyNames();
				string[] array = subKeyNames;
				foreach (string name in array)
				{
					object value = registryKey.OpenSubKey(name).GetValue("name");
					bool flag = false;
					if (value != null)
					{
						flag = value.ToString().Equals("Google Chrome", StringComparison.InvariantCultureIgnoreCase);
					}
					if (flag)
					{
						return "Chrome v" + registryKey.OpenSubKey(name).GetValue("pv");
					}
				}
			}
			return null;
		}

		public static string Check_Chrome2()
		{
			RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Google\\Chrome\\BLBeacon");
			if (registryKey != null)
			{
				string str = null;
				try
				{
					str = registryKey.GetValue("version", string.Empty).ToString();
				}
				catch
				{
				}
				return "Chrome v" + str;
			}
			return null;
		}

		public static void Lock_Rotacio_Intel()
		{
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Intel\\Display\\igfxcui\\HotKeys", writable: true);
				registryKey.SetValue("Enable", 0, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
		}

		public static string Check_Flash()
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Macromedia\\FlashPlayer\\");
			if (registryKey != null)
			{
				string str = null;
				try
				{
					str = registryKey.GetValue("CurrentVersion", string.Empty).ToString();
				}
				catch
				{
				}
				return "FlashPlayer v" + str;
			}
			return null;
		}

		public static string Check_Flash_ActiveX()
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Macromedia\\FlashPlayerActiveX\\");
			if (registryKey != null)
			{
				string str = null;
				try
				{
					str = registryKey.GetValue("Version", string.Empty).ToString();
				}
				catch
				{
				}
				return "FlashPlayerActiveX v" + str;
			}
			return null;
		}

		public static string Check_Flash_Plugin()
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Macromedia\\FlashPlayerPlugin\\");
			if (registryKey != null)
			{
				string str = null;
				try
				{
					str = registryKey.GetValue("Version", string.Empty).ToString();
				}
				catch
				{
				}
				return "FlashPlayerPlugin v" + str;
			}
			return null;
		}

		public static string Check_Java()
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\JavaSoft\\Java Runtime Environment");
			if (registryKey != null)
			{
				string str = null;
				try
				{
					str = registryKey.GetValue("CurrentVersion", string.Empty).ToString();
				}
				catch
				{
				}
				return "Java Runtime Environment v" + str;
			}
			return null;
		}

		public static string Check_Cleaner()
		{
			RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Piriform\\CCleaner");
			if (registryKey != null)
			{
				string str = null;
				try
				{
					str = registryKey.GetValue("NewVersion", string.Empty).ToString();
				}
				catch
				{
				}
				return "Version " + str;
			}
			return null;
		}

		public static string Check_VNC()
		{
			if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\winvnc.exe") && File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\ultravnc.ini"))
			{
				return "OK";
			}
			return null;
		}

		public static string Check_VS()
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\DevDiv\\VC\\Servicing\\12.0\\RuntimeMinimum");
			if (registryKey != null)
			{
				int num = 0;
				try
				{
					num = Convert.ToInt32(registryKey.GetValue("Install", 0).ToString());
				}
				catch
				{
				}
				if (num == 1)
				{
					return "OK";
				}
			}
			return null;
		}

		private static string CheckFor45DotVersion(int releaseKey)
		{
			if (releaseKey >= 393295)
			{
				return "4.6 or later";
			}
			if (releaseKey >= 379893)
			{
				return "4.5.2 or later";
			}
			return "update";
		}

		public static string Check_NET()
		{
			string a = "update";
			try
			{
				a = CheckFor45DotVersion((int)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\", "Release", 0));
			}
			catch
			{
			}
			if (a == "update")
			{
				return null;
			}
			return "OK";
		}

		private static string Get_NET()
		{
			return CheckFor45DotVersion((int)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\", "Release", 0));
		}

		public static string Check_Freeze()
		{
			return null;
		}

		public static string Check_Tools(string _path, string _files)
		{
			if (!Directory.Exists(_path))
			{
				return null;
			}
			string[] array = _files.Split(',');
			if (array.Length <= 0)
			{
				return "No tools to check";
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (!File.Exists(_path + array[i]))
				{
					return null;
				}
			}
			return "OK";
		}

		public static string HTTP_CopyFile(string _url, string _file)
		{
			WebClient webClient = new WebClient();
			try
			{
				byte[] array = webClient.DownloadData(_url);
				FileStream fileStream = File.Create(_file);
				fileStream.Write(array, 0, array.Length);
				fileStream.Close();
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
			return null;
		}

		public static bool FTP_Download(string _url, string _file)
		{
			CheckPaths("c:\\drivers\\");
			try
			{
				FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(_url);
				ftpWebRequest.Method = "RETR";
				ftpWebRequest.Credentials = new NetworkCredential("install", (string)null);
				ftpWebRequest.UsePassive = true;
				ftpWebRequest.UseBinary = true;
				ftpWebRequest.KeepAlive = false;
				Stream responseStream = ftpWebRequest.GetResponse().GetResponseStream();
				if (File.Exists(_file))
				{
					try
					{
						File.Delete(_file);
					}
					catch (Exception ex)
					{
						MessageBox.Show("FTP DELETE ERROR: " + ex.Message + " [" + _file + "]");
						return false;
					}
				}
				BinaryWriter binaryWriter = new BinaryWriter(File.Open(_file, FileMode.CreateNew));
				int num = 0;
				byte[] array = new byte[1024];
				do
				{
					num = 0;
					try
					{
						num = responseStream.Read(array, 0, array.Length);
					}
					catch
					{
					}
					if (num > 0)
					{
						binaryWriter.Write(array, 0, num);
					}
					Application.DoEvents();
				}
				while (num != 0);
				binaryWriter.Flush();
				binaryWriter.Close();
			}
			catch (Exception ex2)
			{
				MessageBox.Show("FTP ERROR: " + ex2.Message + " [" + _file + "]");
				return false;
			}
			return true;
		}

		public static bool InstallPackage(string _file, string _param)
		{
			CheckPaths("c:\\drivers\\");
			if (!File.Exists("c:\\drivers\\" + _file) && !FTP_Download("ftp://ftp.jogarvip.com/" + _file, "c:\\drivers\\" + _file))
			{
				return false;
			}
			if (File.Exists("c:\\drivers\\" + _file))
			{
				Process process = Process.Start("c:\\drivers\\" + _file, _param);
				Thread.Sleep(1000);
				process.WaitForExit();
				return true;
			}
			return false;
		}

		public static bool RunPackage(string _path, string _file, string _param)
		{
			CheckPaths("c:\\drivers\\");
			if (File.Exists(_path + _file))
			{
				Process process = Process.Start(_path + _file, _param);
				Thread.Sleep(1000);
				process.WaitForExit();
				return true;
			}
			return false;
		}

		public static bool CheckPaths(string _path)
		{
			if (!Directory.Exists(_path))
			{
				try
				{
					Directory.CreateDirectory(_path);
				}
				catch
				{
					return false;
				}
			}
			return true;
		}

		public static bool CopyPackages(string _path, string _files, bool _force)
		{
			CheckPaths("c:\\drivers\\");
			if (!Directory.Exists(_path))
			{
				if (!_force)
				{
					return false;
				}
				try
				{
					Directory.CreateDirectory(_path);
				}
				catch
				{
					return false;
				}
			}
			string[] array = _files.Split(',');
			if (array.Length <= 0)
			{
				return true;
			}
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if ((!File.Exists(_path + array[i]) || _force) && !FTP_Download("ftp://ftp.jogarvip.com/" + array[i], _path + array[i]))
				{
					num = 1;
				}
			}
			if (num == 1)
			{
				return false;
			}
			return true;
		}

		public static string ConfigDesktop(string _path)
		{
			Lock_Rotacio_Intel();
			string result = WindowsLocks(_path);
			UnLock_Windows();
			return result;
		}

		public static string ConfigPower(string _path)
		{
			string str = "";
			Process process = new Process();
			string[] array = new string[14]
			{
				"powercfg.exe",
				"-s 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c",
				"powercfg.exe",
				"-change -monitor-timeout-ac 0",
				"powercfg.exe",
				"-change -disk-timeout-ac 0",
				"powercfg.exe",
				"-change -standby-timeout-ac 0",
				"powercfg.exe",
				"-change -hibernate-timeout-ac 0",
				"powercfg.exe",
				"-setAcValueIndex 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c sub_buttons sButtonAction 0",
				"powercfg.exe",
				"-H OFF"
			};
			for (int i = 0; i < array.Length; i += 2)
			{
				try
				{
					process.StartInfo.WorkingDirectory = _path;
					process.StartInfo.FileName = array[i];
					process.StartInfo.Arguments = array[i + 1];
					process.StartInfo.CreateNoWindow = true;
					process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
					str += $"CONFIG {i / 2 + 1}/{array.Length / 2} '{process.StartInfo.FileName}','{process.StartInfo.Arguments}'\r\n";
					process.Start();
					Thread.Sleep(1000);
					process.WaitForExit();
					str += $"CONFIG {i / 2 + 1}/{array.Length / 2} OK\r\n";
				}
				catch (Exception ex)
				{
					return str + $"ERROR CFG: '{process.StartInfo.FileName}','{process.StartInfo.Arguments}','{ex.Message}'\r\n";
				}
			}
			return null;
		}

		public static string ConfigPowerTablet(string _path)
		{
			return null;
		}

		public static string ConfigKeyboard(string _path)
		{
			return null;
		}

		public static string LockKeyboard(string _path)
		{
			string str = "";
			Process process = new Process();
			string[] array = new string[10]
			{
				"setup_block.exe",
				"/verysilent",
				"kbfmgr",
				"/addset +29,+56,+83",
				"kbfmgr",
				"/addset +56,+15",
				"kbfmgr",
				"/addset +56,+1",
				"kbfmgr",
				"/addset +14,+29,+56,+83"
			};
			for (int i = 0; i < array.Length; i += 2)
			{
				try
				{
					process.StartInfo.WorkingDirectory = _path;
					process.StartInfo.FileName = array[i];
					process.StartInfo.Arguments = array[i + 1];
					process.StartInfo.CreateNoWindow = true;
					process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
					str += $"CONFIG {i / 2 + 1}/{array.Length / 2} '{process.StartInfo.FileName}','{process.StartInfo.Arguments}'\r\n";
					process.Start();
					Thread.Sleep(1000);
					process.WaitForExit();
					str += $"CONFIG {i / 2 + 1}/{array.Length / 2} OK\r\n";
				}
				catch (Exception ex)
				{
					return str + $"ERROR CFG: '{process.StartInfo.FileName}','{process.StartInfo.Arguments}','{ex.Message}'\r\n";
				}
			}
			return null;
		}

		public static string WindowsLocks(string _path)
		{
			string str = "";
			Process process = new Process();
			string[] array = new string[10]
			{
				"no_winkey.msi",
				"/quiet",
				"no_usb.msi",
				"/quiet",
				"no_firewall.msi",
				"/quiet",
				"no_cad.msi",
				"/quiet",
				"regedit.exe",
				"/S " + _path + "data3.zip"
			};
			for (int i = 0; i < array.Length; i += 2)
			{
				try
				{
					process.StartInfo.WorkingDirectory = _path;
					process.StartInfo.FileName = array[i];
					process.StartInfo.Arguments = array[i + 1];
					process.StartInfo.CreateNoWindow = true;
					process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
					str += $"CONFIG {i / 2 + 1}/{array.Length / 2} '{process.StartInfo.FileName}','{process.StartInfo.Arguments}'\r\n";
					process.Start();
					Thread.Sleep(1000);
					process.WaitForExit();
					str += $"CONFIG {i / 2 + 1}/{array.Length / 2} OK\r\n";
				}
				catch (Exception ex)
				{
					return str + $"ERROR CFG: '{process.StartInfo.FileName}','{process.StartInfo.Arguments}','{ex.Message}'\r\n";
				}
			}
			try
			{
				string name = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer";
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name, writable: true);
				registryKey.SetValue("NoControlPanel", 0, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				string name2 = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
				RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey(name2, writable: true);
				registryKey2.SetValue("DisableRegistryTools", 0, RegistryValueKind.DWord);
				registryKey2.Close();
			}
			catch
			{
			}
			try
			{
				string name3 = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\AutoRotation";
				RegistryKey registryKey3 = Registry.LocalMachine.OpenSubKey(name3, writable: true);
				registryKey3.SetValue("Enable", 0, RegistryValueKind.DWord);
				registryKey3.Close();
			}
			catch
			{
			}
			return null;
		}

		public static string ConfigBoot(string _windows, string _path)
		{
			string str = "";
			try
			{
				if (!Directory.Exists(_windows + "system32\\oobe\\info"))
				{
					Directory.CreateDirectory(_windows + "system32\\oobe\\info");
				}
				str += $"COPY 1/3 OK\r\n";
			}
			catch (Exception)
			{
				return str + $"COPY 1/3 ERROR\r\n";
			}
			try
			{
				if (!Directory.Exists(_windows + "system32\\oobe\\info\\backgrounds"))
				{
					Directory.CreateDirectory(_windows + "system32\\oobe\\info\\backgrounds");
				}
				str += $"COPY 2/3 OK\r\n";
			}
			catch (Exception)
			{
				return str + $"COPY 2/3 ERROR\r\n";
			}
			try
			{
				if (File.Exists(_path + "fondos\\backgroundDefault.jpg"))
				{
					File.Copy(_path + "fondos\\backgroundDefault.jpg", _windows + "system32\\oobe\\info\\backgrounds\\backgroundDefault.jpg");
				}
				str += $"COPY 3/3 OK\r\n";
			}
			catch (Exception)
			{
				return str + $"COPY 3/3 ERROR\r\n";
			}
			Process process = new Process();
			string text = "";
			try
			{
				text = "/set {default} quietboot true";
				process.StartInfo.WorkingDirectory = "";
				process.StartInfo.FileName = "bcdedit.exe";
				process.StartInfo.Arguments = text;
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				process.Start();
				Thread.Sleep(1000);
				process.WaitForExit();
				str += $"BOOT 1/3 OK\r\n";
			}
			catch (Exception ex4)
			{
				return str + $"ERROR BOOT: {text},{ex4.Message}\r\n";
			}
			try
			{
				text = "/set {default} bootstatuspolicy ignoreallfailures";
				process.StartInfo.WorkingDirectory = "";
				process.StartInfo.FileName = "bcdedit.exe";
				process.StartInfo.Arguments = text;
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				process.Start();
				Thread.Sleep(1000);
				process.WaitForExit();
				str += $"BOOT 2/3 OK\r\n";
			}
			catch (Exception ex5)
			{
				return str + $"ERROR BOOT: {text},{ex5.Message}\r\n";
			}
			try
			{
				text = "/set {default} advancedoptions false";
				process.StartInfo.WorkingDirectory = "";
				process.StartInfo.FileName = "bcdedit.exe";
				process.StartInfo.Arguments = text;
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				process.Start();
				Thread.Sleep(1000);
				process.WaitForExit();
				str += $"BOOT 3/3 OK\r\n";
			}
			catch (Exception ex6)
			{
				return str + $"ERROR BOOT: {text},{ex6.Message}\r\n";
			}
			return null;
		}

		public static string ConfigBootTablet(string _windows, string _path)
		{
			string str = "";
			try
			{
				if (!Directory.Exists(_windows + "system32\\oobe\\info"))
				{
					Directory.CreateDirectory(_windows + "system32\\oobe\\info");
				}
				str += $"COPY 1/3 OK\r\n";
			}
			catch (Exception)
			{
				return str + $"COPY 1/3 ERROR\r\n";
			}
			try
			{
				if (!Directory.Exists(_windows + "system32\\oobe\\info\\backgrounds"))
				{
					Directory.CreateDirectory(_windows + "system32\\oobe\\info\\backgrounds");
				}
				str += $"COPY 2/3 OK\r\n";
			}
			catch (Exception)
			{
				return str + $"COPY 2/3 ERROR\r\n";
			}
			try
			{
				if (File.Exists(_path + "fondos\\backgroundDefault.jpg"))
				{
					File.Copy(_path + "fondos\\backgroundDefault.jpg", _windows + "system32\\oobe\\info\\backgrounds\\backgroundDefault.jpg");
				}
				str += $"COPY 3/3 OK\r\n";
			}
			catch (Exception)
			{
				return str + $"COPY 3/3 ERROR\r\n";
			}
			Process process = new Process();
			string text = "";
			try
			{
				text = "/set {default} quietboot true";
				process.StartInfo.WorkingDirectory = "";
				process.StartInfo.FileName = "bcdedit.exe";
				process.StartInfo.Arguments = text;
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				process.Start();
				Thread.Sleep(1000);
				process.WaitForExit();
				str += $"BOOT 1/3 OK\r\n";
			}
			catch (Exception ex4)
			{
				return str + $"ERROR BOOT: {text},{ex4.Message}\r\n";
			}
			try
			{
				text = "/set {default} bootstatuspolicy ignoreallfailures";
				process.StartInfo.WorkingDirectory = "";
				process.StartInfo.FileName = "bcdedit.exe";
				process.StartInfo.Arguments = text;
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				process.Start();
				Thread.Sleep(1000);
				process.WaitForExit();
				str += $"BOOT 2/3 OK\r\n";
			}
			catch (Exception ex5)
			{
				return str + $"ERROR BOOT: {text},{ex5.Message}\r\n";
			}
			try
			{
				text = "/set {default} advancedoptions false";
				process.StartInfo.WorkingDirectory = "";
				process.StartInfo.FileName = "bcdedit.exe";
				process.StartInfo.Arguments = text;
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				process.Start();
				Thread.Sleep(1000);
				process.WaitForExit();
				str += $"BOOT 3/3 OK\r\n";
			}
			catch (Exception ex6)
			{
				return str + $"ERROR BOOT: {text},{ex6.Message}\r\n";
			}
			return null;
		}

		public static string Get_CPU()
		{
			string arg = string.Empty;
			string arg2 = string.Empty;
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT Name FROM Win32_processor");
			using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = managementObjectSearcher.Get().GetEnumerator())
			{
				if (managementObjectEnumerator.MoveNext())
				{
					ManagementObject managementObject = (ManagementObject)managementObjectEnumerator.Current;
					arg = managementObject["Name"].ToString();
				}
			}
			managementObjectSearcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_processor");
			using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator2 = managementObjectSearcher.Get().GetEnumerator())
			{
				if (managementObjectEnumerator2.MoveNext())
				{
					ManagementObject managementObject2 = (ManagementObject)managementObjectEnumerator2.Current;
					arg2 = managementObject2["ProcessorId"].ToString();
				}
			}
			return $"{arg} ID {arg2}";
		}

		public static string Get_Workgroup()
		{
			ManagementObject managementObject = new ManagementObject($"Win32_ComputerSystem.Name='{Environment.MachineName}'");
			object obj = managementObject["Workgroup"];
			return obj.ToString();
		}

		public static string Get_NetMAC()
		{
			string result = "Not enabled or present";
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			for (int i = 0; i < allNetworkInterfaces.Length; i++)
			{
				if (allNetworkInterfaces[i].OperationalStatus == OperationalStatus.Up && allNetworkInterfaces[i].NetworkInterfaceType == NetworkInterfaceType.Ethernet)
				{
					result = allNetworkInterfaces[i].Description + " MAC " + allNetworkInterfaces[i].GetPhysicalAddress();
					break;
				}
			}
			return result;
		}

		public static string Get_WifiMAC()
		{
			string result = "Not enabled or present";
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			for (int i = 0; i < allNetworkInterfaces.Length; i++)
			{
				if (allNetworkInterfaces[i].OperationalStatus == OperationalStatus.Up && allNetworkInterfaces[i].NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
				{
					result = allNetworkInterfaces[i].Description + " MAC " + allNetworkInterfaces[i].GetPhysicalAddress();
					break;
				}
			}
			return result;
		}

		public static string Get_Windows()
		{
			string arg = string.Empty;
			string arg2 = string.Empty;
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
			using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = managementObjectSearcher.Get().GetEnumerator())
			{
				if (managementObjectEnumerator.MoveNext())
				{
					ManagementObject managementObject = (ManagementObject)managementObjectEnumerator.Current;
					arg = managementObject["Caption"].ToString();
				}
			}
			managementObjectSearcher = new ManagementObjectSearcher("SELECT OSArchitecture FROM Win32_OperatingSystem");
			using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator2 = managementObjectSearcher.Get().GetEnumerator())
			{
				if (managementObjectEnumerator2.MoveNext())
				{
					ManagementObject managementObject2 = (ManagementObject)managementObjectEnumerator2.Current;
					arg2 = managementObject2["OSArchitecture"].ToString();
				}
			}
			return $"{arg} {arg2}";
		}

		public static string Get_ComputerName()
		{
			return Environment.MachineName;
		}

		public static string Get_RAM()
		{
			string queryString = "SELECT Capacity FROM Win32_PhysicalMemory";
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(queryString);
			ulong num = 0uL;
			foreach (ManagementObject item in managementObjectSearcher.Get())
			{
				num += Convert.ToUInt64(item.Properties["Capacity"].Value);
			}
			ulong num2 = num / 1073741824uL;
			if (num2 != 0)
			{
				return $"{num2} Giga Bytes";
			}
			num2 = num / 1048576uL;
			if (num2 != 0)
			{
				return $"{num2} Mega Bytes";
			}
			return $"{num} Bytes";
		}

		public static string Get_Video()
		{
			string queryString = "SELECT Description FROM Win32_VideoController";
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(queryString);
			string text = "";
			foreach (ManagementObject item in managementObjectSearcher.Get())
			{
				if (text != "")
				{
					text += ", ";
				}
				text += Convert.ToString(item.Properties["Description"].Value);
			}
			return text;
		}

		public static string Get_Serials()
		{
			string[] portNames = SerialPort.GetPortNames();
			string text = "";
			if (portNames != null)
			{
				for (int i = 0; i < portNames.Length; i++)
				{
					if (text != "")
					{
						text += ", ";
					}
					text += portNames[i];
				}
			}
			return text;
		}

		public static string Get_IP()
		{
			string text = "";
			bool dhcp = false;
			GetIP(out string[] _, out string[] ipAdresses, out string[] subnets, out string[] gateways, out string[] _, out dhcp);
			if (dhcp)
			{
				text = "Dynamic ";
			}
			if (ipAdresses.Length > 0)
			{
				text = text + " Ip " + ipAdresses[0];
			}
			if (subnets.Length > 0)
			{
				text = text + " Mask " + subnets[0];
			}
			if (gateways.Length > 0)
			{
				text = text + " Gateway " + gateways[0];
			}
			return text;
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

		public static bool CreateShortcut(string _name, string _app, string _icon)
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			using (StreamWriter streamWriter = new StreamWriter(folderPath + "\\" + _name + ".url"))
			{
				streamWriter.WriteLine("[InternetShortcut]");
				_app = _app.Replace('\\', '/');
				streamWriter.WriteLine("URL=file:///" + _app);
				streamWriter.WriteLine("IconIndex=0");
				_icon = _icon.Replace('\\', '/');
				streamWriter.WriteLine("IconFile=" + _icon);
				streamWriter.Flush();
			}
			return true;
		}

		public static bool Internet_Test()
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

		public static bool Ping_IP(string _ip)
		{
			Ping ping = new Ping();
			PingReply pingReply = null;
			try
			{
				pingReply = ping.Send(IPAddress.Parse(_ip));
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

		public static string ConfigNTP()
		{
			string str = "";
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
			for (int i = 0; i < array.Length; i += 2)
			{
				try
				{
					process.StartInfo.FileName = array[i];
					process.StartInfo.Arguments = array[i + 1];
					process.StartInfo.CreateNoWindow = true;
					process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
					str += $"CONFIG {i / 2 + 1}/{array.Length / 2} '{process.StartInfo.FileName}','{process.StartInfo.Arguments}'\r\n";
					process.Start();
					Thread.Sleep(1000);
					process.WaitForExit();
					str += $"CONFIG {i / 2 + 1}/{array.Length / 2} OK\r\n";
				}
				catch (Exception ex)
				{
					return str + $"ERROR CFG: '{process.StartInfo.FileName}','{process.StartInfo.Arguments}','{ex.Message}'\r\n";
				}
			}
			return null;
		}

		public static bool VNC_Running()
		{
			string text = "winvnc";
			Process[] processes = Process.GetProcesses();
			foreach (Process process in processes)
			{
				if (process.ProcessName.ToLower().Contains(text.ToLower()))
				{
					return true;
				}
			}
			return false;
		}
	}
}
