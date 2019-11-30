using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Threading;
using System.Windows.Forms;
using wyUpdate.Properties;

namespace wyUpdate
{
	public class DLS_Login : Form
	{
		public string PassWord;

		private IContainer components;

		private Panel pMenu;

		private Button bOK;

		private Button bCANCEL;

		private TextBox tPASSWORD;

		private Button bKEYB;

		private Button btnNET;

		private Button btnVNC;

		private Button btnReboot;

		public DLS_Login()
		{
			InitializeComponent();
			PassWord = null;
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			PassWord = tPASSWORD.Text;
			Close();
		}

		private void bCANCEL_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void tPASSWORD_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				PassWord = tPASSWORD.Text;
				Close();
			}
		}

		private void DLS_Login_Load(object sender, EventArgs e)
		{
			tPASSWORD.Focus();
		}

		private void bKEYB_Click(object sender, EventArgs e)
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

		private void btnReboot_Click(object sender, EventArgs e)
		{
			Process.Start("shutdown.exe", "/r /t 4");
			while (true)
			{
				Application.DoEvents();
			}
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

		private void btnVNC_Click(object sender, EventArgs e)
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\winvnc.exe";
			if (File.Exists(text))
			{
				if (VNC_Running())
				{
					Process.Start(text, "-kill");
					Thread.Sleep(5000);
				}
				Process.Start(text, "-connect control.game-host.org:5500 -run");
			}
		}

		private void btnNET_Click(object sender, EventArgs e)
		{
			ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
			ManagementObjectCollection instances = managementClass.GetInstances();
			string[] value = new string[2]
			{
				"8.8.8.8",
				"8.8.4.4"
			};
			foreach (ManagementObject item in instances)
			{
				if ((bool)item["IPEnabled"])
				{
					ManagementBaseObject methodParameters = item.GetMethodParameters("SetDNSServerSearchOrder");
					methodParameters["DNSServerSearchOrder"] = value;
					item.InvokeMethod("SetDNSServerSearchOrder", methodParameters, null);
					item.InvokeMethod("EnableDHCP", null, null);
				}
			}
			MessageBox.Show("Setting default network options, dynapic IP");
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
			tPASSWORD = new System.Windows.Forms.TextBox();
			btnReboot = new System.Windows.Forms.Button();
			btnVNC = new System.Windows.Forms.Button();
			btnNET = new System.Windows.Forms.Button();
			bKEYB = new System.Windows.Forms.Button();
			bOK = new System.Windows.Forms.Button();
			bCANCEL = new System.Windows.Forms.Button();
			pMenu.SuspendLayout();
			SuspendLayout();
			pMenu.BackColor = System.Drawing.Color.White;
			pMenu.Controls.Add(btnReboot);
			pMenu.Controls.Add(btnVNC);
			pMenu.Controls.Add(btnNET);
			pMenu.Controls.Add(bKEYB);
			pMenu.Controls.Add(bOK);
			pMenu.Controls.Add(bCANCEL);
			pMenu.Dock = System.Windows.Forms.DockStyle.Bottom;
			pMenu.Location = new System.Drawing.Point(0, 236);
			pMenu.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			pMenu.Name = "pMenu";
			pMenu.Size = new System.Drawing.Size(844, 98);
			pMenu.TabIndex = 0;
			tPASSWORD.Font = new System.Drawing.Font("Microsoft Sans Serif", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			tPASSWORD.Location = new System.Drawing.Point(89, 62);
			tPASSWORD.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			tPASSWORD.Name = "tPASSWORD";
			tPASSWORD.Size = new System.Drawing.Size(543, 44);
			tPASSWORD.TabIndex = 0;
			tPASSWORD.UseSystemPasswordChar = true;
			tPASSWORD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(tPASSWORD_KeyPress);
			btnReboot.Dock = System.Windows.Forms.DockStyle.Left;
			btnReboot.FlatAppearance.BorderSize = 0;
			btnReboot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			btnReboot.Image = wyUpdate.Properties.Resources.ico_bn_reboot;
			btnReboot.Location = new System.Drawing.Point(576, 0);
			btnReboot.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			btnReboot.Name = "btnReboot";
			btnReboot.Size = new System.Drawing.Size(144, 98);
			btnReboot.TabIndex = 5;
			btnReboot.UseVisualStyleBackColor = true;
			btnReboot.Click += new System.EventHandler(btnReboot_Click);
			btnVNC.Dock = System.Windows.Forms.DockStyle.Left;
			btnVNC.FlatAppearance.BorderSize = 0;
			btnVNC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			btnVNC.Image = wyUpdate.Properties.Resources.ico64_remote;
			btnVNC.Location = new System.Drawing.Point(432, 0);
			btnVNC.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			btnVNC.Name = "btnVNC";
			btnVNC.Size = new System.Drawing.Size(144, 98);
			btnVNC.TabIndex = 4;
			btnVNC.UseVisualStyleBackColor = true;
			btnVNC.Click += new System.EventHandler(btnVNC_Click);
			btnNET.Dock = System.Windows.Forms.DockStyle.Left;
			btnNET.FlatAppearance.BorderSize = 0;
			btnNET.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			btnNET.Image = wyUpdate.Properties.Resources.ico64_reset_net;
			btnNET.Location = new System.Drawing.Point(288, 0);
			btnNET.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			btnNET.Name = "btnNET";
			btnNET.Size = new System.Drawing.Size(144, 98);
			btnNET.TabIndex = 3;
			btnNET.UseVisualStyleBackColor = true;
			btnNET.Click += new System.EventHandler(btnNET_Click);
			bKEYB.Dock = System.Windows.Forms.DockStyle.Left;
			bKEYB.FlatAppearance.BorderSize = 0;
			bKEYB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bKEYB.Image = wyUpdate.Properties.Resources.ico_osk;
			bKEYB.Location = new System.Drawing.Point(144, 0);
			bKEYB.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			bKEYB.Name = "bKEYB";
			bKEYB.Size = new System.Drawing.Size(144, 98);
			bKEYB.TabIndex = 2;
			bKEYB.UseVisualStyleBackColor = true;
			bKEYB.Click += new System.EventHandler(bKEYB_Click);
			bOK.Dock = System.Windows.Forms.DockStyle.Right;
			bOK.FlatAppearance.BorderSize = 0;
			bOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bOK.Image = wyUpdate.Properties.Resources.bw_ok_o;
			bOK.Location = new System.Drawing.Point(700, 0);
			bOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			bOK.Name = "bOK";
			bOK.Size = new System.Drawing.Size(144, 98);
			bOK.TabIndex = 0;
			bOK.UseVisualStyleBackColor = true;
			bOK.Click += new System.EventHandler(bOK_Click);
			bCANCEL.Dock = System.Windows.Forms.DockStyle.Left;
			bCANCEL.FlatAppearance.BorderSize = 0;
			bCANCEL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bCANCEL.Image = wyUpdate.Properties.Resources.bw_cancel_s;
			bCANCEL.Location = new System.Drawing.Point(0, 0);
			bCANCEL.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			bCANCEL.Name = "bCANCEL";
			bCANCEL.Size = new System.Drawing.Size(144, 98);
			bCANCEL.TabIndex = 1;
			bCANCEL.UseVisualStyleBackColor = true;
			bCANCEL.Click += new System.EventHandler(bCANCEL_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(9f, 20f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.White;
			base.ClientSize = new System.Drawing.Size(844, 334);
			base.Controls.Add(tPASSWORD);
			base.Controls.Add(pMenu);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			base.Name = "DLS_Login";
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = " ";
			base.Load += new System.EventHandler(DLS_Login_Load);
			pMenu.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
