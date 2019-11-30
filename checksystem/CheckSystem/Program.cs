using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace CheckSystem
{
	internal static class Program
	{
		public const string PathTools = "c:\\drivers\\";

		public const string PathWindows = "c:\\windows\\";

		public const string PathKiosk = "c:\\kiosk\\";

		public const string FTPInstall = "ftp://ftp.jogarvip.com/";

		private const string Flash_V_ActiveX = "23.0.0.185";

		private const string Flash_V_Plugin = "23.0.0.185";

		private const string Java_V = "1.8.0.112";

		[STAThread]
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

		public static bool FTP_Download(string _url, string _file, ref MessageWait w)
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
				if (File.Exists("c:\\drivers\\" + _file))
				{
					try
					{
						File.Delete("c:\\drivers\\" + _file);
					}
					catch (Exception)
					{
						return false;
					}
				}
				BinaryWriter binaryWriter = new BinaryWriter(File.Open("c:\\drivers\\" + _file, FileMode.CreateNew));
				int num = 0;
				int num2 = 0;
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
						num2 += num;
						binaryWriter.Write(array, 0, num);
						w.UpdateInfo(num2);
					}
					Application.DoEvents();
				}
				while (num != 0);
				binaryWriter.Flush();
				binaryWriter.Close();
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public static bool Run(string _file, string _param)
		{
			CheckPaths("c:\\drivers\\");
			if (File.Exists("c:\\drivers\\" + _file))
			{
				Process process = Process.Start("c:\\drivers\\" + _file, _param);
				for (int i = 0; i < 10; i++)
				{
					Application.DoEvents();
					Thread.Sleep(200);
				}
				process.WaitForExit();
				return true;
			}
			return false;
		}

		public static bool Run_Cmd(string _param)
		{
			Process process = new Process();
			process.StartInfo.FileName = "cmd.exe";
			process.StartInfo.Arguments = "/C \"" + _param + "\"";
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.Start();
			return true;
		}

		private static bool Check_VC2013()
		{
			string value = null;
			try
			{
				value = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Classes\\Installer\\Dependencies\\{f65db027-aff3-4070-886a-0d87064aabb1}", "Version", 0);
			}
			catch
			{
			}
			if (string.IsNullOrEmpty(value))
			{
				return false;
			}
			return true;
		}

		private static bool Check_NET()
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
				return false;
			}
			return true;
		}

		private static string Get_NET()
		{
			return CheckFor45DotVersion((int)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\", "Release", 0));
		}

		public static bool Check_Flash(string _flash, string _okflash)
		{
			string[] array = _flash.Split('.');
			string[] array2 = _okflash.Split('.');
			int num = int.Parse(array[0]);
			int num2 = int.Parse(array2[0]);
			if (num < num2)
			{
				return false;
			}
			num = int.Parse(array[1]);
			num2 = int.Parse(array2[1]);
			if (num < num2)
			{
				return false;
			}
			num = int.Parse(array[2]);
			num2 = int.Parse(array2[2]);
			if (num < num2)
			{
				return false;
			}
			num = int.Parse(array[3]);
			num2 = int.Parse(array2[3]);
			if (num < num2)
			{
				return false;
			}
			return true;
		}

		public static bool Check_Flash_ActiveX(out string _flash)
		{
			_flash = "";
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Macromedia\\FlashPlayerActiveX\\");
			if (registryKey != null)
			{
				string text = null;
				try
				{
					text = registryKey.GetValue("Version", string.Empty).ToString();
				}
				catch
				{
				}
				_flash = text;
				if (!Check_Flash(_flash, "23.0.0.185"))
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public static bool Check_Flash_Plugin(out string _flash)
		{
			_flash = "";
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Macromedia\\FlashPlayerPlugin\\");
			if (registryKey != null)
			{
				string text = null;
				try
				{
					text = registryKey.GetValue("Version", string.Empty).ToString();
				}
				catch
				{
				}
				_flash = text;
				if (!Check_Flash(_flash, "23.0.0.185"))
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public static bool Check_Java64(out string _java)
		{
			_java = "";
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\JavaSoft\\Java Runtime Environment");
			if (registryKey != null)
			{
				string text = null;
				try
				{
					text = registryKey.GetValue("CurrentVersion", string.Empty).ToString();
				}
				catch
				{
				}
				_java = text;
				return Check_Java_Home64(out _java);
			}
			return false;
		}

		public static bool Check_Java(out string _java)
		{
			_java = "";
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\JavaSoft\\Java Runtime Environment");
			if (registryKey != null)
			{
				string text = null;
				try
				{
					text = registryKey.GetValue("CurrentVersion", string.Empty).ToString();
				}
				catch
				{
				}
				_java = text;
				return Check_Java_Home(out _java);
			}
			return Check_Java64(out _java);
		}

		public static bool Check_Java_Home(out string _java)
		{
			_java = "";
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\JavaSoft\\Java Runtime Environment\\1.8");
			if (registryKey != null)
			{
				string text = null;
				try
				{
					text = registryKey.GetValue("JavaHome", string.Empty).ToString();
				}
				catch
				{
				}
				int num = text.IndexOf("\\jre1.8");
				_java = text.Remove(0, num + "\\jre".Length);
				_java = _java.Replace('_', '.');
				if (!Check_Flash(_java, "1.8.0.112"))
				{
					return false;
				}
				return true;
			}
			return Check_Java64(out _java);
		}

		public static bool Check_Java_Home64(out string _java)
		{
			_java = "";
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\JavaSoft\\Java Runtime Environment\\1.8");
			if (registryKey != null)
			{
				string text = null;
				try
				{
					text = registryKey.GetValue("JavaHome", string.Empty).ToString();
				}
				catch
				{
				}
				int num = text.IndexOf("\\jre1.8");
				_java = text.Remove(0, num + "\\jre".Length);
				_java = _java.Replace('_', '.');
				if (!Check_Flash(_java, "1.8.0.112"))
				{
					return false;
				}
				return true;
			}
			return Check_Java64(out _java);
		}

		public static bool Install_FlashX()
		{
			if (!Run("flash_activex.exe", "-install"))
			{
				return true;
			}
			return false;
		}

		public static bool Install_Flash()
		{
			if (!Run("flash_plugin.exe", "-install"))
			{
				return true;
			}
			return false;
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
				try
				{
					text += Convert.ToString(item.Properties["Description"].Value);
				}
				catch
				{
				}
			}
			return text;
		}

		public static string Check_VNC()
		{
			if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\winvnc.exe") && File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\ultravnc.ini"))
			{
				return "OK";
			}
			return null;
		}

		public static string Get_Video_Bad_Driver()
		{
			string queryString = "SELECT InfFilename,InfSection FROM Win32_VideoController";
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(queryString);
			string result = "";
			foreach (ManagementObject item in managementObjectSearcher.Get())
			{
				string text = "";
				string text2 = "";
				try
				{
					text = Convert.ToString(item.Properties["InfFilename"].Value);
				}
				catch
				{
				}
				try
				{
					text2 = Convert.ToString(item.Properties["InfSection"].Value);
				}
				catch
				{
				}
				if (text.ToLower() == "display.inf".ToLower() && text2.ToLower() == "vga".ToLower())
				{
					result = "Standard VGA";
				}
			}
			return result;
		}

		private static void Main()
		{
			int num = 0;
			MessageWait w = new MessageWait("Checking your system");
			w.UpdateInfo(-2);
			w.Show();
			for (int i = 0; i < 10; i++)
			{
				Application.DoEvents();
				Thread.Sleep(200);
			}
			w.UpdateMSG("Checking system Date/Time");
			for (int j = 0; j < 10; j++)
			{
				Application.DoEvents();
				Thread.Sleep(200);
			}
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
				for (int k = 0; k < array.Length; k += 2)
				{
					try
					{
						process.StartInfo.FileName = array[k];
						process.StartInfo.Arguments = array[k + 1];
						process.StartInfo.CreateNoWindow = true;
						process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
						process.Start();
						Thread.Sleep(1000);
						process.WaitForExit();
					}
					catch (Exception)
					{
						num++;
						w.UpdateMSG("Can not update Date/Time ");
						for (int l = 0; l < 10; l++)
						{
							Application.DoEvents();
							Thread.Sleep(200);
						}
					}
				}
				now = DateTime.Now;
				num2++;
			}
			while (now.Year < 2016 && num2 < 4);
			if (now.Year < 2016)
			{
				Run_Cmd("date 28-10-2016");
				if (DateTime.Now.Year < 2016)
				{
					Run_Cmd("date 28-10-2016");
					if (DateTime.Now.Year < 2016)
					{
						Run_Cmd("date 10/28/2016");
						if (DateTime.Now.Year < 2016)
						{
							Run_Cmd("date 10/28/2016");
							if (DateTime.Now.Year < 2016)
							{
								num++;
							}
						}
					}
				}
			}
			if (num == 0)
			{
				w.UpdateMSG("Date/Time updated");
				for (int m = 0; m < 10; m++)
				{
					Application.DoEvents();
					Thread.Sleep(200);
				}
			}
			else
			{
				w.UpdateMSG("Is not possible update Date/Time");
				for (int n = 0; n < 10; n++)
				{
					Application.DoEvents();
					Thread.Sleep(200);
				}
			}
			w.UpdateMSG("Checking .NET");
			w.UpdateInfo(-2);
			for (int num3 = 0; num3 < 10; num3++)
			{
				Application.DoEvents();
				Thread.Sleep(200);
			}
			if (!Check_NET())
			{
				w.UpdateMSG("Updating .NET Framework");
				FTP_Download("ftp://ftp.jogarvip.com/net61.exe", "net61.exe", ref w);
				w.UpdateInfo(-1);
				if (!Run("net61.exe", "/qb /passive /norestart"))
				{
					w.UpdateMSG(".NET Framework not installed");
					w.UpdateInfo(-2);
					for (int num4 = 0; num4 < 10; num4++)
					{
						Application.DoEvents();
						Thread.Sleep(200);
					}
				}
			}
			else
			{
				w.UpdateMSG(".NET Framework " + Get_NET());
				w.UpdateInfo(-2);
				for (int num5 = 0; num5 < 10; num5++)
				{
					Application.DoEvents();
					Thread.Sleep(200);
				}
			}
			w.UpdateMSG("Checking Flash Plugin");
			w.UpdateInfo(-2);
			for (int num6 = 0; num6 < 10; num6++)
			{
				Application.DoEvents();
				Thread.Sleep(200);
			}
			if (!Check_Flash_Plugin(out string _flash))
			{
				w.UpdateMSG("Updating Flash Plugin");
				FTP_Download("ftp://ftp.jogarvip.com/flash_plugin.exe", "flash_plugin.exe", ref w);
				w.UpdateInfo(-1);
				if (!Run("flash_plugin.exe", "-install"))
				{
					w.UpdateMSG("Flash Plugin not installed");
					w.UpdateInfo(-2);
					for (int num7 = 0; num7 < 10; num7++)
					{
						Application.DoEvents();
						Thread.Sleep(200);
					}
				}
			}
			else
			{
				w.UpdateMSG("Flash Plugin v" + _flash);
				w.UpdateInfo(-2);
				for (int num8 = 0; num8 < 10; num8++)
				{
					Application.DoEvents();
					Thread.Sleep(200);
				}
			}
			w.UpdateMSG("Checking Runtimes");
			w.UpdateInfo(-2);
			for (int num9 = 0; num9 < 10; num9++)
			{
				Application.DoEvents();
				Thread.Sleep(200);
			}
			if (!Check_VC2013())
			{
				w.UpdateMSG("Updating Runtimes");
				FTP_Download("ftp://ftp.jogarvip.com/vc2013.exe", "vc2013.exe", ref w);
				w.UpdateInfo(-1);
				Run("vc2013.exe", "/install /quiet /norestart");
			}
			w.UpdateMSG("Checking Tools");
			w.UpdateInfo(-2);
			for (int num10 = 0; num10 < 10; num10++)
			{
				Application.DoEvents();
				Thread.Sleep(200);
			}
			if (Check_VNC() != "OK")
			{
				w.UpdateMSG("Updating Tools");
				FTP_Download("ftp://ftp.jogarvip.com/vnc_install.exe", "vnc_install.exe", ref w);
				FTP_Download("ftp://ftp.jogarvip.com/data1.zip", "data1.zip", ref w);
				w.UpdateInfo(-1);
				Run("vnc_install.exe", "/verysilent /norestart");
				if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\ultravnc.ini"))
				{
					try
					{
						File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\ultravnc.ini");
					}
					catch
					{
					}
				}
				try
				{
					File.Copy("c:\\drivers\\data1.zip", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\ultravnc.ini");
				}
				catch
				{
				}
			}
			else
			{
				w.UpdateMSG("Tools installed");
				w.UpdateInfo(-2);
				for (int num11 = 0; num11 < 10; num11++)
				{
					Application.DoEvents();
					Thread.Sleep(200);
				}
			}
			w.UpdateMSG("Checking VGA Drivers");
			w.UpdateInfo(-2);
			for (int num12 = 0; num12 < 10; num12++)
			{
				Application.DoEvents();
				Thread.Sleep(200);
			}
			num = 0;
			if (Get_Video_Bad_Driver() != "")
			{
				num = 1;
				w.UpdateMSG("ATTENTION!\nIncorrect video driver installed\nCause poor performance\nContact with technical service to fix.");
				w.Hide();
				w.UpdateInfo(-3);
				w.ShowDialog();
			}
			if (!Check_VC2013())
			{
				w.UpdateMSG("ERROR: Is not possible update, RUNTIMES error, please contact assistance.");
				w.Show();
				for (int num13 = 0; num13 < 10; num13++)
				{
					Application.DoEvents();
					Thread.Sleep(1000);
				}
				num++;
			}
			if (!Check_NET())
			{
				num++;
				w.UpdateMSG("ERROR: Is not possible update, .NET error, please contact assistance.");
				w.Show();
				for (int num14 = 0; num14 < 10; num14++)
				{
					Application.DoEvents();
					Thread.Sleep(1000);
				}
			}
			if (num == 0)
			{
				w.UpdateMSG("System is ready");
				w.UpdateInfo(-2);
				for (int num15 = 0; num15 < 10; num15++)
				{
					Application.DoEvents();
					Thread.Sleep(200);
				}
				w.Close();
				Environment.Exit(0);
			}
			w.Close();
			Environment.Exit(1);
		}
	}
}
