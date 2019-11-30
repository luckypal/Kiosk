// Decompiled with JetBrains decompiler
// Type: Kiosk.DLG_Registro
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

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
    private IContainer components = (IContainer) null;
    public Configuracion opciones;
    public int Logeado;
    public bool OK;
    public int Login;
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
      this.OK = false;
      this.Login = -1;
      this.InitializeComponent();
      this.opciones = _opc;
      this.Localize();
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.ResumeLayout();
    }

    private void bOK_Click(object sender, EventArgs e)
    {
      if (this.opciones.Login_User(this.tUser.Text, this.tPassword.Text))
      {
        this.OK = true;
        this.Login = 1;
        this.Close();
      }
      else
      {
        int num = (int) MessageBox.Show("Error en el login");
      }
    }

    private void bCancel_Click(object sender, EventArgs e)
    {
      this.Login = 0;
      this.Close();
    }

    private void bNew_Click(object sender, EventArgs e)
    {
      int num1 = 0;
      if (this.tnPassword.Text != this.tcnPassword.Text)
      {
        this.tnPassword.BackColor = Color.Red;
        this.tcnPassword.BackColor = Color.Red;
        ++num1;
      }
      else
      {
        this.tnPassword.BackColor = Color.White;
        this.tcnPassword.BackColor = Color.White;
      }
      if (!Gestion.IsCpf(this.tCPF.Text))
      {
        this.tCPF.BackColor = Color.Red;
        ++num1;
      }
      else
        this.tCPF.BackColor = Color.White;
      if (string.IsNullOrEmpty(this.tName.Text))
      {
        this.tName.BackColor = Color.Red;
        ++num1;
      }
      else
        this.tName.BackColor = Color.White;
      if (string.IsNullOrEmpty(this.tnUser.Text))
      {
        this.tnUser.BackColor = Color.Red;
        ++num1;
      }
      else
        this.tnUser.BackColor = Color.White;
      if (num1 > 0)
      {
        int num2 = (int) MessageBox.Show("Faltan datos o los introducidos son incorrectos");
      }
      else if (!this.opciones.Insert_User(this.tnUser.Text, this.tName.Text, this.tCPF.Text, this.tnPassword.Text, this.tcnPassword.Text))
      {
        int num3 = (int) MessageBox.Show("El usuario ya existe o no son correctos los datos");
      }
      else if (this.opciones.Login_User(this.tnUser.Text, this.tnPassword.Text))
      {
        this.OK = true;
        this.Login = 1;
        this.Close();
      }
      else
      {
        int num4 = (int) MessageBox.Show("Error en el login");
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.label2 = new Label();
      this.tPassword = new PasswordBox();
      this.tUser = new TextBox();
      this.bCancel = new Button();
      this.bOK = new Button();
      this.panel1 = new Panel();
      this.bNew = new Button();
      this.tCPF = new TextBox();
      this.label8 = new Label();
      this.tName = new TextBox();
      this.label7 = new Label();
      this.tcnPassword = new PasswordBox();
      this.label6 = new Label();
      this.tnUser = new TextBox();
      this.tnPassword = new PasswordBox();
      this.label4 = new Label();
      this.label5 = new Label();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(29, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "User";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(13, 39);
      this.label2.Name = "label2";
      this.label2.Size = new Size(53, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Password";
      this.tPassword.Location = new Point(68, 39);
      this.tPassword.Name = "tPassword";
      this.tPassword.Size = new Size(121, 20);
      this.tPassword.TabIndex = 1;
      this.tUser.Location = new Point(68, 13);
      this.tUser.Name = "tUser";
      this.tUser.Size = new Size(121, 20);
      this.tUser.TabIndex = 0;
      this.bCancel.Image = (Image) Resources.ico_del;
      this.bCancel.Location = new Point(211, 13);
      this.bCancel.Name = "bCancel";
      this.bCancel.Size = new Size(48, 48);
      this.bCancel.TabIndex = 3;
      this.bCancel.UseVisualStyleBackColor = true;
      this.bCancel.Click += new EventHandler(this.bCancel_Click);
      this.bOK.Image = (Image) Resources.ico_ok;
      this.bOK.Location = new Point(265, 13);
      this.bOK.Name = "bOK";
      this.bOK.Size = new Size(48, 48);
      this.bOK.TabIndex = 2;
      this.bOK.UseVisualStyleBackColor = true;
      this.bOK.Click += new EventHandler(this.bOK_Click);
      this.panel1.BorderStyle = BorderStyle.FixedSingle;
      this.panel1.Controls.Add((Control) this.bNew);
      this.panel1.Controls.Add((Control) this.tCPF);
      this.panel1.Controls.Add((Control) this.label8);
      this.panel1.Controls.Add((Control) this.tName);
      this.panel1.Controls.Add((Control) this.label7);
      this.panel1.Controls.Add((Control) this.tcnPassword);
      this.panel1.Controls.Add((Control) this.label6);
      this.panel1.Controls.Add((Control) this.tnUser);
      this.panel1.Controls.Add((Control) this.tnPassword);
      this.panel1.Controls.Add((Control) this.label4);
      this.panel1.Controls.Add((Control) this.label5);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 67);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(325, 143);
      this.panel1.TabIndex = 6;
      this.bNew.Image = (Image) Resources.ico_add;
      this.bNew.Location = new Point(264, 21);
      this.bNew.Name = "bNew";
      this.bNew.Size = new Size(48, 48);
      this.bNew.TabIndex = 5;
      this.bNew.UseVisualStyleBackColor = true;
      this.bNew.Click += new EventHandler(this.bNew_Click);
      this.tCPF.Location = new Point(67, 114);
      this.tCPF.Name = "tCPF";
      this.tCPF.Size = new Size(121, 20);
      this.tCPF.TabIndex = 4;
      this.label8.AutoSize = true;
      this.label8.Location = new Point(12, 114);
      this.label8.Name = "label8";
      this.label8.Size = new Size(27, 13);
      this.label8.TabIndex = 14;
      this.label8.Text = "CPF";
      this.tName.Location = new Point(67, 88);
      this.tName.Name = "tName";
      this.tName.Size = new Size(245, 20);
      this.tName.TabIndex = 3;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(12, 88);
      this.label7.Name = "label7";
      this.label7.Size = new Size(35, 13);
      this.label7.TabIndex = 12;
      this.label7.Text = "Name";
      this.tcnPassword.Location = new Point(129, 62);
      this.tcnPassword.Name = "tcnPassword";
      this.tcnPassword.Size = new Size(121, 20);
      this.tcnPassword.TabIndex = 2;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(12, 66);
      this.label6.Name = "label6";
      this.label6.Size = new Size(100, 13);
      this.label6.TabIndex = 10;
      this.label6.Text = "Confirme Password ";
      this.tnUser.Location = new Point(129, 10);
      this.tnUser.Name = "tnUser";
      this.tnUser.Size = new Size(121, 20);
      this.tnUser.TabIndex = 0;
      this.tnPassword.Location = new Point(129, 36);
      this.tnPassword.Name = "tnPassword";
      this.tnPassword.Size = new Size(121, 20);
      this.tnPassword.TabIndex = 1;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(12, 39);
      this.label4.Name = "label4";
      this.label4.Size = new Size(53, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "Password";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(12, 13);
      this.label5.Name = "label5";
      this.label5.Size = new Size(52, 13);
      this.label5.TabIndex = 6;
      this.label5.Text = "New user";
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(325, 210);
      this.ControlBox = false;
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.bCancel);
      this.Controls.Add((Control) this.bOK);
      this.Controls.Add((Control) this.tUser);
      this.Controls.Add((Control) this.tPassword);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DLG_Registro);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Register / Login";
      this.TopMost = true;
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
