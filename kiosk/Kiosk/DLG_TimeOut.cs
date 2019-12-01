using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
	public class DLG_TimeOut : Form
	{
		public bool OK;

		public Configuracion opciones;

		private int Contdown;

		private DateTime StartsShow;

		private IContainer components = null;

		private Button bCancel;

		private Button bOK;

		private Label lP1;

		private PictureBox pictureBox1;

		private Timer tScan;

		private Label lCONT;

		public DLG_TimeOut(ref Configuracion _opc, string _msg)
		{
			OK = false;
			Contdown = 10;
			opciones = _opc;
			InitializeComponent();
			lP1.Text = _msg;
			Localize();
		}

		private void Localize()
		{
			SuspendLayout();
			ResumeLayout();
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			OK = true;
			Close();
		}

		private void bCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void DLG_Abonar_Ticket_Load(object sender, EventArgs e)
		{
			StartsShow = DateTime.Now;
			tScan.Enabled = true;
			Contdown = 10;
			lCONT.Text = string.Concat(Contdown);
		}

		private void tScan_Tick(object sender, EventArgs e)
		{
			int num = (int)(DateTime.Now - StartsShow).TotalSeconds;
			int num2 = 10 - num;
			if (num2 < 0)
			{
				num2 = 0;
			}
			if (num2 != Contdown)
			{
				Contdown = num2;
				lCONT.Text = string.Concat(Contdown);
			}
			if (num2 <= 0)
			{
				OK = true;
				Close();
			}
		}

		private void DLG_TimeOut_FormClosed(object sender, FormClosedEventArgs e)
		{
			opciones.LastMouseMove = DateTime.Now;
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
			pictureBox1 = new System.Windows.Forms.PictureBox();
			bCancel = new System.Windows.Forms.Button();
			bOK = new System.Windows.Forms.Button();
			tScan = new System.Windows.Forms.Timer(components);
			lCONT = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			SuspendLayout();
			lP1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			lP1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lP1.Location = new System.Drawing.Point(12, 106);
			lP1.Name = "lP1";
			lP1.Size = new System.Drawing.Size(514, 72);
			lP1.TabIndex = 14;
			lP1.Text = "Ticket";
			lP1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			pictureBox1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			pictureBox1.BackgroundImage = Kiosk.Properties.Resources.big_warnning;
			pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			pictureBox1.Location = new System.Drawing.Point(218, 12);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new System.Drawing.Size(103, 91);
			pictureBox1.TabIndex = 15;
			pictureBox1.TabStop = false;
			bCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			bCancel.Image = Kiosk.Properties.Resources.ico_del;
			bCancel.Location = new System.Drawing.Point(12, 241);
			bCancel.Name = "bCancel";
			bCancel.Size = new System.Drawing.Size(64, 64);
			bCancel.TabIndex = 2;
			bCancel.UseVisualStyleBackColor = true;
			bCancel.Click += new System.EventHandler(bCancel_Click);
			bOK.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			bOK.Image = Kiosk.Properties.Resources.ico_ok;
			bOK.Location = new System.Drawing.Point(462, 241);
			bOK.Name = "bOK";
			bOK.Size = new System.Drawing.Size(64, 64);
			bOK.TabIndex = 1;
			bOK.UseVisualStyleBackColor = true;
			bOK.Click += new System.EventHandler(bOK_Click);
			tScan.Interval = 500;
			tScan.Tick += new System.EventHandler(tScan_Tick);
			lCONT.Font = new System.Drawing.Font("Microsoft Sans Serif", 30f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lCONT.ForeColor = System.Drawing.Color.Yellow;
			lCONT.Location = new System.Drawing.Point(18, 182);
			lCONT.Name = "lCONT";
			lCONT.Size = new System.Drawing.Size(508, 47);
			lCONT.TabIndex = 16;
			lCONT.Text = "10";
			lCONT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			BackColor = System.Drawing.Color.Red;
			base.ClientSize = new System.Drawing.Size(538, 316);
			base.ControlBox = false;
			base.Controls.Add(lCONT);
			base.Controls.Add(pictureBox1);
			base.Controls.Add(lP1);
			base.Controls.Add(bCancel);
			base.Controls.Add(bOK);
			DoubleBuffered = true;
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "DLG_TimeOut";
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "DLG_Barcode";
			base.TopMost = true;
			base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(DLG_TimeOut_FormClosed);
			base.Load += new System.EventHandler(DLG_Abonar_Ticket_Load);
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			ResumeLayout(false);
		}
	}
}
