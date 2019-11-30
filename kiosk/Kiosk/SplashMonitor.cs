// Decompiled with JetBrains decompiler
// Type: Kiosk.SplashMonitor
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
  public class SplashMonitor : Form
  {
    private IContainer components = (IContainer) null;
    private Label lNum;

    public SplashMonitor(string _text)
    {
      this.InitializeComponent();
      this.lNum.Text = _text;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lNum = new Label();
      this.SuspendLayout();
      this.lNum.Dock = DockStyle.Fill;
      this.lNum.Font = new Font("Microsoft Sans Serif", 120f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lNum.ForeColor = Color.Red;
      this.lNum.Location = new Point(0, 0);
      this.lNum.Name = "lNum";
      this.lNum.Size = new Size(204, 194);
      this.lNum.TabIndex = 0;
      this.lNum.Text = "1";
      this.lNum.TextAlign = ContentAlignment.MiddleCenter;
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = Color.Black;
      this.ClientSize = new Size(204, 194);
      this.Controls.Add((Control) this.lNum);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (SplashMonitor);
      this.StartPosition = FormStartPosition.Manual;
      this.Text = nameof (SplashMonitor);
      this.TopMost = true;
      this.TransparencyKey = Color.Black;
      this.ResumeLayout(false);
    }
  }
}
