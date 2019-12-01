using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
	public class MSG_Ok : Form
	{
		public bool IsClosed;

		public Configuracion opciones;

		public bool OK;

		public string Missatge;

		private IContainer components = null;

		private Label lMSG;

		private Panel pBOTTOM;

		private Button bOk;

		public MSG_Ok(ref Configuracion _opc, string _msg)
		{
			IsClosed = false;
			OK = false;
			opciones = _opc;
			Missatge = _msg;
			InitializeComponent();
			lMSG.Text = Missatge;
			Localize();
		}

		private void Localize()
		{
			SuspendLayout();
			ResumeLayout();
		}

		private void bOk_Click(object sender, EventArgs e)
		{
			OK = true;
			Close();
		}

		private void MSG_Ok_FormClosed(object sender, FormClosedEventArgs e)
		{
			IsClosed = true;
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
			lMSG = new System.Windows.Forms.Label();
			pBOTTOM = new System.Windows.Forms.Panel();
			bOk = new System.Windows.Forms.Button();
			pBOTTOM.SuspendLayout();
			SuspendLayout();
			lMSG.Dock = System.Windows.Forms.DockStyle.Fill;
			lMSG.Font = new System.Drawing.Font("Microsoft Sans Serif", 18f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lMSG.Location = new System.Drawing.Point(0, 0);
			lMSG.Name = "lMSG";
			lMSG.Size = new System.Drawing.Size(384, 163);
			lMSG.TabIndex = 7;
			lMSG.Text = "-";
			lMSG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			pBOTTOM.Controls.Add(bOk);
			pBOTTOM.Dock = System.Windows.Forms.DockStyle.Bottom;
			pBOTTOM.Location = new System.Drawing.Point(0, 163);
			pBOTTOM.Name = "pBOTTOM";
			pBOTTOM.Size = new System.Drawing.Size(384, 48);
			pBOTTOM.TabIndex = 6;
			bOk.Dock = System.Windows.Forms.DockStyle.Right;
			bOk.Image = Kiosk.Properties.Resources.ico_ok;
			bOk.Location = new System.Drawing.Point(336, 0);
			bOk.Name = "bOk";
			bOk.Size = new System.Drawing.Size(48, 48);
			bOk.TabIndex = 0;
			bOk.UseVisualStyleBackColor = true;
			bOk.Click += new System.EventHandler(bOk_Click);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			BackColor = System.Drawing.Color.White;
			base.ClientSize = new System.Drawing.Size(384, 211);
			base.ControlBox = false;
			base.Controls.Add(lMSG);
			base.Controls.Add(pBOTTOM);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "MSG_Ok";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = " ";
			base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(MSG_Ok_FormClosed);
			pBOTTOM.ResumeLayout(false);
			ResumeLayout(false);
		}
	}
}
