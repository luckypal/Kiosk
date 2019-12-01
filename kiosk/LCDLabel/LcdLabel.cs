using System;
using System.Drawing;
using System.Windows.Forms;

namespace LCDLabel
{
	public class LcdLabel : Control
	{
		private PixelSize FPixelSize;

		private PixelShape FPixelShape = PixelShape.Square;

		private DotMatrix FDotMatrix = DotMatrix.mat5x7;

		private int FPixelSpacing;

		private int FCharSpacing;

		private int FLineSpacing;

		private int FBorderSpace;

		private int FTextLines;

		private int FNoOfChars;

		private Color FBackGround = Color.Silver;

		private Color FPixOnColor = Color.Black;

		private Color FPixOffColor = Color.Gray;

		private Color FPixHalfColor;

		private int FPixWidth;

		private int FPixHeight;

		private Color FBorderColor = Color.Black;

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

		public PixelSize PixelSize
		{
			get
			{
				return FPixelSize;
			}
			set
			{
				if (value != FPixelSize)
				{
					FPixelSize = value;
					Invalidate();
				}
			}
		}

		public PixelShape PixelShape
		{
			get
			{
				return FPixelShape;
			}
			set
			{
				if (value != FPixelShape)
				{
					FPixelShape = value;
					Invalidate();
				}
			}
		}

		public DotMatrix DotMatrix
		{
			get
			{
				return FDotMatrix;
			}
			set
			{
				FDotMatrix = value;
				Invalidate();
			}
		}

		public int PixelSpacing
		{
			get
			{
				return FPixelSpacing;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException("Pixel spacing can't be less than zero");
				}
				if (value != FPixelSpacing)
				{
					FPixelSpacing = value;
					Invalidate();
				}
			}
		}

		public int CharSpacing
		{
			get
			{
				return FCharSpacing;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException("Character spacing can't be less than zero");
				}
				if (value != FCharSpacing)
				{
					FCharSpacing = value;
					Invalidate();
				}
			}
		}

		public int LineSpacing
		{
			get
			{
				return FLineSpacing;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException("Line spacing can't be less than zero");
				}
				if (value != FLineSpacing)
				{
					FLineSpacing = value;
					Invalidate();
				}
			}
		}

		public int BorderSpace
		{
			get
			{
				return FBorderSpace;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException("Border spacing can't be less than zero");
				}
				if (value != FBorderSpace)
				{
					FBorderSpace = value;
					Invalidate();
				}
			}
		}

		public int TextLines
		{
			get
			{
				return FTextLines;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentException("Display needs at least one line");
				}
				if (value != FTextLines)
				{
					FTextLines = value;
					Invalidate();
				}
			}
		}

		public int NumberOfCharacters
		{
			get
			{
				return FNoOfChars;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentException("Display needs at least one character");
				}
				if (value != FNoOfChars)
				{
					FNoOfChars = value;
					Invalidate();
				}
			}
		}

		public Color BackGround
		{
			get
			{
				return FBackGround;
			}
			set
			{
				if (value != FBackGround)
				{
					FBackGround = value;
					Invalidate();
				}
			}
		}

		public Color PixelOn
		{
			get
			{
				return FPixOnColor;
			}
			set
			{
				if (value != FPixOnColor)
				{
					FPixOnColor = value;
					CalcHalfColor();
					Invalidate();
				}
			}
		}

		public Color PixelOff
		{
			get
			{
				return FPixOffColor;
			}
			set
			{
				if (value != FPixOffColor)
				{
					FPixOffColor = value;
					Invalidate();
				}
			}
		}

		public int PixelWidth
		{
			get
			{
				return FPixWidth;
			}
			set
			{
				if (FPixelSize == PixelSize.pixCustom && value != FPixWidth)
				{
					if (value < 1)
					{
						throw new ArgumentException("Display pixel width must be 1 or greater");
					}
					FPixWidth = value;
					Invalidate();
				}
			}
		}

		public int PixelHeight
		{
			get
			{
				return FPixHeight;
			}
			set
			{
				if (FPixelSize == PixelSize.pixCustom && value != FPixHeight)
				{
					if (value < 1)
					{
						throw new ArgumentException("Display pixel height must be 1 or greater");
					}
					FPixHeight = value;
					Invalidate();
				}
			}
		}

		public Color BorderColor
		{
			get
			{
				return FBorderColor;
			}
			set
			{
				if (value != FBorderColor)
				{
					FBorderColor = value;
					Invalidate();
				}
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
				if (Text != value)
				{
					base.Text = value;
					Invalidate();
				}
			}
		}

		public LcdLabel()
		{
			DoubleBuffered = true;
			FWidth = 0;
			FHeight = 0;
			FCharSpacing = 2;
			FLineSpacing = 2;
			FPixelSpacing = 1;
			FBorderSpace = 3;
			FTextLines = 1;
			FNoOfChars = 10;
			FBorderColor = Color.Black;
			FBackGround = Color.Silver;
			FPixOnColor = Color.Black;
			FPixOffColor = Color.FromArgb(11184810);
			FPixelSize = PixelSize.pix2x2;
			CalcHalfColor();
			CalcSize();
		}

		private void DrawMatrix(Graphics graphics, int xpos, int ypos, int charindex)
		{
			int num = xpos;
			int num2 = ypos;
			charindex -= first_c;
			using (SolidBrush solidBrush = new SolidBrush(Color.Black))
			{
				for (int i = 0; i < pix_y; i++)
				{
					for (int j = 0; j < pix_x; j++)
					{
						Color color = FPixOffColor;
						switch (FDotMatrix)
						{
						case DotMatrix.mat5x7:
							color = ((Matrix.Char5x7[charindex, i, j] != 1) ? FPixOffColor : FPixOnColor);
							break;
						case DotMatrix.mat5x8:
							color = ((Matrix.Char5x8[charindex, i, j] != 1) ? FPixOffColor : FPixOnColor);
							break;
						case DotMatrix.Hitachi:
							color = ((Matrix.CharHitachi[charindex, i, j] != 1) ? FPixOffColor : FPixOnColor);
							break;
						case DotMatrix.Hitachi2:
							color = ((charindex > 193) ? ((Matrix.CharHitachiExt[charindex, i, j] != 1) ? FPixOffColor : FPixOnColor) : ((i >= 7) ? FPixOffColor : ((Matrix.CharHitachi[charindex, i, j] != 1) ? FPixOffColor : FPixOnColor)));
							break;
						case DotMatrix.mat7x9:
							color = ((Matrix.Char7x9[charindex, i, j] != 1) ? FPixOffColor : FPixOnColor);
							break;
						case DotMatrix.mat9x12:
							color = ((Matrix.Char9x12[charindex, i, j] != 1) ? FPixOffColor : FPixOnColor);
							break;
						case DotMatrix.dos5x7:
							color = ((Matrix.CharDOS5x7[charindex, i, j] != 1) ? FPixOffColor : FPixOnColor);
							break;
						}
						solidBrush.Color = color;
						switch (FPixelShape)
						{
						case PixelShape.Square:
							graphics.FillRectangle(solidBrush, num, num2, psx, psy);
							break;
						case PixelShape.Round:
							graphics.FillEllipse(solidBrush, num, num2, psx, psy);
							break;
						case PixelShape.Shaped:
							if (color == FPixOnColor)
							{
								solidBrush.Color = FPixHalfColor;
								graphics.FillRectangle(solidBrush, num, num2, psx, psy);
								solidBrush.Color = color;
								graphics.FillEllipse(solidBrush, num, num2, psx, psy);
							}
							else
							{
								solidBrush.Color = color;
								graphics.FillRectangle(solidBrush, num, num2, psx, psy);
							}
							break;
						}
						num = num + psx + FPixelSpacing;
					}
					num = xpos;
					num2 = num2 + psy + FPixelSpacing;
				}
			}
		}

		private void DrawCharacters(Graphics graphics)
		{
			if (Text == null)
			{
				return;
			}
			int num = FBorderSpace + 1;
			int num2 = FBorderSpace + 1;
			int num3 = 1;
			bool flag = false;
			for (int i = 1; i <= FTextLines; i++)
			{
				for (int j = 1; j <= FNoOfChars; j++)
				{
					if (!flag && num3 > Text.Length)
					{
						flag = true;
					}
					int num4 = (!flag) ? Convert.ToInt32(Text[num3 - 1]) : 0;
					if (num4 < first_c)
					{
						num4 = first_c;
					}
					if (num4 > last_c)
					{
						num4 = last_c;
					}
					DrawMatrix(graphics, num, num2, num4);
					num = num + charw + FCharSpacing;
					num3++;
				}
				num2 = num2 + charh + FLineSpacing;
				num = FBorderSpace + 1;
			}
		}

		private void CalcHalfColor()
		{
			byte b = (byte)((int)FPixOnColor.B / 2);
			byte b2 = (byte)((int)FPixOnColor.G / 2);
			byte b3 = (byte)((int)FPixOnColor.R / 2);
			byte a = FPixOnColor.A;
			FPixHalfColor = Color.FromArgb(b + b2 * 256 + b3 * 65536 + a * 16777216);
		}

		private void CalcSize()
		{
			if (PixelSize.pixCustom == FPixelSize)
			{
				psx = FPixWidth;
				psy = FPixHeight;
			}
			else
			{
				psx = (int)(FPixelSize + 1);
				psy = psx;
				FPixWidth = psx;
				FPixHeight = psy;
			}
			switch (FDotMatrix)
			{
			case DotMatrix.mat5x7:
			case DotMatrix.Hitachi:
				pix_x = 5;
				pix_y = 7;
				break;
			case DotMatrix.Hitachi2:
				pix_x = 5;
				pix_y = 10;
				break;
			case DotMatrix.mat5x8:
				pix_x = 5;
				pix_y = 8;
				break;
			case DotMatrix.mat7x9:
				pix_x = 7;
				pix_y = 9;
				break;
			case DotMatrix.mat9x12:
				pix_x = 9;
				pix_y = 12;
				break;
			case DotMatrix.dos5x7:
				pix_x = 5;
				pix_y = 7;
				break;
			}
			charw = pix_x * psx + (pix_x - 1) * FPixelSpacing;
			charh = pix_y * psy + (pix_y - 1) * FPixelSpacing;
			base.Width = FBorderSpace * 2 + FCharSpacing * (FNoOfChars - 1) + charw * FNoOfChars + 2;
			base.Height = FBorderSpace * 2 + FLineSpacing * (FTextLines - 1) + charh * FTextLines + 2;
			FWidth = base.Width;
			FHeight = base.Height;
		}

		private void GetAsciiInterval()
		{
			switch (FDotMatrix)
			{
			case DotMatrix.mat5x7:
			case DotMatrix.Hitachi:
				first_c = 32;
				last_c = 223;
				break;
			case DotMatrix.Hitachi2:
				first_c = 32;
				last_c = 223;
				break;
			case DotMatrix.mat5x8:
				first_c = 32;
				last_c = 126;
				break;
			case DotMatrix.mat7x9:
				first_c = 32;
				last_c = 126;
				break;
			case DotMatrix.mat9x12:
				first_c = 32;
				last_c = 126;
				break;
			case DotMatrix.dos5x7:
				first_c = 0;
				last_c = 255;
				break;
			}
		}

		private void CalcCharSize()
		{
			if (PixelSize.pixCustom == FPixelSize)
			{
				psx = FPixWidth;
				psy = FPixHeight;
			}
			else
			{
				psx = (int)(FPixelSize + 1);
				psy = psx;
				FPixWidth = psx;
				FPixHeight = psy;
			}
			switch (FDotMatrix)
			{
			case DotMatrix.mat5x7:
			case DotMatrix.Hitachi:
				pix_x = 5;
				pix_y = 7;
				break;
			case DotMatrix.Hitachi2:
				pix_x = 5;
				pix_y = 10;
				break;
			case DotMatrix.mat5x8:
				pix_x = 5;
				pix_y = 8;
				break;
			case DotMatrix.mat7x9:
				pix_x = 7;
				pix_y = 9;
				break;
			case DotMatrix.mat9x12:
				pix_x = 9;
				pix_y = 12;
				break;
			case DotMatrix.dos5x7:
				pix_x = 5;
				pix_y = 7;
				break;
			}
			charw = pix_x * psx + (pix_x - 1) * FPixelSpacing;
			charh = pix_y * psy + (pix_y - 1) * FPixelSpacing;
			FNoOfChars = (base.Width - 2 * FBorderSpace + FCharSpacing) / (charw + FCharSpacing);
			FTextLines = (base.Height - 2 * FBorderSpace + FLineSpacing) / (charh + FLineSpacing);
			if (FNoOfChars < 1)
			{
				FNoOfChars = 1;
			}
			if (FTextLines < 1)
			{
				FTextLines = 1;
			}
			base.Width = FBorderSpace * 2 + FCharSpacing * (FNoOfChars - 1) + charw * FNoOfChars + 2;
			base.Height = FBorderSpace * 2 + FLineSpacing * (FTextLines - 1) + charh * FTextLines + 2;
			FWidth = base.Width;
			FHeight = base.Height;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			bool flag = false;
			if (base.Width != FWidth)
			{
				flag = true;
				FWidth = base.Width;
			}
			if (base.Height != FHeight)
			{
				flag = true;
				FHeight = base.Height;
			}
			GetAsciiInterval();
			if (flag)
			{
				CalcCharSize();
			}
			else
			{
				CalcSize();
			}
			using (SolidBrush solidBrush = new SolidBrush(FBackGround))
			{
				using (new Pen(solidBrush))
				{
					Rectangle rect = new Rectangle(0, 0, base.Width, base.Height);
					if (base.Visible)
					{
						e.Graphics.FillRectangle(solidBrush, rect);
						if (base.Enabled)
						{
							DrawCharacters(e.Graphics);
						}
					}
					else
					{
						solidBrush.Color = SystemColors.ButtonFace;
						e.Graphics.FillRectangle(solidBrush, rect);
					}
				}
			}
		}
	}
}
