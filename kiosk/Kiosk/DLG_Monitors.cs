using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Kiosk
{
	public class DLG_Monitors : Form
	{
		public bool OK;

		public Configuracion opciones;

		public MainWindow MWin;

		private SplashMonitor Mon1;

		private SplashMonitor Mon2;

		private IContainer components = null;

		private Button bCancel;

		private Button bOk;

		private Label lPubli;

		private TextBox ePubli;

		private Button bCheck;

		private Button bClone;

		private Button bMon12;

		private Button bMon21;

		private CheckBox cMon;

		private Label lMon1;

		private Label lMon2;

		public DLG_Monitors(ref Configuracion _opc)
		{
			OK = false;
			opciones = _opc;
			MWin = null;
			InitializeComponent();
			cMon.Checked = ((opciones.Monitors > 1) ? true : false);
			ePubli.Text = opciones.Publi;
			Localize();
		}

		private void Localize()
		{
			SuspendLayout();
			ResumeLayout();
		}

		public void Update_Info()
		{
			Screen[] allScreens = Screen.AllScreens;
			lMon1.Text = "Monitor 1: " + Screen.PrimaryScreen.Bounds.ToString();
			if (allScreens.Length > 1)
			{
				if (allScreens[1].Bounds.X == Screen.PrimaryScreen.Bounds.X)
				{
					lMon2.Text = "Monitor 2: " + allScreens[0].Bounds.ToString();
				}
				else
				{
					lMon2.Text = "Monitor 2: " + allScreens[1].Bounds.ToString();
				}
			}
			else
			{
				lMon2.Text = "Monitor 2: -";
			}
		}

		public void ID_Monitors_Clear()
		{
			if (Mon1 != null)
			{
				Mon1.Hide();
				Mon1 = null;
			}
			if (Mon2 != null)
			{
				Mon2.Hide();
				Mon2 = null;
			}
		}

		public void ID_Monitors()
		{
			ID_Monitors_Clear();
			Application.DoEvents();
			Thread.Sleep(500);
			Screen[] allScreens = Screen.AllScreens;
			Rectangle bounds = allScreens[0].Bounds;
			Mon1 = new SplashMonitor((bounds.X > 0) ? "2" : "1");
			Mon1.SetBounds(bounds.X + bounds.Width, bounds.Height, 200, 200);
			Mon1.Show();
			if (allScreens.Length > 1)
			{
				Rectangle bounds2 = allScreens[1].Bounds;
				Mon2 = new SplashMonitor((bounds2.X > 0) ? "2" : "1");
				Mon2.SetBounds(bounds2.X + bounds2.Width, bounds2.Height, 200, 200);
				Mon2.Show();
				allScreens = Screen.AllScreens;
				bounds = Screen.PrimaryScreen.Bounds;
				bounds2 = allScreens[1].Bounds;
				if (bounds2.X <= 0)
				{
					bounds2 = allScreens[0].Bounds;
				}
				MWin.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
				MWin.Update();
				if (opciones.Monitors > 1 && MWin.publi != null)
				{
					MWin.publi.SetBounds(bounds2.X, bounds2.Y, bounds2.Width, bounds2.Height);
					MWin.publi.Update();
				}
			}
			else
			{
				bounds = Screen.PrimaryScreen.Bounds;
				Mon1 = new SplashMonitor((bounds.X > 0) ? "2" : "1");
				Mon1.SetBounds(bounds.X + bounds.Width, bounds.Height, 200, 200);
				Mon1.Show();
				if (MWin.publi != null)
				{
					MWin.publi.Hide();
				}
			}
			Update_Info();
		}

		private void bCheck_Click(object sender, EventArgs e)
		{
			ID_Monitors();
		}

		private void bClone_Click(object sender, EventArgs e)
		{
			ID_Monitors_Clear();
			Process process = new Process();
			try
			{
				process.StartInfo.WorkingDirectory = "c:\\windows\\system32";
				process.StartInfo.FileName = "DisplaySwitch.exe";
				process.StartInfo.Arguments = "/clone";
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
				process.Start();
			}
			catch
			{
			}
			Application.DoEvents();
			Thread.Sleep(1000);
			ID_Monitors();
		}

		private void bMon21_Click(object sender, EventArgs e)
		{
			ID_Monitors_Clear();
			Process process = new Process();
			try
			{
				process.StartInfo.WorkingDirectory = "c:\\windows\\system32";
				process.StartInfo.FileName = "DisplaySwitch.exe";
				process.StartInfo.Arguments = "/extend";
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
				process.Start();
			}
			catch
			{
			}
			Application.DoEvents();
			Thread.Sleep(500);
			ControlDisplay.SwapMonitor(1, 0);
			Application.DoEvents();
			Thread.Sleep(1000);
			ID_Monitors();
		}

		private void bMon12_Click(object sender, EventArgs e)
		{
			ID_Monitors_Clear();
			Process process = new Process();
			try
			{
				process.StartInfo.WorkingDirectory = "c:\\windows\\system32";
				process.StartInfo.FileName = "DisplaySwitch.exe";
				process.StartInfo.Arguments = "/extend";
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
				process.Start();
			}
			catch
			{
			}
			Application.DoEvents();
			Thread.Sleep(500);
			ControlDisplay.SwapMonitor(0, 1);
			ControlDisplay.SwapMonitor(0, 1);
			Application.DoEvents();
			Thread.Sleep(1000);
			ID_Monitors();
		}

		private void bOk_Click(object sender, EventArgs e)
		{
			ID_Monitors_Clear();
			opciones.Publi = ePubli.Text;
			opciones.Monitors = ((!cMon.Checked) ? 1 : 2);
			Close();
		}

		private void bCancel_Click(object sender, EventArgs e)
		{
			ID_Monitors_Clear();
			Close();
		}

		private void DLG_Monitors_Load(object sender, EventArgs e)
		{
			ID_Monitors();
		}

		private void DLG_Monitors_FormClosed(object sender, FormClosedEventArgs e)
		{
			ID_Monitors_Clear();
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
			lPubli = new System.Windows.Forms.Label();
			ePubli = new System.Windows.Forms.TextBox();
			bCheck = new System.Windows.Forms.Button();
			bClone = new System.Windows.Forms.Button();
			bMon12 = new System.Windows.Forms.Button();
			bMon21 = new System.Windows.Forms.Button();
			bCancel = new System.Windows.Forms.Button();
			bOk = new System.Windows.Forms.Button();
			cMon = new System.Windows.Forms.CheckBox();
			lMon1 = new System.Windows.Forms.Label();
			lMon2 = new System.Windows.Forms.Label();
			SuspendLayout();
			lPubli.AutoSize = true;
			lPubli.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lPubli.Location = new System.Drawing.Point(12, 44);
			lPubli.Name = "lPubli";
			lPubli.Size = new System.Drawing.Size(124, 13);
			lPubli.TabIndex = 39;
			lPubli.Text = "Slide show alternative url";
			ePubli.Location = new System.Drawing.Point(12, 60);
			ePubli.Name = "ePubli";
			ePubli.Size = new System.Drawing.Size(460, 20);
			ePubli.TabIndex = 40;
			bCheck.Location = new System.Drawing.Point(12, 196);
			bCheck.Name = "bCheck";
			bCheck.Size = new System.Drawing.Size(145, 40);
			bCheck.TabIndex = 43;
			bCheck.Text = "Check";
			bCheck.UseVisualStyleBackColor = true;
			bCheck.Click += new System.EventHandler(bCheck_Click);
			bClone.Location = new System.Drawing.Point(12, 150);
			bClone.Name = "bClone";
			bClone.Size = new System.Drawing.Size(145, 40);
			bClone.TabIndex = 44;
			bClone.Text = "Set Clone monitor";
			bClone.UseVisualStyleBackColor = true;
			bClone.Click += new System.EventHandler(bClone_Click);
			bMon12.Location = new System.Drawing.Point(171, 150);
			bMon12.Name = "bMon12";
			bMon12.Size = new System.Drawing.Size(145, 40);
			bMon12.TabIndex = 45;
			bMon12.Text = "Set Monitor 1/2";
			bMon12.UseVisualStyleBackColor = true;
			bMon12.Click += new System.EventHandler(bMon12_Click);
			bMon21.Location = new System.Drawing.Point(330, 150);
			bMon21.Name = "bMon21";
			bMon21.Size = new System.Drawing.Size(145, 40);
			bMon21.TabIndex = 46;
			bMon21.Text = "Set Monitor 2/1";
			bMon21.UseVisualStyleBackColor = true;
			bMon21.Click += new System.EventHandler(bMon21_Click);
			bCancel.BackgroundImage = Kiosk.Properties.Resources.ico_del;
			bCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			bCancel.Location = new System.Drawing.Point(341, 233);
			bCancel.Name = "bCancel";
			bCancel.Size = new System.Drawing.Size(64, 48);
			bCancel.TabIndex = 15;
			bCancel.UseVisualStyleBackColor = true;
			bCancel.Click += new System.EventHandler(bCancel_Click);
			bOk.BackgroundImage = Kiosk.Properties.Resources.ico_ok;
			bOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			bOk.Location = new System.Drawing.Point(411, 233);
			bOk.Name = "bOk";
			bOk.Size = new System.Drawing.Size(64, 48);
			bOk.TabIndex = 14;
			bOk.UseVisualStyleBackColor = true;
			bOk.Click += new System.EventHandler(bOk_Click);
			cMon.AutoSize = true;
			cMon.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cMon.Location = new System.Drawing.Point(12, 6);
			cMon.Name = "cMon";
			cMon.Size = new System.Drawing.Size(168, 35);
			cMon.TabIndex = 47;
			cMon.Text = "Slide Show";
			cMon.UseVisualStyleBackColor = true;
			lMon1.AutoSize = true;
			lMon1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lMon1.Location = new System.Drawing.Point(15, 87);
			lMon1.Name = "lMon1";
			lMon1.Size = new System.Drawing.Size(71, 17);
			lMon1.TabIndex = 48;
			lMon1.Text = "Monitor 1:";
			lMon2.AutoSize = true;
			lMon2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lMon2.Location = new System.Drawing.Point(15, 112);
			lMon2.Name = "lMon2";
			lMon2.Size = new System.Drawing.Size(71, 17);
			lMon2.TabIndex = 49;
			lMon2.Text = "Monitor 2:";
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(484, 293);
			base.Controls.Add(lMon2);
			base.Controls.Add(lMon1);
			base.Controls.Add(cMon);
			base.Controls.Add(bMon21);
			base.Controls.Add(bMon12);
			base.Controls.Add(bClone);
			base.Controls.Add(bCheck);
			base.Controls.Add(ePubli);
			base.Controls.Add(lPubli);
			base.Controls.Add(bCancel);
			base.Controls.Add(bOk);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "DLG_Monitors";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Monitor Slide Show";
			base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(DLG_Monitors_FormClosed);
			base.Load += new System.EventHandler(DLG_Monitors_Load);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
