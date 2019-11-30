// Decompiled with JetBrains decompiler
// Type: Kiosk.InsertCredits
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
  public class InsertCredits : Form
  {
    private IContainer components = (IContainer) null;
    public Configuracion opciones;
    public bool OK;
    public int Modo;
    private Button bOK;
    private Label lInfo;
    private Timer timerAutoClose;

    public InsertCredits(ref Configuracion _opc, int _modo)
    {
      this.OK = false;
      this.InitializeComponent();
      this.opciones = _opc;
      this.Modo = _modo;
      this.Localize();
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.lInfo.Text = this.Modo == 0 ? this.opciones.Localize.Text("Insert credits") : this.opciones.Localize.Text("Ticket used or invalid");
      this.ResumeLayout();
    }

    private void timerAutoClose_Tick(object sender, EventArgs e)
    {
      this.timerAutoClose.Enabled = false;
      this.Close();
    }

    private void bOK_Click(object sender, EventArgs e)
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
      this.lInfo = new Label();
      this.timerAutoClose = new Timer(this.components);
      this.bOK = new Button();
      this.SuspendLayout();
      this.lInfo.Font = new Font("Microsoft Sans Serif", 16f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lInfo.Location = new Point(12, 15);
      this.lInfo.Name = "lInfo";
      this.lInfo.Size = new Size(260, 39);
      this.lInfo.TabIndex = 2;
      this.lInfo.Text = "Insert credits";
      this.lInfo.TextAlign = ContentAlignment.MiddleCenter;
      this.timerAutoClose.Enabled = true;
      this.timerAutoClose.Interval = 5000;
      this.timerAutoClose.Tick += new EventHandler(this.timerAutoClose_Tick);
      this.bOK.Image = (Image) Resources.ico_ok;
      this.bOK.Location = new Point(105, 67);
      this.bOK.Name = "bOK";
      this.bOK.Size = new Size(75, 41);
      this.bOK.TabIndex = 0;
      this.bOK.UseVisualStyleBackColor = true;
      this.bOK.Click += new EventHandler(this.bOK_Click);
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(284, 120);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lInfo);
      this.Controls.Add((Control) this.bOK);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (InsertCredits);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.TopMost = true;
      this.ResumeLayout(false);
    }
  }
}
