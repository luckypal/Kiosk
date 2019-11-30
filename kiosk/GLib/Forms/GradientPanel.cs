// Decompiled with JetBrains decompiler
// Type: GLib.Forms.GradientPanel
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GLib.Forms
{
  public class GradientPanel : Panel
  {
    private GradientPanel.Alignement _TAlign = GradientPanel.Alignement.Left;
    private IContainer components = (IContainer) null;
    private Color mStartColor;
    private Color mEndColor;
    private string _Text;

    public GradientPanel()
    {
      this.InitializeComponent();
      this.PaintGradient();
    }

    protected override void OnPaint(PaintEventArgs pe)
    {
      base.OnPaint(pe);
    }

    public Color PageStartColor
    {
      get
      {
        return this.mStartColor;
      }
      set
      {
        this.mStartColor = value;
        this.Invalidate();
        this.PaintGradient();
      }
    }

    public Color PageEndColor
    {
      get
      {
        return this.mEndColor;
      }
      set
      {
        this.mEndColor = value;
        this.Invalidate();
        this.PaintGradient();
      }
    }

    public string Title
    {
      get
      {
        return this._Text;
      }
      set
      {
        this._Text = value;
        this.Invalidate();
        this.PaintGradient();
      }
    }

    public GradientPanel.Alignement Title_Alignement
    {
      get
      {
        return this._TAlign;
      }
      set
      {
        this._TAlign = value;
        this.Invalidate();
        this.PaintGradient();
      }
    }

    private void PaintGradient()
    {
      LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new Point(0, 0), new Point(this.Width, this.Height), this.PageStartColor, this.PageEndColor);
      Bitmap bitmap = new Bitmap(this.Width, this.Height);
      Graphics graphics = Graphics.FromImage((Image) bitmap);
      graphics.FillRectangle((Brush) linearGradientBrush, new Rectangle(0, 0, this.Width, this.Height));
      this.BackgroundImage = (Image) bitmap;
      this.BackgroundImageLayout = ImageLayout.Stretch;
      SizeF sizeF = graphics.MeasureString(this._Text, this.Font);
      switch (this._TAlign)
      {
        case GradientPanel.Alignement.Center:
          graphics.DrawString(this._Text, this.Font, Brushes.Black, (float) (this.Width / 2) - sizeF.Width / 2f, (float) (this.Height / 2) - sizeF.Height / 2f);
          break;
        case GradientPanel.Alignement.Right:
          graphics.DrawString(this._Text, this.Font, Brushes.Black, (float) ((double) this.Width - (double) sizeF.Width - 32.0), (float) (this.Height / 2) - sizeF.Height / 2f);
          break;
        default:
          graphics.DrawString(this._Text, this.Font, Brushes.Black, 32f, (float) (this.Height / 2) - sizeF.Height / 2f);
          break;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.ResumeLayout(false);
    }

    public enum Alignement
    {
      Left,
      Center,
      Right,
    }
  }
}
