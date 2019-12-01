using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
	public class DLG_Preferencias : Form
	{
		public bool OK;

		public Configuracion opciones;

		public MainWindow MWin;

		private IContainer components = null;

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
			OK = false;
			opciones = _opc;
			MWin = _base;
			InitializeComponent();
			if (opciones.NoCreditsInGame == 1)
			{
				cCredits.Checked = true;
			}
			if (opciones.FreeGames == 1)
			{
				cFree.Checked = true;
			}
			if (opciones.ModoKiosk == 0)
			{
				cDemo.Checked = true;
			}
			if (opciones.CursorOn == 1)
			{
				cMOUSE.Checked = true;
			}
			if (opciones.Emu_Mouse)
			{
				cMOUSE_EMU.Checked = true;
			}
			if (opciones.TicketHidePay == 1)
			{
				cHide.Checked = true;
			}
			if (opciones.Ticket_Carburante == 1)
			{
				cGAS.Checked = true;
			}
			if (opciones.Dev_Bank == 0)
			{
				rEUROS.Checked = true;
				rREAIS.Checked = false;
				rDOM.Checked = false;
			}
			if (opciones.Dev_Bank == 1)
			{
				rEUROS.Checked = false;
				rREAIS.Checked = true;
				rDOM.Checked = false;
			}
			if (opciones.Dev_Bank == 2)
			{
				rEUROS.Checked = false;
				rREAIS.Checked = false;
				rDOM.Checked = true;
			}
			if (opciones.AutoTicketTime >= 1)
			{
				cATICKET.Checked = false;
			}
			if (opciones.ModoPlayCreditsGratuits == 1)
			{
				cBill.Checked = true;
			}
			if (opciones.CancelTempsOn == 1)
			{
				cCancelOn.Checked = true;
			}
			if (opciones.JoinTicket >= 1)
			{
				cJoinT.Checked = true;
			}
			cBAR.Checked = ((opciones.BrowserBarOn == 1) ? true : false);
			eCred.Text = opciones.ValorTemps.ToString();
			eRCred.Text = opciones.ResetTemps.ToString();
			cLanguage.Items.Clear();
			cLanguage.Items.Add(opciones.Localize.Translation.BaseLoc);
			cLanguage.SelectedIndex = 0;
			for (int i = 0; i < opciones.Localize.Translation.Locs.Length; i++)
			{
				cLanguage.Items.Add(opciones.Localize.Translation.Locs[i]);
				if (opciones.loc == opciones.Localize.Translation.Locs[i])
				{
					cLanguage.SelectedIndex = i + 1;
				}
			}
			bool flag = true;
			rREAIS.Visible = false;
			flag = true;
			flag = true;
			Localize();
		}

		private void Localize()
		{
			SuspendLayout();
			Text = opciones.Localize.Text("Options");
			cCredits.Text = opciones.Localize.Text("Lock credits");
			cFree.Text = opciones.Localize.Text("Free games");
			lLanguage.Text = opciones.Localize.Text("Language");
			lCred.Text = opciones.Localize.Text("Credits / Minute");
			lRCred.Text = opciones.Localize.Text("Reset time");
			cBAR.Text = opciones.Localize.Text("Browser bar");
			cATICKET.Text = opciones.Localize.Text("Auto Ticket Time");
			cHide.Text = opciones.Localize.Text("Hide Ticket points");
			cBill.Text = opciones.Localize.Text("Force play all credits");
			cCancelOn.Text = opciones.Localize.Text("Time use credits");
			cJoinT.Text = opciones.Localize.Text("Only one ticket");
			ResumeLayout();
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			int valorTemps = 25;
			try
			{
				valorTemps = Convert.ToInt32(eCred.Text);
			}
			catch
			{
			}
			opciones.ValorTemps = valorTemps;
			valorTemps = 300;
			try
			{
				valorTemps = Convert.ToInt32(eRCred.Text);
			}
			catch
			{
			}
			opciones.ResetTemps = valorTemps;
			if (opciones.loc != (string)cLanguage.Items[cLanguage.SelectedIndex])
			{
				opciones.loc = (string)cLanguage.Items[cLanguage.SelectedIndex];
				opciones.Localize.Localize = (string)cLanguage.Items[cLanguage.SelectedIndex];
				opciones.Reload_Localizacion();
			}
			opciones.NoCreditsInGame = (cCredits.Checked ? 1 : 0);
			opciones.FreeGames = (cFree.Checked ? 1 : 0);
			opciones.ModoKiosk = ((!cDemo.Checked) ? 1 : 0);
			opciones.CursorOn = (cMOUSE.Checked ? 1 : 0);
			opciones.Emu_Mouse = (cMOUSE_EMU.Checked ? true : false);
			if (rEUROS.Checked)
			{
				opciones.Dev_Bank = 0;
			}
			if (rREAIS.Checked)
			{
				opciones.Dev_Bank = 1;
			}
			if (rDOM.Checked)
			{
				opciones.Dev_Bank = 2;
			}
			opciones.JoinTicket = (cJoinT.Checked ? 1 : 0);
			opciones.BrowserBarOn = (cBAR.Checked ? 1 : 0);
			opciones.AutoTicketTime = (cATICKET.Checked ? 2 : 0);
			opciones.TicketHidePay = (cHide.Checked ? 1 : 0);
			opciones.Ticket_Carburante = (cGAS.Checked ? 1 : 0);
			opciones.ModoPlayCreditsGratuits = (cBill.Checked ? 1 : 0);
			opciones.CancelTempsOn = (cCancelOn.Checked ? 1 : 0);
			opciones.Save_Net();
			OK = true;
			Close();
		}

		private void bCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			bCancel = new System.Windows.Forms.Button();
			bOK = new System.Windows.Forms.Button();
			cCredits = new System.Windows.Forms.CheckBox();
			cLanguage = new System.Windows.Forms.ComboBox();
			lLanguage = new System.Windows.Forms.Label();
			lCred = new System.Windows.Forms.Label();
			eCred = new System.Windows.Forms.TextBox();
			eRCred = new System.Windows.Forms.TextBox();
			lRCred = new System.Windows.Forms.Label();
			cFree = new System.Windows.Forms.CheckBox();
			cDemo = new System.Windows.Forms.CheckBox();
			cMOUSE = new System.Windows.Forms.CheckBox();
			rREAIS = new System.Windows.Forms.RadioButton();
			rEUROS = new System.Windows.Forms.RadioButton();
			cMOUSE_EMU = new System.Windows.Forms.CheckBox();
			cBAR = new System.Windows.Forms.CheckBox();
			cATICKET = new System.Windows.Forms.CheckBox();
			cHide = new System.Windows.Forms.CheckBox();
			cBill = new System.Windows.Forms.CheckBox();
			cCancelOn = new System.Windows.Forms.CheckBox();
			cJoinT = new System.Windows.Forms.CheckBox();
			rDOM = new System.Windows.Forms.RadioButton();
			cGAS = new System.Windows.Forms.CheckBox();
			SuspendLayout();
			bCancel.Image = Kiosk.Properties.Resources.ico_del;
			bCancel.Location = new System.Drawing.Point(373, 559);
			bCancel.Name = "bCancel";
			bCancel.Size = new System.Drawing.Size(48, 48);
			bCancel.TabIndex = 11;
			bCancel.UseVisualStyleBackColor = true;
			bCancel.Click += new System.EventHandler(bCancel_Click);
			bOK.Image = Kiosk.Properties.Resources.ico_ok;
			bOK.Location = new System.Drawing.Point(427, 559);
			bOK.Name = "bOK";
			bOK.Size = new System.Drawing.Size(48, 48);
			bOK.TabIndex = 12;
			bOK.UseVisualStyleBackColor = true;
			bOK.Click += new System.EventHandler(bOK_Click);
			cCredits.AutoSize = true;
			cCredits.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cCredits.ForeColor = System.Drawing.Color.OrangeRed;
			cCredits.Location = new System.Drawing.Point(12, 12);
			cCredits.Name = "cCredits";
			cCredits.Size = new System.Drawing.Size(179, 35);
			cCredits.TabIndex = 1;
			cCredits.Text = "Lock credits";
			cCredits.UseVisualStyleBackColor = true;
			cLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cLanguage.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cLanguage.FormattingEnabled = true;
			cLanguage.Items.AddRange(new object[2]
			{
				"Castellano",
				"Ingles"
			});
			cLanguage.Location = new System.Drawing.Point(153, 472);
			cLanguage.Name = "cLanguage";
			cLanguage.Size = new System.Drawing.Size(177, 39);
			cLanguage.TabIndex = 8;
			lLanguage.AutoSize = true;
			lLanguage.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lLanguage.Location = new System.Drawing.Point(13, 472);
			lLanguage.Name = "lLanguage";
			lLanguage.Size = new System.Drawing.Size(134, 31);
			lLanguage.TabIndex = 20;
			lLanguage.Text = "Language";
			lCred.AutoSize = true;
			lCred.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lCred.Location = new System.Drawing.Point(7, 533);
			lCred.Name = "lCred";
			lCred.Size = new System.Drawing.Size(204, 31);
			lCred.TabIndex = 21;
			lCred.Text = "Credits / minute";
			lCred.Visible = false;
			eCred.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			eCred.Location = new System.Drawing.Point(217, 526);
			eCred.Name = "eCred";
			eCred.Size = new System.Drawing.Size(113, 38);
			eCred.TabIndex = 9;
			eCred.Visible = false;
			eRCred.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			eRCred.Location = new System.Drawing.Point(217, 569);
			eRCred.Name = "eRCred";
			eRCred.Size = new System.Drawing.Size(113, 38);
			eRCred.TabIndex = 10;
			eRCred.Visible = false;
			lRCred.AutoSize = true;
			lRCred.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lRCred.Location = new System.Drawing.Point(7, 572);
			lRCred.Name = "lRCred";
			lRCred.Size = new System.Drawing.Size(144, 31);
			lRCred.TabIndex = 23;
			lRCred.Text = "Reset time";
			lRCred.Visible = false;
			cFree.AutoSize = true;
			cFree.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cFree.Location = new System.Drawing.Point(12, 345);
			cFree.Name = "cFree";
			cFree.Size = new System.Drawing.Size(177, 35);
			cFree.TabIndex = 2;
			cFree.Text = "Free games";
			cFree.UseVisualStyleBackColor = true;
			cFree.Visible = false;
			cDemo.AutoSize = true;
			cDemo.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cDemo.Location = new System.Drawing.Point(12, 382);
			cDemo.Name = "cDemo";
			cDemo.Size = new System.Drawing.Size(179, 35);
			cDemo.TabIndex = 3;
			cDemo.Text = "Demo mode";
			cDemo.UseVisualStyleBackColor = true;
			cDemo.Visible = false;
			cMOUSE.AutoSize = true;
			cMOUSE.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cMOUSE.Location = new System.Drawing.Point(12, 234);
			cMOUSE.Name = "cMOUSE";
			cMOUSE.Size = new System.Drawing.Size(280, 35);
			cMOUSE.TabIndex = 4;
			cMOUSE.Text = "Mouse cursor visible";
			cMOUSE.UseVisualStyleBackColor = true;
			rREAIS.AutoSize = true;
			rREAIS.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			rREAIS.Location = new System.Drawing.Point(147, 434);
			rREAIS.Name = "rREAIS";
			rREAIS.Size = new System.Drawing.Size(114, 35);
			rREAIS.TabIndex = 7;
			rREAIS.Text = "REAIS";
			rREAIS.UseVisualStyleBackColor = true;
			rEUROS.AutoSize = true;
			rEUROS.Checked = true;
			rEUROS.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			rEUROS.Location = new System.Drawing.Point(12, 434);
			rEUROS.Name = "rEUROS";
			rEUROS.Size = new System.Drawing.Size(129, 35);
			rEUROS.TabIndex = 6;
			rEUROS.TabStop = true;
			rEUROS.Text = "EUROS";
			rEUROS.UseVisualStyleBackColor = true;
			cMOUSE_EMU.AutoSize = true;
			cMOUSE_EMU.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cMOUSE_EMU.Location = new System.Drawing.Point(12, 271);
			cMOUSE_EMU.Name = "cMOUSE_EMU";
			cMOUSE_EMU.Size = new System.Drawing.Size(388, 35);
			cMOUSE_EMU.TabIndex = 5;
			cMOUSE_EMU.Text = "Mouse Emulator by Keyboard";
			cMOUSE_EMU.UseVisualStyleBackColor = true;
			cBAR.AutoSize = true;
			cBAR.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cBAR.Location = new System.Drawing.Point(12, 308);
			cBAR.Name = "cBAR";
			cBAR.Size = new System.Drawing.Size(179, 35);
			cBAR.TabIndex = 24;
			cBAR.Text = "Browser bar";
			cBAR.UseVisualStyleBackColor = true;
			cATICKET.AutoSize = true;
			cATICKET.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cATICKET.ForeColor = System.Drawing.Color.OrangeRed;
			cATICKET.Location = new System.Drawing.Point(12, 123);
			cATICKET.Name = "cATICKET";
			cATICKET.Size = new System.Drawing.Size(237, 35);
			cATICKET.TabIndex = 25;
			cATICKET.Text = "Auto Ticket Time";
			cATICKET.UseVisualStyleBackColor = true;
			cHide.AutoSize = true;
			cHide.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cHide.Location = new System.Drawing.Point(12, 160);
			cHide.Name = "cHide";
			cHide.Size = new System.Drawing.Size(241, 35);
			cHide.TabIndex = 26;
			cHide.Text = "Hide ticket points";
			cHide.UseVisualStyleBackColor = true;
			cBill.AutoSize = true;
			cBill.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cBill.ForeColor = System.Drawing.Color.OrangeRed;
			cBill.Location = new System.Drawing.Point(12, 49);
			cBill.Name = "cBill";
			cBill.Size = new System.Drawing.Size(282, 35);
			cBill.TabIndex = 27;
			cBill.Text = "Force play all credits";
			cBill.UseVisualStyleBackColor = true;
			cCancelOn.AutoSize = true;
			cCancelOn.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cCancelOn.ForeColor = System.Drawing.Color.OrangeRed;
			cCancelOn.Location = new System.Drawing.Point(12, 86);
			cCancelOn.Name = "cCancelOn";
			cCancelOn.Size = new System.Drawing.Size(232, 35);
			cCancelOn.TabIndex = 28;
			cCancelOn.Text = "Time use credits";
			cCancelOn.UseVisualStyleBackColor = true;
			cJoinT.AutoSize = true;
			cJoinT.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cJoinT.ForeColor = System.Drawing.Color.OrangeRed;
			cJoinT.Location = new System.Drawing.Point(276, 123);
			cJoinT.Name = "cJoinT";
			cJoinT.Size = new System.Drawing.Size(220, 35);
			cJoinT.TabIndex = 29;
			cJoinT.Text = "Only one ticket ";
			cJoinT.UseVisualStyleBackColor = true;
			rDOM.AutoSize = true;
			rDOM.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			rDOM.Location = new System.Drawing.Point(267, 434);
			rDOM.Name = "rDOM";
			rDOM.Size = new System.Drawing.Size(159, 35);
			rDOM.TabIndex = 30;
			rDOM.Text = "REP.DOM";
			rDOM.UseVisualStyleBackColor = true;
			cGAS.AutoSize = true;
			cGAS.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cGAS.Location = new System.Drawing.Point(12, 197);
			cGAS.Name = "cGAS";
			cGAS.Size = new System.Drawing.Size(171, 35);
			cGAS.TabIndex = 31;
			cGAS.Text = "Ticket GAS";
			cGAS.UseVisualStyleBackColor = true;
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(508, 611);
			base.ControlBox = false;
			base.Controls.Add(cGAS);
			base.Controls.Add(rDOM);
			base.Controls.Add(cJoinT);
			base.Controls.Add(cCancelOn);
			base.Controls.Add(cBill);
			base.Controls.Add(cHide);
			base.Controls.Add(cATICKET);
			base.Controls.Add(cBAR);
			base.Controls.Add(cMOUSE_EMU);
			base.Controls.Add(rREAIS);
			base.Controls.Add(rEUROS);
			base.Controls.Add(cMOUSE);
			base.Controls.Add(cDemo);
			base.Controls.Add(cFree);
			base.Controls.Add(eRCred);
			base.Controls.Add(lRCred);
			base.Controls.Add(eCred);
			base.Controls.Add(lCred);
			base.Controls.Add(cLanguage);
			base.Controls.Add(lLanguage);
			base.Controls.Add(cCredits);
			base.Controls.Add(bCancel);
			base.Controls.Add(bOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DLG_Preferencias";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Preferences";
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
