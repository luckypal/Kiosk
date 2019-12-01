using Kiosk.Properties;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Management;
using System.Windows.Forms;

namespace Kiosk
{
	public class DLG_Net : Form
	{
		private IContainer components = null;

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

		private Button bOk;

		private TextBox tComputer;

		private Label lComputer;

		private TextBox tNetBIOS;

		private Label lNetBIOS;

		public DLG_Net()
		{
			InitializeComponent();
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
			if (!rAutoDNS.Checked)
			{
			}
			if (rAutoDHCP.Checked)
			{
			}
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
			Close();
		}

		private void bCancel_Click(object sender, EventArgs e)
		{
			Close();
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
			bOk = new System.Windows.Forms.Button();
			tComputer = new System.Windows.Forms.TextBox();
			lComputer = new System.Windows.Forms.Label();
			tNetBIOS = new System.Windows.Forms.TextBox();
			lNetBIOS = new System.Windows.Forms.Label();
			gDNS.SuspendLayout();
			gTCP.SuspendLayout();
			SuspendLayout();
			gDNS.Controls.Add(tDNS2);
			gDNS.Controls.Add(tDNS1);
			gDNS.Controls.Add(lDNS2);
			gDNS.Controls.Add(lDNS1);
			gDNS.Controls.Add(rStaticDNS);
			gDNS.Controls.Add(rAutoDNS);
			gDNS.Location = new System.Drawing.Point(12, 213);
			gDNS.Name = "gDNS";
			gDNS.Size = new System.Drawing.Size(393, 129);
			gDNS.TabIndex = 6;
			gDNS.TabStop = false;
			gDNS.Text = "DNS";
			tDNS2.Enabled = false;
			tDNS2.Location = new System.Drawing.Point(87, 95);
			tDNS2.Name = "tDNS2";
			tDNS2.Size = new System.Drawing.Size(133, 20);
			tDNS2.TabIndex = 1;
			tDNS1.Enabled = false;
			tDNS1.Location = new System.Drawing.Point(87, 68);
			tDNS1.Name = "tDNS1";
			tDNS1.Size = new System.Drawing.Size(133, 20);
			tDNS1.TabIndex = 0;
			lDNS2.AutoSize = true;
			lDNS2.Location = new System.Drawing.Point(27, 98);
			lDNS2.Name = "lDNS2";
			lDNS2.Size = new System.Drawing.Size(58, 13);
			lDNS2.TabIndex = 3;
			lDNS2.Text = "Secondary";
			lDNS1.AutoSize = true;
			lDNS1.Location = new System.Drawing.Point(27, 71);
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
			gTCP.Location = new System.Drawing.Point(12, 52);
			gTCP.Name = "gTCP";
			gTCP.Size = new System.Drawing.Size(393, 155);
			gTCP.TabIndex = 5;
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
			bOk.BackgroundImage = Kiosk.Properties.Resources.ico_ok;
			bOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			bOk.Location = new System.Drawing.Point(341, 348);
			bOk.Name = "bOk";
			bOk.Size = new System.Drawing.Size(64, 48);
			bOk.TabIndex = 2;
			bOk.UseVisualStyleBackColor = true;
			bOk.Click += new System.EventHandler(bOk_Click);
			tComputer.Enabled = false;
			tComputer.Location = new System.Drawing.Point(75, 16);
			tComputer.Name = "tComputer";
			tComputer.Size = new System.Drawing.Size(130, 20);
			tComputer.TabIndex = 0;
			lComputer.AutoSize = true;
			lComputer.Location = new System.Drawing.Point(15, 19);
			lComputer.Name = "lComputer";
			lComputer.Size = new System.Drawing.Size(52, 13);
			lComputer.TabIndex = 10;
			lComputer.Text = "Computer";
			tNetBIOS.Enabled = false;
			tNetBIOS.Location = new System.Drawing.Point(279, 16);
			tNetBIOS.Name = "tNetBIOS";
			tNetBIOS.Size = new System.Drawing.Size(126, 20);
			tNetBIOS.TabIndex = 1;
			lNetBIOS.AutoSize = true;
			lNetBIOS.Location = new System.Drawing.Point(213, 19);
			lNetBIOS.Name = "lNetBIOS";
			lNetBIOS.Size = new System.Drawing.Size(60, 13);
			lNetBIOS.TabIndex = 12;
			lNetBIOS.Text = "Workgroup";
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(417, 405);
			base.ControlBox = false;
			base.Controls.Add(tNetBIOS);
			base.Controls.Add(lNetBIOS);
			base.Controls.Add(tComputer);
			base.Controls.Add(lComputer);
			base.Controls.Add(bOk);
			base.Controls.Add(gDNS);
			base.Controls.Add(gTCP);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "DLG_Net";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "System info";
			gDNS.ResumeLayout(false);
			gDNS.PerformLayout();
			gTCP.ResumeLayout(false);
			gTCP.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
