using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using wyDay.Controls;
using wyUpdate.Common;

namespace wyUpdate
{
	internal class PanelDisplay : ContainerControl
	{
		private const int m_LeftPad = 14;

		private const int m_RightPad = 14;

		private const int m_TopPad = 14;

		private const int m_DescriptionOffset = 10;

		private const int m_HeaderHeight = 100;

		public Image SideImage;

		public Image TopImage;

		public ImageAlign HeaderImageAlign;

		public int HeaderIndent = 14;

		public Color HeaderTextColor = Color.White;

		public bool HideHeaderDivider;

		private string m_Title;

		private Font m_TitleFont;

		private Rectangle m_TitleRect;

		private string m_Description;

		private Rectangle m_DescriptionRect;

		private string m_Body;

		private Rectangle m_BodyRect;

		private string m_BottomText;

		private Rectangle m_BottomRect;

		public string ErrorDetails;

		private readonly RichTextBoxEx messageBox = new RichTextBoxEx();

		private readonly Windows7ProgressBar progressBar = new Windows7ProgressBar();

		private int m_Progress;

		private string m_ProgressStatus;

		private Rectangle m_ProgressStatusRect;

		public FrameType TypeofFrame;

		private readonly AnimationControl aniWorking;

		private LinkLabel2 noUpdateAvailableLink;

		private string noUpdateAvailableURL;

		private Button errorDetailsButton;

		public bool ShowChecklist;

		public UpdateItem[] UpdateItems = new UpdateItem[4];

		public int Progress
		{
			set
			{
				m_Progress = value;
				if (progressBar != null)
				{
					if (m_Progress > 100)
					{
						m_Progress = 100;
					}
					else if (m_Progress < 0)
					{
						m_Progress = 0;
					}
					progressBar.Value = m_Progress;
				}
			}
		}

		public string ProgressStatus
		{
			get
			{
				return m_ProgressStatus;
			}
			set
			{
				m_ProgressStatus = value;
				if (progressBar != null && progressBar.Visible)
				{
					m_ProgressStatusRect = UpdateTextSize(m_ProgressStatus, new Padding(14, progressBar.Bottom + 4, 14, 0), TextFormatFlags.SingleLine | TextFormatFlags.WordEllipsis, Font);
					Invalidate(new Rectangle(14, progressBar.Bottom, base.Width - 14 - 14, base.Height - progressBar.Bottom));
				}
			}
		}

		public void PauseProgressBar()
		{
			progressBar.State = ProgressBarState.Pause;
		}

		public void UnPauseProgressBar()
		{
			progressBar.State = ProgressBarState.Normal;
		}

		public void AppendText(string plaintext)
		{
			if (!string.IsNullOrEmpty(plaintext))
			{
				messageBox.Select(messageBox.Text.Length, 0);
				messageBox.SelectedText = plaintext;
			}
		}

		public void AppendRichText(string rtfText)
		{
			messageBox.Select(messageBox.Text.Length, 0);
			messageBox.SelectedRtf = messageBox.SterilizeRTF(rtfText);
		}

		public void AppendAndBoldText(string plaintext)
		{
			messageBox.Select(messageBox.Text.Length, 0);
			Font selectionFont = messageBox.SelectionFont;
			messageBox.SelectionFont = new Font(messageBox.SelectionFont, FontStyle.Bold);
			messageBox.SelectedText = plaintext;
			messageBox.SelectionFont = selectionFont;
		}

		public void ClearText()
		{
			messageBox.Clear();
		}

		public string GetChanges(bool rtf)
		{
			if (!rtf)
			{
				return messageBox.Text;
			}
			return messageBox.Rtf;
		}

		public PanelDisplay(int width, int height)
		{
			base.Width = width - 14;
			base.Height = height - 150;
			SetStyle(ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
			progressBar.Location = new Point(14, base.Height - 60);
			progressBar.Size = new Size(base.Width - 14 - 14, 20);
			progressBar.Maximum = 100;
			progressBar.Value = 0;
			progressBar.Visible = false;
			base.Controls.Add(progressBar);
			messageBox.Location = new Point(14, 134);
			messageBox.Multiline = true;
			messageBox.Size = new Size(base.Width - 14 - 14, 0);
			messageBox.ReadOnly = true;
			messageBox.ScrollBars = RichTextBoxScrollBars.Vertical;
			messageBox.BackColor = SystemColors.Window;
			messageBox.Visible = false;
			base.Controls.Add(messageBox);
			for (int i = 0; i < UpdateItems.Length; i++)
			{
				UpdateItems[i] = new UpdateItem
				{
					AnimationWidth = 16,
					Visible = false,
					Left = 45
				};
				UpdateItems[i].Label.ForeColor = Color.White;
				base.Controls.Add(UpdateItems[i].Animation);
				base.Controls.Add(UpdateItems[i].Label);
			}
			aniWorking = new AnimationControl
			{
				Columns = 18,
				Rows = 1,
				AnimationInterval = 46,
				Visible = false,
				Location = new Point(base.Width / 2 - 25, base.Height / 2),
				StaticImage = false,
				BaseImage = UpdateItem.ProgressImage
			};
			base.Controls.Add(aniWorking);
		}

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

		public void ChangePanel(FrameType panType, string title, string description, string body, string bottom)
		{
			SendMessage(base.Handle, 11u, 0, 0);
			TypeofFrame = panType;
			m_Title = title;
			m_Description = description;
			m_Body = body;
			m_BottomText = bottom;
			if (panType == FrameType.WelcomeFinish)
			{
				if (ErrorDetails == null)
				{
					messageBox.Hide();
				}
				else
				{
					messageBox.Clear();
					AppendText(ErrorDetails);
				}
				progressBar.Hide();
				HideAnimations();
				m_TitleFont = new Font(Font.FontFamily, 12f, FontStyle.Bold);
			}
			else
			{
				m_TitleFont = new Font(Font.FontFamily, Font.Size + 1f, FontStyle.Bold);
			}
			UpdateTextRectangles();
			progressBar.ShowInTaskbar = false;
			switch (panType)
			{
			case FrameType.TextInfo:
				messageBox.Show();
				progressBar.Hide();
				HideAnimations();
				break;
			case FrameType.Update:
				messageBox.Hide();
				progressBar.Show();
				progressBar.ContainerControl = (Form)base.TopLevelControl;
				progressBar.ShowInTaskbar = ShowChecklist;
				if (ShowChecklist)
				{
					if (aniWorking.Visible)
					{
						aniWorking.Hide();
						aniWorking.StopAnimation();
					}
					for (int i = 0; i < UpdateItems.Length; i++)
					{
						UpdateItems[i].Clear();
						UpdateItems[i].Show();
					}
				}
				else
				{
					aniWorking.StartAnimation();
					aniWorking.Show();
				}
				break;
			}
			SendMessage(base.Handle, 11u, 1, 0);
			Refresh();
		}

		public void SetNoUpdateAvailableLink(string text, string link)
		{
			noUpdateAvailableLink = new LinkLabel2
			{
				Text = text,
				Visible = false,
				BackColor = Color.White
			};
			noUpdateAvailableLink.Click += NoUpdateAvailableLink_Click;
			noUpdateAvailableURL = link;
			base.Controls.Add(noUpdateAvailableLink);
		}

		private void NoUpdateAvailableLink_Click(object sender, EventArgs e)
		{
			Process.Start(noUpdateAvailableURL);
		}

		public void SetUpErrorDetails(string detailsButton)
		{
			errorDetailsButton = new Button
			{
				Text = detailsButton,
				FlatStyle = FlatStyle.System,
				Visible = false,
				AutoSize = true,
				Padding = new Padding(6, 0, 6, 0)
			};
			errorDetailsButton.Click += errorDetailsButton_Click;
			base.Controls.Add(errorDetailsButton);
		}

		private void errorDetailsButton_Click(object sender, EventArgs e)
		{
			errorDetailsButton.Visible = false;
			messageBox.Show();
		}

		private void UpdateTextRectangles()
		{
			int num = 14;
			int num2 = HeaderIndent;
			int num3 = 14;
			if (TypeofFrame == FrameType.WelcomeFinish && SideImage != null)
			{
				num += SideImage.Width;
			}
			else if (TypeofFrame != 0 && TopImage != null)
			{
				switch (HeaderImageAlign)
				{
				case ImageAlign.Left:
					num2 += TopImage.Width;
					break;
				case ImageAlign.Right:
					num3 += TopImage.Width;
					break;
				}
			}
			if (TypeofFrame == FrameType.WelcomeFinish)
			{
				m_TitleRect = UpdateTextSize(m_Title, new Padding(num, 14, 14, 0), TextFormatFlags.WordBreak, m_TitleFont);
				m_DescriptionRect = UpdateTextSize(m_Description, new Padding(num, m_TitleRect.Bottom + 14, 14, 0), TextFormatFlags.NoClipping | TextFormatFlags.WordBreak, Font);
			}
			else
			{
				m_TitleRect = UpdateTextSize(m_Title, new Padding(num2, 14, num3, 0), TextFormatFlags.WordEllipsis, m_TitleFont);
				m_DescriptionRect = UpdateTextSize(m_Description, new Padding(num2 + 10, m_TitleRect.Bottom, num3, 0), TextFormatFlags.WordEllipsis, Font);
				m_TitleRect.Location = new Point(num2, 50 - (m_TitleRect.Height + m_DescriptionRect.Height) / 2);
				m_DescriptionRect.Location = new Point(num2 + 10, m_TitleRect.Bottom);
			}
			m_BottomRect = UpdateTextSize(m_BottomText, new Padding(num, 0, 14, 9), TextFormatFlags.WordBreak, Font, ContentAlignment.BottomRight);
			if (TypeofFrame != 0)
			{
				m_BodyRect = UpdateTextSize(m_Body, new Padding(num, 114, 14, 0), TextFormatFlags.WordBreak, Font);
				if (TypeofFrame == FrameType.TextInfo)
				{
					messageBox.Top = m_BodyRect.Bottom + 5;
					messageBox.Height = base.Height - messageBox.Top - 5 - (base.Bottom - m_BottomRect.Top);
				}
				if (ShowChecklist)
				{
					for (int i = 0; i < UpdateItems.Length; i++)
					{
						UpdateItems[i].Top = m_BodyRect.Bottom + 25 + 30 * i;
					}
				}
			}
			else if (noUpdateAvailableLink != null)
			{
				noUpdateAvailableLink.Location = new Point(m_DescriptionRect.Left, m_DescriptionRect.Bottom + 20);
				noUpdateAvailableLink.Visible = true;
			}
			else if (ErrorDetails != null)
			{
				errorDetailsButton.Location = new Point(base.Width - 14 - errorDetailsButton.Width, m_DescriptionRect.Bottom + 5);
				errorDetailsButton.Visible = true;
				messageBox.Location = new Point(num, m_DescriptionRect.Bottom + 5);
				messageBox.Size = new Size(base.Width - num - 14, base.Height - messageBox.Top - 5 - (base.Bottom - m_BottomRect.Top));
			}
		}

		private Rectangle UpdateTextSize(string text, Padding padding, TextFormatFlags flags, Font font)
		{
			return UpdateTextSize(text, padding, flags, font, ContentAlignment.TopLeft);
		}

		private Rectangle UpdateTextSize(string text, Padding padding, TextFormatFlags flags, Font font, ContentAlignment alignment)
		{
			if (font == null)
			{
				font = Font;
			}
			Size size = TextRenderer.MeasureText(text, font, new Size(base.Width - padding.Left - padding.Right, 1), flags | TextFormatFlags.NoPrefix);
			if (alignment == ContentAlignment.BottomRight)
			{
				return new Rectangle(new Point(base.Width - size.Width - padding.Right, base.Height - size.Height - padding.Bottom), size);
			}
			return new Rectangle(new Point(padding.Left, padding.Top), size);
		}

		private void HideAnimations()
		{
			for (int i = 0; i < UpdateItems.Length; i++)
			{
				UpdateItems[i].Hide();
			}
			aniWorking.Hide();
			aniWorking.StopAnimation();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
			e.Graphics.InterpolationMode = InterpolationMode.Low;
			if (TypeofFrame == FrameType.WelcomeFinish)
			{
				e.Graphics.FillRectangle(Brushes.Black, 0, 316, base.Width, base.Height - 316);
				DrawSide(e.Graphics);
			}
			else
			{
				e.Graphics.FillRectangle(Brushes.Black, 0, HideHeaderDivider ? 57 : 59, base.Width, base.Height - (HideHeaderDivider ? 57 : 59));
				DrawTop(e.Graphics);
			}
			DrawMain(e.Graphics);
		}

		private void DrawSide(Graphics gr)
		{
			try
			{
				Rectangle rect = new Rectangle(0, 0, SideImage.Width, SideImage.Height);
				gr.DrawImage(SideImage, rect);
				gr.ExcludeClip(rect);
			}
			catch
			{
			}
		}

		private void DrawTop(Graphics gr)
		{
			try
			{
				Rectangle rect = (HeaderImageAlign == ImageAlign.Right) ? new Rectangle(base.Width - TopImage.Width, 0, TopImage.Width, TopImage.Height) : new Rectangle(0, 0, TopImage.Width, TopImage.Height);
				gr.DrawImage(TopImage, rect);
				gr.ExcludeClip(rect);
			}
			catch
			{
			}
			gr.FillRectangle(Brushes.Black, 0, 0, base.Width, 98);
			gr.ResetClip();
		}

		private void DrawMain(Graphics gr)
		{
			if (TypeofFrame == FrameType.WelcomeFinish)
			{
				TextRenderer.DrawText(gr, m_Title, m_TitleFont, m_TitleRect, Color.White, TextFormatFlags.NoPrefix | TextFormatFlags.WordBreak);
				TextRenderer.DrawText(gr, m_Description, Font, m_DescriptionRect, Color.White, TextFormatFlags.NoPrefix | TextFormatFlags.WordBreak);
			}
			else
			{
				TextRenderer.DrawText(gr, m_Body, Font, m_BodyRect, Color.White, TextFormatFlags.NoPrefix | TextFormatFlags.WordBreak);
			}
			if (!string.IsNullOrEmpty(m_BottomText))
			{
				TextRenderer.DrawText(gr, m_BottomText, Font, m_BottomRect, Color.White, TextFormatFlags.NoPrefix | TextFormatFlags.WordBreak);
			}
			if (!string.IsNullOrEmpty(m_ProgressStatus) && progressBar.Visible)
			{
				TextRenderer.DrawText(gr, m_ProgressStatus, Font, m_ProgressStatusRect, Color.White, TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.WordEllipsis);
			}
		}

		protected static void Draw3DLine(Graphics gr, int x1, int x2, int y1)
		{
			gr.DrawLine(SystemPens.ControlDark, x1, y1, x2, y1);
			gr.DrawLine(SystemPens.ControlLightLight, x1, y1 + 1, x2, y1 + 1);
		}

		private int DrawBranding(Graphics gr, int x, int midPointY)
		{
			SizeF sizeF = gr.MeasureString("wyUpdate", Font);
			midPointY -= (int)((double)sizeF.Height / 2.0);
			gr.DrawString("wyUpdate", Font, SystemBrushes.ControlLightLight, new PointF(x, midPointY));
			gr.DrawString("wyUpdate", Font, SystemBrushes.ControlDark, new PointF(x - 1, midPointY - 1));
			return (int)sizeF.Width + x;
		}
	}
}
