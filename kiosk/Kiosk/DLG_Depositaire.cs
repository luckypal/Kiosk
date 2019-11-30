// Decompiled with JetBrains decompiler
// Type: Kiosk.DLG_Depositaire
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Kiosk
{
  public class DLG_Depositaire : Form
  {
    private IContainer components = (IContainer) null;
    public bool OK;
    public Configuracion opciones;
    public MainWindow MWin;
    private TextBox eLine2;
    private TextBox eLine1;
    private Label lLine2;
    private Label lLine1;
    private Button bCancel;
    private Button bOk;
    private TextBox eLine3;
    private Label lLine3;
    private TextBox eLine4;
    private Label lLine4;
    private TextBox eLine5;
    private Label lLine5;
    private CheckBox cTicket;
    private Button bTick;
    private Button bBar;
    private TextBox eBar;
    private Label lBar;
    private Label lP1;
    private ComboBox cP1;
    private TextBox eLine6;
    private Label lLine6;
    private CheckBox cPS2;
    private ComboBox cPModel;
    private Label lPModel;
    private CheckBox cCut;
    private Label lPLines;
    private TextBox tPLines;
    private Button button1;
    private CheckBox c60mm;
    private TextBox tPHEAD;
    private Label label1;

    public DLG_Depositaire(ref Configuracion _opc)
    {
      this.OK = false;
      this.opciones = _opc;
      this.MWin = (MainWindow) null;
      this.InitializeComponent();
      this.eLine1.Text = this.opciones.Srv_ID_Lin1;
      this.eLine2.Text = this.opciones.Srv_ID_Lin2;
      this.eLine3.Text = this.opciones.Srv_ID_Lin3;
      this.eLine4.Text = this.opciones.Srv_ID_Lin4;
      this.eLine5.Text = this.opciones.Srv_ID_Lin5;
      this.eLine6.Text = this.opciones.Srv_ID_LinBottom;
      this.opciones.ModoTickets = 1;
      this.cTicket.Checked = true;
      this.cTicket.Enabled = false;
      this.cPS2.Checked = this.opciones.ModoPS2 == 1;
      this.Localize();
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.Text = this.opciones.Localize.Text("Ticket");
      this.lLine1.Text = this.opciones.Localize.Text("Name");
      this.lLine2.Text = this.opciones.Localize.Text("Line 1");
      this.lLine3.Text = this.opciones.Localize.Text("Line 2");
      this.lLine4.Text = this.opciones.Localize.Text("Line 3");
      this.lLine5.Text = this.opciones.Localize.Text("RC");
      this.lLine6.Text = this.opciones.Localize.Text("End Ticket Text");
      this.cTicket.Text = this.opciones.Localize.Text("Ticket system");
      this.lP1.Text = this.opciones.Localize.Text("Printer");
      this.bTick.Text = this.opciones.Localize.Text("Test");
      this.bBar.Text = this.opciones.Localize.Text("Detect");
      this.lBar.Text = this.opciones.Localize.Text("Barcode reader");
      this.ResumeLayout();
    }

    private void bOk_Click(object sender, EventArgs e)
    {
      if (this.opciones.modo_XP == 1)
      {
        int num = (int) MessageBox.Show("XP Detected. System ticket not avalilable");
        this.cTicket.Checked = false;
      }
      this.OK = true;
      this.opciones.Srv_ID_Lin1 = this.eLine1.Text;
      this.opciones.Srv_ID_Lin2 = this.eLine2.Text;
      this.opciones.Srv_ID_Lin3 = this.eLine3.Text;
      this.opciones.Srv_ID_Lin4 = this.eLine4.Text;
      this.opciones.Srv_ID_Lin5 = this.eLine5.Text;
      this.opciones.Srv_ID_LinBottom = this.eLine6.Text;
      this.opciones.Impresora_Tck = this.cP1.Text;
      this.opciones.Barcode = this.eBar.Text;
      this.opciones.ModoTickets = this.cTicket.Checked ? 1 : 0;
      this.opciones.ModoPS2 = this.cPS2.Checked ? 1 : 0;
      this.opciones.Ticket_Cut = this.cCut.Checked ? 1 : 0;
      this.opciones.Ticket_60mm = this.c60mm.Checked ? 1 : 0;
      this.opciones.Ticket_Model = this.cPModel.Text == "ESC/POS" ? 1 : 0;
      int num1 = 1;
      try
      {
        num1 = int.Parse(this.tPLines.Text);
      }
      catch
      {
      }
      this.opciones.Ticket_N_FEED = num1;
      int num2 = 0;
      try
      {
        num2 = int.Parse(this.tPHEAD.Text);
      }
      catch
      {
      }
      this.opciones.Ticket_N_HEAD = num2;
      this.opciones.Save_Net();
      this.Close();
    }

    private void bCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void DLG_Depositaire_Load(object sender, EventArgs e)
    {
      if (this.opciones.modo_XP == 0)
      {
        MainWindow.Clean_Printer();
        PrintDocument printDocument = new PrintDocument();
        foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
        {
          this.cP1.Items.Add((object) installedPrinter);
          this.cP1.SelectedIndex = this.cP1.Items.IndexOf((object) installedPrinter);
        }
      }
      this.cPModel.Items.Clear();
      this.cPModel.Items.Add((object) "Nii");
      this.cPModel.Items.Add((object) "ESC/POS");
      if (this.opciones.Ticket_Model == 0)
        this.cPModel.SelectedIndex = this.cPModel.Items.IndexOf((object) "Nii");
      else
        this.cPModel.SelectedIndex = this.cPModel.Items.IndexOf((object) "ESC/POS");
      this.cCut.Checked = this.opciones.Ticket_Cut != 0;
      this.c60mm.Checked = this.opciones.Ticket_60mm != 0;
      this.tPLines.Text = string.Concat((object) this.opciones.Ticket_N_FEED);
      this.tPHEAD.Text = string.Concat((object) this.opciones.Ticket_N_HEAD);
      this.cP1.Text = this.opciones.Impresora_Tck;
      this.eBar.Text = this.opciones.Barcode;
    }

    private void bTick_Click(object sender, EventArgs e)
    {
      int _skeep = 1;
      try
      {
        _skeep = int.Parse(this.tPLines.Text);
      }
      catch
      {
      }
      int _preskeep = 0;
      try
      {
        _preskeep = int.Parse(this.tPHEAD.Text);
      }
      catch
      {
      }
      this.MWin.Ticket(this.cP1.Text, new Decimal(0), 0, 0, this.cPModel.Text == "ESC/POS" ? 1 : 0, this.cCut.Checked ? 1 : 0, _skeep, _preskeep, this.c60mm.Checked ? 1 : 0);
    }

    private void bBar_Click(object sender, EventArgs e)
    {
      int forceAllKey = this.opciones.ForceAllKey;
      this.opciones.ForceAllKey = 0;
      this.opciones.Last_Device = "";
      this.opciones.Test_Barcode = 1;
      this.MWin.TicketToCheck = "";
      this.MWin.TicketOK = 0;
      int num = (int) new DLG_Message(this.opciones.Localize.Text("Read Barcode"), ref this.opciones, false).ShowDialog();
      this.opciones.Test_Barcode = 0;
      if (this.MWin.TicketOK == 1)
        this.eBar.Text = this.opciones.Last_Device;
      else
        this.eBar.Text = "";
      this.opciones.ForceAllKey = forceAllKey;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      int _skeep = 1;
      try
      {
        _skeep = int.Parse(this.tPLines.Text);
      }
      catch
      {
      }
      int _preskeep = 0;
      try
      {
        _preskeep = int.Parse(this.tPHEAD.Text);
      }
      catch
      {
      }
      this.MWin.Ticket_Out(this.cP1.Text, new Decimal(0), 0, this.cPModel.Text == "ESC/POS" ? 1 : 0, this.cCut.Checked ? 1 : 0, _skeep, _preskeep, this.c60mm.Checked ? 1 : 0, DateTime.Now, "000", 0);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.eLine2 = new TextBox();
      this.eLine1 = new TextBox();
      this.lLine2 = new Label();
      this.lLine1 = new Label();
      this.eLine3 = new TextBox();
      this.lLine3 = new Label();
      this.eLine4 = new TextBox();
      this.lLine4 = new Label();
      this.eLine5 = new TextBox();
      this.lLine5 = new Label();
      this.cTicket = new CheckBox();
      this.bTick = new Button();
      this.bBar = new Button();
      this.eBar = new TextBox();
      this.lBar = new Label();
      this.lP1 = new Label();
      this.cP1 = new ComboBox();
      this.eLine6 = new TextBox();
      this.lLine6 = new Label();
      this.cPS2 = new CheckBox();
      this.cPModel = new ComboBox();
      this.lPModel = new Label();
      this.cCut = new CheckBox();
      this.lPLines = new Label();
      this.tPLines = new TextBox();
      this.button1 = new Button();
      this.bCancel = new Button();
      this.bOk = new Button();
      this.c60mm = new CheckBox();
      this.tPHEAD = new TextBox();
      this.label1 = new Label();
      this.SuspendLayout();
      this.eLine2.Location = new Point(17, 284);
      this.eLine2.Name = "eLine2";
      this.eLine2.Size = new Size(455, 20);
      this.eLine2.TabIndex = 7;
      this.eLine1.Location = new Point(17, 235);
      this.eLine1.Name = "eLine1";
      this.eLine1.Size = new Size(455, 20);
      this.eLine1.TabIndex = 6;
      this.lLine2.AutoSize = true;
      this.lLine2.Location = new Point(17, 263);
      this.lLine2.Name = "lLine2";
      this.lLine2.Size = new Size(10, 13);
      this.lLine2.TabIndex = 28;
      this.lLine2.Text = "-";
      this.lLine1.AutoSize = true;
      this.lLine1.Location = new Point(17, 214);
      this.lLine1.Name = "lLine1";
      this.lLine1.Size = new Size(10, 13);
      this.lLine1.TabIndex = 27;
      this.lLine1.Text = "-";
      this.eLine3.Location = new Point(17, 333);
      this.eLine3.Name = "eLine3";
      this.eLine3.Size = new Size(455, 20);
      this.eLine3.TabIndex = 8;
      this.lLine3.AutoSize = true;
      this.lLine3.Location = new Point(17, 312);
      this.lLine3.Name = "lLine3";
      this.lLine3.Size = new Size(10, 13);
      this.lLine3.TabIndex = 30;
      this.lLine3.Text = "-";
      this.eLine4.Location = new Point(17, 382);
      this.eLine4.Name = "eLine4";
      this.eLine4.Size = new Size(455, 20);
      this.eLine4.TabIndex = 9;
      this.lLine4.AutoSize = true;
      this.lLine4.Location = new Point(17, 361);
      this.lLine4.Name = "lLine4";
      this.lLine4.Size = new Size(10, 13);
      this.lLine4.TabIndex = 32;
      this.lLine4.Text = "-";
      this.eLine5.Location = new Point(17, 431);
      this.eLine5.Name = "eLine5";
      this.eLine5.Size = new Size(455, 20);
      this.eLine5.TabIndex = 10;
      this.lLine5.AutoSize = true;
      this.lLine5.Location = new Point(17, 410);
      this.lLine5.Name = "lLine5";
      this.lLine5.Size = new Size(10, 13);
      this.lLine5.TabIndex = 34;
      this.lLine5.Text = "-";
      this.cTicket.AutoSize = true;
      this.cTicket.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cTicket.Location = new Point(17, 12);
      this.cTicket.Name = "cTicket";
      this.cTicket.Size = new Size(201, 35);
      this.cTicket.TabIndex = 0;
      this.cTicket.Text = "Ticket system";
      this.cTicket.UseVisualStyleBackColor = true;
      this.bTick.Location = new Point(408, 56);
      this.bTick.Name = "bTick";
      this.bTick.Size = new Size(64, 48);
      this.bTick.TabIndex = 2;
      this.bTick.Text = "Test";
      this.bTick.UseVisualStyleBackColor = true;
      this.bTick.Click += new EventHandler(this.bTick_Click);
      this.bBar.Location = new Point(408, 144);
      this.bBar.Name = "bBar";
      this.bBar.Size = new Size(64, 48);
      this.bBar.TabIndex = 4;
      this.bBar.Text = "Detect";
      this.bBar.UseVisualStyleBackColor = true;
      this.bBar.Click += new EventHandler(this.bBar_Click);
      this.eBar.Location = new Point(17, 157);
      this.eBar.Name = "eBar";
      this.eBar.Size = new Size(385, 20);
      this.eBar.TabIndex = 3;
      this.lBar.AutoSize = true;
      this.lBar.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lBar.Location = new Point(14, 141);
      this.lBar.Name = "lBar";
      this.lBar.Size = new Size(80, 13);
      this.lBar.TabIndex = 40;
      this.lBar.Text = "Barcode reader";
      this.lP1.AutoSize = true;
      this.lP1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lP1.Location = new Point(14, 55);
      this.lP1.Name = "lP1";
      this.lP1.Size = new Size(37, 13);
      this.lP1.TabIndex = 37;
      this.lP1.Text = "Printer";
      this.cP1.FormattingEnabled = true;
      this.cP1.Location = new Point(17, 71);
      this.cP1.Name = "cP1";
      this.cP1.Size = new Size(385, 21);
      this.cP1.TabIndex = 1;
      this.eLine6.Location = new Point(17, 480);
      this.eLine6.Name = "eLine6";
      this.eLine6.Size = new Size(455, 20);
      this.eLine6.TabIndex = 11;
      this.lLine6.AutoSize = true;
      this.lLine6.Location = new Point(17, 459);
      this.lLine6.Name = "lLine6";
      this.lLine6.Size = new Size(10, 13);
      this.lLine6.TabIndex = 42;
      this.lLine6.Text = "-";
      this.cPS2.AutoSize = true;
      this.cPS2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cPS2.Location = new Point(17, 187);
      this.cPS2.Name = "cPS2";
      this.cPS2.Size = new Size(179, 17);
      this.cPS2.TabIndex = 5;
      this.cPS2.Text = "PS/2 Barcode + PS/2 Keyboard";
      this.cPS2.UseVisualStyleBackColor = true;
      this.cPModel.FormattingEnabled = true;
      this.cPModel.Location = new Point(17, 114);
      this.cPModel.Name = "cPModel";
      this.cPModel.Size = new Size(88, 21);
      this.cPModel.TabIndex = 43;
      this.lPModel.AutoSize = true;
      this.lPModel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lPModel.Location = new Point(17, 98);
      this.lPModel.Name = "lPModel";
      this.lPModel.Size = new Size(60, 13);
      this.lPModel.TabIndex = 44;
      this.lPModel.Text = "Printer type";
      this.cCut.AutoSize = true;
      this.cCut.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cCut.Location = new Point(300, 116);
      this.cCut.Name = "cCut";
      this.cCut.Size = new Size(67, 17);
      this.cCut.TabIndex = 45;
      this.cCut.Text = "Auto Cut";
      this.cCut.UseVisualStyleBackColor = true;
      this.lPLines.AutoSize = true;
      this.lPLines.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lPLines.Location = new Point(198, 97);
      this.lPLines.Name = "lPLines";
      this.lPLines.Size = new Size(113, 13);
      this.lPLines.TabIndex = 46;
      this.lPLines.Text = "Skeep lines before cut";
      this.tPLines.Location = new Point(201, 116);
      this.tPLines.Name = "tPLines";
      this.tPLines.Size = new Size(71, 20);
      this.tPLines.TabIndex = 47;
      this.button1.Location = new Point(408, 2);
      this.button1.Name = "button1";
      this.button1.Size = new Size(64, 48);
      this.button1.TabIndex = 48;
      this.button1.Text = "Test Out";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Visible = false;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.bCancel.BackgroundImage = (Image) Resources.ico_del;
      this.bCancel.BackgroundImageLayout = ImageLayout.Center;
      this.bCancel.Location = new Point(338, 506);
      this.bCancel.Name = "bCancel";
      this.bCancel.Size = new Size(64, 48);
      this.bCancel.TabIndex = 13;
      this.bCancel.UseVisualStyleBackColor = true;
      this.bCancel.Click += new EventHandler(this.bCancel_Click);
      this.bOk.BackgroundImage = (Image) Resources.ico_ok;
      this.bOk.BackgroundImageLayout = ImageLayout.Center;
      this.bOk.Location = new Point(408, 506);
      this.bOk.Name = "bOk";
      this.bOk.Size = new Size(64, 48);
      this.bOk.TabIndex = 12;
      this.bOk.UseVisualStyleBackColor = true;
      this.bOk.Click += new EventHandler(this.bOk_Click);
      this.c60mm.AutoSize = true;
      this.c60mm.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.c60mm.Location = new Point(387, 115);
      this.c60mm.Name = "c60mm";
      this.c60mm.Size = new Size(85, 17);
      this.c60mm.TabIndex = 49;
      this.c60mm.Text = "60mm Paper";
      this.c60mm.UseVisualStyleBackColor = true;
      this.tPHEAD.Location = new Point(117, 116);
      this.tPHEAD.Name = "tPHEAD";
      this.tPHEAD.Size = new Size(71, 20);
      this.tPHEAD.TabIndex = 50;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(113, 96);
      this.label1.Name = "label1";
      this.label1.Size = new Size(65, 13);
      this.label1.TabIndex = 51;
      this.label1.Text = "Skeep head";
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(484, 561);
      this.ControlBox = false;
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.tPHEAD);
      this.Controls.Add((Control) this.c60mm);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.tPLines);
      this.Controls.Add((Control) this.lPLines);
      this.Controls.Add((Control) this.cCut);
      this.Controls.Add((Control) this.lPModel);
      this.Controls.Add((Control) this.cPModel);
      this.Controls.Add((Control) this.cPS2);
      this.Controls.Add((Control) this.eLine6);
      this.Controls.Add((Control) this.lLine6);
      this.Controls.Add((Control) this.bTick);
      this.Controls.Add((Control) this.bBar);
      this.Controls.Add((Control) this.eBar);
      this.Controls.Add((Control) this.lBar);
      this.Controls.Add((Control) this.lP1);
      this.Controls.Add((Control) this.cP1);
      this.Controls.Add((Control) this.cTicket);
      this.Controls.Add((Control) this.eLine5);
      this.Controls.Add((Control) this.lLine5);
      this.Controls.Add((Control) this.eLine4);
      this.Controls.Add((Control) this.lLine4);
      this.Controls.Add((Control) this.eLine3);
      this.Controls.Add((Control) this.lLine3);
      this.Controls.Add((Control) this.eLine2);
      this.Controls.Add((Control) this.eLine1);
      this.Controls.Add((Control) this.lLine2);
      this.Controls.Add((Control) this.lLine1);
      this.Controls.Add((Control) this.bCancel);
      this.Controls.Add((Control) this.bOk);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (DLG_Depositaire);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Ticket";
      this.Load += new EventHandler(this.DLG_Depositaire_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
