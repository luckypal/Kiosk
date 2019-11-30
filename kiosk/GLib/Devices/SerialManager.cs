// Decompiled with JetBrains decompiler
// Type: GLib.Devices.SerialManager
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.IO.Ports;

namespace GLib.Devices
{
  public class SerialManager
  {
    private int idxIn;
    private int nBaudRate;
    private int nPortAvail;
    private int nPortNum;
    private byte[] rxBuff;
    private Array sAvailablePorts;
    private SerialPort ser;
    private byte[] txBuff;

    public event SerialManager.ByteReceivedManager OnReceivedData;

    public SerialManager()
    {
      this.nPortAvail = this.BuildPortsList();
      this.nBaudRate = 9600;
      this.nPortNum = 0;
      this.ser = new SerialPort();
      this.ser.PortName = this.sAvailablePorts == null ? "COM1" : (string) this.sAvailablePorts.GetValue(this.nPortNum);
      this.ser.BaudRate = this.nBaudRate;
      this.ser.DataReceived += new SerialDataReceivedEventHandler(this.datarec);
      this.rxBuff = new byte[1024];
      this.txBuff = new byte[1024];
      this.ser.ReceivedBytesThreshold = 5;
      this.idxIn = 0;
    }

    private int BuildPortsList()
    {
      this.sAvailablePorts = (Array) SerialPort.GetPortNames();
      if (this.sAvailablePorts != null)
        return this.sAvailablePorts.GetLength(0);
      return 0;
    }

    public bool ClosePort()
    {
      if (this.ser.IsOpen)
        this.ser.Close();
      return true;
    }

    private void datarec(object sender, SerialDataReceivedEventArgs ev)
    {
      int receivedBytesThreshold = this.ser.ReceivedBytesThreshold;
      CCTEventArgs e = new CCTEventArgs(this.idxIn, this.idxIn + receivedBytesThreshold);
      if (ev.EventType != SerialData.Chars)
        return;
      this.ser.Read(this.rxBuff, this.idxIn, receivedBytesThreshold);
      this.idxIn += receivedBytesThreshold;
      try
      {
        this.OnReceivedData(sender, e);
      }
      catch
      {
      }
    }

    public byte GetRxByte(int index)
    {
      return this.rxBuff[index];
    }

    public bool OpenPort()
    {
      try
      {
        this.ser.Open();
        this.ser.DtrEnable = true;
      }
      catch (Exception ex)
      {
        throw ex;
      }
      return true;
    }

    public bool OpenPort(string sPortName)
    {
      this.ser.PortName = sPortName;
      try
      {
        this.ser.Open();
        this.ser.DtrEnable = true;
      }
      catch (Exception ex)
      {
        return false;
      }
      return true;
    }

    public bool OpenPort(string sPortName, int nBaudeRate)
    {
      this.ser.PortName = sPortName;
      this.ser.BaudRate = nBaudeRate;
      try
      {
        this.ser.Open();
        this.ser.DtrEnable = true;
      }
      catch (Exception ex)
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
      this.idxIn = 0;
      this.ser.ReceivedBytesThreshold = 5;
    }

    public int SaveBufferContent()
    {
      if (this.ser.BytesToRead <= 0)
        return 0;
      int num = this.ser.Read(this.rxBuff, this.idxIn, this.ser.BytesToRead);
      this.idxIn += num;
      return num;
    }

    public void SetTxByte(int index, byte data)
    {
      this.txBuff[index] = data;
    }

    public int Transmit(int nData)
    {
      this.ser.Write(this.txBuff, 0, nData);
      return 0;
    }

    public int Transmit(byte[] buf, int nData)
    {
      this.ser.Write(buf, 0, nData);
      return 0;
    }

    public int BAUDRATE
    {
      get
      {
        return this.ser.BaudRate;
      }
      set
      {
        this.ser.BaudRate = value;
      }
    }

    public int GetNumPortsAvail
    {
      get
      {
        return this.nPortAvail;
      }
    }

    public Parity PARITY
    {
      get
      {
        return this.ser.Parity;
      }
      set
      {
        this.ser.Parity = value;
      }
    }

    public int PortBaudeRate
    {
      get
      {
        return this.nBaudRate;
      }
      set
      {
        this.nBaudRate = value;
      }
    }

    public string PORTNAME
    {
      get
      {
        return this.ser.PortName;
      }
      set
      {
        this.ser.PortName = value;
      }
    }

    public int PortOpened
    {
      get
      {
        return this.nPortNum;
      }
    }

    public int SetRecThresh
    {
      set
      {
        this.ser.ReceivedBytesThreshold = value;
      }
    }

    public string[] sPortList
    {
      get
      {
        return (string[]) this.sAvailablePorts;
      }
    }

    public delegate void ByteReceivedManager(object sender, CCTEventArgs e);
  }
}
