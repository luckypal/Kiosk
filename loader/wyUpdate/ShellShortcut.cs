using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using wyUpdate.Common;

namespace wyUpdate
{
	public class ShellShortcut : IDisposable
	{
		private const int INFOTIPSIZE = 1024;

		private const int MAX_PATH = 260;

		private IShellLinkW m_Link;

		private readonly string m_sPath;

		public string Arguments
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(1024);
				m_Link.GetArguments(stringBuilder, stringBuilder.Capacity);
				return stringBuilder.ToString();
			}
			set
			{
				m_Link.SetArguments(value ?? string.Empty);
			}
		}

		public string Description
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(1024);
				m_Link.GetDescription(stringBuilder, stringBuilder.Capacity);
				return stringBuilder.ToString();
			}
			set
			{
				m_Link.SetDescription(value ?? string.Empty);
			}
		}

		public string WorkingDirectory
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(260);
				m_Link.GetWorkingDirectory(stringBuilder, stringBuilder.Capacity);
				return stringBuilder.ToString();
			}
			set
			{
				m_Link.SetWorkingDirectory(value ?? string.Empty);
			}
		}

		public string Path
		{
			get
			{
				WIN32_FIND_DATAW pfd = default(WIN32_FIND_DATAW);
				StringBuilder stringBuilder = new StringBuilder(260);
				m_Link.GetPath(stringBuilder, stringBuilder.Capacity, out pfd, SLGP_FLAGS.SLGP_UNCPRIORITY);
				return stringBuilder.ToString();
			}
			set
			{
				m_Link.SetPath(value ?? string.Empty);
			}
		}

		public string IconPath
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(260);
				m_Link.GetIconLocation(stringBuilder, stringBuilder.Capacity, out int _);
				return stringBuilder.ToString();
			}
			set
			{
				m_Link.SetIconLocation(value, IconIndex);
			}
		}

		public int IconIndex
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(260);
				m_Link.GetIconLocation(stringBuilder, stringBuilder.Capacity, out int piIcon);
				return piIcon;
			}
			set
			{
				m_Link.SetIconLocation(IconPath, value);
			}
		}

		public WindowStyle WindowStyle
		{
			get
			{
				m_Link.GetShowCmd(out int piShowCmd);
				return (WindowStyle)piShowCmd;
			}
			set
			{
				m_Link.SetShowCmd((int)value);
			}
		}

		public Keys Hotkey
		{
			get
			{
				m_Link.GetHotkey(out short pwHotkey);
				return (Keys)(((pwHotkey & 0xFF00) << 8) | (pwHotkey & 0xFF));
			}
			set
			{
				if ((value & Keys.Modifiers) == 0)
				{
					throw new ArgumentException("Hotkey must include a modifier key.");
				}
				short hotkey = (short)(((int)(value & Keys.Modifiers) >> 8) | (int)(value & Keys.KeyCode));
				m_Link.SetHotkey(hotkey);
			}
		}

		public object ShellLink => m_Link;

		public ShellShortcut(string linkPath)
		{
			m_sPath = linkPath;
			m_Link = (IShellLinkW)new ShellLink();
			if (File.Exists(linkPath))
			{
				File.Delete(linkPath);
			}
		}

		public void Dispose()
		{
			if (m_Link != null)
			{
				Marshal.ReleaseComObject(m_Link);
				m_Link = null;
			}
		}

		public void Save()
		{
			IPersistFile persistFile = (IPersistFile)m_Link;
			persistFile.Save(m_sPath, fRemember: true);
		}
	}
}
