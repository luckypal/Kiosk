// Decompiled with JetBrains decompiler
// Type: GLib.Devices.Control_Comestero
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Threading;

namespace GLib.Devices
{
  public class Control_Comestero
  {
    private static int MIDA = 16384;
    public int Canales = 8;
    public Decimal Base = new Decimal(100);
    public int SwapControl = 0;
    public int RM5_Command = -1;
    public int RM5_OK = 0;
    public string _f_resp_scom;
    public string _f_com;
    private string[] _f_ports;
    private int _f_cnt;
    private int _f_try;
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

    public Control_Comestero()
    {
      this.comm = (CommPort) null;
      this.inici = 0;
      this.final = 0;
      this.bytes = 0;
      this.respuesta = false;
      this.Base = new Decimal(100);
      this.buffer = new byte[Control_Comestero.MIDA];
      this.port = "COM2";
      this.Creditos = 0;
      this.Canal = new int[this.Canales];
      this.eCanal = new int[this.Canales];
      this.Set_Euro();
      this.SwapControl = 0;
      this.RM5_Command = -1;
      this.RM5_OK = 0;
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
      this.Canal[0] = 50;
      this.Canal[1] = 100;
      this.Canal[2] = 200;
    }

    public void Set_Brazil()
    {
      for (int index = 0; index < this.Canal.Length; ++index)
      {
        this.Canal[index] = 0;
        this.eCanal[index] = 0;
      }
      this.eCanal[0] = 1;
      this.Canal[0] = 1;
    }

    public bool Open()
    {
      for (int index1 = 0; index1 < 4; ++index1)
      {
        if (this.comm != null)
        {
          try
          {
            this.comm.Close();
          }
          catch
          {
          }
        }
        this.OnLine = false;
        this.comm = (CommPort) null;
        this.RM5_OK = 0;
        this.RM5_Command = -1;
        this.SwapControl = 0;
        this._f_ports = SerialPort.GetPortNames();
        int num = 0;
        if (this._f_ports != null)
        {
          for (int index2 = 0; index2 < this._f_ports.Length; ++index2)
          {
            if (this._f_ports[index2].ToLower() == this.port.ToLower())
              num = 1;
          }
        }
        if (this.port == "-" || this.port == "?" || num == 0)
          return false;
        this.comm = new CommPort();
        this.comm.Open(this.port, 9600, Parity.None, 8, StopBits.One);
        if (this.Init_Moneder())
        {
          this.OnLine = true;
          this.Enable();
          return true;
        }
        Thread.Sleep(1000);
      }
      if (this.comm != null)
        this.comm.Close();
      this.comm = (CommPort) null;
      return false;
    }

    private bool Init_Moneder()
    {
      if (this.comm == null)
        return false;
      this.RM5_OK = 0;
      byte[] data1 = new byte[1];
      byte[] data2 = new byte[2]{ (byte) 0, (byte) 1 };
      byte[] data3 = new byte[4]
      {
        (byte) 0,
        (byte) 1,
        (byte) 0,
        (byte) 10
      };
      this.RM5_Command = 0;
      this.comm.Write(data1);
      Thread.Sleep(100);
      this.Parser();
      this.RM5_Command = 1;
      this.comm.Write(data2);
      Thread.Sleep(100);
      this.Parser();
      this.RM5_Command = 0;
      this.comm.Write(data1);
      Thread.Sleep(100);
      this.Parser();
      this.RM5_Command = 10;
      this.comm.Write(data3);
      Thread.Sleep(100);
      this.Parser();
      this.RM5_Command = 10;
      this.comm.Write(data3);
      Thread.Sleep(100);
      this.Parser();
      this.RM5_Command = 10;
      this.comm.Write(data3);
      Thread.Sleep(100);
      this.Parser();
      this.RM5_Command = 0;
      this.comm.Write(data1);
      Thread.Sleep(100);
      this.Parser();
      this.RM5_Command = 0;
      this.comm.Write(data1);
      Thread.Sleep(100);
      this.Parser();
      return this.RM5_OK == 1;
    }

    public bool Close()
    {
      if (this.comm != null)
        this.comm.Close();
      this.OnLine = false;
      return true;
    }

    public void Start_Find_Device()
    {
      this._f_resp_scom = "-";
      this._f_ports = SerialPort.GetPortNames();
      this._f_cnt = 0;
      this._f_try = 0;
      this.Next_Find_Device();
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
        this._f_resp_scom = "-";
        return true;
      }
      if (this._f_cnt < this._f_ports.Length)
      {
        this.respuesta = false;
        this._f_try = 0;
        this._f_resp_scom = "-";
        this._f_com = this._f_ports[this._f_cnt];
        this.comm = new CommPort();
        this.comm.Open(this._f_com, 9600, Parity.None, 8, StopBits.One);
        ++this._f_cnt;
        return false;
      }
      this._f_resp_scom = "-";
      return true;
    }

    public bool Test_Device()
    {
      if (this.comm != null)
      {
        this.comm.Close();
        this.comm = (CommPort) null;
      }
      this._f_ports = SerialPort.GetPortNames();
      this.respuesta = false;
      this._f_try = 0;
      this._f_resp_scom = "-";
      this.comm = new CommPort();
      this.comm.Open(this._f_com, 9600, Parity.None, 8, StopBits.One);
      ++this._f_cnt;
      return this.Poll_Find_Device();
    }

    public bool Poll_Find_Device()
    {
      this.respuesta = false;
      if (!this.Init_Moneder())
        return this.Next_Find_Device();
      this.respuesta = true;
      if (this.comm != null)
      {
        this.comm.Close();
        this.comm = (CommPort) null;
      }
      this._f_resp_scom = this._f_com;
      return true;
    }

    public void Stop_Find_Device()
    {
      if (this.comm == null)
        return;
      this.comm.Close();
      this.comm = (CommPort) null;
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
        int index2 = this.inici + index1 & Control_Comestero.MIDA - 1;
        numArray[index1] = this.buffer[index2];
      }
      return numArray;
    }

    private void Purge(int len)
    {
      this.inici = this.inici + len & Control_Comestero.MIDA - 1;
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
      if (this.RM5_Command == 29)
      {
        while (this.bytes >= 2 && !flag)
        {
          byte[] numArray = this.CopyFlat(2);
          if (numArray[1] == (byte) 128)
            break;
          int num = (int) numArray[1] + ((int) numArray[0] & 63) * 256;
          switch (((int) numArray[0] & 192) >> 6)
          {
            case 0:
              num /= 10;
              break;
            case 1:
              num = num;
              break;
            case 2:
              num *= 10;
              break;
            case 3:
              num *= 100;
              break;
          }
          if (num != 100 && num != 50 && num != 200 && num != 20)
            num = 0;
          if (num > 0)
            this.m_Creditos = num;
          this.Purge(2);
        }
      }
      else
      {
        while (this.bytes >= 1 && !flag)
        {
          byte[] numArray = this.CopyFlat(this.bytes);
          switch (this.RM5_Command)
          {
            case -1:
              this.Purge(1);
              break;
            case 0:
              if (numArray[0] == (byte) 0)
                this.RM5_Command = -1;
              this.Purge(1);
              break;
            case 1:
              if (this.bytes >= 2 && (numArray[0] == (byte) 0 && numArray[1] == (byte) 0))
              {
                this.RM5_Command = -1;
                this.Purge(1);
              }
              this.Purge(1);
              break;
            case 10:
              if (this.bytes >= 5)
              {
                if (numArray[0] == (byte) 0 && numArray[1] == (byte) 0 && ((int) numArray[2] & 15) == 5)
                {
                  this.Purge(4);
                  this.RM5_OK = 1;
                  this.RM5_Command = -1;
                }
                if (numArray[0] == (byte) 0 && numArray[1] == (byte) 0 && ((int) numArray[2] & 15) == 6)
                {
                  this.Purge(4);
                  this.RM5_OK = 2;
                  this.RM5_Command = -1;
                }
              }
              this.Purge(1);
              break;
          }
        }
      }
    }

    public bool Parser()
    {
      if (this.comm == null)
        return false;
      int mida = 0;
      try
      {
        mida = this.comm.Bytes;
      }
      catch
      {
      }
      if (mida > 0)
      {
        byte[] numArray = new byte[0];
        try
        {
          numArray = this.comm.Read(mida);
        }
        catch
        {
        }
        for (int index = 0; index < numArray.Length; ++index)
        {
          this.buffer[this.final] = numArray[index];
          this.final = ++this.final & Control_Comestero.MIDA - 1;
          ++this.bytes;
          if (this.bytes >= Control_Comestero.MIDA)
          {
            this.inici &= ++this.inici & Control_Comestero.MIDA - 1;
            this.bytes = Control_Comestero.MIDA - 1;
          }
        }
      }
      this.Decode();
      return true;
    }

    public bool Poll()
    {
      if (this.comm != null)
        return this.Command((byte) 29);
      return false;
    }

    public bool Poll_Null()
    {
      if (this.comm != null)
        return this.Command((byte) 1);
      return false;
    }

    public bool Command(byte _val)
    {
      if (this.comm == null)
        return false;
      byte[] data = new byte[2]{ (byte) 0, _val };
      try
      {
        this.comm.Write(data);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public bool Enable()
    {
      if (this.comm == null)
        return false;
      this.RM5_Command = 29;
      this.SwapControl = 0;
      return true;
    }

    public bool Disable()
    {
      if (this.comm == null)
        return false;
      this.RM5_Command = -1;
      this.SwapControl = 0;
      return true;
    }
  }
}
