using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
	public class CreditManager : Form
	{
		public Configuracion Opcions;

		private IContainer components = null;

		private Button bAddTime;

		private Button bOK;

		public CreditManager(ref Configuracion _opc)
		{
			Opcions = _opc;
			InitializeComponent();
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			Opcions.ComprarTemps = 0;
			Close();
		}

		private void bAddTime_Click(object sender, EventArgs e)
		{
			if (!(Opcions.Sub_Credits > 0m))
			{
				int num = 0;
				if (Opcions.Credits >= (decimal)Opcions.ValorTemps)
				{
					num = Opcions.ValorTemps;
				}
				if (num > 0)
				{
					Opcions.Temps += 60;
					Opcions.Sub_Credits = num;
				}
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
			bAddTime = new System.Windows.Forms.Button();
			bOK = new System.Windows.Forms.Button();
			SuspendLayout();
			bAddTime.Image = Kiosk.Properties.Resources.ico_add;
			bAddTime.Location = new System.Drawing.Point(-1, 0);
			bAddTime.Name = "bAddTime";
			bAddTime.Size = new System.Drawing.Size(48, 48);
			bAddTime.TabIndex = 1;
			bAddTime.UseVisualStyleBackColor = true;
			bAddTime.Click += new System.EventHandler(bAddTime_Click);
			bOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			bOK.Image = Kiosk.Properties.Resources.ico_ok;
			bOK.Location = new System.Drawing.Point(47, 0);
			bOK.Name = "bOK";
			bOK.Size = new System.Drawing.Size(48, 48);
			bOK.TabIndex = 0;
			bOK.UseVisualStyleBackColor = true;
			bOK.Click += new System.EventHandler(bOK_Click);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(95, 48);
			base.ControlBox = false;
			base.Controls.Add(bOK);
			base.Controls.Add(bAddTime);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "CreditManager";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			base.TopMost = true;
			ResumeLayout(false);
		}
	}
}
