// Decompiled with JetBrains decompiler
// Type: Kiosk.DLG_Barcode
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using GLib;
using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
  public class DLG_Barcode : Form
  {
    private IContainer components = (IContainer) null;
    public bool OK;
    public Configuracion opciones;
    public MainWindow MWin;
    private TextBox eBar;
    private Button bOK;
    private Button bRestet;
    private Label lP1;
    private Timer tScan;
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
    private Button button1;
    private Button tX;

    public DLG_Barcode(ref Configuracion _opc)
    {
      this.OK = false;
      this.opciones = _opc;
      this.MWin = (MainWindow) null;
      this.InitializeComponent();
      this.Localize();
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.lP1.Text = this.opciones.Localize.Text("Scan code");
      this.ResumeLayout();
    }

    private void bOK_Click(object sender, EventArgs e)
    {
      KeyPressEventArgs e1 = new KeyPressEventArgs('\r');
      this.eBar_KeyPress(sender, e1);
      this.Close();
    }

    private void bCancel_Click(object sender, EventArgs e)
    {
      this.eBar.Text = "";
    }

    private void bRestet_Click(object sender, EventArgs e)
    {
      this.MWin.TicketToCheck = "";
      this.MWin.TicketOK = 0;
      this.MWin.Parser_Ticket(this.opciones.Srv_User, this.eBar.Text, 0);
      this.eBar.Text = "";
      if (this.MWin.TicketOK != 1)
        return;
      this.MWin.Validate_Ticket();
      this.Close();
    }

    private void eBar_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\r')
        return;
      this.MWin.TicketToCheck = "";
      this.MWin.TicketOK = 0;
      int num = this.MWin.Parser_Ticket(this.opciones.Srv_User, this.eBar.Text, 1);
      this.MWin.LastTicket = this.eBar.Text;
      this.eBar.Text = "";
      if (this.MWin.TicketOK == 1)
      {
        if (num == 1)
        {
          int _ticket = 0;
          int _id = 0;
          Gestion.Decode_Mod10(this.MWin.LastTicket, out _ticket, out _id);
          this.opciones.TicketTemps = _ticket;
          this.opciones.IdTicketTemps = _id;
          if (this.opciones.IdTicketTemps > 0)
            this.MWin.Srv_Sub_Ticket(_id, 0);
        }
        if (num == 2)
        {
          if (this.MWin.ValidacioTicket == null)
          {
            this.MWin.ValidacioTicket = new DLG_ValidarTicket(ref this.opciones);
            this.MWin.ValidacioTicket.MWin = this.MWin;
          }
          if (this.MWin.ValidacioTicket.IsDisposed)
          {
            this.MWin.ValidacioTicket = new DLG_ValidarTicket(ref this.opciones);
            this.MWin.ValidacioTicket.MWin = this.MWin;
          }
          this.MWin.ValidacioTicket.Ticket = this.MWin.LastTicket;
          try
          {
            this.MWin.Opcions.Verificar_Ticket = int.Parse(this.MWin.LastTicket.Substring(1, 11));
          }
          catch
          {
          }
          this.MWin.Srv_Verificar_Ticket(this.MWin.Opcions.Verificar_Ticket, 0);
          this.MWin.ValidacioTicket.Show();
          this.Close();
        }
      }
    }

    private void DLG_Barcode_Load(object sender, EventArgs e)
    {
      this.MWin.TicketOK = 0;
      this.tScan.Enabled = true;
      this.opciones.LastMouseMove = DateTime.Now;
    }

    private void tScan_Tick(object sender, EventArgs e)
    {
      if (this.MWin != null && this.MWin.TicketOK == 1)
      {
        if (this.opciones.ModoPS2 == 1)
          this.eBar.Text = "OK";
        else
          this.Close();
      }
      if ((int) (DateTime.Now - this.opciones.LastMouseMove).TotalSeconds <= 60)
        return;
      this.Close();
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
      this.eBar = new TextBox();
      this.lP1 = new Label();
      this.tScan = new Timer(this.components);
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
      this.tX = new Button();
      this.button1 = new Button();
      this.bRestet = new Button();
      this.bOK = new Button();
      this.SuspendLayout();
      this.eBar.AcceptsReturn = true;
      this.eBar.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.eBar.Location = new Point(13, 44);
      this.eBar.Name = "eBar";
      this.eBar.Size = new Size(276, 38);
      this.eBar.TabIndex = 0;
      this.eBar.KeyPress += new KeyPressEventHandler(this.eBar_KeyPress);
      this.lP1.AutoSize = true;
      this.lP1.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lP1.Location = new Point(79, 6);
      this.lP1.Name = "lP1";
      this.lP1.Size = new Size(142, 31);
      this.lP1.TabIndex = 14;
      this.lP1.Text = "Scan code";
      this.tScan.Interval = 500;
      this.tScan.Tick += new EventHandler(this.tScan_Tick);
      this.tG.FlatStyle = FlatStyle.Flat;
      this.tG.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tG.Location = new Point(68, 253);
      this.tG.Name = "tG";
      this.tG.Size = new Size(56, 56);
      this.tG.TabIndex = 17;
      this.tG.Text = "G";
      this.tG.UseVisualStyleBackColor = true;
      this.tG.Click += new EventHandler(this.tG_Click);
      this.tG.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.tF.FlatStyle = FlatStyle.Flat;
      this.tF.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tF.Location = new Point(13, 253);
      this.tF.Name = "tF";
      this.tF.Size = new Size(56, 56);
      this.tF.TabIndex = 16;
      this.tF.Text = "F";
      this.tF.UseVisualStyleBackColor = true;
      this.tF.Click += new EventHandler(this.tF_Click);
      this.tF.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.tE.FlatStyle = FlatStyle.Flat;
      this.tE.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tE.Location = new Point(233, 198);
      this.tE.Name = "tE";
      this.tE.Size = new Size(56, 56);
      this.tE.TabIndex = 15;
      this.tE.Text = "E";
      this.tE.UseVisualStyleBackColor = true;
      this.tE.Click += new EventHandler(this.tE_Click);
      this.tE.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.tD.FlatStyle = FlatStyle.Flat;
      this.tD.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tD.Location = new Point(178, 198);
      this.tD.Name = "tD";
      this.tD.Size = new Size(56, 56);
      this.tD.TabIndex = 14;
      this.tD.Text = "D";
      this.tD.UseVisualStyleBackColor = true;
      this.tD.Click += new EventHandler(this.tD_Click);
      this.tD.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.tC.FlatStyle = FlatStyle.Flat;
      this.tC.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tC.Location = new Point(123, 198);
      this.tC.Name = "tC";
      this.tC.Size = new Size(56, 56);
      this.tC.TabIndex = 13;
      this.tC.Text = "C";
      this.tC.UseVisualStyleBackColor = true;
      this.tC.Click += new EventHandler(this.tC_Click);
      this.tC.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.tB.FlatStyle = FlatStyle.Flat;
      this.tB.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tB.Location = new Point(68, 198);
      this.tB.Name = "tB";
      this.tB.Size = new Size(56, 56);
      this.tB.TabIndex = 12;
      this.tB.Text = "B";
      this.tB.UseVisualStyleBackColor = true;
      this.tB.Click += new EventHandler(this.tB_Click);
      this.tB.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.tA.FlatStyle = FlatStyle.Flat;
      this.tA.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.tA.Location = new Point(13, 198);
      this.tA.Name = "tA";
      this.tA.Size = new Size(56, 56);
      this.tA.TabIndex = 11;
      this.tA.Text = "A";
      this.tA.UseVisualStyleBackColor = true;
      this.tA.Click += new EventHandler(this.tA_Click);
      this.tA.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.t0.FlatStyle = FlatStyle.Flat;
      this.t0.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.t0.Location = new Point(233, 143);
      this.t0.Name = "t0";
      this.t0.Size = new Size(56, 56);
      this.t0.TabIndex = 10;
      this.t0.Text = "0";
      this.t0.UseVisualStyleBackColor = true;
      this.t0.Click += new EventHandler(this.t0_Click);
      this.t0.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.t9.FlatStyle = FlatStyle.Flat;
      this.t9.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.t9.Location = new Point(178, 143);
      this.t9.Name = "t9";
      this.t9.Size = new Size(56, 56);
      this.t9.TabIndex = 9;
      this.t9.Text = "9";
      this.t9.UseVisualStyleBackColor = true;
      this.t9.Click += new EventHandler(this.t9_Click);
      this.t9.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.t8.FlatStyle = FlatStyle.Flat;
      this.t8.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.t8.Location = new Point(123, 143);
      this.t8.Name = "t8";
      this.t8.Size = new Size(56, 56);
      this.t8.TabIndex = 8;
      this.t8.Text = "8";
      this.t8.UseVisualStyleBackColor = true;
      this.t8.Click += new EventHandler(this.t8_Click);
      this.t8.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.t7.FlatStyle = FlatStyle.Flat;
      this.t7.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.t7.Location = new Point(68, 143);
      this.t7.Name = "t7";
      this.t7.Size = new Size(56, 56);
      this.t7.TabIndex = 7;
      this.t7.Text = "7";
      this.t7.UseVisualStyleBackColor = true;
      this.t7.Click += new EventHandler(this.t7_Click);
      this.t7.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.t6.FlatStyle = FlatStyle.Flat;
      this.t6.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.t6.Location = new Point(13, 143);
      this.t6.Name = "t6";
      this.t6.Size = new Size(56, 56);
      this.t6.TabIndex = 6;
      this.t6.Text = "6";
      this.t6.UseVisualStyleBackColor = true;
      this.t6.Click += new EventHandler(this.t6_Click);
      this.t6.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.t5.FlatStyle = FlatStyle.Flat;
      this.t5.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.t5.Location = new Point(233, 88);
      this.t5.Name = "t5";
      this.t5.Size = new Size(56, 56);
      this.t5.TabIndex = 5;
      this.t5.Text = "5";
      this.t5.UseVisualStyleBackColor = true;
      this.t5.Click += new EventHandler(this.t5_Click);
      this.t5.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.t4.FlatStyle = FlatStyle.Flat;
      this.t4.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.t4.Location = new Point(178, 88);
      this.t4.Name = "t4";
      this.t4.Size = new Size(56, 56);
      this.t4.TabIndex = 4;
      this.t4.Text = "4";
      this.t4.UseVisualStyleBackColor = true;
      this.t4.Click += new EventHandler(this.t4_Click);
      this.t4.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.t3.FlatStyle = FlatStyle.Flat;
      this.t3.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.t3.Location = new Point(123, 88);
      this.t3.Name = "t3";
      this.t3.Size = new Size(56, 56);
      this.t3.TabIndex = 3;
      this.t3.Text = "3";
      this.t3.UseVisualStyleBackColor = true;
      this.t3.Click += new EventHandler(this.t3_Click);
      this.t3.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.t2.FlatStyle = FlatStyle.Flat;
      this.t2.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.t2.Location = new Point(68, 88);
      this.t2.Name = "t2";
      this.t2.Size = new Size(56, 56);
      this.t2.TabIndex = 2;
      this.t2.Text = "2";
      this.t2.UseVisualStyleBackColor = true;
      this.t2.Click += new EventHandler(this.t2_Click);
      this.t2.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.t1.FlatStyle = FlatStyle.Flat;
      this.t1.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.t1.Location = new Point(13, 88);
      this.t1.Name = "t1";
      this.t1.Size = new Size(56, 56);
      this.t1.TabIndex = 1;
      this.t1.Text = "1";
      this.t1.UseVisualStyleBackColor = true;
      this.t1.Click += new EventHandler(this.t1_Click);
      this.t1.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.tX.FlatStyle = FlatStyle.Flat;
      this.tX.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tX.Image = (Image) Resources.backspace;
      this.tX.Location = new Point(123, 253);
      this.tX.Name = "tX";
      this.tX.Size = new Size(56, 56);
      this.tX.TabIndex = 18;
      this.tX.UseVisualStyleBackColor = true;
      this.tX.Click += new EventHandler(this.tX_Click);
      this.tX.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.button1.FlatStyle = FlatStyle.Flat;
      this.button1.Image = (Image) Resources.ico_barcodeX;
      this.button1.Location = new Point(178, 253);
      this.button1.Name = "button1";
      this.button1.Size = new Size(56, 56);
      this.button1.TabIndex = 19;
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.bCancel_Click);
      this.button1.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.bRestet.FlatStyle = FlatStyle.Flat;
      this.bRestet.Image = (Image) Resources.ico_barcode;
      this.bRestet.Location = new Point(13, 315);
      this.bRestet.Name = "bRestet";
      this.bRestet.Size = new Size(111, 48);
      this.bRestet.TabIndex = 20;
      this.bRestet.UseVisualStyleBackColor = true;
      this.bRestet.Click += new EventHandler(this.bRestet_Click);
      this.bRestet.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.bOK.FlatStyle = FlatStyle.Flat;
      this.bOK.Image = (Image) Resources.ico_ok;
      this.bOK.Location = new Point(178, 315);
      this.bOK.Name = "bOK";
      this.bOK.Size = new Size(111, 48);
      this.bOK.TabIndex = 21;
      this.bOK.UseVisualStyleBackColor = true;
      this.bOK.Click += new EventHandler(this.bOK_Click);
      this.bOK.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(301, 374);
      this.ControlBox = false;
      this.Controls.Add((Control) this.tX);
      this.Controls.Add((Control) this.tG);
      this.Controls.Add((Control) this.tF);
      this.Controls.Add((Control) this.tE);
      this.Controls.Add((Control) this.tD);
      this.Controls.Add((Control) this.tC);
      this.Controls.Add((Control) this.tB);
      this.Controls.Add((Control) this.tA);
      this.Controls.Add((Control) this.t0);
      this.Controls.Add((Control) this.t9);
      this.Controls.Add((Control) this.t8);
      this.Controls.Add((Control) this.t7);
      this.Controls.Add((Control) this.t6);
      this.Controls.Add((Control) this.t5);
      this.Controls.Add((Control) this.t4);
      this.Controls.Add((Control) this.t3);
      this.Controls.Add((Control) this.t2);
      this.Controls.Add((Control) this.t1);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.lP1);
      this.Controls.Add((Control) this.bRestet);
      this.Controls.Add((Control) this.bOK);
      this.Controls.Add((Control) this.eBar);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (DLG_Barcode);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = nameof (DLG_Barcode);
      this.TopMost = true;
      this.Load += new EventHandler(this.DLG_Barcode_Load);
      this.KeyPress += new KeyPressEventHandler(this.t1_KeyPress);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
