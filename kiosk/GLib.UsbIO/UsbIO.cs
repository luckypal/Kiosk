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
			KEY_USB = 32767;
			TECLAS = 32767;
			IsOpen = false;
			usbapi = null;
		}

		public bool Close()
		{
			usbapi.Dispose();
			IsOpen = false;
			TECLAS = 32767;
			return true;
		}

		public bool Open()
		{
			IsOpen = false;
			try
			{
				usbapi = new UsbHidPort();
				usbapi.ProductId = 63;
				usbapi.VendorId = 1240;
				usbapi.OnSpecifiedDeviceArrived += usb_OnSpecifiedDeviceArrived;
				usbapi.OnSpecifiedDeviceRemoved += usb_OnSpecifiedDeviceRemoved;
				usbapi.OnDeviceArrived += usb_OnDeviceArrived;
				usbapi.OnDeviceRemoved += usb_OnDeviceRemoved;
				usbapi.OnDataRecieved += usb_OnDataRecieved;
				usbapi.OnDataSend += usb_OnDataSend;
				usbapi.CheckDevicePresent();
				Refresh();
				Thread.Sleep(500);
			}
			catch
			{
			}
			return IsOpen;
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
			IsOpen = true;
			int num = 0;
			int num2 = 1;
			int num3 = 0;
			byte[] data = args.data;
			foreach (byte b in data)
			{
				if (num3 > 1)
				{
					if (b == 1)
					{
						num |= num2;
					}
					num2 <<= 1;
				}
				if (num3++ == 15)
				{
					break;
				}
			}
			KEY_USB = (num | 0x7000);
			num = 32639;
			if ((KEY_USB & 2) == 0)
			{
				num &= 0x5F7F;
			}
			if ((KEY_USB & 4) == 0)
			{
				num &= 0x7F7E;
			}
			if ((KEY_USB & 8) == 0)
			{
				num &= 0x7F77;
			}
			if ((KEY_USB & 0x10) == 0)
			{
				num &= 0x7F7B;
			}
			if ((KEY_USB & 0x20) == 0)
			{
				num &= 0x7F7D;
			}
			if ((KEY_USB & 0x40) == 0)
			{
				num &= 0x7F5F;
			}
			if ((KEY_USB & 0x80) == 0)
			{
				num &= 0x6F7F;
			}
			if ((KEY_USB & 0x100) == 0)
			{
				num &= 0x3F7F;
			}
			if ((KEY_USB & 0x400) == 0)
			{
				num &= 0x7E7F;
			}
			if ((KEY_USB & 0x800) == 0)
			{
				num &= 0x7D7F;
			}
			if ((KEY_USB & 0x200) == 0)
			{
				num &= 0x7B7F;
			}
			TECLAS = num;
		}

		private void Refresh()
		{
			byte[] array = new byte[66];
			if (usbapi.SpecifiedDevice != null)
			{
				array[0] = 0;
				array[1] = 129;
				usbapi.SpecifiedDevice.SendData(array);
			}
		}
	}
}
