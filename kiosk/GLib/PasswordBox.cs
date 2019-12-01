#define DEBUG
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GLib
{
	[ToolboxBitmap(typeof(PasswordBox), "images.PasswordBox.bmp")]
	public class PasswordBox : TextBox
	{
		private const int WS_MAXIMIZEBOX = 65536;

		private const int ES_PASSWORD = 32;

		private const int ES_AUTOHSCROLL = 128;

		private const int WM_CHAR = 258;

		private StringBuilder _internalBuffer = new StringBuilder();

		private new bool Multiline => false;

		[Localizable(false)]
		public override string Text
		{
			get
			{
				return _internalBuffer.ToString();
			}
			set
			{
				base.Text = ((value == null) ? value : new string(' ', value.Length));
				_internalBuffer = new StringBuilder(value, MaxLength);
				Debug.Assert(base.TextLength == _internalBuffer.Length);
			}
		}

		public override int TextLength
		{
			get
			{
				Debug.Assert(base.TextLength == _internalBuffer.Length, "base.TextLength != _internalBuffer.Length");
				return _internalBuffer.Length;
			}
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				if (!base.DesignMode)
				{
					createParams.Style |= 32;
				}
				return createParams;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
			}
			base.Dispose(disposing);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == (Keys)131158 || keyData == (Keys.LButton | Keys.MButton | Keys.Back | Keys.Space | Keys.Shift))
			{
				IDataObject dataObject = Clipboard.GetDataObject();
				if (dataObject.GetDataPresent(DataFormats.Text))
				{
					ReplaceSelection((string)dataObject.GetData(DataFormats.Text));
					return true;
				}
			}
			else
			{
				if (keyData == (Keys)131139 || keyData == (Keys.LButton | Keys.MButton | Keys.Back | Keys.Space | Keys.Control) || keyData == (Keys.RButton | Keys.MButton | Keys.Back | Keys.Space | Keys.Shift))
				{
					return true;
				}
				if (keyData == Keys.Delete)
				{
					DeleteChar();
				}
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		protected override bool ProcessKeyMessage(ref Message m)
		{
			bool flag = true;
			int msg = m.Msg;
			if (msg == 258)
			{
				Keys keys = (Keys)(int)m.WParam;
				if (keys == Keys.Back)
				{
					ProcessChar('\b');
				}
				else
				{
					char c = (char)(int)m.WParam;
					if (!char.IsControl(c))
					{
						m.WParam = (IntPtr)32;
						ProcessChar(c);
					}
				}
			}
			return ProcessKeyEventArgs(ref m);
		}

		private void DeleteChar()
		{
			int selectionStart = base.SelectionStart;
			int selectionLength = SelectionLength;
			if (selectionLength > 0)
			{
				_internalBuffer.Remove(selectionStart, selectionLength);
			}
			else if (selectionStart < _internalBuffer.Length - 1)
			{
				_internalBuffer.Remove(selectionStart, 1);
			}
		}

		private void ReplaceSelection(string s)
		{
			int selectionStart = base.SelectionStart;
			int selectionLength = SelectionLength;
			if (selectionLength > 0)
			{
				_internalBuffer.Remove(selectionStart, selectionLength);
			}
			Text = _internalBuffer.Insert(selectionStart, s).ToString();
			Select(selectionStart + s.Length, 0);
		}

		private void ProcessChar(char c)
		{
			int selectionStart = base.SelectionStart;
			int selectionLength = SelectionLength;
			if (selectionLength > 0)
			{
				_internalBuffer.Remove(selectionStart, selectionLength);
			}
			else if (c == '\b')
			{
				if (selectionStart > 0)
				{
					_internalBuffer.Remove(selectionStart - 1, 1);
				}
				return;
			}
			_internalBuffer.Insert(selectionStart, c);
		}
	}
}
