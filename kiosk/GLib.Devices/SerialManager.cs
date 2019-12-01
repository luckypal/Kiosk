using System;
using System.IO.Ports;

namespace GLib.Devices
{
	public class SerialManager
	{
		public delegate void ByteReceivedManager(object sender, CCTEventArgs e);

		private int idxIn;

		private int nBaudRate;

		private int nPortAvail;

		private int nPortNum;

		private byte[] rxBuff;

		private Array sAvailablePorts;

		private SerialPort ser;

		private byte[] txBuff;

		public int BAUDRATE
		{
			get
			{
				return ser.BaudRate;
			}
			set
			{
				ser.BaudRate = value;
			}
		}

		public int GetNumPortsAvail => nPortAvail;

		public Parity PARITY
		{
			get
			{
				return ser.Parity;
			}
			set
			{
				ser.Parity = value;
			}
		}

		public int PortBaudeRate
		{
			get
			{
				return nBaudRate;
			}
			set
			{
				nBaudRate = value;
			}
		}

		public string PORTNAME
		{
			get
			{
				return ser.PortName;
			}
			set
			{
				ser.PortName = value;
			}
		}

		public int PortOpened => nPortNum;

		public int SetRecThresh
		{
			set
			{
				ser.ReceivedBytesThreshold = value;
			}
		}

		public string[] sPortList => (string[])sAvailablePorts;

		public event ByteReceivedManager OnReceivedData;

		public SerialManager()
		{
			nPortAvail = BuildPortsList();
			nBaudRate = 9600;
			nPortNum = 0;
			ser = new SerialPort();
			if (sAvailablePorts != null)
			{
				ser.PortName = (string)sAvailablePorts.GetValue(nPortNum);
			}
			else
			{
				ser.PortName = "COM1";
			}
			ser.BaudRate = nBaudRate;
			ser.DataReceived += datarec;
			rxBuff = new byte[1024];
			txBuff = new byte[1024];
			ser.ReceivedBytesThreshold = 5;
			idxIn = 0;
		}

		private int BuildPortsList()
		{
			sAvailablePorts = SerialPort.GetPortNames();
			if (sAvailablePorts != null)
			{
				return sAvailablePorts.GetLength(0);
			}
			return 0;
		}

		public bool ClosePort()
		{
			if (ser.IsOpen)
			{
				ser.Close();
			}
			return true;
		}

		private void datarec(object sender, SerialDataReceivedEventArgs ev)
		{
			int receivedBytesThreshold = ser.ReceivedBytesThreshold;
			CCTEventArgs e = new CCTEventArgs(idxIn, idxIn + receivedBytesThreshold);
			if (ev.EventType == SerialData.Chars)
			{
				ser.Read(rxBuff, idxIn, receivedBytesThreshold);
				idxIn += receivedBytesThreshold;
				try
				{
					this.OnReceivedData(sender, e);
				}
				catch
				{
				}
			}
		}

		public byte GetRxByte(int index)
		{
			return rxBuff[index];
		}

		public bool OpenPort()
		{
			try
			{
				ser.Open();
				ser.DtrEnable = true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return true;
		}

		public bool OpenPort(string sPortName)
		{
			ser.PortName = sPortName;
			try
			{
				ser.Open();
				ser.DtrEnable = true;
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public bool OpenPort(string sPortName, int nBaudeRate)
		{
			ser.PortName = sPortName;
			ser.BaudRate = nBaudeRate;
			try
			{
				ser.Open();
				ser.DtrEnable = true;
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public int Receive()
		{
			return 0;
		}

		public void ResetRecIdx()
		{
			idxIn = 0;
			ser.ReceivedBytesThreshold = 5;
		}

		public int SaveBufferContent()
		{
			if (ser.BytesToRead > 0)
			{
				int num = ser.Read(rxBuff, idxIn, ser.BytesToRead);
				idxIn += num;
				return num;
			}
			return 0;
		}

		public void SetTxByte(int index, byte data)
		{
			txBuff[index] = data;
		}

		public int Transmit(int nData)
		{
			ser.Write(txBuff, 0, nData);
			return 0;
		}

		public int Transmit(byte[] buf, int nData)
		{
			ser.Write(buf, 0, nData);
			return 0;
		}
	}
}
