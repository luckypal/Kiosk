// Decompiled with JetBrains decompiler
// Type: Kiosk.CreditManagerFull
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using Kiosk.Properties;
using LCDLabel;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
  public class CreditManagerFull : Form
  {
    private IContainer components = (IContainer) null;
    private Decimal ucrd;
    public Configuracion opciones;
    private Button bOK;
    private Button bAddTime;
    private LcdLabel lcdClock;
    private Label lBuy;
    private Label lCredits;
    private Timer timerCredits;

    public CreditManagerFull(ref Configuracion _opc)
    {
      this.opciones = _opc;
      this.InitializeComponent();
      this.lcdClock.Text = string.Format("{0:00}:{1:00}", (object) (this.opciones.Temps / 60), (object) (this.opciones.Temps % 60));
      this.lCredits.Text = string.Format("{0}: {1}", (object) this.opciones.Localize.Text("Credits"), (object) this.opciones.Credits);
      this.ucrd = this.opciones.Credits;
      this.Localize();
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.lBuy.Text = this.opciones.Localize.Text("Buy time");
      this.lCredits.Text = this.opciones.Localize.Text("Credits:");
      this.ResumeLayout();
    }

    private void bExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void bOK_Click(object sender, EventArgs e)
    {
      this.opciones.ComprarTemps = 0;
      this.Close();
    }

    private void bAddTime_Click(object sender, EventArgs e)
    {
      if (this.opciones.Sub_Credits > new Decimal(0))
        return;
      int num = 0;
      if (this.opciones.Credits >= (Decimal) this.opciones.ValorTemps)
        num = this.opciones.ValorTemps;
      if (num > 0)
      {
        this.opciones.Temps += 60;
        this.opciones.Sub_Credits = (Decimal) num;
      }
      this.lcdClock.Text = string.Format("{0:00}:{1:00}", (object) (this.opciones.Temps / 60), (object) (this.opciones.Temps % 60));
    }

    private void timerCredits_Tick(object sender, EventArgs e)
    {
      if (this.ucrd != this.opciones.Credits)
        this.lCredits.Text = string.Format("{0}: {1}", (object) this.opciones.Localize.Text("Credits"), (object) this.opciones.Credits);
      this.ucrd = this.opciones.Credits;
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
      this.bAddTime = new Button();
      this.lBuy = new Label();
      this.lCredits = new Label();
      this.lcdClock = new LcdLabel();
      this.timerCredits = new Timer(this.components);
      this.SuspendLayout();
      this.bOK.BackgroundImageLayout = ImageLayout.Center;
      this.bOK.Image = (Image) Resources.ico_ok;
      this.bOK.Location = new Point(146, 108);
      this.bOK.Name = "bOK";
      this.bOK.Size = new Size(48, 48);
      this.bOK.TabIndex = 0;
      this.bOK.UseVisualStyleBackColor = true;
      this.bOK.Click += new EventHandler(this.bOK_Click);
      this.bAddTime.Image = (Image) Resources.ico_add;
      this.bAddTime.Location = new Point(90, 108);
      this.bAddTime.Name = "bAddTime";
      this.bAddTime.Size = new Size(48, 48);
      this.bAddTime.TabIndex = 1;
      this.bAddTime.UseVisualStyleBackColor = true;
      this.bAddTime.Click += new EventHandler(this.bAddTime_Click);
      this.lBuy.Dock = DockStyle.Top;
      this.lBuy.Font = new Font("Microsoft Sans Serif", 16f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lBuy.Location = new Point(0, 0);
      this.lBuy.Name = "lBuy";
      this.lBuy.Size = new Size(284, 36);
      this.lBuy.TabIndex = 10;
      this.lBuy.Text = "Buy Time";
      this.lBuy.TextAlign = ContentAlignment.MiddleCenter;
      this.lCredits.Font = new Font("Microsoft Sans Serif", 16f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lCredits.Location = new Point(0, 64);
      this.lCredits.Name = "lCredits";
      this.lCredits.Size = new Size(284, 36);
      this.lCredits.TabIndex = 12;
      this.lCredits.Text = "Credits: 0";
      this.lCredits.TextAlign = ContentAlignment.MiddleCenter;
      this.lcdClock.BackGround = SystemColors.Control;
      this.lcdClock.BorderColor = Color.Black;
      this.lcdClock.BorderSpace = 3;
      this.lcdClock.CharSpacing = 2;
      this.lcdClock.DotMatrix = DotMatrix.mat5x7;
      this.lcdClock.LineSpacing = 2;
      this.lcdClock.Location = new Point(99, 36);
      this.lcdClock.Name = "lcdClock";
      this.lcdClock.NumberOfCharacters = 5;
      this.lcdClock.PixelHeight = 2;
      this.lcdClock.PixelOff = Color.FromArgb(0, 170, 170, 170);
      this.lcdClock.PixelOn = Color.Black;
      this.lcdClock.PixelShape = PixelShape.Square;
      this.lcdClock.PixelSize = PixelSize.pix2x2;
      this.lcdClock.PixelSpacing = 1;
      this.lcdClock.PixelWidth = 2;
      this.lcdClock.Size = new Size(86, 28);
      this.lcdClock.TabIndex = 9;
      this.lcdClock.Text = "00:00";
      this.lcdClock.TextLines = 1;
      this.timerCredits.Enabled = true;
      this.timerCredits.Tick += new EventHandler(this.timerCredits_Tick);
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(284, 174);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lCredits);
      this.Controls.Add((Control) this.lBuy);
      this.Controls.Add((Control) this.lcdClock);
      this.Controls.Add((Control) this.bOK);
      this.Controls.Add((Control) this.bAddTime);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (CreditManagerFull);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.TopMost = true;
      this.ResumeLayout(false);
    }
  }
}
