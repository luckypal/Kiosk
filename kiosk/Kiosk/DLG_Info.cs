using Kiosk.Properties;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Management;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace Kiosk
{
	public class DLG_Info : Form
	{
		public bool OK;

		public bool IsClosed;

		public Configuracion opciones;

		private IContainer components = null;

		private Panel pBOTTOM;

		private Button bOk;

		private TextBox tNetBIOS;

		private Label lNetBIOS;

		private TextBox tComputer;

		private Label lComputer;

		private GroupBox gDNS;

		private TextBox tDNS2;

		private TextBox tDNS1;

		private Label lDNS2;

		private Label lDNS1;

		private RadioButton rStaticDNS;

		private RadioButton rAutoDNS;

		private GroupBox gTCP;

		private TextBox tGateway;

		private TextBox tMask;

		private TextBox tIP;

		private Label lGateway;

		private Label lMask;

		private Label lIP;

		private RadioButton rStaticDHCP;

		private RadioButton rAutoDHCP;

		private Label lServer;

		private TextBox iServer;

		private TextBox iUser;

		private Label lUser;

		private TextBox iSelector;

		private Label lSelector;

		private TextBox iValidator;

		private Label lValidator;

		private TextBox iPrinter;

		private Label lPrinter;

		private TextBox iEthernet;

		private Label lEthernet;

		private TextBox iCPU;

		private Label lCPU;

		private TextBox iBarcode;

		private Label lBarcode;

		private Label lSystem;

		private TextBox iSystem;

		private TextBox iWifi;

		private Label lWifi;

		private TextBox iVNC;

		private Label lVNC;

		public DLG_Info(ref Configuracion _opc)
		{
			OK = false;
			IsClosed = false;
			opciones = _opc;
			InitializeComponent();
			Localize();
			bool dhcp = false;
			tComputer.Text = Environment.MachineName;
			tNetBIOS.Text = GetWorkGroup();
			GetIP(out string[] enableDHCP, out string[] ipAdresses, out string[] subnets, out string[] gateways, out string[] dnses, out dhcp);
			if (dhcp && enableDHCP != null)
			{
				rAutoDHCP.Checked = true;
				rStaticDHCP.Checked = false;
			}
			else
			{
				rAutoDHCP.Checked = false;
				rStaticDHCP.Checked = true;
				if (ipAdresses.Length > 0)
				{
					tIP.Text = ipAdresses[0];
				}
				if (subnets.Length > 0)
				{
					tMask.Text = subnets[0];
				}
				if (gateways.Length > 0)
				{
					tGateway.Text = gateways[0];
				}
			}
			if (dhcp && enableDHCP != null)
			{
				rAutoDNS.Checked = true;
				rStaticDNS.Checked = false;
			}
			else
			{
				rAutoDNS.Checked = false;
				rStaticDNS.Checked = true;
				if (dnses.Length > 0)
				{
					tDNS1.Text = dnses[0];
				}
				if (dnses.Length > 1)
				{
					tDNS2.Text = dnses[1];
				}
			}
			iServer.Text = opciones.Srv_Ip + "," + opciones.Srv_port;
			iUser.Text = opciones.Srv_User;
			if (opciones.Dev_Coin == "-")
			{
				iSelector.Text = "DISABLED";
			}
			else
			{
				iSelector.Text = opciones.Dev_Coin + " Port: " + opciones.Dev_Coin_P;
			}
			if (opciones.Dev_BNV == "-")
			{
				iValidator.Text = "DISABLED";
			}
			else
			{
				iValidator.Text = opciones.Dev_BNV + " Port: " + opciones.Dev_BNV_P;
			}
			if (string.IsNullOrEmpty(opciones.Impresora_Tck))
			{
				iPrinter.Text = "DISABLED";
			}
			else
			{
				iPrinter.Text = opciones.Impresora_Tck;
			}
			if (string.IsNullOrEmpty(opciones.Barcode))
			{
				iBarcode.Text = "DISABLED";
			}
			else
			{
				iBarcode.Text = opciones.Barcode;
			}
			iVNC.Text = opciones.Server_VNC;
			iCPU.Text = opciones.CPUID;
			iEthernet.Text = "";
			iWifi.Text = "";
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			for (int i = 0; i < allNetworkInterfaces.Length; i++)
			{
				if (allNetworkInterfaces[i].NetworkInterfaceType == NetworkInterfaceType.Ethernet)
				{
					if (allNetworkInterfaces[i].OperationalStatus == OperationalStatus.Up)
					{
						iEthernet.Text = allNetworkInterfaces[i].Description + "\r\nCONNECTED MAC " + allNetworkInterfaces[i].GetPhysicalAddress();
					}
					else
					{
						iEthernet.Text = allNetworkInterfaces[i].Description + "\r\nDISCONNECTED MAC " + allNetworkInterfaces[i].GetPhysicalAddress();
					}
				}
				if (!allNetworkInterfaces[i].Description.ToLower().Contains("virtual") && allNetworkInterfaces[i].NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
				{
					if (allNetworkInterfaces[i].OperationalStatus == OperationalStatus.Up)
					{
						iWifi.Text = allNetworkInterfaces[i].Description + "\r\nCONNECTED MAC " + allNetworkInterfaces[i].GetPhysicalAddress();
					}
					else
					{
						iWifi.Text = allNetworkInterfaces[i].Description + "\r\nDISCONNECTED MAC " + allNetworkInterfaces[i].GetPhysicalAddress();
					}
				}
			}
			iSystem.Text = "Cpu: " + Get_CPU() + "\r\nRAM: " + Get_RAM() + "\r\nVideo: " + Get_Video() + "\r\nSerials: " + Get_Serials() + "\r\nFlash: " + Check_Flash() + " / " + Check_Flash_ActiveX() + " / " + Check_Flash_Plugin() + "\r\nJava: " + Check_Java() + "\r\nVNC: " + Check_VNC() + "\r\nCleaner: " + Check_Cleaner() + "\r\n";
		}

		private void Localize()
		{
			SuspendLayout();
			ResumeLayout();
		}

		public static string GetWorkGroup()
		{
			ManagementObject managementObject = new ManagementObject($"Win32_ComputerSystem.Name='{Environment.MachineName}'");
			object obj = managementObject["Workgroup"];
			return obj.ToString();
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

		public static void SetIP(string IpAddresses, string SubnetMask, string Gateway, string DnsSearchOrder)
		{
			ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
			ManagementObjectCollection instances = managementClass.GetInstances();
			foreach (ManagementObject item in instances)
			{
				if ((bool)item["IPEnabled"])
				{
					ManagementBaseObject methodParameters = item.GetMethodParameters("EnableStatic");
					ManagementBaseObject methodParameters2 = item.GetMethodParameters("SetGateways");
					ManagementBaseObject methodParameters3 = item.GetMethodParameters("SetDNSServerSearchOrder");
					methodParameters2["DefaultIPGateway"] = new string[1]
					{
						Gateway
					};
					methodParameters2["GatewayCostMetric"] = new int[1]
					{
						1
					};
					methodParameters["IPAddress"] = IpAddresses.Split(',');
					methodParameters["SubnetMask"] = new string[1]
					{
						SubnetMask
					};
					methodParameters3["DNSServerSearchOrder"] = DnsSearchOrder.Split(',');
					ManagementBaseObject managementBaseObject = item.InvokeMethod("EnableStatic", methodParameters, null);
					ManagementBaseObject managementBaseObject2 = item.InvokeMethod("SetGateways", methodParameters2, null);
					ManagementBaseObject managementBaseObject3 = item.InvokeMethod("SetDNSServerSearchOrder", methodParameters3, null);
					break;
				}
			}
		}

		public static void SetDHCP()
		{
			ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
			ManagementObjectCollection instances = managementClass.GetInstances();
			foreach (ManagementObject item in instances)
			{
				if ((bool)item["IPEnabled"])
				{
					ManagementBaseObject methodParameters = item.GetMethodParameters("SetDNSServerSearchOrder");
					methodParameters["DNSServerSearchOrder"] = null;
					ManagementBaseObject managementBaseObject = item.InvokeMethod("EnableDHCP", null, null);
					ManagementBaseObject managementBaseObject2 = item.InvokeMethod("SetDNSServerSearchOrder", methodParameters, null);
				}
			}
		}

		private void NewShell(string exe)
		{
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon", writable: true);
				registryKey.SetValue("Shell", exe, RegistryValueKind.String);
				registryKey.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error al modificar el programa de arranque : " + ex.Message);
			}
		}

		private void DisableDesktop(int o)
		{
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", writable: true);
				registryKey.SetValue("NoDesktop", o, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error al configurar el Desktop : " + ex.Message);
			}
		}

		public static void DisableTaskManager(int o)
		{
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", writable: true);
				registryKey.SetValue("DisableTaskMgr", o, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error al configurar el TaskManager : " + ex.Message);
			}
		}

		private void DisableRegedit(int o)
		{
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", writable: true);
				registryKey.SetValue("DisableRegistryTools", o, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch (Exception ex)
			{
				try
				{
					using (RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", writable: true))
					{
						registryKey2.SetValue("DisableRegistryTools", o);
						registryKey2.Close();
					}
				}
				catch
				{
					MessageBox.Show("Error al configurar el Regedit 1/4 : " + ex.Message);
				}
			}
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", writable: true);
				registryKey.SetValue("DisableRegistryTools", o, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch (Exception ex)
			{
				try
				{
					using (RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies", writable: true))
					{
						registryKey2.CreateSubKey("System");
						registryKey2.Close();
					}
				}
				catch
				{
					MessageBox.Show("Error al configurar el Regedit 2/4 : " + ex.Message);
				}
				try
				{
					using (RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", writable: true))
					{
						registryKey2.SetValue("DisableRegistryTools", o);
						registryKey2.Close();
					}
				}
				catch
				{
					MessageBox.Show("Error al configurar el Regedit 3/4 : " + ex.Message);
				}
			}
		}

		private void DisableControlPanel(int o)
		{
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", writable: true);
				registryKey.SetValue("NoControlPanel ", o, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error al configurar el Control Panel : " + ex.Message);
			}
		}

		private void bOk_Click(object sender, EventArgs e)
		{
			OK = true;
			Close();
		}

		private void FRM_Info_FormClosed(object sender, FormClosedEventArgs e)
		{
			IsClosed = true;
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

		public static string Get_Video_Bad_Driver()
		{
			string queryString = "SELECT Description FROM Win32_VideoController";
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(queryString);
			string result = "";
			foreach (ManagementObject item in managementObjectSearcher.Get())
			{
				if (Convert.ToString(item.Properties["InfFilename"].Value).ToLower() == "display.inf".ToLower() && Convert.ToString(item.Properties["InfSection"].Value).ToLower() == "vga".ToLower())
				{
					result = Convert.ToString(item.Properties["Description"].Value);
				}
			}
			return result;
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

		public static string Get_CPU()
		{
			string arg = string.Empty;
			string empty = string.Empty;
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
			using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = managementObjectSearcher.Get().GetEnumerator())
			{
				if (managementObjectEnumerator.MoveNext())
				{
					ManagementObject managementObject = (ManagementObject)managementObjectEnumerator.Current;
					empty = managementObject["ProcessorId"].ToString();
				}
			}
			return $"{arg}";
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			pBOTTOM = new System.Windows.Forms.Panel();
			bOk = new System.Windows.Forms.Button();
			tNetBIOS = new System.Windows.Forms.TextBox();
			lNetBIOS = new System.Windows.Forms.Label();
			tComputer = new System.Windows.Forms.TextBox();
			lComputer = new System.Windows.Forms.Label();
			gDNS = new System.Windows.Forms.GroupBox();
			tDNS2 = new System.Windows.Forms.TextBox();
			tDNS1 = new System.Windows.Forms.TextBox();
			lDNS2 = new System.Windows.Forms.Label();
			lDNS1 = new System.Windows.Forms.Label();
			rStaticDNS = new System.Windows.Forms.RadioButton();
			rAutoDNS = new System.Windows.Forms.RadioButton();
			gTCP = new System.Windows.Forms.GroupBox();
			tGateway = new System.Windows.Forms.TextBox();
			tMask = new System.Windows.Forms.TextBox();
			tIP = new System.Windows.Forms.TextBox();
			lGateway = new System.Windows.Forms.Label();
			lMask = new System.Windows.Forms.Label();
			lIP = new System.Windows.Forms.Label();
			rStaticDHCP = new System.Windows.Forms.RadioButton();
			rAutoDHCP = new System.Windows.Forms.RadioButton();
			lServer = new System.Windows.Forms.Label();
			iServer = new System.Windows.Forms.TextBox();
			iUser = new System.Windows.Forms.TextBox();
			lUser = new System.Windows.Forms.Label();
			iSelector = new System.Windows.Forms.TextBox();
			lSelector = new System.Windows.Forms.Label();
			iValidator = new System.Windows.Forms.TextBox();
			lValidator = new System.Windows.Forms.Label();
			iPrinter = new System.Windows.Forms.TextBox();
			lPrinter = new System.Windows.Forms.Label();
			iEthernet = new System.Windows.Forms.TextBox();
			lEthernet = new System.Windows.Forms.Label();
			iCPU = new System.Windows.Forms.TextBox();
			lCPU = new System.Windows.Forms.Label();
			iBarcode = new System.Windows.Forms.TextBox();
			lBarcode = new System.Windows.Forms.Label();
			lSystem = new System.Windows.Forms.Label();
			iSystem = new System.Windows.Forms.TextBox();
			iWifi = new System.Windows.Forms.TextBox();
			lWifi = new System.Windows.Forms.Label();
			iVNC = new System.Windows.Forms.TextBox();
			lVNC = new System.Windows.Forms.Label();
			pBOTTOM.SuspendLayout();
			gDNS.SuspendLayout();
			gTCP.SuspendLayout();
			SuspendLayout();
			pBOTTOM.Controls.Add(bOk);
			pBOTTOM.Dock = System.Windows.Forms.DockStyle.Bottom;
			pBOTTOM.Location = new System.Drawing.Point(0, 434);
			pBOTTOM.Name = "pBOTTOM";
			pBOTTOM.Size = new System.Drawing.Size(784, 48);
			pBOTTOM.TabIndex = 5;
			bOk.Dock = System.Windows.Forms.DockStyle.Right;
			bOk.Image = Kiosk.Properties.Resources.ico_ok;
			bOk.Location = new System.Drawing.Point(736, 0);
			bOk.Name = "bOk";
			bOk.Size = new System.Drawing.Size(48, 48);
			bOk.TabIndex = 0;
			bOk.UseVisualStyleBackColor = true;
			bOk.Click += new System.EventHandler(bOk_Click);
			tNetBIOS.Enabled = false;
			tNetBIOS.Location = new System.Drawing.Point(442, 34);
			tNetBIOS.Name = "tNetBIOS";
			tNetBIOS.Size = new System.Drawing.Size(330, 20);
			tNetBIOS.TabIndex = 14;
			lNetBIOS.AutoSize = true;
			lNetBIOS.Location = new System.Drawing.Point(376, 37);
			lNetBIOS.Name = "lNetBIOS";
			lNetBIOS.Size = new System.Drawing.Size(60, 13);
			lNetBIOS.TabIndex = 18;
			lNetBIOS.Text = "Workgroup";
			tComputer.Enabled = false;
			tComputer.Location = new System.Drawing.Point(442, 6);
			tComputer.Name = "tComputer";
			tComputer.Size = new System.Drawing.Size(330, 20);
			tComputer.TabIndex = 13;
			lComputer.AutoSize = true;
			lComputer.Location = new System.Drawing.Point(376, 9);
			lComputer.Name = "lComputer";
			lComputer.Size = new System.Drawing.Size(52, 13);
			lComputer.TabIndex = 17;
			lComputer.Text = "Computer";
			gDNS.Controls.Add(tDNS2);
			gDNS.Controls.Add(tDNS1);
			gDNS.Controls.Add(lDNS2);
			gDNS.Controls.Add(lDNS1);
			gDNS.Controls.Add(rStaticDNS);
			gDNS.Controls.Add(rAutoDNS);
			gDNS.Location = new System.Drawing.Point(379, 317);
			gDNS.Name = "gDNS";
			gDNS.Size = new System.Drawing.Size(393, 111);
			gDNS.TabIndex = 16;
			gDNS.TabStop = false;
			gDNS.Text = "DNS";
			tDNS2.Enabled = false;
			tDNS2.Location = new System.Drawing.Point(133, 71);
			tDNS2.Name = "tDNS2";
			tDNS2.Size = new System.Drawing.Size(133, 20);
			tDNS2.TabIndex = 1;
			tDNS1.Enabled = false;
			tDNS1.Location = new System.Drawing.Point(133, 44);
			tDNS1.Name = "tDNS1";
			tDNS1.Size = new System.Drawing.Size(133, 20);
			tDNS1.TabIndex = 0;
			lDNS2.AutoSize = true;
			lDNS2.Location = new System.Drawing.Point(73, 74);
			lDNS2.Name = "lDNS2";
			lDNS2.Size = new System.Drawing.Size(58, 13);
			lDNS2.TabIndex = 3;
			lDNS2.Text = "Secondary";
			lDNS1.AutoSize = true;
			lDNS1.Location = new System.Drawing.Point(73, 47);
			lDNS1.Name = "lDNS1";
			lDNS1.Size = new System.Drawing.Size(41, 13);
			lDNS1.TabIndex = 2;
			lDNS1.Text = "Primary";
			rStaticDNS.AutoSize = true;
			rStaticDNS.Enabled = false;
			rStaticDNS.Location = new System.Drawing.Point(7, 44);
			rStaticDNS.Name = "rStaticDNS";
			rStaticDNS.Size = new System.Drawing.Size(48, 17);
			rStaticDNS.TabIndex = 1;
			rStaticDNS.TabStop = true;
			rStaticDNS.Text = "DNS";
			rStaticDNS.UseVisualStyleBackColor = true;
			rAutoDNS.AutoSize = true;
			rAutoDNS.Enabled = false;
			rAutoDNS.Location = new System.Drawing.Point(7, 20);
			rAutoDNS.Name = "rAutoDNS";
			rAutoDNS.Size = new System.Drawing.Size(98, 17);
			rAutoDNS.TabIndex = 0;
			rAutoDNS.TabStop = true;
			rAutoDNS.Text = "DNS Automatic";
			rAutoDNS.UseVisualStyleBackColor = true;
			gTCP.Controls.Add(tGateway);
			gTCP.Controls.Add(tMask);
			gTCP.Controls.Add(tIP);
			gTCP.Controls.Add(lGateway);
			gTCP.Controls.Add(lMask);
			gTCP.Controls.Add(lIP);
			gTCP.Controls.Add(rStaticDHCP);
			gTCP.Controls.Add(rAutoDHCP);
			gTCP.Location = new System.Drawing.Point(379, 164);
			gTCP.Name = "gTCP";
			gTCP.Size = new System.Drawing.Size(393, 150);
			gTCP.TabIndex = 15;
			gTCP.TabStop = false;
			gTCP.Text = "TCP";
			tGateway.Enabled = false;
			tGateway.Location = new System.Drawing.Point(87, 122);
			tGateway.Name = "tGateway";
			tGateway.Size = new System.Drawing.Size(133, 20);
			tGateway.TabIndex = 2;
			tMask.Enabled = false;
			tMask.Location = new System.Drawing.Point(87, 95);
			tMask.Name = "tMask";
			tMask.Size = new System.Drawing.Size(133, 20);
			tMask.TabIndex = 1;
			tIP.Enabled = false;
			tIP.Location = new System.Drawing.Point(87, 68);
			tIP.Name = "tIP";
			tIP.Size = new System.Drawing.Size(133, 20);
			tIP.TabIndex = 0;
			lGateway.AutoSize = true;
			lGateway.Location = new System.Drawing.Point(27, 125);
			lGateway.Name = "lGateway";
			lGateway.Size = new System.Drawing.Size(49, 13);
			lGateway.TabIndex = 4;
			lGateway.Text = "Gateway";
			lMask.AutoSize = true;
			lMask.Location = new System.Drawing.Point(27, 98);
			lMask.Name = "lMask";
			lMask.Size = new System.Drawing.Size(33, 13);
			lMask.TabIndex = 3;
			lMask.Text = "Mask";
			lIP.AutoSize = true;
			lIP.Location = new System.Drawing.Point(27, 71);
			lIP.Name = "lIP";
			lIP.Size = new System.Drawing.Size(17, 13);
			lIP.TabIndex = 2;
			lIP.Text = "IP";
			rStaticDHCP.AutoSize = true;
			rStaticDHCP.Enabled = false;
			rStaticDHCP.Location = new System.Drawing.Point(7, 44);
			rStaticDHCP.Name = "rStaticDHCP";
			rStaticDHCP.Size = new System.Drawing.Size(65, 17);
			rStaticDHCP.TabIndex = 1;
			rStaticDHCP.TabStop = true;
			rStaticDHCP.Text = "IP Static";
			rStaticDHCP.UseVisualStyleBackColor = true;
			rAutoDHCP.AutoSize = true;
			rAutoDHCP.Enabled = false;
			rAutoDHCP.Location = new System.Drawing.Point(7, 20);
			rAutoDHCP.Name = "rAutoDHCP";
			rAutoDHCP.Size = new System.Drawing.Size(76, 17);
			rAutoDHCP.TabIndex = 0;
			rAutoDHCP.TabStop = true;
			rAutoDHCP.Text = "IP Dinamic";
			rAutoDHCP.UseVisualStyleBackColor = true;
			lServer.AutoSize = true;
			lServer.Location = new System.Drawing.Point(12, 16);
			lServer.Name = "lServer";
			lServer.Size = new System.Drawing.Size(38, 13);
			lServer.TabIndex = 19;
			lServer.Text = "Server";
			iServer.Enabled = false;
			iServer.Location = new System.Drawing.Point(70, 13);
			iServer.Name = "iServer";
			iServer.Size = new System.Drawing.Size(288, 20);
			iServer.TabIndex = 20;
			iUser.Enabled = false;
			iUser.Location = new System.Drawing.Point(70, 41);
			iUser.Name = "iUser";
			iUser.Size = new System.Drawing.Size(288, 20);
			iUser.TabIndex = 22;
			lUser.AutoSize = true;
			lUser.Location = new System.Drawing.Point(12, 44);
			lUser.Name = "lUser";
			lUser.Size = new System.Drawing.Size(29, 13);
			lUser.TabIndex = 21;
			lUser.Text = "User";
			iSelector.Enabled = false;
			iSelector.Location = new System.Drawing.Point(70, 69);
			iSelector.Name = "iSelector";
			iSelector.Size = new System.Drawing.Size(288, 20);
			iSelector.TabIndex = 24;
			lSelector.AutoSize = true;
			lSelector.Location = new System.Drawing.Point(12, 72);
			lSelector.Name = "lSelector";
			lSelector.Size = new System.Drawing.Size(46, 13);
			lSelector.TabIndex = 23;
			lSelector.Text = "Selector";
			iValidator.Enabled = false;
			iValidator.Location = new System.Drawing.Point(70, 97);
			iValidator.Name = "iValidator";
			iValidator.Size = new System.Drawing.Size(288, 20);
			iValidator.TabIndex = 26;
			lValidator.AutoSize = true;
			lValidator.Location = new System.Drawing.Point(12, 100);
			lValidator.Name = "lValidator";
			lValidator.Size = new System.Drawing.Size(48, 13);
			lValidator.TabIndex = 25;
			lValidator.Text = "Validator";
			iPrinter.Enabled = false;
			iPrinter.Location = new System.Drawing.Point(70, 125);
			iPrinter.Name = "iPrinter";
			iPrinter.Size = new System.Drawing.Size(288, 20);
			iPrinter.TabIndex = 28;
			lPrinter.AutoSize = true;
			lPrinter.Location = new System.Drawing.Point(12, 128);
			lPrinter.Name = "lPrinter";
			lPrinter.Size = new System.Drawing.Size(37, 13);
			lPrinter.TabIndex = 27;
			lPrinter.Text = "Printer";
			iEthernet.Enabled = false;
			iEthernet.Location = new System.Drawing.Point(442, 90);
			iEthernet.Multiline = true;
			iEthernet.Name = "iEthernet";
			iEthernet.Size = new System.Drawing.Size(330, 36);
			iEthernet.TabIndex = 31;
			lEthernet.AutoSize = true;
			lEthernet.Location = new System.Drawing.Point(376, 93);
			lEthernet.Name = "lEthernet";
			lEthernet.Size = new System.Drawing.Size(47, 13);
			lEthernet.TabIndex = 32;
			lEthernet.Text = "Ethernet";
			iCPU.Enabled = false;
			iCPU.Location = new System.Drawing.Point(442, 62);
			iCPU.Name = "iCPU";
			iCPU.Size = new System.Drawing.Size(330, 20);
			iCPU.TabIndex = 33;
			lCPU.AutoSize = true;
			lCPU.Location = new System.Drawing.Point(376, 65);
			lCPU.Name = "lCPU";
			lCPU.Size = new System.Drawing.Size(18, 13);
			lCPU.TabIndex = 34;
			lCPU.Text = "ID";
			iBarcode.Enabled = false;
			iBarcode.Location = new System.Drawing.Point(70, 153);
			iBarcode.Name = "iBarcode";
			iBarcode.Size = new System.Drawing.Size(288, 20);
			iBarcode.TabIndex = 36;
			lBarcode.AutoSize = true;
			lBarcode.Location = new System.Drawing.Point(12, 156);
			lBarcode.Name = "lBarcode";
			lBarcode.Size = new System.Drawing.Size(47, 13);
			lBarcode.TabIndex = 35;
			lBarcode.Text = "Barcode";
			lSystem.AutoSize = true;
			lSystem.Location = new System.Drawing.Point(12, 210);
			lSystem.Name = "lSystem";
			lSystem.Size = new System.Drawing.Size(41, 13);
			lSystem.TabIndex = 37;
			lSystem.Text = "System";
			iSystem.Enabled = false;
			iSystem.Location = new System.Drawing.Point(15, 230);
			iSystem.Multiline = true;
			iSystem.Name = "iSystem";
			iSystem.Size = new System.Drawing.Size(343, 198);
			iSystem.TabIndex = 38;
			iWifi.Enabled = false;
			iWifi.Location = new System.Drawing.Point(442, 130);
			iWifi.Multiline = true;
			iWifi.Name = "iWifi";
			iWifi.Size = new System.Drawing.Size(330, 36);
			iWifi.TabIndex = 39;
			lWifi.AutoSize = true;
			lWifi.Location = new System.Drawing.Point(376, 133);
			lWifi.Name = "lWifi";
			lWifi.Size = new System.Drawing.Size(25, 13);
			lWifi.TabIndex = 40;
			lWifi.Text = "Wifi";
			iVNC.Enabled = false;
			iVNC.Location = new System.Drawing.Point(70, 181);
			iVNC.Name = "iVNC";
			iVNC.Size = new System.Drawing.Size(288, 20);
			iVNC.TabIndex = 42;
			lVNC.AutoSize = true;
			lVNC.Location = new System.Drawing.Point(12, 184);
			lVNC.Name = "lVNC";
			lVNC.Size = new System.Drawing.Size(29, 13);
			lVNC.TabIndex = 41;
			lVNC.Text = "VNC";
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(784, 482);
			base.ControlBox = false;
			base.Controls.Add(iVNC);
			base.Controls.Add(lVNC);
			base.Controls.Add(iWifi);
			base.Controls.Add(lWifi);
			base.Controls.Add(iSystem);
			base.Controls.Add(lSystem);
			base.Controls.Add(iBarcode);
			base.Controls.Add(lBarcode);
			base.Controls.Add(iCPU);
			base.Controls.Add(lCPU);
			base.Controls.Add(iEthernet);
			base.Controls.Add(lEthernet);
			base.Controls.Add(iPrinter);
			base.Controls.Add(lPrinter);
			base.Controls.Add(iValidator);
			base.Controls.Add(lValidator);
			base.Controls.Add(iSelector);
			base.Controls.Add(lSelector);
			base.Controls.Add(iUser);
			base.Controls.Add(lUser);
			base.Controls.Add(iServer);
			base.Controls.Add(lServer);
			base.Controls.Add(tNetBIOS);
			base.Controls.Add(lNetBIOS);
			base.Controls.Add(tComputer);
			base.Controls.Add(lComputer);
			base.Controls.Add(gDNS);
			base.Controls.Add(gTCP);
			base.Controls.Add(pBOTTOM);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "DLG_Info";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = " ";
			base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(FRM_Info_FormClosed);
			pBOTTOM.ResumeLayout(false);
			gDNS.ResumeLayout(false);
			gDNS.PerformLayout();
			gTCP.ResumeLayout(false);
			gTCP.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
