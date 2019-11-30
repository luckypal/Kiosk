// Decompiled with JetBrains decompiler
// Type: GLib.Devices.Control_F40_CCTalk
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.Globalization;
using System.IO.Ports;
using System.Threading;
using System.Timers;

namespace GLib.Devices
{
  public class Control_F40_CCTalk
  {
    private byte[] cct_rx = new byte[256];
    private byte[][] cct_tx = new byte[4][]
    {
      new byte[1024],
      new byte[1024],
      new byte[1024],
      new byte[1024]
    };
    private int[] msgTxLen = new int[4];
    private Control_F40_CCTalk.NUMERICFORMATTYPE nNumericFormat = Control_F40_CCTalk.NUMERICFORMATTYPE.HEX;
    private SerialManager sm = (SerialManager) null;
    public bool OnLine = false;
    private bool bFoundAcceptor = false;
    private bool confirm_OK = false;
    public Decimal Base = new Decimal(100);
    private int NoMoney = 1;
    public int Canales = 8;
    private byte Ult_ser = byte.MaxValue;
    private byte bill = 0;
    private string echoNull = "";
    public int TimeOutComs = 0;
    public const int idCCTALK = 40;
    public int[] Canal;
    private bool bChecksum;
    private Control_F40_CCTalk.CHECKSUMTYPE bChecksumType;
    private bool bEchoEnabled;
    private bool bEncrypt;
    private bool bWaitForEcho;
    private int iEncryptionKey;
    private int iReadtTimeout;
    private System.Timers.Timer tTimeOut;
    private int m_Creditos;
    public int[] eCanal;
    public string _f_resp_scom;
    public string _f_com;
    private string[] _f_ports;
    private int _f_cnt;
    public bool respuesta;
    public string port;
    private string trans_cmd;

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

    public bool Poll()
    {
      this.CCT_PollsTransmit(1);
      ++this.TimeOutComs;
      return true;
    }

    public event Control_F40_CCTalk.CCtalkReceivedData OnCCTRxData;

    public event Control_F40_CCTalk.CCtalkTransmittedData OnCCTTxData;

    public Control_F40_CCTalk()
    {
      this.READ_TIMEOUT = 100;
      this.tTimeOut = new System.Timers.Timer();
      this.tTimeOut.Elapsed += new ElapsedEventHandler(this.tTimeOut_Elapsed);
      this.bEchoEnabled = true;
      this.bWaitForEcho = false;
      this.bChecksumType = Control_F40_CCTalk.CHECKSUMTYPE.CHK8BIT;
      this.NoMoney = 1;
      this.confirm_OK = false;
      this.Creditos = 0;
      this.bFoundAcceptor = false;
      this.Base = new Decimal(100);
      this.Canal = new int[this.Canales];
      this.eCanal = new int[this.Canales];
      for (int index = 0; index < this.Canales; ++index)
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
      this.respuesta = false;
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
      this.Canal[0] = 200;
      this.Canal[1] = 500;
      this.Canal[2] = 1000;
      this.Canal[3] = 2000;
      this.Canal[4] = 5000;
      this.Canal[5] = 10000;
      this.Canal[6] = 20000;
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

    public void Start_Find_Device()
    {
      this._f_resp_scom = "-";
      this._f_ports = SerialPort.GetPortNames();
      this._f_cnt = 0;
    }

    public bool Next_Find_Device()
    {
      if (this.sm != null)
      {
        this.sm.ClosePort();
        this.sm = (SerialManager) null;
      }
      if (this._f_ports == null)
      {
        this.respuesta = false;
        this._f_resp_scom = "-";
        this._f_com = "-";
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
      this._f_com = "-";
      return true;
    }

    public bool Poll_Find_Device()
    {
      this.respuesta = false;
      return this.Next_Find_Device();
    }

    public void Stop_Find_Device()
    {
      if (this.sm == null)
        return;
      this.sm.ClosePort();
      this.sm = (SerialManager) null;
    }

    private unsafe void CCT_AddChecksum(int idx)
    {
      ushort num1 = 0;
      switch (this.bChecksumType)
      {
        case Control_F40_CCTalk.CHECKSUMTYPE.CHK8BIT:
          for (int index = 0; index < this.msgTxLen[idx]; ++index)
            num1 += (ushort) this.cct_tx[idx][index];
          ushort num2 = (ushort) (byte) -num1;
          this.cct_tx[idx][this.msgTxLen[idx]++] = (byte) ((uint) num2 & (uint) byte.MaxValue);
          break;
        case Control_F40_CCTalk.CHECKSUMTYPE.CRC16BIT:
          ushort num3;
          fixed (byte* pC1 = this.cct_tx[idx])
            fixed (byte* pC2 = &this.cct_tx[idx][3])
            {
              ushort seed = CCtalkUtilityDll.crcCalculation(2, pC1, (ushort) 0);
              num3 = CCtalkUtilityDll.crcCalculation(this.msgTxLen[idx] - 3, pC2, seed);
            }
          this.cct_tx[idx][this.msgTxLen[idx]++] = (byte) (((int) num3 & 65280) >> 8);
          this.cct_tx[idx][2] = (byte) ((uint) num3 & (uint) byte.MaxValue);
          break;
      }
    }

    public int CCT_PollsTransmit(int idx)
    {
      this.CCT_Send(idx);
      return 0;
    }

    public int CCT_PrepareMsg(int idx, string str, bool bAddChecksum)
    {
      this.msgTxLen[idx] = this.ParseTransmittingData(ref this.cct_tx[idx], str);
      if (bAddChecksum && this.msgTxLen[idx] > 0)
        this.CCT_AddChecksum(idx);
      return this.msgTxLen[idx];
    }

    private void CCT_Send(int idx)
    {
      if (this.sm == null)
        return;
      int index = 0;
      CCTEventArgs e = new CCTEventArgs(this.msgTxLen[idx]);
      this.sm.ResetRecIdx();
      if (this.bEchoEnabled)
      {
        this.bWaitForEcho = true;
        this.sm.SetRecThresh = this.msgTxLen[idx];
      }
      this.sm.Transmit(this.cct_tx[idx], this.msgTxLen[idx]);
      for (; index < this.msgTxLen[idx]; ++index)
        e.buf[index] = this.cct_tx[idx][index];
      e.CCTEvType = (byte) 0;
      this.tTimeOut.Interval = 200.0;
      this.tTimeOut.Start();
      if (this.OnCCTTxData == null)
        return;
      this.OnCCTTxData((object) this, e);
    }

    public int CCT_Transmit(string str, bool bAddChecksum)
    {
      this.CCT_PrepareMsg(0, str, bAddChecksum);
      this.CCT_Send(0);
      return 0;
    }

    private unsafe ushort CCT_VerifyChecksum(int nBytes)
    {
      ushort num1 = 0;
      switch (this.bChecksumType)
      {
        case Control_F40_CCTalk.CHECKSUMTYPE.CHK8BIT:
          for (int index = 0; index < nBytes; ++index)
            num1 += (ushort) this.cct_rx[index];
          return num1;
        case Control_F40_CCTalk.CHECKSUMTYPE.CRC16BIT:
          byte[] numArray = new byte[2]
          {
            this.cct_rx[nBytes - 1],
            this.cct_rx[2]
          };
          ushort num2;
          fixed (byte* pC1 = this.cct_rx)
            fixed (byte* pC2 = &this.cct_rx[3])
              fixed (byte* pC3 = numArray)
              {
                ushort seed1 = CCtalkUtilityDll.crcCalculation(2, pC1, (ushort) 0);
                ushort seed2 = CCtalkUtilityDll.crcCalculation(nBytes - 3, pC2, seed1);
                num2 = CCtalkUtilityDll.crcCalculation(2, pC3, seed2);
              }
          return num2;
        default:
          return num1;
      }
    }

    public void Close()
    {
      if (this.sm != null)
      {
        this.Disable_All();
        Thread.Sleep(100);
        this.Poll();
        this.sm.ClosePort();
        this.sm = (SerialManager) null;
      }
      this.OnLine = false;
    }

    private byte[] ConvertiLaStringa(string s)
    {
      byte[] numArray = new byte[s.Length + 2];
      char[] chArray = new char[s.Length + 2];
      int num = 1;
      chArray = s.ToCharArray();
      if (s[0] == '"' && s[s.Length - 1] == '"')
      {
        s = s.Substring(1, s.Length - 2);
        foreach (char ch in s.ToCharArray())
          numArray[num++] = (byte) ch;
        numArray[0] = (byte) s.Length;
        return numArray;
      }
      numArray[0] = (byte) 0;
      return numArray;
    }

    public void CreateCmdString(byte[] bySeq, bool send)
    {
      string str = "";
      for (int index = 0; index < bySeq.GetLength(0); ++index)
        str += string.Format("{0:X2} ", (object) bySeq[index]);
      this.CCT_Transmit(str.Trim(), true);
    }

    private bool CheckForAnswer(string s)
    {
      return !s.Contains("2A 2A 2A");
    }

    private void cct_OnCCTRxData(object sender, CCTEventArgs e)
    {
      if (this.sm == null)
        return;
      string s = "";
      for (int index = 0; index < e.nData; ++index)
        s += string.Format("{0:X} ", (object) e.buf[index]);
      if (e.nData >= 16)
      {
        this.TimeOutComs = 0;
        if ((int) this.Ult_ser != (int) e.buf[4])
        {
          this.Ult_ser = e.buf[4];
          if (e.buf[6] == (byte) 1)
          {
            this.bill = byte.MaxValue;
            if (e.buf[5] < (byte) 15)
            {
              this.CreateCmdString(new byte[5]
              {
                (byte) 40,
                (byte) 1,
                (byte) 1,
                (byte) 154,
                (byte) 1
              }, true);
              this.confirm_OK = true;
            }
            else
            {
              this.CreateCmdString(new byte[5]
              {
                (byte) 40,
                (byte) 1,
                (byte) 1,
                (byte) 154,
                (byte) 0
              }, true);
              this.confirm_OK = false;
            }
          }
          else if (e.buf[6] == (byte) 0 && e.buf[8] == (byte) 1 && this.confirm_OK)
          {
            this.CreateCmdString(new byte[5]
            {
              (byte) 40,
              (byte) 1,
              (byte) 1,
              (byte) 154,
              (byte) 1
            }, true);
            this.confirm_OK = false;
            this.bill = e.buf[5];
            if (this.bill > (byte) 0 && this.bill < (byte) 15 && this.NoMoney == 0 && this.Canal[(int) this.bill - 1] > 0)
              this.m_Creditos = this.Canal[(int) this.bill - 1];
          }
        }
      }
      if (e.CCTEvType != (byte) 2 || this.bFoundAcceptor || !this.CheckForAnswer(s) || !(s != "28 0 31 9F 5D ") || (s[0] != '1' || s[1] != ' ' || s[2] != 'B'))
        return;
      this.bFoundAcceptor = true;
    }

    private void cct_OnCCTTxData(object sender, CCTEventArgs e)
    {
      if (this.sm == null)
        ;
    }

    public bool Open()
    {
      if (this.sm != null)
      {
        this.sm.ClosePort();
        this.sm = (SerialManager) null;
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
      if (this.sm == null)
        this.sm = new SerialManager();
      this.sm.BAUDRATE = 9600;
      this.sm.PORTNAME = this.port;
      this.sm.OnReceivedData += new SerialManager.ByteReceivedManager(this.sm_OnReceivedData);
      this.OnCCTRxData += new Control_F40_CCTalk.CCtalkReceivedData(this.cct_OnCCTRxData);
      this.OnCCTTxData += new Control_F40_CCTalk.CCtalkTransmittedData(this.cct_OnCCTTxData);
      this.ECHO = false;
      this.CHKTYPE = Control_F40_CCTalk.CHECKSUMTYPE.CRC16BIT;
      this.OnLine = false;
      try
      {
        this.sm.OpenPort();
      }
      catch (Exception ex)
      {
        return false;
      }
      this.OnLine = true;
      this.NoMoney = 1;
      this.CCT_PrepareMsg(1, "28 0 1 9F", true);
      for (int index = 0; index < 5; ++index)
      {
        this.Poll();
        Thread.Sleep(100);
      }
      this.Enable();
      for (int index = 0; index < 10; ++index)
      {
        this.Poll();
        Thread.Sleep(100);
      }
      this.TimeOutComs = 0;
      return true;
    }

    public string Find_Device()
    {
      string[] portNames = SerialPort.GetPortNames();
      this.ECHO = false;
      this.CHKTYPE = Control_F40_CCTalk.CHECKSUMTYPE.CRC16BIT;
      bool flag = false;
      this.CCT_PrepareMsg(1, "28 0 1 9F", true);
      this._f_resp_scom = "-";
      this.respuesta = false;
      if (portNames == null)
        return "-";
      foreach (string str in portNames)
      {
        try
        {
          this.bFoundAcceptor = false;
          if (this.sm == null)
            this.sm = new SerialManager();
          this.sm.OnReceivedData += new SerialManager.ByteReceivedManager(this.sm_OnReceivedData);
          this.OnCCTRxData += new Control_F40_CCTalk.CCtalkReceivedData(this.cct_OnCCTRxData);
          this.OnCCTTxData += new Control_F40_CCTalk.CCtalkTransmittedData(this.cct_OnCCTTxData);
          this.sm.BAUDRATE = 9600;
          this.port = str;
          this.sm.PORTNAME = this.port;
          this.sm.OpenPort();
          this.bFoundAcceptor = false;
          flag = false;
          this.CCT_PrepareMsg(1, "28 0 1 9F", true);
          this.Poll();
          Thread.Sleep(100);
          this.Enable();
          this.Poll();
          Thread.Sleep(100);
          for (int index = 0; index < 20; ++index)
          {
            this.Poll();
            Thread.Sleep(100);
            if (this.bFoundAcceptor)
            {
              this.Disable();
              if (this.sm != null)
              {
                this.sm.ClosePort();
                this.sm = (SerialManager) null;
              }
              this.respuesta = true;
              this._f_resp_scom = str;
              return str;
            }
          }
          if (this.sm != null)
          {
            this.sm.ClosePort();
            this.sm = (SerialManager) null;
          }
        }
        catch
        {
          if (this.sm != null)
          {
            this.sm.ClosePort();
            this.sm = (SerialManager) null;
          }
        }
      }
      return "-";
    }

    public bool Enable_All()
    {
      try
      {
        this.CreateCmdString(new byte[5]
        {
          (byte) 40,
          (byte) 1,
          (byte) 234,
          (byte) 228,
          (byte) 1
        }, true);
        this.NoMoney = 0;
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public bool Enable()
    {
      try
      {
        this.CreateCmdString(new byte[6]
        {
          (byte) 40,
          (byte) 2,
          (byte) 110,
          (byte) 231,
          byte.MaxValue,
          byte.MaxValue
        }, true);
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public bool Disable()
    {
      try
      {
        this.CreateCmdString(new byte[6]
        {
          (byte) 40,
          (byte) 2,
          (byte) 110,
          (byte) 231,
          (byte) 0,
          (byte) 0
        }, true);
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public bool Disable_All()
    {
      try
      {
        this.CreateCmdString(new byte[5]
        {
          (byte) 40,
          (byte) 1,
          (byte) 234,
          (byte) 228,
          (byte) 0
        }, true);
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    private int ParseTransmittingData(ref byte[] buff, string str)
    {
      int index1 = 0;
      byte[] numArray1 = new byte[100];
      bool flag = false;
      string str1 = str.Trim();
      int startIndex1 = 0;
      for (int startIndex2 = str1.IndexOf("  ", startIndex1); startIndex2 != -1; startIndex2 = str1.IndexOf("  ", startIndex2))
        str1 = str1.Remove(startIndex2, 1);
      while (!flag)
      {
        int num1 = str1.IndexOf(" ", startIndex1);
        string s1;
        if (num1 != -1)
        {
          s1 = str1.Substring(startIndex1, num1 - startIndex1);
        }
        else
        {
          s1 = str1.Substring(startIndex1);
          flag = true;
        }
        if (!string.IsNullOrEmpty(s1))
        {
          if (s1[0] == '"')
          {
            int num2 = str1.IndexOf('"', startIndex1 + 1);
            string s2;
            if (num2 != -1)
            {
              s2 = str1.Substring(startIndex1, num2 - startIndex1 + 1);
            }
            else
            {
              s2 = str1.Substring(startIndex1);
              flag = true;
            }
            byte[] numArray2 = this.ConvertiLaStringa(s2);
            for (int index2 = 0; index2 < (int) numArray2[0]; ++index2)
              buff[index1++] = numArray2[index2 + 1];
            startIndex1 = num2 + 1;
          }
          else
          {
            int num2 = s1.IndexOf("0x");
            if (num2 != -1)
              s1 = s1.Substring(num2 + 2);
            try
            {
              if (this.nNumericFormat == Control_F40_CCTalk.NUMERICFORMATTYPE.HEX)
                buff[index1] = byte.Parse(s1, NumberStyles.HexNumber);
              else if (this.nNumericFormat == Control_F40_CCTalk.NUMERICFORMATTYPE.DEC)
                buff[index1] = byte.Parse(s1, NumberStyles.Integer);
              startIndex1 = num1 + 1;
              ++index1;
            }
            catch (Exception ex)
            {
              return 0;
            }
          }
        }
      }
      return index1;
    }

    private void sm_OnReceivedData(object sender, CCTEventArgs e)
    {
      for (int idxStart = e.idxStart; idxStart < e.idxEnd; ++idxStart)
        this.cct_rx[idxStart] = this.sm.GetRxByte(idxStart);
      if (this.bWaitForEcho)
      {
        this.bWaitForEcho = false;
        e.msgChk = this.CCT_VerifyChecksum(e.nData);
        for (int index = 0; index < e.idxEnd; ++index)
          e.buf[index] = this.cct_rx[index];
        e.CCTEvType = (byte) 1;
        if (this.OnCCTRxData != null)
          this.OnCCTRxData((object) this, e);
        this.sm.ResetRecIdx();
      }
      else
      {
        e.CCTEvType = (byte) 2;
        if (e.idxStart == 0)
        {
          if (this.cct_rx[1] == (byte) 0)
          {
            e.msgChk = this.CCT_VerifyChecksum(e.nData);
            for (int index = 0; index < e.idxEnd; ++index)
              e.buf[index] = this.cct_rx[index];
            this.tTimeOut.Stop();
            if (this.OnCCTRxData != null)
              this.OnCCTRxData((object) this, e);
            this.sm.ResetRecIdx();
          }
          else
          {
            int num = (int) this.cct_rx[1] - this.sm.SaveBufferContent();
            if (num > 0)
            {
              this.sm.SetRecThresh = num;
            }
            else
            {
              for (int idxEnd = e.idxEnd; idxEnd < e.idxEnd + (int) this.cct_rx[1]; ++idxEnd)
                this.cct_rx[idxEnd] = this.sm.GetRxByte(idxEnd);
              e.idxEnd += (int) this.cct_rx[1];
              e.nData = e.idxEnd;
              e.msgChk = this.CCT_VerifyChecksum(e.nData);
              for (int index = 0; index < e.idxEnd; ++index)
                e.buf[index] = this.cct_rx[index];
              this.tTimeOut.Stop();
              if (this.OnCCTRxData != null)
                this.OnCCTRxData((object) this, e);
              this.sm.ResetRecIdx();
            }
          }
        }
        else
        {
          for (int index = 0; index < e.idxEnd; ++index)
            this.cct_rx[index] = this.sm.GetRxByte(index);
          e.msgChk = this.CCT_VerifyChecksum(e.idxEnd);
          for (int index = 0; index < e.idxEnd; ++index)
            e.buf[index] = this.cct_rx[index];
          e.nData = e.idxEnd;
          this.tTimeOut.Stop();
          if (this.OnCCTRxData != null)
            this.OnCCTRxData((object) this, e);
          this.sm.ResetRecIdx();
        }
      }
    }

    private void tTimeOut_Elapsed(object sender, ElapsedEventArgs e)
    {
      CCTEventArgs e1 = new CCTEventArgs(3);
      this.tTimeOut.Stop();
      int num = 0;
      while (num < 3)
        e1.buf[num++] = (byte) 42;
      e1.CCTEvType = (byte) 2;
      if (this.OnCCTRxData == null)
        return;
      this.OnCCTRxData((object) this, e1);
    }

    public bool ChecksumEnabled
    {
      get
      {
        return this.bChecksum;
      }
      set
      {
        this.bChecksum = value;
      }
    }

    public Control_F40_CCTalk.CHECKSUMTYPE CHKTYPE
    {
      get
      {
        return this.bChecksumType;
      }
      set
      {
        this.bChecksumType = value;
      }
    }

    public bool ECHO
    {
      get
      {
        return this.bEchoEnabled;
      }
      set
      {
        this.bEchoEnabled = value;
      }
    }

    public bool EncryprionEnable
    {
      get
      {
        return this.bEncrypt;
      }
      set
      {
        this.bEncrypt = value;
      }
    }

    public int EncryptionKey
    {
      get
      {
        return this.iEncryptionKey;
      }
      set
      {
        this.iEncryptionKey = value;
      }
    }

    public SerialManager GetSMRef
    {
      get
      {
        return this.sm;
      }
    }

    public Control_F40_CCTalk.NUMERICFORMATTYPE NUMFORMAT
    {
      get
      {
        return this.nNumericFormat;
      }
      set
      {
        this.nNumericFormat = value;
      }
    }

    public int READ_TIMEOUT
    {
      get
      {
        return this.iReadtTimeout;
      }
      set
      {
        this.iReadtTimeout = value;
      }
    }

    public delegate void CCtalkReceivedData(object sender, CCTEventArgs e);

    public delegate void CCtalkTransmittedData(object sender, CCTEventArgs e);

    public enum CHECKSUMTYPE
    {
      CHK8BIT,
      CRC16BIT,
    }

    public enum NUMERICFORMATTYPE
    {
      HEX,
      DEC,
    }
  }
}
