// Decompiled with JetBrains decompiler
// Type: Kiosk.DLG_New_User
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
  public class DLG_New_User : Form
  {
    private IContainer components = (IContainer) null;
    public string Web;
    public string User;
    public string Password;
    public bool OK;
    public Configuracion opciones;
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
      this.OK = false;
      this.opciones = _opc;
      this.InitializeComponent();
      this.tWeb.Text = _opc.Srv_Ip;
      this.tLogin.Text = _opc.Srv_User;
      this.tPassword.Text = _opc.Srv_User_P;
      this.Localize();
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.Text = this.opciones.Localize.Text("Register");
      this.lWeb.Text = this.opciones.Localize.Text("Web");
      this.lLogin.Text = this.opciones.Localize.Text("Login");
      this.lPassword.Text = this.opciones.Localize.Text("Password");
      this.ResumeLayout();
    }

    public void Update_Info(string _u, string _p, string _w)
    {
      this.Web = _w;
      this.User = _u;
      this.Password = _p;
      this.tWeb.Text = this.Web;
      this.tLogin.Text = this.User;
      this.tPassword.Text = this.Password;
    }

    private void bOK_Click(object sender, EventArgs e)
    {
      this.Web = this.tWeb.Text;
      this.User = this.tLogin.Text;
      this.Password = this.tPassword.Text;
      this.OK = true;
      this.Close();
    }

    private void bCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void bTest_Click(object sender, EventArgs e)
    {
    }

    private void bServer_Click(object sender, EventArgs e)
    {
      this.tWeb.Visible = true;
      this.lWeb.Visible = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.bCancel = new Button();
      this.bOK = new Button();
      this.lWeb = new Label();
      this.tWeb = new TextBox();
      this.lLogin = new Label();
      this.tLogin = new TextBox();
      this.tPassword = new TextBox();
      this.lPassword = new Label();
      this.bServer = new Button();
      this.SuspendLayout();
      this.bCancel.Image = (Image) Resources.ico_del;
      this.bCancel.Location = new Point(290, 171);
      this.bCancel.Name = "bCancel";
      this.bCancel.Size = new Size(48, 48);
      this.bCancel.TabIndex = 4;
      this.bCancel.UseVisualStyleBackColor = true;
      this.bCancel.Click += new EventHandler(this.bCancel_Click);
      this.bOK.Image = (Image) Resources.ico_ok;
      this.bOK.Location = new Point(344, 171);
      this.bOK.Name = "bOK";
      this.bOK.Size = new Size(48, 48);
      this.bOK.TabIndex = 3;
      this.bOK.UseVisualStyleBackColor = true;
      this.bOK.Click += new EventHandler(this.bOK_Click);
      this.lWeb.AutoSize = true;
      this.lWeb.Location = new Point(9, 10);
      this.lWeb.Name = "lWeb";
      this.lWeb.Size = new Size(10, 13);
      this.lWeb.TabIndex = 18;
      this.lWeb.Text = "-";
      this.lWeb.Visible = false;
      this.tWeb.Location = new Point(12, 31);
      this.tWeb.Name = "tWeb";
      this.tWeb.Size = new Size(380, 20);
      this.tWeb.TabIndex = 0;
      this.tWeb.Visible = false;
      this.lLogin.AutoSize = true;
      this.lLogin.Location = new Point(9, 59);
      this.lLogin.Name = "lLogin";
      this.lLogin.Size = new Size(10, 13);
      this.lLogin.TabIndex = 14;
      this.lLogin.Text = "-";
      this.tLogin.Location = new Point(12, 80);
      this.tLogin.Name = "tLogin";
      this.tLogin.Size = new Size(380, 20);
      this.tLogin.TabIndex = 1;
      this.tPassword.Location = new Point(12, 129);
      this.tPassword.Name = "tPassword";
      this.tPassword.Size = new Size(380, 20);
      this.tPassword.TabIndex = 2;
      this.lPassword.AutoSize = true;
      this.lPassword.Location = new Point(9, 108);
      this.lPassword.Name = "lPassword";
      this.lPassword.Size = new Size(10, 13);
      this.lPassword.TabIndex = 16;
      this.lPassword.Text = "-";
      this.bServer.Image = (Image) Resources.ico_net;
      this.bServer.Location = new Point(12, 171);
      this.bServer.Name = "bServer";
      this.bServer.Size = new Size(48, 48);
      this.bServer.TabIndex = 5;
      this.bServer.UseVisualStyleBackColor = true;
      this.bServer.Click += new EventHandler(this.bServer_Click);
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(404, 231);
      this.Controls.Add((Control) this.bServer);
      this.Controls.Add((Control) this.lWeb);
      this.Controls.Add((Control) this.tWeb);
      this.Controls.Add((Control) this.lLogin);
      this.Controls.Add((Control) this.bCancel);
      this.Controls.Add((Control) this.tLogin);
      this.Controls.Add((Control) this.bOK);
      this.Controls.Add((Control) this.tPassword);
      this.Controls.Add((Control) this.lPassword);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (DLG_New_User);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Register";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
