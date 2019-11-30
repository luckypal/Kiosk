// Decompiled with JetBrains decompiler
// Type: Kiosk.Disconnect
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
  public class Disconnect : Form
  {
    private IContainer components = (IContainer) null;
    public Configuracion opciones;
    public bool OK;
    private Label lInfo;

    public Disconnect(ref Configuracion _opc)
    {
      this.OK = false;
      this.InitializeComponent();
      this.opciones = _opc;
      this.Localize();
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.lInfo.Text = this.opciones.Localize.Text("Disconnect, Kiosk halted");
      this.ResumeLayout();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lInfo = new Label();
      this.SuspendLayout();
      this.lInfo.Dock = DockStyle.Fill;
      this.lInfo.ForeColor = Color.Yellow;
      this.lInfo.Location = new Point(0, 0);
      this.lInfo.Name = "lInfo";
      this.lInfo.Size = new Size(333, 136);
      this.lInfo.TabIndex = 0;
      this.lInfo.Text = "-";
      this.lInfo.TextAlign = ContentAlignment.MiddleCenter;
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = Color.Red;
      this.ClientSize = new Size(333, 136);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lInfo);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (Disconnect);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.TopMost = true;
      this.ResumeLayout(false);
    }
  }
}
