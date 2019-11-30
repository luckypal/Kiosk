using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
	public class MessageWait : Form
	{
		private int oPOS;

		private string MSG;

		private IContainer components;

		private Label INFO;

		public MessageWait(string _msg)
		{
			oPOS = 0;
			MSG = _msg;
			InitializeComponent();
			UpdateInfo(0);
		}

		public void UpdateInfo(int p)
		{
			if (p != oPOS)
			{
				oPOS = p;
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
			SuspendLayout();
			INFO.Dock = System.Windows.Forms.DockStyle.Fill;
			INFO.Location = new System.Drawing.Point(0, 0);
			INFO.Name = "INFO";
			INFO.Size = new System.Drawing.Size(460, 111);
			INFO.TabIndex = 0;
			INFO.Text = "Updating your system";
			INFO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(460, 111);
			base.Controls.Add(INFO);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "MessageWait";
			Text = "MessageWait";
			ResumeLayout(false);
		}
	}
}
