// Decompiled with JetBrains decompiler
// Type: LCDLabel.LcdLabel
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.Drawing;
using System.Windows.Forms;

namespace LCDLabel
{
  public class LcdLabel : Control
  {
    private PixelShape FPixelShape = PixelShape.Square;
    private DotMatrix FDotMatrix = DotMatrix.mat5x7;
    private Color FBackGround = Color.Silver;
    private Color FPixOnColor = Color.Black;
    private Color FPixOffColor = Color.Gray;
    private Color FBorderColor = Color.Black;
    private PixelSize FPixelSize;
    private int FPixelSpacing;
    private int FCharSpacing;
    private int FLineSpacing;
    private int FBorderSpace;
    private int FTextLines;
    private int FNoOfChars;
    private Color FPixHalfColor;
    private int FPixWidth;
    private int FPixHeight;
    private int FWidth;
    private int FHeight;
    private int charw;
    private int charh;
    private int psx;
    private int psy;
    private int pix_x;
    private int pix_y;
    private int first_c;
    private int last_c;

    public LcdLabel()
    {
      this.DoubleBuffered = true;
      this.FWidth = 0;
      this.FHeight = 0;
      this.FCharSpacing = 2;
      this.FLineSpacing = 2;
      this.FPixelSpacing = 1;
      this.FBorderSpace = 3;
      this.FTextLines = 1;
      this.FNoOfChars = 10;
      this.FBorderColor = Color.Black;
      this.FBackGround = Color.Silver;
      this.FPixOnColor = Color.Black;
      this.FPixOffColor = Color.FromArgb(11184810);
      this.FPixelSize = PixelSize.pix2x2;
      this.CalcHalfColor();
      this.CalcSize();
    }

    public PixelSize PixelSize
    {
      get
      {
        return this.FPixelSize;
      }
      set
      {
        if (value == this.FPixelSize)
          return;
        this.FPixelSize = value;
        this.Invalidate();
      }
    }

    public PixelShape PixelShape
    {
      get
      {
        return this.FPixelShape;
      }
      set
      {
        if (value == this.FPixelShape)
          return;
        this.FPixelShape = value;
        this.Invalidate();
      }
    }

    public DotMatrix DotMatrix
    {
      get
      {
        return this.FDotMatrix;
      }
      set
      {
        this.FDotMatrix = value;
        this.Invalidate();
      }
    }

    public int PixelSpacing
    {
      get
      {
        return this.FPixelSpacing;
      }
      set
      {
        if (value < 0)
          throw new ArgumentException("Pixel spacing can't be less than zero");
        if (value == this.FPixelSpacing)
          return;
        this.FPixelSpacing = value;
        this.Invalidate();
      }
    }

    public int CharSpacing
    {
      get
      {
        return this.FCharSpacing;
      }
      set
      {
        if (value < 0)
          throw new ArgumentException("Character spacing can't be less than zero");
        if (value == this.FCharSpacing)
          return;
        this.FCharSpacing = value;
        this.Invalidate();
      }
    }

    public int LineSpacing
    {
      get
      {
        return this.FLineSpacing;
      }
      set
      {
        if (value < 0)
          throw new ArgumentException("Line spacing can't be less than zero");
        if (value == this.FLineSpacing)
          return;
        this.FLineSpacing = value;
        this.Invalidate();
      }
    }

    public int BorderSpace
    {
      get
      {
        return this.FBorderSpace;
      }
      set
      {
        if (value < 0)
          throw new ArgumentException("Border spacing can't be less than zero");
        if (value == this.FBorderSpace)
          return;
        this.FBorderSpace = value;
        this.Invalidate();
      }
    }

    public int TextLines
    {
      get
      {
        return this.FTextLines;
      }
      set
      {
        if (value < 1)
          throw new ArgumentException("Display needs at least one line");
        if (value == this.FTextLines)
          return;
        this.FTextLines = value;
        this.Invalidate();
      }
    }

    public int NumberOfCharacters
    {
      get
      {
        return this.FNoOfChars;
      }
      set
      {
        if (value < 1)
          throw new ArgumentException("Display needs at least one character");
        if (value == this.FNoOfChars)
          return;
        this.FNoOfChars = value;
        this.Invalidate();
      }
    }

    public Color BackGround
    {
      get
      {
        return this.FBackGround;
      }
      set
      {
        if (!(value != this.FBackGround))
          return;
        this.FBackGround = value;
        this.Invalidate();
      }
    }

    public Color PixelOn
    {
      get
      {
        return this.FPixOnColor;
      }
      set
      {
        if (!(value != this.FPixOnColor))
          return;
        this.FPixOnColor = value;
        this.CalcHalfColor();
        this.Invalidate();
      }
    }

    public Color PixelOff
    {
      get
      {
        return this.FPixOffColor;
      }
      set
      {
        if (!(value != this.FPixOffColor))
          return;
        this.FPixOffColor = value;
        this.Invalidate();
      }
    }

    public int PixelWidth
    {
      get
      {
        return this.FPixWidth;
      }
      set
      {
        if (this.FPixelSize != PixelSize.pixCustom || value == this.FPixWidth)
          return;
        if (value < 1)
          throw new ArgumentException("Display pixel width must be 1 or greater");
        this.FPixWidth = value;
        this.Invalidate();
      }
    }

    public int PixelHeight
    {
      get
      {
        return this.FPixHeight;
      }
      set
      {
        if (this.FPixelSize != PixelSize.pixCustom || value == this.FPixHeight)
          return;
        if (value < 1)
          throw new ArgumentException("Display pixel height must be 1 or greater");
        this.FPixHeight = value;
        this.Invalidate();
      }
    }

    public Color BorderColor
    {
      get
      {
        return this.FBorderColor;
      }
      set
      {
        if (!(value != this.FBorderColor))
          return;
        this.FBorderColor = value;
        this.Invalidate();
      }
    }

    public override string Text
    {
      get
      {
        return base.Text;
      }
      set
      {
        if (!(this.Text != value))
          return;
        base.Text = value;
        this.Invalidate();
      }
    }

    private void DrawMatrix(Graphics graphics, int xpos, int ypos, int charindex)
    {
      int x = xpos;
      int y = ypos;
      charindex -= this.first_c;
      using (SolidBrush solidBrush = new SolidBrush(Color.Black))
      {
        for (int index1 = 0; index1 < this.pix_y; ++index1)
        {
          for (int index2 = 0; index2 < this.pix_x; ++index2)
          {
            Color color = this.FPixOffColor;
            switch (this.FDotMatrix)
            {
              case DotMatrix.mat5x7:
                color = Matrix.Char5x7[charindex, index1, index2] != (byte) 1 ? this.FPixOffColor : this.FPixOnColor;
                break;
              case DotMatrix.mat5x8:
                color = Matrix.Char5x8[charindex, index1, index2] != (byte) 1 ? this.FPixOffColor : this.FPixOnColor;
                break;
              case DotMatrix.mat7x9:
                color = Matrix.Char7x9[charindex, index1, index2] != (byte) 1 ? this.FPixOffColor : this.FPixOnColor;
                break;
              case DotMatrix.mat9x12:
                color = Matrix.Char9x12[charindex, index1, index2] != (byte) 1 ? this.FPixOffColor : this.FPixOnColor;
                break;
              case DotMatrix.Hitachi:
                color = Matrix.CharHitachi[charindex, index1, index2] != (byte) 1 ? this.FPixOffColor : this.FPixOnColor;
                break;
              case DotMatrix.Hitachi2:
                color = charindex > 193 ? (Matrix.CharHitachiExt[charindex, index1, index2] != (byte) 1 ? this.FPixOffColor : this.FPixOnColor) : (index1 >= 7 ? this.FPixOffColor : (Matrix.CharHitachi[charindex, index1, index2] != (byte) 1 ? this.FPixOffColor : this.FPixOnColor));
                break;
              case DotMatrix.dos5x7:
                color = Matrix.CharDOS5x7[charindex, index1, index2] != (byte) 1 ? this.FPixOffColor : this.FPixOnColor;
                break;
            }
            solidBrush.Color = color;
            switch (this.FPixelShape)
            {
              case PixelShape.Square:
                graphics.FillRectangle((Brush) solidBrush, x, y, this.psx, this.psy);
                break;
              case PixelShape.Round:
                graphics.FillEllipse((Brush) solidBrush, x, y, this.psx, this.psy);
                break;
              case PixelShape.Shaped:
                if (color == this.FPixOnColor)
                {
                  solidBrush.Color = this.FPixHalfColor;
                  graphics.FillRectangle((Brush) solidBrush, x, y, this.psx, this.psy);
                  solidBrush.Color = color;
                  graphics.FillEllipse((Brush) solidBrush, x, y, this.psx, this.psy);
                  break;
                }
                solidBrush.Color = color;
                graphics.FillRectangle((Brush) solidBrush, x, y, this.psx, this.psy);
                break;
            }
            x = x + this.psx + this.FPixelSpacing;
          }
          x = xpos;
          y = y + this.psy + this.FPixelSpacing;
        }
      }
    }

    private void DrawCharacters(Graphics graphics)
    {
      if (this.Text == null)
        return;
      int xpos = this.FBorderSpace + 1;
      int ypos = this.FBorderSpace + 1;
      int num = 1;
      bool flag = false;
      for (int index1 = 1; index1 <= this.FTextLines; ++index1)
      {
        for (int index2 = 1; index2 <= this.FNoOfChars; ++index2)
        {
          if (!flag && num > this.Text.Length)
            flag = true;
          int charindex = !flag ? Convert.ToInt32(this.Text[num - 1]) : 0;
          if (charindex < this.first_c)
            charindex = this.first_c;
          if (charindex > this.last_c)
            charindex = this.last_c;
          this.DrawMatrix(graphics, xpos, ypos, charindex);
          xpos = xpos + this.charw + this.FCharSpacing;
          ++num;
        }
        ypos = ypos + this.charh + this.FLineSpacing;
        xpos = this.FBorderSpace + 1;
      }
    }

    private void CalcHalfColor()
    {
      this.FPixHalfColor = Color.FromArgb((int) (byte) ((uint) this.FPixOnColor.B / 2U) + (int) (byte) ((uint) this.FPixOnColor.G / 2U) * 256 + (int) (byte) ((uint) this.FPixOnColor.R / 2U) * 65536 + (int) this.FPixOnColor.A * 16777216);
    }

    private void CalcSize()
    {
      if (PixelSize.pixCustom == this.FPixelSize)
      {
        this.psx = this.FPixWidth;
        this.psy = this.FPixHeight;
      }
      else
      {
        this.psx = (int) (this.FPixelSize + 1);
        this.psy = this.psx;
        this.FPixWidth = this.psx;
        this.FPixHeight = this.psy;
      }
      switch (this.FDotMatrix)
      {
        case DotMatrix.mat5x7:
        case DotMatrix.Hitachi:
          this.pix_x = 5;
          this.pix_y = 7;
          break;
        case DotMatrix.mat5x8:
          this.pix_x = 5;
          this.pix_y = 8;
          break;
        case DotMatrix.mat7x9:
          this.pix_x = 7;
          this.pix_y = 9;
          break;
        case DotMatrix.mat9x12:
          this.pix_x = 9;
          this.pix_y = 12;
          break;
        case DotMatrix.Hitachi2:
          this.pix_x = 5;
          this.pix_y = 10;
          break;
        case DotMatrix.dos5x7:
          this.pix_x = 5;
          this.pix_y = 7;
          break;
      }
      this.charw = this.pix_x * this.psx + (this.pix_x - 1) * this.FPixelSpacing;
      this.charh = this.pix_y * this.psy + (this.pix_y - 1) * this.FPixelSpacing;
      this.Width = this.FBorderSpace * 2 + this.FCharSpacing * (this.FNoOfChars - 1) + this.charw * this.FNoOfChars + 2;
      this.Height = this.FBorderSpace * 2 + this.FLineSpacing * (this.FTextLines - 1) + this.charh * this.FTextLines + 2;
      this.FWidth = this.Width;
      this.FHeight = this.Height;
    }

    private void GetAsciiInterval()
    {
      switch (this.FDotMatrix)
      {
        case DotMatrix.mat5x7:
        case DotMatrix.Hitachi:
          this.first_c = 32;
          this.last_c = 223;
          break;
        case DotMatrix.mat5x8:
          this.first_c = 32;
          this.last_c = 126;
          break;
        case DotMatrix.mat7x9:
          this.first_c = 32;
          this.last_c = 126;
          break;
        case DotMatrix.mat9x12:
          this.first_c = 32;
          this.last_c = 126;
          break;
        case DotMatrix.Hitachi2:
          this.first_c = 32;
          this.last_c = 223;
          break;
        case DotMatrix.dos5x7:
          this.first_c = 0;
          this.last_c = (int) byte.MaxValue;
          break;
      }
    }

    private void CalcCharSize()
    {
      if (PixelSize.pixCustom == this.FPixelSize)
      {
        this.psx = this.FPixWidth;
        this.psy = this.FPixHeight;
      }
      else
      {
        this.psx = (int) (this.FPixelSize + 1);
        this.psy = this.psx;
        this.FPixWidth = this.psx;
        this.FPixHeight = this.psy;
      }
      switch (this.FDotMatrix)
      {
        case DotMatrix.mat5x7:
        case DotMatrix.Hitachi:
          this.pix_x = 5;
          this.pix_y = 7;
          break;
        case DotMatrix.mat5x8:
          this.pix_x = 5;
          this.pix_y = 8;
          break;
        case DotMatrix.mat7x9:
          this.pix_x = 7;
          this.pix_y = 9;
          break;
        case DotMatrix.mat9x12:
          this.pix_x = 9;
          this.pix_y = 12;
          break;
        case DotMatrix.Hitachi2:
          this.pix_x = 5;
          this.pix_y = 10;
          break;
        case DotMatrix.dos5x7:
          this.pix_x = 5;
          this.pix_y = 7;
          break;
      }
      this.charw = this.pix_x * this.psx + (this.pix_x - 1) * this.FPixelSpacing;
      this.charh = this.pix_y * this.psy + (this.pix_y - 1) * this.FPixelSpacing;
      this.FNoOfChars = (this.Width - 2 * this.FBorderSpace + this.FCharSpacing) / (this.charw + this.FCharSpacing);
      this.FTextLines = (this.Height - 2 * this.FBorderSpace + this.FLineSpacing) / (this.charh + this.FLineSpacing);
      if (this.FNoOfChars < 1)
        this.FNoOfChars = 1;
      if (this.FTextLines < 1)
        this.FTextLines = 1;
      this.Width = this.FBorderSpace * 2 + this.FCharSpacing * (this.FNoOfChars - 1) + this.charw * this.FNoOfChars + 2;
      this.Height = this.FBorderSpace * 2 + this.FLineSpacing * (this.FTextLines - 1) + this.charh * this.FTextLines + 2;
      this.FWidth = this.Width;
      this.FHeight = this.Height;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      bool flag = false;
      if (this.Width != this.FWidth)
      {
        flag = true;
        this.FWidth = this.Width;
      }
      if (this.Height != this.FHeight)
      {
        flag = true;
        this.FHeight = this.Height;
      }
      this.GetAsciiInterval();
      if (flag)
        this.CalcCharSize();
      else
        this.CalcSize();
      using (SolidBrush solidBrush = new SolidBrush(this.FBackGround))
      {
        using (new Pen((Brush) solidBrush))
        {
          Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
          if (this.Visible)
          {
            e.Graphics.FillRectangle((Brush) solidBrush, rect);
            if (!this.Enabled)
              return;
            this.DrawCharacters(e.Graphics);
          }
          else
          {
            solidBrush.Color = SystemColors.ButtonFace;
            e.Graphics.FillRectangle((Brush) solidBrush, rect);
          }
        }
      }
    }
  }
}
