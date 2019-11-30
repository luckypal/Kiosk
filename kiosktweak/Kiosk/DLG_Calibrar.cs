using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Kiosk
{
	public class DLG_Calibrar : Form
	{
		public bool OK;

		private IContainer components;

		private Button bOK;

		private Button bGALAX;

		private Button b3M;

		private Button eELO;

		private Button bETWO;

		private Button bGEN;

		private Button bTALENT;

		private Button bASUS;

		private ToolTip toolTip1;

		public DLG_Calibrar()
		{
			OK = false;
			InitializeComponent();
			Localize();
			string path = "c:\\Windows\\TouchUSM\\TouchCali.exe";
			if (!File.Exists(path))
			{
				bTALENT.Enabled = false;
			}
			path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Elo TouchSystems\\EloVa.exe";
			if (!File.Exists(path))
			{
				eELO.Enabled = false;
			}
			path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\ETSerTouch\\etsercalib.exe";
			if (!File.Exists(path))
			{
				bETWO.Enabled = false;
			}
			path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\MicroTouch\\MT 7\\TwCalib.exe";
			if (!File.Exists(path))
			{
				b3M.Enabled = false;
			}
			path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\eGalaxTouch\\xAuto4PtsCal.exe";
			if (!File.Exists(path))
			{
				bGALAX.Enabled = false;
			}
			path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\GeneralTouch\\APP\\x86\\GenCalib.exe";
			if (!File.Exists(path))
			{
				bGEN.Enabled = false;
			}
			path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Touch Package\\Touchpack.exe";
			if (!File.Exists(path))
			{
				bASUS.Enabled = false;
			}
		}

		private void Localize()
		{
			SuspendLayout();
			ResumeLayout();
		}

		private void eELO_Click(object sender, EventArgs e)
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Elo TouchSystems\\EloVa.exe";
			if (File.Exists(text))
			{
				Process.Start(text);
			}
			else
			{
				MessageBox.Show("Missing: [" + text + "]");
			}
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void bETWO_Click(object sender, EventArgs e)
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\ETSerTouch\\etsercalib.exe";
			if (File.Exists(text))
			{
				Process.Start(text);
			}
			else
			{
				MessageBox.Show("Missing: [" + text + "]");
			}
		}

		private void b3M_Click(object sender, EventArgs e)
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\MicroTouch\\MT 7\\TwCalib.exe";
			if (File.Exists(text))
			{
				Process.Start(text);
			}
			else
			{
				MessageBox.Show("Missing: [" + text + "]");
			}
		}

		private void bGALAX_Click(object sender, EventArgs e)
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\eGalaxTouch\\xAuto4PtsCal.exe";
			if (File.Exists(text))
			{
				Process.Start(text);
			}
			else
			{
				MessageBox.Show("Missing: [" + text + "]");
			}
		}

		private void bGEN_Click(object sender, EventArgs e)
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\GeneralTouch\\APP\\x86\\GenCalib.exe";
			if (File.Exists(text))
			{
				Process.Start(text);
			}
			else
			{
				MessageBox.Show("Missing: [" + text + "]");
			}
		}

		private void bTALENT_Click(object sender, EventArgs e)
		{
			string text = "c:\\Windows\\TouchUSM\\TouchCali.exe";
			if (File.Exists(text))
			{
				Process.Start(text);
			}
			else
			{
				MessageBox.Show("Missing: [" + text + "]");
			}
		}

		private void bASUS_Click(object sender, EventArgs e)
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Touch Package\\Touchpack.exe";
			if (File.Exists(text))
			{
				Process.Start(text);
			}
			else
			{
				MessageBox.Show("Missing: [" + text + "]");
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
			bOK = new System.Windows.Forms.Button();
			bASUS = new System.Windows.Forms.Button();
			bTALENT = new System.Windows.Forms.Button();
			bGEN = new System.Windows.Forms.Button();
			bETWO = new System.Windows.Forms.Button();
			eELO = new System.Windows.Forms.Button();
			b3M = new System.Windows.Forms.Button();
			bGALAX = new System.Windows.Forms.Button();
			toolTip1 = new System.Windows.Forms.ToolTip(components);
			SuspendLayout();
			bOK.Image = Kiosk.Properties.Resources.ico_ok;
			bOK.Location = new System.Drawing.Point(395, 12);
			bOK.Name = "bOK";
			bOK.Size = new System.Drawing.Size(48, 48);
			bOK.TabIndex = 5;
			bOK.UseVisualStyleBackColor = true;
			bOK.Click += new System.EventHandler(bOK_Click);
			bASUS.Image = Kiosk.Properties.Resources.ico_asus;
			bASUS.Location = new System.Drawing.Point(336, 12);
			bASUS.Name = "bASUS";
			bASUS.Size = new System.Drawing.Size(48, 48);
			bASUS.TabIndex = 7;
			toolTip1.SetToolTip(bASUS, "Calibrate ASUS eePC");
			bASUS.UseVisualStyleBackColor = true;
			bASUS.Click += new System.EventHandler(bASUS_Click);
			bTALENT.Image = Kiosk.Properties.Resources.ico_ttouch;
			bTALENT.Location = new System.Drawing.Point(282, 12);
			bTALENT.Name = "bTALENT";
			bTALENT.Size = new System.Drawing.Size(48, 48);
			bTALENT.TabIndex = 6;
			toolTip1.SetToolTip(bTALENT, "Calibrate Talent Touch");
			bTALENT.UseVisualStyleBackColor = true;
			bTALENT.Click += new System.EventHandler(bTALENT_Click);
			bGEN.Image = Kiosk.Properties.Resources.ico_general;
			bGEN.Location = new System.Drawing.Point(228, 12);
			bGEN.Name = "bGEN";
			bGEN.Size = new System.Drawing.Size(48, 48);
			bGEN.TabIndex = 4;
			toolTip1.SetToolTip(bGEN, "Calibrate General Touch");
			bGEN.UseVisualStyleBackColor = true;
			bGEN.Click += new System.EventHandler(bGEN_Click);
			bETWO.Image = Kiosk.Properties.Resources.ico_etwo;
			bETWO.Location = new System.Drawing.Point(120, 12);
			bETWO.Name = "bETWO";
			bETWO.Size = new System.Drawing.Size(48, 48);
			bETWO.TabIndex = 2;
			toolTip1.SetToolTip(bETWO, "Calibrate Etwo Touch");
			bETWO.UseVisualStyleBackColor = true;
			bETWO.Click += new System.EventHandler(bETWO_Click);
			eELO.Image = Kiosk.Properties.Resources.ico_elo;
			eELO.Location = new System.Drawing.Point(12, 12);
			eELO.Name = "eELO";
			eELO.Size = new System.Drawing.Size(48, 48);
			eELO.TabIndex = 0;
			toolTip1.SetToolTip(eELO, "Calibrate Elo Touch");
			eELO.UseVisualStyleBackColor = true;
			eELO.Click += new System.EventHandler(eELO_Click);
			b3M.Image = Kiosk.Properties.Resources.ico_3m;
			b3M.Location = new System.Drawing.Point(66, 12);
			b3M.Name = "b3M";
			b3M.Size = new System.Drawing.Size(48, 48);
			b3M.TabIndex = 1;
			toolTip1.SetToolTip(b3M, "Calibrate MicroTouch");
			b3M.UseVisualStyleBackColor = true;
			b3M.Click += new System.EventHandler(b3M_Click);
			bGALAX.Image = Kiosk.Properties.Resources.ico_galax;
			bGALAX.Location = new System.Drawing.Point(174, 12);
			bGALAX.Name = "bGALAX";
			bGALAX.Size = new System.Drawing.Size(48, 48);
			bGALAX.TabIndex = 3;
			toolTip1.SetToolTip(bGALAX, "Calibrate Galaxy Touch");
			bGALAX.UseVisualStyleBackColor = true;
			bGALAX.Click += new System.EventHandler(bGALAX_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(455, 77);
			base.Controls.Add(bASUS);
			base.Controls.Add(bTALENT);
			base.Controls.Add(bGEN);
			base.Controls.Add(bETWO);
			base.Controls.Add(eELO);
			base.Controls.Add(b3M);
			base.Controls.Add(bGALAX);
			base.Controls.Add(bOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "DLG_Calibrar";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Select Touch Screen";
			ResumeLayout(false);
		}
	}
}
