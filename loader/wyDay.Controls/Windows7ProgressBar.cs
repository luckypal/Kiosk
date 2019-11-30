using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

namespace wyDay.Controls
{
	[ToolboxBitmap(typeof(ProgressBar))]
	public class Windows7ProgressBar : ProgressBar
	{
		private bool showInTaskbar;

		private ProgressBarState m_State = ProgressBarState.Normal;

		private ContainerControl ownerForm;

		public ContainerControl ContainerControl
		{
			get
			{
				return ownerForm;
			}
			set
			{
				ownerForm = value;
				if (!ownerForm.Visible)
				{
					((Form)ownerForm).Shown += Windows7ProgressBar_Shown;
				}
			}
		}

		public override ISite Site
		{
			set
			{
				base.Site = value;
				if (value != null)
				{
					IDesignerHost designerHost = value.GetService(typeof(IDesignerHost)) as IDesignerHost;
					if (designerHost != null)
					{
						IComponent rootComponent = designerHost.RootComponent;
						ContainerControl = (rootComponent as ContainerControl);
					}
				}
			}
		}

		[DefaultValue(false)]
		public bool ShowInTaskbar
		{
			get
			{
				return showInTaskbar;
			}
			set
			{
				if (showInTaskbar == value)
				{
					return;
				}
				showInTaskbar = value;
				if (ownerForm != null)
				{
					if (Style != ProgressBarStyle.Marquee)
					{
						SetValueInTB();
					}
					SetStateInTB();
				}
			}
		}

		public new int Value
		{
			get
			{
				return base.Value;
			}
			set
			{
				base.Value = value;
				SetValueInTB();
			}
		}

		public new ProgressBarStyle Style
		{
			get
			{
				return base.Style;
			}
			set
			{
				base.Style = value;
				if (showInTaskbar && ownerForm != null)
				{
					SetStateInTB();
				}
			}
		}

		[DefaultValue(ProgressBarState.Normal)]
		public ProgressBarState State
		{
			get
			{
				return m_State;
			}
			set
			{
				m_State = value;
				bool flag = Style == ProgressBarStyle.Marquee;
				if (flag)
				{
					Style = ProgressBarStyle.Blocks;
				}
				Windows7Taskbar.SendMessage(base.Handle, 1040, (int)value, 0);
				if (flag)
				{
					SetValueInTB();
				}
				else
				{
					SetStateInTB();
				}
			}
		}

		public Windows7ProgressBar()
		{
		}

		public Windows7ProgressBar(ContainerControl parentControl)
		{
			ContainerControl = parentControl;
		}

		private void Windows7ProgressBar_Shown(object sender, EventArgs e)
		{
			if (ShowInTaskbar)
			{
				if (Style != ProgressBarStyle.Marquee)
				{
					SetValueInTB();
				}
				SetStateInTB();
			}
			((Form)ownerForm).Shown -= Windows7ProgressBar_Shown;
		}

		public new void Increment(int value)
		{
			base.Increment(value);
			SetValueInTB();
		}

		public new void PerformStep()
		{
			base.PerformStep();
			SetValueInTB();
		}

		private void SetValueInTB()
		{
			if (showInTaskbar)
			{
				ulong maximum = (ulong)(base.Maximum - base.Minimum);
				ulong current = (ulong)(Value - base.Minimum);
				Windows7Taskbar.SetProgressValue(ownerForm.Handle, current, maximum);
			}
		}

		private void SetStateInTB()
		{
			if (ownerForm != null)
			{
				ThumbnailProgressState state = ThumbnailProgressState.Normal;
				if (!showInTaskbar)
				{
					state = ThumbnailProgressState.NoProgress;
				}
				else if (Style == ProgressBarStyle.Marquee)
				{
					state = ThumbnailProgressState.Indeterminate;
				}
				else if (m_State == ProgressBarState.Error)
				{
					state = ThumbnailProgressState.Error;
				}
				else if (m_State == ProgressBarState.Pause)
				{
					state = ThumbnailProgressState.Paused;
				}
				Windows7Taskbar.SetProgressState(ownerForm.Handle, state);
			}
		}
	}
}
