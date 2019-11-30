// Decompiled with JetBrains decompiler
// Type: Kiosk.DLG_Dispenser_Out
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
  public class DLG_Dispenser_Out : Form
  {
    public bool IsClosed = false;
    private int oldT = -1;
    private int oldC = -1;
    private IContainer components = (IContainer) null;
    public Configuracion opciones;
    public int TimeOutMsg;
    private DateTime Pausa;
    private Timer ctrl;
    private Label lINFO;

    public DLG_Dispenser_Out(ref Configuracion _opc)
    {
      this.IsClosed = false;
      this.TimeOutMsg = 0;
      this.opciones = _opc;
      this.InitializeComponent();
      this.Localize();
      this.Opacity = 0.7;
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.ResumeLayout();
    }

    private void Update_Info()
    {
      this.lINFO.Text = "••• LIVRAISON DES CHÉQUES CADEAUX •••\r\n\r\nPoints à livrer: " + (object) this.opciones.Disp_Pay_Ticket_Credits + "\r\n\r\nChèques cadeaux pour livrer: " + (object) this.opciones.Disp_Pay_Ticket + "\r\n\r\nChèques cadeaux livrés: " + (object) this.opciones.Disp_Pay_Ticket_Out;
      this.oldT = this.opciones.Disp_Pay_Ticket;
      this.oldC = this.opciones.Disp_Pay_Ticket_Credits;
      this.Invalidate();
    }

    private void ctrl_Tick(object sender, EventArgs e)
    {
      if (this.opciones.Disp_Pay_Ticket_Fail == 0 && (this.oldT != this.opciones.Disp_Pay_Ticket || this.oldC != this.opciones.Disp_Pay_Ticket_Credits))
        this.Update_Info();
      if (this.opciones.Disp_Pay_Ticket_Fail == 1)
      {
        if (this.opciones.Disp_Pay_Ticket_Credits <= 0)
          this.lINFO.Text = "••• LIVRAISON DES CHÉQUES CADEAUX •••\r\n\r\nChèques cadeaux livrés: " + (object) this.opciones.Disp_Pay_Ticket_Out;
        else
          this.lINFO.Text = "••• LIVRAISON DES CHÉQUES CADEAUX •••\r\n\r\nPas de chèques cadeaux disponibles sur le borne\r\n\r\nUtilisez le bon pour obtenir les chèques cadeaux\r\n\r\nPour " + (object) this.opciones.Disp_Pay_Ticket_Credits + " crédits\r\n\r\nChèques cadeaux livrés: " + (object) this.opciones.Disp_Pay_Ticket_Out;
        this.BackColor = Color.OrangeRed;
        this.Invalidate();
        this.opciones.Disp_Pay_Ticket_Fail = 2;
        this.Pausa = DateTime.Now;
      }
      if (this.opciones.Disp_Pay_Ticket_Fail == 2)
        this.TimeOutMsg = 1;
      if (this.opciones.Disp_Pay_Ticket_Fail == 3)
      {
        this.BackColor = Color.DarkGreen;
        if (this.opciones.Disp_Pay_Ticket_Out <= 0)
          this.BackColor = Color.OrangeRed;
        if (this.opciones.Disp_Pay_Ticket_Credits <= 0)
          this.lINFO.Text = "••• LIVRAISON DES CHÉQUES CADEAUX •••\r\n\r\nChèques cadeaux livrés: " + (object) this.opciones.Disp_Pay_Ticket_Out;
        else
          this.lINFO.Text = "••• LIVRAISON DES CHÉQUES CADEAUX •••\r\n\r\nPas de chèques cadeaux disponibles sur le borne\r\n\r\nUtilisez le bon pour obtenir les chèques cadeaux\r\n\r\nPour " + (object) this.opciones.Disp_Pay_Ticket_Credits + " crédits\r\n\r\nChèques cadeaux livrés: " + (object) this.opciones.Disp_Pay_Ticket_Out;
        this.Invalidate();
        this.Pausa = DateTime.Now;
        this.TimeOutMsg = 1;
        this.opciones.Disp_Pay_Ticket_Fail = 2;
      }
      if (this.TimeOutMsg != 1 || (int) (DateTime.Now - this.Pausa).TotalSeconds <= 5)
        return;
      this.IsClosed = true;
      this.Close();
    }

    private void DLG_Dispenser_Out_Load(object sender, EventArgs e)
    {
      this.Update_Info();
      this.ctrl.Enabled = true;
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
      this.ctrl = new Timer(this.components);
      this.lINFO = new Label();
      this.SuspendLayout();
      this.ctrl.Tick += new EventHandler(this.ctrl_Tick);
      this.lINFO.BackColor = Color.Transparent;
      this.lINFO.Dock = DockStyle.Fill;
      this.lINFO.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lINFO.Location = new Point(0, 0);
      this.lINFO.Name = "lINFO";
      this.lINFO.Size = new Size(929, 546);
      this.lINFO.TabIndex = 2;
      this.lINFO.Text = "Dispensing tickets";
      this.lINFO.TextAlign = ContentAlignment.MiddleCenter;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Blue;
      this.ClientSize = new Size(929, 546);
      this.Controls.Add((Control) this.lINFO);
      this.DoubleBuffered = true;
      this.ForeColor = Color.White;
      this.FormBorderStyle = FormBorderStyle.None;
      this.Margin = new Padding(2);
      this.Name = nameof (DLG_Dispenser_Out);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = nameof (DLG_Dispenser_Out);
      this.TopMost = true;
      this.WindowState = FormWindowState.Maximized;
      this.Load += new EventHandler(this.DLG_Dispenser_Out_Load);
      this.ResumeLayout(false);
    }
  }
}
