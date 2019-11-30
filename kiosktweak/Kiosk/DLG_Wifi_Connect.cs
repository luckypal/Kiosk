using GLib;
using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
	public class DLG_Wifi_Connect : Form
	{
		public bool OK;

		public bool IsClosed;

		public string SID;

		public string Password;

		public string SEC;

		public bool AlwaysConnect;

		public bool NewProfile;

		private IContainer components;

		private Panel pBOTTOM;

		private Button bCancel;

		private Button bOk;

		private Label lPassword;

		private GLib.PasswordBox ePassword;

		private TextBox tSID;

		private Label lSID;

		private Label lSec;

		private ComboBox cSec;

		private ToolTip toolTip1;

		private CheckBox cNew;

		private CheckBox cAlways;

		public DLG_Wifi_Connect(string _sid, string _pw, string _sec)
		{
			IsClosed = false;
			OK = false;
			AlwaysConnect = true;
			NewProfile = false;
			InitializeComponent();
			Localize();
			Password = _pw;
			SID = _sid;
			ePassword.Text = Password;
			tSID.Text = SID;
			SEC = _sec;
			string a;
			if ((a = _sec) == null)
			{
				return;
			}
			if (!(a == "IEEE80211_Open"))
			{
				if (!(a == "IEEE80211_SharedKey"))
				{
					if (!(a == "WPA"))
					{
						if (!(a == "WPA_PSK"))
						{
							if (!(a == "RSNA"))
							{
								if (a == "RSNA_PSK")
								{
									cSec.Text = "WPA2 PSK";
								}
							}
							else
							{
								cSec.Text = "WPA2";
							}
						}
						else
						{
							cSec.Text = "WPA PSK";
						}
					}
					else
					{
						cSec.Text = "WPA";
					}
				}
				else
				{
					cSec.Text = "WEP";
				}
			}
			else
			{
				cSec.Text = "OPEN";
			}
		}

		private void Localize()
		{
			SuspendLayout();
			lSID.Text = "SID";
			lPassword.Text = "New Password";
			lSec.Text = "Security";
			ResumeLayout();
		}

		private void bOk_Click(object sender, EventArgs e)
		{
			SID = tSID.Text;
			Password = ePassword.Text;
			OK = true;
			switch (cSec.Text)
			{
			case "OPEN":
				SEC = "IEEE80211_Open";
				break;
			case "WEP":
				SEC = "IEEE80211_SharedKey";
				break;
			case "WPA":
				SEC = "WPA";
				break;
			case "WPA PSK":
				SEC = "WPA_PSK";
				break;
			case "RSNA":
				SEC = "WPA2";
				break;
			case "RSNA PSK":
				SEC = "WPA2_PSK";
				break;
			}
			NewProfile = cNew.Checked;
			AlwaysConnect = cAlways.Checked;
			Close();
		}

		private void bCancel_Click(object sender, EventArgs e)
		{
			OK = false;
			Close();
		}

		private void DLG_Wifi_Connect_FormClosed(object sender, FormClosedEventArgs e)
		{
			IsClosed = true;
		}

		private void DLG_Wifi_Connect_Load(object sender, EventArgs e)
		{
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
			pBOTTOM = new System.Windows.Forms.Panel();
			bCancel = new System.Windows.Forms.Button();
			bOk = new System.Windows.Forms.Button();
			lPassword = new System.Windows.Forms.Label();
			tSID = new System.Windows.Forms.TextBox();
			lSID = new System.Windows.Forms.Label();
			lSec = new System.Windows.Forms.Label();
			cSec = new System.Windows.Forms.ComboBox();
			ePassword = new GLib.PasswordBox();
			toolTip1 = new System.Windows.Forms.ToolTip(components);
			cAlways = new System.Windows.Forms.CheckBox();
			cNew = new System.Windows.Forms.CheckBox();
			pBOTTOM.SuspendLayout();
			SuspendLayout();
			pBOTTOM.Controls.Add(cNew);
			pBOTTOM.Controls.Add(bCancel);
			pBOTTOM.Controls.Add(bOk);
			pBOTTOM.Dock = System.Windows.Forms.DockStyle.Bottom;
			pBOTTOM.Location = new System.Drawing.Point(0, 265);
			pBOTTOM.Name = "pBOTTOM";
			pBOTTOM.Size = new System.Drawing.Size(453, 48);
			pBOTTOM.TabIndex = 6;
			bCancel.Dock = System.Windows.Forms.DockStyle.Right;
			bCancel.Image = Kiosk.Properties.Resources.ico_del;
			bCancel.Location = new System.Drawing.Point(357, 0);
			bCancel.Name = "bCancel";
			bCancel.Size = new System.Drawing.Size(48, 48);
			bCancel.TabIndex = 1;
			toolTip1.SetToolTip(bCancel, "Cancel connection attempt");
			bCancel.UseVisualStyleBackColor = true;
			bCancel.Click += new System.EventHandler(bCancel_Click);
			bOk.Dock = System.Windows.Forms.DockStyle.Right;
			bOk.Image = Kiosk.Properties.Resources.ico_wifi_connect;
			bOk.Location = new System.Drawing.Point(405, 0);
			bOk.Name = "bOk";
			bOk.Size = new System.Drawing.Size(48, 48);
			bOk.TabIndex = 0;
			toolTip1.SetToolTip(bOk, "Try to connect Wi-Fi");
			bOk.UseVisualStyleBackColor = true;
			bOk.Click += new System.EventHandler(bOk_Click);
			lPassword.AutoSize = true;
			lPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lPassword.Location = new System.Drawing.Point(10, 81);
			lPassword.Name = "lPassword";
			lPassword.Size = new System.Drawing.Size(16, 24);
			lPassword.TabIndex = 8;
			lPassword.Text = "-";
			tSID.Font = new System.Drawing.Font("Microsoft Sans Serif", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			tSID.Location = new System.Drawing.Point(12, 42);
			tSID.Name = "tSID";
			tSID.Size = new System.Drawing.Size(430, 29);
			tSID.TabIndex = 0;
			lSID.AutoSize = true;
			lSID.Font = new System.Drawing.Font("Microsoft Sans Serif", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lSID.Location = new System.Drawing.Point(11, 15);
			lSID.Name = "lSID";
			lSID.Size = new System.Drawing.Size(16, 24);
			lSID.TabIndex = 10;
			lSID.Text = "-";
			lSec.AutoSize = true;
			lSec.Font = new System.Drawing.Font("Microsoft Sans Serif", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lSec.Location = new System.Drawing.Point(11, 155);
			lSec.Name = "lSec";
			lSec.Size = new System.Drawing.Size(16, 24);
			lSec.TabIndex = 11;
			lSec.Text = "-";
			cSec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cSec.Font = new System.Drawing.Font("Microsoft Sans Serif", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cSec.FormattingEnabled = true;
			cSec.Items.AddRange(new object[5]
			{
				"WEP",
				"WPA",
				"WPA PSK",
				"WPA2",
				"WPA2 PSK"
			});
			cSec.Location = new System.Drawing.Point(11, 182);
			cSec.Name = "cSec";
			cSec.Size = new System.Drawing.Size(430, 32);
			cSec.TabIndex = 2;
			ePassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			ePassword.Location = new System.Drawing.Point(11, 110);
			ePassword.Name = "ePassword";
			ePassword.Size = new System.Drawing.Size(430, 29);
			ePassword.TabIndex = 1;
			cAlways.AutoSize = true;
			cAlways.Checked = true;
			cAlways.CheckState = System.Windows.Forms.CheckState.Checked;
			cAlways.Font = new System.Drawing.Font("Microsoft Sans Serif", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cAlways.Location = new System.Drawing.Point(14, 224);
			cAlways.Name = "cAlways";
			cAlways.Size = new System.Drawing.Size(230, 28);
			cAlways.TabIndex = 12;
			cAlways.Text = "Connect when available";
			cAlways.UseVisualStyleBackColor = true;
			cNew.AutoSize = true;
			cNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cNew.Location = new System.Drawing.Point(15, 9);
			cNew.Name = "cNew";
			cNew.Size = new System.Drawing.Size(197, 28);
			cNew.TabIndex = 13;
			cNew.Text = "Is a new connection";
			cNew.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(453, 313);
			base.ControlBox = false;
			base.Controls.Add(cAlways);
			base.Controls.Add(cSec);
			base.Controls.Add(lSec);
			base.Controls.Add(lSID);
			base.Controls.Add(tSID);
			base.Controls.Add(lPassword);
			base.Controls.Add(ePassword);
			base.Controls.Add(pBOTTOM);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "DLG_Wifi_Connect";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = " ";
			base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(DLG_Wifi_Connect_FormClosed);
			base.Load += new System.EventHandler(DLG_Wifi_Connect_Load);
			pBOTTOM.ResumeLayout(false);
			pBOTTOM.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
