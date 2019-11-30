// Decompiled with JetBrains decompiler
// Type: Kiosk.DLG_Preferencias
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
  public class DLG_Preferencias : Form
  {
    private IContainer components = (IContainer) null;
    public bool OK;
    public Configuracion opciones;
    public MainWindow MWin;
    private Button bCancel;
    private Button bOK;
    private CheckBox cCredits;
    private ComboBox cLanguage;
    private Label lLanguage;
    private Label lCred;
    private TextBox eCred;
    private TextBox eRCred;
    private Label lRCred;
    private CheckBox cFree;
    private CheckBox cDemo;
    private CheckBox cMOUSE;
    private RadioButton rREAIS;
    private RadioButton rEUROS;
    private CheckBox cMOUSE_EMU;
    private CheckBox cBAR;
    private CheckBox cATICKET;
    private CheckBox cHide;
    private CheckBox cBill;
    private CheckBox cCancelOn;
    private CheckBox cJoinT;
    private RadioButton rDOM;
    private CheckBox cGAS;

    public DLG_Preferencias(ref Configuracion _opc, ref MainWindow _base)
    {
      this.OK = false;
      this.opciones = _opc;
      this.MWin = _base;
      this.InitializeComponent();
      if (this.opciones.NoCreditsInGame == 1)
        this.cCredits.Checked = true;
      if (this.opciones.FreeGames == 1)
        this.cFree.Checked = true;
      if (this.opciones.ModoKiosk == 0)
        this.cDemo.Checked = true;
      if (this.opciones.CursorOn == 1)
        this.cMOUSE.Checked = true;
      if (this.opciones.Emu_Mouse)
        this.cMOUSE_EMU.Checked = true;
      if (this.opciones.TicketHidePay == 1)
        this.cHide.Checked = true;
      if (this.opciones.Ticket_Carburante == 1)
        this.cGAS.Checked = true;
      if (this.opciones.Dev_Bank == 0)
      {
        this.rEUROS.Checked = true;
        this.rREAIS.Checked = false;
        this.rDOM.Checked = false;
      }
      if (this.opciones.Dev_Bank == 1)
      {
        this.rEUROS.Checked = false;
        this.rREAIS.Checked = true;
        this.rDOM.Checked = false;
      }
      if (this.opciones.Dev_Bank == 2)
      {
        this.rEUROS.Checked = false;
        this.rREAIS.Checked = false;
        this.rDOM.Checked = true;
      }
      if (this.opciones.AutoTicketTime >= 1)
        this.cATICKET.Checked = false;
      if (this.opciones.ModoPlayCreditsGratuits == 1)
        this.cBill.Checked = true;
      if (this.opciones.CancelTempsOn == 1)
        this.cCancelOn.Checked = true;
      if (this.opciones.JoinTicket >= 1)
        this.cJoinT.Checked = true;
      this.cBAR.Checked = this.opciones.BrowserBarOn == 1;
      this.eCred.Text = this.opciones.ValorTemps.ToString();
      this.eRCred.Text = this.opciones.ResetTemps.ToString();
      this.cLanguage.Items.Clear();
      this.cLanguage.Items.Add((object) this.opciones.Localize.Translation.BaseLoc);
      this.cLanguage.SelectedIndex = 0;
      for (int index = 0; index < this.opciones.Localize.Translation.Locs.Length; ++index)
      {
        this.cLanguage.Items.Add((object) this.opciones.Localize.Translation.Locs[index]);
        if (this.opciones.loc == this.opciones.Localize.Translation.Locs[index])
          this.cLanguage.SelectedIndex = index + 1;
      }
      bool flag = true;
      this.rREAIS.Visible = false;
      flag = true;
      flag = true;
      this.Localize();
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.Text = this.opciones.Localize.Text("Options");
      this.cCredits.Text = this.opciones.Localize.Text("Lock credits");
      this.cFree.Text = this.opciones.Localize.Text("Free games");
      this.lLanguage.Text = this.opciones.Localize.Text("Language");
      this.lCred.Text = this.opciones.Localize.Text("Credits / Minute");
      this.lRCred.Text = this.opciones.Localize.Text("Reset time");
      this.cBAR.Text = this.opciones.Localize.Text("Browser bar");
      this.cATICKET.Text = this.opciones.Localize.Text("Auto Ticket Time");
      this.cHide.Text = this.opciones.Localize.Text("Hide Ticket points");
      this.cBill.Text = this.opciones.Localize.Text("Force play all credits");
      this.cCancelOn.Text = this.opciones.Localize.Text("Time use credits");
      this.cJoinT.Text = this.opciones.Localize.Text("Only one ticket");
      this.ResumeLayout();
    }

    private void bOK_Click(object sender, EventArgs e)
    {
      int num1 = 25;
      try
      {
        num1 = Convert.ToInt32(this.eCred.Text);
      }
      catch
      {
      }
      this.opciones.ValorTemps = num1;
      int num2 = 300;
      try
      {
        num2 = Convert.ToInt32(this.eRCred.Text);
      }
      catch
      {
      }
      this.opciones.ResetTemps = num2;
      if (this.opciones.loc != (string) this.cLanguage.Items[this.cLanguage.SelectedIndex])
      {
        this.opciones.loc = (string) this.cLanguage.Items[this.cLanguage.SelectedIndex];
        this.opciones.Localize.Localize = (string) this.cLanguage.Items[this.cLanguage.SelectedIndex];
        this.opciones.Reload_Localizacion();
      }
      this.opciones.NoCreditsInGame = this.cCredits.Checked ? 1 : 0;
      this.opciones.FreeGames = this.cFree.Checked ? 1 : 0;
      this.opciones.ModoKiosk = this.cDemo.Checked ? 0 : 1;
      this.opciones.CursorOn = this.cMOUSE.Checked ? 1 : 0;
      this.opciones.Emu_Mouse = this.cMOUSE_EMU.Checked;
      if (this.rEUROS.Checked)
        this.opciones.Dev_Bank = 0;
      if (this.rREAIS.Checked)
        this.opciones.Dev_Bank = 1;
      if (this.rDOM.Checked)
        this.opciones.Dev_Bank = 2;
      this.opciones.JoinTicket = this.cJoinT.Checked ? 1 : 0;
      this.opciones.BrowserBarOn = this.cBAR.Checked ? 1 : 0;
      this.opciones.AutoTicketTime = this.cATICKET.Checked ? 2 : 0;
      this.opciones.TicketHidePay = this.cHide.Checked ? 1 : 0;
      this.opciones.Ticket_Carburante = this.cGAS.Checked ? 1 : 0;
      this.opciones.ModoPlayCreditsGratuits = this.cBill.Checked ? 1 : 0;
      this.opciones.CancelTempsOn = this.cCancelOn.Checked ? 1 : 0;
      this.opciones.Save_Net();
      this.OK = true;
      this.Close();
    }

    private void bCancel_Click(object sender, EventArgs e)
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
      this.bCancel = new Button();
      this.bOK = new Button();
      this.cCredits = new CheckBox();
      this.cLanguage = new ComboBox();
      this.lLanguage = new Label();
      this.lCred = new Label();
      this.eCred = new TextBox();
      this.eRCred = new TextBox();
      this.lRCred = new Label();
      this.cFree = new CheckBox();
      this.cDemo = new CheckBox();
      this.cMOUSE = new CheckBox();
      this.rREAIS = new RadioButton();
      this.rEUROS = new RadioButton();
      this.cMOUSE_EMU = new CheckBox();
      this.cBAR = new CheckBox();
      this.cATICKET = new CheckBox();
      this.cHide = new CheckBox();
      this.cBill = new CheckBox();
      this.cCancelOn = new CheckBox();
      this.cJoinT = new CheckBox();
      this.rDOM = new RadioButton();
      this.cGAS = new CheckBox();
      this.SuspendLayout();
      this.bCancel.Image = (Image) Resources.ico_del;
      this.bCancel.Location = new Point(373, 559);
      this.bCancel.Name = "bCancel";
      this.bCancel.Size = new Size(48, 48);
      this.bCancel.TabIndex = 11;
      this.bCancel.UseVisualStyleBackColor = true;
      this.bCancel.Click += new EventHandler(this.bCancel_Click);
      this.bOK.Image = (Image) Resources.ico_ok;
      this.bOK.Location = new Point(427, 559);
      this.bOK.Name = "bOK";
      this.bOK.Size = new Size(48, 48);
      this.bOK.TabIndex = 12;
      this.bOK.UseVisualStyleBackColor = true;
      this.bOK.Click += new EventHandler(this.bOK_Click);
      this.cCredits.AutoSize = true;
      this.cCredits.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cCredits.ForeColor = Color.OrangeRed;
      this.cCredits.Location = new Point(12, 12);
      this.cCredits.Name = "cCredits";
      this.cCredits.Size = new Size(179, 35);
      this.cCredits.TabIndex = 1;
      this.cCredits.Text = "Lock credits";
      this.cCredits.UseVisualStyleBackColor = true;
      this.cLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cLanguage.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cLanguage.FormattingEnabled = true;
      this.cLanguage.Items.AddRange(new object[2]
      {
        (object) "Castellano",
        (object) "Ingles"
      });
      this.cLanguage.Location = new Point(153, 472);
      this.cLanguage.Name = "cLanguage";
      this.cLanguage.Size = new Size(177, 39);
      this.cLanguage.TabIndex = 8;
      this.lLanguage.AutoSize = true;
      this.lLanguage.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lLanguage.Location = new Point(13, 472);
      this.lLanguage.Name = "lLanguage";
      this.lLanguage.Size = new Size(134, 31);
      this.lLanguage.TabIndex = 20;
      this.lLanguage.Text = "Language";
      this.lCred.AutoSize = true;
      this.lCred.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lCred.Location = new Point(7, 533);
      this.lCred.Name = "lCred";
      this.lCred.Size = new Size(204, 31);
      this.lCred.TabIndex = 21;
      this.lCred.Text = "Credits / minute";
      this.lCred.Visible = false;
      this.eCred.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.eCred.Location = new Point(217, 526);
      this.eCred.Name = "eCred";
      this.eCred.Size = new Size(113, 38);
      this.eCred.TabIndex = 9;
      this.eCred.Visible = false;
      this.eRCred.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.eRCred.Location = new Point(217, 569);
      this.eRCred.Name = "eRCred";
      this.eRCred.Size = new Size(113, 38);
      this.eRCred.TabIndex = 10;
      this.eRCred.Visible = false;
      this.lRCred.AutoSize = true;
      this.lRCred.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lRCred.Location = new Point(7, 572);
      this.lRCred.Name = "lRCred";
      this.lRCred.Size = new Size(144, 31);
      this.lRCred.TabIndex = 23;
      this.lRCred.Text = "Reset time";
      this.lRCred.Visible = false;
      this.cFree.AutoSize = true;
      this.cFree.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cFree.Location = new Point(12, 345);
      this.cFree.Name = "cFree";
      this.cFree.Size = new Size(177, 35);
      this.cFree.TabIndex = 2;
      this.cFree.Text = "Free games";
      this.cFree.UseVisualStyleBackColor = true;
      this.cFree.Visible = false;
      this.cDemo.AutoSize = true;
      this.cDemo.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cDemo.Location = new Point(12, 382);
      this.cDemo.Name = "cDemo";
      this.cDemo.Size = new Size(179, 35);
      this.cDemo.TabIndex = 3;
      this.cDemo.Text = "Demo mode";
      this.cDemo.UseVisualStyleBackColor = true;
      this.cDemo.Visible = false;
      this.cMOUSE.AutoSize = true;
      this.cMOUSE.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cMOUSE.Location = new Point(12, 234);
      this.cMOUSE.Name = "cMOUSE";
      this.cMOUSE.Size = new Size(280, 35);
      this.cMOUSE.TabIndex = 4;
      this.cMOUSE.Text = "Mouse cursor visible";
      this.cMOUSE.UseVisualStyleBackColor = true;
      this.rREAIS.AutoSize = true;
      this.rREAIS.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rREAIS.Location = new Point(147, 434);
      this.rREAIS.Name = "rREAIS";
      this.rREAIS.Size = new Size(114, 35);
      this.rREAIS.TabIndex = 7;
      this.rREAIS.Text = "REAIS";
      this.rREAIS.UseVisualStyleBackColor = true;
      this.rEUROS.AutoSize = true;
      this.rEUROS.Checked = true;
      this.rEUROS.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rEUROS.Location = new Point(12, 434);
      this.rEUROS.Name = "rEUROS";
      this.rEUROS.Size = new Size(129, 35);
      this.rEUROS.TabIndex = 6;
      this.rEUROS.TabStop = true;
      this.rEUROS.Text = "EUROS";
      this.rEUROS.UseVisualStyleBackColor = true;
      this.cMOUSE_EMU.AutoSize = true;
      this.cMOUSE_EMU.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cMOUSE_EMU.Location = new Point(12, 271);
      this.cMOUSE_EMU.Name = "cMOUSE_EMU";
      this.cMOUSE_EMU.Size = new Size(388, 35);
      this.cMOUSE_EMU.TabIndex = 5;
      this.cMOUSE_EMU.Text = "Mouse Emulator by Keyboard";
      this.cMOUSE_EMU.UseVisualStyleBackColor = true;
      this.cBAR.AutoSize = true;
      this.cBAR.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cBAR.Location = new Point(12, 308);
      this.cBAR.Name = "cBAR";
      this.cBAR.Size = new Size(179, 35);
      this.cBAR.TabIndex = 24;
      this.cBAR.Text = "Browser bar";
      this.cBAR.UseVisualStyleBackColor = true;
      this.cATICKET.AutoSize = true;
      this.cATICKET.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cATICKET.ForeColor = Color.OrangeRed;
      this.cATICKET.Location = new Point(12, 123);
      this.cATICKET.Name = "cATICKET";
      this.cATICKET.Size = new Size(237, 35);
      this.cATICKET.TabIndex = 25;
      this.cATICKET.Text = "Auto Ticket Time";
      this.cATICKET.UseVisualStyleBackColor = true;
      this.cHide.AutoSize = true;
      this.cHide.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cHide.Location = new Point(12, 160);
      this.cHide.Name = "cHide";
      this.cHide.Size = new Size(241, 35);
      this.cHide.TabIndex = 26;
      this.cHide.Text = "Hide ticket points";
      this.cHide.UseVisualStyleBackColor = true;
      this.cBill.AutoSize = true;
      this.cBill.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cBill.ForeColor = Color.OrangeRed;
      this.cBill.Location = new Point(12, 49);
      this.cBill.Name = "cBill";
      this.cBill.Size = new Size(282, 35);
      this.cBill.TabIndex = 27;
      this.cBill.Text = "Force play all credits";
      this.cBill.UseVisualStyleBackColor = true;
      this.cCancelOn.AutoSize = true;
      this.cCancelOn.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cCancelOn.ForeColor = Color.OrangeRed;
      this.cCancelOn.Location = new Point(12, 86);
      this.cCancelOn.Name = "cCancelOn";
      this.cCancelOn.Size = new Size(232, 35);
      this.cCancelOn.TabIndex = 28;
      this.cCancelOn.Text = "Time use credits";
      this.cCancelOn.UseVisualStyleBackColor = true;
      this.cJoinT.AutoSize = true;
      this.cJoinT.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cJoinT.ForeColor = Color.OrangeRed;
      this.cJoinT.Location = new Point(276, 123);
      this.cJoinT.Name = "cJoinT";
      this.cJoinT.Size = new Size(220, 35);
      this.cJoinT.TabIndex = 29;
      this.cJoinT.Text = "Only one ticket ";
      this.cJoinT.UseVisualStyleBackColor = true;
      this.rDOM.AutoSize = true;
      this.rDOM.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rDOM.Location = new Point(267, 434);
      this.rDOM.Name = "rDOM";
      this.rDOM.Size = new Size(159, 35);
      this.rDOM.TabIndex = 30;
      this.rDOM.Text = "REP.DOM";
      this.rDOM.UseVisualStyleBackColor = true;
      this.cGAS.AutoSize = true;
      this.cGAS.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cGAS.Location = new Point(12, 197);
      this.cGAS.Name = "cGAS";
      this.cGAS.Size = new Size(171, 35);
      this.cGAS.TabIndex = 31;
      this.cGAS.Text = "Ticket GAS";
      this.cGAS.UseVisualStyleBackColor = true;
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(508, 611);
      this.ControlBox = false;
      this.Controls.Add((Control) this.cGAS);
      this.Controls.Add((Control) this.rDOM);
      this.Controls.Add((Control) this.cJoinT);
      this.Controls.Add((Control) this.cCancelOn);
      this.Controls.Add((Control) this.cBill);
      this.Controls.Add((Control) this.cHide);
      this.Controls.Add((Control) this.cATICKET);
      this.Controls.Add((Control) this.cBAR);
      this.Controls.Add((Control) this.cMOUSE_EMU);
      this.Controls.Add((Control) this.rREAIS);
      this.Controls.Add((Control) this.rEUROS);
      this.Controls.Add((Control) this.cMOUSE);
      this.Controls.Add((Control) this.cDemo);
      this.Controls.Add((Control) this.cFree);
      this.Controls.Add((Control) this.eRCred);
      this.Controls.Add((Control) this.lRCred);
      this.Controls.Add((Control) this.eCred);
      this.Controls.Add((Control) this.lCred);
      this.Controls.Add((Control) this.cLanguage);
      this.Controls.Add((Control) this.lLanguage);
      this.Controls.Add((Control) this.cCredits);
      this.Controls.Add((Control) this.bCancel);
      this.Controls.Add((Control) this.bOK);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DLG_Preferencias);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Preferences";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
