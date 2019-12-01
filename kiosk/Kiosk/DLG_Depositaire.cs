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
		public bool OK;

		public Configuracion opciones;

		public MainWindow MWin;

		private IContainer components = null;

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
			OK = false;
			opciones = _opc;
			MWin = null;
			InitializeComponent();
			eLine1.Text = opciones.Srv_ID_Lin1;
			eLine2.Text = opciones.Srv_ID_Lin2;
			eLine3.Text = opciones.Srv_ID_Lin3;
			eLine4.Text = opciones.Srv_ID_Lin4;
			eLine5.Text = opciones.Srv_ID_Lin5;
			eLine6.Text = opciones.Srv_ID_LinBottom;
			bool flag = false;
			opciones.ModoTickets = 1;
			cTicket.Checked = true;
			cTicket.Enabled = false;
			cPS2.Checked = ((opciones.ModoPS2 == 1) ? true : false);
			Localize();
		}

		private void Localize()
		{
			SuspendLayout();
			Text = opciones.Localize.Text("Ticket");
			lLine1.Text = opciones.Localize.Text("Name");
			lLine2.Text = opciones.Localize.Text("Line 1");
			lLine3.Text = opciones.Localize.Text("Line 2");
			lLine4.Text = opciones.Localize.Text("Line 3");
			lLine5.Text = opciones.Localize.Text("RC");
			lLine6.Text = opciones.Localize.Text("End Ticket Text");
			cTicket.Text = opciones.Localize.Text("Ticket system");
			lP1.Text = opciones.Localize.Text("Printer");
			bTick.Text = opciones.Localize.Text("Test");
			bBar.Text = opciones.Localize.Text("Detect");
			lBar.Text = opciones.Localize.Text("Barcode reader");
			ResumeLayout();
		}

		private void bOk_Click(object sender, EventArgs e)
		{
			if (opciones.modo_XP == 1)
			{
				MessageBox.Show("XP Detected. System ticket not avalilable");
				cTicket.Checked = false;
			}
			OK = true;
			opciones.Srv_ID_Lin1 = eLine1.Text;
			opciones.Srv_ID_Lin2 = eLine2.Text;
			opciones.Srv_ID_Lin3 = eLine3.Text;
			opciones.Srv_ID_Lin4 = eLine4.Text;
			opciones.Srv_ID_Lin5 = eLine5.Text;
			opciones.Srv_ID_LinBottom = eLine6.Text;
			opciones.Impresora_Tck = cP1.Text;
			opciones.Barcode = eBar.Text;
			opciones.ModoTickets = (cTicket.Checked ? 1 : 0);
			opciones.ModoPS2 = (cPS2.Checked ? 1 : 0);
			opciones.Ticket_Cut = (cCut.Checked ? 1 : 0);
			opciones.Ticket_60mm = (c60mm.Checked ? 1 : 0);
			opciones.Ticket_Model = ((cPModel.Text == "ESC/POS") ? 1 : 0);
			int ticket_N_FEED = 1;
			try
			{
				ticket_N_FEED = int.Parse(tPLines.Text);
			}
			catch
			{
			}
			opciones.Ticket_N_FEED = ticket_N_FEED;
			ticket_N_FEED = 0;
			try
			{
				ticket_N_FEED = int.Parse(tPHEAD.Text);
			}
			catch
			{
			}
			opciones.Ticket_N_HEAD = ticket_N_FEED;
			opciones.Save_Net();
			Close();
		}

		private void bCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void DLG_Depositaire_Load(object sender, EventArgs e)
		{
			if (opciones.modo_XP == 0)
			{
				MainWindow.Clean_Printer();
				PrintDocument printDocument = new PrintDocument();
				foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
				{
					cP1.Items.Add(installedPrinter);
					cP1.SelectedIndex = cP1.Items.IndexOf(installedPrinter);
				}
			}
			cPModel.Items.Clear();
			cPModel.Items.Add("Nii");
			cPModel.Items.Add("ESC/POS");
			if (opciones.Ticket_Model == 0)
			{
				cPModel.SelectedIndex = cPModel.Items.IndexOf("Nii");
			}
			else
			{
				cPModel.SelectedIndex = cPModel.Items.IndexOf("ESC/POS");
			}
			cCut.Checked = ((opciones.Ticket_Cut != 0) ? true : false);
			c60mm.Checked = ((opciones.Ticket_60mm != 0) ? true : false);
			tPLines.Text = string.Concat(opciones.Ticket_N_FEED);
			tPHEAD.Text = string.Concat(opciones.Ticket_N_HEAD);
			cP1.Text = opciones.Impresora_Tck;
			eBar.Text = opciones.Barcode;
		}

		private void bTick_Click(object sender, EventArgs e)
		{
			int skeep = 1;
			try
			{
				skeep = int.Parse(tPLines.Text);
			}
			catch
			{
			}
			int preskeep = 0;
			try
			{
				preskeep = int.Parse(tPHEAD.Text);
			}
			catch
			{
			}
			MWin.Ticket(cP1.Text, 0m, 0, 0, (cPModel.Text == "ESC/POS") ? 1 : 0, cCut.Checked ? 1 : 0, skeep, preskeep, c60mm.Checked ? 1 : 0);
		}

		private void bBar_Click(object sender, EventArgs e)
		{
			int forceAllKey = opciones.ForceAllKey;
			opciones.ForceAllKey = 0;
			opciones.Last_Device = "";
			opciones.Test_Barcode = 1;
			MWin.TicketToCheck = "";
			MWin.TicketOK = 0;
			DLG_Message dLG_Message = new DLG_Message(opciones.Localize.Text("Read Barcode"), ref opciones);
			dLG_Message.ShowDialog();
			opciones.Test_Barcode = 0;
			if (MWin.TicketOK == 1)
			{
				eBar.Text = opciones.Last_Device;
			}
			else
			{
				eBar.Text = "";
			}
			opciones.ForceAllKey = forceAllKey;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			int skeep = 1;
			try
			{
				skeep = int.Parse(tPLines.Text);
			}
			catch
			{
			}
			int preskeep = 0;
			try
			{
				preskeep = int.Parse(tPHEAD.Text);
			}
			catch
			{
			}
			MWin.Ticket_Out(cP1.Text, 0m, 0, (cPModel.Text == "ESC/POS") ? 1 : 0, cCut.Checked ? 1 : 0, skeep, preskeep, c60mm.Checked ? 1 : 0, DateTime.Now, "000", 0);
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
			eLine2 = new System.Windows.Forms.TextBox();
			eLine1 = new System.Windows.Forms.TextBox();
			lLine2 = new System.Windows.Forms.Label();
			lLine1 = new System.Windows.Forms.Label();
			eLine3 = new System.Windows.Forms.TextBox();
			lLine3 = new System.Windows.Forms.Label();
			eLine4 = new System.Windows.Forms.TextBox();
			lLine4 = new System.Windows.Forms.Label();
			eLine5 = new System.Windows.Forms.TextBox();
			lLine5 = new System.Windows.Forms.Label();
			cTicket = new System.Windows.Forms.CheckBox();
			bTick = new System.Windows.Forms.Button();
			bBar = new System.Windows.Forms.Button();
			eBar = new System.Windows.Forms.TextBox();
			lBar = new System.Windows.Forms.Label();
			lP1 = new System.Windows.Forms.Label();
			cP1 = new System.Windows.Forms.ComboBox();
			eLine6 = new System.Windows.Forms.TextBox();
			lLine6 = new System.Windows.Forms.Label();
			cPS2 = new System.Windows.Forms.CheckBox();
			cPModel = new System.Windows.Forms.ComboBox();
			lPModel = new System.Windows.Forms.Label();
			cCut = new System.Windows.Forms.CheckBox();
			lPLines = new System.Windows.Forms.Label();
			tPLines = new System.Windows.Forms.TextBox();
			button1 = new System.Windows.Forms.Button();
			bCancel = new System.Windows.Forms.Button();
			bOk = new System.Windows.Forms.Button();
			c60mm = new System.Windows.Forms.CheckBox();
			tPHEAD = new System.Windows.Forms.TextBox();
			label1 = new System.Windows.Forms.Label();
			SuspendLayout();
			eLine2.Location = new System.Drawing.Point(17, 284);
			eLine2.Name = "eLine2";
			eLine2.Size = new System.Drawing.Size(455, 20);
			eLine2.TabIndex = 7;
			eLine1.Location = new System.Drawing.Point(17, 235);
			eLine1.Name = "eLine1";
			eLine1.Size = new System.Drawing.Size(455, 20);
			eLine1.TabIndex = 6;
			lLine2.AutoSize = true;
			lLine2.Location = new System.Drawing.Point(17, 263);
			lLine2.Name = "lLine2";
			lLine2.Size = new System.Drawing.Size(10, 13);
			lLine2.TabIndex = 28;
			lLine2.Text = "-";
			lLine1.AutoSize = true;
			lLine1.Location = new System.Drawing.Point(17, 214);
			lLine1.Name = "lLine1";
			lLine1.Size = new System.Drawing.Size(10, 13);
			lLine1.TabIndex = 27;
			lLine1.Text = "-";
			eLine3.Location = new System.Drawing.Point(17, 333);
			eLine3.Name = "eLine3";
			eLine3.Size = new System.Drawing.Size(455, 20);
			eLine3.TabIndex = 8;
			lLine3.AutoSize = true;
			lLine3.Location = new System.Drawing.Point(17, 312);
			lLine3.Name = "lLine3";
			lLine3.Size = new System.Drawing.Size(10, 13);
			lLine3.TabIndex = 30;
			lLine3.Text = "-";
			eLine4.Location = new System.Drawing.Point(17, 382);
			eLine4.Name = "eLine4";
			eLine4.Size = new System.Drawing.Size(455, 20);
			eLine4.TabIndex = 9;
			lLine4.AutoSize = true;
			lLine4.Location = new System.Drawing.Point(17, 361);
			lLine4.Name = "lLine4";
			lLine4.Size = new System.Drawing.Size(10, 13);
			lLine4.TabIndex = 32;
			lLine4.Text = "-";
			eLine5.Location = new System.Drawing.Point(17, 431);
			eLine5.Name = "eLine5";
			eLine5.Size = new System.Drawing.Size(455, 20);
			eLine5.TabIndex = 10;
			lLine5.AutoSize = true;
			lLine5.Location = new System.Drawing.Point(17, 410);
			lLine5.Name = "lLine5";
			lLine5.Size = new System.Drawing.Size(10, 13);
			lLine5.TabIndex = 34;
			lLine5.Text = "-";
			cTicket.AutoSize = true;
			cTicket.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cTicket.Location = new System.Drawing.Point(17, 12);
			cTicket.Name = "cTicket";
			cTicket.Size = new System.Drawing.Size(201, 35);
			cTicket.TabIndex = 0;
			cTicket.Text = "Ticket system";
			cTicket.UseVisualStyleBackColor = true;
			bTick.Location = new System.Drawing.Point(408, 56);
			bTick.Name = "bTick";
			bTick.Size = new System.Drawing.Size(64, 48);
			bTick.TabIndex = 2;
			bTick.Text = "Test";
			bTick.UseVisualStyleBackColor = true;
			bTick.Click += new System.EventHandler(bTick_Click);
			bBar.Location = new System.Drawing.Point(408, 144);
			bBar.Name = "bBar";
			bBar.Size = new System.Drawing.Size(64, 48);
			bBar.TabIndex = 4;
			bBar.Text = "Detect";
			bBar.UseVisualStyleBackColor = true;
			bBar.Click += new System.EventHandler(bBar_Click);
			eBar.Location = new System.Drawing.Point(17, 157);
			eBar.Name = "eBar";
			eBar.Size = new System.Drawing.Size(385, 20);
			eBar.TabIndex = 3;
			lBar.AutoSize = true;
			lBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lBar.Location = new System.Drawing.Point(14, 141);
			lBar.Name = "lBar";
			lBar.Size = new System.Drawing.Size(80, 13);
			lBar.TabIndex = 40;
			lBar.Text = "Barcode reader";
			lP1.AutoSize = true;
			lP1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lP1.Location = new System.Drawing.Point(14, 55);
			lP1.Name = "lP1";
			lP1.Size = new System.Drawing.Size(37, 13);
			lP1.TabIndex = 37;
			lP1.Text = "Printer";
			cP1.FormattingEnabled = true;
			cP1.Location = new System.Drawing.Point(17, 71);
			cP1.Name = "cP1";
			cP1.Size = new System.Drawing.Size(385, 21);
			cP1.TabIndex = 1;
			eLine6.Location = new System.Drawing.Point(17, 480);
			eLine6.Name = "eLine6";
			eLine6.Size = new System.Drawing.Size(455, 20);
			eLine6.TabIndex = 11;
			lLine6.AutoSize = true;
			lLine6.Location = new System.Drawing.Point(17, 459);
			lLine6.Name = "lLine6";
			lLine6.Size = new System.Drawing.Size(10, 13);
			lLine6.TabIndex = 42;
			lLine6.Text = "-";
			cPS2.AutoSize = true;
			cPS2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cPS2.Location = new System.Drawing.Point(17, 187);
			cPS2.Name = "cPS2";
			cPS2.Size = new System.Drawing.Size(179, 17);
			cPS2.TabIndex = 5;
			cPS2.Text = "PS/2 Barcode + PS/2 Keyboard";
			cPS2.UseVisualStyleBackColor = true;
			cPModel.FormattingEnabled = true;
			cPModel.Location = new System.Drawing.Point(17, 114);
			cPModel.Name = "cPModel";
			cPModel.Size = new System.Drawing.Size(88, 21);
			cPModel.TabIndex = 43;
			lPModel.AutoSize = true;
			lPModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lPModel.Location = new System.Drawing.Point(17, 98);
			lPModel.Name = "lPModel";
			lPModel.Size = new System.Drawing.Size(60, 13);
			lPModel.TabIndex = 44;
			lPModel.Text = "Printer type";
			cCut.AutoSize = true;
			cCut.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cCut.Location = new System.Drawing.Point(300, 116);
			cCut.Name = "cCut";
			cCut.Size = new System.Drawing.Size(67, 17);
			cCut.TabIndex = 45;
			cCut.Text = "Auto Cut";
			cCut.UseVisualStyleBackColor = true;
			lPLines.AutoSize = true;
			lPLines.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lPLines.Location = new System.Drawing.Point(198, 97);
			lPLines.Name = "lPLines";
			lPLines.Size = new System.Drawing.Size(113, 13);
			lPLines.TabIndex = 46;
			lPLines.Text = "Skeep lines before cut";
			tPLines.Location = new System.Drawing.Point(201, 116);
			tPLines.Name = "tPLines";
			tPLines.Size = new System.Drawing.Size(71, 20);
			tPLines.TabIndex = 47;
			button1.Location = new System.Drawing.Point(408, 2);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(64, 48);
			button1.TabIndex = 48;
			button1.Text = "Test Out";
			button1.UseVisualStyleBackColor = true;
			button1.Visible = false;
			button1.Click += new System.EventHandler(button1_Click);
			bCancel.BackgroundImage = Kiosk.Properties.Resources.ico_del;
			bCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			bCancel.Location = new System.Drawing.Point(338, 506);
			bCancel.Name = "bCancel";
			bCancel.Size = new System.Drawing.Size(64, 48);
			bCancel.TabIndex = 13;
			bCancel.UseVisualStyleBackColor = true;
			bCancel.Click += new System.EventHandler(bCancel_Click);
			bOk.BackgroundImage = Kiosk.Properties.Resources.ico_ok;
			bOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			bOk.Location = new System.Drawing.Point(408, 506);
			bOk.Name = "bOk";
			bOk.Size = new System.Drawing.Size(64, 48);
			bOk.TabIndex = 12;
			bOk.UseVisualStyleBackColor = true;
			bOk.Click += new System.EventHandler(bOk_Click);
			c60mm.AutoSize = true;
			c60mm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			c60mm.Location = new System.Drawing.Point(387, 115);
			c60mm.Name = "c60mm";
			c60mm.Size = new System.Drawing.Size(85, 17);
			c60mm.TabIndex = 49;
			c60mm.Text = "60mm Paper";
			c60mm.UseVisualStyleBackColor = true;
			tPHEAD.Location = new System.Drawing.Point(117, 116);
			tPHEAD.Name = "tPHEAD";
			tPHEAD.Size = new System.Drawing.Size(71, 20);
			tPHEAD.TabIndex = 50;
			label1.AutoSize = true;
			label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			label1.Location = new System.Drawing.Point(113, 96);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(65, 13);
			label1.TabIndex = 51;
			label1.Text = "Skeep head";
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(484, 561);
			base.ControlBox = false;
			base.Controls.Add(label1);
			base.Controls.Add(tPHEAD);
			base.Controls.Add(c60mm);
			base.Controls.Add(button1);
			base.Controls.Add(tPLines);
			base.Controls.Add(lPLines);
			base.Controls.Add(cCut);
			base.Controls.Add(lPModel);
			base.Controls.Add(cPModel);
			base.Controls.Add(cPS2);
			base.Controls.Add(eLine6);
			base.Controls.Add(lLine6);
			base.Controls.Add(bTick);
			base.Controls.Add(bBar);
			base.Controls.Add(eBar);
			base.Controls.Add(lBar);
			base.Controls.Add(lP1);
			base.Controls.Add(cP1);
			base.Controls.Add(cTicket);
			base.Controls.Add(eLine5);
			base.Controls.Add(lLine5);
			base.Controls.Add(eLine4);
			base.Controls.Add(lLine4);
			base.Controls.Add(eLine3);
			base.Controls.Add(lLine3);
			base.Controls.Add(eLine2);
			base.Controls.Add(eLine1);
			base.Controls.Add(lLine2);
			base.Controls.Add(lLine1);
			base.Controls.Add(bCancel);
			base.Controls.Add(bOk);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "DLG_Depositaire";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Ticket";
			base.Load += new System.EventHandler(DLG_Depositaire_Load);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
