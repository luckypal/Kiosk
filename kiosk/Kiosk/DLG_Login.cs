// Decompiled with JetBrains decompiler
// Type: Kiosk.DLG_Login
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

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
    private IContainer components = (IContainer) null;
    public Configuracion opciones;
    public int Logeado;
    public bool OK;
    public int Qui;
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
      this.Qui = _qui;
      this.OK = false;
      this.InitializeComponent();
      this.opciones = _opc;
      this.Logeado = 0;
      this.Localize();
      this.opciones.Log_Debug("Try Login");
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.Text = this.opciones.Localize.Text("Stop!");
      this.lPassword.Text = this.opciones.Localize.Text("Password");
      this.ResumeLayout();
    }

    private void bCancel_Click(object sender, EventArgs e)
    {
      this.Logeado = 0;
      this.Close();
    }

    private void bOK_Click(object sender, EventArgs e)
    {
      this.OK = true;
      string text = this.ePassword.Text;
      this.Logeado = 0;
      string str = XmlConfig.X2Y(text);
      if (this.Qui == 1)
      {
        if (str != "0" && str == XmlConfig.X2Y("gowindows"))
          this.Logeado = 1;
      }
      else if (str != "0" && str == this.opciones.PasswordADM && this.opciones.PasswordADM != "")
        this.Logeado = 1;
      this.Close();
    }

    [DllImport("User32.dll")]
    private static extern int SetFocus(IntPtr hWnd);

    private void DLG_Login_Load(object sender, EventArgs e)
    {
      this.Focus();
      this.opciones.LastMouseMove = DateTime.Now;
      this.tScan.Enabled = true;
    }

    private void ePassword_KeyDown(object sender, KeyEventArgs e)
    {
      if ((e.Modifiers & Keys.Control) == Keys.Control)
        this.lCtrl.Text = "Control Pressed";
      else
        this.lCtrl.Text = "";
    }

    private void ePassword_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\r')
        return;
      this.bOK_Click(sender, (EventArgs) null);
    }

    private void bUnlock_Click(object sender, EventArgs e)
    {
      this.opciones.ForceAllKey = 1;
    }

    private void nKey_Click(object sender, EventArgs e)
    {
      if (Process.GetProcessesByName("KVKeyboard").Length >= 1)
        new PipeClient().Send("QUIT", "KVKeyboard", 1000);
      else
        Process.Start("KVKeyboard.exe", "es");
    }

    private void DLG_Login_Enter(object sender, EventArgs e)
    {
      this.ePassword.Focus();
    }

    private void tScan_Tick(object sender, EventArgs e)
    {
      if ((int) (DateTime.Now - this.opciones.LastMouseMove).TotalSeconds <= 60)
        return;
      this.Logeado = 0;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      this.lPassword = new Label();
      this.bOK = new Button();
      this.bCancel = new Button();
      this.lCtrl = new Label();
      this.bUnlock = new Button();
      this.ePassword = new PasswordBox();
      this.nKey = new Button();
      this.tScan = new Timer(this.components);
      this.SuspendLayout();
      this.lPassword.AutoSize = true;
      this.lPassword.Location = new Point(13, 14);
      this.lPassword.Name = "lPassword";
      this.lPassword.Size = new Size(10, 13);
      this.lPassword.TabIndex = 3;
      this.lPassword.Text = "-";
      this.bOK.Image = (Image) Resources.ico_ok;
      this.bOK.Location = new Point(232, 70);
      this.bOK.Name = "bOK";
      this.bOK.Size = new Size(48, 48);
      this.bOK.TabIndex = 2;
      this.bOK.UseVisualStyleBackColor = true;
      this.bOK.Click += new EventHandler(this.bOK_Click);
      this.bCancel.Image = (Image) Resources.ico_del;
      this.bCancel.Location = new Point(178, 70);
      this.bCancel.Name = "bCancel";
      this.bCancel.Size = new Size(48, 48);
      this.bCancel.TabIndex = 1;
      this.bCancel.UseVisualStyleBackColor = true;
      this.bCancel.Click += new EventHandler(this.bCancel_Click);
      this.lCtrl.AutoSize = true;
      this.lCtrl.ForeColor = Color.Red;
      this.lCtrl.Location = new Point(13, 60);
      this.lCtrl.Name = "lCtrl";
      this.lCtrl.Size = new Size(0, 13);
      this.lCtrl.TabIndex = 4;
      this.bUnlock.Image = (Image) Resources.ico_barcodeX;
      this.bUnlock.Location = new Point(12, 70);
      this.bUnlock.Name = "bUnlock";
      this.bUnlock.Size = new Size(48, 48);
      this.bUnlock.TabIndex = 3;
      this.bUnlock.UseVisualStyleBackColor = true;
      this.bUnlock.Click += new EventHandler(this.bUnlock_Click);
      this.ePassword.Location = new Point(12, 35);
      this.ePassword.Name = "ePassword";
      this.ePassword.Size = new Size(268, 20);
      this.ePassword.TabIndex = 0;
      this.ePassword.KeyDown += new KeyEventHandler(this.ePassword_KeyDown);
      this.ePassword.KeyPress += new KeyPressEventHandler(this.ePassword_KeyPress);
      this.nKey.Image = (Image) Resources.ico_keyboard;
      this.nKey.Location = new Point(66, 70);
      this.nKey.Name = "nKey";
      this.nKey.Size = new Size(48, 48);
      this.nKey.TabIndex = 5;
      this.nKey.UseVisualStyleBackColor = true;
      this.nKey.Click += new EventHandler(this.nKey_Click);
      this.tScan.Interval = 500;
      this.tScan.Tick += new EventHandler(this.tScan_Tick);
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(292, 130);
      this.ControlBox = false;
      this.Controls.Add((Control) this.nKey);
      this.Controls.Add((Control) this.bUnlock);
      this.Controls.Add((Control) this.lCtrl);
      this.Controls.Add((Control) this.bCancel);
      this.Controls.Add((Control) this.bOK);
      this.Controls.Add((Control) this.lPassword);
      this.Controls.Add((Control) this.ePassword);
      this.DoubleBuffered = true;
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.Name = nameof (DLG_Login);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Stop!";
      this.TopMost = true;
      this.Load += new EventHandler(this.DLG_Login_Load);
      this.Enter += new EventHandler(this.DLG_Login_Enter);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
