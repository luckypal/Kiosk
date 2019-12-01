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
		public bool OK;

		public Configuracion opciones;

		public MainWindow MWin;

		private IContainer components = null;

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
			OK = false;
			opciones = _opc;
			MWin = null;
			InitializeComponent();
			Localize();
		}

		private void Localize()
		{
			SuspendLayout();
			lP1.Text = opciones.Localize.Text("Scan code");
			ResumeLayout();
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			KeyPressEventArgs e2 = new KeyPressEventArgs('\r');
			eBar_KeyPress(sender, e2);
			Close();
		}

		private void bCancel_Click(object sender, EventArgs e)
		{
			eBar.Text = "";
		}

		private void bRestet_Click(object sender, EventArgs e)
		{
			MWin.TicketToCheck = "";
			MWin.TicketOK = 0;
			MWin.Parser_Ticket(opciones.Srv_User, eBar.Text, 0);
			eBar.Text = "";
			if (MWin.TicketOK == 1)
			{
				MWin.Validate_Ticket();
				Close();
			}
		}

		private void eBar_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar != '\r')
			{
				return;
			}
			MWin.TicketToCheck = "";
			MWin.TicketOK = 0;
			int num = MWin.Parser_Ticket(opciones.Srv_User, eBar.Text, 1);
			MWin.LastTicket = eBar.Text;
			eBar.Text = "";
			if (MWin.TicketOK != 1)
			{
				return;
			}
			if (num == 1)
			{
				int _ticket = 0;
				int _id = 0;
				Gestion.Decode_Mod10(MWin.LastTicket, out _ticket, out _id);
				opciones.TicketTemps = _ticket;
				opciones.IdTicketTemps = _id;
				if (opciones.IdTicketTemps > 0)
				{
					MWin.Srv_Sub_Ticket(_id, 0);
				}
			}
			if (num == 2)
			{
				bool flag = false;
				if (MWin.ValidacioTicket == null)
				{
					MWin.ValidacioTicket = new DLG_ValidarTicket(ref opciones);
					MWin.ValidacioTicket.MWin = MWin;
				}
				if (MWin.ValidacioTicket.IsDisposed)
				{
					MWin.ValidacioTicket = new DLG_ValidarTicket(ref opciones);
					MWin.ValidacioTicket.MWin = MWin;
				}
				MWin.ValidacioTicket.Ticket = MWin.LastTicket;
				try
				{
					MWin.Opcions.Verificar_Ticket = int.Parse(MWin.LastTicket.Substring(1, 11));
				}
				catch
				{
				}
				MWin.Srv_Verificar_Ticket(MWin.Opcions.Verificar_Ticket, 0);
				MWin.ValidacioTicket.Show();
				Close();
			}
		}

		private void DLG_Barcode_Load(object sender, EventArgs e)
		{
			MWin.TicketOK = 0;
			tScan.Enabled = true;
			opciones.LastMouseMove = DateTime.Now;
		}

		private void tScan_Tick(object sender, EventArgs e)
		{
			if (MWin != null && MWin.TicketOK == 1)
			{
				if (opciones.ModoPS2 == 1)
				{
					eBar.Text = "OK";
				}
				else
				{
					Close();
				}
			}
			int num = (int)(DateTime.Now - opciones.LastMouseMove).TotalSeconds;
			if (num > 60)
			{
				Close();
			}
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
			eBar = new System.Windows.Forms.TextBox();
			lP1 = new System.Windows.Forms.Label();
			tScan = new System.Windows.Forms.Timer(components);
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
			tX = new System.Windows.Forms.Button();
			button1 = new System.Windows.Forms.Button();
			bRestet = new System.Windows.Forms.Button();
			bOK = new System.Windows.Forms.Button();
			SuspendLayout();
			eBar.AcceptsReturn = true;
			eBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			eBar.Location = new System.Drawing.Point(13, 44);
			eBar.Name = "eBar";
			eBar.Size = new System.Drawing.Size(276, 38);
			eBar.TabIndex = 0;
			eBar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(eBar_KeyPress);
			lP1.AutoSize = true;
			lP1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lP1.Location = new System.Drawing.Point(79, 6);
			lP1.Name = "lP1";
			lP1.Size = new System.Drawing.Size(142, 31);
			lP1.TabIndex = 14;
			lP1.Text = "Scan code";
			tScan.Interval = 500;
			tScan.Tick += new System.EventHandler(tScan_Tick);
			tG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			tG.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			tG.Location = new System.Drawing.Point(68, 253);
			tG.Name = "tG";
			tG.Size = new System.Drawing.Size(56, 56);
			tG.TabIndex = 17;
			tG.Text = "G";
			tG.UseVisualStyleBackColor = true;
			tG.Click += new System.EventHandler(tG_Click);
			tG.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			tF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			tF.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			tF.Location = new System.Drawing.Point(13, 253);
			tF.Name = "tF";
			tF.Size = new System.Drawing.Size(56, 56);
			tF.TabIndex = 16;
			tF.Text = "F";
			tF.UseVisualStyleBackColor = true;
			tF.Click += new System.EventHandler(tF_Click);
			tF.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			tE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			tE.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			tE.Location = new System.Drawing.Point(233, 198);
			tE.Name = "tE";
			tE.Size = new System.Drawing.Size(56, 56);
			tE.TabIndex = 15;
			tE.Text = "E";
			tE.UseVisualStyleBackColor = true;
			tE.Click += new System.EventHandler(tE_Click);
			tE.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			tD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			tD.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			tD.Location = new System.Drawing.Point(178, 198);
			tD.Name = "tD";
			tD.Size = new System.Drawing.Size(56, 56);
			tD.TabIndex = 14;
			tD.Text = "D";
			tD.UseVisualStyleBackColor = true;
			tD.Click += new System.EventHandler(tD_Click);
			tD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			tC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			tC.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			tC.Location = new System.Drawing.Point(123, 198);
			tC.Name = "tC";
			tC.Size = new System.Drawing.Size(56, 56);
			tC.TabIndex = 13;
			tC.Text = "C";
			tC.UseVisualStyleBackColor = true;
			tC.Click += new System.EventHandler(tC_Click);
			tC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			tB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			tB.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			tB.Location = new System.Drawing.Point(68, 198);
			tB.Name = "tB";
			tB.Size = new System.Drawing.Size(56, 56);
			tB.TabIndex = 12;
			tB.Text = "B";
			tB.UseVisualStyleBackColor = true;
			tB.Click += new System.EventHandler(tB_Click);
			tB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			tA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			tA.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			tA.Location = new System.Drawing.Point(13, 198);
			tA.Name = "tA";
			tA.Size = new System.Drawing.Size(56, 56);
			tA.TabIndex = 11;
			tA.Text = "A";
			tA.UseVisualStyleBackColor = true;
			tA.Click += new System.EventHandler(tA_Click);
			tA.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			t0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			t0.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			t0.Location = new System.Drawing.Point(233, 143);
			t0.Name = "t0";
			t0.Size = new System.Drawing.Size(56, 56);
			t0.TabIndex = 10;
			t0.Text = "0";
			t0.UseVisualStyleBackColor = true;
			t0.Click += new System.EventHandler(t0_Click);
			t0.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			t9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			t9.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			t9.Location = new System.Drawing.Point(178, 143);
			t9.Name = "t9";
			t9.Size = new System.Drawing.Size(56, 56);
			t9.TabIndex = 9;
			t9.Text = "9";
			t9.UseVisualStyleBackColor = true;
			t9.Click += new System.EventHandler(t9_Click);
			t9.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			t8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			t8.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			t8.Location = new System.Drawing.Point(123, 143);
			t8.Name = "t8";
			t8.Size = new System.Drawing.Size(56, 56);
			t8.TabIndex = 8;
			t8.Text = "8";
			t8.UseVisualStyleBackColor = true;
			t8.Click += new System.EventHandler(t8_Click);
			t8.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			t7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			t7.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			t7.Location = new System.Drawing.Point(68, 143);
			t7.Name = "t7";
			t7.Size = new System.Drawing.Size(56, 56);
			t7.TabIndex = 7;
			t7.Text = "7";
			t7.UseVisualStyleBackColor = true;
			t7.Click += new System.EventHandler(t7_Click);
			t7.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			t6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			t6.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			t6.Location = new System.Drawing.Point(13, 143);
			t6.Name = "t6";
			t6.Size = new System.Drawing.Size(56, 56);
			t6.TabIndex = 6;
			t6.Text = "6";
			t6.UseVisualStyleBackColor = true;
			t6.Click += new System.EventHandler(t6_Click);
			t6.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			t5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			t5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			t5.Location = new System.Drawing.Point(233, 88);
			t5.Name = "t5";
			t5.Size = new System.Drawing.Size(56, 56);
			t5.TabIndex = 5;
			t5.Text = "5";
			t5.UseVisualStyleBackColor = true;
			t5.Click += new System.EventHandler(t5_Click);
			t5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			t4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			t4.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			t4.Location = new System.Drawing.Point(178, 88);
			t4.Name = "t4";
			t4.Size = new System.Drawing.Size(56, 56);
			t4.TabIndex = 4;
			t4.Text = "4";
			t4.UseVisualStyleBackColor = true;
			t4.Click += new System.EventHandler(t4_Click);
			t4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			t3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			t3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			t3.Location = new System.Drawing.Point(123, 88);
			t3.Name = "t3";
			t3.Size = new System.Drawing.Size(56, 56);
			t3.TabIndex = 3;
			t3.Text = "3";
			t3.UseVisualStyleBackColor = true;
			t3.Click += new System.EventHandler(t3_Click);
			t3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			t2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			t2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			t2.Location = new System.Drawing.Point(68, 88);
			t2.Name = "t2";
			t2.Size = new System.Drawing.Size(56, 56);
			t2.TabIndex = 2;
			t2.Text = "2";
			t2.UseVisualStyleBackColor = true;
			t2.Click += new System.EventHandler(t2_Click);
			t2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			t1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			t1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			t1.Location = new System.Drawing.Point(13, 88);
			t1.Name = "t1";
			t1.Size = new System.Drawing.Size(56, 56);
			t1.TabIndex = 1;
			t1.Text = "1";
			t1.UseVisualStyleBackColor = true;
			t1.Click += new System.EventHandler(t1_Click);
			t1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			tX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			tX.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			tX.Image = Kiosk.Properties.Resources.backspace;
			tX.Location = new System.Drawing.Point(123, 253);
			tX.Name = "tX";
			tX.Size = new System.Drawing.Size(56, 56);
			tX.TabIndex = 18;
			tX.UseVisualStyleBackColor = true;
			tX.Click += new System.EventHandler(tX_Click);
			tX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			button1.Image = Kiosk.Properties.Resources.ico_barcodeX;
			button1.Location = new System.Drawing.Point(178, 253);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(56, 56);
			button1.TabIndex = 19;
			button1.UseVisualStyleBackColor = true;
			button1.Click += new System.EventHandler(bCancel_Click);
			button1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			bRestet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bRestet.Image = Kiosk.Properties.Resources.ico_barcode;
			bRestet.Location = new System.Drawing.Point(13, 315);
			bRestet.Name = "bRestet";
			bRestet.Size = new System.Drawing.Size(111, 48);
			bRestet.TabIndex = 20;
			bRestet.UseVisualStyleBackColor = true;
			bRestet.Click += new System.EventHandler(bRestet_Click);
			bRestet.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			bOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bOK.Image = Kiosk.Properties.Resources.ico_ok;
			bOK.Location = new System.Drawing.Point(178, 315);
			bOK.Name = "bOK";
			bOK.Size = new System.Drawing.Size(111, 48);
			bOK.TabIndex = 21;
			bOK.UseVisualStyleBackColor = true;
			bOK.Click += new System.EventHandler(bOK_Click);
			bOK.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(301, 374);
			base.ControlBox = false;
			base.Controls.Add(tX);
			base.Controls.Add(tG);
			base.Controls.Add(tF);
			base.Controls.Add(tE);
			base.Controls.Add(tD);
			base.Controls.Add(tC);
			base.Controls.Add(tB);
			base.Controls.Add(tA);
			base.Controls.Add(t0);
			base.Controls.Add(t9);
			base.Controls.Add(t8);
			base.Controls.Add(t7);
			base.Controls.Add(t6);
			base.Controls.Add(t5);
			base.Controls.Add(t4);
			base.Controls.Add(t3);
			base.Controls.Add(t2);
			base.Controls.Add(t1);
			base.Controls.Add(button1);
			base.Controls.Add(lP1);
			base.Controls.Add(bRestet);
			base.Controls.Add(bOK);
			base.Controls.Add(eBar);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "DLG_Barcode";
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "DLG_Barcode";
			base.TopMost = true;
			base.Load += new System.EventHandler(DLG_Barcode_Load);
			base.KeyPress += new System.Windows.Forms.KeyPressEventHandler(t1_KeyPress);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
