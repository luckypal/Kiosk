using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CheckSystem
{
	public class MessageWait : Form
	{
		private int oPOS;

		private string MSG;

		private IContainer components;

		private Label INFO;

		private Button bOK;

		public MessageWait(string _msg)
		{
			oPOS = 0;
			MSG = _msg;
			InitializeComponent();
			bOK.Visible = false;
			UpdateInfo(0);
		}

		public void UpdateInfo(int p)
		{
			if (p != oPOS)
			{
				oPOS = p;
				bOK.Visible = false;
				base.WindowState = FormWindowState.Normal;
				BackColor = Color.LightBlue;
				switch (oPOS)
				{
				case -3:
					INFO.Text = MSG;
					bOK.Visible = true;
					base.WindowState = FormWindowState.Maximized;
					BackColor = Color.OrangeRed;
					break;
				case -2:
					INFO.Text = MSG;
					break;
				case -1:
					INFO.Text = MSG + "\r\n\r\nInstalling please wait";
					break;
				default:
					INFO.Text = MSG + "\r\n\r\nDownload " + oPOS;
					break;
				}
				INFO.Invalidate();
			}
		}

		public void UpdateMSG(string _msg)
		{
			if (_msg != MSG)
			{
				MSG = _msg;
				switch (oPOS)
				{
				case -2:
					INFO.Text = MSG;
					break;
				case -1:
					INFO.Text = MSG + "\r\n\r\nInstalling please wait";
					break;
				default:
					INFO.Text = MSG + "\r\n\r\nDownload " + oPOS;
					break;
				}
				INFO.Invalidate();
			}
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			Hide();
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
			INFO = new System.Windows.Forms.Label();
			bOK = new System.Windows.Forms.Button();
			SuspendLayout();
			INFO.Dock = System.Windows.Forms.DockStyle.Fill;
			INFO.Font = new System.Drawing.Font("Microsoft Sans Serif", 15f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			INFO.Location = new System.Drawing.Point(0, 0);
			INFO.Name = "INFO";
			INFO.Size = new System.Drawing.Size(460, 111);
			INFO.TabIndex = 0;
			INFO.Text = "Updating your system";
			INFO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			bOK.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			bOK.Location = new System.Drawing.Point(191, 12);
			bOK.Name = "bOK";
			bOK.Size = new System.Drawing.Size(75, 90);
			bOK.TabIndex = 1;
			bOK.Text = "Continue";
			bOK.UseVisualStyleBackColor = true;
			bOK.Click += new System.EventHandler(bOK_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(460, 111);
			base.Controls.Add(bOK);
			base.Controls.Add(INFO);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "MessageWait";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "MessageWait";
			base.TopMost = true;
			ResumeLayout(false);
		}
	}
}
