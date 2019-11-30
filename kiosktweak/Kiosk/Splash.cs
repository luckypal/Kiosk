using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
	public class Splash : Form
	{
		private IContainer components;

		private Label label1;

		public Splash(string _msg)
		{
			InitializeComponent();
			label1.Text = _msg;
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
			label1 = new System.Windows.Forms.Label();
			SuspendLayout();
			label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			label1.Dock = System.Windows.Forms.DockStyle.Fill;
			label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			label1.Location = new System.Drawing.Point(0, 0);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(328, 83);
			label1.TabIndex = 0;
			label1.Text = "-";
			label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(328, 83);
			base.Controls.Add(label1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "Splash";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Splash";
			base.TopMost = true;
			ResumeLayout(false);
		}
	}
}
