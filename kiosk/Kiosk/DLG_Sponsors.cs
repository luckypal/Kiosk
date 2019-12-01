using Kiosk.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows.Forms;

namespace Kiosk
{
	public class DLG_Sponsors : Form
	{
		public bool OK;

		public Configuracion opciones;

		private IContainer components = null;

		private Button bOk;

		private Button bCancel;

		private OpenFileDialog openDisk;

		private TextBox tLogin;

		private Label lLogin;

		private TextBox tPassword;

		private Label lPassword;

		private TextBox tWeb;

		private Label lWeb;

		private Button bDEF;

		public DLG_Sponsors(ref Configuracion _opc)
		{
			OK = false;
			opciones = _opc;
			InitializeComponent();
			bool flag = false;
			tWeb.Text = _opc.Srv_Ip;
			tLogin.Text = _opc.Srv_User;
			tPassword.Text = _opc.Srv_User_P;
			Localize();
		}

		private void Localize()
		{
			SuspendLayout();
			Text = opciones.Localize.Text("Server");
			lWeb.Text = opciones.Localize.Text("Web server");
			lLogin.Text = opciones.Localize.Text("User login");
			lPassword.Text = opciones.Localize.Text("Password");
			ResumeLayout();
		}

		private void bOk_Click(object sender, EventArgs e)
		{
			Configuracion.Access_Log("User Kiosk changed");
			opciones.Srv_Ip = tWeb.Text;
			opciones.Srv_User = tLogin.Text;
			opciones.Srv_User_P = tPassword.Text;
			opciones.Save_Net();
			OK = true;
			Close();
		}

		private void bCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void bHDisk_Click(object sender, EventArgs e)
		{
			DialogResult dialogResult = openDisk.ShowDialog();
			if (dialogResult == DialogResult.OK)
			{
				Config_Import_Disk(openDisk.FileName);
			}
		}

		public bool Zip_Expand(string _file)
		{
			ZipStorer zipStorer;
			try
			{
				zipStorer = ZipStorer.Open(_file, FileAccess.Read);
			}
			catch (Exception)
			{
				return false;
			}
			List<ZipStorer.ZipFileEntry> list = zipStorer.ReadCentralDir();
			foreach (ZipStorer.ZipFileEntry item in list)
			{
				string filename = Path.Combine(Environment.CurrentDirectory + "\\data\\local\\", Path.GetFileName(item.FilenameInZip));
				try
				{
					zipStorer.ExtractFile(item, filename);
				}
				catch (Exception)
				{
					return false;
				}
			}
			zipStorer.Close();
			return true;
		}

		public bool Config_Import_Disk(string _file)
		{
			string text = Environment.CurrentDirectory + "\\data\\local\\" + Path.GetFileName(_file);
			try
			{
				File.Copy(_file, text, overwrite: true);
			}
			catch (Exception)
			{
				return false;
			}
			return Zip_Expand(text);
		}

		public bool Config_Import_Web(string _web)
		{
			string text = Environment.CurrentDirectory + "\\data\\local\\" + Path.GetFileName(_web);
			string empty = string.Empty;
			string empty2 = string.Empty;
			try
			{
				WebClient webClient = new WebClient();
				webClient.DownloadFile(_web, text);
			}
			catch (Exception)
			{
				return false;
			}
			return Zip_Expand(text);
		}

		private void bHWeb_Click(object sender, EventArgs e)
		{
		}

		private void bDEF_Click(object sender, EventArgs e)
		{
			bool flag = false;
			opciones.Servidor_Lux();
			tWeb.Text = opciones.Srv_Ip;
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
			openDisk = new System.Windows.Forms.OpenFileDialog();
			tLogin = new System.Windows.Forms.TextBox();
			lLogin = new System.Windows.Forms.Label();
			tPassword = new System.Windows.Forms.TextBox();
			lPassword = new System.Windows.Forms.Label();
			tWeb = new System.Windows.Forms.TextBox();
			lWeb = new System.Windows.Forms.Label();
			bCancel = new System.Windows.Forms.Button();
			bOk = new System.Windows.Forms.Button();
			bDEF = new System.Windows.Forms.Button();
			SuspendLayout();
			openDisk.FileName = "Source directory";
			openDisk.Filter = "Zip file|*.zip|All files|*.*";
			openDisk.Title = "Select ZIP file";
			tLogin.Location = new System.Drawing.Point(15, 79);
			tLogin.Name = "tLogin";
			tLogin.Size = new System.Drawing.Size(396, 20);
			tLogin.TabIndex = 1;
			lLogin.AutoSize = true;
			lLogin.Location = new System.Drawing.Point(12, 58);
			lLogin.Name = "lLogin";
			lLogin.Size = new System.Drawing.Size(33, 13);
			lLogin.TabIndex = 14;
			lLogin.Text = "Login";
			tPassword.Location = new System.Drawing.Point(15, 128);
			tPassword.Name = "tPassword";
			tPassword.Size = new System.Drawing.Size(396, 20);
			tPassword.TabIndex = 2;
			tPassword.UseSystemPasswordChar = true;
			lPassword.AutoSize = true;
			lPassword.Location = new System.Drawing.Point(12, 107);
			lPassword.Name = "lPassword";
			lPassword.Size = new System.Drawing.Size(53, 13);
			lPassword.TabIndex = 16;
			lPassword.Text = "Password";
			tWeb.Location = new System.Drawing.Point(15, 30);
			tWeb.Name = "tWeb";
			tWeb.Size = new System.Drawing.Size(326, 20);
			tWeb.TabIndex = 0;
			lWeb.AutoSize = true;
			lWeb.Location = new System.Drawing.Point(12, 9);
			lWeb.Name = "lWeb";
			lWeb.Size = new System.Drawing.Size(30, 13);
			lWeb.TabIndex = 18;
			lWeb.Text = "Web";
			bCancel.BackgroundImage = Kiosk.Properties.Resources.ico_del;
			bCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			bCancel.Location = new System.Drawing.Point(277, 168);
			bCancel.Name = "bCancel";
			bCancel.Size = new System.Drawing.Size(64, 48);
			bCancel.TabIndex = 4;
			bCancel.UseVisualStyleBackColor = true;
			bCancel.Click += new System.EventHandler(bCancel_Click);
			bOk.BackgroundImage = Kiosk.Properties.Resources.ico_ok;
			bOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			bOk.Location = new System.Drawing.Point(347, 168);
			bOk.Name = "bOk";
			bOk.Size = new System.Drawing.Size(64, 48);
			bOk.TabIndex = 3;
			bOk.UseVisualStyleBackColor = true;
			bOk.Click += new System.EventHandler(bOk_Click);
			bDEF.BackgroundImage = Kiosk.Properties.Resources.ico_net;
			bDEF.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			bDEF.Location = new System.Drawing.Point(347, 15);
			bDEF.Name = "bDEF";
			bDEF.Size = new System.Drawing.Size(64, 48);
			bDEF.TabIndex = 19;
			bDEF.UseVisualStyleBackColor = true;
			bDEF.Click += new System.EventHandler(bDEF_Click);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(427, 228);
			base.ControlBox = false;
			base.Controls.Add(bDEF);
			base.Controls.Add(lWeb);
			base.Controls.Add(tWeb);
			base.Controls.Add(lLogin);
			base.Controls.Add(bCancel);
			base.Controls.Add(tLogin);
			base.Controls.Add(bOk);
			base.Controls.Add(tPassword);
			base.Controls.Add(lPassword);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "DLG_Sponsors";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Sponsors";
			base.TopMost = true;
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
