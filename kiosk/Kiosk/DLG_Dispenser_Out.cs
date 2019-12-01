using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
	public class DLG_Dispenser_Out : Form
	{
		public Configuracion opciones;

		public int TimeOutMsg;

		private DateTime Pausa;

		public bool IsClosed = false;

		private int oldT = -1;

		private int oldC = -1;

		private IContainer components = null;

		private Timer ctrl;

		private Label lINFO;

		public DLG_Dispenser_Out(ref Configuracion _opc)
		{
			IsClosed = false;
			TimeOutMsg = 0;
			opciones = _opc;
			InitializeComponent();
			Localize();
			base.Opacity = 0.7;
			bool flag = true;
		}

		private void Localize()
		{
			SuspendLayout();
			ResumeLayout();
		}

		private void Update_Info()
		{
			lINFO.Text = "••• LIVRAISON DES CHÉQUES CADEAUX •••\r\n\r\nPoints à livrer: " + opciones.Disp_Pay_Ticket_Credits + "\r\n\r\nChèques cadeaux pour livrer: " + opciones.Disp_Pay_Ticket + "\r\n\r\nChèques cadeaux livrés: " + opciones.Disp_Pay_Ticket_Out;
			oldT = opciones.Disp_Pay_Ticket;
			oldC = opciones.Disp_Pay_Ticket_Credits;
			Invalidate();
		}

		private void ctrl_Tick(object sender, EventArgs e)
		{
			if (opciones.Disp_Pay_Ticket_Fail == 0 && (oldT != opciones.Disp_Pay_Ticket || oldC != opciones.Disp_Pay_Ticket_Credits))
			{
				Update_Info();
			}
			if (opciones.Disp_Pay_Ticket_Fail == 1)
			{
				if (opciones.Disp_Pay_Ticket_Credits <= 0)
				{
					lINFO.Text = "••• LIVRAISON DES CHÉQUES CADEAUX •••\r\n\r\nChèques cadeaux livrés: " + opciones.Disp_Pay_Ticket_Out;
				}
				else
				{
					lINFO.Text = "••• LIVRAISON DES CHÉQUES CADEAUX •••\r\n\r\nPas de chèques cadeaux disponibles sur le borne\r\n\r\nUtilisez le bon pour obtenir les chèques cadeaux\r\n\r\nPour " + opciones.Disp_Pay_Ticket_Credits + " crédits\r\n\r\nChèques cadeaux livrés: " + opciones.Disp_Pay_Ticket_Out;
				}
				BackColor = Color.OrangeRed;
				Invalidate();
				opciones.Disp_Pay_Ticket_Fail = 2;
				Pausa = DateTime.Now;
			}
			if (opciones.Disp_Pay_Ticket_Fail == 2)
			{
				TimeOutMsg = 1;
			}
			if (opciones.Disp_Pay_Ticket_Fail == 3)
			{
				BackColor = Color.DarkGreen;
				if (opciones.Disp_Pay_Ticket_Out <= 0)
				{
					BackColor = Color.OrangeRed;
				}
				if (opciones.Disp_Pay_Ticket_Credits <= 0)
				{
					lINFO.Text = "••• LIVRAISON DES CHÉQUES CADEAUX •••\r\n\r\nChèques cadeaux livrés: " + opciones.Disp_Pay_Ticket_Out;
				}
				else
				{
					lINFO.Text = "••• LIVRAISON DES CHÉQUES CADEAUX •••\r\n\r\nPas de chèques cadeaux disponibles sur le borne\r\n\r\nUtilisez le bon pour obtenir les chèques cadeaux\r\n\r\nPour " + opciones.Disp_Pay_Ticket_Credits + " crédits\r\n\r\nChèques cadeaux livrés: " + opciones.Disp_Pay_Ticket_Out;
				}
				Invalidate();
				Pausa = DateTime.Now;
				TimeOutMsg = 1;
				opciones.Disp_Pay_Ticket_Fail = 2;
			}
			if (TimeOutMsg == 1)
			{
				int num = (int)(DateTime.Now - Pausa).TotalSeconds;
				if (num > 5)
				{
					IsClosed = true;
					Close();
				}
			}
		}

		private void DLG_Dispenser_Out_Load(object sender, EventArgs e)
		{
			Update_Info();
			ctrl.Enabled = true;
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
			ctrl = new System.Windows.Forms.Timer(components);
			lINFO = new System.Windows.Forms.Label();
			SuspendLayout();
			ctrl.Tick += new System.EventHandler(ctrl_Tick);
			lINFO.BackColor = System.Drawing.Color.Transparent;
			lINFO.Dock = System.Windows.Forms.DockStyle.Fill;
			lINFO.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lINFO.Location = new System.Drawing.Point(0, 0);
			lINFO.Name = "lINFO";
			lINFO.Size = new System.Drawing.Size(929, 546);
			lINFO.TabIndex = 2;
			lINFO.Text = "Dispensing tickets";
			lINFO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.Blue;
			base.ClientSize = new System.Drawing.Size(929, 546);
			base.Controls.Add(lINFO);
			DoubleBuffered = true;
			ForeColor = System.Drawing.Color.White;
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Margin = new System.Windows.Forms.Padding(2);
			base.Name = "DLG_Dispenser_Out";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "DLG_Dispenser_Out";
			base.TopMost = true;
			base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			base.Load += new System.EventHandler(DLG_Dispenser_Out_Load);
			ResumeLayout(false);
		}
	}
}
