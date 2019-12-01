using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
	public class DLG_New_User : Form
	{
		public string Web;

		public string User;

		public string Password;

		public bool OK;

		public Configuracion opciones;

		private IContainer components = null;

		private Button bCancel;

		private Button bOK;

		private Label lWeb;

		private TextBox tWeb;

		private Label lLogin;

		private TextBox tLogin;

		private TextBox tPassword;

		private Label lPassword;

		private Button bServer;

		public DLG_New_User(ref Configuracion _opc)
		{
			OK = false;
			opciones = _opc;
			InitializeComponent();
			tWeb.Text = _opc.Srv_Ip;
			tLogin.Text = _opc.Srv_User;
			tPassword.Text = _opc.Srv_User_P;
			Localize();
		}

		private void Localize()
		{
			SuspendLayout();
			Text = opciones.Localize.Text("Register");
			lWeb.Text = opciones.Localize.Text("Web");
			lLogin.Text = opciones.Localize.Text("Login");
			lPassword.Text = opciones.Localize.Text("Password");
			ResumeLayout();
		}

		public void Update_Info(string _u, string _p, string _w)
		{
			Web = _w;
			User = _u;
			Password = _p;
			tWeb.Text = Web;
			tLogin.Text = User;
			tPassword.Text = Password;
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			Web = tWeb.Text;
			User = tLogin.Text;
			Password = tPassword.Text;
			OK = true;
			Close();
		}

		private void bCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void bTest_Click(object sender, EventArgs e)
		{
		}

		private void bServer_Click(object sender, EventArgs e)
		{
			tWeb.Visible = true;
			lWeb.Visible = true;
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
			lWeb = new System.Windows.Forms.Label();
			tWeb = new System.Windows.Forms.TextBox();
			lLogin = new System.Windows.Forms.Label();
			tLogin = new System.Windows.Forms.TextBox();
			tPassword = new System.Windows.Forms.TextBox();
			lPassword = new System.Windows.Forms.Label();
			bServer = new System.Windows.Forms.Button();
			SuspendLayout();
			bCancel.Image = Kiosk.Properties.Resources.ico_del;
			bCancel.Location = new System.Drawing.Point(290, 171);
			bCancel.Name = "bCancel";
			bCancel.Size = new System.Drawing.Size(48, 48);
			bCancel.TabIndex = 4;
			bCancel.UseVisualStyleBackColor = true;
			bCancel.Click += new System.EventHandler(bCancel_Click);
			bOK.Image = Kiosk.Properties.Resources.ico_ok;
			bOK.Location = new System.Drawing.Point(344, 171);
			bOK.Name = "bOK";
			bOK.Size = new System.Drawing.Size(48, 48);
			bOK.TabIndex = 3;
			bOK.UseVisualStyleBackColor = true;
			bOK.Click += new System.EventHandler(bOK_Click);
			lWeb.AutoSize = true;
			lWeb.Location = new System.Drawing.Point(9, 10);
			lWeb.Name = "lWeb";
			lWeb.Size = new System.Drawing.Size(10, 13);
			lWeb.TabIndex = 18;
			lWeb.Text = "-";
			lWeb.Visible = false;
			tWeb.Location = new System.Drawing.Point(12, 31);
			tWeb.Name = "tWeb";
			tWeb.Size = new System.Drawing.Size(380, 20);
			tWeb.TabIndex = 0;
			tWeb.Visible = false;
			lLogin.AutoSize = true;
			lLogin.Location = new System.Drawing.Point(9, 59);
			lLogin.Name = "lLogin";
			lLogin.Size = new System.Drawing.Size(10, 13);
			lLogin.TabIndex = 14;
			lLogin.Text = "-";
			tLogin.Location = new System.Drawing.Point(12, 80);
			tLogin.Name = "tLogin";
			tLogin.Size = new System.Drawing.Size(380, 20);
			tLogin.TabIndex = 1;
			tPassword.Location = new System.Drawing.Point(12, 129);
			tPassword.Name = "tPassword";
			tPassword.Size = new System.Drawing.Size(380, 20);
			tPassword.TabIndex = 2;
			lPassword.AutoSize = true;
			lPassword.Location = new System.Drawing.Point(9, 108);
			lPassword.Name = "lPassword";
			lPassword.Size = new System.Drawing.Size(10, 13);
			lPassword.TabIndex = 16;
			lPassword.Text = "-";
			bServer.Image = Kiosk.Properties.Resources.ico_net;
			bServer.Location = new System.Drawing.Point(12, 171);
			bServer.Name = "bServer";
			bServer.Size = new System.Drawing.Size(48, 48);
			bServer.TabIndex = 5;
			bServer.UseVisualStyleBackColor = true;
			bServer.Click += new System.EventHandler(bServer_Click);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(404, 231);
			base.Controls.Add(bServer);
			base.Controls.Add(lWeb);
			base.Controls.Add(tWeb);
			base.Controls.Add(lLogin);
			base.Controls.Add(bCancel);
			base.Controls.Add(tLogin);
			base.Controls.Add(bOK);
			base.Controls.Add(tPassword);
			base.Controls.Add(lPassword);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "DLG_New_User";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Register";
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
