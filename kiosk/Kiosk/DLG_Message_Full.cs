using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
	public class DLG_Message_Full : Form
	{
		public Configuracion opciones;

		public bool IsClosed;

		private DateTime Pausa;

		private IContainer components = null;

		private Label lBuy;

		private Button bOK;

		private Timer tScan;

		public DLG_Message_Full(string _msg, ref Configuracion _opc, bool _warn = false)
		{
			IsClosed = false;
			opciones = _opc;
			InitializeComponent();
			if (_warn)
			{
				BackColor = Color.Red;
				lBuy.ForeColor = Color.Yellow;
				base.Height *= 2;
				base.Width *= 2;
			}
			lBuy.Text = _msg;
			base.Opacity = 0.7;
			bool flag = true;
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			IsClosed = true;
			Close();
		}

		private void tScan_Tick(object sender, EventArgs e)
		{
			int num = (int)(DateTime.Now - Pausa).TotalSeconds;
			if (num > 5)
			{
				IsClosed = true;
				Close();
			}
		}

		private void lBuy_Click(object sender, EventArgs e)
		{
		}

		private void DLG_Message_Load(object sender, EventArgs e)
		{
			Pausa = DateTime.Now;
			tScan.Enabled = true;
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
			lBuy = new System.Windows.Forms.Label();
			bOK = new System.Windows.Forms.Button();
			tScan = new System.Windows.Forms.Timer(components);
			SuspendLayout();
			lBuy.Dock = System.Windows.Forms.DockStyle.Fill;
			lBuy.Font = new System.Drawing.Font("Microsoft Sans Serif", 16f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lBuy.Location = new System.Drawing.Point(0, 0);
			lBuy.Name = "lBuy";
			lBuy.Size = new System.Drawing.Size(418, 165);
			lBuy.TabIndex = 12;
			lBuy.Text = "-";
			lBuy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			lBuy.Click += new System.EventHandler(lBuy_Click);
			bOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			bOK.Dock = System.Windows.Forms.DockStyle.Bottom;
			bOK.Image = Kiosk.Properties.Resources.ico_ok;
			bOK.Location = new System.Drawing.Point(0, 165);
			bOK.Name = "bOK";
			bOK.Size = new System.Drawing.Size(418, 142);
			bOK.TabIndex = 0;
			bOK.UseVisualStyleBackColor = true;
			bOK.Click += new System.EventHandler(bOK_Click);
			tScan.Interval = 500;
			tScan.Tick += new System.EventHandler(tScan_Tick);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(418, 307);
			base.ControlBox = false;
			base.Controls.Add(lBuy);
			base.Controls.Add(bOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "DLG_Message_Full";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			base.TopMost = true;
			base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			base.Load += new System.EventHandler(DLG_Message_Load);
			ResumeLayout(false);
		}
	}
}
