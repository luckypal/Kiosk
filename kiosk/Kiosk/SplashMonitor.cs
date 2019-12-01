using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
	public class SplashMonitor : Form
	{
		private IContainer components = null;

		private Label lNum;

		public SplashMonitor(string _text)
		{
			InitializeComponent();
			lNum.Text = _text;
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
			lNum = new System.Windows.Forms.Label();
			SuspendLayout();
			lNum.Dock = System.Windows.Forms.DockStyle.Fill;
			lNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 120f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lNum.ForeColor = System.Drawing.Color.Red;
			lNum.Location = new System.Drawing.Point(0, 0);
			lNum.Name = "lNum";
			lNum.Size = new System.Drawing.Size(204, 194);
			lNum.TabIndex = 0;
			lNum.Text = "1";
			lNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			BackColor = System.Drawing.Color.Black;
			base.ClientSize = new System.Drawing.Size(204, 194);
			base.Controls.Add(lNum);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "SplashMonitor";
			base.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			Text = "SplashMonitor";
			base.TopMost = true;
			base.TransparencyKey = System.Drawing.Color.Black;
			ResumeLayout(false);
		}
	}
}
