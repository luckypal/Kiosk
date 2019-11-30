// Decompiled with JetBrains decompiler
// Type: GLib.Devices.CCTEventArgs
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

namespace GLib.Devices
{
  public class CCTEventArgs
  {
    public byte[] buf;
    public byte CCTEvType;
    public int idxEnd;
    public int idxStart;
    public ushort msgChk;
    public int nData;

    public CCTEventArgs()
    {
      this.nData = this.idxStart = this.idxEnd = (int) (this.msgChk = (ushort) (this.CCTEvType = (byte) 0));
      this.buf = new byte[1024];
    }

    public CCTEventArgs(int n)
      : this()
    {
      this.nData = n;
    }

    public CCTEventArgs(int start, int end)
      : this()
    {
      this.idxStart = start;
      this.idxEnd = end;
      this.nData = end - start;
    }
  }
}
