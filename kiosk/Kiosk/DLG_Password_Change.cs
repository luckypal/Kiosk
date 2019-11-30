// Decompiled with JetBrains decompiler
// Type: Kiosk.DLG_Password_Change
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

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
    private IContainer components = (IContainer) null;
    public Configuracion opciones;
    public bool OK;
    public bool Logeado;
    private int id;
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
      this.OK = false;
      this.InitializeComponent();
      this.TopMost = true;
      this.opciones = _opc;
      this.id = _id;
      this.Logeado = false;
      this.Localize();
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.Text = this.opciones.Localize.Text("Modify password");
      this.lPasswordNew.Text = this.opciones.Localize.Text("New password");
      this.lPasswordOld.Text = this.opciones.Localize.Text("Current password");
      this.lPasswordVer.Text = this.opciones.Localize.Text("Repeat new password");
      this.ResumeLayout();
    }

    private void bCancel_Click(object sender, EventArgs e)
    {
      this.Logeado = false;
      this.Close();
    }

    private void bOK_Click(object sender, EventArgs e)
    {
      this.OK = true;
      string text = this.ePasswordOld.Text;
      int num1 = 0;
      this.Logeado = false;
      if (this.id == 1)
      {
        string str = XmlConfig.X2Y(text);
        if (str != "0" && str == this.opciones.PasswordADM && this.opciones.PasswordADM != "" && (this.ePasswordNew.Text == this.ePasswordVer.Text && this.ePasswordNew.Text != ""))
        {
          this.opciones.PasswordADM = XmlConfig.X2Y(this.ePasswordNew.Text);
          num1 = 1;
          Configuracion.Access_Log("Password changed");
        }
      }
      if (num1 == 1)
      {
        this.opciones.Save_Net();
        int num2 = (int) MessageBox.Show("Password changed");
      }
      this.Logeado = true;
      this.Close();
    }

    private void DLG_Password_Change_Load(object sender, EventArgs e)
    {
      this.Focus();
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
      this.lPasswordOld = new Label();
      this.lPasswordNew = new Label();
      this.lPasswordVer = new Label();
      this.ePasswordVer = new PasswordBox();
      this.ePasswordNew = new PasswordBox();
      this.ePasswordOld = new PasswordBox();
      this.SuspendLayout();
      this.bCancel.Image = (Image) Resources.ico_del;
      this.bCancel.Location = new Point(178, 167);
      this.bCancel.Name = "bCancel";
      this.bCancel.Size = new Size(48, 48);
      this.bCancel.TabIndex = 4;
      this.bCancel.UseVisualStyleBackColor = true;
      this.bCancel.Click += new EventHandler(this.bCancel_Click);
      this.bOK.Image = (Image) Resources.ico_ok;
      this.bOK.Location = new Point(232, 167);
      this.bOK.Name = "bOK";
      this.bOK.Size = new Size(48, 48);
      this.bOK.TabIndex = 3;
      this.bOK.UseVisualStyleBackColor = true;
      this.bOK.Click += new EventHandler(this.bOK_Click);
      this.lPasswordOld.AutoSize = true;
      this.lPasswordOld.Location = new Point(13, 7);
      this.lPasswordOld.Name = "lPasswordOld";
      this.lPasswordOld.Size = new Size(10, 13);
      this.lPasswordOld.TabIndex = 5;
      this.lPasswordOld.Text = "-";
      this.lPasswordNew.AutoSize = true;
      this.lPasswordNew.Location = new Point(13, 56);
      this.lPasswordNew.Name = "lPasswordNew";
      this.lPasswordNew.Size = new Size(10, 13);
      this.lPasswordNew.TabIndex = 6;
      this.lPasswordNew.Text = "-";
      this.lPasswordVer.AutoSize = true;
      this.lPasswordVer.Location = new Point(13, 105);
      this.lPasswordVer.Name = "lPasswordVer";
      this.lPasswordVer.Size = new Size(10, 13);
      this.lPasswordVer.TabIndex = 7;
      this.lPasswordVer.Text = "-";
      this.ePasswordVer.Location = new Point(12, 126);
      this.ePasswordVer.Name = "ePasswordVer";
      this.ePasswordVer.Size = new Size(268, 20);
      this.ePasswordVer.TabIndex = 2;
      this.ePasswordNew.Location = new Point(12, 77);
      this.ePasswordNew.Name = "ePasswordNew";
      this.ePasswordNew.Size = new Size(268, 20);
      this.ePasswordNew.TabIndex = 1;
      this.ePasswordOld.Location = new Point(12, 28);
      this.ePasswordOld.Name = "ePasswordOld";
      this.ePasswordOld.Size = new Size(268, 20);
      this.ePasswordOld.TabIndex = 0;
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(292, 227);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lPasswordVer);
      this.Controls.Add((Control) this.ePasswordVer);
      this.Controls.Add((Control) this.lPasswordNew);
      this.Controls.Add((Control) this.ePasswordNew);
      this.Controls.Add((Control) this.bCancel);
      this.Controls.Add((Control) this.bOK);
      this.Controls.Add((Control) this.lPasswordOld);
      this.Controls.Add((Control) this.ePasswordOld);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (DLG_Password_Change);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Modify password";
      this.TopMost = true;
      this.Load += new EventHandler(this.DLG_Password_Change_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
