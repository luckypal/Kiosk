using GLib;
using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
	public class DLG_Registro : Form
	{
		public Configuracion opciones;

		public int Logeado;

		public bool OK;

		public int Login;

		private IContainer components = null;

		private Label label1;

		private Label label2;

		private PasswordBox tPassword;

		private TextBox tUser;

		private Button bCancel;

		private Button bOK;

		private Panel panel1;

		private PasswordBox tcnPassword;

		private Label label6;

		private TextBox tnUser;

		private PasswordBox tnPassword;

		private Label label4;

		private Label label5;

		private TextBox tCPF;

		private Label label8;

		private TextBox tName;

		private Label label7;

		private Button bNew;

		public DLG_Registro(ref Configuracion _opc)
		{
			OK = false;
			Login = -1;
			InitializeComponent();
			opciones = _opc;
			Localize();
		}

		private void Localize()
		{
			SuspendLayout();
			ResumeLayout();
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			if (opciones.Login_User(tUser.Text, tPassword.Text))
			{
				OK = true;
				Login = 1;
				Close();
			}
			else
			{
				MessageBox.Show("Error en el login");
			}
		}

		private void bCancel_Click(object sender, EventArgs e)
		{
			Login = 0;
			Close();
		}

		private void bNew_Click(object sender, EventArgs e)
		{
			int num = 0;
			if (tnPassword.Text != tcnPassword.Text)
			{
				tnPassword.BackColor = Color.Red;
				tcnPassword.BackColor = Color.Red;
				num++;
			}
			else
			{
				tnPassword.BackColor = Color.White;
				tcnPassword.BackColor = Color.White;
			}
			if (!Gestion.IsCpf(tCPF.Text))
			{
				tCPF.BackColor = Color.Red;
				num++;
			}
			else
			{
				tCPF.BackColor = Color.White;
			}
			if (string.IsNullOrEmpty(tName.Text))
			{
				tName.BackColor = Color.Red;
				num++;
			}
			else
			{
				tName.BackColor = Color.White;
			}
			if (string.IsNullOrEmpty(tnUser.Text))
			{
				tnUser.BackColor = Color.Red;
				num++;
			}
			else
			{
				tnUser.BackColor = Color.White;
			}
			if (num > 0)
			{
				MessageBox.Show("Faltan datos o los introducidos son incorrectos");
			}
			else if (!opciones.Insert_User(tnUser.Text, tName.Text, tCPF.Text, tnPassword.Text, tcnPassword.Text))
			{
				MessageBox.Show("El usuario ya existe o no son correctos los datos");
			}
			else if (opciones.Login_User(tnUser.Text, tnPassword.Text))
			{
				OK = true;
				Login = 1;
				Close();
			}
			else
			{
				MessageBox.Show("Error en el login");
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
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			tPassword = new GLib.PasswordBox();
			tUser = new System.Windows.Forms.TextBox();
			bCancel = new System.Windows.Forms.Button();
			bOK = new System.Windows.Forms.Button();
			panel1 = new System.Windows.Forms.Panel();
			bNew = new System.Windows.Forms.Button();
			tCPF = new System.Windows.Forms.TextBox();
			label8 = new System.Windows.Forms.Label();
			tName = new System.Windows.Forms.TextBox();
			label7 = new System.Windows.Forms.Label();
			tcnPassword = new GLib.PasswordBox();
			label6 = new System.Windows.Forms.Label();
			tnUser = new System.Windows.Forms.TextBox();
			tnPassword = new GLib.PasswordBox();
			label4 = new System.Windows.Forms.Label();
			label5 = new System.Windows.Forms.Label();
			panel1.SuspendLayout();
			SuspendLayout();
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(13, 13);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(29, 13);
			label1.TabIndex = 0;
			label1.Text = "User";
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(13, 39);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(53, 13);
			label2.TabIndex = 1;
			label2.Text = "Password";
			tPassword.Location = new System.Drawing.Point(68, 39);
			tPassword.Name = "tPassword";
			tPassword.Size = new System.Drawing.Size(121, 20);
			tPassword.TabIndex = 1;
			tUser.Location = new System.Drawing.Point(68, 13);
			tUser.Name = "tUser";
			tUser.Size = new System.Drawing.Size(121, 20);
			tUser.TabIndex = 0;
			bCancel.Image = Kiosk.Properties.Resources.ico_del;
			bCancel.Location = new System.Drawing.Point(211, 13);
			bCancel.Name = "bCancel";
			bCancel.Size = new System.Drawing.Size(48, 48);
			bCancel.TabIndex = 3;
			bCancel.UseVisualStyleBackColor = true;
			bCancel.Click += new System.EventHandler(bCancel_Click);
			bOK.Image = Kiosk.Properties.Resources.ico_ok;
			bOK.Location = new System.Drawing.Point(265, 13);
			bOK.Name = "bOK";
			bOK.Size = new System.Drawing.Size(48, 48);
			bOK.TabIndex = 2;
			bOK.UseVisualStyleBackColor = true;
			bOK.Click += new System.EventHandler(bOK_Click);
			panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			panel1.Controls.Add(bNew);
			panel1.Controls.Add(tCPF);
			panel1.Controls.Add(label8);
			panel1.Controls.Add(tName);
			panel1.Controls.Add(label7);
			panel1.Controls.Add(tcnPassword);
			panel1.Controls.Add(label6);
			panel1.Controls.Add(tnUser);
			panel1.Controls.Add(tnPassword);
			panel1.Controls.Add(label4);
			panel1.Controls.Add(label5);
			panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			panel1.Location = new System.Drawing.Point(0, 67);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(325, 143);
			panel1.TabIndex = 6;
			bNew.Image = Kiosk.Properties.Resources.ico_add;
			bNew.Location = new System.Drawing.Point(264, 21);
			bNew.Name = "bNew";
			bNew.Size = new System.Drawing.Size(48, 48);
			bNew.TabIndex = 5;
			bNew.UseVisualStyleBackColor = true;
			bNew.Click += new System.EventHandler(bNew_Click);
			tCPF.Location = new System.Drawing.Point(67, 114);
			tCPF.Name = "tCPF";
			tCPF.Size = new System.Drawing.Size(121, 20);
			tCPF.TabIndex = 4;
			label8.AutoSize = true;
			label8.Location = new System.Drawing.Point(12, 114);
			label8.Name = "label8";
			label8.Size = new System.Drawing.Size(27, 13);
			label8.TabIndex = 14;
			label8.Text = "CPF";
			tName.Location = new System.Drawing.Point(67, 88);
			tName.Name = "tName";
			tName.Size = new System.Drawing.Size(245, 20);
			tName.TabIndex = 3;
			label7.AutoSize = true;
			label7.Location = new System.Drawing.Point(12, 88);
			label7.Name = "label7";
			label7.Size = new System.Drawing.Size(35, 13);
			label7.TabIndex = 12;
			label7.Text = "Name";
			tcnPassword.Location = new System.Drawing.Point(129, 62);
			tcnPassword.Name = "tcnPassword";
			tcnPassword.Size = new System.Drawing.Size(121, 20);
			tcnPassword.TabIndex = 2;
			label6.AutoSize = true;
			label6.Location = new System.Drawing.Point(12, 66);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(100, 13);
			label6.TabIndex = 10;
			label6.Text = "Confirme Password ";
			tnUser.Location = new System.Drawing.Point(129, 10);
			tnUser.Name = "tnUser";
			tnUser.Size = new System.Drawing.Size(121, 20);
			tnUser.TabIndex = 0;
			tnPassword.Location = new System.Drawing.Point(129, 36);
			tnPassword.Name = "tnPassword";
			tnPassword.Size = new System.Drawing.Size(121, 20);
			tnPassword.TabIndex = 1;
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(12, 39);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(53, 13);
			label4.TabIndex = 7;
			label4.Text = "Password";
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point(12, 13);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(52, 13);
			label5.TabIndex = 6;
			label5.Text = "New user";
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(325, 210);
			base.ControlBox = false;
			base.Controls.Add(panel1);
			base.Controls.Add(bCancel);
			base.Controls.Add(bOK);
			base.Controls.Add(tUser);
			base.Controls.Add(tPassword);
			base.Controls.Add(label2);
			base.Controls.Add(label1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DLG_Registro";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Register / Login";
			base.TopMost = true;
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
