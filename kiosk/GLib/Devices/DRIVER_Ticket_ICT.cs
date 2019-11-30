// Decompiled with JetBrains decompiler
// Type: GLib.Devices.DRIVER_Ticket_ICT
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.IO.Ports;
using System.Threading;

namespace GLib.Devices
{
  public class DRIVER_Ticket_ICT
  {
    private static int MIDA = 16384;
    private int CntError = 0;
    private int UtimCnt = 0;
    private int UtimCntOK = 0;
    private const int BLOCK = 6;
    public byte ID;
    public bool OnLine;
    public bool Ready;
    public bool Fail;
    public bool Check;
    public CommPort comm;
    public string port;
    public byte Tickets;
    public int Tickets_Out;
    public int Tickets_Fail;
    public int Tickets_Send;
    public int Tickets_Total;
    public int CntCmd;
    public bool EnError;
    private volatile int inici;
    private volatile int final;
    private volatile int bytes;
    private volatile byte[] buffer;
    private volatile bool respuesta;
    private DateTime Check_Timeout;

    public DRIVER_Ticket_ICT()
    {
      this.comm = (CommPort) null;
      this.ID = (byte) 0;
      this.inici = 0;
      this.final = 0;
      this.bytes = 0;
      this.CntCmd = 0;
      this.respuesta = false;
      this.Fail = false;
      this.Ready = false;
      this.Check = false;
      this.EnError = false;
      this.buffer = new byte[DRIVER_Ticket_ICT.MIDA];
      this.port = "COM1";
      this.Tickets_Total = 0;
      this.Tickets = (byte) 0;
      this.Tickets_Out = 0;
      this.Tickets_Fail = 0;
      this.Tickets_Send = 0;
      this.Reset_Timeout();
    }

    public bool Open()
    {
      if (this.comm != null)
        this.comm.Close();
      this.OnLine = false;
      this.comm = (CommPort) null;
      if (this.port == "-" || this.port == "" || this.port == "?" || this.port == null)
        return false;
      string[] portNames = SerialPort.GetPortNames();
      int num = 0;
      if (portNames != null)
      {
        for (int index = 0; index < portNames.Length; ++index)
        {
          if (portNames[index].ToLower() == this.port.ToLower())
            num = 1;
        }
      }
      if (this.port == "-" || this.port == "?" || num == 0)
        return false;
      this.comm = new CommPort();
      this.comm.Open(this.port, 9600, Parity.Even, 8, StopBits.One);
      this.OnLine = true;
      this.Fail = false;
      this.Ready = false;
      this.EnError = false;
      return true;
    }

    public bool Close()
    {
      if (this.comm != null)
        this.comm.Close();
      this.OnLine = false;
      return true;
    }

    private byte[] CopyFlat(int len)
    {
      byte[] numArray = new byte[len];
      for (int index1 = 0; index1 < len; ++index1)
      {
        int index2 = this.inici + index1 & DRIVER_Ticket_ICT.MIDA - 1;
        numArray[index1] = this.buffer[index2];
      }
      return numArray;
    }

    private void Purge(int len)
    {
      this.inici = this.inici + len & DRIVER_Ticket_ICT.MIDA - 1;
      this.bytes -= len;
      if (this.bytes >= 0)
        return;
      this.bytes = 0;
      this.inici = 0;
      this.final = 0;
    }

    public void Decode()
    {
      bool flag = false;
      while (this.bytes >= 6 && !flag)
      {
        byte[] _pck = this.CopyFlat(6);
        if (_pck[0] == (byte) 1 && _pck[1] == (byte) 1)
        {
          if (this.Checksum(ref _pck))
          {
            this.respuesta = true;
            ++this.CntCmd;
            switch (_pck[3])
            {
              case 0:
              case 2:
              case 10:
                this.UtimCnt = (int) _pck[4];
                this.UtimCntOK = (int) _pck[4];
                this.Fail = false;
                this.Ready = true;
                this.Check = true;
                this.CntError = 0;
                break;
              case 9:
                this.UtimCnt = (int) _pck[4];
                if (this.UtimCnt == 0)
                  this.UtimCntOK = 0;
                ++this.CntError;
                if (this.CntError > 10)
                {
                  this.CntError = 0;
                  this.Reset();
                  break;
                }
                break;
              case 170:
                byte num = _pck[4];
                this.UtimCntOK = (int) num;
                this.UtimCnt = (int) num;
                this.CntError = 0;
                ++this.Tickets_Out;
                --this.Tickets;
                --this.Tickets_Send;
                if (this.Tickets_Send <= 0)
                {
                  this.Tickets_Send = 0;
                  this.Fail = false;
                  this.Ready = true;
                  this.Check = true;
                }
                this.EnError = false;
                break;
              case 187:
                this.UtimCnt = (int) _pck[4];
                if (this.UtimCnt == 0)
                  this.UtimCntOK = 0;
                this.CntError = 0;
                ++this.Tickets_Fail;
                this.Ready = false;
                this.Fail = true;
                this.Check = false;
                this.EnError = true;
                break;
              default:
                this.UtimCnt = (int) _pck[4];
                if (this.UtimCnt == 0)
                  this.UtimCntOK = 0;
                this.CntError = 0;
                this.Check = false;
                this.Ready = false;
                this.Fail = true;
                this.EnError = true;
                break;
            }
            this.Purge(6);
            ++this.CntCmd;
          }
          else
            this.Purge(1);
        }
        else
          this.Purge(1);
      }
    }

    public bool Parser()
    {
      if (this.comm == null)
        return false;
      int bytes = this.comm.Bytes;
      if (bytes > 0)
      {
        foreach (byte num in this.comm.Read(bytes))
        {
          this.buffer[this.final] = num;
          this.final = ++this.final & DRIVER_Ticket_ICT.MIDA - 1;
          ++this.bytes;
          if (this.bytes >= DRIVER_Ticket_ICT.MIDA)
          {
            this.inici &= ++this.inici & DRIVER_Ticket_ICT.MIDA - 1;
            this.bytes = DRIVER_Ticket_ICT.MIDA - 1;
          }
        }
      }
      this.Decode();
      return true;
    }

    private bool Checksum(ref byte[] _pck)
    {
      byte num = 0;
      try
      {
        for (int index = 0; index < 5; ++index)
          num += _pck[index];
        if ((int) _pck[5] == (int) num)
          return true;
      }
      catch
      {
      }
      return false;
    }

    private byte[] CMD_Payout(byte _cnt)
    {
      byte[] numArray = new byte[6]
      {
        (byte) 1,
        (byte) 16,
        this.ID,
        (byte) 16,
        _cnt,
        (byte) 0
      };
      numArray[5] = (byte) ((uint) numArray[0] + (uint) numArray[1] + (uint) numArray[2] + (uint) numArray[3] + (uint) numArray[4]);
      return numArray;
    }

    public bool Payout(byte val)
    {
      if (this.comm == null)
        return false;
      if (!this.OnLine)
        this.Open();
      this.Tickets += val;
      this.Tickets_Total += (int) val;
      return true;
    }

    public bool Reset()
    {
      if (this.comm == null)
        return false;
      if (!this.OnLine)
        this.Open();
      if (!this.OnLine)
        return false;
      byte[] data = new byte[6]
      {
        (byte) 1,
        (byte) 16,
        this.ID,
        (byte) 18,
        (byte) 0,
        (byte) 0
      };
      data[5] = (byte) ((uint) data[0] + (uint) data[1] + (uint) data[2] + (uint) data[3] + (uint) data[4]);
      this.EnError = false;
      this.Fail = false;
      this.Ready = false;
      this.Check = false;
      this.comm.Write(data);
      return true;
    }

    public bool Check_Status()
    {
      this.Reset_Timeout();
      if (this.comm == null)
        return false;
      if (!this.OnLine)
        this.Open();
      if (!this.OnLine)
        return false;
      byte[] data = new byte[6]
      {
        (byte) 1,
        (byte) 16,
        this.ID,
        (byte) 17,
        (byte) 0,
        (byte) 0
      };
      data[5] = (byte) ((uint) data[0] + (uint) data[1] + (uint) data[2] + (uint) data[3] + (uint) data[4]);
      this.Fail = false;
      this.Ready = false;
      this.Check = false;
      this.comm.Write(data);
      this.Reset_Timeout();
      return true;
    }

    private void Reset_Timeout()
    {
      this.Check_Timeout = DateTime.Now;
    }

    public bool Poll()
    {
      if (this.comm == null)
        return false;
      this.Parser();
      if (this.Tickets > (byte) 0)
      {
        int totalMilliseconds = (int) (DateTime.Now - this.Check_Timeout).TotalMilliseconds;
        if ((!this.Check || this.Check && !this.Ready && !this.Fail) && totalMilliseconds > 2000)
        {
          if (this.UtimCnt == this.UtimCntOK)
            this.Check_Status();
          this.Check_Timeout = DateTime.Now;
          ++this.CntCmd;
        }
      }
      if (this.Ready && !this.Fail && this.Check && this.Tickets > (byte) 0)
      {
        this.Ready = false;
        byte _cnt = 5;
        if ((int) _cnt > (int) this.Tickets)
          _cnt = this.Tickets;
        this.Tickets_Send = (int) _cnt;
        ++this.CntCmd;
        this.comm.Write(this.CMD_Payout(_cnt));
      }
      return true;
    }

    public string Find_Device()
    {
      string port = this.port;
      string[] portNames = SerialPort.GetPortNames();
      if (this.comm != null)
      {
        this.comm.Close();
        this.comm = (CommPort) null;
      }
      if (portNames == null)
      {
        this.respuesta = false;
        return "-";
      }
      for (int index = 0; index < portNames.Length; ++index)
      {
        try
        {
          this.respuesta = false;
          this.port = portNames[index];
          try
          {
            if (new SerialPort(this.port).IsOpen)
              continue;
          }
          catch
          {
          }
          this.Open();
          Thread.Sleep(100);
          this.Reset();
          Thread.Sleep(100);
          this.Poll();
          this.Check_Status();
          Thread.Sleep(100);
          this.Poll();
          this.Close();
          if (this.respuesta)
            return portNames[index];
        }
        catch
        {
        }
      }
      this.port = port;
      return "-";
    }
  }
}
