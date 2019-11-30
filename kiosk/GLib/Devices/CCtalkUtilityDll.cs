// Decompiled with JetBrains decompiler
// Type: GLib.Devices.CCtalkUtilityDll
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System.Runtime.InteropServices;

namespace GLib.Devices
{
  public class CCtalkUtilityDll
  {
    [DllImport("CRCDLL.dll")]
    public static extern unsafe ushort crcCalculation(int nLen, byte* pC, ushort seed);
  }
}
