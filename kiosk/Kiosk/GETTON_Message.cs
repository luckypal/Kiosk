// Decompiled with JetBrains decompiler
// Type: Kiosk.GETTON_Message
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
  public class GETTON_Message : Form
  {
    private IContainer components = (IContainer) null;
    public Configuracion opciones;
    private Label lBuy;
    private Button bOK;

    public GETTON_Message(string _msg, ref Configuracion _opc)
    {
      this.opciones = _opc;
      this.InitializeComponent();
      this.Top = 0;
      this.Left = 300;
      this.lBuy.Text = _msg;
    }

    private void bOK_Click(object sender, EventArgs e)
    {
      this.opciones.Add_Getton = 1;
    }

    private void DLG_Message_Load(object sender, EventArgs e)
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
      this.lBuy = new Label();
      this.bOK = new Button();
      this.SuspendLayout();
      this.lBuy.Dock = DockStyle.Fill;
      this.lBuy.Font = new Font("Microsoft Sans Serif", 16f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lBuy.Location = new Point(0, 0);
      this.lBuy.Name = "lBuy";
      this.lBuy.Size = new Size(424, 48);
      this.lBuy.TabIndex = 12;
      this.lBuy.Text = "-";
      this.lBuy.TextAlign = ContentAlignment.MiddleCenter;
      this.bOK.BackgroundImageLayout = ImageLayout.Center;
      this.bOK.Dock = DockStyle.Right;
      this.bOK.Image = (Image) Resources.ico_ok;
      this.bOK.Location = new Point(424, 0);
      this.bOK.Name = "bOK";
      this.bOK.Size = new Size(74, 48);
      this.bOK.TabIndex = 0;
      this.bOK.UseVisualStyleBackColor = true;
      this.bOK.Click += new EventHandler(this.bOK_Click);
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(498, 48);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lBuy);
      this.Controls.Add((Control) this.bOK);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (GETTON_Message);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.Manual;
      this.TopMost = true;
      this.Load += new EventHandler(this.DLG_Message_Load);
      this.ResumeLayout(false);
    }
  }
}
