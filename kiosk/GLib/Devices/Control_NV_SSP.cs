// Decompiled with JetBrains decompiler
// Type: GLib.Devices.Control_NV_SSP
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using ITLlib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Threading;

namespace GLib.Devices
{
  public class Control_NV_SSP
  {
    public static int MaxCanales = 16;
    public int TimeOutComs = 0;
    private Decimal _Creditos = new Decimal(0);
    public int ReStartNeed = 0;
    public string _f_resp_scom;
    public string _f_com;
    private string[] _f_ports;
    private int _f_cnt;
    private SSPComms eSSP;
    private SSP_COMMAND cmd;
    private SSP_COMMAND storedCmd;
    private SSP_KEYS keys;
    private SSP_FULL_KEY sspKey;
    private SSP_COMMAND_INFO info;
    private char m_UnitType;
    private int m_NumStackedNotes;
    public int m_NumberOfChannels;
    private int m_ValueMultiplier;
    public string m_log;
    public List<ChannelData> m_UnitDataList;
    public bool OnLine;
    public string port;
    public int[] Canal;
    public int[] eCanal;
    public bool respuesta;
    public System.Windows.Forms.Timer reconnectionTimer;
    private int reconnectionInterval;
    public bool IsEnable;

    public Control_NV_SSP()
    {
      this.eSSP = new SSPComms();
      this.cmd = new SSP_COMMAND();
      this.storedCmd = new SSP_COMMAND();
      this.keys = new SSP_KEYS();
      this.sspKey = new SSP_FULL_KEY();
      this.info = new SSP_COMMAND_INFO();
      this.m_NumberOfChannels = 0;
      this.m_ValueMultiplier = 1;
      this.m_UnitType = 'ÿ';
      this.m_UnitDataList = new List<ChannelData>();
      this.reconnectionInterval = 2;
      this.reconnectionTimer = new System.Windows.Forms.Timer();
      this.respuesta = false;
      this.port = "COM3";
      this.Creditos = new Decimal(0);
      this.Canal = new int[Control_NV_SSP.MaxCanales];
      this.eCanal = new int[Control_NV_SSP.MaxCanales];
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
      this.Canal[6] = 50000;
      this.OnLine = false;
    }

    public string Find_Device()
    {
      return "-";
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
      if (this.eSSP != null)
      {
        this.eSSP.CloseComPort();
        this.eSSP = (SSPComms) null;
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
      if (this.eSSP == null)
        return;
      this.eSSP.CloseComPort();
      this.eSSP = (SSPComms) null;
    }

    public bool Open()
    {
      this.IsEnable = false;
      this.ReStartNeed = 0;
      this.TimeOutComs = 0;
      this.m_log = "";
      if (this.OnLine)
      {
        this.OnLine = false;
        this.DisableValidator(ref this.m_log);
      }
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
      this.CommandStructure.ComPort = this.port;
      this.CommandStructure.SSPAddress = (byte) 0;
      this.CommandStructure.Timeout = 3000U;
      if (!this.ConnectToValidator())
      {
        this.m_log += "fail";
        this.respuesta = false;
        return false;
      }
      this.OnLine = true;
      this._f_resp_scom = this.port;
      this.respuesta = true;
      return true;
    }

    public bool Poll()
    {
      if (!this.OnLine)
        return false;
      this.m_log = "";
      return this.DoPoll(ref this.m_log);
    }

    public void Close()
    {
      this.IsEnable = false;
      if (this.eSSP != null)
      {
        this.Disable();
        Thread.Sleep(100);
        this.Poll();
      }
      if (this.eSSP == null)
        return;
      this.eSSP.CloseComPort();
      this.eSSP = (SSPComms) null;
    }

    public bool Enable()
    {
      this.IsEnable = true;
      if (!this.OnLine)
        return false;
      this.TimeOutComs = 0;
      this.m_log = "";
      this.EnableValidator(ref this.m_log);
      return true;
    }

    public bool Disable()
    {
      this.IsEnable = false;
      if (!this.OnLine)
        return false;
      this.TimeOutComs = 0;
      this.m_log = "";
      this.DisableValidator(ref this.m_log);
      return true;
    }

    private byte FindMaxProtocolVersion()
    {
      this.m_log = "";
      byte num = 0;
      for (byte pVersion = 1; pVersion < (byte) 10; ++pVersion)
      {
        this.SetProtocolVersion(pVersion, ref this.m_log);
        Thread.Sleep(200);
        if (this.CommandStructure.ResponseData[0] == (byte) 240)
          num = pVersion;
      }
      return num;
    }

    private bool IsUnitTypeSupported(char type)
    {
      return type == char.MinValue;
    }

    private bool ConnectToValidator()
    {
      this.m_log = "";
      this.reconnectionTimer.Interval = this.reconnectionInterval * 1000;
      this.reconnectionTimer.Enabled = true;
      if (this.eSSP != null)
      {
        this.eSSP.CloseComPort();
        this.eSSP = (SSPComms) null;
      }
      if (this.eSSP == null)
        this.eSSP = new SSPComms();
      this.CommandStructure.EncryptionStatus = false;
      if (!this.OpenComPort(ref this.m_log) || !this.NegotiateKeys(ref this.m_log))
        return false;
      this.CommandStructure.EncryptionStatus = true;
      if (this.FindMaxProtocolVersion() < (byte) 6)
        return false;
      this.SetProtocolVersion((byte) 6, ref this.m_log);
      this.SetupRequest(ref this.m_log);
      if (!this.IsUnitTypeSupported(this.UnitType))
        return false;
      this.SetInhibits(ref this.m_log);
      this.EnableValidator(ref this.m_log);
      return true;
    }

    [Description("Creditos")]
    public Decimal Creditos
    {
      get
      {
        return this._Creditos;
      }
      set
      {
        this._Creditos = value;
      }
    }

    [Description("Multiplicador")]
    public int Multiplicador
    {
      get
      {
        return this.m_ValueMultiplier;
      }
      set
      {
        this.m_ValueMultiplier = value;
      }
    }

    [Description("Canales")]
    public int Canales
    {
      get
      {
        return this.m_NumberOfChannels;
      }
      set
      {
        this.m_NumberOfChannels = value;
      }
    }

    public SSP_COMMAND CommandStructure
    {
      get
      {
        return this.cmd;
      }
      set
      {
        this.cmd = value;
      }
    }

    public SSP_COMMAND_INFO InfoStructure
    {
      get
      {
        return this.info;
      }
      set
      {
        this.info = value;
      }
    }

    public char UnitType
    {
      get
      {
        return this.m_UnitType;
      }
    }

    public int NumberOfChannels
    {
      get
      {
        return this.m_NumberOfChannels;
      }
      set
      {
        this.m_NumberOfChannels = value;
      }
    }

    public int NumberOfNotesStacked
    {
      get
      {
        return this.m_NumStackedNotes;
      }
      set
      {
        this.m_NumStackedNotes = value;
      }
    }

    public int Multiplier
    {
      get
      {
        return this.m_ValueMultiplier;
      }
      set
      {
        this.m_ValueMultiplier = value;
      }
    }

    public bool GetChannelInhibit(int channelNum)
    {
      if (channelNum >= 1 && channelNum <= this.m_NumberOfChannels)
      {
        foreach (ChannelData unitData in this.m_UnitDataList)
        {
          if ((int) unitData.Channel == channelNum)
            return unitData.Inhibit;
        }
      }
      return true;
    }

    public bool GetChannelEnabled(int channelNum)
    {
      if (channelNum >= 1 && channelNum <= this.m_NumberOfChannels)
      {
        foreach (ChannelData unitData in this.m_UnitDataList)
        {
          if ((int) unitData.Channel == channelNum)
            return unitData.Enabled;
        }
      }
      return false;
    }

    public int GetChannelValue(int channelNum)
    {
      if (channelNum >= 1 && channelNum <= this.m_NumberOfChannels)
      {
        foreach (ChannelData unitData in this.m_UnitDataList)
        {
          if ((int) unitData.Channel == channelNum)
            return unitData.Value;
        }
      }
      return -1;
    }

    public string GetChannelCurrency(int channelNum)
    {
      if (channelNum >= 1 && channelNum <= this.m_NumberOfChannels)
      {
        foreach (ChannelData unitData in this.m_UnitDataList)
        {
          if ((int) unitData.Channel == channelNum)
            return new string(unitData.Currency);
        }
      }
      return "";
    }

    public void EnableValidator(ref string log)
    {
      this.cmd.CommandData[0] = (byte) 10;
      this.cmd.CommandDataLength = (byte) 1;
      if (!this.SendCommand(ref log) || (!this.CheckGenericResponses(ref log) || log == null))
        return;
      log += "Unit enabled\r\n";
    }

    public void DisableValidator(ref string log)
    {
      this.cmd.CommandData[0] = (byte) 9;
      this.cmd.CommandDataLength = (byte) 1;
      if (!this.SendCommand(ref log) || (!this.CheckGenericResponses(ref log) || log == null))
        return;
      log += "Unit disabled\r\n";
    }

    public void Reset(ref string log)
    {
      this.cmd.CommandData[0] = (byte) 1;
      this.cmd.CommandDataLength = (byte) 1;
      if (!this.SendCommand(ref log) || !this.CheckGenericResponses(ref log))
        return;
      log += "Resetting unit\r\n";
    }

    public bool SendSync(string log = null)
    {
      this.cmd.CommandData[0] = (byte) 17;
      this.cmd.CommandDataLength = (byte) 1;
      if (!this.SendCommand(ref log))
        return false;
      if (this.CheckGenericResponses(ref log))
        log += "Successfully sent sync\r\n";
      return true;
    }

    public void SetProtocolVersion(byte pVersion, ref string log)
    {
      this.cmd.CommandData[0] = (byte) 6;
      this.cmd.CommandData[1] = pVersion;
      this.cmd.CommandDataLength = (byte) 2;
      if (!this.SendCommand(ref log))
        ;
    }

    public void QueryRejection(ref string log)
    {
      this.cmd.CommandData[0] = (byte) 23;
      this.cmd.CommandDataLength = (byte) 1;
      if (!this.SendCommand(ref log) || !this.CheckGenericResponses(ref log) || log == null)
        return;
      switch (this.cmd.ResponseData[1])
      {
        case 0:
          log += "Note accepted\r\n";
          break;
        case 1:
          log += "Note length incorrect\r\n";
          break;
        case 2:
          log += "Invalid note\r\n";
          break;
        case 3:
          log += "Invalid note\r\n";
          break;
        case 4:
          log += "Invalid note\r\n";
          break;
        case 5:
          log += "Invalid note\r\n";
          break;
        case 6:
          log += "Channel inhibited\r\n";
          break;
        case 7:
          log += "Second note inserted during read\r\n";
          break;
        case 8:
          log += "Host rejected note\r\n";
          break;
        case 9:
          log += "Invalid note\r\n";
          break;
        case 10:
          log += "Invalid note read\r\n";
          break;
        case 11:
          log += "Note too long\r\n";
          break;
        case 12:
          log += "Validator disabled\r\n";
          break;
        case 13:
          log += "Mechanism slow/stalled\r\n";
          break;
        case 14:
          log += "Strim attempt\r\n";
          break;
        case 15:
          log += "Fraud channel reject\r\n";
          break;
        case 16:
          log += "No notes inserted\r\n";
          break;
        case 17:
          log += "Invalid note read\r\n";
          break;
        case 18:
          log += "Twisted note detected\r\n";
          break;
        case 19:
          log += "Escrow time-out\r\n";
          break;
        case 20:
          log += "Bar code scan fail\r\n";
          break;
        case 21:
          log += "Invalid note read\r\n";
          break;
        case 22:
          log += "Invalid note read\r\n";
          break;
        case 23:
          log += "Invalid note read\r\n";
          break;
        case 24:
          log += "Invalid note read\r\n";
          break;
        case 25:
          log += "Incorrect note width\r\n";
          break;
        case 26:
          log += "Note too short\r\n";
          break;
      }
    }

    public bool NegotiateKeys(ref string log)
    {
      this.cmd.EncryptionStatus = false;
      if (log != null)
        log += "Syncing... \r\n";
      this.cmd.CommandData[0] = (byte) 17;
      this.cmd.CommandDataLength = (byte) 1;
      if (!this.SendCommand(ref log))
        return false;
      if (log != null)
        log += "Success\r\n";
      this.eSSP.InitiateSSPHostKeys(this.keys, this.cmd);
      this.cmd.CommandData[0] = (byte) 74;
      this.cmd.CommandDataLength = (byte) 9;
      if (log != null)
        log += "Setting generator... \r\n";
      for (byte index = 0; index < (byte) 8; ++index)
        this.cmd.CommandData[(int) index + 1] = (byte) (this.keys.Generator >> 8 * (int) index);
      if (!this.SendCommand(ref log))
        return false;
      if (log != null)
        log += "Success\r\n";
      this.cmd.CommandData[0] = (byte) 75;
      this.cmd.CommandDataLength = (byte) 9;
      if (log != null)
        log += "Sending modulus... \r\n";
      for (byte index = 0; index < (byte) 8; ++index)
        this.cmd.CommandData[(int) index + 1] = (byte) (this.keys.Modulus >> 8 * (int) index);
      if (!this.SendCommand(ref log))
        return false;
      if (log != null)
        log += "Success\r\n";
      this.cmd.CommandData[0] = (byte) 76;
      this.cmd.CommandDataLength = (byte) 9;
      if (log != null)
        log += "Exchanging keys... ";
      for (byte index = 0; index < (byte) 8; ++index)
        this.cmd.CommandData[(int) index + 1] = (byte) (this.keys.HostInter >> 8 * (int) index);
      if (!this.SendCommand(ref log))
        return false;
      if (log != null)
        log += "Success\r\n";
      this.keys.SlaveInterKey = 0UL;
      for (byte index = 0; index < (byte) 8; ++index)
        this.keys.SlaveInterKey += (ulong) this.cmd.ResponseData[1 + (int) index] << 8 * (int) index;
      this.eSSP.CreateSSPHostEncryptionKey(this.keys);
      this.cmd.Key.FixedKey = 81985526925837671UL;
      this.cmd.Key.VariableKey = this.keys.KeyHost;
      if (log != null)
        log += "Keys successfully negotiated\r\n";
      return true;
    }

    public void SetupRequest(ref string log)
    {
      this.cmd.CommandData[0] = (byte) 5;
      this.cmd.CommandDataLength = (byte) 1;
      if (!this.SendCommand(ref log))
        return;
      string str1 = "Unit Type: ";
      int num1 = 1;
      byte[] responseData1 = this.cmd.ResponseData;
      int index1 = num1;
      int num2 = index1 + 1;
      this.m_UnitType = (char) responseData1[index1];
      string str2;
      switch (this.m_UnitType)
      {
        case char.MinValue:
          str2 = str1 + "Validator";
          break;
        case '\x0003':
          str2 = str1 + "SMART Hopper";
          break;
        case '\x0006':
          str2 = str1 + "SMART Payout";
          break;
        case '\a':
          str2 = str1 + "NV11";
          break;
        default:
          str2 = str1 + "Unknown Type";
          break;
      }
      string str3 = str2 + "\r\nFirmware: ";
      while (num2 <= 5)
      {
        str3 += (string) (object) (char) this.cmd.ResponseData[num2++];
        if (num2 == 4)
          str3 += ".";
      }
      int num3 = 9;
      int num4 = 12;
      string str4 = str3 + "\r\nNumber of Channels: ";
      byte[] responseData2 = this.cmd.ResponseData;
      int index2 = num4;
      num3 = index2 + 1;
      int num5 = (int) responseData2[index2];
      this.m_NumberOfChannels = num5;
      string str5 = str4 + (object) num5 + "\r\n";
      num3 = 13 + this.m_NumberOfChannels;
      int[] numArray1 = new int[this.m_NumberOfChannels];
      for (int index3 = 0; index3 < this.m_NumberOfChannels; ++index3)
        numArray1[index3] = (int) this.cmd.ResponseData[13 + this.m_NumberOfChannels + index3];
      int index4 = 13 + this.m_NumberOfChannels * 2;
      string str6 = str5 + "Real Value Multiplier: ";
      this.m_ValueMultiplier = (int) this.cmd.ResponseData[index4 + 2];
      this.m_ValueMultiplier += (int) this.cmd.ResponseData[index4 + 1] << 8;
      this.m_ValueMultiplier += (int) this.cmd.ResponseData[index4] << 16;
      string str7 = str6 + (object) this.m_ValueMultiplier + "\r\nProtocol Version: ";
      num3 = index4 + 3;
      int num6 = 16 + this.m_NumberOfChannels * 2;
      byte[] responseData3 = this.cmd.ResponseData;
      int index5 = num6;
      num3 = index5 + 1;
      int num7 = (int) responseData3[index5];
      string str8 = str7 + (object) num7 + "\r\n";
      int num8 = 17 + this.m_NumberOfChannels * 2;
      int num9 = 17 + this.m_NumberOfChannels * 5;
      int num10 = 0;
      byte[] numArray2 = new byte[3 * this.m_NumberOfChannels];
      while (num8 < num9)
      {
        string str9 = str8 + "Channel " + (object) (num10 / 3 + 1) + ", currency: ";
        byte[] numArray3 = numArray2;
        int index3 = num10;
        byte[] responseData4 = this.cmd.ResponseData;
        int index6 = num8;
        int num11 = index6 + 1;
        int num12 = (int) responseData4[index6];
        numArray3[index3] = (byte) num12;
        string str10 = str9;
        byte[] numArray4 = numArray2;
        int index7 = num10;
        int num13 = index7 + 1;
        // ISSUE: variable of a boxed type
        char local1 = (char) numArray4[index7];
        string str11 = str10 + (object) local1;
        byte[] numArray5 = numArray2;
        int index8 = num13;
        byte[] responseData5 = this.cmd.ResponseData;
        int index9 = num11;
        int num14 = index9 + 1;
        int num15 = (int) responseData5[index9];
        numArray5[index8] = (byte) num15;
        string str12 = str11;
        byte[] numArray6 = numArray2;
        int index10 = num13;
        int num16 = index10 + 1;
        // ISSUE: variable of a boxed type
        char local2 = (char) numArray6[index10];
        string str13 = str12 + (object) local2;
        byte[] numArray7 = numArray2;
        int index11 = num16;
        byte[] responseData6 = this.cmd.ResponseData;
        int index12 = num14;
        num8 = index12 + 1;
        int num17 = (int) responseData6[index12];
        numArray7[index11] = (byte) num17;
        string str14 = str13;
        byte[] numArray8 = numArray2;
        int index13 = num16;
        num10 = index13 + 1;
        // ISSUE: variable of a boxed type
        char local3 = (char) numArray8[index13];
        str8 = str14 + (object) local3 + "\r\n";
      }
      int index14 = num9;
      string str15 = str8 + "Expanded channel values:\r\n";
      int num18 = 17 + this.m_NumberOfChannels * 9;
      int index15 = 0;
      int[] numArray9 = new int[this.m_NumberOfChannels];
      while (index14 < num18)
      {
        int int32 = CHelpers.ConvertBytesToInt32(this.cmd.ResponseData, index14);
        numArray9[index15] = int32;
        index14 += 4;
        str15 = str15 + "Channel " + (object) ++index15 + ", value = " + (object) int32 + "\r\n";
      }
      this.m_UnitDataList.Clear();
      for (byte index3 = 0; (int) index3 < this.m_NumberOfChannels; ++index3)
      {
        ChannelData channelData = new ChannelData();
        channelData.Channel = index3;
        ++channelData.Channel;
        channelData.Value = numArray9[(int) index3] * this.Multiplier;
        channelData.Currency[0] = (char) numArray2[(int) index3 * 3];
        channelData.Currency[1] = (char) numArray2[1 + (int) index3 * 3];
        channelData.Currency[2] = (char) numArray2[2 + (int) index3 * 3];
        channelData.Level = 0;
        channelData.Recycling = false;
        channelData.Enabled = numArray1[(int) index3] != 4;
        channelData.Inhibit = false;
        this.m_UnitDataList.Add(channelData);
      }
      this.m_UnitDataList.Sort((Comparison<ChannelData>) ((d1, d2) => d1.Value.CompareTo(d2.Value)));
      if (log == null)
        return;
      log += str15;
    }

    public void SetInhibits(ref string log)
    {
      this.cmd.CommandData[0] = (byte) 2;
      this.cmd.CommandData[1] = byte.MaxValue;
      this.cmd.CommandData[2] = byte.MaxValue;
      this.cmd.CommandDataLength = (byte) 3;
      if (!this.SendCommand(ref log) || (!this.CheckGenericResponses(ref log) || log == null))
        return;
      log += "Inhibits set\r\n";
    }

    public void SetEnableds(int _mask, ref string log)
    {
      this.cmd.CommandData[0] = (byte) 2;
      this.cmd.CommandData[1] = (byte) (_mask & (int) byte.MaxValue);
      this.cmd.CommandData[2] = (byte) (_mask >> 8 & (int) byte.MaxValue);
      this.cmd.CommandDataLength = (byte) 3;
      if (!this.SendCommand(ref log) || (!this.CheckGenericResponses(ref log) || log == null))
        return;
      log += "Enableds set\r\n";
    }

    public bool DoPoll(ref string log)
    {
      this.cmd.CommandData[0] = (byte) 7;
      this.cmd.CommandDataLength = (byte) 1;
      this.cmd.ResponseDataLength = (byte) 0;
      if (!this.SendCommand(ref log))
        return false;
      this.ReStartNeed = 0;
      if (this.IsEnable)
        ++this.TimeOutComs;
      if (this.cmd.ResponseDataLength == (byte) 1 && this.cmd.ResponseData[0] == (byte) 250)
        this.TimeOutComs = 1000;
      for (byte index = 1; (int) index < (int) this.cmd.ResponseDataLength; ++index)
      {
        this.TimeOutComs = 0;
        switch (this.cmd.ResponseData[(int) index])
        {
          case 181:
            log += "All channels inhibited, unit disabled\r\n";
            break;
          case 204:
            log += "Stacking note...\r\n";
            break;
          case 209:
            log += "Bar code ticket accepted\r\n";
            break;
          case 224:
            log += "Note path open\r\n";
            break;
          case 225:
            ref string local1 = ref log;
            local1 = local1 + (object) this.GetChannelValue((int) this.cmd.ResponseData[(int) index + 1]) + " note cleared from front at reset.\r\n";
            ++index;
            break;
          case 226:
            ref string local2 = ref log;
            local2 = local2 + (object) this.GetChannelValue((int) this.cmd.ResponseData[(int) index + 1]) + " note cleared to stacker at reset.\r\n";
            ++index;
            break;
          case 227:
            log += "Cashbox removed...\r\n";
            break;
          case 228:
            log += "Cashbox replaced\r\n";
            break;
          case 229:
            log += "Bar code ticket validated\r\n";
            break;
          case 230:
            ref string local3 = ref log;
            local3 = local3 + "Fraud attempt, note type: " + (object) this.GetChannelValue((int) this.cmd.ResponseData[(int) index + 1]) + "\r\n";
            ++index;
            break;
          case 231:
            log += "Stacker full\r\n";
            break;
          case 232:
            this.TimeOutComs = 1000;
            break;
          case 233:
            log += "Unsafe jam\r\n";
            break;
          case 234:
            log += "Safe jam\r\n";
            break;
          case 235:
            log += "Note stacked\r\n";
            break;
          case 236:
            log += "Note rejected\r\n";
            this.QueryRejection(ref log);
            break;
          case 237:
            log += "Rejecting note...\r\n";
            break;
          case 238:
            int channelValue1 = this.GetChannelValue((int) this.cmd.ResponseData[(int) index + 1]);
            ref string local4 = ref log;
            local4 = local4 + "Credit " + CHelpers.FormatToCurrency(channelValue1) + "\r\n";
            this.Creditos = (Decimal) channelValue1;
            ++this.m_NumStackedNotes;
            ++index;
            break;
          case 239:
            if (this.cmd.ResponseData[(int) index + 1] > (byte) 0)
            {
              int channelValue2 = this.GetChannelValue((int) this.cmd.ResponseData[(int) index + 1]);
              ref string local5 = ref log;
              local5 = local5 + "Note in escrow, amount: " + CHelpers.FormatToCurrency(channelValue2) + "\r\n";
            }
            else
              log += "Reading note...\r\n";
            ++index;
            break;
          case 241:
            log += "Unit reset\r\n";
            break;
          default:
            ref string local6 = ref log;
            local6 = local6 + "Unrecognised poll response detected " + (object) (int) this.cmd.ResponseData[(int) index] + "\r\n";
            break;
        }
      }
      return true;
    }

    public bool OpenComPort(ref string log)
    {
      if (log != null)
        log += "Opening com port\r\n";
      if (this.eSSP == null)
        this.eSSP = new SSPComms();
      return this.eSSP.OpenSSPComPort(this.cmd);
    }

    private bool CheckGenericResponses(ref string log)
    {
      if (this.cmd.ResponseData[0] == (byte) 240)
        return true;
      if (log == null)
        return false;
      switch (this.cmd.ResponseData[0])
      {
        case 242:
          log += "Command response is UNKNOWN\r\n";
          return false;
        case 243:
          log += "Command response is WRONG PARAMETERS\r\n";
          return false;
        case 244:
          log += "Command response is PARAM OUT OF RANGE\r\n";
          return false;
        case 245:
          if (this.cmd.ResponseData[1] == (byte) 3)
          {
            log += "Validator has responded with \"Busy\", command cannot be processed at this time\r\n";
          }
          else
          {
            ref string local = ref log;
            local = local + "Command response is CANNOT PROCESS COMMAND, error code - 0x" + BitConverter.ToString(this.cmd.ResponseData, 1, 1) + "\r\n";
          }
          return false;
        case 246:
          log += "Command response is SOFTWARE ERROR\r\n";
          return false;
        case 248:
          log += "Command response is FAIL\r\n";
          return false;
        case 250:
          log += "Command response is KEY NOT SET, Validator requires encryption on this command or there isa problem with the encryption on this request\r\n";
          return false;
        default:
          return false;
      }
    }

    public bool SendCommand(ref string log)
    {
      this.cmd.CommandData.CopyTo((Array) new byte[(int) byte.MaxValue], 0);
      byte commandDataLength = this.cmd.CommandDataLength;
      if (this.eSSP == null)
      {
        this.eSSP = new SSPComms();
        this.Open();
      }
      if (this.eSSP.SSPSendCommand(this.cmd, this.info))
        return true;
      this.eSSP.CloseComPort();
      if (log != null)
      {
        ref string local = ref log;
        local = local + "Sending command failed\r\nPort status: " + this.cmd.ResponseStatus.ToString() + "\r\n";
      }
      return false;
    }

    public class CCommands
    {
      public const byte SSP_CMD_RESET = 1;
      public const byte SSP_CMD_HOST_PROTOCOL_VERSION = 6;
      public const byte SSP_CMD_SYNC = 17;
      public const byte SSP_CMD_SET_GENERATOR = 74;
      public const byte SSP_CMD_SET_MODULUS = 75;
      public const byte SSP_CMD_KEY_EXCHANGE = 76;
      public const byte SSP_CMD_SET_INHIBITS = 2;
      public const byte SSP_CMD_ENABLE = 10;
      public const byte SSP_CMD_DISABLE = 9;
      public const byte SSP_CMD_POLL = 7;
      public const byte SSP_CMD_SETUP_REQUEST = 5;
      public const byte SSP_CMD_DISPLAY_ON = 3;
      public const byte SSP_CMD_DISPLAY_OFF = 4;
      public const byte SSP_CMD_EMPTY = 63;
      public const byte SSP_CMD_LAST_REJECT_CODE = 23;
      public const byte SSP_POLL_RESET = 241;
      public const byte SSP_POLL_NOTE_READ = 239;
      public const byte SSP_POLL_CREDIT = 238;
      public const byte SSP_POLL_REJECTING = 237;
      public const byte SSP_POLL_REJECTED = 236;
      public const byte SSP_POLL_STACKING = 204;
      public const byte SSP_POLL_STACKED = 235;
      public const byte SSP_POLL_SAFE_JAM = 234;
      public const byte SSP_POLL_UNSAFE_JAM = 233;
      public const byte SSP_POLL_DISABLED = 232;
      public const byte SSP_POLL_FRAUD_ATTEMPT = 230;
      public const byte SSP_POLL_STACKER_FULL = 231;
      public const byte SSP_POLL_NOTE_CLEARED_FROM_FRONT = 225;
      public const byte SSP_POLL_NOTE_CLEARED_TO_CASHBOX = 226;
      public const byte SSP_POLL_CASHBOX_REMOVED = 227;
      public const byte SSP_POLL_CASHBOX_REPLACED = 228;
      public const byte SSP_POLL_BAR_CODE_VALIDATED = 229;
      public const byte SSP_POLL_BAR_CODE_ACK = 209;
      public const byte SSP_POLL_NOTE_PATH_OPEN = 224;
      public const byte SSP_POLL_CHANNEL_DISABLE = 181;
      public const byte SSP_RESPONSE_CMD_OK = 240;
      public const byte SSP_RESPONSE_CMD_UNKNOWN = 242;
      public const byte SSP_RESPONSE_CMD_WRONG_PARAMS = 243;
      public const byte SSP_RESPONSE_CMD_PARAM_OUT_OF_RANGE = 244;
      public const byte SSP_RESPONSE_CMD_CANNOT_PROCESS = 245;
      public const byte SSP_RESPONSE_CMD_SOFTWARE_ERROR = 246;
      public const byte SSP_RESPONSE_CMD_FAIL = 248;
      public const byte SSP_RESPONSE_CMD_KEY_NOT_SET = 250;
    }
  }
}
