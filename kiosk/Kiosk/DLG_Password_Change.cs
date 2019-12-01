using GLib;
using GLib.Config;
using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
	public class DLG_Password_Change : Form
	{
		public Configuracion opciones;

		public bool OK;

		public bool Logeado;

		private int id;

		private IContainer components = null;

		private Button bCancel;

		private Button bOK;

		private Label lPasswordOld;

		private PasswordBox ePasswordOld;

		private Label lPasswordNew;

		private PasswordBox ePasswordNew;

		private Label lPasswordVer;

		private PasswordBox ePasswordVer;

		public DLG_Password_Change(ref Configuracion _opc, int _id)
		{
			OK = false;
			InitializeComponent();
			base.TopMost = true;
			opciones = _opc;
			id = _id;
			Logeado = false;
			Localize();
		}

		private void Localize()
		{
			SuspendLayout();
			Text = opciones.Localize.Text("Modify password");
			lPasswordNew.Text = opciones.Localize.Text("New password");
			lPasswordOld.Text = opciones.Localize.Text("Current password");
			lPasswordVer.Text = opciones.Localize.Text("Repeat new password");
			ResumeLayout();
		}

		private void bCancel_Click(object sender, EventArgs e)
		{
			Logeado = false;
			Close();
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			OK = true;
			string text = ePasswordOld.Text;
			int num = 0;
			Logeado = false;
			int num2 = id;
			if (num2 == 1)
			{
				string a = XmlConfig.X2Y(text);
				if (a != "0" && a == opciones.PasswordADM && opciones.PasswordADM != "" && ePasswordNew.Text == ePasswordVer.Text && ePasswordNew.Text != "")
				{
					opciones.PasswordADM = XmlConfig.X2Y(ePasswordNew.Text);
					num = 1;
					Configuracion.Access_Log("Password changed");
				}
			}
			if (num == 1)
			{
				opciones.Save_Net();
				MessageBox.Show("Password changed");
			}
			Logeado = true;
			Close();
		}

		private void DLG_Password_Change_Load(object sender, EventArgs e)
		{
			Focus();
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
			bCancel = new System.Windows.Forms.Button();
			bOK = new System.Windows.Forms.Button();
			lPasswordOld = new System.Windows.Forms.Label();
			lPasswordNew = new System.Windows.Forms.Label();
			lPasswordVer = new System.Windows.Forms.Label();
			ePasswordVer = new GLib.PasswordBox();
			ePasswordNew = new GLib.PasswordBox();
			ePasswordOld = new GLib.PasswordBox();
			SuspendLayout();
			bCancel.Image = Kiosk.Properties.Resources.ico_del;
			bCancel.Location = new System.Drawing.Point(178, 167);
			bCancel.Name = "bCancel";
			bCancel.Size = new System.Drawing.Size(48, 48);
			bCancel.TabIndex = 4;
			bCancel.UseVisualStyleBackColor = true;
			bCancel.Click += new System.EventHandler(bCancel_Click);
			bOK.Image = Kiosk.Properties.Resources.ico_ok;
			bOK.Location = new System.Drawing.Point(232, 167);
			bOK.Name = "bOK";
			bOK.Size = new System.Drawing.Size(48, 48);
			bOK.TabIndex = 3;
			bOK.UseVisualStyleBackColor = true;
			bOK.Click += new System.EventHandler(bOK_Click);
			lPasswordOld.AutoSize = true;
			lPasswordOld.Location = new System.Drawing.Point(13, 7);
			lPasswordOld.Name = "lPasswordOld";
			lPasswordOld.Size = new System.Drawing.Size(10, 13);
			lPasswordOld.TabIndex = 5;
			lPasswordOld.Text = "-";
			lPasswordNew.AutoSize = true;
			lPasswordNew.Location = new System.Drawing.Point(13, 56);
			lPasswordNew.Name = "lPasswordNew";
			lPasswordNew.Size = new System.Drawing.Size(10, 13);
			lPasswordNew.TabIndex = 6;
			lPasswordNew.Text = "-";
			lPasswordVer.AutoSize = true;
			lPasswordVer.Location = new System.Drawing.Point(13, 105);
			lPasswordVer.Name = "lPasswordVer";
			lPasswordVer.Size = new System.Drawing.Size(10, 13);
			lPasswordVer.TabIndex = 7;
			lPasswordVer.Text = "-";
			ePasswordVer.Location = new System.Drawing.Point(12, 126);
			ePasswordVer.Name = "ePasswordVer";
			ePasswordVer.Size = new System.Drawing.Size(268, 20);
			ePasswordVer.TabIndex = 2;
			ePasswordNew.Location = new System.Drawing.Point(12, 77);
			ePasswordNew.Name = "ePasswordNew";
			ePasswordNew.Size = new System.Drawing.Size(268, 20);
			ePasswordNew.TabIndex = 1;
			ePasswordOld.Location = new System.Drawing.Point(12, 28);
			ePasswordOld.Name = "ePasswordOld";
			ePasswordOld.Size = new System.Drawing.Size(268, 20);
			ePasswordOld.TabIndex = 0;
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(292, 227);
			base.ControlBox = false;
			base.Controls.Add(lPasswordVer);
			base.Controls.Add(ePasswordVer);
			base.Controls.Add(lPasswordNew);
			base.Controls.Add(ePasswordNew);
			base.Controls.Add(bCancel);
			base.Controls.Add(bOK);
			base.Controls.Add(lPasswordOld);
			base.Controls.Add(ePasswordOld);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "DLG_Password_Change";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Modify password";
			base.TopMost = true;
			base.Load += new System.EventHandler(DLG_Password_Change_Load);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
