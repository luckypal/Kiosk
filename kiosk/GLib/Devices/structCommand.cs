// Decompiled with JetBrains decompiler
// Type: GLib.Devices.structCommand
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

namespace GLib.Devices
{
  public struct structCommand
  {
    public string descr;
    public byte[] cmdSeq;
    private byte nData;

    public structCommand(string des, byte[] bySeq)
    {
      this.descr = des;
      this.cmdSeq = bySeq;
      this.nData = bySeq[1];
    }

    public string GetDesc
    {
      get
      {
        return this.descr;
      }
    }

    public byte nDATA
    {
      get
      {
        return this.nData;
      }
    }

    public byte[] CMDSEQ
    {
      get
      {
        return this.cmdSeq;
      }
    }
  }
}
