// Decompiled with JetBrains decompiler
// Type: Kiosk.DLG_Check_Tiquets
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using Kiosk.Properties;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Kiosk
{
  public class DLG_Check_Tiquets : Form
  {
    private IContainer components = (IContainer) null;
    private Button bOK;
    private ListBox lTickets;

    public DLG_Check_Tiquets()
    {
      this.InitializeComponent();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.bOK = new Button();
      this.lTickets = new ListBox();
      this.SuspendLayout();
      this.bOK.Image = (Image) Resources.ico_ok;
      this.bOK.Location = new Point(352, 398);
      this.bOK.Name = "bOK";
      this.bOK.Size = new Size(48, 48);
      this.bOK.TabIndex = 7;
      this.bOK.UseVisualStyleBackColor = true;
      this.lTickets.FormattingEnabled = true;
      this.lTickets.Location = new Point(13, 13);
      this.lTickets.Name = "lTickets";
      this.lTickets.Size = new Size(387, 368);
      this.lTickets.TabIndex = 8;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(412, 458);
      this.Controls.Add((Control) this.lTickets);
      this.Controls.Add((Control) this.bOK);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (DLG_Check_Tiquets);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Tiquet Information";
      this.ResumeLayout(false);
    }
  }
}
