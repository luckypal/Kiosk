using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace wyUpdate
{
	public class MSGBOX_Timer : Form
	{
		private IContainer components;

		private Button bOK;

		private Timer tWAIT;

		private Label lINFO;

		public MSGBOX_Timer(string _info, string _boto, int _time)
		{
			InitializeComponent();
			lINFO.Text = _info;
			bOK.Text = _boto;
			tWAIT.Interval = _time * 1000;
			tWAIT.Enabled = true;
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void tWAIT_Tick(object sender, EventArgs e)
		{
			Close();
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
			tWAIT = new System.Windows.Forms.Timer(components);
			lINFO = new System.Windows.Forms.Label();
			SuspendLayout();
			bOK.Location = new System.Drawing.Point(191, 124);
			bOK.Name = "bOK";
			bOK.Size = new System.Drawing.Size(99, 52);
			bOK.TabIndex = 0;
			bOK.Text = "OK";
			bOK.UseVisualStyleBackColor = true;
			bOK.Click += new System.EventHandler(bOK_Click);
			tWAIT.Tick += new System.EventHandler(tWAIT_Tick);
			lINFO.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lINFO.Location = new System.Drawing.Point(13, 13);
			lINFO.Name = "lINFO";
			lINFO.Size = new System.Drawing.Size(457, 98);
			lINFO.TabIndex = 1;
			lINFO.Text = "-";
			lINFO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(482, 188);
			base.Controls.Add(lINFO);
			base.Controls.Add(bOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "MSGBOX_Timer";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "MSGBOX_Timer";
			base.TopMost = true;
			ResumeLayout(false);
		}
	}
}
