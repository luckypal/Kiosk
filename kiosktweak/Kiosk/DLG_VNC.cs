using InstallKiosk;
using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Kiosk
{
	public class DLG_VNC : Form
	{
		private string IP;

		private string IP_Def;

		private IContainer components;

		private Button bVNC;

		private Button bCancel;

		private Button bKey;

		private TextBox tIP;

		private Button bVNC_OFF;

		private Button bIP;

		private Label lIP;

		private ToolTip toolTip1;

		private Button bInstall;

		public DLG_VNC()
		{
			InitializeComponent();
			IP_Def = "control.game-host.org";
			IP = IP_Def;
			tIP.Text = IP;
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void bKey_Click(object sender, EventArgs e)
		{
			Process[] processesByName = Process.GetProcessesByName("KVKeyboard");
			if (processesByName.Length < 1)
			{
				if (File.Exists("c:\\kiosk\\KVKeyboard.exe"))
				{
					try
					{
						CheckCommands.RunPackage("c:\\kiosk\\", "KVKeyboard.exe", "es");
					}
					catch
					{
						MessageBox.Show("Keyboard on screeen not present");
					}
				}
				else
				{
					MessageBox.Show("Keyboard on screeen not present");
				}
			}
		}

		private void bVNC_Click(object sender, EventArgs e)
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\winvnc.exe";
			string str = IP_Def;
			IP = tIP.Text;
			if (!string.IsNullOrEmpty(IP))
			{
				str = IP;
			}
			if (File.Exists(text))
			{
				Splash splash = new Splash("Starting Remote Control ...");
				splash.Show();
				splash.Invalidate();
				Application.DoEvents();
				if (CheckCommands.VNC_Running())
				{
					Process.Start(text, "-kill");
					while (CheckCommands.VNC_Running())
					{
					}
				}
				Process.Start(text, "-connect " + str + ":5500 -run");
				splash.Hide();
				splash.Dispose();
				Application.DoEvents();
				MessageBox.Show("Waiting connection ...");
			}
			else
			{
				MessageBox.Show("Remote Control Software is not installed");
			}
		}

		private void bVNC_OFF_Click(object sender, EventArgs e)
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\winvnc.exe";
			if (File.Exists(text))
			{
				if (CheckCommands.VNC_Running())
				{
					Splash splash = new Splash("Stop Remote Control ...");
					splash.Show();
					splash.Invalidate();
					Application.DoEvents();
					Process.Start(text, "-kill");
					while (CheckCommands.VNC_Running())
					{
					}
					splash.Hide();
					splash.Dispose();
					Application.DoEvents();
				}
				else
				{
					MessageBox.Show("Remote Control is stoped");
				}
			}
			else
			{
				MessageBox.Show("Remote Control Software is not installed");
			}
		}

		private void bCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void bIP_Click(object sender, EventArgs e)
		{
			lIP.Visible = true;
			tIP.Visible = true;
			tIP.Text = IP;
		}

		private void DLG_VNC_Load(object sender, EventArgs e)
		{
			if (CheckCommands.Check_VNC() == null)
			{
				bVNC.Enabled = false;
				bVNC_OFF.Enabled = false;
			}
		}

		private void bInstall_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Atention!", "Install Remote Control Software in computer", MessageBoxButtons.YesNo) != DialogResult.Yes)
			{
				return;
			}
			Splash splash = new Splash("Installing Remote Control software...");
			splash.Show();
			splash.Invalidate();
			Application.DoEvents();
			if (CheckCommands.CopyPackages("c:\\drivers\\", "vnc_install.exe,data1.zip", _force: false))
			{
				if (CheckCommands.InstallPackage("vnc_install.exe", "/verysilent /norestart"))
				{
					if (CheckCommands.CopyPackages("c:\\drivers\\", "data1.zip", _force: true))
					{
						if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\ultravnc.ini"))
						{
							File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\ultravnc.ini");
						}
						File.Copy("c:\\drivers\\data1.zip", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\ultravnc.ini");
					}
					MessageBox.Show("Remote Control software installed");
					if (CheckCommands.Check_VNC() == null)
					{
						bVNC.Enabled = false;
						bVNC_OFF.Enabled = false;
					}
				}
				else
				{
					splash.Hide();
					MessageBox.Show("Error installing Remote Control software");
				}
			}
			else
			{
				splash.Hide();
				MessageBox.Show("Error installing Remote Control software");
			}
			splash.Hide();
			splash.Dispose();
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
			components = new System.ComponentModel.Container();
			tIP = new System.Windows.Forms.TextBox();
			lIP = new System.Windows.Forms.Label();
			toolTip1 = new System.Windows.Forms.ToolTip(components);
			bInstall = new System.Windows.Forms.Button();
			bIP = new System.Windows.Forms.Button();
			bVNC_OFF = new System.Windows.Forms.Button();
			bKey = new System.Windows.Forms.Button();
			bCancel = new System.Windows.Forms.Button();
			bVNC = new System.Windows.Forms.Button();
			SuspendLayout();
			tIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			tIP.Location = new System.Drawing.Point(228, 30);
			tIP.Name = "tIP";
			tIP.Size = new System.Drawing.Size(245, 29);
			tIP.TabIndex = 23;
			tIP.Visible = false;
			lIP.AutoSize = true;
			lIP.Location = new System.Drawing.Point(228, 12);
			lIP.Name = "lIP";
			lIP.Size = new System.Drawing.Size(116, 13);
			lIP.TabIndex = 26;
			lIP.Text = "New IP Remote Server";
			lIP.Visible = false;
			bInstall.Image = Kiosk.Properties.Resources.box_tall;
			bInstall.Location = new System.Drawing.Point(12, 66);
			bInstall.Name = "bInstall";
			bInstall.Size = new System.Drawing.Size(48, 48);
			bInstall.TabIndex = 27;
			toolTip1.SetToolTip(bInstall, "Install VNC Software");
			bInstall.UseVisualStyleBackColor = true;
			bInstall.Click += new System.EventHandler(bInstall_Click);
			bIP.Image = Kiosk.Properties.Resources.ico_options;
			bIP.Location = new System.Drawing.Point(174, 12);
			bIP.Name = "bIP";
			bIP.Size = new System.Drawing.Size(48, 48);
			bIP.TabIndex = 25;
			toolTip1.SetToolTip(bIP, "Modify VNC Server IP");
			bIP.UseVisualStyleBackColor = true;
			bIP.Click += new System.EventHandler(bIP_Click);
			bVNC_OFF.Image = Kiosk.Properties.Resources.ico_monitors_del;
			bVNC_OFF.Location = new System.Drawing.Point(66, 12);
			bVNC_OFF.Name = "bVNC_OFF";
			bVNC_OFF.Size = new System.Drawing.Size(48, 48);
			bVNC_OFF.TabIndex = 24;
			toolTip1.SetToolTip(bVNC_OFF, "Stop remote control");
			bVNC_OFF.UseVisualStyleBackColor = true;
			bVNC_OFF.Click += new System.EventHandler(bVNC_OFF_Click);
			bKey.Image = Kiosk.Properties.Resources.ico_keyboard;
			bKey.Location = new System.Drawing.Point(120, 12);
			bKey.Name = "bKey";
			bKey.Size = new System.Drawing.Size(48, 48);
			bKey.TabIndex = 22;
			toolTip1.SetToolTip(bKey, "Launch Keyboard on Screen");
			bKey.UseVisualStyleBackColor = true;
			bKey.Click += new System.EventHandler(bKey_Click);
			bCancel.Image = Kiosk.Properties.Resources.ico_ok;
			bCancel.Location = new System.Drawing.Point(425, 65);
			bCancel.Name = "bCancel";
			bCancel.Size = new System.Drawing.Size(48, 48);
			bCancel.TabIndex = 21;
			bCancel.UseVisualStyleBackColor = true;
			bCancel.Click += new System.EventHandler(bCancel_Click);
			bVNC.Image = Kiosk.Properties.Resources.ico_monitors;
			bVNC.Location = new System.Drawing.Point(12, 12);
			bVNC.Name = "bVNC";
			bVNC.Size = new System.Drawing.Size(48, 48);
			bVNC.TabIndex = 19;
			toolTip1.SetToolTip(bVNC, "Start remote control");
			bVNC.UseVisualStyleBackColor = true;
			bVNC.Click += new System.EventHandler(bVNC_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(480, 121);
			base.Controls.Add(bInstall);
			base.Controls.Add(lIP);
			base.Controls.Add(bIP);
			base.Controls.Add(bVNC_OFF);
			base.Controls.Add(tIP);
			base.Controls.Add(bKey);
			base.Controls.Add(bCancel);
			base.Controls.Add(bVNC);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "DLG_VNC";
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Remote Control";
			base.Load += new System.EventHandler(DLG_VNC_Load);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
