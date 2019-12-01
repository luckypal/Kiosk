using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
	public class Disconnect : Form
	{
		public Configuracion opciones;

		public bool OK;

		private IContainer components = null;

		private Label lInfo;

		public Disconnect(ref Configuracion _opc)
		{
			OK = false;
			InitializeComponent();
			opciones = _opc;
			Localize();
		}

		private void Localize()
		{
			SuspendLayout();
			lInfo.Text = opciones.Localize.Text("Disconnect, Kiosk halted");
			ResumeLayout();
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
			lInfo = new System.Windows.Forms.Label();
			SuspendLayout();
			lInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			lInfo.ForeColor = System.Drawing.Color.Yellow;
			lInfo.Location = new System.Drawing.Point(0, 0);
			lInfo.Name = "lInfo";
			lInfo.Size = new System.Drawing.Size(333, 136);
			lInfo.TabIndex = 0;
			lInfo.Text = "-";
			lInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			BackColor = System.Drawing.Color.Red;
			base.ClientSize = new System.Drawing.Size(333, 136);
			base.ControlBox = false;
			base.Controls.Add(lInfo);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "Disconnect";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			base.TopMost = true;
			ResumeLayout(false);
		}
	}
}
