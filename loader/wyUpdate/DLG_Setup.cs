using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;
using wyUpdate.Properties;

namespace wyUpdate
{
	public class DLG_Setup : Form
	{
		private IContainer components;

		private Panel pMenu;

		private Button bOK;

		private Button bWINDOWS;

		private TextBox tInfo;

		private Button bKey;

		public DLG_Setup()
		{
			InitializeComponent();
		}

		public string GetCPUID()
		{
			ManagementObjectCollection managementObjectCollection = null;
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * From Win32_processor");
			managementObjectCollection = managementObjectSearcher.Get();
			string result = "";
			foreach (ManagementObject item in managementObjectCollection)
			{
				result = item["ProcessorID"].ToString();
			}
			return result;
		}

		public string GetMACAddress()
		{
			string text = string.Empty;
			try
			{
				NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
				NetworkInterface[] array = allNetworkInterfaces;
				foreach (NetworkInterface networkInterface in array)
				{
					if (text == string.Empty)
					{
						networkInterface.GetIPProperties();
						text = networkInterface.GetPhysicalAddress().ToString();
					}
				}
				return text;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return text;
			}
		}

		private void DLG_Setup_Load(object sender, EventArgs e)
		{
			tInfo.Text = "";
			TextBox textBox = tInfo;
			textBox.Text = textBox.Text + "ID: " + GetCPUID() + "\r\n";
			TextBox textBox2 = tInfo;
			textBox2.Text = textBox2.Text + "MAC: " + GetMACAddress() + "\r\n";
		}

		public static void Freeze_On()
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
			string text = folderPath + "\\Toolwiz Time Freeze 2014\\ToolwizTimeFreeze.exe";
			if (File.Exists(text))
			{
				Process.Start(text, "/freeze /usepass=kiosksave");
				Thread.Sleep(1000);
			}
		}

		public static void Freeze_Off()
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
			string text = folderPath + "\\Toolwiz Time Freeze 2014\\ToolwizTimeFreeze.exe";
			if (File.Exists(text))
			{
				Process.Start(text, "/unfreeze /usepass=kiosksave");
				Thread.Sleep(1000);
			}
		}

		public static string Freeze_Timestamp()
		{
			return $"{DateTime.Now.Ticks / 10000000}";
		}

		public static void Freeze_Build_Timestamp()
		{
			string path = "c:\\kiosk\\unfreezeboot.tmp";
			if (!File.Exists(path))
			{
				try
				{
					File.Delete(path);
					Application.DoEvents();
				}
				catch
				{
				}
			}
			try
			{
				string contents = Freeze_Timestamp();
				File.WriteAllText(path, contents);
			}
			catch
			{
			}
			Application.DoEvents();
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
					MessageBox.Show("FREEZE UNLOCK " + (num - num2));
					return 1;
				}
			}
			catch
			{
			}
			return 0;
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

		public void UnLock_Windows()
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

		private void Reset()
		{
			Process.Start("shutdown.exe", "/r /t 4");
			while (true)
			{
				Application.DoEvents();
			}
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void bWINDOWS_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Turn to Windows Mode", "Stop", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
				string path = folderPath + "\\Toolwiz Time Freeze 2014\\ToolwizTimeFreeze.exe";
				if (File.Exists(path) && Freeze_Check() == 1)
				{
					Freeze_Build_Timestamp();
					Freeze_Off();
					Reset();
				}
				else
				{
					UnLock_Windows();
					try
					{
						RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\WinLogon", writable: true);
						registryKey.SetValue("Shell", "explorer.exe", RegistryValueKind.String);
						registryKey.Close();
					}
					catch
					{
					}
					try
					{
						RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\WinLogon", writable: true);
						registryKey2.SetValue("Shell", "explorer.exe", RegistryValueKind.String);
						registryKey2.Close();
					}
					catch
					{
					}
					Reset();
				}
			}
		}

		private void bKey_Click(object sender, EventArgs e)
		{
			Process[] processesByName = Process.GetProcessesByName("KVKeyboard");
			if (processesByName.Length < 1)
			{
				ProcessStartInfo processStartInfo = new ProcessStartInfo();
				processStartInfo.Arguments = "es";
				processStartInfo.FileName = "KVKeyboard.exe";
				processStartInfo.WorkingDirectory = "c:\\kiosk";
				processStartInfo.WindowStyle = ProcessWindowStyle.Normal;
				Process.Start(processStartInfo);
			}
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
			pMenu = new System.Windows.Forms.Panel();
			bOK = new System.Windows.Forms.Button();
			bWINDOWS = new System.Windows.Forms.Button();
			tInfo = new System.Windows.Forms.TextBox();
			bKey = new System.Windows.Forms.Button();
			pMenu.SuspendLayout();
			SuspendLayout();
			pMenu.BackColor = System.Drawing.Color.White;
			pMenu.Controls.Add(bKey);
			pMenu.Controls.Add(bOK);
			pMenu.Controls.Add(bWINDOWS);
			pMenu.Dock = System.Windows.Forms.DockStyle.Bottom;
			pMenu.Location = new System.Drawing.Point(0, 154);
			pMenu.Name = "pMenu";
			pMenu.Size = new System.Drawing.Size(464, 64);
			pMenu.TabIndex = 1;
			bOK.Dock = System.Windows.Forms.DockStyle.Right;
			bOK.FlatAppearance.BorderSize = 0;
			bOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bOK.Image = wyUpdate.Properties.Resources.big_ok;
			bOK.Location = new System.Drawing.Point(368, 0);
			bOK.Name = "bOK";
			bOK.Size = new System.Drawing.Size(96, 64);
			bOK.TabIndex = 1;
			bOK.UseVisualStyleBackColor = true;
			bOK.Click += new System.EventHandler(bOK_Click);
			bWINDOWS.Dock = System.Windows.Forms.DockStyle.Left;
			bWINDOWS.FlatAppearance.BorderSize = 0;
			bWINDOWS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bWINDOWS.Image = wyUpdate.Properties.Resources.ico_win;
			bWINDOWS.Location = new System.Drawing.Point(0, 0);
			bWINDOWS.Name = "bWINDOWS";
			bWINDOWS.Size = new System.Drawing.Size(96, 64);
			bWINDOWS.TabIndex = 0;
			bWINDOWS.UseVisualStyleBackColor = true;
			bWINDOWS.Click += new System.EventHandler(bWINDOWS_Click);
			tInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
			tInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			tInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			tInfo.Location = new System.Drawing.Point(0, 0);
			tInfo.Margin = new System.Windows.Forms.Padding(0);
			tInfo.Multiline = true;
			tInfo.Name = "tInfo";
			tInfo.ReadOnly = true;
			tInfo.Size = new System.Drawing.Size(464, 154);
			tInfo.TabIndex = 2;
			bKey.Dock = System.Windows.Forms.DockStyle.Left;
			bKey.FlatAppearance.BorderSize = 0;
			bKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bKey.Image = wyUpdate.Properties.Resources.ico_keyboard;
			bKey.Location = new System.Drawing.Point(96, 0);
			bKey.Name = "bKey";
			bKey.Size = new System.Drawing.Size(96, 64);
			bKey.TabIndex = 2;
			bKey.UseVisualStyleBackColor = true;
			bKey.Click += new System.EventHandler(bKey_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(464, 218);
			base.Controls.Add(tInfo);
			base.Controls.Add(pMenu);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "DLG_Setup";
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = " ";
			base.Load += new System.EventHandler(DLG_Setup_Load);
			pMenu.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
