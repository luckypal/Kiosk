// Decompiled with JetBrains decompiler
// Type: GLib.Devices.Control_CCTALK_COIN
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
  public class Control_CCTALK_COIN
  {
    private static int MIDA = 16384;
    public readonly string ID = "CCTALK";
    public readonly int MaxCanales = 32;
    public bool EnablePoll = false;
    private bool m_Test = false;
    private int lastPoll = 0;
    private int CntTimeOut = 0;
    public byte[] Send_Command;
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
    public string Coin_Acceptor_Status;
    public string Coin_Acceptor_Speed;
    public string Coin_Acceptor_Fraud;
    public string Coin_Acceptor_Rejected;
    public string Coin_Acceptor_Accepted;
    public string Coin_Acceptor_Inserted;
    public string Coin_Acceptor_Product;
    public string Coin_Acceptor_Build;
    public string Coin_Acceptor_Manufacturer;
    public string Coin_Acceptor_Serial;
    public string Coin_Acceptor_Software;
    public string Coin_Acceptor_Comms;
    public string Coin_Acceptor_CheckSum;
    public string Coin_Acceptor_Bank;
    public string Coin_Acceptor_Test;
    public bool Coin_Acceptor_Enabled;
    public int Coin_Acceptor_Inhibit;
    public string Coin_Acceptor_Storage;
    public bool _IsCoinPresent;
    public string Last_Cmd_Text;
    public string Last_Packet;
    public string Coin_Acceptor;
    public int Coin_Acceptor_ID;
    public string Coin_Acceptor_Currency;
    public byte[] Last_Data;
    public bool SendPoll;
    public string logs;
    public bool Connected;
    public bool CoinConnected;
    private int inici;
    private int final;
    private int bytes;
    private byte[] buffer;
    private int m_Creditos;
    private int m_Respuesta;
    private byte poll_mask;
    private byte last_cmd;
    private bool version;
    public string _f_resp_scom;
    public string _f_com;
    private string[] _f_ports;
    private int _f_cnt;
    private long TimeoutRead;
    private long CoinTimeoutPoll;
    private long BillTimeoutPoll;
    private int m_Respuesta_Point;
    private int lockCommand;

    public Control_CCTALK_COIN()
    {
      this.port = "COM1";
      this.comm = (CommPort) null;
      this.OnLine = false;
      this.Enabled = false;
      this.inici = 0;
      this.final = 0;
      this.bytes = 0;
      this.lockCommand = 0;
      this.ProtocolVersion = (byte) 0;
      this.Device_ID = (byte) 0;
      this.Creditos = 0;
      this.poll_mask = (byte) 0;
      this.m_Respuesta_Point = 0;
      this.m_Respuesta = 0;
      this.Multiplier = 1;
      this.CoinConnected = false;
      this.Connected = false;
      this.Base = 100;
      this.m_Test = false;
      this.Canales = this.MaxCanales;
      this.Currency = "EUR";
      this.buffer = new byte[Control_CCTALK_COIN.MIDA];
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

    public byte GetCRC(ref byte[] _buffer, int _len)
    {
      byte num = 0;
      for (int index = 0; index < _len; ++index)
        num += _buffer[index];
      return num;
    }

    public byte GetCRC(ref byte[] _buffer)
    {
      return this.GetCRC(ref _buffer, _buffer.Length);
    }

    public bool IsCRCValid(ref byte[] _buffer)
    {
      if (_buffer.Length < 5)
        return false;
      byte crc = this.GetCRC(ref _buffer, _buffer.Length - 1);
      return (int) _buffer[_buffer.Length - 1] == (int) crc;
    }

    public bool Open()
    {
      this.OnLine = false;
      if (this.comm != null)
      {
        this.comm.Close();
        this.comm = (CommPort) null;
      }
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
      this.Send_Command = (byte[]) null;
      this.Connected = false;
      this.EnablePoll = false;
      this.CoinConnected = false;
      this.lockCommand = 0;
      this.comm = new CommPort();
      this.comm.Open(this.port, 9600, Parity.None, 8, StopBits.One);
      this.Enabled = false;
      this._f_resp_scom = this.port;
      return this.Startup();
    }

    public string GetIDString(int _dev, byte[] _cmd)
    {
      string str = "?";
      for (int index = 0; index < 4; ++index)
      {
        this.Command(_dev, new byte[1]{ (byte) 254 });
        if (this.Last_Cmd_Text == "ACK")
        {
          this.Command(_dev, _cmd);
          if (this.Wait_Resposta_Ack() && this.Last_Cmd_Text != "ACK")
          {
            str = this.Last_Cmd_Text;
            break;
          }
        }
      }
      return str;
    }

    public string GetIDSerial(int _dev, byte[] _cmd)
    {
      string str = "?";
      for (int index = 0; index < 4; ++index)
      {
        this.Command(_dev, new byte[1]{ (byte) 254 });
        if (this.Last_Cmd_Text == "ACK")
        {
          this.Command(_dev, _cmd);
          if (this.Wait_Resposta_Ack() && this.Last_Cmd_Text != "ACK")
          {
            int num = 0;
            if (this.Last_Data.Length == 1)
              num = (int) this.Last_Data[0];
            if (this.Last_Data.Length == 2)
              num = (int) this.Last_Data[0] + (int) this.Last_Data[1] * 256;
            if (this.Last_Data.Length == 3)
              num = (int) this.Last_Data[0] + (int) this.Last_Data[1] * 256 + (int) this.Last_Data[2] * 65536;
            if (this.Last_Data.Length == 4)
              num = (int) this.Last_Data[0] + (int) this.Last_Data[1] * 256 + (int) this.Last_Data[2] * 65536 + (int) this.Last_Data[3] * 16777216;
            str = string.Concat((object) num);
            break;
          }
        }
      }
      return str;
    }

    public int GetIDInt(int _dev, byte[] _cmd)
    {
      int num = 0;
      for (int index = 0; index < 4; ++index)
      {
        this.Command(_dev, new byte[1]{ (byte) 254 });
        if (this.Last_Cmd_Text == "ACK")
        {
          this.Command(_dev, _cmd);
          if (this.Wait_Resposta_Ack() && this.Last_Cmd_Text != "ACK")
          {
            num = (int) this.Last_Data[0] + (int) this.Last_Data[1] * 256;
            break;
          }
        }
      }
      return num;
    }

    public string GetIDSInt(int _dev, byte[] _cmd)
    {
      string str = "?";
      for (int index = 0; index < 4; ++index)
      {
        this.Command(_dev, new byte[1]{ (byte) 254 });
        if (this.Last_Cmd_Text == "ACK")
        {
          this.Command(_dev, _cmd);
          if (this.Wait_Resposta_Ack() && this.Last_Cmd_Text != "ACK")
          {
            str = ((int) this.Last_Data[0]).ToString() + (object) ((int) this.Last_Data[1] * 256);
            break;
          }
        }
      }
      return str;
    }

    public int GetIDLong(int _dev, byte[] _cmd)
    {
      int num = 0;
      for (int index = 0; index < 4; ++index)
      {
        this.Command(_dev, new byte[1]{ (byte) 254 });
        if (this.Last_Cmd_Text == "ACK")
        {
          this.Command(_dev, _cmd);
          if (this.Wait_Resposta_Ack() && this.Last_Cmd_Text != "ACK")
          {
            num = (int) this.Last_Data[0] + (int) this.Last_Data[1] * 256 + (int) this.Last_Data[2] * 65536 + (int) this.Last_Data[3] * 16777216;
            break;
          }
        }
      }
      return num;
    }

    public string GetIDSLong(int _dev, byte[] _cmd, int _x)
    {
      string str = "?";
      for (int index = 0; index < 4; ++index)
      {
        this.Command(_dev, new byte[1]{ (byte) 254 });
        if (this.Last_Cmd_Text == "ACK")
        {
          this.Command(_dev, _cmd);
          if (this.Wait_Resposta_Ack() && this.Last_Cmd_Text != "ACK")
          {
            int num = (int) this.Last_Data[0] + (int) this.Last_Data[1] * 256 + (int) this.Last_Data[2] * 65536 + (int) this.Last_Data[3] * 16777216;
            str = _x != 16 ? string.Format("{0}", (object) num) : string.Format("{0:X08}", (object) num);
            break;
          }
        }
      }
      return str;
    }

    public string GetIDVer(int _dev, byte[] _cmd)
    {
      string str = "?";
      for (int index = 0; index < 4; ++index)
      {
        this.Command(_dev, new byte[1]{ (byte) 254 });
        if (this.Last_Cmd_Text == "ACK")
        {
          this.Command(_dev, _cmd);
          if (this.Wait_Resposta_Ack() && this.Last_Cmd_Text != "ACK")
          {
            str = ((int) this.Last_Data[0]).ToString() + "." + (object) this.Last_Data[1] + "." + (object) this.Last_Data[2];
            break;
          }
        }
      }
      return str;
    }

    public string GetIDTest(int _dev, byte[] _cmd)
    {
      string str = "";
      for (int index = 0; index < 4; ++index)
      {
        this.Command(_dev, new byte[1]{ (byte) 254 });
        if (this.Last_Cmd_Text == "ACK")
        {
          this.Command(_dev, _cmd);
          if (this.Wait_Resposta_Ack() && this.Last_Cmd_Text != "ACK")
          {
            str = this.Last_Data[0] == (byte) 0 ? "OK" : "ERROR " + (object) this.Last_Data[0];
            if (this.Last_Data.Length <= 1)
              break;
            break;
          }
        }
      }
      return str;
    }

    public string GetIDStorage(int _dev, byte[] _cmd)
    {
      string str = "";
      for (int index = 0; index < 4; ++index)
      {
        this.Command(_dev, new byte[1]{ (byte) 254 });
        if (this.Last_Cmd_Text == "ACK")
        {
          this.Command(_dev, _cmd);
          if (this.Wait_Resposta_Ack() && this.Last_Cmd_Text != "ACK")
          {
            switch (this.Last_Data[0])
            {
              case 0:
              case 1:
                str = "TYPE RAM";
                goto label_9;
              case 2:
                str = "TYPE EEPROM";
                goto label_9;
              case 3:
                str = "TYPE FRAM / BATTERY RAM";
                goto label_9;
              default:
                goto label_9;
            }
          }
        }
      }
label_9:
      return str;
    }

    public string GetIDSpeed(int _dev, byte[] _cmd)
    {
      string str = "";
      for (int index = 0; index < 4; ++index)
      {
        this.Command(_dev, new byte[1]{ (byte) 254 });
        if (this.Last_Cmd_Text == "ACK")
        {
          this.Command(_dev, _cmd);
          if (this.Wait_Resposta_Ack() && this.Last_Cmd_Text != "ACK")
          {
            switch (this.Last_Data[0])
            {
              case 0:
                str = "?";
                goto label_15;
              case 1:
                str = ((int) this.Last_Data[1]).ToString() + "ms";
                goto label_15;
              case 2:
                str = ((int) this.Last_Data[1] * 10).ToString() + "ms";
                goto label_15;
              case 3:
                str = ((int) this.Last_Data[1]).ToString() + "seconds";
                goto label_15;
              case 4:
                str = ((int) this.Last_Data[1]).ToString() + "min";
                goto label_15;
              case 5:
                str = ((int) this.Last_Data[1]).ToString() + "hours";
                goto label_15;
              case 6:
                str = ((int) this.Last_Data[1]).ToString() + "days";
                goto label_15;
              case 7:
                str = ((int) this.Last_Data[1]).ToString() + "months";
                goto label_15;
              case 8:
                str = ((int) this.Last_Data[1]).ToString() + "years";
                goto label_15;
              default:
                goto label_15;
            }
          }
        }
      }
label_15:
      return str;
    }

    public string GetIDSByte(int _dev, byte[] _cmd)
    {
      string str = "?";
      for (int index = 0; index < 4; ++index)
      {
        this.Command(_dev, new byte[1]{ (byte) 254 });
        if (this.Last_Cmd_Text == "ACK")
        {
          this.Command(_dev, _cmd);
          if (this.Wait_Resposta_Ack() && this.Last_Cmd_Text != "ACK")
          {
            str = string.Concat((object) this.Last_Data[0]);
            break;
          }
        }
      }
      return str;
    }

    public byte GetIDByte(int _dev, byte[] _cmd)
    {
      byte num = 0;
      for (int index = 0; index < 4; ++index)
      {
        this.Command(_dev, new byte[1]{ (byte) 254 });
        if (this.Last_Cmd_Text == "ACK")
        {
          this.Command(_dev, _cmd);
          if (this.Wait_Resposta_Ack() && this.Last_Cmd_Text != "ACK")
          {
            num = this.Last_Data[0];
            break;
          }
        }
      }
      return num;
    }

    public bool GetIDBool(int _dev, byte[] _cmd)
    {
      bool flag = false;
      for (int index = 0; index < 4; ++index)
      {
        this.Command(_dev, new byte[1]{ (byte) 254 });
        if (this.Last_Cmd_Text == "ACK")
        {
          this.Command(_dev, _cmd);
          if (this.Wait_Resposta_Ack() && this.Last_Cmd_Text != "ACK")
          {
            flag = ((int) this.Last_Data[0] & 1) == 1;
            break;
          }
        }
      }
      return flag;
    }

    public string GetIDSBool(int _dev, byte[] _cmd)
    {
      string str = "?";
      for (int index = 0; index < 4; ++index)
      {
        this.Command(_dev, new byte[1]{ (byte) 254 });
        if (this.Last_Cmd_Text == "ACK")
        {
          this.Command(_dev, _cmd);
          if (this.Wait_Resposta_Ack() && this.Last_Cmd_Text != "ACK")
          {
            str = string.Concat((object) (((int) this.Last_Data[0] & 1) == 1));
            break;
          }
        }
      }
      return str;
    }

    public string GetCoinAcceptorInfo()
    {
      return this.Coin_Acceptor_Product + "\r\n" + this.Coin_Acceptor_Manufacturer + "\r\n" + this.Coin_Acceptor_Currency;
    }

    public bool IsAscii(char _c)
    {
      return _c >= 'a' && _c <= 'z' || _c >= 'A' && _c <= 'Z';
    }

    public bool IsAsciiNumbers(char _c)
    {
      return _c >= 'a' && _c <= 'z' || _c >= 'A' && _c <= 'Z' || _c >= '0' && _c <= '1';
    }

    public int CVF_Credits(string _cvf)
    {
      char ch1 = _cvf[2];
      char ch2 = _cvf[3];
      char ch3 = _cvf[4];
      if (ch1 == '.')
        ch1 = '0';
      if (ch2 == '.')
        ch2 = '0';
      if (ch3 == '.')
        ch3 = '0';
      string s = ((int) ch1).ToString() + (object) ch2 + (object) ch3;
      int num = 0;
      try
      {
        num = int.Parse(s);
      }
      catch
      {
      }
      return num;
    }

    public string CVF_Credits_Currency(int _c)
    {
      return string.Format("{0:0.00}", (object) ((Decimal) _c / new Decimal(100)));
    }

    public string GetChannelDescription(int _c)
    {
      int num = 0;
      if (_c <= this.Canales)
        num = this.Canal[_c - 1];
      return string.Format("{0:0.00} {1}", (object) num, (object) this.GetChannelCurrency(_c));
    }

    public string CVF_Currency(string _cvf)
    {
      switch ((((int) _cvf[0]).ToString() + (object) _cvf[1]).ToLower())
      {
        case "ar":
          return "Peso (Argentina)";
        case "eu":
          return "Euro";
        case "br":
          return "Real (Brazil)";
        case "ch":
          return "Franc (Switzerland)";
        default:
          return "Credits";
      }
    }

    public bool GetInfo(int _dev, string _cmd)
    {
      switch (_cmd)
      {
        case "TYPE":
          this.Command(_dev, new byte[1]{ (byte) 245 });
          return true;
        case "PRODUCT_COIN":
          this.Coin_Acceptor_Product = this.GetIDString(_dev, new byte[1]
          {
            (byte) 244
          });
          Control_CCTALK_COIN controlCctalkCoin1 = this;
          controlCctalkCoin1.logs = controlCctalkCoin1.logs + "Product: " + this.Coin_Acceptor_Product + "\r\n";
          return true;
        case "BUILD_COIN":
          this.Coin_Acceptor_Build = this.GetIDString(_dev, new byte[1]
          {
            (byte) 192
          });
          Control_CCTALK_COIN controlCctalkCoin2 = this;
          controlCctalkCoin2.logs = controlCctalkCoin2.logs + "Build: " + this.Coin_Acceptor_Build + "\r\n";
          return true;
        case "MANUFACTURER_COIN":
          this.Coin_Acceptor_Manufacturer = this.GetIDString(_dev, new byte[1]
          {
            (byte) 246
          });
          Control_CCTALK_COIN controlCctalkCoin3 = this;
          controlCctalkCoin3.logs = controlCctalkCoin3.logs + "Manufacturer: " + this.Coin_Acceptor_Manufacturer + "\r\n";
          return true;
        case "SERIAL_COIN":
          this.Coin_Acceptor_Serial = this.GetIDSerial(_dev, new byte[1]
          {
            (byte) 242
          });
          Control_CCTALK_COIN controlCctalkCoin4 = this;
          controlCctalkCoin4.logs = controlCctalkCoin4.logs + "Serial: " + this.Coin_Acceptor_Serial + "\r\n";
          return true;
        case "SOFT_COIN":
          this.Coin_Acceptor_Software = this.GetIDString(_dev, new byte[1]
          {
            (byte) 241
          });
          Control_CCTALK_COIN controlCctalkCoin5 = this;
          controlCctalkCoin5.logs = controlCctalkCoin5.logs + "Software: " + this.Coin_Acceptor_Software + "\r\n";
          return true;
        case "COMMS_COIN":
          this.Coin_Acceptor_Comms = this.GetIDVer(_dev, new byte[1]
          {
            (byte) 4
          });
          Control_CCTALK_COIN controlCctalkCoin6 = this;
          controlCctalkCoin6.logs = controlCctalkCoin6.logs + "Comms: " + this.Coin_Acceptor_Comms + "\r\n";
          return true;
        case "ENABLED_COIN":
          this.Coin_Acceptor_Enabled = this.GetIDBool(_dev, new byte[1]
          {
            (byte) 227
          });
          Control_CCTALK_COIN controlCctalkCoin7 = this;
          controlCctalkCoin7.logs = controlCctalkCoin7.logs + "Enabled: " + (object) this.Coin_Acceptor_Enabled + "\r\n";
          return true;
        case "INHIBIT_COIN":
          this.Coin_Acceptor_Inhibit = this.GetIDInt(_dev, new byte[1]
          {
            (byte) 230
          });
          Control_CCTALK_COIN controlCctalkCoin8 = this;
          controlCctalkCoin8.logs = controlCctalkCoin8.logs + "Inhibits: " + (object) this.Coin_Acceptor_Inhibit + "\r\n";
          return true;
        case "CHECKSUM_COIN":
          this.Coin_Acceptor_CheckSum = this.GetIDSLong(_dev, new byte[1]
          {
            (byte) 197
          }, 16);
          Control_CCTALK_COIN controlCctalkCoin9 = this;
          controlCctalkCoin9.logs = controlCctalkCoin9.logs + "Checksum: " + this.Coin_Acceptor_CheckSum + "\r\n";
          return true;
        case "TEST_COIN":
          this.Coin_Acceptor_Test = this.GetIDTest(_dev, new byte[1]
          {
            (byte) 232
          });
          Control_CCTALK_COIN controlCctalkCoin10 = this;
          controlCctalkCoin10.logs = controlCctalkCoin10.logs + "Status: " + this.Coin_Acceptor_Test + "\r\n";
          return true;
        case "CLEAR_COIN":
          this.Command(_dev, new byte[1]{ (byte) 3 });
          return true;
        case "STORAGE_COIN":
          this.Coin_Acceptor_Storage = this.GetIDStorage(_dev, new byte[1]
          {
            (byte) 216
          });
          Control_CCTALK_COIN controlCctalkCoin11 = this;
          controlCctalkCoin11.logs = controlCctalkCoin11.logs + "Storage: " + this.Coin_Acceptor_Storage + "\r\n";
          return true;
        case "STATUS_COIN":
          this.Coin_Acceptor_Status = this.GetIDTest(_dev, new byte[1]
          {
            (byte) 248
          });
          Control_CCTALK_COIN controlCctalkCoin12 = this;
          controlCctalkCoin12.logs = controlCctalkCoin12.logs + "Status: " + this.Coin_Acceptor_Status + "\r\n";
          return true;
        case "SPEED_COIN":
          this.Coin_Acceptor_Speed = this.GetIDSpeed(_dev, new byte[1]
          {
            (byte) 249
          });
          Control_CCTALK_COIN controlCctalkCoin13 = this;
          controlCctalkCoin13.logs = controlCctalkCoin13.logs + "Speed: " + this.Coin_Acceptor_Speed + "\r\n";
          return true;
        case "INSERTED_COIN":
          this.Coin_Acceptor_Inserted = this.GetIDSerial(_dev, new byte[1]
          {
            (byte) 226
          });
          Control_CCTALK_COIN controlCctalkCoin14 = this;
          controlCctalkCoin14.logs = controlCctalkCoin14.logs + "Inserted coins: " + this.Coin_Acceptor_Inserted + "\r\n";
          return true;
        case "ACCEPTED_COIN":
          this.Coin_Acceptor_Accepted = this.GetIDSerial(_dev, new byte[1]
          {
            (byte) 225
          });
          Control_CCTALK_COIN controlCctalkCoin15 = this;
          controlCctalkCoin15.logs = controlCctalkCoin15.logs + "Accepted coins: " + this.Coin_Acceptor_Accepted + "\r\n";
          return true;
        case "FRAUD_COIN":
          this.Coin_Acceptor_Fraud = this.GetIDSerial(_dev, new byte[1]
          {
            (byte) 193
          });
          Control_CCTALK_COIN controlCctalkCoin16 = this;
          controlCctalkCoin16.logs = controlCctalkCoin16.logs + "Fraud coins: " + this.Coin_Acceptor_Fraud + "\r\n";
          return true;
        case "REJECTED_COIN":
          this.Coin_Acceptor_Rejected = this.GetIDSerial(_dev, new byte[1]
          {
            (byte) 194
          });
          Control_CCTALK_COIN controlCctalkCoin17 = this;
          controlCctalkCoin17.logs = controlCctalkCoin17.logs + "Rejected coins: " + this.Coin_Acceptor_Rejected + "\r\n";
          return true;
        case "RESET":
          this.Command(_dev, new byte[1]{ (byte) 1 });
          this.logs += "!RESET\r\n";
          this.lastPoll = 0;
          return true;
        case "POLL":
          this.Command(_dev, new byte[1]{ (byte) 254 });
          return true;
        case "CURRENCY_COIN":
          this.Coin_Acceptor_Currency = "";
          int num1 = 0;
          int num2 = 1;
          while (num2 < 8)
          {
            this.Command(_dev, new byte[1]{ (byte) 254 });
            if (this.Last_Cmd_Text == "ACK")
            {
              this.Command(_dev, new byte[2]
              {
                (byte) 184,
                (byte) num2
              });
              if (this.Wait_Resposta_Ack() && this.Last_Cmd_Text != "ACK")
              {
                if (this.IsAscii(this.Last_Cmd_Text[0]))
                {
                  this.eCanal[num2 - 1] = 1;
                  this.iCanal[num2 - 1] = 1;
                  this.Canal[num2 - 1] = this.CVF_Credits(this.Last_Cmd_Text);
                  Control_CCTALK_COIN controlCctalkCoin18 = this;
                  controlCctalkCoin18.Coin_Acceptor_Currency = controlCctalkCoin18.Coin_Acceptor_Currency + this.CVF_Credits_Currency(this.Canal[num2 - 1]) + " " + this.CVF_Currency(this.Last_Cmd_Text) + " (" + this.Last_Cmd_Text + ")\r\n";
                  num1 = 1;
                }
                else
                  this.eCanal[num2 - 1] = 0;
                ++num2;
              }
            }
          }
          if (num1 == 0)
            this.Coin_Acceptor_Currency = "";
          Control_CCTALK_COIN controlCctalkCoin19 = this;
          controlCctalkCoin19.logs = controlCctalkCoin19.logs + "Currencies:\r\n" + this.Coin_Acceptor_Currency;
          return true;
        default:
          return false;
      }
    }

    public bool Startup()
    {
      ++this.lockCommand;
      this.SerialNumber = "";
      Thread.Sleep(200);
      this.Reset(this.Coin_Acceptor_ID);
      Thread.Sleep(200);
      this.GetInfo(this.Coin_Acceptor_ID, "TEST_COIN");
      this.Coin_Acceptor_ID = 2;
      this.GetInfo(this.Coin_Acceptor_ID, "TYPE");
      if (this._IsCoinPresent)
      {
        this.GetInfo(this.Coin_Acceptor_ID, "TEST_COIN");
        this.GetInfo(this.Coin_Acceptor_ID, "POLL");
        this.GetInfo(this.Coin_Acceptor_ID, "RESET_COIN");
        this.GetInfo(this.Coin_Acceptor_ID, "POLL");
        this.GetInfo(this.Coin_Acceptor_ID, "STATUS_COIN");
        this.GetInfo(this.Coin_Acceptor_ID, "CURRENCY_COIN");
        this.GetInfo(this.Coin_Acceptor_ID, "INHIBIT_COIN");
        if (this.Coin_Acceptor_Currency == "")
        {
          this._IsCoinPresent = false;
          this.EnablePoll = false;
        }
        else
          this.EnablePoll = true;
      }
      if (this._IsCoinPresent)
        this.OnLine = true;
      --this.lockCommand;
      if (this.OnLine)
        this.Inhibits(this.Coin_Acceptor_ID, new int[4]
        {
          20,
          50,
          100,
          200
        });
      return this.OnLine;
    }

    public bool Wait_Resposta()
    {
      Thread.Sleep(150);
      DateTime dateTime = DateTime.Now.AddMilliseconds(1050.0);
      while (this.m_Respuesta == this.m_Respuesta_Point)
      {
        this.Poll();
        if (DateTime.Now > dateTime)
          break;
      }
      return this.m_Respuesta != this.m_Respuesta_Point;
    }

    public bool Wait_Resposta_Ack()
    {
      Thread.Sleep(150);
      DateTime dateTime = DateTime.Now.AddMilliseconds(1050.0);
      while (this.m_Respuesta == this.m_Respuesta_Point || this.Last_Cmd_Text == "ACK")
      {
        this.Poll();
        if (this.Last_Cmd_Text == "ACK")
          this.m_Respuesta_Point = this.m_Respuesta;
        if (DateTime.Now > dateTime)
          break;
      }
      return this.m_Respuesta != this.m_Respuesta_Point;
    }

    public bool Close()
    {
      this.OnLine = false;
      if (this.comm != null)
        this.comm.Close();
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

    public byte[] Build_Command(int _id, byte[] cmd)
    {
      int num1 = 0;
      byte[] _buffer = new byte[cmd.Length + 3];
      byte[] numArray1 = _buffer;
      int index1 = num1;
      int num2 = index1 + 1;
      int num3 = (int) (byte) _id;
      numArray1[index1] = (byte) num3;
      byte[] numArray2 = _buffer;
      int index2 = num2;
      int num4 = index2 + 1;
      int num5 = (int) (byte) (cmd.Length - 1);
      numArray2[index2] = (byte) num5;
      byte[] numArray3 = _buffer;
      int index3 = num4;
      int num6 = index3 + 1;
      numArray3[index3] = (byte) 1;
      for (int index4 = 0; index4 < cmd.Length; ++index4)
        _buffer[num6++] = cmd[index4];
      ushort crc = (ushort) this.GetCRC(ref _buffer);
      byte[] numArray4 = new byte[cmd.Length + 4];
      int index5 = 0;
      for (int index4 = 0; index4 < _buffer.Length; ++index4)
        numArray4[index5++] = _buffer[index4];
      numArray4[index5] = (byte) crc;
      return numArray4;
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
        this._f_resp_scom = "-";
        return true;
      }
      if (this._f_cnt < this._f_ports.Length)
      {
        this._f_resp_scom = "-";
        this._f_com = this._f_ports[this._f_cnt];
        this.port = this._f_com;
        ++this._f_cnt;
        return this.Open();
      }
      this._f_resp_scom = "-";
      return true;
    }

    public bool Poll_Find_Device()
    {
      return this.Next_Find_Device();
    }

    public void Stop_Find_Device()
    {
      if (this.comm == null)
        return;
      this.comm.Close();
      this.comm = (CommPort) null;
    }

    private byte[] CopyFlat(int len)
    {
      byte[] numArray = new byte[len];
      for (int index1 = 0; index1 < len; ++index1)
      {
        int index2 = this.inici + index1 & Control_CCTALK_COIN.MIDA - 1;
        numArray[index1] = this.buffer[index2];
      }
      return numArray;
    }

    private byte[] CopyFlat(byte[] _buff, int _pos, int len)
    {
      byte[] numArray = new byte[len];
      for (int index = 0; index < len; ++index)
        numArray[index] = _buff[_pos + index];
      return numArray;
    }

    private void Purge(int len)
    {
      for (int index = 0; index < len; ++index)
        this.buffer[this.inici + index & Control_CCTALK_COIN.MIDA - 1] = (byte) 0;
      this.inici = this.inici + len & Control_CCTALK_COIN.MIDA - 1;
      this.bytes -= len;
      if (this.bytes > 0)
        return;
      this.Clear_Buffer();
    }

    private void Clear_Buffer()
    {
      this.bytes = 0;
      this.inici = 0;
      this.final = 0;
      for (int index = 0; index < Control_CCTALK_COIN.MIDA; ++index)
        this.buffer[index] = (byte) 0;
    }

    public void Reset(int _dev)
    {
      this.logs = "";
      this.Command(_dev, new byte[1]{ (byte) 1 });
    }

    private void Parse_Command(byte[] _data)
    {
      if (_data[3] != (byte) 0)
        return;
      if (_data[1] > (byte) 0)
      {
        this.Last_Data = this.CopyFlat(_data, 4, (int) _data[1]);
        if (this.SendPoll)
        {
          if (this.OnLine)
          {
            int num1 = (int) _data[4] - this.lastPoll;
            if ((int) _data[4] != this.lastPoll)
            {
              int num2 = 0;
              if ((int) _data[2] == this.Coin_Acceptor_ID && this.CoinConnected)
                num2 = 1;
              if (num1 < 5)
              {
                for (int index = 0; index < num1 * 2; index += 2)
                {
                  int num3 = (int) _data[5 + index];
                  if (num3 > 0 && num3 < 8 && num2 == 1)
                  {
                    this.Last_Cmd_Text = "COIN " + this.CVF_Credits_Currency(this.Canal[num3 - 1]);
                    Control_CCTALK_COIN controlCctalkCoin = this;
                    controlCctalkCoin.logs = controlCctalkCoin.logs + this.Last_Cmd_Text + "\r\n";
                    this.Creditos = this.Canal[num3 - 1];
                  }
                  else
                    this.Last_Cmd_Text = "ERROR COIN";
                }
              }
            }
            this.lastPoll = (int) _data[4];
          }
        }
        else
        {
          string str = Encoding.Default.GetString(this.Last_Data);
          this.Last_Cmd_Text = !this.IsAsciiNumbers(str[0]) ? "!" : str;
          if (this.Last_Cmd_Text.ToLower() == "Coin Acceptor".ToLower())
          {
            this._IsCoinPresent = true;
            this.Coin_Acceptor = this.Last_Cmd_Text;
            this.Coin_Acceptor_ID = (int) _data[2];
          }
        }
      }
      else
        this.Last_Cmd_Text = "ACK";
      this.SendPoll = false;
    }

    public void Decode()
    {
      bool flag = false;
      while (this.bytes >= 5 && !flag)
      {
        int num1 = 1;
        byte[] numArray = this.CopyFlat(5);
        byte num2 = (byte) ((uint) numArray[1] + 5U);
        if (this.bytes >= 5 && (numArray[0] != (byte) 1 && numArray[2] != (byte) 1))
          num1 = 0;
        DateTime now;
        if (this.bytes >= (int) num2)
        {
          byte[] _buffer = this.CopyFlat((int) num2);
          if (this.IsCRCValid(ref _buffer))
          {
            if (numArray[0] == (byte) 1)
            {
              if ((int) _buffer[2] == this.Coin_Acceptor_ID)
              {
                now = DateTime.Now;
                this.CoinTimeoutPoll = now.Ticks / 10000000L;
              }
              ++this.m_Respuesta;
              this.Last_Packet = "";
              for (int index = 0; index < _buffer.Length; ++index)
              {
                Control_CCTALK_COIN controlCctalkCoin = this;
                controlCctalkCoin.Last_Packet = controlCctalkCoin.Last_Packet + Convert.ToString(_buffer[index], 16) + " ";
              }
              this.Parse_Command(_buffer);
            }
            this.Purge((int) num2);
          }
          else
            num1 = 0;
        }
        else
        {
          now = DateTime.Now;
          if (now.Ticks / 10000L > this.TimeoutRead)
            num1 = 0;
          flag = true;
        }
        if (num1 == 0)
          this.Purge(1);
      }
    }

    public bool Parser()
    {
      if (this.comm == null)
        return false;
      int bytes;
      do
      {
        bytes = this.comm.Bytes;
        if (bytes > 0)
        {
          this.TimeoutRead = DateTime.Now.Ticks / 10000L + 250L;
          foreach (byte num in this.comm.Read(bytes))
          {
            this.buffer[this.final] = num;
            this.final = ++this.final & Control_CCTALK_COIN.MIDA - 1;
            ++this.bytes;
            if (this.bytes >= Control_CCTALK_COIN.MIDA)
            {
              this.inici &= ++this.inici & Control_CCTALK_COIN.MIDA - 1;
              this.bytes = Control_CCTALK_COIN.MIDA - 1;
            }
          }
        }
        this.Decode();
      }
      while (bytes > 0);
      return true;
    }

    public bool Poll()
    {
      if (this.comm == null)
        return false;
      if (this.Parser())
      {
        if (this.Send_Command != null)
        {
          this.comm.Write(this.Send_Command);
          this.Send_Command = (byte[]) null;
        }
        else
        {
          if (this.lockCommand > 0 || !this.EnablePoll)
            return true;
          if (this._IsCoinPresent)
          {
            if (DateTime.Now.Ticks / 10000000L - this.CoinTimeoutPoll > 5L && this.CoinConnected)
            {
              this.Clear_Buffer();
              this.SendPoll = false;
              this.CoinConnected = false;
              this.comm.Write(this.Build_Command(this.Coin_Acceptor_ID, new byte[2]
              {
                (byte) 228,
                (byte) 0
              }));
            }
            else
            {
              this.Clear_Buffer();
              this.SendPoll = true;
              this.comm.Write(this.Build_Command(this.Coin_Acceptor_ID, new byte[1]
              {
                (byte) 229
              }));
            }
          }
          this.Connected = this.CoinConnected;
        }
      }
      return true;
    }

    public bool Command(int _id, byte[] _cmd)
    {
      if (this.comm == null)
        return false;
      DateTime dateTime1 = DateTime.Now.AddMilliseconds(1000.0);
      this.Poll();
      while (this.Send_Command != null)
      {
        this.Poll();
        if (DateTime.Now > dateTime1)
          return false;
      }
      this.m_Respuesta_Point = this.m_Respuesta;
      this.Last_Cmd_Text = (string) null;
      this.Send_Command = this.Build_Command(_id, _cmd);
      DateTime dateTime2 = DateTime.Now.AddMilliseconds(1050.0);
      while (this.m_Respuesta == this.m_Respuesta_Point)
      {
        this.Poll();
        if (DateTime.Now > dateTime2)
          break;
      }
      return this.m_Respuesta != this.m_Respuesta_Point;
    }

    private bool IsChannelEnabled(int _v, int[] _list)
    {
      for (int index = 0; index < _list.Length; ++index)
      {
        if (_v == _list[index])
          return true;
      }
      return false;
    }

    public bool Inhibits(int _dev, int[] _enabled)
    {
      if (this.comm == null)
        return false;
      ++this.lockCommand;
      bool flag = false;
      byte num1 = 1;
      byte num2 = 0;
      int index = 0;
      while (index < 8)
      {
        if (this.IsChannelEnabled(this.Canal[index], _enabled))
          num2 |= num1;
        ++index;
        num1 <<= 1;
      }
      byte num3 = 1;
      byte num4 = 0;
      int num5 = 0;
      while (num5 < 8)
      {
        if (this.IsChannelEnabled(this.Canal[num5 + 8], _enabled))
          num4 |= num3;
        ++num5;
        num3 <<= 1;
      }
      int num6 = (int) num2 + (int) num4 * 256;
      if (_dev == this.Coin_Acceptor_ID)
      {
        if (num6 != this.Coin_Acceptor_Inhibit)
          flag = this.Command(_dev, new byte[3]
          {
            (byte) 231,
            num2,
            num4
          });
        this.GetInfo(_dev, "INHIBIT_COIN");
        if (num6 != this.Coin_Acceptor_Inhibit)
          flag = false;
      }
      --this.lockCommand;
      return flag;
    }

    public bool Enable(int _dev)
    {
      if (this.comm == null)
        return false;
      ++this.lockCommand;
      bool flag = this.Command(_dev, new byte[2]
      {
        (byte) 228,
        (byte) 1
      });
      this.SendPoll = false;
      this.EnablePoll = true;
      if (_dev == this.Coin_Acceptor_ID)
      {
        this.CoinConnected = true;
        this.CoinTimeoutPoll = DateTime.Now.Ticks / 10000000L;
      }
      this.Connected = this.CoinConnected;
      --this.lockCommand;
      return flag;
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

    public bool Disable(int _dev)
    {
      if (this.comm == null)
        return false;
      ++this.lockCommand;
      if (_dev == this.Coin_Acceptor_ID)
        this.CoinConnected = false;
      bool flag = this.Command(_dev, new byte[2]
      {
        (byte) 228,
        (byte) 0
      });
      --this.lockCommand;
      return flag;
    }
  }
}
