// Decompiled with JetBrains decompiler
// Type: Kiosk.MSG_Ok
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
  public class MSG_Ok : Form
  {
    private IContainer components = (IContainer) null;
    public bool IsClosed;
    public Configuracion opciones;
    public bool OK;
    public string Missatge;
    private Label lMSG;
    private Panel pBOTTOM;
    private Button bOk;

    public MSG_Ok(ref Configuracion _opc, string _msg)
    {
      this.IsClosed = false;
      this.OK = false;
      this.opciones = _opc;
      this.Missatge = _msg;
      this.InitializeComponent();
      this.lMSG.Text = this.Missatge;
      this.Localize();
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.ResumeLayout();
    }

    private void bOk_Click(object sender, EventArgs e)
    {
      this.OK = true;
      this.Close();
    }

    private void MSG_Ok_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.IsClosed = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lMSG = new Label();
      this.pBOTTOM = new Panel();
      this.bOk = new Button();
      this.pBOTTOM.SuspendLayout();
      this.SuspendLayout();
      this.lMSG.Dock = DockStyle.Fill;
      this.lMSG.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lMSG.Location = new Point(0, 0);
      this.lMSG.Name = "lMSG";
      this.lMSG.Size = new Size(384, 163);
      this.lMSG.TabIndex = 7;
      this.lMSG.Text = "-";
      this.lMSG.TextAlign = ContentAlignment.MiddleCenter;
      this.pBOTTOM.Controls.Add((Control) this.bOk);
      this.pBOTTOM.Dock = DockStyle.Bottom;
      this.pBOTTOM.Location = new Point(0, 163);
      this.pBOTTOM.Name = "pBOTTOM";
      this.pBOTTOM.Size = new Size(384, 48);
      this.pBOTTOM.TabIndex = 6;
      this.bOk.Dock = DockStyle.Right;
      this.bOk.Image = (Image) Resources.ico_ok;
      this.bOk.Location = new Point(336, 0);
      this.bOk.Name = "bOk";
      this.bOk.Size = new Size(48, 48);
      this.bOk.TabIndex = 0;
      this.bOk.UseVisualStyleBackColor = true;
      this.bOk.Click += new EventHandler(this.bOk_Click);
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = Color.White;
      this.ClientSize = new Size(384, 211);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lMSG);
      this.Controls.Add((Control) this.pBOTTOM);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (MSG_Ok);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = " ";
      this.FormClosed += new FormClosedEventHandler(this.MSG_Ok_FormClosed);
      this.pBOTTOM.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
