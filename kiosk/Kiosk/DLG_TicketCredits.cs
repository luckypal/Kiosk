// Decompiled with JetBrains decompiler
// Type: Kiosk.DLG_TicketCredits
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
  public class DLG_TicketCredits : Form
  {
    private int check = 0;
    private IContainer components = (IContainer) null;
    public bool OK;
    public Configuracion opciones;
    public MainWindow MWin;
    private string eBar;
    private Label lTicket;
    private Label lTime;
    private Label lVal;
    private Label lCODE;
    private Timer tAUTO;

    public DLG_TicketCredits(ref Configuracion _opc)
    {
      this.OK = false;
      this.opciones = _opc;
      this.MWin = (MainWindow) null;
      this.InitializeComponent();
      this.Localize();
      this.lCODE.Text = "-";
      this.lVal.Text = "-";
      this.lTime.Text = "-";
      this.check = 0;
      this.BackColor = Color.Gray;
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.ResumeLayout();
    }

    public void Update_Info()
    {
      this.lCODE.Text = this.opciones.Ticket_Verificar.CRC;
      this.lVal.Text = string.Format("{0}.{1:00} {2}", (object) (this.opciones.Ticket_Verificar.Pago / 100), (object) (this.opciones.Ticket_Verificar.Pago % 100), (object) this.opciones.Localize.Text("Euros"));
      this.lTime.Text = string.Format("{2:00}:{3:00} {0}/{1:00}", (object) this.opciones.Ticket_Verificar.DataT.Day, (object) this.opciones.Ticket_Verificar.DataT.Month, (object) this.opciones.Ticket_Verificar.DataT.Hour, (object) this.opciones.Ticket_Verificar.DataT.Minute);
      switch (this.opciones.Ticket_Verificar.Verificado)
      {
        case 0:
          this.BackColor = Color.Green;
          break;
        case 1:
          this.lVal.Text = "X";
          this.BackColor = Color.Red;
          break;
        case 2:
          this.lVal.Text = "INVALID";
          this.BackColor = Color.Red;
          break;
        default:
          this.lVal.Text = "INVALID";
          this.BackColor = Color.Red;
          break;
      }
      if (this.check == 0)
        this.tAUTO.Enabled = true;
      this.Invalidate();
    }

    public void GoExit()
    {
      this.MWin.TicketToCheck = "";
      this.MWin.TicketOK = 0;
      this.Close();
    }

    public string Ticket
    {
      get
      {
        return this.eBar;
      }
      set
      {
        this.lTicket.Text = value;
        this.eBar = value;
        this.Invalidate();
      }
    }

    private void tAUTO_Tick(object sender, EventArgs e)
    {
      this.check = 1;
      this.tAUTO.Enabled = false;
      if (this.opciones.Ticket_Verificar.Verificado == 0)
      {
        this.MWin.Srv_Anular_Ticket(int.Parse(this.lTicket.Text.Substring(1, 11)), true, 0);
        this.MWin.Internal_Add_Credits(this.opciones.Ticket_Verificar.Pago);
      }
      this.Close();
    }

    private void DLG_TicketCredits_Load(object sender, EventArgs e)
    {
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
      this.lTicket = new Label();
      this.lTime = new Label();
      this.lVal = new Label();
      this.lCODE = new Label();
      this.tAUTO = new Timer(this.components);
      this.SuspendLayout();
      this.lTicket.Dock = DockStyle.Top;
      this.lTicket.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lTicket.Location = new Point(0, 124);
      this.lTicket.Name = "lTicket";
      this.lTicket.Size = new Size(287, 32);
      this.lTicket.TabIndex = 31;
      this.lTicket.Text = "-";
      this.lTicket.TextAlign = ContentAlignment.MiddleCenter;
      this.lTime.Dock = DockStyle.Top;
      this.lTime.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lTime.Location = new Point(0, 92);
      this.lTime.Name = "lTime";
      this.lTime.Size = new Size(287, 32);
      this.lTime.TabIndex = 32;
      this.lTime.Text = "-";
      this.lTime.TextAlign = ContentAlignment.MiddleCenter;
      this.lVal.Dock = DockStyle.Top;
      this.lVal.Font = new Font("Microsoft Sans Serif", 25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lVal.Location = new Point(0, 46);
      this.lVal.Name = "lVal";
      this.lVal.Size = new Size(287, 46);
      this.lVal.TabIndex = 33;
      this.lVal.Text = "-";
      this.lVal.TextAlign = ContentAlignment.MiddleCenter;
      this.lCODE.Dock = DockStyle.Top;
      this.lCODE.Font = new Font("Microsoft Sans Serif", 32f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lCODE.Location = new Point(0, 0);
      this.lCODE.Name = "lCODE";
      this.lCODE.Size = new Size(287, 46);
      this.lCODE.TabIndex = 29;
      this.lCODE.Text = "-";
      this.lCODE.TextAlign = ContentAlignment.MiddleCenter;
      this.tAUTO.Interval = 2000;
      this.tAUTO.Tick += new EventHandler(this.tAUTO_Tick);
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(287, 198);
      this.Controls.Add((Control) this.lTicket);
      this.Controls.Add((Control) this.lTime);
      this.Controls.Add((Control) this.lVal);
      this.Controls.Add((Control) this.lCODE);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (DLG_TicketCredits);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = nameof (DLG_TicketCredits);
      this.TopMost = true;
      this.Load += new EventHandler(this.DLG_TicketCredits_Load);
      this.ResumeLayout(false);
    }
  }
}
