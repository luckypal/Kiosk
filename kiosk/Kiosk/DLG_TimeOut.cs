// Decompiled with JetBrains decompiler
// Type: Kiosk.DLG_TimeOut
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
  public class DLG_TimeOut : Form
  {
    private IContainer components = (IContainer) null;
    public bool OK;
    public Configuracion opciones;
    private int Contdown;
    private DateTime StartsShow;
    private Button bCancel;
    private Button bOK;
    private Label lP1;
    private PictureBox pictureBox1;
    private Timer tScan;
    private Label lCONT;

    public DLG_TimeOut(ref Configuracion _opc, string _msg)
    {
      this.OK = false;
      this.Contdown = 10;
      this.opciones = _opc;
      this.InitializeComponent();
      this.lP1.Text = _msg;
      this.Localize();
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.ResumeLayout();
    }

    private void bOK_Click(object sender, EventArgs e)
    {
      this.OK = true;
      this.Close();
    }

    private void bCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void DLG_Abonar_Ticket_Load(object sender, EventArgs e)
    {
      this.StartsShow = DateTime.Now;
      this.tScan.Enabled = true;
      this.Contdown = 10;
      this.lCONT.Text = string.Concat((object) this.Contdown);
    }

    private void tScan_Tick(object sender, EventArgs e)
    {
      int num = 10 - (int) (DateTime.Now - this.StartsShow).TotalSeconds;
      if (num < 0)
        num = 0;
      if (num != this.Contdown)
      {
        this.Contdown = num;
        this.lCONT.Text = string.Concat((object) this.Contdown);
      }
      if (num > 0)
        return;
      this.OK = true;
      this.Close();
    }

    private void DLG_TimeOut_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.opciones.LastMouseMove = DateTime.Now;
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
      this.lP1 = new Label();
      this.pictureBox1 = new PictureBox();
      this.bCancel = new Button();
      this.bOK = new Button();
      this.tScan = new Timer(this.components);
      this.lCONT = new Label();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.lP1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lP1.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lP1.Location = new Point(12, 106);
      this.lP1.Name = "lP1";
      this.lP1.Size = new Size(514, 72);
      this.lP1.TabIndex = 14;
      this.lP1.Text = "Ticket";
      this.lP1.TextAlign = ContentAlignment.MiddleCenter;
      this.pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pictureBox1.BackgroundImage = (Image) Resources.big_warnning;
      this.pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
      this.pictureBox1.Location = new Point(218, 12);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(103, 91);
      this.pictureBox1.TabIndex = 15;
      this.pictureBox1.TabStop = false;
      this.bCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.bCancel.Image = (Image) Resources.ico_del;
      this.bCancel.Location = new Point(12, 241);
      this.bCancel.Name = "bCancel";
      this.bCancel.Size = new Size(64, 64);
      this.bCancel.TabIndex = 2;
      this.bCancel.UseVisualStyleBackColor = true;
      this.bCancel.Click += new EventHandler(this.bCancel_Click);
      this.bOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.bOK.Image = (Image) Resources.ico_ok;
      this.bOK.Location = new Point(462, 241);
      this.bOK.Name = "bOK";
      this.bOK.Size = new Size(64, 64);
      this.bOK.TabIndex = 1;
      this.bOK.UseVisualStyleBackColor = true;
      this.bOK.Click += new EventHandler(this.bOK_Click);
      this.tScan.Interval = 500;
      this.tScan.Tick += new EventHandler(this.tScan_Tick);
      this.lCONT.Font = new Font("Microsoft Sans Serif", 30f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lCONT.ForeColor = Color.Yellow;
      this.lCONT.Location = new Point(18, 182);
      this.lCONT.Name = "lCONT";
      this.lCONT.Size = new Size(508, 47);
      this.lCONT.TabIndex = 16;
      this.lCONT.Text = "10";
      this.lCONT.TextAlign = ContentAlignment.MiddleCenter;
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = Color.Red;
      this.ClientSize = new Size(538, 316);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lCONT);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.lP1);
      this.Controls.Add((Control) this.bCancel);
      this.Controls.Add((Control) this.bOK);
      this.DoubleBuffered = true;
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (DLG_TimeOut);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "DLG_Barcode";
      this.TopMost = true;
      this.FormClosed += new FormClosedEventHandler(this.DLG_TimeOut_FormClosed);
      this.Load += new EventHandler(this.DLG_Abonar_Ticket_Load);
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
    }
  }
}
