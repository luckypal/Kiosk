// Decompiled with JetBrains decompiler
// Type: Kiosk.CreditManager
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
  public class CreditManager : Form
  {
    private IContainer components = (IContainer) null;
    public Configuracion Opcions;
    private Button bAddTime;
    private Button bOK;

    public CreditManager(ref Configuracion _opc)
    {
      this.Opcions = _opc;
      this.InitializeComponent();
    }

    private void bOK_Click(object sender, EventArgs e)
    {
      this.Opcions.ComprarTemps = 0;
      this.Close();
    }

    private void bAddTime_Click(object sender, EventArgs e)
    {
      if (this.Opcions.Sub_Credits > new Decimal(0))
        return;
      int num = 0;
      if (this.Opcions.Credits >= (Decimal) this.Opcions.ValorTemps)
        num = this.Opcions.ValorTemps;
      if (num <= 0)
        return;
      this.Opcions.Temps += 60;
      this.Opcions.Sub_Credits = (Decimal) num;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.bAddTime = new Button();
      this.bOK = new Button();
      this.SuspendLayout();
      this.bAddTime.Image = (Image) Resources.ico_add;
      this.bAddTime.Location = new Point(-1, 0);
      this.bAddTime.Name = "bAddTime";
      this.bAddTime.Size = new Size(48, 48);
      this.bAddTime.TabIndex = 1;
      this.bAddTime.UseVisualStyleBackColor = true;
      this.bAddTime.Click += new EventHandler(this.bAddTime_Click);
      this.bOK.BackgroundImageLayout = ImageLayout.Center;
      this.bOK.Image = (Image) Resources.ico_ok;
      this.bOK.Location = new Point(47, 0);
      this.bOK.Name = "bOK";
      this.bOK.Size = new Size(48, 48);
      this.bOK.TabIndex = 0;
      this.bOK.UseVisualStyleBackColor = true;
      this.bOK.Click += new EventHandler(this.bOK_Click);
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(95, 48);
      this.ControlBox = false;
      this.Controls.Add((Control) this.bOK);
      this.Controls.Add((Control) this.bAddTime);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CreditManager);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.Manual;
      this.TopMost = true;
      this.ResumeLayout(false);
    }
  }
}
