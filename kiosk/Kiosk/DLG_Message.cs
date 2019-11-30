// Decompiled with JetBrains decompiler
// Type: Kiosk.DLG_Message
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
  public class DLG_Message : Form
  {
    private IContainer components = (IContainer) null;
    public Configuracion opciones;
    private Label lBuy;
    private Button bOK;
    private Timer tScan;

    public DLG_Message(string _msg, ref Configuracion _opc, bool _warn = false)
    {
      this.opciones = _opc;
      this.InitializeComponent();
      if (_warn)
      {
        this.BackColor = Color.Red;
        this.lBuy.ForeColor = Color.Yellow;
        this.Height *= 2;
        this.Width *= 2;
      }
      this.lBuy.Text = _msg;
    }

    private void bOK_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void tScan_Tick(object sender, EventArgs e)
    {
      if ((int) (DateTime.Now - this.opciones.LastMouseMove).TotalSeconds <= 60)
        return;
      this.Close();
    }

    private void lBuy_Click(object sender, EventArgs e)
    {
    }

    private void DLG_Message_Load(object sender, EventArgs e)
    {
      this.opciones.LastMouseMove = DateTime.Now;
      this.tScan.Enabled = true;
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
      this.lBuy = new Label();
      this.bOK = new Button();
      this.tScan = new Timer(this.components);
      this.SuspendLayout();
      this.lBuy.Dock = DockStyle.Fill;
      this.lBuy.Font = new Font("Microsoft Sans Serif", 16f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lBuy.Location = new Point(0, 0);
      this.lBuy.Name = "lBuy";
      this.lBuy.Size = new Size(284, 71);
      this.lBuy.TabIndex = 12;
      this.lBuy.Text = "-";
      this.lBuy.TextAlign = ContentAlignment.MiddleCenter;
      this.lBuy.Click += new EventHandler(this.lBuy_Click);
      this.bOK.BackgroundImageLayout = ImageLayout.Center;
      this.bOK.Dock = DockStyle.Bottom;
      this.bOK.Image = (Image) Resources.ico_ok;
      this.bOK.Location = new Point(0, 71);
      this.bOK.Name = "bOK";
      this.bOK.Size = new Size(284, 48);
      this.bOK.TabIndex = 0;
      this.bOK.UseVisualStyleBackColor = true;
      this.bOK.Click += new EventHandler(this.bOK_Click);
      this.tScan.Interval = 500;
      this.tScan.Tick += new EventHandler(this.tScan_Tick);
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(284, 119);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lBuy);
      this.Controls.Add((Control) this.bOK);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (DLG_Message);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.TopMost = true;
      this.Load += new EventHandler(this.DLG_Message_Load);
      this.ResumeLayout(false);
    }
  }
}
