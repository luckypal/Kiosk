using Kiosk.Properties;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
	public class DLG_Check_Tiquets : Form
	{
		private IContainer components = null;

		private Button bOK;

		private ListBox lTickets;

		public DLG_Check_Tiquets()
		{
			InitializeComponent();
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
			bOK = new System.Windows.Forms.Button();
			lTickets = new System.Windows.Forms.ListBox();
			SuspendLayout();
			bOK.Image = Kiosk.Properties.Resources.ico_ok;
			bOK.Location = new System.Drawing.Point(352, 398);
			bOK.Name = "bOK";
			bOK.Size = new System.Drawing.Size(48, 48);
			bOK.TabIndex = 7;
			bOK.UseVisualStyleBackColor = true;
			lTickets.FormattingEnabled = true;
			lTickets.Location = new System.Drawing.Point(13, 13);
			lTickets.Name = "lTickets";
			lTickets.Size = new System.Drawing.Size(387, 368);
			lTickets.TabIndex = 8;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(412, 458);
			base.Controls.Add(lTickets);
			base.Controls.Add(bOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "DLG_Check_Tiquets";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Tiquet Information";
			ResumeLayout(false);
		}
	}
}
