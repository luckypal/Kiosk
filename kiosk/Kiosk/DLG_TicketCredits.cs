using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
	public class DLG_TicketCredits : Form
	{
		public bool OK;

		public Configuracion opciones;

		public MainWindow MWin;

		private string eBar;

		private int check = 0;

		private IContainer components = null;

		private Label lTicket;

		private Label lTime;

		private Label lVal;

		private Label lCODE;

		private Timer tAUTO;

		public string Ticket
		{
			get
			{
				return eBar;
			}
			set
			{
				lTicket.Text = value;
				eBar = value;
				Invalidate();
			}
		}

		public DLG_TicketCredits(ref Configuracion _opc)
		{
			OK = false;
			opciones = _opc;
			MWin = null;
			InitializeComponent();
			Localize();
			lCODE.Text = "-";
			lVal.Text = "-";
			lTime.Text = "-";
			check = 0;
			BackColor = Color.Gray;
		}

		private void Localize()
		{
			SuspendLayout();
			ResumeLayout();
		}

		public void Update_Info()
		{
			lCODE.Text = opciones.Ticket_Verificar.CRC;
			lVal.Text = string.Format("{0}.{1:00} {2}", opciones.Ticket_Verificar.Pago / 100, opciones.Ticket_Verificar.Pago % 100, opciones.Localize.Text("Euros"));
			lTime.Text = string.Format("{2:00}:{3:00} {0}/{1:00}", opciones.Ticket_Verificar.DataT.Day, opciones.Ticket_Verificar.DataT.Month, opciones.Ticket_Verificar.DataT.Hour, opciones.Ticket_Verificar.DataT.Minute);
			switch (opciones.Ticket_Verificar.Verificado)
			{
			case 0:
				BackColor = Color.Green;
				break;
			case 1:
				lVal.Text = "X";
				BackColor = Color.Red;
				break;
			case 2:
				lVal.Text = "INVALID";
				BackColor = Color.Red;
				break;
			default:
				lVal.Text = "INVALID";
				BackColor = Color.Red;
				break;
			}
			if (check == 0)
			{
				tAUTO.Enabled = true;
			}
			Invalidate();
		}

		public void GoExit()
		{
			MWin.TicketToCheck = "";
			MWin.TicketOK = 0;
			Close();
		}

		private void tAUTO_Tick(object sender, EventArgs e)
		{
			check = 1;
			tAUTO.Enabled = false;
			if (opciones.Ticket_Verificar.Verificado == 0)
			{
				string s = lTicket.Text.Substring(1, 11);
				int tck = int.Parse(s);
				MWin.Srv_Anular_Ticket(tck, _est: true, 0);
				MWin.Internal_Add_Credits(opciones.Ticket_Verificar.Pago);
			}
			Close();
		}

		private void DLG_TicketCredits_Load(object sender, EventArgs e)
		{
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
			lTicket = new System.Windows.Forms.Label();
			lTime = new System.Windows.Forms.Label();
			lVal = new System.Windows.Forms.Label();
			lCODE = new System.Windows.Forms.Label();
			tAUTO = new System.Windows.Forms.Timer(components);
			SuspendLayout();
			lTicket.Dock = System.Windows.Forms.DockStyle.Top;
			lTicket.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lTicket.Location = new System.Drawing.Point(0, 124);
			lTicket.Name = "lTicket";
			lTicket.Size = new System.Drawing.Size(287, 32);
			lTicket.TabIndex = 31;
			lTicket.Text = "-";
			lTicket.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			lTime.Dock = System.Windows.Forms.DockStyle.Top;
			lTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lTime.Location = new System.Drawing.Point(0, 92);
			lTime.Name = "lTime";
			lTime.Size = new System.Drawing.Size(287, 32);
			lTime.TabIndex = 32;
			lTime.Text = "-";
			lTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			lVal.Dock = System.Windows.Forms.DockStyle.Top;
			lVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lVal.Location = new System.Drawing.Point(0, 46);
			lVal.Name = "lVal";
			lVal.Size = new System.Drawing.Size(287, 46);
			lVal.TabIndex = 33;
			lVal.Text = "-";
			lVal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			lCODE.Dock = System.Windows.Forms.DockStyle.Top;
			lCODE.Font = new System.Drawing.Font("Microsoft Sans Serif", 32f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lCODE.Location = new System.Drawing.Point(0, 0);
			lCODE.Name = "lCODE";
			lCODE.Size = new System.Drawing.Size(287, 46);
			lCODE.TabIndex = 29;
			lCODE.Text = "-";
			lCODE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			tAUTO.Interval = 2000;
			tAUTO.Tick += new System.EventHandler(tAUTO_Tick);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(287, 198);
			base.Controls.Add(lTicket);
			base.Controls.Add(lTime);
			base.Controls.Add(lVal);
			base.Controls.Add(lCODE);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "DLG_TicketCredits";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "DLG_TicketCredits";
			base.TopMost = true;
			base.Load += new System.EventHandler(DLG_TicketCredits_Load);
			ResumeLayout(false);
		}
	}
}
