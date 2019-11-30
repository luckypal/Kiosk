using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace wyDay.Controls
{
	public class AnimationControl : Control
	{
		private Image m_BaseImage;

		private int m_Rows = 1;

		private int m_Columns = 1;

		private bool m_SkipFirstFrame;

		private readonly Timer aniTimer = new Timer();

		private int m_AnimationInterval = 1000;

		private int columnOn = 1;

		private int rowOn = 1;

		private int frameWidth;

		private int frameHeight;

		private bool staticImage;

		private readonly float[][] ptsArray = new float[5][]
		{
			new float[5]
			{
				1f,
				0f,
				0f,
				0f,
				0f
			},
			new float[5]
			{
				0f,
				1f,
				0f,
				0f,
				0f
			},
			new float[5]
			{
				0f,
				0f,
				1f,
				0f,
				0f
			},
			new float[5],
			new float[5]
			{
				0f,
				0f,
				0f,
				0f,
				1f
			}
		};

		private readonly ImageAttributes imgAttributes = new ImageAttributes();

		public int AnimationInterval
		{
			get
			{
				return m_AnimationInterval;
			}
			set
			{
				m_AnimationInterval = value;
				aniTimer.Interval = m_AnimationInterval;
			}
		}

		public Image BaseImage
		{
			get
			{
				return m_BaseImage;
			}
			set
			{
				m_BaseImage = value;
				if (m_BaseImage != null)
				{
					if (staticImage)
					{
						base.Width = (frameWidth = m_BaseImage.Width);
						base.Height = (frameHeight = m_BaseImage.Height);
					}
					else
					{
						base.Width = (frameWidth = m_BaseImage.Width / m_Columns);
						base.Height = (frameHeight = m_BaseImage.Height / m_Rows);
					}
				}
				else
				{
					base.Width = (frameWidth = 0);
					base.Height = (frameHeight = 0);
				}
			}
		}

		public int Columns
		{
			get
			{
				return m_Columns;
			}
			set
			{
				m_Columns = value;
			}
		}

		public int Rows
		{
			get
			{
				return m_Rows;
			}
			set
			{
				m_Rows = value;
			}
		}

		public bool StaticImage
		{
			get
			{
				return staticImage;
			}
			set
			{
				staticImage = value;
			}
		}

		public bool CurrentlyAnimating => aniTimer.Enabled;

		public bool SkipFirstFrame
		{
			get
			{
				return m_SkipFirstFrame;
			}
			set
			{
				m_SkipFirstFrame = value;
			}
		}

		public AnimationControl()
		{
			aniTimer.Enabled = false;
			aniTimer.Tick += aniTimer_Tick;
			SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.FixedWidth | ControlStyles.FixedHeight | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
			SetStyle(ControlStyles.Selectable, value: false);
		}

		private void aniTimer_Tick(object sender, EventArgs e)
		{
			if (staticImage)
			{
				if (ptsArray[3][3] >= 1f)
				{
					StopAnimation();
					ptsArray[3][3] = 1f;
				}
				else
				{
					ptsArray[3][3] += 0.05f;
				}
			}
			else if (columnOn == m_Columns)
			{
				if (rowOn == m_Rows)
				{
					columnOn = ((!m_SkipFirstFrame) ? 1 : 2);
					rowOn = 1;
				}
				else
				{
					columnOn = 1;
					rowOn++;
				}
			}
			else
			{
				columnOn++;
			}
			Refresh();
		}

		public void StartAnimation()
		{
			if (!aniTimer.Enabled)
			{
				aniTimer.Start();
				if (staticImage)
				{
					ptsArray[3][3] = 0.05f;
				}
				else
				{
					columnOn++;
				}
				Refresh();
			}
		}

		public void StopAnimation()
		{
			aniTimer.Stop();
			columnOn = 1;
			rowOn = 1;
			Refresh();
			ptsArray[3][3] = 0f;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
			e.Graphics.InterpolationMode = InterpolationMode.Low;
			if (m_BaseImage != null)
			{
				if (staticImage)
				{
					imgAttributes.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
					e.Graphics.DrawImage(m_BaseImage, new Rectangle(0, 0, frameWidth, frameHeight), 0, 0, frameWidth, frameHeight, GraphicsUnit.Pixel, imgAttributes);
				}
				else
				{
					e.Graphics.DrawImage(m_BaseImage, new Rectangle(0, 0, frameWidth, frameHeight), new Rectangle((columnOn - 1) * frameWidth, (rowOn - 1) * frameHeight, frameWidth, frameHeight), GraphicsUnit.Pixel);
				}
			}
		}
	}
}
