using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace wyDay.Controls
{
	public class LinkLabel2 : Control
	{
		private Font hoverFont;

		private Rectangle textRect;

		private bool isHovered;

		private bool keyAlreadyProcessed;

		private Image image;

		private int imageRightPad = 8;

		[DefaultValue(8)]
		public int ImageRightPad
		{
			get
			{
				return imageRightPad;
			}
			set
			{
				imageRightPad = value;
				RefreshTextRect();
				Invalidate();
			}
		}

		[DefaultValue(null)]
		public Image Image
		{
			get
			{
				return image;
			}
			set
			{
				image = value;
				RefreshTextRect();
				Invalidate();
			}
		}

		[DefaultValue(true)]
		public bool HoverUnderline
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool UseSystemColor
		{
			get;
			set;
		}

		public Color RegularColor
		{
			get;
			set;
		}

		public Color HoverColor
		{
			get;
			set;
		}

		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				RefreshTextRect();
				Invalidate();
			}
		}

		[DllImport("user32.dll")]
		public static extern int LoadCursor(int hInstance, int lpCursorName);

		[DllImport("user32.dll")]
		public static extern int SetCursor(int hCursor);

		public LinkLabel2()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.FixedWidth | ControlStyles.FixedHeight | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
			SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, value: false);
			hoverFont = new Font(Font, FontStyle.Underline);
			ForeColor = Color.White;
			UseSystemColor = true;
			HoverUnderline = true;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Focus();
			}
			base.OnMouseDown(e);
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			isHovered = true;
			Invalidate();
			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			isHovered = false;
			Invalidate();
			base.OnMouseLeave(e);
		}

		protected override void OnMouseMove(MouseEventArgs mevent)
		{
			base.OnMouseMove(mevent);
			if (mevent.Button == MouseButtons.None)
			{
				return;
			}
			if (!base.ClientRectangle.Contains(mevent.Location))
			{
				if (isHovered)
				{
					isHovered = false;
					Invalidate();
				}
			}
			else if (!isHovered)
			{
				isHovered = true;
				Invalidate();
			}
		}

		protected override void OnGotFocus(EventArgs e)
		{
			Invalidate();
			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			keyAlreadyProcessed = false;
			Invalidate();
			base.OnLostFocus(e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (!keyAlreadyProcessed && e.KeyCode == Keys.Return)
			{
				keyAlreadyProcessed = true;
				OnClick(e);
			}
			base.OnKeyDown(e);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			keyAlreadyProcessed = false;
			base.OnKeyUp(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (isHovered && e.Clicks == 1 && (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle))
			{
				OnClick(e);
			}
			base.OnMouseUp(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
			e.Graphics.InterpolationMode = InterpolationMode.Low;
			if (image != null)
			{
				e.Graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
			}
			TextRenderer.DrawText(e.Graphics, Text, (isHovered && HoverUnderline) ? hoverFont : Font, textRect, UseSystemColor ? ForeColor : (isHovered ? HoverColor : RegularColor), TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
			if (Focused && ShowFocusCues)
			{
				ControlPaint.DrawFocusRectangle(e.Graphics, base.ClientRectangle);
			}
		}

		protected override void OnFontChanged(EventArgs e)
		{
			hoverFont = new Font(Font, Font.Style | FontStyle.Underline);
			RefreshTextRect();
			base.OnFontChanged(e);
		}

		private void RefreshTextRect()
		{
			textRect = new Rectangle(Point.Empty, TextRenderer.MeasureText(Text, Font, base.Size, TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine));
			int width = textRect.Width + 1;
			int height = textRect.Height + 1;
			if (image != null)
			{
				width = textRect.Width + 1 + image.Width + imageRightPad;
				textRect.X += image.Width + imageRightPad;
				if (image.Height > textRect.Height)
				{
					height = image.Height + 1;
					textRect.Y += (image.Height - textRect.Height) / 2;
				}
			}
			base.Size = new Size(width, height);
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 32)
			{
				SetCursor(LoadCursor(0, 32649));
				m.Result = IntPtr.Zero;
			}
			else
			{
				base.WndProc(ref m);
			}
		}
	}
}
