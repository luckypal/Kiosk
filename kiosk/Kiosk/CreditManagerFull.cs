using Kiosk.Properties;
using LCDLabel;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
	public class CreditManagerFull : Form
	{
		private decimal ucrd;

		public Configuracion opciones;

		private IContainer components = null;

		private Button bOK;

		private Button bAddTime;

		private LcdLabel lcdClock;

		private Label lBuy;

		private Label lCredits;

		private Timer timerCredits;

		public CreditManagerFull(ref Configuracion _opc)
		{
			opciones = _opc;
			InitializeComponent();
			string text = $"{opciones.Temps / 60:00}:{opciones.Temps % 60:00}";
			lcdClock.Text = text;
			lCredits.Text = string.Format("{0}: {1}", opciones.Localize.Text("Credits"), opciones.Credits);
			ucrd = opciones.Credits;
			Localize();
		}

		private void Localize()
		{
			SuspendLayout();
			lBuy.Text = opciones.Localize.Text("Buy time");
			lCredits.Text = opciones.Localize.Text("Credits:");
			ResumeLayout();
		}

		private void bExit_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			opciones.ComprarTemps = 0;
			Close();
		}

		private void bAddTime_Click(object sender, EventArgs e)
		{
			if (!(opciones.Sub_Credits > 0m))
			{
				int num = 0;
				if (opciones.Credits >= (decimal)opciones.ValorTemps)
				{
					num = opciones.ValorTemps;
				}
				if (num > 0)
				{
					opciones.Temps += 60;
					opciones.Sub_Credits = num;
				}
				string text = $"{opciones.Temps / 60:00}:{opciones.Temps % 60:00}";
				lcdClock.Text = text;
			}
		}

		private void timerCredits_Tick(object sender, EventArgs e)
		{
			if (ucrd != opciones.Credits)
			{
				lCredits.Text = string.Format("{0}: {1}", opciones.Localize.Text("Credits"), opciones.Credits);
			}
			ucrd = opciones.Credits;
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
			bOK = new System.Windows.Forms.Button();
			bAddTime = new System.Windows.Forms.Button();
			lBuy = new System.Windows.Forms.Label();
			lCredits = new System.Windows.Forms.Label();
			lcdClock = new LCDLabel.LcdLabel();
			timerCredits = new System.Windows.Forms.Timer(components);
			SuspendLayout();
			bOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			bOK.Image = Kiosk.Properties.Resources.ico_ok;
			bOK.Location = new System.Drawing.Point(146, 108);
			bOK.Name = "bOK";
			bOK.Size = new System.Drawing.Size(48, 48);
			bOK.TabIndex = 0;
			bOK.UseVisualStyleBackColor = true;
			bOK.Click += new System.EventHandler(bOK_Click);
			bAddTime.Image = Kiosk.Properties.Resources.ico_add;
			bAddTime.Location = new System.Drawing.Point(90, 108);
			bAddTime.Name = "bAddTime";
			bAddTime.Size = new System.Drawing.Size(48, 48);
			bAddTime.TabIndex = 1;
			bAddTime.UseVisualStyleBackColor = true;
			bAddTime.Click += new System.EventHandler(bAddTime_Click);
			lBuy.Dock = System.Windows.Forms.DockStyle.Top;
			lBuy.Font = new System.Drawing.Font("Microsoft Sans Serif", 16f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lBuy.Location = new System.Drawing.Point(0, 0);
			lBuy.Name = "lBuy";
			lBuy.Size = new System.Drawing.Size(284, 36);
			lBuy.TabIndex = 10;
			lBuy.Text = "Buy Time";
			lBuy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			lCredits.Font = new System.Drawing.Font("Microsoft Sans Serif", 16f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lCredits.Location = new System.Drawing.Point(0, 64);
			lCredits.Name = "lCredits";
			lCredits.Size = new System.Drawing.Size(284, 36);
			lCredits.TabIndex = 12;
			lCredits.Text = "Credits: 0";
			lCredits.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			lcdClock.BackGround = System.Drawing.SystemColors.Control;
			lcdClock.BorderColor = System.Drawing.Color.Black;
			lcdClock.BorderSpace = 3;
			lcdClock.CharSpacing = 2;
			lcdClock.DotMatrix = LCDLabel.DotMatrix.mat5x7;
			lcdClock.LineSpacing = 2;
			lcdClock.Location = new System.Drawing.Point(99, 36);
			lcdClock.Name = "lcdClock";
			lcdClock.NumberOfCharacters = 5;
			lcdClock.PixelHeight = 2;
			lcdClock.PixelOff = System.Drawing.Color.FromArgb(0, 170, 170, 170);
			lcdClock.PixelOn = System.Drawing.Color.Black;
			lcdClock.PixelShape = LCDLabel.PixelShape.Square;
			lcdClock.PixelSize = LCDLabel.PixelSize.pix2x2;
			lcdClock.PixelSpacing = 1;
			lcdClock.PixelWidth = 2;
			lcdClock.Size = new System.Drawing.Size(86, 28);
			lcdClock.TabIndex = 9;
			lcdClock.Text = "00:00";
			lcdClock.TextLines = 1;
			timerCredits.Enabled = true;
			timerCredits.Tick += new System.EventHandler(timerCredits_Tick);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(284, 174);
			base.ControlBox = false;
			base.Controls.Add(lCredits);
			base.Controls.Add(lBuy);
			base.Controls.Add(lcdClock);
			base.Controls.Add(bOK);
			base.Controls.Add(bAddTime);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "CreditManagerFull";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			base.TopMost = true;
			ResumeLayout(false);
		}
	}
}
