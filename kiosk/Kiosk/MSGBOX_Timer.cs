// Decompiled with JetBrains decompiler
// Type: Kiosk.MSGBOX_Timer
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
  public class MSGBOX_Timer : Form
  {
    private IContainer components = (IContainer) null;
    private Button bOK;
    private Timer tWAIT;
    private Label lINFO;

    public MSGBOX_Timer(string _info, string _boto, int _time, bool _full = false)
    {
      this.InitializeComponent();
      this.lINFO.Text = _info;
      this.bOK.Text = _boto;
      this.tWAIT.Interval = _time * 1000;
      this.tWAIT.Enabled = true;
      if (_boto == "")
      {
        this.bOK.Enabled = false;
        this.bOK.Visible = false;
      }
      if (!_full)
        return;
      this.lINFO.Dock = DockStyle.Fill;
      this.WindowState = FormWindowState.Maximized;
    }

    private void bOK_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void tWAIT_Tick(object sender, EventArgs e)
    {
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
      this.bOK = new Button();
      this.tWAIT = new Timer(this.components);
      this.lINFO = new Label();
      this.SuspendLayout();
      this.bOK.Location = new Point(191, 124);
      this.bOK.Name = "bOK";
      this.bOK.Size = new Size(99, 52);
      this.bOK.TabIndex = 0;
      this.bOK.Text = "OK";
      this.bOK.UseVisualStyleBackColor = true;
      this.bOK.Click += new EventHandler(this.bOK_Click);
      this.tWAIT.Tick += new EventHandler(this.tWAIT_Tick);
      this.lINFO.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lINFO.Location = new Point(13, 13);
      this.lINFO.Name = "lINFO";
      this.lINFO.Size = new Size(457, 98);
      this.lINFO.TabIndex = 1;
      this.lINFO.Text = "-";
      this.lINFO.TextAlign = ContentAlignment.MiddleCenter;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(482, 188);
      this.Controls.Add((Control) this.lINFO);
      this.Controls.Add((Control) this.bOK);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (MSGBOX_Timer);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = nameof (MSGBOX_Timer);
      this.TopMost = true;
      this.ResumeLayout(false);
    }
  }
}
