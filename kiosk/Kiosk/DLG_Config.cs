using Kiosk.Properties;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Kiosk
{
	public class DLG_Config : Form
	{
		public MainWindow MWin;

		public bool OK;

		public Configuracion opciones;

		private int mw = 0;

		private int reboot = 0;

		private IContainer components = null;

		private Button bCancel;

		private Button bOK;

		private Button bSponsor;

		private Button bValidators;

		private Button bPassword;

		private Button bOptions;

		private Button bPanel;

		private Button bExplorer;

		private Button bMin;

		private Button bKey;

		private Button bDepositaire;

		private Button button1;

		private Button bRESET;

		private Button bTouch;

		private Button bMonitors;

		private Button button2;

		private Button btn_mail;

		private Label lInfo;

		private Button bVNC;

		private Button bVNC2;

		private Button bFreeze;

		private Button bWifi;

		private Button btnTICKET;

		public DLG_Config(ref Configuracion _opc)
		{
			OK = false;
			opciones = _opc;
			InitializeComponent();
			Localize();
			Text = "Kiosk v" + _opc.VersionPRG + " ID:" + _opc.IDMAQUINA;
			opciones.Log_Debug("Enter config mode");
			MWin = null;
			reboot = 0;
			mw = 0;
		}

		private void Localize()
		{
			SuspendLayout();
			bOptions.Text = "1. " + opciones.Localize.Text("Preferences");
			bSponsor.Text = "2. " + opciones.Localize.Text("Server");
			bValidators.Text = "3. " + opciones.Localize.Text("Validators");
			bPassword.Text = "4. " + opciones.Localize.Text("Password");
			bDepositaire.Text = "5. " + opciones.Localize.Text("Ticket");
			bMonitors.Text = "6. " + opciones.Localize.Text("Slide Show");
			ResumeLayout();
		}

		private void bSponsor_Click(object sender, EventArgs e)
		{
			DLG_Sponsors dLG_Sponsors = new DLG_Sponsors(ref opciones);
			dLG_Sponsors.Focus();
			dLG_Sponsors.ShowDialog();
		}

		private void bValidators_Click(object sender, EventArgs e)
		{
			Devices_Wizard devices_Wizard = new Devices_Wizard(ref opciones);
			devices_Wizard.Focus();
			devices_Wizard.ShowDialog();
			lInfo.Text = "Coins: " + opciones.Dev_Coin.ToUpper() + " (" + opciones.Dev_Coin_P.ToUpper() + ") / Notes: " + opciones.Dev_BNV.ToUpper() + " (" + opciones.Dev_BNV_P.ToUpper() + ")";
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			Configuracion.Freeze_On();
			OK = false;
			opciones.Save_Net();
			opciones.Load_Net();
			opciones.SEND_Mail("SAVE", opciones.Update_Info());
			Close();
		}

		private void bCancel_Click(object sender, EventArgs e)
		{
			Configuracion.Freeze_On();
			Close();
		}

		private void bPassword_Click(object sender, EventArgs e)
		{
			DLG_Password_Change dLG_Password_Change = new DLG_Password_Change(ref opciones, 1);
			dLG_Password_Change.ShowDialog();
		}

		private void bOptions_Click(object sender, EventArgs e)
		{
			DLG_Preferencias dLG_Preferencias = new DLG_Preferencias(ref opciones, ref MWin);
			dLG_Preferencias.ShowDialog();
			Localize();
		}

		private void bNet_Click(object sender, EventArgs e)
		{
		}

		private void bExplorer_Click(object sender, EventArgs e)
		{
			Process.Start("explorer.exe");
		}

		private void bPanel_Click(object sender, EventArgs e)
		{
			bool flag = false;
			mw = 1;
			MWin.UnLock_Windows();
			Configuracion.Freeze_Off();
			Process.Start("control.exe");
		}

		private void DLG_Config_Load(object sender, EventArgs e)
		{
			if (Configuracion.Freeze_Check() == 1)
			{
				bFreeze.BackColor = Color.Red;
			}
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
			string path = folderPath + "\\uvnc bvba\\UltraVNC\\winvnc.exe";
			if (File.Exists(path))
			{
				bVNC.Visible = true;
				bVNC2.Visible = true;
			}
			path = folderPath + "\\Toolwiz Time Freeze 2014\\ToolwizTimeFreeze.exe";
			if (!File.Exists(path))
			{
				bFreeze.Visible = false;
			}
			lInfo.Text = "Coins: " + opciones.Dev_Coin.ToUpper() + " (" + opciones.Dev_Coin_P.ToUpper() + ") / Notes: " + opciones.Dev_BNV.ToUpper() + " (" + opciones.Dev_BNV_P.ToUpper() + ")";
			opciones.SEND_Mail("CONFIG", "ENTER TO CONFIG");
		}

		private void bMin_Click(object sender, EventArgs e)
		{
			if (MWin != null)
			{
				if (MWin.Visible)
				{
					MWin.Hide();
				}
				else
				{
					MWin.Show();
				}
			}
		}

		private void DLG_Config_FormClosed(object sender, FormClosedEventArgs e)
		{
			bool flag = false;
			if (mw == 1 && reboot == 0)
			{
				MWin.Lock_Windows();
			}
			if (MWin != null)
			{
				MWin.Show();
			}
		}

		private void bKey_Click(object sender, EventArgs e)
		{
			Process[] processesByName = Process.GetProcessesByName("KVKeyboard");
			if (processesByName.Length < 1)
			{
				Process.Start("KVKeyboard.exe", "es");
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			DLG_Info dLG_Info = new DLG_Info(ref opciones);
			dLG_Info.ShowDialog();
		}

		private void bDepositaire_Click(object sender, EventArgs e)
		{
			DLG_Depositaire dLG_Depositaire = new DLG_Depositaire(ref opciones);
			dLG_Depositaire.MWin = MWin;
			dLG_Depositaire.ShowDialog();
		}

		private void bRESET_Click(object sender, EventArgs e)
		{
			Configuracion.Freeze_On();
			if (MessageBox.Show("Reset", "Stop", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				string path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\winvnc.exe";
				if (File.Exists(path) && Configuracion.VNC_Running())
				{
					Configuracion.VNC_Build_Timestamp();
				}
				Process.Start("shutdown.exe", "/r /t 2");
			}
		}

		private void bTouch_Click(object sender, EventArgs e)
		{
			DLG_Calibrar dLG_Calibrar = new DLG_Calibrar(ref opciones, 0);
			dLG_Calibrar.Focus();
			dLG_Calibrar.ShowDialog();
		}

		private void bMonitors_Click(object sender, EventArgs e)
		{
			DLG_Monitors dLG_Monitors = new DLG_Monitors(ref opciones);
			dLG_Monitors.Focus();
			dLG_Monitors.MWin = MWin;
			dLG_Monitors.ShowDialog();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Turn to Windows Mode", "Stop", MessageBoxButtons.YesNo) != DialogResult.Yes)
			{
				return;
			}
			DLG_Login dLG_Login = new DLG_Login(ref opciones, 1);
			Application.DoEvents();
			dLG_Login.Focus();
			dLG_Login.ShowDialog();
			int logeado = dLG_Login.Logeado;
			if (logeado == 1)
			{
				reboot = 1;
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
				string path = folderPath + "\\Toolwiz Time Freeze 2014\\ToolwizTimeFreeze.exe";
				if (File.Exists(path) && Configuracion.Freeze_Check() == 1)
				{
					Configuracion.Freeze_Build_Timestamp();
					Configuracion.Freeze_Build_TimestampBoot();
					Configuracion.Freeze_Off();
					Configuracion.WinReset();
				}
				else
				{
					MWin.UnLock_Windows();
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
						RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\WinLogon", writable: true);
						registryKey.SetValue("Shell", "explorer.exe", RegistryValueKind.String);
						registryKey.Close();
					}
					catch
					{
					}
					Configuracion.Access_Log("Turn Windows Mode");
					Configuracion.WinReset();
				}
			}
		}

		private void btn_mail_Click(object sender, EventArgs e)
		{
			opciones.SEND_Mail_Pub("STATUS", opciones.Update_Info());
		}

		private void bVNC_Click(object sender, EventArgs e)
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\winvnc.exe";
			if (File.Exists(text))
			{
				if (Configuracion.VNC_Running())
				{
					Process.Start(text, "-kill");
					Thread.Sleep(5000);
				}
				Process.Start(text, "-connect " + opciones.Server_VNC + ":5500 -run");
				MessageBox.Show("Waiting connection");
			}
			else
			{
				MessageBox.Show("No VNC Installed");
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\winvnc.exe";
			if (File.Exists(text))
			{
				if (Configuracion.VNC_Running())
				{
					Process.Start(text, "-kill");
				}
			}
			else
			{
				MessageBox.Show("No VNC Installed");
			}
		}

		private void bFreeze_Click(object sender, EventArgs e)
		{
			if (Configuracion.Freeze_Check() == 1)
			{
				if (MessageBox.Show("Turn to UNFREEZED MODE", "Stop", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					DLG_Login dLG_Login = new DLG_Login(ref opciones, 1);
					Application.DoEvents();
					dLG_Login.Focus();
					dLG_Login.ShowDialog();
					int logeado = dLG_Login.Logeado;
					if (logeado == 1)
					{
						Configuracion.Freeze_Build_Timestamp();
						Configuracion.Freeze_Off();
						Configuracion.WinReset();
					}
				}
			}
			else
			{
				MessageBox.Show("System is UNFREEZED");
			}
		}

		private void bWifi_Click(object sender, EventArgs e)
		{
			DLG_Wifi dLG_Wifi = new DLG_Wifi(ref opciones);
			dLG_Wifi.ShowDialog();
		}

		private void bOptions_KeyPress(object sender, KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
			case '\r':
			case '1':
				bOptions_Click(sender, null);
				break;
			case '2':
				bSponsor_Click(sender, null);
				break;
			case '3':
				bValidators_Click(sender, null);
				break;
			case '4':
				bPassword_Click(sender, null);
				break;
			case '5':
				bDepositaire_Click(sender, null);
				break;
			case '6':
				bMonitors_Click(sender, null);
				break;
			case 'W':
			case 'w':
				button2_Click(sender, null);
				break;
			case 'R':
			case 'r':
				bVNC_Click(sender, null);
				break;
			case 'S':
			case 's':
				bOK_Click(sender, null);
				break;
			case 'X':
			case 'x':
				bCancel_Click(sender, null);
				break;
			case 'T':
			case 't':
				bTouch_Click(sender, null);
				break;
			}
		}

		private void bSponsor_KeyPress(object sender, KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
			case '1':
				bOptions_Click(sender, null);
				break;
			case '\r':
			case '2':
				bSponsor_Click(sender, null);
				break;
			case '3':
				bValidators_Click(sender, null);
				break;
			case '4':
				bPassword_Click(sender, null);
				break;
			case '5':
				bDepositaire_Click(sender, null);
				break;
			case '6':
				bMonitors_Click(sender, null);
				break;
			case 'W':
			case 'w':
				button2_Click(sender, null);
				break;
			case 'R':
			case 'r':
				bVNC_Click(sender, null);
				break;
			case 'S':
			case 's':
				bOK_Click(sender, null);
				break;
			case 'X':
			case 'x':
				bCancel_Click(sender, null);
				break;
			case 'T':
			case 't':
				bTouch_Click(sender, null);
				break;
			}
		}

		private void bValidators_KeyPress(object sender, KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
			case '1':
				bOptions_Click(sender, null);
				break;
			case '2':
				bSponsor_Click(sender, null);
				break;
			case '\r':
			case '3':
				bValidators_Click(sender, null);
				break;
			case '4':
				bPassword_Click(sender, null);
				break;
			case '5':
				bDepositaire_Click(sender, null);
				break;
			case '6':
				bMonitors_Click(sender, null);
				break;
			case 'W':
			case 'w':
				button2_Click(sender, null);
				break;
			case 'R':
			case 'r':
				bVNC_Click(sender, null);
				break;
			case 'S':
			case 's':
				bOK_Click(sender, null);
				break;
			case 'X':
			case 'x':
				bCancel_Click(sender, null);
				break;
			case 'T':
			case 't':
				bTouch_Click(sender, null);
				break;
			}
		}

		private void bPassword_KeyPress(object sender, KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
			case '1':
				bOptions_Click(sender, null);
				break;
			case '2':
				bSponsor_Click(sender, null);
				break;
			case '3':
				bValidators_Click(sender, null);
				break;
			case '\r':
			case '4':
				bPassword_Click(sender, null);
				break;
			case '5':
				bDepositaire_Click(sender, null);
				break;
			case '6':
				bMonitors_Click(sender, null);
				break;
			case 'W':
			case 'w':
				button2_Click(sender, null);
				break;
			case 'R':
			case 'r':
				bVNC_Click(sender, null);
				break;
			case 'S':
			case 's':
				bOK_Click(sender, null);
				break;
			case 'X':
			case 'x':
				bCancel_Click(sender, null);
				break;
			case 'T':
			case 't':
				bTouch_Click(sender, null);
				break;
			}
		}

		private void bDepositaire_KeyPress(object sender, KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
			case '1':
				bOptions_Click(sender, null);
				break;
			case '2':
				bSponsor_Click(sender, null);
				break;
			case '3':
				bValidators_Click(sender, null);
				break;
			case '4':
				bPassword_Click(sender, null);
				break;
			case '\r':
			case '5':
				bDepositaire_Click(sender, null);
				break;
			case '6':
				bMonitors_Click(sender, null);
				break;
			case 'W':
			case 'w':
				button2_Click(sender, null);
				break;
			case 'R':
			case 'r':
				bVNC_Click(sender, null);
				break;
			case 'S':
			case 's':
				bOK_Click(sender, null);
				break;
			case 'X':
			case 'x':
				bCancel_Click(sender, null);
				break;
			case 'T':
			case 't':
				bTouch_Click(sender, null);
				break;
			}
		}

		private void bMonitors_KeyPress(object sender, KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
			case '1':
				bOptions_Click(sender, null);
				break;
			case '2':
				bSponsor_Click(sender, null);
				break;
			case '3':
				bValidators_Click(sender, null);
				break;
			case '4':
				bPassword_Click(sender, null);
				break;
			case '5':
				bDepositaire_Click(sender, null);
				break;
			case '\r':
			case '6':
				bMonitors_Click(sender, null);
				break;
			case 'W':
			case 'w':
				button2_Click(sender, null);
				break;
			case 'R':
			case 'r':
				bVNC_Click(sender, null);
				break;
			case 'S':
			case 's':
				bOK_Click(sender, null);
				break;
			case 'X':
			case 'x':
				bCancel_Click(sender, null);
				break;
			case 'T':
			case 't':
				bTouch_Click(sender, null);
				break;
			}
		}

		private void DLG_All_KeyPress(object sender, KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
			case '1':
				bOptions_Click(sender, null);
				break;
			case '2':
				bSponsor_Click(sender, null);
				break;
			case '3':
				bValidators_Click(sender, null);
				break;
			case '4':
				bPassword_Click(sender, null);
				break;
			case '5':
				bDepositaire_Click(sender, null);
				break;
			case '6':
				bMonitors_Click(sender, null);
				break;
			case 'W':
			case 'w':
				button2_Click(sender, null);
				break;
			case 'R':
			case 'r':
				bVNC_Click(sender, null);
				break;
			case 'S':
			case 's':
				bOK_Click(sender, null);
				break;
			case 'X':
			case 'x':
				bCancel_Click(sender, null);
				break;
			case 'T':
			case 't':
				bTouch_Click(sender, null);
				break;
			}
		}

		private void btnTICKET_Click(object sender, EventArgs e)
		{
			DLG_Dispenser dLG_Dispenser = new DLG_Dispenser(ref opciones);
			dLG_Dispenser.ShowDialog();
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
			lInfo = new System.Windows.Forms.Label();
			btnTICKET = new System.Windows.Forms.Button();
			bWifi = new System.Windows.Forms.Button();
			bFreeze = new System.Windows.Forms.Button();
			bVNC2 = new System.Windows.Forms.Button();
			bVNC = new System.Windows.Forms.Button();
			btn_mail = new System.Windows.Forms.Button();
			button2 = new System.Windows.Forms.Button();
			bMonitors = new System.Windows.Forms.Button();
			bTouch = new System.Windows.Forms.Button();
			bRESET = new System.Windows.Forms.Button();
			button1 = new System.Windows.Forms.Button();
			bDepositaire = new System.Windows.Forms.Button();
			bKey = new System.Windows.Forms.Button();
			bMin = new System.Windows.Forms.Button();
			bExplorer = new System.Windows.Forms.Button();
			bPanel = new System.Windows.Forms.Button();
			bOptions = new System.Windows.Forms.Button();
			bPassword = new System.Windows.Forms.Button();
			bValidators = new System.Windows.Forms.Button();
			bSponsor = new System.Windows.Forms.Button();
			bCancel = new System.Windows.Forms.Button();
			bOK = new System.Windows.Forms.Button();
			SuspendLayout();
			lInfo.AutoSize = true;
			lInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lInfo.Location = new System.Drawing.Point(9, 309);
			lInfo.Name = "lInfo";
			lInfo.Size = new System.Drawing.Size(14, 20);
			lInfo.TabIndex = 17;
			lInfo.Text = "-";
			btnTICKET.Image = Kiosk.Properties.Resources.Ticket;
			btnTICKET.Location = new System.Drawing.Point(207, 208);
			btnTICKET.Name = "btnTICKET";
			btnTICKET.Size = new System.Drawing.Size(189, 49);
			btnTICKET.TabIndex = 21;
			btnTICKET.UseVisualStyleBackColor = true;
			btnTICKET.Click += new System.EventHandler(btnTICKET_Click);
			bWifi.Image = Kiosk.Properties.Resources.ico_wifi_connect;
			bWifi.Location = new System.Drawing.Point(293, 334);
			bWifi.Name = "bWifi";
			bWifi.Size = new System.Drawing.Size(48, 48);
			bWifi.TabIndex = 12;
			bWifi.UseVisualStyleBackColor = true;
			bWifi.Click += new System.EventHandler(bWifi_Click);
			bWifi.KeyPress += new System.Windows.Forms.KeyPressEventHandler(DLG_All_KeyPress);
			bFreeze.Image = Kiosk.Properties.Resources.ico_lock_open;
			bFreeze.Location = new System.Drawing.Point(348, 280);
			bFreeze.Name = "bFreeze";
			bFreeze.Size = new System.Drawing.Size(48, 48);
			bFreeze.TabIndex = 20;
			bFreeze.TabStop = false;
			bFreeze.UseVisualStyleBackColor = true;
			bFreeze.Click += new System.EventHandler(bFreeze_Click);
			bVNC2.Image = Kiosk.Properties.Resources.ico_monitors_del;
			bVNC2.Location = new System.Drawing.Point(229, 394);
			bVNC2.Name = "bVNC2";
			bVNC2.Size = new System.Drawing.Size(48, 48);
			bVNC2.TabIndex = 18;
			bVNC2.UseVisualStyleBackColor = true;
			bVNC2.Visible = false;
			bVNC2.Click += new System.EventHandler(button3_Click);
			bVNC2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(DLG_All_KeyPress);
			bVNC.Image = Kiosk.Properties.Resources.ico_monitors;
			bVNC.Location = new System.Drawing.Point(175, 394);
			bVNC.Name = "bVNC";
			bVNC.Size = new System.Drawing.Size(48, 48);
			bVNC.TabIndex = 17;
			bVNC.Text = "R";
			bVNC.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			bVNC.UseVisualStyleBackColor = true;
			bVNC.Visible = false;
			bVNC.Click += new System.EventHandler(bVNC_Click);
			bVNC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(DLG_All_KeyPress);
			btn_mail.Image = Kiosk.Properties.Resources.ico_at;
			btn_mail.Location = new System.Drawing.Point(121, 394);
			btn_mail.Name = "btn_mail";
			btn_mail.Size = new System.Drawing.Size(48, 48);
			btn_mail.TabIndex = 16;
			btn_mail.UseVisualStyleBackColor = true;
			btn_mail.Click += new System.EventHandler(btn_mail_Click);
			btn_mail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(DLG_All_KeyPress);
			button2.Image = Kiosk.Properties.Resources.ico_modewin;
			button2.Location = new System.Drawing.Point(67, 394);
			button2.Name = "button2";
			button2.Size = new System.Drawing.Size(48, 48);
			button2.TabIndex = 15;
			button2.Text = "W";
			button2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			button2.UseVisualStyleBackColor = true;
			button2.Click += new System.EventHandler(button2_Click);
			button2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(DLG_All_KeyPress);
			bMonitors.Image = Kiosk.Properties.Resources.ico_monitors;
			bMonitors.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			bMonitors.Location = new System.Drawing.Point(13, 257);
			bMonitors.Name = "bMonitors";
			bMonitors.Size = new System.Drawing.Size(383, 49);
			bMonitors.TabIndex = 6;
			bMonitors.UseVisualStyleBackColor = true;
			bMonitors.Click += new System.EventHandler(bMonitors_Click);
			bMonitors.KeyPress += new System.Windows.Forms.KeyPressEventHandler(bMonitors_KeyPress);
			bTouch.Image = Kiosk.Properties.Resources.ico_touch;
			bTouch.Location = new System.Drawing.Point(347, 334);
			bTouch.Name = "bTouch";
			bTouch.Size = new System.Drawing.Size(48, 48);
			bTouch.TabIndex = 13;
			bTouch.Text = "T";
			bTouch.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			bTouch.UseVisualStyleBackColor = true;
			bTouch.Click += new System.EventHandler(bTouch_Click);
			bTouch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(DLG_All_KeyPress);
			bRESET.Image = Kiosk.Properties.Resources.ico_poff;
			bRESET.Location = new System.Drawing.Point(13, 394);
			bRESET.Name = "bRESET";
			bRESET.Size = new System.Drawing.Size(48, 48);
			bRESET.TabIndex = 14;
			bRESET.UseVisualStyleBackColor = true;
			bRESET.Click += new System.EventHandler(bRESET_Click);
			bRESET.KeyPress += new System.Windows.Forms.KeyPressEventHandler(DLG_All_KeyPress);
			button1.Image = Kiosk.Properties.Resources.ico_net;
			button1.Location = new System.Drawing.Point(229, 334);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(48, 48);
			button1.TabIndex = 11;
			button1.UseVisualStyleBackColor = true;
			button1.Click += new System.EventHandler(button1_Click);
			button1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(DLG_All_KeyPress);
			bDepositaire.Image = Kiosk.Properties.Resources.ico_note;
			bDepositaire.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			bDepositaire.Location = new System.Drawing.Point(13, 208);
			bDepositaire.Name = "bDepositaire";
			bDepositaire.Size = new System.Drawing.Size(195, 49);
			bDepositaire.TabIndex = 5;
			bDepositaire.UseVisualStyleBackColor = true;
			bDepositaire.Click += new System.EventHandler(bDepositaire_Click);
			bDepositaire.KeyPress += new System.Windows.Forms.KeyPressEventHandler(bDepositaire_KeyPress);
			bKey.Image = Kiosk.Properties.Resources.ico_keyboard;
			bKey.Location = new System.Drawing.Point(175, 334);
			bKey.Name = "bKey";
			bKey.Size = new System.Drawing.Size(48, 48);
			bKey.TabIndex = 10;
			bKey.UseVisualStyleBackColor = true;
			bKey.Click += new System.EventHandler(bKey_Click);
			bKey.KeyPress += new System.Windows.Forms.KeyPressEventHandler(DLG_All_KeyPress);
			bMin.Image = Kiosk.Properties.Resources.ico_windows;
			bMin.Location = new System.Drawing.Point(121, 334);
			bMin.Name = "bMin";
			bMin.Size = new System.Drawing.Size(48, 48);
			bMin.TabIndex = 9;
			bMin.UseVisualStyleBackColor = true;
			bMin.Click += new System.EventHandler(bMin_Click);
			bMin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(DLG_All_KeyPress);
			bExplorer.Image = Kiosk.Properties.Resources.ico_fexp;
			bExplorer.Location = new System.Drawing.Point(67, 334);
			bExplorer.Name = "bExplorer";
			bExplorer.Size = new System.Drawing.Size(48, 48);
			bExplorer.TabIndex = 8;
			bExplorer.UseVisualStyleBackColor = true;
			bExplorer.Click += new System.EventHandler(bExplorer_Click);
			bExplorer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(DLG_All_KeyPress);
			bPanel.Image = Kiosk.Properties.Resources.ico_gear;
			bPanel.Location = new System.Drawing.Point(13, 334);
			bPanel.Name = "bPanel";
			bPanel.Size = new System.Drawing.Size(48, 48);
			bPanel.TabIndex = 7;
			bPanel.UseVisualStyleBackColor = true;
			bPanel.Click += new System.EventHandler(bPanel_Click);
			bPanel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(DLG_All_KeyPress);
			bOptions.Image = Kiosk.Properties.Resources.ico_preferences;
			bOptions.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			bOptions.Location = new System.Drawing.Point(13, 12);
			bOptions.Name = "bOptions";
			bOptions.Size = new System.Drawing.Size(383, 49);
			bOptions.TabIndex = 1;
			bOptions.UseVisualStyleBackColor = true;
			bOptions.Click += new System.EventHandler(bOptions_Click);
			bOptions.KeyPress += new System.Windows.Forms.KeyPressEventHandler(bOptions_KeyPress);
			bPassword.Image = Kiosk.Properties.Resources.ico_key;
			bPassword.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			bPassword.Location = new System.Drawing.Point(13, 159);
			bPassword.Name = "bPassword";
			bPassword.Size = new System.Drawing.Size(383, 49);
			bPassword.TabIndex = 4;
			bPassword.UseVisualStyleBackColor = true;
			bPassword.Click += new System.EventHandler(bPassword_Click);
			bPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(bPassword_KeyPress);
			bValidators.Image = Kiosk.Properties.Resources.ico_money;
			bValidators.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			bValidators.Location = new System.Drawing.Point(13, 110);
			bValidators.Name = "bValidators";
			bValidators.Size = new System.Drawing.Size(383, 49);
			bValidators.TabIndex = 3;
			bValidators.UseVisualStyleBackColor = true;
			bValidators.Click += new System.EventHandler(bValidators_Click);
			bValidators.KeyPress += new System.Windows.Forms.KeyPressEventHandler(bValidators_KeyPress);
			bSponsor.Image = Kiosk.Properties.Resources.ico_at;
			bSponsor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			bSponsor.Location = new System.Drawing.Point(13, 61);
			bSponsor.Name = "bSponsor";
			bSponsor.Size = new System.Drawing.Size(383, 49);
			bSponsor.TabIndex = 2;
			bSponsor.UseVisualStyleBackColor = true;
			bSponsor.Click += new System.EventHandler(bSponsor_Click);
			bSponsor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(bSponsor_KeyPress);
			bCancel.Image = Kiosk.Properties.Resources.ico_del;
			bCancel.Location = new System.Drawing.Point(294, 394);
			bCancel.Name = "bCancel";
			bCancel.Size = new System.Drawing.Size(48, 48);
			bCancel.TabIndex = 19;
			bCancel.Text = "X";
			bCancel.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			bCancel.UseVisualStyleBackColor = true;
			bCancel.Click += new System.EventHandler(bCancel_Click);
			bCancel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(DLG_All_KeyPress);
			bOK.Image = Kiosk.Properties.Resources.ico_ok;
			bOK.Location = new System.Drawing.Point(348, 394);
			bOK.Name = "bOK";
			bOK.Size = new System.Drawing.Size(48, 48);
			bOK.TabIndex = 0;
			bOK.Text = "S";
			bOK.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			bOK.UseVisualStyleBackColor = true;
			bOK.Click += new System.EventHandler(bOK_Click);
			bOK.KeyPress += new System.Windows.Forms.KeyPressEventHandler(DLG_All_KeyPress);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(406, 457);
			base.ControlBox = false;
			base.Controls.Add(btnTICKET);
			base.Controls.Add(bWifi);
			base.Controls.Add(bFreeze);
			base.Controls.Add(bVNC2);
			base.Controls.Add(bVNC);
			base.Controls.Add(lInfo);
			base.Controls.Add(btn_mail);
			base.Controls.Add(button2);
			base.Controls.Add(bMonitors);
			base.Controls.Add(bTouch);
			base.Controls.Add(bRESET);
			base.Controls.Add(button1);
			base.Controls.Add(bDepositaire);
			base.Controls.Add(bKey);
			base.Controls.Add(bMin);
			base.Controls.Add(bExplorer);
			base.Controls.Add(bPanel);
			base.Controls.Add(bOptions);
			base.Controls.Add(bPassword);
			base.Controls.Add(bValidators);
			base.Controls.Add(bSponsor);
			base.Controls.Add(bCancel);
			base.Controls.Add(bOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "DLG_Config";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "0";
			base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(DLG_Config_FormClosed);
			base.Load += new System.EventHandler(DLG_Config_Load);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
