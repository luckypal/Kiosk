using GLib;
using GLib.Config;
using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Kiosk
{
	public class DLG_Login : Form
	{
		public Configuracion opciones;

		public int Logeado;

		public bool OK;

		public int Qui;

		private IContainer components = null;

		private PasswordBox ePassword;

		private Label lPassword;

		private Button bOK;

		private Button bCancel;

		private Label lCtrl;

		private Button bUnlock;

		private Button nKey;

		private Timer tScan;

		public DLG_Login(ref Configuracion _opc, int _qui)
		{
			Qui = _qui;
			OK = false;
			InitializeComponent();
			opciones = _opc;
			Logeado = 0;
			Localize();
			opciones.Log_Debug("Try Login");
		}

		private void Localize()
		{
			SuspendLayout();
			Text = opciones.Localize.Text("Stop!");
			lPassword.Text = opciones.Localize.Text("Password");
			ResumeLayout();
		}

		private void bCancel_Click(object sender, EventArgs e)
		{
			Logeado = 0;
			Close();
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			OK = true;
			string text = ePassword.Text;
			Logeado = 0;
			string a = XmlConfig.X2Y(text);
			int qui = Qui;
			if (qui == 1)
			{
				if (a != "0" && a == XmlConfig.X2Y("gowindows"))
				{
					Logeado = 1;
				}
			}
			else if (a != "0" && a == opciones.PasswordADM && opciones.PasswordADM != "")
			{
				Logeado = 1;
			}
			Close();
		}

		[DllImport("User32.dll")]
		private static extern int SetFocus(IntPtr hWnd);

		private void DLG_Login_Load(object sender, EventArgs e)
		{
			Focus();
			opciones.LastMouseMove = DateTime.Now;
			tScan.Enabled = true;
		}

		private void ePassword_KeyDown(object sender, KeyEventArgs e)
		{
			if ((e.Modifiers & Keys.Control) == Keys.Control)
			{
				lCtrl.Text = "Control Pressed";
			}
			else
			{
				lCtrl.Text = "";
			}
		}

		private void ePassword_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				bOK_Click(sender, null);
			}
		}

		private void bUnlock_Click(object sender, EventArgs e)
		{
			opciones.ForceAllKey = 1;
		}

		private void nKey_Click(object sender, EventArgs e)
		{
			Process[] processesByName = Process.GetProcessesByName("KVKeyboard");
			if (processesByName.Length >= 1)
			{
				PipeClient pipeClient = new PipeClient();
				pipeClient.Send("QUIT", "KVKeyboard");
			}
			else
			{
				Process.Start("KVKeyboard.exe", "es");
			}
		}

		private void DLG_Login_Enter(object sender, EventArgs e)
		{
			ePassword.Focus();
		}

		private void tScan_Tick(object sender, EventArgs e)
		{
			int num = (int)(DateTime.Now - opciones.LastMouseMove).TotalSeconds;
			if (num > 60)
			{
				Logeado = 0;
				Close();
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
			components = new System.ComponentModel.Container();
			lPassword = new System.Windows.Forms.Label();
			bOK = new System.Windows.Forms.Button();
			bCancel = new System.Windows.Forms.Button();
			lCtrl = new System.Windows.Forms.Label();
			bUnlock = new System.Windows.Forms.Button();
			ePassword = new GLib.PasswordBox();
			nKey = new System.Windows.Forms.Button();
			tScan = new System.Windows.Forms.Timer(components);
			SuspendLayout();
			lPassword.AutoSize = true;
			lPassword.Location = new System.Drawing.Point(13, 14);
			lPassword.Name = "lPassword";
			lPassword.Size = new System.Drawing.Size(10, 13);
			lPassword.TabIndex = 3;
			lPassword.Text = "-";
			bOK.Image = Kiosk.Properties.Resources.ico_ok;
			bOK.Location = new System.Drawing.Point(232, 70);
			bOK.Name = "bOK";
			bOK.Size = new System.Drawing.Size(48, 48);
			bOK.TabIndex = 2;
			bOK.UseVisualStyleBackColor = true;
			bOK.Click += new System.EventHandler(bOK_Click);
			bCancel.Image = Kiosk.Properties.Resources.ico_del;
			bCancel.Location = new System.Drawing.Point(178, 70);
			bCancel.Name = "bCancel";
			bCancel.Size = new System.Drawing.Size(48, 48);
			bCancel.TabIndex = 1;
			bCancel.UseVisualStyleBackColor = true;
			bCancel.Click += new System.EventHandler(bCancel_Click);
			lCtrl.AutoSize = true;
			lCtrl.ForeColor = System.Drawing.Color.Red;
			lCtrl.Location = new System.Drawing.Point(13, 60);
			lCtrl.Name = "lCtrl";
			lCtrl.Size = new System.Drawing.Size(0, 13);
			lCtrl.TabIndex = 4;
			bUnlock.Image = Kiosk.Properties.Resources.ico_barcodeX;
			bUnlock.Location = new System.Drawing.Point(12, 70);
			bUnlock.Name = "bUnlock";
			bUnlock.Size = new System.Drawing.Size(48, 48);
			bUnlock.TabIndex = 3;
			bUnlock.UseVisualStyleBackColor = true;
			bUnlock.Click += new System.EventHandler(bUnlock_Click);
			ePassword.Location = new System.Drawing.Point(12, 35);
			ePassword.Name = "ePassword";
			ePassword.Size = new System.Drawing.Size(268, 20);
			ePassword.TabIndex = 0;
			ePassword.KeyDown += new System.Windows.Forms.KeyEventHandler(ePassword_KeyDown);
			ePassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ePassword_KeyPress);
			nKey.Image = Kiosk.Properties.Resources.ico_keyboard;
			nKey.Location = new System.Drawing.Point(66, 70);
			nKey.Name = "nKey";
			nKey.Size = new System.Drawing.Size(48, 48);
			nKey.TabIndex = 5;
			nKey.UseVisualStyleBackColor = true;
			nKey.Click += new System.EventHandler(nKey_Click);
			tScan.Interval = 500;
			tScan.Tick += new System.EventHandler(tScan_Tick);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(292, 130);
			base.ControlBox = false;
			base.Controls.Add(nKey);
			base.Controls.Add(bUnlock);
			base.Controls.Add(lCtrl);
			base.Controls.Add(bCancel);
			base.Controls.Add(bOK);
			base.Controls.Add(lPassword);
			base.Controls.Add(ePassword);
			DoubleBuffered = true;
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.Name = "DLG_Login";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Stop!";
			base.TopMost = true;
			base.Load += new System.EventHandler(DLG_Login_Load);
			base.Enter += new System.EventHandler(DLG_Login_Enter);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
