// Decompiled with JetBrains decompiler
// Type: GLib.Devices.Control_NV_SIO
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Threading;

namespace GLib.Devices
{
  public class Control_NV_SIO
  {
    private static int MIDA = 16384;
    public int Canales = 16;
    public Decimal Base = new Decimal(100);
    private bool test = false;
    private string[] _f_ports;
    public string _f_resp_scom;
    public bool OnLine;
    public CommPort comm;
    public string port;
    public int[] Canal;
    public int[] eCanal;
    public int CriticalJamp;
    private int inici;
    private int final;
    private int bytes;
    private byte[] buffer;
    private int m_Creditos;
    private bool respuesta;

    public Control_NV_SIO()
    {
      this.comm = (CommPort) null;
      this.inici = 0;
      this.final = 0;
      this.bytes = 0;
      this.CriticalJamp = 0;
      this.respuesta = false;
      this.buffer = new byte[Control_NV_SIO.MIDA];
      this.port = "COM1";
      this._f_resp_scom = "-";
      this.Creditos = 0;
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
      this.eCanal[5] = 1;
      this.Canal[0] = 500;
      this.Canal[1] = 1000;
      this.Canal[2] = 2000;
      this.Canal[3] = 5000;
      this.Canal[4] = 10000;
      this.Canal[5] = 20000;
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
      this.Canal[0] = 0;
      this.Canal[1] = 0;
      this.Canal[2] = 500;
      this.Canal[3] = 1000;
      this.Canal[4] = 2000;
      this.Canal[5] = 5000;
    }

    public void Set_Dominicana()
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
      this.eCanal[5] = 1;
      this.eCanal[6] = 1;
      this.Canal[0] = 20;
      this.Canal[1] = 50;
      this.Canal[2] = 100;
      this.Canal[3] = 200;
      this.Canal[4] = 500;
      this.Canal[5] = 1000;
      this.Canal[6] = 2000;
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
      this.comm.Open(this.port, 300, Parity.None, 8, StopBits.Two);
      this.OnLine = true;
      this.CriticalJamp = 0;
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
      string str = "-";
      this._f_resp_scom = "-";
      this.comm = new CommPort();
      for (int index1 = 1; index1 < 16; ++index1)
      {
        string port = "COM" + (object) index1;
        this.test = true;
        try
        {
          this.comm.Open(port, 300, Parity.None, 8, StopBits.Two);
          this.respuesta = false;
          for (int index2 = 0; index2 < 5; ++index2)
          {
            this.Command((byte) 182);
            Thread.Sleep(200);
            this.Parser();
            if (this.respuesta)
            {
              this.comm.Close();
              this._f_resp_scom = port;
              this.test = false;
              return port;
            }
            this.Purge(this.bytes);
          }
          this.comm.Close();
        }
        catch
        {
          str = "-";
        }
      }
      this.test = false;
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

    private byte[] CopyFlat(int len)
    {
      byte[] numArray = new byte[len];
      for (int index1 = 0; index1 < len; ++index1)
      {
        int index2 = this.inici + index1 & Control_NV_SIO.MIDA - 1;
        numArray[index1] = this.buffer[index2];
      }
      return numArray;
    }

    private void Purge(int len)
    {
      this.inici = this.inici + len & Control_NV_SIO.MIDA - 1;
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
      while (this.bytes >= 1 && !flag)
      {
        if (this.test)
        {
          byte[] numArray = this.CopyFlat(this.bytes);
          if (numArray[0] == (byte) 182 && this.bytes == 1 || numArray[1] == (byte) 0 && this.bytes == 2 || numArray[2] == (byte) 0 && this.bytes == 3)
            break;
          if (numArray[0] == (byte) 182 && numArray[1] == (byte) 0 && (numArray[2] == (byte) 0 && numArray[3] == (byte) 0) && this.bytes >= 4)
            this.respuesta = true;
        }
        byte[] numArray1 = this.CopyFlat(1);
        switch (numArray1[0])
        {
          case 1:
          case 2:
          case 3:
          case 4:
          case 5:
          case 6:
          case 7:
          case 8:
          case 9:
          case 10:
          case 11:
          case 12:
          case 13:
          case 14:
          case 15:
          case 16:
            int index = (int) numArray1[0] - 1;
            if (this.Canal[index] > 0)
              this.m_Creditos = this.Canal[index];
            if (this.Canal[index] <= 0)
            {
              if (this.CriticalJamp == 0)
                this.CriticalJamp = 1;
              this.m_Creditos = 0;
              break;
            }
            break;
          case 20:
            this.respuesta = true;
            break;
          case 30:
            this.respuesta = true;
            break;
          case 40:
            this.respuesta = true;
            break;
          case 50:
            this.respuesta = true;
            break;
          case 60:
            this.respuesta = true;
            break;
          case 70:
            this.respuesta = true;
            break;
          case 80:
            this.respuesta = true;
            break;
          case 120:
            this.respuesta = true;
            break;
          case 121:
            this.respuesta = true;
            break;
          case byte.MaxValue:
            this.respuesta = true;
            break;
        }
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
          this.final = ++this.final & Control_NV_SIO.MIDA - 1;
          ++this.bytes;
          if (this.bytes >= Control_NV_SIO.MIDA)
          {
            this.inici &= ++this.inici & Control_NV_SIO.MIDA - 1;
            this.bytes = Control_NV_SIO.MIDA - 1;
          }
        }
      }
      this.Decode();
      return true;
    }

    public bool Poll()
    {
      return this.comm != null;
    }

    public bool Command(byte _val)
    {
      if (this.comm == null)
        return false;
      this.comm.Write(new byte[1]{ _val });
      return true;
    }

    public bool Enable()
    {
      this.Command((byte) 184);
      if (this.comm == null)
        return false;
      for (int index = 0; index < this.Canal.Length; ++index)
      {
        Thread.Sleep(40);
        this.Poll();
        this.Parser();
        if (this.Canal[index] <= 0)
          this.Command((byte) (131 + index));
        else
          this.Command((byte) (151 + index));
      }
      return true;
    }

    public bool Disable()
    {
      if (this.comm != null)
        return this.Command((byte) 185);
      return false;
    }
  }
}
