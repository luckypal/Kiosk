// Decompiled with JetBrains decompiler
// Type: GLib.Devices.CommPort
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace GLib.Devices
{
  public class CommPort
  {
    public static int buffer_size = 8192;
    public int buffer_inici;
    public int buffer_final;
    public int buffer_bytes;
    public byte[] buffer;
    private bool _keepReading;
    private SerialPort _serialPort;
    private Thread _readThread;

    public CommPort()
    {
      this._serialPort = new SerialPort();
      this._serialPort.ReadBufferSize = CommPort.buffer_size;
      this._readThread = (Thread) null;
      this._keepReading = false;
      this.buffer = new byte[CommPort.buffer_size];
      this.buffer_inici = this.buffer_final = this.buffer_bytes = 0;
    }

    private bool StartUpReading()
    {
      if (this._keepReading)
        return false;
      this._keepReading = true;
      this._readThread = new Thread(new ThreadStart(this.ReadPort));
      this._readThread.Start();
      return true;
    }

    private bool ShutDownReading()
    {
      if (!this._keepReading)
        return false;
      this._keepReading = false;
      this._readThread.Join();
      this._readThread = (Thread) null;
      return true;
    }

    private void ReadPort()
    {
      while (this._keepReading)
      {
        if (this._serialPort.IsOpen)
        {
          byte[] numArray = new byte[this._serialPort.ReadBufferSize + 1];
          try
          {
            int count = 0;
            try
            {
              count = this._serialPort.Read(numArray, 0, this._serialPort.ReadBufferSize);
            }
            catch
            {
            }
            Encoding.ASCII.GetString(numArray, 0, count);
            for (int index = 0; index < count; ++index)
            {
              this.buffer[this.buffer_final] = numArray[index];
              ++this.buffer_final;
              this.buffer_final &= CommPort.buffer_size - 1;
              ++this.buffer_bytes;
              if (this.buffer_bytes > CommPort.buffer_size)
              {
                this.buffer_inici = this.buffer_final;
                --this.buffer_bytes;
              }
            }
          }
          catch (TimeoutException ex)
          {
          }
        }
        else
          Thread.Sleep(new TimeSpan(0, 0, 0, 0, 50));
      }
    }

    public int Bytes
    {
      get
      {
        return this.buffer_bytes;
      }
    }

    public byte[] Read(int mida)
    {
      byte[] numArray;
      if (this.IsOpen)
      {
        numArray = new byte[mida];
        for (int index = 0; index < mida; ++index)
        {
          numArray[index] = this.buffer[this.buffer_inici];
          ++this.buffer_inici;
          this.buffer_inici &= CommPort.buffer_size - 1;
          --this.buffer_bytes;
          if (this.buffer_bytes <= 0)
            this.buffer_bytes = this.buffer_inici = this.buffer_final = 0;
        }
      }
      else
      {
        numArray = new byte[0];
        this.buffer_bytes = this.buffer_inici = this.buffer_final = 0;
      }
      return numArray;
    }

    public byte Read()
    {
      byte num = 0;
      if (this.IsOpen)
      {
        num = this.buffer[this.buffer_inici];
        ++this.buffer_inici;
        this.buffer_inici &= CommPort.buffer_size - 1;
        --this.buffer_bytes;
        if (this.buffer_bytes < 0)
          this.buffer_bytes = this.buffer_inici = this.buffer_final = 0;
      }
      return num;
    }

    public bool Open(string port, int baudrate, Parity parity, int databits, StopBits stopbits)
    {
      if (this.IsOpen)
        this.Close();
      if (port == "-")
        return false;
      try
      {
        this._serialPort.PortName = port;
        this._serialPort.BaudRate = baudrate;
        this._serialPort.Parity = parity;
        this._serialPort.DataBits = databits;
        this._serialPort.Handshake = Handshake.None;
        this._serialPort.ReadTimeout = 50;
        this._serialPort.WriteTimeout = 50;
        this._serialPort.Open();
        this.StartUpReading();
      }
      catch (IOException ex)
      {
      }
      catch (UnauthorizedAccessException ex)
      {
      }
      return this._serialPort.IsOpen;
    }

    public void Close()
    {
      this.ShutDownReading();
      try
      {
        this._serialPort.Close();
      }
      catch
      {
      }
    }

    public bool IsOpen
    {
      get
      {
        return this._serialPort.IsOpen;
      }
    }

    public string[] Puertos()
    {
      return SerialPort.GetPortNames();
    }

    public int PorEnviar()
    {
      return this._serialPort.BytesToWrite;
    }

    public bool Write(byte[] data)
    {
      if (!this.IsOpen)
        return false;
      try
      {
        this._serialPort.Write(data, 0, data.Length);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public bool Write(string data)
    {
      if (this.IsOpen)
      {
        byte[] bytes = new ASCIIEncoding().GetBytes(data);
        try
        {
          this._serialPort.Write(bytes, 0, bytes.Length);
          return true;
        }
        catch
        {
        }
      }
      return false;
    }
  }
}
