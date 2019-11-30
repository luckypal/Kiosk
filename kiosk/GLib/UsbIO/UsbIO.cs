// Decompiled with JetBrains decompiler
// Type: GLib.UsbIO.UsbIO
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.Threading;
using UsbLibrary;

namespace GLib.UsbIO
{
  public class UsbIO
  {
    private UsbHidPort usbapi;
    private int KEY_USB;
    public int TECLAS;
    public bool IsOpen;

    public UsbIO()
    {
      this.KEY_USB = (int) short.MaxValue;
      this.TECLAS = (int) short.MaxValue;
      this.IsOpen = false;
      this.usbapi = (UsbHidPort) null;
    }

    public bool Close()
    {
      this.usbapi.Dispose();
      this.IsOpen = false;
      this.TECLAS = (int) short.MaxValue;
      return true;
    }

    public bool Open()
    {
      this.IsOpen = false;
      try
      {
        this.usbapi = new UsbHidPort();
        this.usbapi.ProductId = 63;
        this.usbapi.VendorId = 1240;
        this.usbapi.OnSpecifiedDeviceArrived += new EventHandler(this.usb_OnSpecifiedDeviceArrived);
        this.usbapi.OnSpecifiedDeviceRemoved += new EventHandler(this.usb_OnSpecifiedDeviceRemoved);
        this.usbapi.OnDeviceArrived += new EventHandler(this.usb_OnDeviceArrived);
        this.usbapi.OnDeviceRemoved += new EventHandler(this.usb_OnDeviceRemoved);
        this.usbapi.OnDataRecieved += new DataRecievedEventHandler(this.usb_OnDataRecieved);
        this.usbapi.OnDataSend += new EventHandler(this.usb_OnDataSend);
        this.usbapi.CheckDevicePresent();
        this.Refresh();
        Thread.Sleep(500);
      }
      catch
      {
      }
      return this.IsOpen;
    }

    private void usb_OnSpecifiedDeviceArrived(object sender, EventArgs e)
    {
    }

    private void usb_OnDeviceArrived(object sender, EventArgs e)
    {
    }

    private void usb_OnDeviceRemoved(object sender, EventArgs e)
    {
    }

    private void usb_OnSpecifiedDeviceRemoved(object sender, EventArgs e)
    {
    }

    private void usb_OnDataSend(object sender, EventArgs e)
    {
    }

    private void usb_OnDataRecieved(object sender, DataRecievedEventArgs args)
    {
      this.IsOpen = true;
      int num1 = 0;
      int num2 = 1;
      int num3 = 0;
      foreach (byte num4 in args.data)
      {
        if (num3 > 1)
        {
          if (num4 == (byte) 1)
            num1 |= num2;
          num2 <<= 1;
        }
        if (num3++ == 15)
          break;
      }
      this.KEY_USB = num1 | 28672;
      int num5 = 32639;
      if ((this.KEY_USB & 2) == 0)
        num5 &= 24447;
      if ((this.KEY_USB & 4) == 0)
        num5 &= 32638;
      if ((this.KEY_USB & 8) == 0)
        num5 &= 32631;
      if ((this.KEY_USB & 16) == 0)
        num5 &= 32635;
      if ((this.KEY_USB & 32) == 0)
        num5 &= 32637;
      if ((this.KEY_USB & 64) == 0)
        num5 &= 32607;
      if ((this.KEY_USB & 128) == 0)
        num5 &= 28543;
      if ((this.KEY_USB & 256) == 0)
        num5 &= 16255;
      if ((this.KEY_USB & 1024) == 0)
        num5 &= 32383;
      if ((this.KEY_USB & 2048) == 0)
        num5 &= 32127;
      if ((this.KEY_USB & 512) == 0)
        num5 &= 31615;
      this.TECLAS = num5;
    }

    private void Refresh()
    {
      byte[] data = new byte[66];
      if (this.usbapi.SpecifiedDevice == null)
        return;
      data[0] = (byte) 0;
      data[1] = (byte) 129;
      this.usbapi.SpecifiedDevice.SendData(data);
    }
  }
}
