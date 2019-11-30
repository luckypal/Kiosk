// Decompiled with JetBrains decompiler
// Type: GLib.Devices.Control_NV_SSP_P6
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace GLib.Devices
{
  public class Control_NV_SSP_P6
  {
    private static int[] CRC_Table = new int[256]
    {
      0,
      32773,
      32783,
      10,
      32795,
      30,
      20,
      32785,
      32819,
      54,
      60,
      32825,
      40,
      32813,
      32807,
      34,
      32867,
      102,
      108,
      32873,
      120,
      32893,
      32887,
      114,
      80,
      32853,
      32863,
      90,
      32843,
      78,
      68,
      32833,
      32963,
      198,
      204,
      32969,
      216,
      32989,
      32983,
      210,
      240,
      33013,
      33023,
      250,
      33003,
      238,
      228,
      32993,
      160,
      32933,
      32943,
      170,
      32955,
      190,
      180,
      32945,
      32915,
      150,
      156,
      32921,
      136,
      32909,
      32903,
      130,
      33155,
      390,
      396,
      33161,
      408,
      33181,
      33175,
      402,
      432,
      33205,
      33215,
      442,
      33195,
      430,
      420,
      33185,
      480,
      33253,
      33263,
      490,
      33275,
      510,
      500,
      33265,
      33235,
      470,
      476,
      33241,
      456,
      33229,
      33223,
      450,
      320,
      33093,
      33103,
      330,
      33115,
      350,
      340,
      33105,
      33139,
      374,
      380,
      33145,
      360,
      33133,
      33127,
      354,
      33059,
      294,
      300,
      33065,
      312,
      33085,
      33079,
      306,
      272,
      33045,
      33055,
      282,
      33035,
      270,
      260,
      33025,
      33539,
      774,
      780,
      33545,
      792,
      33565,
      33559,
      786,
      816,
      33589,
      33599,
      826,
      33579,
      814,
      804,
      33569,
      864,
      33637,
      33647,
      874,
      33659,
      894,
      884,
      33649,
      33619,
      854,
      860,
      33625,
      840,
      33613,
      33607,
      834,
      960,
      33733,
      33743,
      970,
      33755,
      990,
      980,
      33745,
      33779,
      1014,
      1020,
      33785,
      1000,
      33773,
      33767,
      994,
      33699,
      934,
      940,
      33705,
      952,
      33725,
      33719,
      946,
      912,
      33685,
      33695,
      922,
      33675,
      910,
      900,
      33665,
      640,
      33413,
      33423,
      650,
      33435,
      670,
      660,
      33425,
      33459,
      694,
      700,
      33465,
      680,
      33453,
      33447,
      674,
      33507,
      742,
      748,
      33513,
      760,
      33533,
      33527,
      754,
      720,
      33493,
      33503,
      730,
      33483,
      718,
      708,
      33473,
      33347,
      582,
      588,
      33353,
      600,
      33373,
      33367,
      594,
      624,
      33397,
      33407,
      634,
      33387,
      622,
      612,
      33377,
      544,
      33317,
      33327,
      554,
      33339,
      574,
      564,
      33329,
      33299,
      534,
      540,
      33305,
      520,
      33293,
      33287,
      514
    };
    private static int MIDA = 16384;
    public readonly string ID = "SSP3";
    public readonly int MaxCanales = 16;
    private bool m_Test = false;
    public int TimeOutComs = 0;
    public bool OnLine;
    public CommPort comm;
    public string port;
    public int[] Canal;
    public int[] eCanal;
    public int[] iCanal;
    public int Canales;
    public bool Enabled;
    public string Currency;
    public int Base;
    public int Multiplier;
    public string Firmware;
    public byte Device_ID;
    public string SerialNumber;
    public byte ProtocolVersion;
    public byte Error;
    private int inici;
    private int final;
    private int bytes;
    private byte[] buffer;
    private int m_Creditos;
    public bool m_Respuesta;
    private byte poll_mask;
    private byte last_cmd;
    private bool version;
    public string _f_resp_scom;
    public string _f_com;
    private string[] _f_ports;
    private int _f_cnt;

    public Control_NV_SSP_P6()
    {
      this.port = "COM1";
      this.comm = (CommPort) null;
      this.OnLine = false;
      this.Enabled = false;
      this.inici = 0;
      this.final = 0;
      this.bytes = 0;
      this.ProtocolVersion = (byte) 0;
      this.Device_ID = (byte) 0;
      this.Creditos = 0;
      this.poll_mask = (byte) 0;
      this.Multiplier = 1;
      this.Base = 100;
      this.m_Respuesta = false;
      this.m_Test = false;
      this.Canales = this.MaxCanales;
      this.Currency = "EUR";
      this.buffer = new byte[Control_NV_SSP_P6.MIDA];
      this.Canal = new int[this.MaxCanales];
      this.eCanal = new int[this.MaxCanales];
      this.iCanal = new int[this.MaxCanales];
      for (int index = 0; index < this.MaxCanales; ++index)
      {
        this.Canal[index] = 0;
        this.eCanal[index] = 0;
        this.iCanal[index] = 0;
      }
    }

    public static byte[] GetCRC(byte[] inData)
    {
      byte num1 = byte.MaxValue;
      byte num2 = byte.MaxValue;
      for (int index1 = 0; index1 < inData.Length; ++index1)
      {
        int index2 = (int) inData[index1] ^ (int) num2;
        num2 = (byte) ((uint) (Control_NV_SSP_P6.CRC_Table[index2] >> 8) ^ (uint) num1);
        num1 = (byte) (Control_NV_SSP_P6.CRC_Table[index2] & (int) byte.MaxValue);
      }
      return new byte[2]{ num1, num2 };
    }

    public static bool IsCRCValid(byte[] inData)
    {
      if (inData.Length < 5)
        return false;
      byte[] inData1 = new byte[inData.Length - 3];
      Array.Copy((Array) inData, 1, (Array) inData1, 0, inData.Length - 3);
      byte[] crc = Control_NV_SSP_P6.GetCRC(inData1);
      return (int) crc[0] == (int) inData[inData.Length - 2] && (int) crc[1] == (int) inData[inData.Length - 1];
    }

    public bool Open()
    {
      if (this.comm != null)
      {
        this.OnLine = false;
        this.comm.Close();
        this.comm = (CommPort) null;
      }
      this.TimeOutComs = 0;
      if (!this.port.ToUpper().Contains("COM"))
        return false;
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
      this.comm.Open(this.port, 9600, Parity.None, 8, StopBits.Two);
      this.OnLine = false;
      this.Enabled = false;
      this._f_resp_scom = this.port;
      return this.Startup();
    }

    public bool Startup()
    {
      this.SerialNumber = "";
      int num1 = 2;
      this.m_Respuesta = false;
      while (this.SerialNumber == "" && !this.m_Respuesta && num1 > 0)
      {
        --num1;
        this.poll_mask = (byte) 128;
        this.Command(new byte[1]{ (byte) 17 });
        if (this.m_Respuesta)
        {
          this.Command(new byte[1]{ (byte) 9 });
          int num2 = 3;
          do
          {
            this.version = true;
            this.Command(new byte[2]
            {
              (byte) 6,
              (byte) num2
            });
            if (this.version)
              ++num2;
          }
          while (this.version);
          if (num2 - 1 > 3)
          {
            this.comm.Close();
            return false;
          }
          this.Command(new byte[2]{ (byte) 6, (byte) 3 });
          this.Command(new byte[1]{ (byte) 12 });
          this.Command(new byte[1]{ (byte) 5 });
          this.Inhibits();
        }
      }
      if (this.SerialNumber == "")
      {
        this.comm.Close();
        this.OnLine = false;
        return false;
      }
      this.OnLine = true;
      return true;
    }

    public void Wait_Resposta()
    {
      Thread.Sleep(100);
      DateTime dateTime = DateTime.Now.AddMilliseconds(500.0);
      this.m_Respuesta = false;
      while (!this.m_Respuesta)
      {
        this.Parser();
        if (DateTime.Now > dateTime)
        {
          this.version = false;
          break;
        }
      }
    }

    public bool Close()
    {
      if (this.comm != null)
      {
        this.Disable();
        Thread.Sleep(100);
        this.Poll();
      }
      if (this.comm != null)
        this.comm.Close();
      this.OnLine = false;
      return true;
    }

    public static byte[] InsertByteStuff(byte[] inData)
    {
      List<byte> byteList = new List<byte>(10);
      for (int index = 0; index < inData.Length; ++index)
      {
        if (inData[index] == (byte) 127)
        {
          byteList.Add(inData[index]);
          byteList.Add(inData[index]);
        }
        else
          byteList.Add(inData[index]);
      }
      return byteList.ToArray();
    }

    public byte[] Build_Command(byte[] cmd)
    {
      int num1 = 0;
      byte[] inData1 = new byte[cmd.Length + 2];
      byte[] numArray1 = inData1;
      int index1 = num1;
      int num2 = index1 + 1;
      int num3 = (int) (byte) ((uint) this.Device_ID | (uint) this.poll_mask);
      numArray1[index1] = (byte) num3;
      byte[] numArray2 = inData1;
      int index2 = num2;
      int num4 = index2 + 1;
      int length = (int) (byte) cmd.Length;
      numArray2[index2] = (byte) length;
      this.last_cmd = cmd[0];
      for (int index3 = 0; index3 < cmd.Length; ++index3)
        inData1[num4++] = cmd[index3];
      byte[] crc = Control_NV_SSP_P6.GetCRC(inData1);
      byte[] inData2 = new byte[cmd.Length + 5];
      inData2[0] = (byte) 0;
      for (int index3 = 0; index3 < inData1.Length; ++index3)
        inData2[index3 + 1] = inData1[index3];
      inData2[inData2.Length - 2] = crc[0];
      inData2[inData2.Length - 1] = crc[1];
      byte[] numArray3 = Control_NV_SSP_P6.InsertByteStuff(inData2);
      numArray3[0] = (byte) 127;
      this.poll_mask ^= (byte) 128;
      return numArray3;
    }

    public string Find_Device()
    {
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

    public void Start_Find_Device()
    {
      this._f_resp_scom = "-";
      this._f_ports = SerialPort.GetPortNames();
      this._f_cnt = 0;
    }

    public bool Last_Find_Device()
    {
      if (this._f_ports == null)
        return false;
      return this._f_cnt < this._f_ports.Length;
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
        this.m_Respuesta = false;
        this._f_resp_scom = "-";
        return true;
      }
      if (this._f_cnt < this._f_ports.Length)
      {
        this.m_Respuesta = false;
        this._f_resp_scom = "-";
        this._f_com = this._f_ports[this._f_cnt];
        this.port = this._f_com;
        ++this._f_cnt;
        return this.Open();
      }
      this.m_Respuesta = false;
      this._f_resp_scom = "-";
      return true;
    }

    public bool Poll_Find_Device()
    {
      this.m_Respuesta = false;
      return this.Next_Find_Device();
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
      if (_c <= this.Canales)
        return this.iCanal[_c - 1] == 0;
      return true;
    }

    public Decimal GetChannelValue(int _c)
    {
      if (_c <= this.Canales)
        return (Decimal) (this.Canal[_c - 1] * this.Multiplier * this.Base);
      return new Decimal(0);
    }

    public bool GetChannelEnabled(int _c)
    {
      return this.Canal[_c - 1] > 0 && this.eCanal[_c - 1] != 4;
    }

    public string GetChannelCurrency(int _c)
    {
      return this.Currency;
    }

    private byte[] CopyFlat(int len)
    {
      byte[] numArray = new byte[len];
      for (int index1 = 0; index1 < len; ++index1)
      {
        int index2 = this.inici + index1 & Control_NV_SSP_P6.MIDA - 1;
        numArray[index1] = this.buffer[index2];
      }
      return numArray;
    }

    private void Purge(int len)
    {
      this.inici = this.inici + len & Control_NV_SSP_P6.MIDA - 1;
      this.bytes -= len;
      if (this.bytes >= 0)
        return;
      this.bytes = 0;
      this.inici = 0;
      this.final = 0;
    }

    public void Reset()
    {
      this.Command(new byte[1]{ (byte) 1 });
      this.poll_mask = (byte) 128;
      this.comm.Write(this.Build_Command(new byte[1]
      {
        (byte) 17
      }));
      this.Wait_Resposta();
    }

    public void Decode()
    {
      bool flag = false;
      int num1 = 0;
      while (this.bytes >= 6 && !flag)
      {
        int num2 = 0;
        byte[] numArray = this.CopyFlat(3);
        if (numArray[0] == (byte) 127 && (int) numArray[1] == (int) (byte) ((uint) this.Device_ID | (uint) this.poll_mask ^ 128U))
        {
          byte num3 = (byte) (5U + (uint) numArray[2]);
          if (this.bytes >= (int) num3)
          {
            byte[] inData = this.CopyFlat((int) num3);
            if (Control_NV_SSP_P6.IsCRCValid(inData))
            {
              num2 = 1;
              switch (inData[3])
              {
                case 240:
                  this.m_Respuesta = true;
                  if (num3 >= (byte) 7)
                  {
                    switch (inData[4])
                    {
                      case 204:
                      case 235:
                        if (inData[5] == (byte) 238)
                        {
                          this.Creditos = this.Canal[(int) inData[6] - 1] * this.Multiplier * this.Base;
                          break;
                        }
                        break;
                      case 232:
                        num1 = 1;
                        break;
                      case 238:
                        this.Creditos = this.Canal[(int) inData[5] - 1] * this.Multiplier * this.Base;
                        break;
                      case 239:
                        if (inData[6] == (byte) 238)
                        {
                          this.Creditos = this.Canal[(int) inData[6] - 1] * this.Multiplier * this.Base;
                          break;
                        }
                        break;
                    }
                  }
                  switch (this.last_cmd)
                  {
                    case 5:
                      byte[] bytes1 = new byte[4];
                      Array.Copy((Array) inData, 5, (Array) bytes1, 0, 4);
                      this.Firmware = Encoding.UTF8.GetString(bytes1);
                      byte[] bytes2 = new byte[3];
                      Array.Copy((Array) inData, 9, (Array) bytes2, 0, 3);
                      this.Currency = Encoding.UTF8.GetString(bytes2);
                      this.Multiplier = (int) inData[12] * 65536 + (int) inData[13] * 256 + (int) inData[14];
                      this.Canales = (int) inData[15];
                      int num4 = 16;
                      for (int index = 0; index < this.MaxCanales; ++index)
                      {
                        this.Canal[index] = 0;
                        this.eCanal[index] = 0;
                        this.iCanal[index] = 0;
                      }
                      for (int index = 0; index < this.Canales; ++index)
                      {
                        this.Canal[index] = (int) inData[num4 + index];
                        this.eCanal[index] = (int) inData[num4 + index + this.Canales];
                        this.iCanal[index] = this.eCanal[index] == 4 ? 0 : 1;
                      }
                      this.ProtocolVersion = inData[num4 + this.Canales * 2 + 3];
                      break;
                    case 12:
                      this.SerialNumber = string.Concat((object) ((int) inData[4] * 16777216 + (int) inData[5] * 65536 + (int) inData[6] * 256 + (int) inData[7]));
                      break;
                  }
                case 242:
                case 243:
                case 244:
                case 245:
                case 246:
                case 250:
                  this.Error = inData[3];
                  this.m_Respuesta = true;
                  break;
                case 248:
                  this.Error = inData[3];
                  if (this.last_cmd == (byte) 6)
                  {
                    this.version = false;
                    break;
                  }
                  break;
              }
              this.Purge((int) num3);
            }
          }
          if (num1 == 0)
            this.TimeOutComs = 0;
        }
        if (num2 == 0)
          this.Purge(1);
      }
    }

    public bool Parser()
    {
      ++this.TimeOutComs;
      if (this.comm == null)
        return false;
      int bytes = this.comm.Bytes;
      if (bytes > 0)
      {
        foreach (byte num in this.comm.Read(bytes))
        {
          this.buffer[this.final] = num;
          this.final = ++this.final & Control_NV_SSP_P6.MIDA - 1;
          ++this.bytes;
          if (this.bytes >= Control_NV_SSP_P6.MIDA)
          {
            this.inici &= ++this.inici & Control_NV_SSP_P6.MIDA - 1;
            this.bytes = Control_NV_SSP_P6.MIDA - 1;
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
      this.Parser();
      this.comm.Write(this.Build_Command(new byte[1]
      {
        (byte) 7
      }));
      return this.TimeOutComs <= 100;
    }

    public bool Inhibits()
    {
      if (this.comm == null)
        return false;
      byte num1 = 1;
      byte num2 = 0;
      int num3 = 0;
      while (num3 < 8)
      {
        if (!this.GetChannelInhibit(num3 + 1))
          num2 |= num1;
        ++num3;
        num1 <<= 1;
      }
      byte num4 = 1;
      byte num5 = 0;
      int num6 = 8;
      while (num6 < 16)
      {
        if (!this.GetChannelInhibit(num6 + 1))
          num5 |= num4;
        ++num6;
        num4 <<= 1;
      }
      this.Command(new byte[3]{ (byte) 2, num2, num5 });
      return true;
    }

    public bool Enable()
    {
      bool flag = this.Inhibits();
      this.Command(new byte[1]{ (byte) 10 });
      return flag;
    }

    public void Command(byte[] _cmd)
    {
      this.comm.Write(this.Build_Command(_cmd));
      this.Wait_Resposta();
    }

    public bool Disable()
    {
      if (this.comm == null)
        return false;
      this.Command(new byte[3]
      {
        (byte) 2,
        (byte) 0,
        (byte) 0
      });
      return true;
    }
  }
}
