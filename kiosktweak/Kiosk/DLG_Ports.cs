using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Management;
using System.Windows.Forms;

namespace Kiosk
{
	public class DLG_Ports : Form
	{
		private IContainer components;

		private Button bOK;

		private Button eFind;

		private ListBox cP1;

		private ToolTip toolTip1;

		public DLG_Ports()
		{
			InitializeComponent();
			eFind_Click(null, null);
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void eFind_Click(object sender, EventArgs e)
		{
			string[] portNames = SerialPort.GetPortNames();
			portNames = GetCOMPortsInfo();
			cP1.Items.Clear();
			if (portNames == null)
			{
				return;
			}
			for (int i = 0; i < portNames.Length; i++)
			{
				if (!string.IsNullOrEmpty(portNames[i]))
				{
					cP1.Items.Add(portNames[i]);
				}
			}
		}

		public string[] GetCOMPortsInfo()
		{
			string text = "";
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE ConfigManagerErrorCode = 0");
			using (managementObjectSearcher)
			{
				string text2 = null;
				foreach (ManagementObject item in managementObjectSearcher.Get())
				{
					if (item != null)
					{
						object obj = item["Caption"];
						if (obj != null)
						{
							text2 = obj.ToString();
							if (text2.Contains("(COM"))
							{
								text = text + text2 + "@";
							}
						}
					}
				}
			}
			return text.Split('@');
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
			eFind = new System.Windows.Forms.Button();
			bOK = new System.Windows.Forms.Button();
			cP1 = new System.Windows.Forms.ListBox();
			toolTip1 = new System.Windows.Forms.ToolTip(components);
			SuspendLayout();
			eFind.Image = Kiosk.Properties.Resources.ico_find;
			eFind.Location = new System.Drawing.Point(12, 12);
			eFind.Name = "eFind";
			eFind.Size = new System.Drawing.Size(48, 48);
			eFind.TabIndex = 0;
			toolTip1.SetToolTip(eFind, "Find new serial devices");
			eFind.UseVisualStyleBackColor = true;
			eFind.Click += new System.EventHandler(eFind_Click);
			bOK.Image = Kiosk.Properties.Resources.ico_ok;
			bOK.Location = new System.Drawing.Point(371, 262);
			bOK.Name = "bOK";
			bOK.Size = new System.Drawing.Size(48, 48);
			bOK.TabIndex = 1;
			bOK.UseVisualStyleBackColor = true;
			bOK.Click += new System.EventHandler(bOK_Click);
			cP1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cP1.FormattingEnabled = true;
			cP1.ItemHeight = 24;
			cP1.Location = new System.Drawing.Point(65, 12);
			cP1.Name = "cP1";
			cP1.Size = new System.Drawing.Size(354, 244);
			cP1.TabIndex = 3;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(433, 318);
			base.Controls.Add(cP1);
			base.Controls.Add(eFind);
			base.Controls.Add(bOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "DLG_Ports";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Tweak Ports";
			ResumeLayout(false);
		}
	}
}
