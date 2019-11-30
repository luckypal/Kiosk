// Decompiled with JetBrains decompiler
// Type: Kiosk.DLG_ValidarTicket
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
  public class DLG_ValidarTicket : Form
  {
    private IContainer components = (IContainer) null;
    public bool OK;
    public Configuracion opciones;
    public MainWindow MWin;
    private Label lP1;
    private Button bCancel;
    private Button bOK;
    private TextBox eBar;
    private Panel panel1;
    private Label lTicket;
    private Label lCODE;
    private Button bAnular;
    private Button bValidar;
    private Timer tScan;
    private Label lTime;
    private Label lVal;
    private Label lSec;
    private Label lOKT;
    private Panel panel2;
    private Button tG;
    private Button tF;
    private Button tE;
    private Button tD;
    private Button tC;
    private Button tB;
    private Button tA;
    private Button t0;
    private Button t9;
    private Button t8;
    private Button t7;
    private Button t6;
    private Button t5;
    private Button t4;
    private Button t3;
    private Button t2;
    private Button t1;
    private Button tX;

    public DLG_ValidarTicket(ref Configuracion _opc)
    {
      this.OK = false;
      this.opciones = _opc;
      this.MWin = (MainWindow) null;
      this.InitializeComponent();
      this.Localize();
      this.lCODE.Text = "-";
      this.lVal.Text = "-";
      this.lTime.Text = "-";
      this.panel1.BackColor = Color.Gray;
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.lP1.Text = this.opciones.Localize.Text("Scan code");
      this.ResumeLayout();
    }

    public void Update_Info()
    {
      this.lCODE.Text = this.opciones.Ticket_Verificar.CRC;
      this.lVal.Text = string.Format("{0}.{1:00} {2}", (object) (this.opciones.Ticket_Verificar.Pago / 100), (object) (this.opciones.Ticket_Verificar.Pago % 100), (object) this.opciones.Localize.Text("Euros"));
      string str = string.Concat((object) this.opciones.Ticket_Verificar.Pago);
      this.lTime.Text = string.Format("{3:00}:{4:00} {2:00}/{1:00}/{0:00}", (object) this.opciones.Ticket_Verificar.DataT.Day, (object) this.opciones.Ticket_Verificar.DataT.Month, (object) (this.opciones.Ticket_Verificar.DataT.Year - 2000), (object) this.opciones.Ticket_Verificar.DataT.Hour, (object) this.opciones.Ticket_Verificar.DataT.Minute);
      this.lSec.Text = string.Format("{0}", (object) string.Format("{0:X}/{1:X2}{2:X}{3:X2}/{4:X2}{5:X2}/{6}", (object) this.opciones.Ticket_Verificar.Pago, (object) this.opciones.Ticket_Verificar.DataT.Day, (object) this.opciones.Ticket_Verificar.DataT.Month, (object) (this.opciones.Ticket_Verificar.DataT.Year - 2000), (object) this.opciones.Ticket_Verificar.DataT.Hour, (object) this.opciones.Ticket_Verificar.DataT.Minute, (object) str.Length));
      switch (this.opciones.Ticket_Verificar.Verificado)
      {
        case 0:
          this.panel1.BackColor = Color.Green;
          this.bValidar.Enabled = true;
          this.bValidar.Visible = true;
          this.lOKT.Text = "";
          break;
        case 1:
          this.panel1.BackColor = Color.Red;
          this.bValidar.Enabled = false;
          this.lOKT.Text = "TICKET CANCELLED";
          this.bValidar.Visible = false;
          break;
        case 2:
          this.lCODE.Text = "";
          this.lVal.Text = "";
          this.lTime.Text = "";
          this.lSec.Text = "";
          this.panel1.BackColor = Color.Red;
          this.bValidar.Enabled = false;
          this.lOKT.Text = "TICKET INVALID";
          this.bValidar.Visible = false;
          break;
        default:
          this.lCODE.Text = "";
          this.lVal.Text = "";
          this.lTime.Text = "";
          this.lSec.Text = "";
          this.panel1.BackColor = Color.Red;
          this.bValidar.Enabled = false;
          this.lOKT.Text = "TICKET INVALID";
          this.bValidar.Visible = false;
          break;
      }
      this.Invalidate();
    }

    private void bOK_Click(object sender, EventArgs e)
    {
      this.MWin.TicketToCheck = "";
      this.MWin.TicketOK = 0;
      this.Close();
    }

    private void bCancel_Click(object sender, EventArgs e)
    {
      this.eBar.Text = "";
    }

    public string Ticket
    {
      get
      {
        return this.eBar.Text;
      }
      set
      {
        this.lTicket.Text = value;
        this.Invalidate();
      }
    }

    private void eBar_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\r')
        return;
      this.lCODE.Text = "";
      this.lVal.Text = "";
      this.lTime.Text = "";
      this.lTicket.Text = "";
      this.lSec.Text = "";
      this.lOKT.Text = "";
      this.MWin.TicketToCheck = "";
      this.MWin.TicketOK = 0;
      this.MWin.Parser_Ticket(this.opciones.Srv_User, this.eBar.Text, 1);
      this.eBar.Text = "";
      if (this.MWin.TicketOK == 1)
      {
        this.MWin.Opcions.Verificar_Ticket = 0;
        try
        {
          this.MWin.Opcions.Verificar_Ticket = int.Parse(this.MWin.LastTicket.Substring(1, 11));
        }
        catch
        {
        }
        this.Ticket = this.MWin.LastTicket;
        this.MWin.Srv_Verificar_Ticket(this.MWin.Opcions.Verificar_Ticket, 0);
      }
    }

    private void DLG_ValidarTicket_Load(object sender, EventArgs e)
    {
      this.MWin.TicketOK = 0;
      this.tScan.Enabled = true;
      this.opciones.LastMouseMove = DateTime.Now;
    }

    private void tScan_Tick(object sender, EventArgs e)
    {
      if (this.MWin == null)
        return;
      if (this.MWin.TicketOK == 1 && this.opciones.ModoPS2 == 1)
        this.eBar.Text = "OK";
      if ((int) (DateTime.Now - this.opciones.LastMouseMove).TotalSeconds > 60)
        this.Close();
    }

    private void bValidar_Click(object sender, EventArgs e)
    {
      this.MWin.Srv_Anular_Ticket(int.Parse(this.lTicket.Text.Substring(1, 11)), true, 0);
      this.MWin.Ticket_Out_Conf(this.opciones.Impresora_Tck, (Decimal) this.opciones.Ticket_Verificar.Pago, this.opciones.Ticket_Verificar.Ticket, this.opciones.Ticket_Model, this.opciones.Ticket_Cut, this.opciones.Ticket_N_FEED, this.opciones.Ticket_N_HEAD, this.opciones.Ticket_60mm, this.opciones.Ticket_Verificar.DataT, this.opciones.Ticket_Verificar.CRC);
    }

    private void bAnular_Click(object sender, EventArgs e)
    {
      KeyPressEventArgs e1 = new KeyPressEventArgs('\r');
      this.eBar_KeyPress(sender, e1);
    }

    private void t1_Click(object sender, EventArgs e)
    {
      this.eBar.Text += "1";
    }

    private void t2_Click(object sender, EventArgs e)
    {
      this.eBar.Text += "2";
    }

    private void t3_Click(object sender, EventArgs e)
    {
      this.eBar.Text += "3";
    }

    private void t4_Click(object sender, EventArgs e)
    {
      this.eBar.Text += "4";
    }

    private void t5_Click(object sender, EventArgs e)
    {
      this.eBar.Text += "5";
    }

    private void t6_Click(object sender, EventArgs e)
    {
      this.eBar.Text += "6";
    }

    private void tB_Click(object sender, EventArgs e)
    {
      this.eBar.Text += "B";
    }

    private void tA_Click(object sender, EventArgs e)
    {
      this.eBar.Text += "A";
    }

    private void t7_Click(object sender, EventArgs e)
    {
      this.eBar.Text += "7";
    }

    private void t8_Click(object sender, EventArgs e)
    {
      this.eBar.Text += "8";
    }

    private void t9_Click(object sender, EventArgs e)
    {
      this.eBar.Text += "9";
    }

    private void t0_Click(object sender, EventArgs e)
    {
      this.eBar.Text += "0";
    }

    private void tC_Click(object sender, EventArgs e)
    {
      this.eBar.Text += "C";
    }

    private void tD_Click(object sender, EventArgs e)
    {
      this.eBar.Text += "D";
    }

    private void tE_Click(object sender, EventArgs e)
    {
      this.eBar.Text += "E";
    }

    private void tF_Click(object sender, EventArgs e)
    {
      this.eBar.Text += "F";
    }

    private void tG_Click(object sender, EventArgs e)
    {
      this.eBar.Text += "G";
    }

    private void t1_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == '1')
        this.eBar.Text += "1";
      if (e.KeyChar == '2')
        this.eBar.Text += "2";
      if (e.KeyChar == '3')
        this.eBar.Text += "3";
      if (e.KeyChar == '4')
        this.eBar.Text += "4";
      if (e.KeyChar == '5')
        this.eBar.Text += "5";
      if (e.KeyChar == '6')
        this.eBar.Text += "6";
      if (e.KeyChar == '7')
        this.eBar.Text += "7";
      if (e.KeyChar == '8')
        this.eBar.Text += "8";
      if (e.KeyChar == '9')
        this.eBar.Text += "9";
      if (e.KeyChar == '0')
        this.eBar.Text += "0";
      if (e.KeyChar == 'A')
        this.eBar.Text += "A";
      if (e.KeyChar == 'a')
        this.eBar.Text += "A";
      if (e.KeyChar == 'B')
        this.eBar.Text += "B";
      if (e.KeyChar == 'b')
        this.eBar.Text += "B";
      if (e.KeyChar == 'C')
        this.eBar.Text += "C";
      if (e.KeyChar == 'c')
        this.eBar.Text += "C";
      if (e.KeyChar == 'D')
        this.eBar.Text += "D";
      if (e.KeyChar == 'd')
        this.eBar.Text += "D";
      if (e.KeyChar == 'E')
        this.eBar.Text += "E";
      if (e.KeyChar == 'e')
        this.eBar.Text += "E";
      if (e.KeyChar == 'F')
        this.eBar.Text += "F";
      if (e.KeyChar == 'f')
        this.eBar.Text += "F";
      if (e.KeyChar == 'G')
        this.eBar.Text += "G";
      if (e.KeyChar == 'g')
        this.eBar.Text += "G";
      if (e.KeyChar == '\b' && this.eBar.Text.Length > 0)
        this.eBar.Text = this.eBar.Text.Substring(0, this.eBar.Text.Length - 1);
      if (e.KeyChar == '\r')
        this.eBar_KeyPress(sender, e);
      if (e.KeyChar != '\x001B')
        return;
      this.MWin.TicketToCheck = "";
      this.MWin.TicketOK = 0;
      this.Close();
    }

    private void tX_Click(object sender, EventArgs e)
    {
      if (this.eBar.Text.Length <= 0)
        return;
      this.eBar.Text = this.eBar.Text.Substring(0, this.eBar.Text.Length - 1);
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
      this.eBar = new TextBox();
      this.panel1 = new Panel();
      this.lOKT = new Label();
      this.lSec = new Label();
      this.lTicket = new Label();
      this.bValidar = new Button();
      this.lTime = new Label();
      this.lVal = new Label();
      this.lCODE = new Label();
      this.tScan = new Timer(this.components);
      this.panel2 = new Panel();
      this.tX = new Button();
      this.tG = new Button();
      this.tF = new Button();
      this.tE = new Button();
      this.tD = new Button();
      this.tC = new Button();
      this.tB = new Button();
      this.tA = new Button();
      this.t0 = new Button();
      this.t9 = new Button();
      this.t8 = new Button();
      this.t7 = new Button();
      this.t6 = new Button();
      this.t5 = new Button();
      this.t4 = new Button();
      this.t3 = new Button();
      this.t2 = new Button();
      this.t1 = new Button();
      this.bCancel = new Button();
      this.bAnular = new Button();
      this.bOK = new Button();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.lP1.Dock = DockStyle.Top;
      this.lP1.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lP1.Location = new Point(0, 0);
      this.lP1.Name = "lP1";
      this.lP1.Size = new Size(277, 31);
      this.lP1.TabIndex = 19;
      this.lP1.Text = "Code";
      this.lP1.TextAlign = ContentAlignment.MiddleCenter;
      this.eBar.AcceptsReturn = true;
      this.eBar.Dock = DockStyle.Top;
      this.eBar.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.eBar.Location = new Point(0, 31);
      this.eBar.Name = "eBar";
      this.eBar.Size = new Size(277, 38);
      this.eBar.TabIndex = 0;
      this.eBar.KeyPress += new KeyPressEventHandler(this.eBar_KeyPress);
      this.panel1.Controls.Add((Control) this.lOKT);
      this.panel1.Controls.Add((Control) this.lSec);
      this.panel1.Controls.Add((Control) this.lTicket);
      this.panel1.Controls.Add((Control) this.bValidar);
      this.panel1.Controls.Add((Control) this.lTime);
      this.panel1.Controls.Add((Control) this.lVal);
      this.panel1.Controls.Add((Control) this.lCODE);
      this.panel1.Location = new Point(12, 12);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(295, 343);
      this.panel1.TabIndex = 20;
      this.lOKT.Dock = DockStyle.Top;
      this.lOKT.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lOKT.Location = new Point(0, 188);
      this.lOKT.Name = "lOKT";
      this.lOKT.Size = new Size(295, 32);
      this.lOKT.TabIndex = 24;
      this.lOKT.Text = "-";
      this.lOKT.TextAlign = ContentAlignment.MiddleCenter;
      this.lSec.Dock = DockStyle.Top;
      this.lSec.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lSec.Location = new Point(0, 156);
      this.lSec.Name = "lSec";
      this.lSec.Size = new Size(295, 32);
      this.lSec.TabIndex = 23;
      this.lSec.Text = "-";
      this.lSec.TextAlign = ContentAlignment.MiddleCenter;
      this.lTicket.Dock = DockStyle.Top;
      this.lTicket.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lTicket.Location = new Point(0, 124);
      this.lTicket.Name = "lTicket";
      this.lTicket.Size = new Size(295, 32);
      this.lTicket.TabIndex = 20;
      this.lTicket.Text = "-";
      this.lTicket.TextAlign = ContentAlignment.MiddleCenter;
      this.bValidar.Dock = DockStyle.Bottom;
      this.bValidar.Enabled = false;
      this.bValidar.FlatStyle = FlatStyle.Flat;
      this.bValidar.Image = (Image) Resources.TicketOK;
      this.bValidar.Location = new Point(0, 289);
      this.bValidar.Name = "bValidar";
      this.bValidar.Size = new Size(295, 54);
      this.bValidar.TabIndex = 0;
      this.bValidar.UseVisualStyleBackColor = false;
      this.bValidar.Click += new EventHandler(this.bValidar_Click);
      this.bValidar.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.lTime.Dock = DockStyle.Top;
      this.lTime.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lTime.Location = new Point(0, 92);
      this.lTime.Name = "lTime";
      this.lTime.Size = new Size(295, 32);
      this.lTime.TabIndex = 21;
      this.lTime.Text = "-";
      this.lTime.TextAlign = ContentAlignment.MiddleCenter;
      this.lVal.Dock = DockStyle.Top;
      this.lVal.Font = new Font("Microsoft Sans Serif", 25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lVal.Location = new Point(0, 46);
      this.lVal.Name = "lVal";
      this.lVal.Size = new Size(295, 46);
      this.lVal.TabIndex = 22;
      this.lVal.Text = "-";
      this.lVal.TextAlign = ContentAlignment.MiddleCenter;
      this.lCODE.Dock = DockStyle.Top;
      this.lCODE.Font = new Font("Microsoft Sans Serif", 32f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lCODE.Location = new Point(0, 0);
      this.lCODE.Name = "lCODE";
      this.lCODE.Size = new Size(295, 46);
      this.lCODE.TabIndex = 19;
      this.lCODE.Text = "-";
      this.lCODE.TextAlign = ContentAlignment.MiddleCenter;
      this.tScan.Interval = 500;
      this.tScan.Tick += new EventHandler(this.tScan_Tick);
      this.panel2.Controls.Add((Control) this.tX);
      this.panel2.Controls.Add((Control) this.tG);
      this.panel2.Controls.Add((Control) this.tF);
      this.panel2.Controls.Add((Control) this.tE);
      this.panel2.Controls.Add((Control) this.tD);
      this.panel2.Controls.Add((Control) this.tC);
      this.panel2.Controls.Add((Control) this.tB);
      this.panel2.Controls.Add((Control) this.tA);
      this.panel2.Controls.Add((Control) this.t0);
      this.panel2.Controls.Add((Control) this.t9);
      this.panel2.Controls.Add((Control) this.t8);
      this.panel2.Controls.Add((Control) this.t7);
      this.panel2.Controls.Add((Control) this.t6);
      this.panel2.Controls.Add((Control) this.t5);
      this.panel2.Controls.Add((Control) this.t4);
      this.panel2.Controls.Add((Control) this.t3);
      this.panel2.Controls.Add((Control) this.t2);
      this.panel2.Controls.Add((Control) this.t1);
      this.panel2.Controls.Add((Control) this.eBar);
      this.panel2.Controls.Add((Control) this.lP1);
      this.panel2.Controls.Add((Control) this.bCancel);
      this.panel2.Controls.Add((Control) this.bAnular);
      this.panel2.Controls.Add((Control) this.bOK);
      this.panel2.Location = new Point(313, 12);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(277, 342);
      this.panel2.TabIndex = 21;
      this.tX.FlatStyle = FlatStyle.Flat;
      this.tX.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tX.Image = (Image) Resources.backspace;
      this.tX.Location = new Point(110, 235);
      this.tX.Name = "tX";
      this.tX.Size = new Size(56, 56);
      this.tX.TabIndex = 18;
      this.tX.UseVisualStyleBackColor = true;
      this.tX.Click += new EventHandler(this.tX_Click);
      this.tX.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.tG.FlatStyle = FlatStyle.Flat;
      this.tG.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tG.Location = new Point(55, 235);
      this.tG.Name = "tG";
      this.tG.Size = new Size(56, 56);
      this.tG.TabIndex = 17;
      this.tG.Text = "G";
      this.tG.UseVisualStyleBackColor = true;
      this.tG.Click += new EventHandler(this.tG_Click);
      this.tG.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.tF.FlatStyle = FlatStyle.Flat;
      this.tF.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tF.Location = new Point(0, 235);
      this.tF.Name = "tF";
      this.tF.Size = new Size(56, 56);
      this.tF.TabIndex = 16;
      this.tF.Text = "F";
      this.tF.UseVisualStyleBackColor = true;
      this.tF.Click += new EventHandler(this.tF_Click);
      this.tF.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.tE.FlatStyle = FlatStyle.Flat;
      this.tE.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tE.Location = new Point(220, 180);
      this.tE.Name = "tE";
      this.tE.Size = new Size(56, 56);
      this.tE.TabIndex = 15;
      this.tE.Text = "E";
      this.tE.UseVisualStyleBackColor = true;
      this.tE.Click += new EventHandler(this.tE_Click);
      this.tE.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.tD.FlatStyle = FlatStyle.Flat;
      this.tD.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tD.Location = new Point(165, 180);
      this.tD.Name = "tD";
      this.tD.Size = new Size(56, 56);
      this.tD.TabIndex = 14;
      this.tD.Text = "D";
      this.tD.UseVisualStyleBackColor = true;
      this.tD.Click += new EventHandler(this.tD_Click);
      this.tD.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.tC.FlatStyle = FlatStyle.Flat;
      this.tC.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tC.Location = new Point(110, 180);
      this.tC.Name = "tC";
      this.tC.Size = new Size(56, 56);
      this.tC.TabIndex = 13;
      this.tC.Text = "C";
      this.tC.UseVisualStyleBackColor = true;
      this.tC.Click += new EventHandler(this.tC_Click);
      this.tC.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.tB.FlatStyle = FlatStyle.Flat;
      this.tB.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tB.Location = new Point(55, 180);
      this.tB.Name = "tB";
      this.tB.Size = new Size(56, 56);
      this.tB.TabIndex = 12;
      this.tB.Text = "B";
      this.tB.UseVisualStyleBackColor = true;
      this.tB.Click += new EventHandler(this.tB_Click);
      this.tB.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.tA.FlatStyle = FlatStyle.Flat;
      this.tA.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tA.Location = new Point(0, 180);
      this.tA.Name = "tA";
      this.tA.Size = new Size(56, 56);
      this.tA.TabIndex = 11;
      this.tA.Text = "A";
      this.tA.UseVisualStyleBackColor = true;
      this.tA.Click += new EventHandler(this.tA_Click);
      this.tA.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.t0.FlatStyle = FlatStyle.Flat;
      this.t0.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.t0.Location = new Point(220, 125);
      this.t0.Name = "t0";
      this.t0.Size = new Size(56, 56);
      this.t0.TabIndex = 10;
      this.t0.Text = "0";
      this.t0.UseVisualStyleBackColor = true;
      this.t0.Click += new EventHandler(this.t0_Click);
      this.t0.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.t9.FlatStyle = FlatStyle.Flat;
      this.t9.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.t9.Location = new Point(165, 125);
      this.t9.Name = "t9";
      this.t9.Size = new Size(56, 56);
      this.t9.TabIndex = 9;
      this.t9.Text = "9";
      this.t9.UseVisualStyleBackColor = true;
      this.t9.Click += new EventHandler(this.t9_Click);
      this.t9.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.t8.FlatStyle = FlatStyle.Flat;
      this.t8.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.t8.Location = new Point(110, 125);
      this.t8.Name = "t8";
      this.t8.Size = new Size(56, 56);
      this.t8.TabIndex = 8;
      this.t8.Text = "8";
      this.t8.UseVisualStyleBackColor = true;
      this.t8.Click += new EventHandler(this.t8_Click);
      this.t8.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.t7.FlatStyle = FlatStyle.Flat;
      this.t7.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.t7.Location = new Point(55, 125);
      this.t7.Name = "t7";
      this.t7.Size = new Size(56, 56);
      this.t7.TabIndex = 7;
      this.t7.Text = "7";
      this.t7.UseVisualStyleBackColor = true;
      this.t7.Click += new EventHandler(this.t7_Click);
      this.t7.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.t6.FlatStyle = FlatStyle.Flat;
      this.t6.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.t6.Location = new Point(0, 125);
      this.t6.Name = "t6";
      this.t6.Size = new Size(56, 56);
      this.t6.TabIndex = 6;
      this.t6.Text = "6";
      this.t6.UseVisualStyleBackColor = true;
      this.t6.Click += new EventHandler(this.t6_Click);
      this.t6.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.t5.FlatStyle = FlatStyle.Flat;
      this.t5.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.t5.Location = new Point(220, 70);
      this.t5.Name = "t5";
      this.t5.Size = new Size(56, 56);
      this.t5.TabIndex = 5;
      this.t5.Text = "5";
      this.t5.UseVisualStyleBackColor = true;
      this.t5.Click += new EventHandler(this.t5_Click);
      this.t5.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.t4.FlatStyle = FlatStyle.Flat;
      this.t4.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.t4.Location = new Point(165, 70);
      this.t4.Name = "t4";
      this.t4.Size = new Size(56, 56);
      this.t4.TabIndex = 4;
      this.t4.Text = "4";
      this.t4.UseVisualStyleBackColor = true;
      this.t4.Click += new EventHandler(this.t4_Click);
      this.t4.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.t3.FlatStyle = FlatStyle.Flat;
      this.t3.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.t3.Location = new Point(110, 70);
      this.t3.Name = "t3";
      this.t3.Size = new Size(56, 56);
      this.t3.TabIndex = 3;
      this.t3.Text = "3";
      this.t3.UseVisualStyleBackColor = true;
      this.t3.Click += new EventHandler(this.t3_Click);
      this.t3.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.t2.FlatStyle = FlatStyle.Flat;
      this.t2.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.t2.Location = new Point(55, 70);
      this.t2.Name = "t2";
      this.t2.Size = new Size(56, 56);
      this.t2.TabIndex = 2;
      this.t2.Text = "2";
      this.t2.UseVisualStyleBackColor = true;
      this.t2.Click += new EventHandler(this.t2_Click);
      this.t2.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.t1.FlatStyle = FlatStyle.Flat;
      this.t1.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.t1.Location = new Point(0, 70);
      this.t1.Name = "t1";
      this.t1.Size = new Size(56, 56);
      this.t1.TabIndex = 1;
      this.t1.Text = "1";
      this.t1.UseVisualStyleBackColor = true;
      this.t1.Click += new EventHandler(this.t1_Click);
      this.t1.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.bCancel.FlatStyle = FlatStyle.Flat;
      this.bCancel.Image = (Image) Resources.ico_barcodeX;
      this.bCancel.Location = new Point(165, 235);
      this.bCancel.Name = "bCancel";
      this.bCancel.Size = new Size(56, 56);
      this.bCancel.TabIndex = 19;
      this.bCancel.UseVisualStyleBackColor = true;
      this.bCancel.Click += new EventHandler(this.bCancel_Click);
      this.bCancel.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.bAnular.FlatStyle = FlatStyle.Flat;
      this.bAnular.Image = (Image) Resources.TicketFind;
      this.bAnular.Location = new Point(0, 290);
      this.bAnular.Name = "bAnular";
      this.bAnular.Size = new Size(111, 52);
      this.bAnular.TabIndex = 20;
      this.bAnular.UseVisualStyleBackColor = true;
      this.bAnular.Click += new EventHandler(this.bAnular_Click);
      this.bAnular.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.bOK.FlatStyle = FlatStyle.Flat;
      this.bOK.Image = (Image) Resources.dlgExit;
      this.bOK.Location = new Point(165, 290);
      this.bOK.Name = "bOK";
      this.bOK.Size = new Size(111, 52);
      this.bOK.TabIndex = 21;
      this.bOK.UseVisualStyleBackColor = true;
      this.bOK.Click += new EventHandler(this.bOK_Click);
      this.bOK.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(602, 367);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (DLG_ValidarTicket);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = nameof (DLG_ValidarTicket);
      this.TopMost = true;
      this.Load += new EventHandler(this.DLG_ValidarTicket_Load);
      this.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.panel1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
