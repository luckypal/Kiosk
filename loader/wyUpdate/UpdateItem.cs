using System.Drawing;
using System.Windows.Forms;
using wyDay.Controls;

namespace wyUpdate
{
	internal class UpdateItem
	{
		private static readonly Image ErrorImage = new Bitmap(typeof(UpdateItem), "cross.png");

		private static readonly Image SuccessImage = new Bitmap(typeof(UpdateItem), "tick.png");

		public static readonly Image ProgressImage = new Bitmap(typeof(UpdateItem), "loading-blue.png");

		public AnimationControl Animation = new AnimationControl();

		public Label Label = new Label
		{
			AutoSize = true
		};

		private UpdateItemStatus m_Status;

		private int m_Left;

		private int m_Top;

		public int AnimationWidth
		{
			get;
			set;
		}

		public int Left
		{
			get
			{
				return m_Left;
			}
			set
			{
				m_Left = value;
				Animation.Left = m_Left;
				Label.Left = m_Left + AnimationWidth + 5;
			}
		}

		public int Top
		{
			get
			{
				return m_Top;
			}
			set
			{
				m_Top = value;
				Animation.Top = m_Top;
				Label.Top = m_Top;
			}
		}

		public string Text
		{
			get
			{
				return Label.Text;
			}
			set
			{
				Label.Text = value;
			}
		}

		public bool Visible
		{
			get
			{
				return Animation.Visible;
			}
			set
			{
				Animation.Visible = value;
				Label.Visible = value;
			}
		}

		public UpdateItemStatus Status
		{
			get
			{
				return m_Status;
			}
			set
			{
				if (m_Status != value)
				{
					m_Status = value;
					switch (m_Status)
					{
					case UpdateItemStatus.Error:
						Animation.StopAnimation();
						Animation.StaticImage = true;
						Animation.Rows = 4;
						Animation.Columns = 8;
						Animation.AnimationInterval = 25;
						Animation.BaseImage = ErrorImage;
						Animation.StartAnimation();
						Label.Font = new Font(Label.Font, FontStyle.Regular);
						break;
					case UpdateItemStatus.Nothing:
						Animation.BaseImage = null;
						Animation.StartAnimation();
						break;
					case UpdateItemStatus.Working:
						Animation.StopAnimation();
						Animation.StaticImage = false;
						Animation.Rows = 1;
						Animation.Columns = 18;
						Animation.AnimationInterval = 46;
						Animation.BaseImage = ProgressImage;
						Animation.StartAnimation();
						Label.Font = new Font(Label.Font, FontStyle.Bold);
						break;
					case UpdateItemStatus.Success:
						Animation.StopAnimation();
						Animation.StaticImage = true;
						Animation.Rows = 4;
						Animation.Columns = 8;
						Animation.AnimationInterval = 25;
						Animation.BaseImage = SuccessImage;
						Animation.StartAnimation();
						Label.Font = new Font(Label.Font, FontStyle.Regular);
						break;
					}
				}
			}
		}

		public void Show()
		{
			Visible = true;
		}

		public void Hide()
		{
			Visible = false;
		}

		public void Clear()
		{
			m_Status = UpdateItemStatus.Nothing;
			Label.Text = string.Empty;
		}
	}
}
