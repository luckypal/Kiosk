using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GLib.Forms
{
	public class GradientPanel : Panel
	{
		public enum Alignement
		{
			Left,
			Center,
			Right
		}

		private Color mStartColor;

		private Color mEndColor;

		private string _Text;

		private Alignement _TAlign = Alignement.Left;

		private IContainer components = null;

		public Color PageStartColor
		{
			get
			{
				return mStartColor;
			}
			set
			{
				mStartColor = value;
				Invalidate();
				PaintGradient();
			}
		}

		public Color PageEndColor
		{
			get
			{
				return mEndColor;
			}
			set
			{
				mEndColor = value;
				Invalidate();
				PaintGradient();
			}
		}

		public string Title
		{
			get
			{
				return _Text;
			}
			set
			{
				_Text = value;
				Invalidate();
				PaintGradient();
			}
		}

		public Alignement Title_Alignement
		{
			get
			{
				return _TAlign;
			}
			set
			{
				_TAlign = value;
				Invalidate();
				PaintGradient();
			}
		}

		public GradientPanel()
		{
			InitializeComponent();
			PaintGradient();
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
		}

		private void PaintGradient()
		{
			LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(base.Width, base.Height), PageStartColor, PageEndColor);
			Bitmap bitmap = new Bitmap(base.Width, base.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.FillRectangle(brush, new Rectangle(0, 0, base.Width, base.Height));
			BackgroundImage = bitmap;
			BackgroundImageLayout = ImageLayout.Stretch;
			SizeF sizeF = graphics.MeasureString(_Text, Font);
			switch (_TAlign)
			{
			case Alignement.Right:
				graphics.DrawString(_Text, Font, Brushes.Black, (float)base.Width - sizeF.Width - 32f, (float)(base.Height / 2) - sizeF.Height / 2f);
				break;
			case Alignement.Center:
				graphics.DrawString(_Text, Font, Brushes.Black, (float)(base.Width / 2) - sizeF.Width / 2f, (float)(base.Height / 2) - sizeF.Height / 2f);
				break;
			default:
				graphics.DrawString(_Text, Font, Brushes.Black, 32f, (float)(base.Height / 2) - sizeF.Height / 2f);
				break;
			}
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
			SuspendLayout();
			ResumeLayout(false);
		}
	}
}
