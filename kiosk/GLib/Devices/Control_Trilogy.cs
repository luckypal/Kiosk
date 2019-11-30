// Decompiled with JetBrains decompiler
// Type: GLib.Devices.Control_Trilogy
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Threading;

namespace GLib.Devices
{
  public class Control_Trilogy
  {
    private static int MIDA = 16384;
    public Decimal Base = new Decimal(100);
    public int Canales = 8;
    public bool OnLine;
    public CommPort comm;
    public string port;
    public int[] Canal;
    public int[] eCanal;
    private int inici;
    private int final;
    private int bytes;
    private byte[] buffer;
    private int m_Creditos;
    private bool respuesta;
    private byte Canales_On;
    private byte XorPoll;
    public string _f_resp_scom;
    public string _f_com;
    private string[] _f_ports;
    private int _f_cnt;

    public Control_Trilogy()
    {
      this.comm = (CommPort) null;
      this.inici = 0;
      this.final = 0;
      this.bytes = 0;
      this.Base = new Decimal(100);
      this.respuesta = false;
      this.buffer = new byte[Control_Trilogy.MIDA];
      this.port = "COM1";
      this.Creditos = 0;
      this.XorPoll = (byte) 16;
      this.Canales_On = (byte) 0;
      this.Canal = new int[this.Canales];
      this.eCanal = new int[this.Canales];
      this.Set_Euro();
      this.OnLine = false;
    }

    public void Set_Euro()
    {
      for (int index = 0; index < this.Canal.Length; ++index)
      {
        this.Canal[index] = 0;
        this.eCanal[index] = 0;
      }
      this.eCanal[0] = 1;
      this.eCanal[1] = 1;
      this.eCanal[2] = 1;
      this.eCanal[3] = 1;
      this.eCanal[4] = 1;
      this.Canal[0] = 500;
      this.Canal[1] = 1000;
      this.Canal[2] = 2000;
      this.Canal[3] = 5000;
      this.Canal[4] = 10000;
    }

    public void Set_Brazil()
    {
      for (int index = 0; index < this.Canal.Length; ++index)
      {
        this.Canal[index] = 0;
        this.eCanal[index] = 0;
      }
      this.eCanal[0] = 0;
      this.eCanal[1] = 0;
      this.eCanal[2] = 1;
      this.eCanal[3] = 1;
      this.eCanal[4] = 1;
      this.eCanal[5] = 1;
      this.eCanal[6] = 1;
      this.Canal[0] = 0;
      this.Canal[1] = 0;
      this.Canal[2] = 500;
      this.Canal[3] = 1000;
      this.Canal[4] = 2000;
      this.Canal[5] = 5000;
      this.Canal[6] = 10000;
    }

    public bool GetChannelInhibit(int _c)
    {
      if (_c < this.Canales)
        return this.eCanal[_c - 1] == 0;
      return true;
    }

    public int GetChannelValue(int _c)
    {
      if (_c < this.Canales)
        return this.Canal[_c - 1];
      return 0;
    }

    public bool GetChannelEnabled(int _c)
    {
      return _c < this.Canales;
    }

    public string GetChannelCurrency(int _c)
    {
      return "EUR";
    }

    public bool Open()
    {
      if (this.comm != null)
      {
        this.OnLine = false;
        this.comm.Close();
      }
      this.comm = (CommPort) null;
      this._f_ports = SerialPort.GetPortNames();
      int num = 0;
      if (this._f_ports != null)
      {
        for (int index = 0; index < this._f_ports.Length; ++index)
        {
          if (this._f_ports[index].ToLower() == this.port.ToLower())
            num = 1;
        }
      }
      if (this.port == "-" || this.port == "?" || num == 0)
        return false;
      this.comm = new CommPort();
      this.comm.Open(this.port, 9600, Parity.Even, 7, StopBits.One);
      this.OnLine = true;
      this.Enable();
      return true;
    }

    public bool Close()
    {
      if (this.comm != null)
        this.comm.Close();
      this.OnLine = false;
      return true;
    }

    public string Find_Device()
    {
      string[] portNames = SerialPort.GetPortNames();
      if (portNames == null)
        return "-";
      this.comm = new CommPort();
      foreach (string port in portNames)
      {
        try
        {
          this.port = port;
          this.comm.Open(port, 9600, Parity.Even, 7, StopBits.One);
          this.Disable();
          this.respuesta = false;
          for (int index = 0; index < 5; ++index)
          {
            if (this.ResetCom((byte) 0))
            {
              this.Poll();
              Thread.Sleep(200);
              this.Parser();
              if (this.respuesta)
              {
                this._f_resp_scom = this.port;
                this.comm.Close();
                return port;
              }
            }
          }
          this.comm.Close();
        }
        catch
        {
        }
      }
      return "-";
    }

    [Description("Creditos")]
    public int Creditos
    {
      get
      {
        return this.m_Creditos;
      }
      set
      {
        this.m_Creditos = value;
      }
    }

    private byte[] CopyFlat(int len)
    {
      byte[] numArray = new byte[len];
      for (int index1 = 0; index1 < len; ++index1)
      {
        int index2 = this.inici + index1 & Control_Trilogy.MIDA - 1;
        numArray[index1] = this.buffer[index2];
      }
      return numArray;
    }

    private void Purge(int len)
    {
      this.inici = this.inici + len & Control_Trilogy.MIDA - 1;
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
      while (this.bytes >= 4 && !flag)
      {
        byte[] numArray = this.CopyFlat(2);
        int len = (int) numArray[1];
        if (numArray[0] == (byte) 2 && numArray.Length >= 2 && len >= 2)
        {
          this.respuesta = true;
          if (this.bytes >= len)
          {
            byte[] _pck = this.CopyFlat(len);
            if (this.ChecksumV(ref _pck))
            {
              if (((int) _pck[3] & 2) != 2)
                ;
              if (((int) _pck[3] & 16) == 16)
              {
                int num = (int) _pck[5] >> 3 & 31;
                if (this.Canal[num - 1] > 0)
                  this.m_Creditos = this.Canal[num - 1];
              }
              this.Purge(len);
            }
            else
              this.Purge(1);
          }
          else
            flag = true;
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
      if (bytes > 4)
      {
        foreach (byte num in this.comm.Read(bytes))
        {
          this.buffer[this.final] = num;
          this.final = ++this.final & Control_Trilogy.MIDA - 1;
          ++this.bytes;
          if (this.bytes >= Control_Trilogy.MIDA)
          {
            this.inici &= ++this.inici & Control_Trilogy.MIDA - 1;
            this.bytes = Control_Trilogy.MIDA - 1;
          }
        }
      }
      this.Decode();
      return true;
    }

    public bool Poll()
    {
      if (this.comm == null)
        return false;
      this.Command((byte) 0);
      return true;
    }

    private void Checksum(ref byte[] _pck)
    {
      byte num1 = 0;
      try
      {
        int num2 = (int) _pck[1];
        for (int index = 1; index < num2 - 2; ++index)
          num1 ^= _pck[index];
        _pck[num2 - 1] = num1;
      }
      catch
      {
      }
    }

    private bool ChecksumV(ref byte[] _pck)
    {
      byte num1 = 0;
      int num2 = (int) _pck[1];
      try
      {
        for (int index = 1; index < num2 - 2; ++index)
          num1 ^= _pck[index];
        if ((int) _pck[num2 - 1] == (int) num1)
          return true;
      }
      catch
      {
        return false;
      }
      return false;
    }

    public void Start_Find_Device()
    {
      this._f_resp_scom = "-";
      this._f_ports = SerialPort.GetPortNames();
      this._f_cnt = 0;
    }

    public bool Next_Find_Device()
    {
      if (this.comm != null)
      {
        this.comm.Close();
        this.comm = (CommPort) null;
      }
      if (this._f_ports == null)
      {
        this.respuesta = false;
        this._f_resp_scom = "-";
        return true;
      }
      if (this._f_cnt < this._f_ports.Length)
      {
        this.respuesta = false;
        this._f_resp_scom = "-";
        this._f_com = this._f_ports[this._f_cnt];
        this.port = this._f_com;
        ++this._f_cnt;
        return this.Open();
      }
      this.respuesta = false;
      this._f_resp_scom = "-";
      return true;
    }

    public bool Poll_Find_Device()
    {
      this.respuesta = false;
      return this.Next_Find_Device();
    }

    public void Stop_Find_Device()
    {
      if (this.comm == null)
        return;
      this.comm.Close();
      this.comm = (CommPort) null;
    }

    public bool Command(byte _val)
    {
      if (this.comm == null)
        return false;
      this.XorPoll ^= (byte) 1;
      byte[] _pck = new byte[8]
      {
        (byte) 2,
        (byte) 8,
        this.XorPoll,
        this.Canales_On,
        (byte) 0,
        (byte) 0,
        (byte) 3,
        (byte) 103
      };
      this.Checksum(ref _pck);
      this.comm.Write(_pck);
      return true;
    }

    public bool ResetCom(byte _val)
    {
      if (this.comm == null)
        return false;
      this.XorPoll ^= (byte) 1;
      byte[] _pck = new byte[9]
      {
        (byte) 63,
        (byte) 63,
        (byte) 63,
        (byte) 63,
        (byte) 63,
        (byte) 63,
        (byte) 63,
        (byte) 63,
        (byte) 63
      };
      this.Checksum(ref _pck);
      this.comm.Write(_pck);
      Thread.Sleep(300);
      byte[] numArray = this.comm.Read(this.comm.Bytes);
      return numArray.Length != 9 || (numArray[0] != (byte) 63 || numArray[1] != (byte) 63 || (numArray[2] != (byte) 63 || numArray[3] != (byte) 63) || (numArray[4] != (byte) 63 || numArray[5] != (byte) 63 || numArray[6] != (byte) 63) || numArray[7] != (byte) 63);
    }

    public bool Enable()
    {
      if (this.comm == null)
        return false;
      this.Canales_On = (byte) 0;
      for (int index = 0; index < this.Canal.Length; ++index)
      {
        if (this.Canal[index] > 0)
          this.Canales_On |= (byte) (1 << index);
      }
      return true;
    }

    public bool Disable()
    {
      if (this.comm == null)
        return false;
      this.Canales_On = (byte) 0;
      return true;
    }
  }
}
