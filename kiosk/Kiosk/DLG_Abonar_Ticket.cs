// Decompiled with JetBrains decompiler
// Type: Kiosk.DLG_Abonar_Ticket
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
  public class DLG_Abonar_Ticket : Form
  {
    private IContainer components = (IContainer) null;
    public bool OK;
    public Configuracion opciones;
    private Button bCancel;
    private Button bOK;
    private Label lP1;
    private PictureBox pictureBox1;
    private Timer tScan;

    public DLG_Abonar_Ticket(ref Configuracion _opc, string _msg)
    {
      this.OK = false;
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
      this.opciones.LastMouseMove = DateTime.Now;
      this.tScan.Enabled = true;
    }

    private void tScan_Tick(object sender, EventArgs e)
    {
      if ((int) (DateTime.Now - this.opciones.LastMouseMove).TotalSeconds <= 60)
        return;
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
      this.lP1 = new Label();
      this.pictureBox1 = new PictureBox();
      this.bCancel = new Button();
      this.bOK = new Button();
      this.tScan = new Timer(this.components);
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.lP1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lP1.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lP1.Location = new Point(12, 106);
      this.lP1.Name = "lP1";
      this.lP1.Size = new Size(667, 299);
      this.lP1.TabIndex = 14;
      this.lP1.Text = "Ticket";
      this.lP1.TextAlign = ContentAlignment.MiddleCenter;
      this.pictureBox1.BackgroundImage = (Image) Resources.big_warnning;
      this.pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
      this.pictureBox1.Location = new Point(294, 12);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(103, 91);
      this.pictureBox1.TabIndex = 15;
      this.pictureBox1.TabStop = false;
      this.bCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.bCancel.Image = (Image) Resources.ico_del;
      this.bCancel.Location = new Point(12, 408);
      this.bCancel.Name = "bCancel";
      this.bCancel.Size = new Size(48, 48);
      this.bCancel.TabIndex = 2;
      this.bCancel.UseVisualStyleBackColor = true;
      this.bCancel.Click += new EventHandler(this.bCancel_Click);
      this.bOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.bOK.Image = (Image) Resources.ico_ok;
      this.bOK.Location = new Point(631, 408);
      this.bOK.Name = "bOK";
      this.bOK.Size = new Size(48, 48);
      this.bOK.TabIndex = 1;
      this.bOK.UseVisualStyleBackColor = true;
      this.bOK.Click += new EventHandler(this.bOK_Click);
      this.tScan.Interval = 500;
      this.tScan.Tick += new EventHandler(this.tScan_Tick);
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(691, 468);
      this.ControlBox = false;
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.lP1);
      this.Controls.Add((Control) this.bCancel);
      this.Controls.Add((Control) this.bOK);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (DLG_Abonar_Ticket);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "DLG_Barcode";
      this.TopMost = true;
      this.Load += new EventHandler(this.DLG_Abonar_Ticket_Load);
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
    }
  }
}
