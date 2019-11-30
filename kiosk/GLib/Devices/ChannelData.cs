// Decompiled with JetBrains decompiler
// Type: GLib.Devices.ChannelData
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

namespace GLib.Devices
{
  public class ChannelData
  {
    public int Value;
    public byte Channel;
    public char[] Currency;
    public int Level;
    public bool Recycling;
    public bool Inhibit;
    public bool Enabled;

    public ChannelData()
    {
      this.Value = 0;
      this.Channel = (byte) 0;
      this.Currency = new char[3];
      this.Level = 0;
      this.Inhibit = false;
      this.Enabled = false;
      this.Recycling = false;
    }
  }
}
