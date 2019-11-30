// Decompiled with JetBrains decompiler
// Type: Kiosk.DLG_Wifi_Connect
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
  public class DLG_Wifi_Connect : Form
  {
    private IContainer components = (IContainer) null;
    public bool OK;
    public bool IsClosed;
    public Configuracion opciones;
    public string SID;
    public string Password;
    public string SEC;
    public bool AlwaysConnect;
    public bool NewProfile;
    private Panel pBOTTOM;
    private Button bCancel;
    private Button bOk;
    private Label lPassword;
    private TextBox tSID;
    private Label lSID;
    private Label lSec;
    private ComboBox cSec;
    private CheckBox cNew;
    private CheckBox cAlways;
    private TextBox ePassword;

    public DLG_Wifi_Connect(ref Configuracion _opc, string _sid, string _pw, string _sec)
    {
      this.IsClosed = false;
      this.OK = false;
      this.opciones = _opc;
      this.AlwaysConnect = true;
      this.NewProfile = false;
      this.InitializeComponent();
      this.Localize();
      this.Password = _pw;
      this.SID = _sid;
      this.ePassword.Text = this.Password;
      this.tSID.Text = this.SID;
      this.SEC = _sec;
      switch (_sec)
      {
        case "IEEE80211_Open":
          this.cSec.Text = "OPEN";
          break;
        case "IEEE80211_SharedKey":
          this.cSec.Text = "WEP";
          break;
        case "WPA":
          this.cSec.Text = "WPA";
          break;
        case "WPA_PSK":
          this.cSec.Text = "WPA PSK";
          break;
        case "RSNA":
          this.cSec.Text = "WPA2";
          break;
        case "RSNA_PSK":
          this.cSec.Text = "WPA2 PSK";
          break;
      }
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.lSID.Text = this.opciones.Localize.Text("SID");
      this.lPassword.Text = this.opciones.Localize.Text("New Password");
      this.lSec.Text = this.opciones.Localize.Text("Security");
      this.ResumeLayout();
    }

    private void bOk_Click(object sender, EventArgs e)
    {
      this.SID = this.tSID.Text;
      this.Password = this.ePassword.Text;
      this.OK = true;
      switch (this.cSec.Text)
      {
        case "OPEN":
          this.SEC = "IEEE80211_Open";
          break;
        case "WEP":
          this.SEC = "IEEE80211_SharedKey";
          break;
        case "WPA":
          this.SEC = "WPA";
          break;
        case "WPA PSK":
          this.SEC = "WPA_PSK";
          break;
        case "RSNA":
          this.SEC = "WPA2";
          break;
        case "RSNA PSK":
          this.SEC = "WPA2_PSK";
          break;
      }
      this.NewProfile = this.cNew.Checked;
      this.AlwaysConnect = this.cAlways.Checked;
      this.Close();
    }

    private void bCancel_Click(object sender, EventArgs e)
    {
      this.OK = false;
      this.Close();
    }

    private void DLG_Wifi_Connect_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.IsClosed = true;
    }

    private void DLG_Wifi_Connect_Load(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pBOTTOM = new Panel();
      this.cNew = new CheckBox();
      this.bCancel = new Button();
      this.bOk = new Button();
      this.lPassword = new Label();
      this.tSID = new TextBox();
      this.lSID = new Label();
      this.lSec = new Label();
      this.cSec = new ComboBox();
      this.cAlways = new CheckBox();
      this.ePassword = new TextBox();
      this.pBOTTOM.SuspendLayout();
      this.SuspendLayout();
      this.pBOTTOM.Controls.Add((Control) this.cNew);
      this.pBOTTOM.Controls.Add((Control) this.bCancel);
      this.pBOTTOM.Controls.Add((Control) this.bOk);
      this.pBOTTOM.Dock = DockStyle.Bottom;
      this.pBOTTOM.Location = new Point(0, 230);
      this.pBOTTOM.Name = "pBOTTOM";
      this.pBOTTOM.Size = new Size(453, 48);
      this.pBOTTOM.TabIndex = 6;
      this.cNew.AutoSize = true;
      this.cNew.Font = new Font("Microsoft Sans Serif", 14f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cNew.Location = new Point(15, 21);
      this.cNew.Name = "cNew";
      this.cNew.Size = new Size(197, 28);
      this.cNew.TabIndex = 13;
      this.cNew.Text = "Is a new connection";
      this.cNew.UseVisualStyleBackColor = true;
      this.bCancel.Dock = DockStyle.Right;
      this.bCancel.Image = (Image) Resources.ico_del;
      this.bCancel.Location = new Point(357, 0);
      this.bCancel.Name = "bCancel";
      this.bCancel.Size = new Size(48, 48);
      this.bCancel.TabIndex = 1;
      this.bCancel.UseVisualStyleBackColor = true;
      this.bCancel.Click += new EventHandler(this.bCancel_Click);
      this.bOk.Dock = DockStyle.Right;
      this.bOk.Image = (Image) Resources.ico_ok;
      this.bOk.Location = new Point(405, 0);
      this.bOk.Name = "bOk";
      this.bOk.Size = new Size(48, 48);
      this.bOk.TabIndex = 0;
      this.bOk.UseVisualStyleBackColor = true;
      this.bOk.Click += new EventHandler(this.bOk_Click);
      this.lPassword.AutoSize = true;
      this.lPassword.Font = new Font("Microsoft Sans Serif", 14f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lPassword.Location = new Point(10, 81);
      this.lPassword.Name = "lPassword";
      this.lPassword.Size = new Size(16, 24);
      this.lPassword.TabIndex = 8;
      this.lPassword.Text = "-";
      this.tSID.Font = new Font("Microsoft Sans Serif", 14f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tSID.Location = new Point(12, 42);
      this.tSID.Name = "tSID";
      this.tSID.Size = new Size(430, 29);
      this.tSID.TabIndex = 0;
      this.lSID.AutoSize = true;
      this.lSID.Font = new Font("Microsoft Sans Serif", 14f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lSID.Location = new Point(11, 15);
      this.lSID.Name = "lSID";
      this.lSID.Size = new Size(16, 24);
      this.lSID.TabIndex = 10;
      this.lSID.Text = "-";
      this.lSec.AutoSize = true;
      this.lSec.Font = new Font("Microsoft Sans Serif", 14f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lSec.Location = new Point(11, 155);
      this.lSec.Name = "lSec";
      this.lSec.Size = new Size(16, 24);
      this.lSec.TabIndex = 11;
      this.lSec.Text = "-";
      this.cSec.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cSec.Font = new Font("Microsoft Sans Serif", 14f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cSec.FormattingEnabled = true;
      this.cSec.Items.AddRange(new object[5]
      {
        (object) "WEP",
        (object) "WPA",
        (object) "WPA PSK",
        (object) "WPA2",
        (object) "WPA2 PSK"
      });
      this.cSec.Location = new Point(11, 182);
      this.cSec.Name = "cSec";
      this.cSec.Size = new Size(430, 32);
      this.cSec.TabIndex = 2;
      this.cAlways.AutoSize = true;
      this.cAlways.Checked = true;
      this.cAlways.CheckState = CheckState.Checked;
      this.cAlways.Font = new Font("Microsoft Sans Serif", 14f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cAlways.Location = new Point(14, 225);
      this.cAlways.Name = "cAlways";
      this.cAlways.Size = new Size(230, 28);
      this.cAlways.TabIndex = 12;
      this.cAlways.Text = "Connect when available";
      this.cAlways.UseVisualStyleBackColor = true;
      this.ePassword.Font = new Font("Microsoft Sans Serif", 14f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ePassword.Location = new Point(11, 108);
      this.ePassword.Name = "ePassword";
      this.ePassword.Size = new Size(430, 29);
      this.ePassword.TabIndex = 13;
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(453, 278);
      this.ControlBox = false;
      this.Controls.Add((Control) this.ePassword);
      this.Controls.Add((Control) this.cAlways);
      this.Controls.Add((Control) this.cSec);
      this.Controls.Add((Control) this.lSec);
      this.Controls.Add((Control) this.lSID);
      this.Controls.Add((Control) this.tSID);
      this.Controls.Add((Control) this.lPassword);
      this.Controls.Add((Control) this.pBOTTOM);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (DLG_Wifi_Connect);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = " ";
      this.FormClosed += new FormClosedEventHandler(this.DLG_Wifi_Connect_FormClosed);
      this.Load += new EventHandler(this.DLG_Wifi_Connect_Load);
      this.pBOTTOM.ResumeLayout(false);
      this.pBOTTOM.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
