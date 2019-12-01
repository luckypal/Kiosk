using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
	public class DLG_Dispenser : Form
	{
		public Configuracion opciones;

		private IContainer components = null;

		private Button bCancel;

		private Button bOk;

		private CheckBox cMIN;

		private CheckBox cMAX;

		private CheckBox cPASS;

		private TextBox eMIN;

		private TextBox eMAX;

		private TextBox ePASS;

		private CheckBox cON;

		private CheckBox cHALT;

		private CheckBox cBILL;

		public DLG_Dispenser(ref Configuracion _opc)
		{
			opciones = _opc;
			InitializeComponent();
			cON.Checked = ((opciones.Disp_Enable == 1) ? true : false);
			cMAX.Checked = ((opciones.Disp_Max > 0) ? true : false);
			cMIN.Checked = ((opciones.Disp_Min > 0) ? true : false);
			eMAX.Text = string.Concat((opciones.Disp_Max >= 0) ? opciones.Disp_Max : (opciones.Disp_Max * -1));
			eMIN.Text = string.Concat((opciones.Disp_Min >= 0) ? opciones.Disp_Min : (opciones.Disp_Min * -1));
			cHALT.Checked = ((opciones.Disp_Out == 1) ? true : false);
			cBILL.Checked = ((opciones.Disp_Ticket == 1) ? true : false);
			cPASS.Checked = ((opciones.Disp_Recovery == 1) ? true : false);
			ePASS.Text = opciones.Disp_Recovery_Pass;
		}

		private void Localize()
		{
			SuspendLayout();
			ResumeLayout();
		}

		private void bOk_Click(object sender, EventArgs e)
		{
			opciones.Disp_Enable = (cON.Checked ? 1 : 0);
			if (cMAX.Checked)
			{
				try
				{
					opciones.Disp_Max = int.Parse(eMAX.Text);
				}
				catch
				{
				}
			}
			else
			{
				try
				{
					opciones.Disp_Max = int.Parse(eMAX.Text) * -1;
				}
				catch
				{
				}
			}
			if (cMIN.Checked)
			{
				try
				{
					opciones.Disp_Min = int.Parse(eMIN.Text);
				}
				catch
				{
				}
			}
			else
			{
				try
				{
					opciones.Disp_Min = int.Parse(eMIN.Text) * -1;
				}
				catch
				{
				}
			}
			opciones.Disp_Out = (cHALT.Checked ? 1 : 0);
			opciones.Disp_Ticket = (cBILL.Checked ? 1 : 0);
			opciones.Disp_Recovery = (cPASS.Checked ? 1 : 0);
			opciones.Disp_Recovery_Pass = ePASS.Text;
			Close();
		}

		private void bCancel_Click(object sender, EventArgs e)
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
			bCancel = new System.Windows.Forms.Button();
			bOk = new System.Windows.Forms.Button();
			cMIN = new System.Windows.Forms.CheckBox();
			cMAX = new System.Windows.Forms.CheckBox();
			cPASS = new System.Windows.Forms.CheckBox();
			eMIN = new System.Windows.Forms.TextBox();
			eMAX = new System.Windows.Forms.TextBox();
			ePASS = new System.Windows.Forms.TextBox();
			cON = new System.Windows.Forms.CheckBox();
			cHALT = new System.Windows.Forms.CheckBox();
			cBILL = new System.Windows.Forms.CheckBox();
			SuspendLayout();
			bCancel.BackgroundImage = Kiosk.Properties.Resources.ico_del;
			bCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			bCancel.Location = new System.Drawing.Point(305, 361);
			bCancel.Name = "bCancel";
			bCancel.Size = new System.Drawing.Size(64, 48);
			bCancel.TabIndex = 17;
			bCancel.UseVisualStyleBackColor = true;
			bCancel.Click += new System.EventHandler(bCancel_Click);
			bOk.BackgroundImage = Kiosk.Properties.Resources.ico_ok;
			bOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			bOk.Location = new System.Drawing.Point(374, 361);
			bOk.Name = "bOk";
			bOk.Size = new System.Drawing.Size(64, 48);
			bOk.TabIndex = 16;
			bOk.UseVisualStyleBackColor = true;
			bOk.Click += new System.EventHandler(bOk_Click);
			cMIN.AutoSize = true;
			cMIN.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cMIN.Location = new System.Drawing.Point(15, 211);
			cMIN.Name = "cMIN";
			cMIN.Size = new System.Drawing.Size(259, 35);
			cMIN.TabIndex = 18;
			cMIN.Text = "Minimum for check";
			cMIN.UseVisualStyleBackColor = true;
			cMAX.AutoSize = true;
			cMAX.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cMAX.Location = new System.Drawing.Point(15, 266);
			cMAX.Name = "cMAX";
			cMAX.Size = new System.Drawing.Size(266, 35);
			cMAX.TabIndex = 19;
			cMAX.Text = "Maximum for check";
			cMAX.UseVisualStyleBackColor = true;
			cPASS.AutoSize = true;
			cPASS.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cPASS.Location = new System.Drawing.Point(15, 161);
			cPASS.Name = "cPASS";
			cPASS.Size = new System.Drawing.Size(215, 35);
			cPASS.TabIndex = 20;
			cPASS.Text = "Recovery code";
			cPASS.UseVisualStyleBackColor = true;
			eMIN.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			eMIN.Location = new System.Drawing.Point(331, 211);
			eMIN.Name = "eMIN";
			eMIN.Size = new System.Drawing.Size(113, 38);
			eMIN.TabIndex = 24;
			eMAX.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			eMAX.Location = new System.Drawing.Point(331, 264);
			eMAX.Name = "eMAX";
			eMAX.Size = new System.Drawing.Size(113, 38);
			eMAX.TabIndex = 26;
			ePASS.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			ePASS.Location = new System.Drawing.Point(331, 161);
			ePASS.Name = "ePASS";
			ePASS.Size = new System.Drawing.Size(113, 38);
			ePASS.TabIndex = 28;
			cON.AutoSize = true;
			cON.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cON.Location = new System.Drawing.Point(15, 15);
			cON.Name = "cON";
			cON.Size = new System.Drawing.Size(321, 35);
			cON.TabIndex = 30;
			cON.Text = "Enable check dispenser";
			cON.UseVisualStyleBackColor = true;
			cHALT.AutoSize = true;
			cHALT.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cHALT.Location = new System.Drawing.Point(15, 65);
			cHALT.Name = "cHALT";
			cHALT.Size = new System.Drawing.Size(201, 35);
			cHALT.TabIndex = 31;
			cHALT.Text = "Out of service";
			cHALT.UseVisualStyleBackColor = true;
			cBILL.AutoSize = true;
			cBILL.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cBILL.Location = new System.Drawing.Point(15, 112);
			cBILL.Name = "cBILL";
			cBILL.Size = new System.Drawing.Size(234, 35);
			cBILL.TabIndex = 32;
			cBILL.Text = "Alternative ticket";
			cBILL.UseVisualStyleBackColor = true;
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(452, 419);
			base.Controls.Add(cBILL);
			base.Controls.Add(cHALT);
			base.Controls.Add(cON);
			base.Controls.Add(ePASS);
			base.Controls.Add(eMAX);
			base.Controls.Add(eMIN);
			base.Controls.Add(cPASS);
			base.Controls.Add(cMAX);
			base.Controls.Add(cMIN);
			base.Controls.Add(bCancel);
			base.Controls.Add(bOk);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			base.Name = "DLG_Dispenser";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "DLG_Dispenser";
			base.TopMost = true;
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
