using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
	public class GETTON_Message : Form
	{
		public Configuracion opciones;

		private IContainer components = null;

		private Label lBuy;

		private Button bOK;

		public GETTON_Message(string _msg, ref Configuracion _opc)
		{
			opciones = _opc;
			InitializeComponent();
			base.Top = 0;
			base.Left = 300;
			lBuy.Text = _msg;
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			opciones.Add_Getton = 1;
		}

		private void DLG_Message_Load(object sender, EventArgs e)
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
			lBuy = new System.Windows.Forms.Label();
			bOK = new System.Windows.Forms.Button();
			SuspendLayout();
			lBuy.Dock = System.Windows.Forms.DockStyle.Fill;
			lBuy.Font = new System.Drawing.Font("Microsoft Sans Serif", 16f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lBuy.Location = new System.Drawing.Point(0, 0);
			lBuy.Name = "lBuy";
			lBuy.Size = new System.Drawing.Size(424, 48);
			lBuy.TabIndex = 12;
			lBuy.Text = "-";
			lBuy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			bOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			bOK.Dock = System.Windows.Forms.DockStyle.Right;
			bOK.Image = Kiosk.Properties.Resources.ico_ok;
			bOK.Location = new System.Drawing.Point(424, 0);
			bOK.Name = "bOK";
			bOK.Size = new System.Drawing.Size(74, 48);
			bOK.TabIndex = 0;
			bOK.UseVisualStyleBackColor = true;
			bOK.Click += new System.EventHandler(bOK_Click);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(498, 48);
			base.ControlBox = false;
			base.Controls.Add(lBuy);
			base.Controls.Add(bOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "GETTON_Message";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			base.TopMost = true;
			base.Load += new System.EventHandler(DLG_Message_Load);
			ResumeLayout(false);
		}
	}
}
