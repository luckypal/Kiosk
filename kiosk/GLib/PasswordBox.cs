// Decompiled with JetBrains decompiler
// Type: GLib.PasswordBox
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GLib
{
  [ToolboxBitmap(typeof (PasswordBox), "images.PasswordBox.bmp")]
  public class PasswordBox : TextBox
  {
    private StringBuilder _internalBuffer = new StringBuilder();
    private const int WS_MAXIMIZEBOX = 65536;
    private const int ES_PASSWORD = 32;
    private const int ES_AUTOHSCROLL = 128;
    private const int WM_CHAR = 258;

    private new bool Multiline
    {
      get
      {
        return false;
      }
    }

    [Localizable(false)]
    public override string Text
    {
      get
      {
        return this._internalBuffer.ToString();
      }
      set
      {
        base.Text = value == null ? value : new string(' ', value.Length);
        this._internalBuffer = new StringBuilder(value, this.MaxLength);
        Debug.Assert(base.TextLength == this._internalBuffer.Length);
      }
    }

    public override int TextLength
    {
      get
      {
        Debug.Assert(base.TextLength == this._internalBuffer.Length, "base.TextLength != _internalBuffer.Length");
        return this._internalBuffer.Length;
      }
    }

    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams createParams = base.CreateParams;
        if (!this.DesignMode)
          createParams.Style |= 32;
        return createParams;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (!disposing)
        ;
      base.Dispose(disposing);
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      if (keyData == (Keys.V | Keys.Control) || keyData == (Keys.Insert | Keys.Shift))
      {
        IDataObject dataObject = Clipboard.GetDataObject();
        if (dataObject.GetDataPresent(DataFormats.Text))
        {
          this.ReplaceSelection((string) dataObject.GetData(DataFormats.Text));
          return true;
        }
      }
      else
      {
        if (keyData == (Keys.C | Keys.Control) || keyData == (Keys.Insert | Keys.Control) || keyData == (Keys.Delete | Keys.Shift))
          return true;
        if (keyData == Keys.Delete)
          this.DeleteChar();
      }
      return base.ProcessCmdKey(ref msg, keyData);
    }

    protected override bool ProcessKeyMessage(ref Message m)
    {
      if (m.Msg == 258)
      {
        if ((int) m.WParam == 8)
        {
          this.ProcessChar('\b');
        }
        else
        {
          char wparam = (char) (int) m.WParam;
          if (!char.IsControl(wparam))
          {
            m.WParam = (IntPtr) 32;
            this.ProcessChar(wparam);
          }
        }
      }
      return this.ProcessKeyEventArgs(ref m);
    }

    private void DeleteChar()
    {
      int selectionStart = this.SelectionStart;
      int selectionLength = this.SelectionLength;
      if (selectionLength > 0)
        this._internalBuffer.Remove(selectionStart, selectionLength);
      else if (selectionStart < this._internalBuffer.Length - 1)
        this._internalBuffer.Remove(selectionStart, 1);
    }

    private void ReplaceSelection(string s)
    {
      int selectionStart = this.SelectionStart;
      int selectionLength = this.SelectionLength;
      if (selectionLength > 0)
        this._internalBuffer.Remove(selectionStart, selectionLength);
      this.Text = this._internalBuffer.Insert(selectionStart, s).ToString();
      this.Select(selectionStart + s.Length, 0);
    }

    private void ProcessChar(char c)
    {
      int selectionStart = this.SelectionStart;
      int selectionLength = this.SelectionLength;
      if (selectionLength > 0)
        this._internalBuffer.Remove(selectionStart, selectionLength);
      else if (c == '\b')
      {
        if (selectionStart <= 0)
          return;
        this._internalBuffer.Remove(selectionStart - 1, 1);
        return;
      }
      this._internalBuffer.Insert(selectionStart, c);
    }
  }
}
