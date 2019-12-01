using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
	public class InsertCredits : Form
	{
		public Configuracion opciones;

		public bool OK;

		public int Modo;

		private IContainer components = null;

		private Button bOK;

		private Label lInfo;

		private Timer timerAutoClose;

		public InsertCredits(ref Configuracion _opc, int _modo)
		{
			OK = false;
			InitializeComponent();
			opciones = _opc;
			Modo = _modo;
			Localize();
		}

		private void Localize()
		{
			SuspendLayout();
			lInfo.Text = ((Modo == 0) ? opciones.Localize.Text("Insert credits") : opciones.Localize.Text("Ticket used or invalid"));
			ResumeLayout();
		}

		private void timerAutoClose_Tick(object sender, EventArgs e)
		{
			timerAutoClose.Enabled = false;
			Close();
		}

		private void bOK_Click(object sender, EventArgs e)
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
			components = new System.ComponentModel.Container();
			lInfo = new System.Windows.Forms.Label();
			timerAutoClose = new System.Windows.Forms.Timer(components);
			bOK = new System.Windows.Forms.Button();
			SuspendLayout();
			lInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lInfo.Location = new System.Drawing.Point(12, 15);
			lInfo.Name = "lInfo";
			lInfo.Size = new System.Drawing.Size(260, 39);
			lInfo.TabIndex = 2;
			lInfo.Text = "Insert credits";
			lInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			timerAutoClose.Enabled = true;
			timerAutoClose.Interval = 5000;
			timerAutoClose.Tick += new System.EventHandler(timerAutoClose_Tick);
			bOK.Image = Kiosk.Properties.Resources.ico_ok;
			bOK.Location = new System.Drawing.Point(105, 67);
			bOK.Name = "bOK";
			bOK.Size = new System.Drawing.Size(75, 41);
			bOK.TabIndex = 0;
			bOK.UseVisualStyleBackColor = true;
			bOK.Click += new System.EventHandler(bOK_Click);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(284, 120);
			base.ControlBox = false;
			base.Controls.Add(lInfo);
			base.Controls.Add(bOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "InsertCredits";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			base.TopMost = true;
			ResumeLayout(false);
		}
	}
}
