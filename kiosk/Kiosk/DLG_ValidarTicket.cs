using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
	public class DLG_ValidarTicket : Form
	{
		public bool OK;

		public Configuracion opciones;

		public MainWindow MWin;

		private IContainer components = null;

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

		public string Ticket
		{
			get
			{
				return eBar.Text;
			}
			set
			{
				lTicket.Text = value;
				Invalidate();
			}
		}

		public DLG_ValidarTicket(ref Configuracion _opc)
		{
			OK = false;
			opciones = _opc;
			MWin = null;
			InitializeComponent();
			Localize();
			lCODE.Text = "-";
			lVal.Text = "-";
			lTime.Text = "-";
			panel1.BackColor = Color.Gray;
		}

		private void Localize()
		{
			SuspendLayout();
			lP1.Text = opciones.Localize.Text("Scan code");
			ResumeLayout();
		}

		public void Update_Info()
		{
			lCODE.Text = opciones.Ticket_Verificar.CRC;
			lVal.Text = string.Format("{0}.{1:00} {2}", opciones.Ticket_Verificar.Pago / 100, opciones.Ticket_Verificar.Pago % 100, opciones.Localize.Text("Euros"));
			string text = string.Concat(opciones.Ticket_Verificar.Pago);
			lTime.Text = string.Format("{3:00}:{4:00} {2:00}/{1:00}/{0:00}", opciones.Ticket_Verificar.DataT.Day, opciones.Ticket_Verificar.DataT.Month, opciones.Ticket_Verificar.DataT.Year - 2000, opciones.Ticket_Verificar.DataT.Hour, opciones.Ticket_Verificar.DataT.Minute);
			string arg = $"{opciones.Ticket_Verificar.Pago:X}/{opciones.Ticket_Verificar.DataT.Day:X2}{opciones.Ticket_Verificar.DataT.Month:X}{opciones.Ticket_Verificar.DataT.Year - 2000:X2}/{opciones.Ticket_Verificar.DataT.Hour:X2}{opciones.Ticket_Verificar.DataT.Minute:X2}/{text.Length}";
			lSec.Text = $"{arg}";
			switch (opciones.Ticket_Verificar.Verificado)
			{
			case 0:
				panel1.BackColor = Color.Green;
				bValidar.Enabled = true;
				bValidar.Visible = true;
				lOKT.Text = "";
				break;
			case 1:
				panel1.BackColor = Color.Red;
				bValidar.Enabled = false;
				lOKT.Text = "TICKET CANCELLED";
				bValidar.Visible = false;
				break;
			case 2:
				lCODE.Text = "";
				lVal.Text = "";
				lTime.Text = "";
				lSec.Text = "";
				panel1.BackColor = Color.Red;
				bValidar.Enabled = false;
				lOKT.Text = "TICKET INVALID";
				bValidar.Visible = false;
				break;
			default:
				lCODE.Text = "";
				lVal.Text = "";
				lTime.Text = "";
				lSec.Text = "";
				panel1.BackColor = Color.Red;
				bValidar.Enabled = false;
				lOKT.Text = "TICKET INVALID";
				bValidar.Visible = false;
				break;
			}
			Invalidate();
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			MWin.TicketToCheck = "";
			MWin.TicketOK = 0;
			Close();
		}

		private void bCancel_Click(object sender, EventArgs e)
		{
			eBar.Text = "";
		}

		private void eBar_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				lCODE.Text = "";
				lVal.Text = "";
				lTime.Text = "";
				lTicket.Text = "";
				lSec.Text = "";
				lOKT.Text = "";
				MWin.TicketToCheck = "";
				MWin.TicketOK = 0;
				MWin.Parser_Ticket(opciones.Srv_User, eBar.Text, 1);
				eBar.Text = "";
				if (MWin.TicketOK == 1)
				{
					MWin.Opcions.Verificar_Ticket = 0;
					try
					{
						MWin.Opcions.Verificar_Ticket = int.Parse(MWin.LastTicket.Substring(1, 11));
					}
					catch
					{
					}
					Ticket = MWin.LastTicket;
					MWin.Srv_Verificar_Ticket(MWin.Opcions.Verificar_Ticket, 0);
				}
			}
		}

		private void DLG_ValidarTicket_Load(object sender, EventArgs e)
		{
			MWin.TicketOK = 0;
			tScan.Enabled = true;
			opciones.LastMouseMove = DateTime.Now;
		}

		private void tScan_Tick(object sender, EventArgs e)
		{
			if (MWin != null)
			{
				if (MWin.TicketOK == 1 && opciones.ModoPS2 == 1)
				{
					eBar.Text = "OK";
				}
				int num = (int)(DateTime.Now - opciones.LastMouseMove).TotalSeconds;
				if (num > 60)
				{
					Close();
				}
			}
		}

		private void bValidar_Click(object sender, EventArgs e)
		{
			string s = lTicket.Text.Substring(1, 11);
			int tck = int.Parse(s);
			MWin.Srv_Anular_Ticket(tck, _est: true, 0);
			bool flag = false;
			MWin.Ticket_Out_Conf(opciones.Impresora_Tck, opciones.Ticket_Verificar.Pago, opciones.Ticket_Verificar.Ticket, opciones.Ticket_Model, opciones.Ticket_Cut, opciones.Ticket_N_FEED, opciones.Ticket_N_HEAD, opciones.Ticket_60mm, opciones.Ticket_Verificar.DataT, opciones.Ticket_Verificar.CRC);
		}

		private void bAnular_Click(object sender, EventArgs e)
		{
			KeyPressEventArgs e2 = new KeyPressEventArgs('\r');
			eBar_KeyPress(sender, e2);
		}

		private void t1_Click(object sender, EventArgs e)
		{
			eBar.Text += "1";
		}

		private void t2_Click(object sender, EventArgs e)
		{
			eBar.Text += "2";
		}

		private void t3_Click(object sender, EventArgs e)
		{
			eBar.Text += "3";
		}

		private void t4_Click(object sender, EventArgs e)
		{
			eBar.Text += "4";
		}

		private void t5_Click(object sender, EventArgs e)
		{
			eBar.Text += "5";
		}

		private void t6_Click(object sender, EventArgs e)
		{
			eBar.Text += "6";
		}

		private void tB_Click(object sender, EventArgs e)
		{
			eBar.Text += "B";
		}

		private void tA_Click(object sender, EventArgs e)
		{
			eBar.Text += "A";
		}

		private void t7_Click(object sender, EventArgs e)
		{
			eBar.Text += "7";
		}

		private void t8_Click(object sender, EventArgs e)
		{
			eBar.Text += "8";
		}

		private void t9_Click(object sender, EventArgs e)
		{
			eBar.Text += "9";
		}

		private void t0_Click(object sender, EventArgs e)
		{
			eBar.Text += "0";
		}

		private void tC_Click(object sender, EventArgs e)
		{
			eBar.Text += "C";
		}

		private void tD_Click(object sender, EventArgs e)
		{
			eBar.Text += "D";
		}

		private void tE_Click(object sender, EventArgs e)
		{
			eBar.Text += "E";
		}

		private void tF_Click(object sender, EventArgs e)
		{
			eBar.Text += "F";
		}

		private void tG_Click(object sender, EventArgs e)
		{
			eBar.Text += "G";
		}

		private void t1_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '1')
			{
				eBar.Text += "1";
			}
			if (e.KeyChar == '2')
			{
				eBar.Text += "2";
			}
			if (e.KeyChar == '3')
			{
				eBar.Text += "3";
			}
			if (e.KeyChar == '4')
			{
				eBar.Text += "4";
			}
			if (e.KeyChar == '5')
			{
				eBar.Text += "5";
			}
			if (e.KeyChar == '6')
			{
				eBar.Text += "6";
			}
			if (e.KeyChar == '7')
			{
				eBar.Text += "7";
			}
			if (e.KeyChar == '8')
			{
				eBar.Text += "8";
			}
			if (e.KeyChar == '9')
			{
				eBar.Text += "9";
			}
			if (e.KeyChar == '0')
			{
				eBar.Text += "0";
			}
			if (e.KeyChar == 'A')
			{
				eBar.Text += "A";
			}
			if (e.KeyChar == 'a')
			{
				eBar.Text += "A";
			}
			if (e.KeyChar == 'B')
			{
				eBar.Text += "B";
			}
			if (e.KeyChar == 'b')
			{
				eBar.Text += "B";
			}
			if (e.KeyChar == 'C')
			{
				eBar.Text += "C";
			}
			if (e.KeyChar == 'c')
			{
				eBar.Text += "C";
			}
			if (e.KeyChar == 'D')
			{
				eBar.Text += "D";
			}
			if (e.KeyChar == 'd')
			{
				eBar.Text += "D";
			}
			if (e.KeyChar == 'E')
			{
				eBar.Text += "E";
			}
			if (e.KeyChar == 'e')
			{
				eBar.Text += "E";
			}
			if (e.KeyChar == 'F')
			{
				eBar.Text += "F";
			}
			if (e.KeyChar == 'f')
			{
				eBar.Text += "F";
			}
			if (e.KeyChar == 'G')
			{
				eBar.Text += "G";
			}
			if (e.KeyChar == 'g')
			{
				eBar.Text += "G";
			}
			if (e.KeyChar == '\b' && eBar.Text.Length > 0)
			{
				eBar.Text = eBar.Text.Substring(0, eBar.Text.Length - 1);
			}
			if (e.KeyChar == '\r')
			{
				eBar_KeyPress(sender, e);
			}
			if (e.KeyChar == '\u001b')
			{
				MWin.TicketToCheck = "";
				MWin.TicketOK = 0;
				Close();
			}
		}

		private void tX_Click(object sender, EventArgs e)
		{
			if (eBar.Text.Length > 0)
			{
				eBar.Text = eBar.Text.Substring(0, eBar.Text.Length - 1);
			}
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
			components = new System.ComponentModel.Container();
			lP1 = new System.Windows.Forms.Label();
			eBar = new System.Windows.Forms.TextBox();
			panel1 = new System.Windows.Forms.Panel();
			lOKT = new System.Windows.Forms.Label();
			lSec = new System.Windows.Forms.Label();
			lTicket = new System.Windows.Forms.Label();
			bValidar = new System.Windows.Forms.Button();
			lTime = new System.Windows.Forms.Label();
			lVal = new System.Windows.Forms.Label();
			lCODE = new System.Windows.Forms.Label();
			tScan = new System.Windows.Forms.Timer(components);
			panel2 = new System.Windows.Forms.Panel();
			tX = new System.Windows.Forms.Button();
			tG = new System.Windows.Forms.Button();
			tF = new System.Windows.Forms.Button();
			tE = new System.Windows.Forms.Button();
			tD = new System.Windows.Forms.Button();
			tC = new System.Windows.Forms.Button();
			tB = new System.Windows.Forms.Button();
			tA = new System.Windows.Forms.Button();
			t0 = new System.Windows.Forms.Button();
			t9 = new System.Windows.Forms.Button();
			t8 = new System.Windows.Forms.Button();
			t7 = new System.Windows.Forms.Button();
			t6 = new System.Windows.Forms.Button();
			t5 = new System.Windows.Forms.Button();
			t4 = new System.Windows.Forms.Button();
			t3 = new System.Windows.Forms.Button();
			t2 = new System.Windows.Forms.Button();
			t1 = new System.Windows.Forms.Button();
			bCancel = new System.Windows.Forms.Button();
			bAnular = new System.Windows.Forms.Button();
			bOK = new System.Windows.Forms.Button();
			panel1.SuspendLayout();
			panel2.SuspendLayout();
			SuspendLayout();
			lP1.Dock = System.Windows.Forms.DockStyle.Top;
			lP1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lP1.Location = new System.Drawing.Point(0, 0);
			lP1.Name = "lP1";
			lP1.Size = new System.Drawing.Size(277, 31);
			lP1.TabIndex = 19;
			lP1.Text = "Code";
			lP1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			eBar.AcceptsReturn = true;
			eBar.Dock = System.Windows.Forms.DockStyle.Top;
			eBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			eBar.Location = new System.Drawing.Point(0, 31);
			eBar.Name = "eBar";
			eBar.Size = new System.Drawing.Size(277, 38);
			eBar.TabIndex = 0;
			eBar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(eBar_KeyPress);
			panel1.Controls.Add(lOKT);
			panel1.Controls.Add(lSec);
			panel1.Controls.Add(lTicket);
			panel1.Controls.Add(bValidar);
			panel1.Controls.Add(lTime);
			panel1.Controls.Add(lVal);
			panel1.Controls.Add(lCODE);
			panel1.Location = new System.Drawing.Point(12, 12);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(295, 343);
			panel1.TabIndex = 20;
			lOKT.Dock = System.Windows.Forms.DockStyle.Top;
			lOKT.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lOKT.Location = new System.Drawing.Point(0, 188);
			lOKT.Name = "lOKT";
			lOKT.Size = new System.Drawing.Size(295, 32);
			lOKT.TabIndex = 24;
			lOKT.Text = "-";
			lOKT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			lSec.Dock = System.Windows.Forms.DockStyle.Top;
			lSec.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lSec.Location = new System.Drawing.Point(0, 156);
			lSec.Name = "lSec";
			lSec.Size = new System.Drawing.Size(295, 32);
			lSec.TabIndex = 23;
			lSec.Text = "-";
			lSec.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			lTicket.Dock = System.Windows.Forms.DockStyle.Top;
			lTicket.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lTicket.Location = new System.Drawing.Point(0, 124);
			lTicket.Name = "lTicket";
			lTicket.Size = new System.Drawing.Size(295, 32);
			lTicket.TabIndex = 20;
			lTicket.Text = "-";
			lTicket.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			bValidar.Dock = System.Windows.Forms.DockStyle.Bottom;
			bValidar.Enabled = false;
			bValidar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bValidar.Image = Kiosk.Properties.Resources.TicketOK;
			bValidar.Location = new System.Drawing.Point(0, 289);
			bValidar.Name = "bValidar";
			bValidar.Size = new System.Drawing.Size(295, 54);
			bValidar.TabIndex = 0;
			bValidar.UseVisualStyleBackColor = false;
			bValidar.Click += new System.EventHandler(bValidar_Click);
			bValidar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			lTime.Dock = System.Windows.Forms.DockStyle.Top;
			lTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lTime.Location = new System.Drawing.Point(0, 92);
			lTime.Name = "lTime";
			lTime.Size = new System.Drawing.Size(295, 32);
			lTime.TabIndex = 21;
			lTime.Text = "-";
			lTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			lVal.Dock = System.Windows.Forms.DockStyle.Top;
			lVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lVal.Location = new System.Drawing.Point(0, 46);
			lVal.Name = "lVal";
			lVal.Size = new System.Drawing.Size(295, 46);
			lVal.TabIndex = 22;
			lVal.Text = "-";
			lVal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			lCODE.Dock = System.Windows.Forms.DockStyle.Top;
			lCODE.Font = new System.Drawing.Font("Microsoft Sans Serif", 32f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lCODE.Location = new System.Drawing.Point(0, 0);
			lCODE.Name = "lCODE";
			lCODE.Size = new System.Drawing.Size(295, 46);
			lCODE.TabIndex = 19;
			lCODE.Text = "-";
			lCODE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			tScan.Interval = 500;
			tScan.Tick += new System.EventHandler(tScan_Tick);
			panel2.Controls.Add(tX);
			panel2.Controls.Add(tG);
			panel2.Controls.Add(tF);
			panel2.Controls.Add(tE);
			panel2.Controls.Add(tD);
			panel2.Controls.Add(tC);
			panel2.Controls.Add(tB);
			panel2.Controls.Add(tA);
			panel2.Controls.Add(t0);
			panel2.Controls.Add(t9);
			panel2.Controls.Add(t8);
			panel2.Controls.Add(t7);
			panel2.Controls.Add(t6);
			panel2.Controls.Add(t5);
			panel2.Controls.Add(t4);
			panel2.Controls.Add(t3);
			panel2.Controls.Add(t2);
			panel2.Controls.Add(t1);
			panel2.Controls.Add(eBar);
			panel2.Controls.Add(lP1);
			panel2.Controls.Add(bCancel);
			panel2.Controls.Add(bAnular);
			panel2.Controls.Add(bOK);
			panel2.Location = new System.Drawing.Point(313, 12);
			panel2.Name = "panel2";
			panel2.Size = new System.Drawing.Size(277, 342);
			panel2.TabIndex = 21;
			tX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			tX.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			tX.Image = Kiosk.Properties.Resources.backspace;
			tX.Location = new System.Drawing.Point(110, 235);
			tX.Name = "tX";
			tX.Size = new System.Drawing.Size(56, 56);
			tX.TabIndex = 18;
			tX.UseVisualStyleBackColor = true;
			tX.Click += new System.EventHandler(tX_Click);
			tX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			tG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			tG.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			tG.Location = new System.Drawing.Point(55, 235);
			tG.Name = "tG";
			tG.Size = new System.Drawing.Size(56, 56);
			tG.TabIndex = 17;
			tG.Text = "G";
			tG.UseVisualStyleBackColor = true;
			tG.Click += new System.EventHandler(tG_Click);
			tG.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			tF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			tF.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			tF.Location = new System.Drawing.Point(0, 235);
			tF.Name = "tF";
			tF.Size = new System.Drawing.Size(56, 56);
			tF.TabIndex = 16;
			tF.Text = "F";
			tF.UseVisualStyleBackColor = true;
			tF.Click += new System.EventHandler(tF_Click);
			tF.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			tE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			tE.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			tE.Location = new System.Drawing.Point(220, 180);
			tE.Name = "tE";
			tE.Size = new System.Drawing.Size(56, 56);
			tE.TabIndex = 15;
			tE.Text = "E";
			tE.UseVisualStyleBackColor = true;
			tE.Click += new System.EventHandler(tE_Click);
			tE.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			tD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			tD.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			tD.Location = new System.Drawing.Point(165, 180);
			tD.Name = "tD";
			tD.Size = new System.Drawing.Size(56, 56);
			tD.TabIndex = 14;
			tD.Text = "D";
			tD.UseVisualStyleBackColor = true;
			tD.Click += new System.EventHandler(tD_Click);
			tD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			tC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			tC.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			tC.Location = new System.Drawing.Point(110, 180);
			tC.Name = "tC";
			tC.Size = new System.Drawing.Size(56, 56);
			tC.TabIndex = 13;
			tC.Text = "C";
			tC.UseVisualStyleBackColor = true;
			tC.Click += new System.EventHandler(tC_Click);
			tC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			tB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			tB.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			tB.Location = new System.Drawing.Point(55, 180);
			tB.Name = "tB";
			tB.Size = new System.Drawing.Size(56, 56);
			tB.TabIndex = 12;
			tB.Text = "B";
			tB.UseVisualStyleBackColor = true;
			tB.Click += new System.EventHandler(tB_Click);
			tB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			tA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			tA.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			tA.Location = new System.Drawing.Point(0, 180);
			tA.Name = "tA";
			tA.Size = new System.Drawing.Size(56, 56);
			tA.TabIndex = 11;
			tA.Text = "A";
			tA.UseVisualStyleBackColor = true;
			tA.Click += new System.EventHandler(tA_Click);
			tA.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			t0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			t0.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			t0.Location = new System.Drawing.Point(220, 125);
			t0.Name = "t0";
			t0.Size = new System.Drawing.Size(56, 56);
			t0.TabIndex = 10;
			t0.Text = "0";
			t0.UseVisualStyleBackColor = true;
			t0.Click += new System.EventHandler(t0_Click);
			t0.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			t9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			t9.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			t9.Location = new System.Drawing.Point(165, 125);
			t9.Name = "t9";
			t9.Size = new System.Drawing.Size(56, 56);
			t9.TabIndex = 9;
			t9.Text = "9";
			t9.UseVisualStyleBackColor = true;
			t9.Click += new System.EventHandler(t9_Click);
			t9.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			t8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			t8.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			t8.Location = new System.Drawing.Point(110, 125);
			t8.Name = "t8";
			t8.Size = new System.Drawing.Size(56, 56);
			t8.TabIndex = 8;
			t8.Text = "8";
			t8.UseVisualStyleBackColor = true;
			t8.Click += new System.EventHandler(t8_Click);
			t8.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			t7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			t7.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			t7.Location = new System.Drawing.Point(55, 125);
			t7.Name = "t7";
			t7.Size = new System.Drawing.Size(56, 56);
			t7.TabIndex = 7;
			t7.Text = "7";
			t7.UseVisualStyleBackColor = true;
			t7.Click += new System.EventHandler(t7_Click);
			t7.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			t6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			t6.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			t6.Location = new System.Drawing.Point(0, 125);
			t6.Name = "t6";
			t6.Size = new System.Drawing.Size(56, 56);
			t6.TabIndex = 6;
			t6.Text = "6";
			t6.UseVisualStyleBackColor = true;
			t6.Click += new System.EventHandler(t6_Click);
			t6.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			t5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			t5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			t5.Location = new System.Drawing.Point(220, 70);
			t5.Name = "t5";
			t5.Size = new System.Drawing.Size(56, 56);
			t5.TabIndex = 5;
			t5.Text = "5";
			t5.UseVisualStyleBackColor = true;
			t5.Click += new System.EventHandler(t5_Click);
			t5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			t4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			t4.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			t4.Location = new System.Drawing.Point(165, 70);
			t4.Name = "t4";
			t4.Size = new System.Drawing.Size(56, 56);
			t4.TabIndex = 4;
			t4.Text = "4";
			t4.UseVisualStyleBackColor = true;
			t4.Click += new System.EventHandler(t4_Click);
			t4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			t3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			t3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			t3.Location = new System.Drawing.Point(110, 70);
			t3.Name = "t3";
			t3.Size = new System.Drawing.Size(56, 56);
			t3.TabIndex = 3;
			t3.Text = "3";
			t3.UseVisualStyleBackColor = true;
			t3.Click += new System.EventHandler(t3_Click);
			t3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			t2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			t2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			t2.Location = new System.Drawing.Point(55, 70);
			t2.Name = "t2";
			t2.Size = new System.Drawing.Size(56, 56);
			t2.TabIndex = 2;
			t2.Text = "2";
			t2.UseVisualStyleBackColor = true;
			t2.Click += new System.EventHandler(t2_Click);
			t2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			t1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			t1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			t1.Location = new System.Drawing.Point(0, 70);
			t1.Name = "t1";
			t1.Size = new System.Drawing.Size(56, 56);
			t1.TabIndex = 1;
			t1.Text = "1";
			t1.UseVisualStyleBackColor = true;
			t1.Click += new System.EventHandler(t1_Click);
			t1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			bCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bCancel.Image = Kiosk.Properties.Resources.ico_barcodeX;
			bCancel.Location = new System.Drawing.Point(165, 235);
			bCancel.Name = "bCancel";
			bCancel.Size = new System.Drawing.Size(56, 56);
			bCancel.TabIndex = 19;
			bCancel.UseVisualStyleBackColor = true;
			bCancel.Click += new System.EventHandler(bCancel_Click);
			bCancel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			bAnular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bAnular.Image = Kiosk.Properties.Resources.TicketFind;
			bAnular.Location = new System.Drawing.Point(0, 290);
			bAnular.Name = "bAnular";
			bAnular.Size = new System.Drawing.Size(111, 52);
			bAnular.TabIndex = 20;
			bAnular.UseVisualStyleBackColor = true;
			bAnular.Click += new System.EventHandler(bAnular_Click);
			bAnular.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			bOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bOK.Image = Kiosk.Properties.Resources.dlgExit;
			bOK.Location = new System.Drawing.Point(165, 290);
			bOK.Name = "bOK";
			bOK.Size = new System.Drawing.Size(111, 52);
			bOK.TabIndex = 21;
			bOK.UseVisualStyleBackColor = true;
			bOK.Click += new System.EventHandler(bOK_Click);
			bOK.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(602, 367);
			base.Controls.Add(panel2);
			base.Controls.Add(panel1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "DLG_ValidarTicket";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "DLG_ValidarTicket";
			base.TopMost = true;
			base.Load += new System.EventHandler(DLG_ValidarTicket_Load);
			base.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			panel1.ResumeLayout(false);
			panel2.ResumeLayout(false);
			panel2.PerformLayout();
			ResumeLayout(false);
		}
	}
}
