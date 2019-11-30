using GLib.Procesos;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using TurnKiosk.Properties;

namespace TurnKiosk
{
	public class FormTURNKIOSK : Form
	{
		private IContainer components = null;

		private PictureBox pImage;

		private Label lMSG;

		private Panel pBOTTOM;

		private Button bCancel;

		private Button bOk;

		private Label label1;

		public FormTURNKIOSK()
		{
			InitializeComponent();
			Text = "Turn Kiosk v1.40";
			lMSG.Text = "Turn SYSTEM to KIOSK mode";
		}

		private void bCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		public void Lock_Windows()
		{
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Keyboard Layout", writable: true);
				byte[] value = new byte[24]
				{
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					3,
					0,
					0,
					0,
					0,
					0,
					91,
					224,
					0,
					0,
					92,
					224,
					0,
					0,
					0,
					0
				};
				registryKey.SetValue("Scancode Map", value, RegistryValueKind.Binary);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", writable: true);
				registryKey.SetValue("NoWinKeys", 1, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", writable: true);
				registryKey.SetValue("NoWinKeys", 1, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", writable: true);
				registryKey.SetValue("NoViewContextMenu", 1, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", writable: true);
				registryKey.SetValue("NoViewContextMenu", 1, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", writable: true);
				registryKey.SetValue("DisableTaskMgr", 1, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", writable: true);
				registryKey.SetValue("NoDesktop", 1, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon", writable: true);
				registryKey.SetValue("Shell", "c:\\kiosk\\loader.exe", RegistryValueKind.String);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon", writable: true);
				registryKey.SetValue("Shell", "c:\\kiosk\\loader.exe", RegistryValueKind.String);
				registryKey.Close();
			}
			catch
			{
			}
			bool flag = true;
		}

		private void CleanUp()
		{
			string str = "c:\\Kiosk";
			string text = str + "\\ccleaner.exe";
			if (File.Exists(text))
			{
				Process process = Process.Start(text, "/AUTO");
				Thread.Sleep(1000);
				process.WaitForExit();
			}
		}

		private void FreezeWindows()
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
			string text = folderPath + "\\Toolwiz Time Freeze 2014\\ToolwizTimeFreeze.exe";
			if (File.Exists(text))
			{
				Process.Start(text, "/check /usepass=kiosksave");
				DelayApp(1000);
				Process.Start(text, "/freezealways /usepass=kiosksave");
				DelayApp(1000);
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

		private void DisableSafeMode()
		{
			string name = "System\\CurrentControlSet\\Control\\SafeBoot";
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name, writable: true);
				RegistryUtilities.RenameSubKey(registryKey, "Minimal", "MinimalX");
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name, writable: true);
				RegistryUtilities.RenameSubKey(registryKey, "Network", "NetworkX");
				registryKey.Close();
			}
			catch
			{
			}
		}

		public bool ChangePasswordAdmin(string _password)
		{
			try
			{
				string path = "WinNT://" + Environment.MachineName + ",adminuser";
				DirectoryEntry directoryEntry = new DirectoryEntry(path);
				object[] args = new object[1]
				{
					_password
				};
				directoryEntry.Invoke("SetPassword", args);
				directoryEntry.CommitChanges();
				return true;
			}
			catch
			{
			}
			return false;
		}

		public bool ChangePassword(string _user, string _password)
		{
			try
			{
				string path = "WinNT://" + Environment.MachineName + ",computer";
				DirectoryEntry directoryEntry = new DirectoryEntry(path);
				DirectoryEntry directoryEntry2 = directoryEntry.Children.Find(_user, "user");
				directoryEntry2.Invoke("SetPassword", _password);
				directoryEntry2.CommitChanges();
				return true;
			}
			catch
			{
			}
			return false;
		}

		private string CheckSystem()
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
			string folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.System);
			string str = "c:\\Kiosk";
			string text = str + "\\ccleaner.exe";
			if (!File.Exists(text))
			{
				return "Missing CLEANER: " + text;
			}
			text = folderPath2 + "\\kbfmgr.exe";
			File.Exists(text);
			bool flag = 0 == 0;
			text = str + "\\loader.exe";
			if (!File.Exists(text))
			{
				return "Missing LOADER: " + text;
			}
			text = str + "\\kiosk.exe";
			if (!File.Exists(text))
			{
				return "Missing KIOSK: " + text;
			}
			return "OK";
		}

		private void DelayApp(int _milis)
		{
			int num = _milis / 100;
			do
			{
				Application.DoEvents();
				Thread.Sleep(100);
				num--;
			}
			while (num > 0);
		}

		private void bOk_Click(object sender, EventArgs e)
		{
			if (CheckSystem() == "OK")
			{
				bOk.Enabled = false;
				bCancel.Enabled = false;
				lMSG.Text = "Please Wait 1/5";
				DelayApp(100);
				CleanUp();
				DelayApp(1000);
				lMSG.Text = "Please Wait 2/5";
				DelayApp(100);
				Lock_Windows();
				lMSG.Text = "Please Wait 3/5";
				DelayApp(100);
				lMSG.Text = "Please Wait 4/5";
				DelayApp(100);
				DisableSafeMode();
				DelayApp(1000);
				lMSG.Text = "Please Wait 5/5";
				DelayApp(100);
				ChangePasswordAdmin("repair7");
				DelayApp(1000);
				Process.Start("shutdown.exe", "/r /t 5");
				lMSG.Text = "Restarting...";
				while (true)
				{
					Application.DoEvents();
					bool flag = true;
				}
			}
			MessageBox.Show(CheckSystem());
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
			pImage = new System.Windows.Forms.PictureBox();
			lMSG = new System.Windows.Forms.Label();
			pBOTTOM = new System.Windows.Forms.Panel();
			bCancel = new System.Windows.Forms.Button();
			bOk = new System.Windows.Forms.Button();
			label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)pImage).BeginInit();
			pBOTTOM.SuspendLayout();
			SuspendLayout();
			pImage.Image = TurnKiosk.Properties.Resources.big_warnning;
			pImage.Location = new System.Drawing.Point(136, 2);
			pImage.Name = "pImage";
			pImage.Size = new System.Drawing.Size(128, 128);
			pImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			pImage.TabIndex = 9;
			pImage.TabStop = false;
			lMSG.Dock = System.Windows.Forms.DockStyle.Fill;
			lMSG.Font = new System.Drawing.Font("Microsoft Sans Serif", 18f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lMSG.Location = new System.Drawing.Point(0, 0);
			lMSG.Name = "lMSG";
			lMSG.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
			lMSG.Size = new System.Drawing.Size(400, 174);
			lMSG.TabIndex = 8;
			lMSG.Text = "-";
			lMSG.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			pBOTTOM.Controls.Add(bCancel);
			pBOTTOM.Controls.Add(bOk);
			pBOTTOM.Dock = System.Windows.Forms.DockStyle.Bottom;
			pBOTTOM.Location = new System.Drawing.Point(0, 197);
			pBOTTOM.Name = "pBOTTOM";
			pBOTTOM.Size = new System.Drawing.Size(400, 64);
			pBOTTOM.TabIndex = 7;
			bCancel.Dock = System.Windows.Forms.DockStyle.Left;
			bCancel.FlatAppearance.BorderSize = 0;
			bCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bCancel.Image = TurnKiosk.Properties.Resources.big_cancel;
			bCancel.Location = new System.Drawing.Point(0, 0);
			bCancel.Name = "bCancel";
			bCancel.Size = new System.Drawing.Size(96, 64);
			bCancel.TabIndex = 1;
			bCancel.UseVisualStyleBackColor = true;
			bCancel.Click += new System.EventHandler(bCancel_Click);
			bOk.Dock = System.Windows.Forms.DockStyle.Right;
			bOk.FlatAppearance.BorderSize = 0;
			bOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bOk.Image = TurnKiosk.Properties.Resources.big_ok;
			bOk.Location = new System.Drawing.Point(304, 0);
			bOk.Name = "bOk";
			bOk.Size = new System.Drawing.Size(96, 64);
			bOk.TabIndex = 0;
			bOk.UseVisualStyleBackColor = true;
			bOk.Click += new System.EventHandler(bOk_Click);
			label1.Dock = System.Windows.Forms.DockStyle.Bottom;
			label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			label1.ForeColor = System.Drawing.Color.OrangeRed;
			label1.Location = new System.Drawing.Point(0, 174);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(400, 23);
			label1.TabIndex = 10;
			label1.Text = "Warnning, can lost all access to computer!";
			label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(400, 261);
			base.ControlBox = false;
			base.Controls.Add(pImage);
			base.Controls.Add(lMSG);
			base.Controls.Add(label1);
			base.Controls.Add(pBOTTOM);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "FormTURNKIOSK";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Turn Kiosk ON";
			((System.ComponentModel.ISupportInitialize)pImage).EndInit();
			pBOTTOM.ResumeLayout(false);
			ResumeLayout(false);
		}
	}
}
