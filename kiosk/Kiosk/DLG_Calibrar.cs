using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Kiosk
{
	public class DLG_Calibrar : Form
	{
		public bool OK;

		public Configuracion opciones;

		private IContainer components = null;

		private Button bOK;

		private Button bGALAX;

		private Button b3M;

		private Button eELO;

		private Button bETWO;

		private Button bGEN;

		private Button bTALENT;

		private Button bASUS;

		private Button bRESET;

		private Button bOFF;

		private Label lINFO;

		private Button bClock;

		public DLG_Calibrar(ref Configuracion _opc, int _modo)
		{
			OK = false;
			opciones = _opc;
			InitializeComponent();
			Localize();
			string path = "c:\\Windows\\TouchUSM\\TouchCali.exe";
			if (!File.Exists(path))
			{
				bTALENT.Enabled = false;
			}
			path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Elo TouchSystems\\EloVa.exe";
			if (!File.Exists(path))
			{
				eELO.Enabled = false;
			}
			path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\ETSerTouch\\etsercalib.exe";
			if (!File.Exists(path))
			{
				bETWO.Enabled = false;
			}
			path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\MicroTouch\\MT 7\\TwCalib.exe";
			if (!File.Exists(path))
			{
				b3M.Enabled = false;
			}
			path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\eGalaxTouch\\xAuto4PtsCal.exe";
			if (!File.Exists(path))
			{
				bGALAX.Enabled = false;
			}
			path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\GeneralTouch\\APP\\x86\\GenCalib.exe";
			if (!File.Exists(path))
			{
				bGEN.Enabled = false;
			}
			path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Touch Package\\Touchpack.exe";
			if (!File.Exists(path))
			{
				bASUS.Enabled = false;
			}
			if (_modo == 0)
			{
				bRESET.Visible = false;
			}
			else
			{
				bool flag = false;
				base.WindowState = FormWindowState.Maximized;
				base.FormBorderStyle = FormBorderStyle.None;
			}
			lINFO.Text = "Version " + opciones.VersionPRG + "\n[" + opciones.Srv_Ip + " - " + opciones.Srv_port + " - " + opciones.Srv_User + " - " + opciones.IDMAQUINA + "]\n";
		}

		private void Localize()
		{
			SuspendLayout();
			Text = opciones.Localize.Text("Touch Screen Calibration");
			ResumeLayout();
		}

		private void eELO_Click(object sender, EventArgs e)
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Elo TouchSystems\\EloVa.exe";
			if (File.Exists(text))
			{
				Process.Start(text);
			}
			else
			{
				MessageBox.Show("Missing: [" + text + "]");
			}
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void bETWO_Click(object sender, EventArgs e)
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\ETSerTouch\\etsercalib.exe";
			if (File.Exists(text))
			{
				Process.Start(text);
			}
			else
			{
				MessageBox.Show("Missing: [" + text + "]");
			}
		}

		private void b3M_Click(object sender, EventArgs e)
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\MicroTouch\\MT 7\\TwCalib.exe";
			if (File.Exists(text))
			{
				Process.Start(text);
			}
			else
			{
				MessageBox.Show("Missing: [" + text + "]");
			}
		}

		private void bGALAX_Click(object sender, EventArgs e)
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\eGalaxTouch\\xAuto4PtsCal.exe";
			if (File.Exists(text))
			{
				Process.Start(text);
			}
			else
			{
				MessageBox.Show("Missing: [" + text + "]");
			}
		}

		private void bGEN_Click(object sender, EventArgs e)
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\GeneralTouch\\APP\\x86\\GenCalib.exe";
			if (File.Exists(text))
			{
				Process.Start(text);
			}
			else
			{
				MessageBox.Show("Missing: [" + text + "]");
			}
		}

		private void bTALENT_Click(object sender, EventArgs e)
		{
			string text = "c:\\Windows\\TouchUSM\\TouchCali.exe";
			if (File.Exists(text))
			{
				Process.Start(text);
			}
			else
			{
				MessageBox.Show("Missing: [" + text + "]");
			}
		}

		private void bASUS_Click(object sender, EventArgs e)
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Touch Package\\Touchpack.exe";
			if (File.Exists(text))
			{
				Process.Start(text);
			}
			else
			{
				MessageBox.Show("Missing: [" + text + "]");
			}
		}

		private void bRESET_Click(object sender, EventArgs e)
		{
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

		private void DLG_Calibrar_KeyPress(object sender, KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
			case '1':
				eELO_Click(sender, null);
				break;
			case '2':
				b3M_Click(sender, null);
				break;
			case '3':
				bETWO_Click(sender, null);
				break;
			case '4':
				bGALAX_Click(sender, null);
				break;
			case '5':
				bGEN_Click(sender, null);
				break;
			case '6':
				bTALENT_Click(sender, null);
				break;
			case '7':
				bASUS_Click(sender, null);
				break;
			case '0':
				bRESET_Click(sender, null);
				break;
			}
		}

		private void DLG_Calibrar_KeyDown(object sender, KeyEventArgs e)
		{
			Keys keyCode = e.KeyCode;
			if (keyCode == Keys.F10)
			{
				Close();
			}
		}

		private void eELO_KeyPress(object sender, KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
			case '\r':
			case '1':
				eELO_Click(sender, null);
				break;
			case '2':
				b3M_Click(sender, null);
				break;
			case '3':
				bETWO_Click(sender, null);
				break;
			case '4':
				bGALAX_Click(sender, null);
				break;
			case '5':
				bGEN_Click(sender, null);
				break;
			case '6':
				bTALENT_Click(sender, null);
				break;
			case '7':
				bASUS_Click(sender, null);
				break;
			case '0':
				bRESET_Click(sender, null);
				break;
			}
		}

		private void bOFF_Click(object sender, EventArgs e)
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\winvnc.exe";
			if (File.Exists(path) && Configuracion.VNC_Running())
			{
				Configuracion.VNC_Build_Timestamp();
			}
			Process.Start("shutdown.exe", "/s /t 2");
		}

		private void b3M_KeyPress(object sender, KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
			case '1':
				eELO_Click(sender, null);
				break;
			case '\r':
			case '2':
				b3M_Click(sender, null);
				break;
			case '3':
				bETWO_Click(sender, null);
				break;
			case '4':
				bGALAX_Click(sender, null);
				break;
			case '5':
				bGEN_Click(sender, null);
				break;
			case '6':
				bTALENT_Click(sender, null);
				break;
			case '7':
				bASUS_Click(sender, null);
				break;
			case 'X':
			case 'x':
				bRESET_Click(sender, null);
				break;
			case 'T':
			case 't':
				bClock_Click(sender, null);
				break;
			}
		}

		private void bETWO_KeyPress(object sender, KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
			case '1':
				eELO_Click(sender, null);
				break;
			case '2':
				b3M_Click(sender, null);
				break;
			case '\r':
			case '3':
				bETWO_Click(sender, null);
				break;
			case '4':
				bGALAX_Click(sender, null);
				break;
			case '5':
				bGEN_Click(sender, null);
				break;
			case '6':
				bTALENT_Click(sender, null);
				break;
			case '7':
				bASUS_Click(sender, null);
				break;
			case 'X':
			case 'x':
				bRESET_Click(sender, null);
				break;
			case 'T':
			case 't':
				bClock_Click(sender, null);
				break;
			}
		}

		private void bGALAX_KeyPress(object sender, KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
			case '1':
				eELO_Click(sender, null);
				break;
			case '2':
				b3M_Click(sender, null);
				break;
			case '3':
				bETWO_Click(sender, null);
				break;
			case '\r':
			case '4':
				bGALAX_Click(sender, null);
				break;
			case '5':
				bGEN_Click(sender, null);
				break;
			case '6':
				bTALENT_Click(sender, null);
				break;
			case '7':
				bASUS_Click(sender, null);
				break;
			case 'X':
			case 'x':
				bRESET_Click(sender, null);
				break;
			case 'T':
			case 't':
				bClock_Click(sender, null);
				break;
			}
		}

		private void bGEN_KeyPress(object sender, KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
			case '1':
				eELO_Click(sender, null);
				break;
			case '2':
				b3M_Click(sender, null);
				break;
			case '3':
				bETWO_Click(sender, null);
				break;
			case '4':
				bGALAX_Click(sender, null);
				break;
			case '\r':
			case '5':
				bGEN_Click(sender, null);
				break;
			case '6':
				bTALENT_Click(sender, null);
				break;
			case '7':
				bASUS_Click(sender, null);
				break;
			case 'X':
			case 'x':
				bRESET_Click(sender, null);
				break;
			case 'T':
			case 't':
				bClock_Click(sender, null);
				break;
			}
		}

		private void bTALENT_KeyPress(object sender, KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
			case '1':
				eELO_Click(sender, null);
				break;
			case '2':
				b3M_Click(sender, null);
				break;
			case '3':
				bETWO_Click(sender, null);
				break;
			case '4':
				bGALAX_Click(sender, null);
				break;
			case '5':
				bGEN_Click(sender, null);
				break;
			case '\r':
			case '6':
				bTALENT_Click(sender, null);
				break;
			case '7':
				bASUS_Click(sender, null);
				break;
			case 'X':
			case 'x':
				bRESET_Click(sender, null);
				break;
			case 'T':
			case 't':
				bClock_Click(sender, null);
				break;
			}
		}

		private void bASUS_KeyPress(object sender, KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
			case '1':
				eELO_Click(sender, null);
				break;
			case '2':
				b3M_Click(sender, null);
				break;
			case '3':
				bETWO_Click(sender, null);
				break;
			case '4':
				bGALAX_Click(sender, null);
				break;
			case '5':
				bGEN_Click(sender, null);
				break;
			case '6':
				bTALENT_Click(sender, null);
				break;
			case '\r':
			case '7':
				bASUS_Click(sender, null);
				break;
			case 'X':
			case 'x':
				bRESET_Click(sender, null);
				break;
			case 'T':
			case 't':
				bClock_Click(sender, null);
				break;
			}
		}

		private void bOFF_KeyPress(object sender, KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
			case '1':
				eELO_Click(sender, null);
				break;
			case '2':
				b3M_Click(sender, null);
				break;
			case '3':
				bETWO_Click(sender, null);
				break;
			case '4':
				bGALAX_Click(sender, null);
				break;
			case '5':
				bGEN_Click(sender, null);
				break;
			case '6':
				bTALENT_Click(sender, null);
				break;
			case '7':
				bASUS_Click(sender, null);
				break;
			case 'X':
			case 'x':
				bRESET_Click(sender, null);
				break;
			case '\r':
				bOFF_Click(sender, null);
				break;
			case 'T':
			case 't':
				bClock_Click(sender, null);
				break;
			}
		}

		private void bRESET_KeyPress(object sender, KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
			case '1':
				eELO_Click(sender, null);
				break;
			case '2':
				b3M_Click(sender, null);
				break;
			case '3':
				bETWO_Click(sender, null);
				break;
			case '4':
				bGALAX_Click(sender, null);
				break;
			case '5':
				bGEN_Click(sender, null);
				break;
			case '6':
				bTALENT_Click(sender, null);
				break;
			case '7':
				bASUS_Click(sender, null);
				break;
			case '\r':
			case 'X':
			case 'x':
				bRESET_Click(sender, null);
				break;
			case 'T':
			case 't':
				bClock_Click(sender, null);
				break;
			}
		}

		private void bOK_KeyPress(object sender, KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
			case '1':
				eELO_Click(sender, null);
				break;
			case '2':
				b3M_Click(sender, null);
				break;
			case '3':
				bETWO_Click(sender, null);
				break;
			case '4':
				bGALAX_Click(sender, null);
				break;
			case '5':
				bGEN_Click(sender, null);
				break;
			case '6':
				bTALENT_Click(sender, null);
				break;
			case '7':
				bASUS_Click(sender, null);
				break;
			case 'X':
			case 'x':
				bRESET_Click(sender, null);
				break;
			case '\r':
				bOK_Click(sender, null);
				break;
			case 'T':
			case 't':
				bClock_Click(sender, null);
				break;
			}
		}

		private void bClock_Click(object sender, EventArgs e)
		{
			MSGBOX_Timer mSGBOX_Timer = new MSGBOX_Timer("Updating Date/Time from Internet server. Wait 30 seconds", "", 30, _full: true);
			mSGBOX_Timer.Show();
			Program.Update_DateTime();
			Thread.Sleep(30000);
			mSGBOX_Timer.Close();
			MSGBOX_Timer mSGBOX_Timer2 = new MSGBOX_Timer("Current Date/Time " + DateTime.Now.ToLongDateString(), "", 5, _full: true);
			mSGBOX_Timer2.ShowDialog();
		}

		private void bClock_KeyPress(object sender, KeyPressEventArgs e)
		{
			switch (e.KeyChar)
			{
			case '1':
				eELO_Click(sender, null);
				break;
			case '2':
				b3M_Click(sender, null);
				break;
			case '3':
				bETWO_Click(sender, null);
				break;
			case '4':
				bGALAX_Click(sender, null);
				break;
			case '5':
				bGEN_Click(sender, null);
				break;
			case '6':
				bTALENT_Click(sender, null);
				break;
			case '7':
				bASUS_Click(sender, null);
				break;
			case 'X':
			case 'x':
				bRESET_Click(sender, null);
				break;
			case '\r':
			case 'T':
			case 't':
				bClock_Click(sender, null);
				break;
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
			lINFO = new System.Windows.Forms.Label();
			bClock = new System.Windows.Forms.Button();
			bOFF = new System.Windows.Forms.Button();
			bRESET = new System.Windows.Forms.Button();
			bASUS = new System.Windows.Forms.Button();
			bTALENT = new System.Windows.Forms.Button();
			bGEN = new System.Windows.Forms.Button();
			bETWO = new System.Windows.Forms.Button();
			eELO = new System.Windows.Forms.Button();
			b3M = new System.Windows.Forms.Button();
			bGALAX = new System.Windows.Forms.Button();
			bOK = new System.Windows.Forms.Button();
			SuspendLayout();
			lINFO.AutoSize = true;
			lINFO.Font = new System.Drawing.Font("Microsoft Sans Serif", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lINFO.Location = new System.Drawing.Point(13, 13);
			lINFO.Name = "lINFO";
			lINFO.Size = new System.Drawing.Size(19, 26);
			lINFO.TabIndex = 10;
			lINFO.Text = "-";
			bClock.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			bClock.Image = Kiosk.Properties.Resources.Clock;
			bClock.Location = new System.Drawing.Point(141, 170);
			bClock.Name = "bClock";
			bClock.Size = new System.Drawing.Size(110, 96);
			bClock.TabIndex = 11;
			bClock.Text = "Set Time (T)";
			bClock.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			bClock.UseVisualStyleBackColor = true;
			bClock.Click += new System.EventHandler(bClock_Click);
			bClock.KeyPress += new System.Windows.Forms.KeyPressEventHandler(bClock_KeyPress);
			bOFF.BackColor = System.Drawing.Color.Orange;
			bOFF.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			bOFF.Image = Kiosk.Properties.Resources.ico_poff;
			bOFF.Location = new System.Drawing.Point(257, 170);
			bOFF.Name = "bOFF";
			bOFF.Size = new System.Drawing.Size(110, 96);
			bOFF.TabIndex = 9;
			bOFF.Text = "Power OFF";
			bOFF.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			bOFF.UseVisualStyleBackColor = false;
			bOFF.Click += new System.EventHandler(bOFF_Click);
			bOFF.KeyPress += new System.Windows.Forms.KeyPressEventHandler(bOFF_KeyPress);
			bRESET.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			bRESET.Image = Kiosk.Properties.Resources.ico_poff;
			bRESET.Location = new System.Drawing.Point(367, 170);
			bRESET.Name = "bRESET";
			bRESET.Size = new System.Drawing.Size(110, 96);
			bRESET.TabIndex = 7;
			bRESET.Text = "Reboot (X)";
			bRESET.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			bRESET.UseVisualStyleBackColor = true;
			bRESET.Click += new System.EventHandler(bRESET_Click);
			bRESET.KeyPress += new System.Windows.Forms.KeyPressEventHandler(bRESET_KeyPress);
			bASUS.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			bASUS.Image = Kiosk.Properties.Resources.Asus;
			bASUS.Location = new System.Drawing.Point(12, 170);
			bASUS.Name = "bASUS";
			bASUS.Size = new System.Drawing.Size(96, 96);
			bASUS.TabIndex = 6;
			bASUS.Text = "7";
			bASUS.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			bASUS.UseVisualStyleBackColor = true;
			bASUS.Click += new System.EventHandler(bASUS_Click);
			bASUS.KeyPress += new System.Windows.Forms.KeyPressEventHandler(bASUS_KeyPress);
			bTALENT.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			bTALENT.Image = Kiosk.Properties.Resources.ico_ttouch;
			bTALENT.Location = new System.Drawing.Point(492, 68);
			bTALENT.Name = "bTALENT";
			bTALENT.Size = new System.Drawing.Size(96, 96);
			bTALENT.TabIndex = 5;
			bTALENT.Text = "6";
			bTALENT.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			bTALENT.UseVisualStyleBackColor = true;
			bTALENT.Click += new System.EventHandler(bTALENT_Click);
			bTALENT.KeyPress += new System.Windows.Forms.KeyPressEventHandler(bTALENT_KeyPress);
			bGEN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			bGEN.Image = Kiosk.Properties.Resources.ico_general;
			bGEN.Location = new System.Drawing.Point(396, 68);
			bGEN.Name = "bGEN";
			bGEN.Size = new System.Drawing.Size(96, 96);
			bGEN.TabIndex = 4;
			bGEN.Text = "5";
			bGEN.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			bGEN.UseVisualStyleBackColor = true;
			bGEN.Click += new System.EventHandler(bGEN_Click);
			bGEN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(bGEN_KeyPress);
			bETWO.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			bETWO.Image = Kiosk.Properties.Resources.ico_etwo;
			bETWO.Location = new System.Drawing.Point(204, 68);
			bETWO.Name = "bETWO";
			bETWO.Size = new System.Drawing.Size(96, 96);
			bETWO.TabIndex = 2;
			bETWO.Text = "3";
			bETWO.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			bETWO.UseVisualStyleBackColor = true;
			bETWO.Click += new System.EventHandler(bETWO_Click);
			bETWO.KeyPress += new System.Windows.Forms.KeyPressEventHandler(bETWO_KeyPress);
			eELO.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			eELO.Image = Kiosk.Properties.Resources.ico_elo;
			eELO.Location = new System.Drawing.Point(12, 68);
			eELO.Name = "eELO";
			eELO.Size = new System.Drawing.Size(96, 96);
			eELO.TabIndex = 0;
			eELO.Text = "1";
			eELO.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			eELO.UseVisualStyleBackColor = true;
			eELO.Click += new System.EventHandler(eELO_Click);
			eELO.KeyPress += new System.Windows.Forms.KeyPressEventHandler(eELO_KeyPress);
			b3M.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			b3M.Image = Kiosk.Properties.Resources.ico_3m;
			b3M.Location = new System.Drawing.Point(108, 68);
			b3M.Name = "b3M";
			b3M.Size = new System.Drawing.Size(96, 96);
			b3M.TabIndex = 1;
			b3M.Text = "2";
			b3M.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			b3M.UseVisualStyleBackColor = true;
			b3M.Click += new System.EventHandler(b3M_Click);
			b3M.KeyPress += new System.Windows.Forms.KeyPressEventHandler(b3M_KeyPress);
			bGALAX.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			bGALAX.Image = Kiosk.Properties.Resources.ico_galax;
			bGALAX.Location = new System.Drawing.Point(300, 68);
			bGALAX.Name = "bGALAX";
			bGALAX.Size = new System.Drawing.Size(96, 96);
			bGALAX.TabIndex = 3;
			bGALAX.Text = "4";
			bGALAX.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			bGALAX.UseVisualStyleBackColor = true;
			bGALAX.Click += new System.EventHandler(bGALAX_Click);
			bGALAX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(bGALAX_KeyPress);
			bOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			bOK.Image = Kiosk.Properties.Resources.ico_ok;
			bOK.Location = new System.Drawing.Point(477, 170);
			bOK.Name = "bOK";
			bOK.Size = new System.Drawing.Size(110, 96);
			bOK.TabIndex = 8;
			bOK.Text = "Exit F10";
			bOK.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			bOK.UseVisualStyleBackColor = true;
			bOK.Click += new System.EventHandler(bOK_Click);
			bOK.KeyPress += new System.Windows.Forms.KeyPressEventHandler(bOK_KeyPress);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(598, 341);
			base.Controls.Add(bClock);
			base.Controls.Add(lINFO);
			base.Controls.Add(bOFF);
			base.Controls.Add(bRESET);
			base.Controls.Add(bASUS);
			base.Controls.Add(bTALENT);
			base.Controls.Add(bGEN);
			base.Controls.Add(bETWO);
			base.Controls.Add(eELO);
			base.Controls.Add(b3M);
			base.Controls.Add(bGALAX);
			base.Controls.Add(bOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "DLG_Calibrar";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Calibration";
			base.KeyDown += new System.Windows.Forms.KeyEventHandler(DLG_Calibrar_KeyDown);
			base.KeyPress += new System.Windows.Forms.KeyPressEventHandler(DLG_Calibrar_KeyPress);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
