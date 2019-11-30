// Decompiled with JetBrains decompiler
// Type: Kiosk.DLG_Dispenser
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
  public class DLG_Dispenser : Form
  {
    private IContainer components = (IContainer) null;
    public Configuracion opciones;
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
      this.opciones = _opc;
      this.InitializeComponent();
      this.cON.Checked = this.opciones.Disp_Enable == 1;
      this.cMAX.Checked = this.opciones.Disp_Max > 0;
      this.cMIN.Checked = this.opciones.Disp_Min > 0;
      this.eMAX.Text = string.Concat((object) (this.opciones.Disp_Max >= 0 ? this.opciones.Disp_Max : this.opciones.Disp_Max * -1));
      this.eMIN.Text = string.Concat((object) (this.opciones.Disp_Min >= 0 ? this.opciones.Disp_Min : this.opciones.Disp_Min * -1));
      this.cHALT.Checked = this.opciones.Disp_Out == 1;
      this.cBILL.Checked = this.opciones.Disp_Ticket == 1;
      this.cPASS.Checked = this.opciones.Disp_Recovery == 1;
      this.ePASS.Text = this.opciones.Disp_Recovery_Pass;
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.ResumeLayout();
    }

    private void bOk_Click(object sender, EventArgs e)
    {
      this.opciones.Disp_Enable = this.cON.Checked ? 1 : 0;
      if (this.cMAX.Checked)
      {
        try
        {
          this.opciones.Disp_Max = int.Parse(this.eMAX.Text);
        }
        catch
        {
        }
      }
      else
      {
        try
        {
          this.opciones.Disp_Max = int.Parse(this.eMAX.Text) * -1;
        }
        catch
        {
        }
      }
      if (this.cMIN.Checked)
      {
        try
        {
          this.opciones.Disp_Min = int.Parse(this.eMIN.Text);
        }
        catch
        {
        }
      }
      else
      {
        try
        {
          this.opciones.Disp_Min = int.Parse(this.eMIN.Text) * -1;
        }
        catch
        {
        }
      }
      this.opciones.Disp_Out = this.cHALT.Checked ? 1 : 0;
      this.opciones.Disp_Ticket = this.cBILL.Checked ? 1 : 0;
      this.opciones.Disp_Recovery = this.cPASS.Checked ? 1 : 0;
      this.opciones.Disp_Recovery_Pass = this.ePASS.Text;
      this.Close();
    }

    private void bCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.bCancel = new Button();
      this.bOk = new Button();
      this.cMIN = new CheckBox();
      this.cMAX = new CheckBox();
      this.cPASS = new CheckBox();
      this.eMIN = new TextBox();
      this.eMAX = new TextBox();
      this.ePASS = new TextBox();
      this.cON = new CheckBox();
      this.cHALT = new CheckBox();
      this.cBILL = new CheckBox();
      this.SuspendLayout();
      this.bCancel.BackgroundImage = (Image) Resources.ico_del;
      this.bCancel.BackgroundImageLayout = ImageLayout.Center;
      this.bCancel.Location = new Point(305, 361);
      this.bCancel.Name = "bCancel";
      this.bCancel.Size = new Size(64, 48);
      this.bCancel.TabIndex = 17;
      this.bCancel.UseVisualStyleBackColor = true;
      this.bCancel.Click += new EventHandler(this.bCancel_Click);
      this.bOk.BackgroundImage = (Image) Resources.ico_ok;
      this.bOk.BackgroundImageLayout = ImageLayout.Center;
      this.bOk.Location = new Point(374, 361);
      this.bOk.Name = "bOk";
      this.bOk.Size = new Size(64, 48);
      this.bOk.TabIndex = 16;
      this.bOk.UseVisualStyleBackColor = true;
      this.bOk.Click += new EventHandler(this.bOk_Click);
      this.cMIN.AutoSize = true;
      this.cMIN.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cMIN.Location = new Point(15, 211);
      this.cMIN.Name = "cMIN";
      this.cMIN.Size = new Size(259, 35);
      this.cMIN.TabIndex = 18;
      this.cMIN.Text = "Minimum for check";
      this.cMIN.UseVisualStyleBackColor = true;
      this.cMAX.AutoSize = true;
      this.cMAX.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cMAX.Location = new Point(15, 266);
      this.cMAX.Name = "cMAX";
      this.cMAX.Size = new Size(266, 35);
      this.cMAX.TabIndex = 19;
      this.cMAX.Text = "Maximum for check";
      this.cMAX.UseVisualStyleBackColor = true;
      this.cPASS.AutoSize = true;
      this.cPASS.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cPASS.Location = new Point(15, 161);
      this.cPASS.Name = "cPASS";
      this.cPASS.Size = new Size(215, 35);
      this.cPASS.TabIndex = 20;
      this.cPASS.Text = "Recovery code";
      this.cPASS.UseVisualStyleBackColor = true;
      this.eMIN.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.eMIN.Location = new Point(331, 211);
      this.eMIN.Name = "eMIN";
      this.eMIN.Size = new Size(113, 38);
      this.eMIN.TabIndex = 24;
      this.eMAX.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.eMAX.Location = new Point(331, 264);
      this.eMAX.Name = "eMAX";
      this.eMAX.Size = new Size(113, 38);
      this.eMAX.TabIndex = 26;
      this.ePASS.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ePASS.Location = new Point(331, 161);
      this.ePASS.Name = "ePASS";
      this.ePASS.Size = new Size(113, 38);
      this.ePASS.TabIndex = 28;
      this.cON.AutoSize = true;
      this.cON.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cON.Location = new Point(15, 15);
      this.cON.Name = "cON";
      this.cON.Size = new Size(321, 35);
      this.cON.TabIndex = 30;
      this.cON.Text = "Enable check dispenser";
      this.cON.UseVisualStyleBackColor = true;
      this.cHALT.AutoSize = true;
      this.cHALT.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cHALT.Location = new Point(15, 65);
      this.cHALT.Name = "cHALT";
      this.cHALT.Size = new Size(201, 35);
      this.cHALT.TabIndex = 31;
      this.cHALT.Text = "Out of service";
      this.cHALT.UseVisualStyleBackColor = true;
      this.cBILL.AutoSize = true;
      this.cBILL.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cBILL.Location = new Point(15, 112);
      this.cBILL.Name = "cBILL";
      this.cBILL.Size = new Size(234, 35);
      this.cBILL.TabIndex = 32;
      this.cBILL.Text = "Alternative ticket";
      this.cBILL.UseVisualStyleBackColor = true;
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(452, 419);
      this.Controls.Add((Control) this.cBILL);
      this.Controls.Add((Control) this.cHALT);
      this.Controls.Add((Control) this.cON);
      this.Controls.Add((Control) this.ePASS);
      this.Controls.Add((Control) this.eMAX);
      this.Controls.Add((Control) this.eMIN);
      this.Controls.Add((Control) this.cPASS);
      this.Controls.Add((Control) this.cMAX);
      this.Controls.Add((Control) this.cMIN);
      this.Controls.Add((Control) this.bCancel);
      this.Controls.Add((Control) this.bOk);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Margin = new Padding(2, 2, 2, 2);
      this.Name = nameof (DLG_Dispenser);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = nameof (DLG_Dispenser);
      this.TopMost = true;
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
