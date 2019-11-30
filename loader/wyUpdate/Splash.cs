using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using wyUpdate.Properties;

namespace wyUpdate
{
	public class Splash : Form
	{
		private int _anmWorking;

		private int _anmWorking_Max = 1;

		public string wifi;

		public int GoConfig;

		private IContainer components;

		private Timer timerANM;

		public Splash()
		{
			GoConfig = 0;
			_anmWorking = 0;
			_anmWorking_Max = Resources.list_working.Width / 16;
			wifi = "";
			InitializeComponent();
			timerANM.Enabled = true;
		}

		private void timerANM_Tick(object sender, EventArgs e)
		{
			_anmWorking++;
			if (_anmWorking >= _anmWorking_Max)
			{
				_anmWorking = 0;
			}
			Invalidate();
		}

		private void Splash_Paint(object sender, PaintEventArgs e)
		{
			string text = "1.50";
			e.Graphics.DrawImage(Resources.list_working, new Rectangle(base.Width / 2 - 8, base.Height - 28, 16, 16), new Rectangle(_anmWorking * 16, 0, 16, 16), GraphicsUnit.Pixel);
			Font font = new Font("Arial", 8f, FontStyle.Bold);
			SizeF sizeF = e.Graphics.MeasureString(text, font);
			e.Graphics.DrawString(text, font, Brushes.LightGray, (float)base.Width - sizeF.Width, (float)base.Height - sizeF.Height);
			e.Graphics.DrawString(wifi, font, Brushes.LightGray, 10f, (float)base.Height - sizeF.Height);
		}

		private void Splash_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.VolumeDown)
			{
				GoConfig = 1;
				Invalidate();
			}
		}

		private void Splash_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.VolumeDown)
			{
				GoConfig = 1;
				Invalidate();
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
			timerANM = new System.Windows.Forms.Timer(components);
			SuspendLayout();
			timerANM.Tick += new System.EventHandler(timerANM_Tick);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackgroundImage = wyUpdate.Properties.Resources.kiosk2;
			base.ClientSize = new System.Drawing.Size(206, 109);
			DoubleBuffered = true;
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "Splash";
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Splash";
			base.Paint += new System.Windows.Forms.PaintEventHandler(Splash_Paint);
			base.KeyDown += new System.Windows.Forms.KeyEventHandler(Splash_KeyDown);
			base.KeyUp += new System.Windows.Forms.KeyEventHandler(Splash_KeyUp);
			ResumeLayout(false);
		}
	}
}
