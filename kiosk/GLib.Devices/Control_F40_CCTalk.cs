using System;
using System.Globalization;
using System.IO.Ports;
using System.Threading;
using System.Timers;

namespace GLib.Devices
{
	public class Control_F40_CCTalk
	{
		public delegate void CCtalkReceivedData(object sender, CCTEventArgs e);

		public delegate void CCtalkTransmittedData(object sender, CCTEventArgs e);

		public enum CHECKSUMTYPE
		{
			CHK8BIT,
			CRC16BIT
		}

		public enum NUMERICFORMATTYPE
		{
			HEX,
			DEC
		}

		public const int idCCTALK = 40;

		public int[] Canal;

		private bool bChecksum;

		private CHECKSUMTYPE bChecksumType;

		private bool bEchoEnabled;

		private bool bEncrypt;

		private bool bWaitForEcho;

		private byte[] cct_rx = new byte[256];

		private byte[][] cct_tx = new byte[4][]
		{
			new byte[1024],
			new byte[1024],
			new byte[1024],
			new byte[1024]
		};

		private int iEncryptionKey;

		private int iReadtTimeout;

		private int[] msgTxLen = new int[4];

		private NUMERICFORMATTYPE nNumericFormat = NUMERICFORMATTYPE.HEX;

		private SerialManager sm = null;

		private System.Timers.Timer tTimeOut;

		public bool OnLine = false;

		private int m_Creditos;

		private bool bFoundAcceptor = false;

		private bool confirm_OK = false;

		public int[] eCanal;

		public decimal Base = 100m;

		private int NoMoney = 1;

		public string _f_resp_scom;

		public string _f_com;

		private string[] _f_ports;

		private int _f_cnt;

		public bool respuesta;

		public string port;

		public int Canales = 8;

		private byte Ult_ser = byte.MaxValue;

		private byte bill = 0;

		private string trans_cmd;

		private string echoNull = "";

		public int TimeOutComs = 0;

		public int Creditos
		{
			get
			{
				return m_Creditos;
			}
			set
			{
				m_Creditos = value;
			}
		}

		public bool ChecksumEnabled
		{
			get
			{
				return bChecksum;
			}
			set
			{
				bChecksum = value;
			}
		}

		public CHECKSUMTYPE CHKTYPE
		{
			get
			{
				return bChecksumType;
			}
			set
			{
				bChecksumType = value;
			}
		}

		public bool ECHO
		{
			get
			{
				return bEchoEnabled;
			}
			set
			{
				bEchoEnabled = value;
			}
		}

		public bool EncryprionEnable
		{
			get
			{
				return bEncrypt;
			}
			set
			{
				bEncrypt = value;
			}
		}

		public int EncryptionKey
		{
			get
			{
				return iEncryptionKey;
			}
			set
			{
				iEncryptionKey = value;
			}
		}

		public SerialManager GetSMRef => sm;

		public NUMERICFORMATTYPE NUMFORMAT
		{
			get
			{
				return nNumericFormat;
			}
			set
			{
				nNumericFormat = value;
			}
		}

		public int READ_TIMEOUT
		{
			get
			{
				return iReadtTimeout;
			}
			set
			{
				iReadtTimeout = value;
			}
		}

		public event CCtalkReceivedData OnCCTRxData;

		public event CCtalkTransmittedData OnCCTTxData;

		public bool Poll()
		{
			CCT_PollsTransmit(1);
			TimeOutComs++;
			return true;
		}

		public Control_F40_CCTalk()
		{
			READ_TIMEOUT = 100;
			tTimeOut = new System.Timers.Timer();
			tTimeOut.Elapsed += tTimeOut_Elapsed;
			bEchoEnabled = true;
			bWaitForEcho = false;
			bChecksumType = CHECKSUMTYPE.CHK8BIT;
			NoMoney = 1;
			confirm_OK = false;
			Creditos = 0;
			bFoundAcceptor = false;
			Base = 100m;
			Canal = new int[Canales];
			eCanal = new int[Canales];
			for (int i = 0; i < Canales; i++)
			{
				Canal[i] = 0;
				eCanal[i] = 0;
			}
			eCanal[0] = 1;
			eCanal[1] = 1;
			eCanal[2] = 1;
			eCanal[3] = 1;
			eCanal[4] = 1;
			eCanal[5] = 1;
			Canal[0] = 500;
			Canal[1] = 1000;
			Canal[2] = 2000;
			Canal[3] = 5000;
			Canal[4] = 10000;
			Canal[5] = 20000;
			respuesta = false;
			OnLine = false;
		}

		public void Set_Euro()
		{
			for (int i = 0; i < Canal.Length; i++)
			{
				Canal[i] = 0;
				eCanal[i] = 0;
			}
			eCanal[0] = 1;
			eCanal[1] = 1;
			eCanal[2] = 1;
			eCanal[3] = 1;
			eCanal[4] = 1;
			eCanal[5] = 1;
			Canal[0] = 500;
			Canal[1] = 1000;
			Canal[2] = 2000;
			Canal[3] = 5000;
			Canal[4] = 10000;
			Canal[5] = 20000;
		}

		public void Set_Brazil()
		{
			for (int i = 0; i < Canal.Length; i++)
			{
				Canal[i] = 0;
				eCanal[i] = 0;
			}
			eCanal[0] = 0;
			eCanal[1] = 0;
			eCanal[2] = 1;
			eCanal[3] = 1;
			eCanal[4] = 1;
			eCanal[5] = 1;
			Canal[0] = 0;
			Canal[1] = 0;
			Canal[2] = 500;
			Canal[3] = 1000;
			Canal[4] = 2000;
			Canal[5] = 5000;
		}

		public void Set_Dominicana()
		{
			for (int i = 0; i < Canal.Length; i++)
			{
				Canal[i] = 0;
				eCanal[i] = 0;
			}
			eCanal[0] = 1;
			eCanal[1] = 1;
			eCanal[2] = 1;
			eCanal[3] = 1;
			eCanal[4] = 1;
			eCanal[5] = 1;
			eCanal[6] = 1;
			Canal[0] = 200;
			Canal[1] = 500;
			Canal[2] = 1000;
			Canal[3] = 2000;
			Canal[4] = 5000;
			Canal[5] = 10000;
			Canal[6] = 20000;
		}

		public bool GetChannelInhibit(int _c)
		{
			if (_c < Canales)
			{
				return (eCanal[_c - 1] == 0) ? true : false;
			}
			return true;
		}

		public int GetChannelValue(int _c)
		{
			if (_c < Canales)
			{
				return Canal[_c - 1];
			}
			return 0;
		}

		public bool GetChannelEnabled(int _c)
		{
			if (_c >= Canales)
			{
				return false;
			}
			return true;
		}

		public string GetChannelCurrency(int _c)
		{
			return "EUR";
		}

		public void Start_Find_Device()
		{
			_f_resp_scom = "-";
			_f_ports = SerialPort.GetPortNames();
			_f_cnt = 0;
		}

		public bool Next_Find_Device()
		{
			if (sm != null)
			{
				sm.ClosePort();
				sm = null;
			}
			if (_f_ports == null)
			{
				respuesta = false;
				_f_resp_scom = "-";
				_f_com = "-";
				return true;
			}
			if (_f_cnt < _f_ports.Length)
			{
				respuesta = false;
				_f_resp_scom = "-";
				_f_com = _f_ports[_f_cnt];
				port = _f_com;
				_f_cnt++;
				return Open();
			}
			respuesta = false;
			_f_resp_scom = "-";
			_f_com = "-";
			return true;
		}

		public bool Poll_Find_Device()
		{
			respuesta = false;
			return Next_Find_Device();
		}

		public void Stop_Find_Device()
		{
			if (sm != null)
			{
				sm.ClosePort();
				sm = null;
			}
		}

		private unsafe void CCT_AddChecksum(int idx)
		{
			ushort num = 0;
			switch (bChecksumType)
			{
			case CHECKSUMTYPE.CHK8BIT:
			{
				for (int i = 0; i < msgTxLen[idx]; i++)
				{
					num = (ushort)(num + cct_tx[idx][i]);
				}
				num = (byte)(-num);
				cct_tx[idx][msgTxLen[idx]++] = (byte)(num & 0xFF);
				break;
			}
			case CHECKSUMTYPE.CRC16BIT:
				fixed (byte* pC = cct_tx[idx])
				{
					fixed (byte* pC2 = &cct_tx[idx][3])
					{
						num = CCtalkUtilityDll.crcCalculation(2, pC, 0);
						num = CCtalkUtilityDll.crcCalculation(msgTxLen[idx] - 3, pC2, num);
					}
				}
				cct_tx[idx][msgTxLen[idx]++] = (byte)((num & 0xFF00) >> 8);
				cct_tx[idx][2] = (byte)(num & 0xFF);
				break;
			}
		}

		public int CCT_PollsTransmit(int idx)
		{
			CCT_Send(idx);
			return 0;
		}

		public int CCT_PrepareMsg(int idx, string str, bool bAddChecksum)
		{
			msgTxLen[idx] = ParseTransmittingData(ref cct_tx[idx], str);
			if (bAddChecksum && msgTxLen[idx] > 0)
			{
				CCT_AddChecksum(idx);
			}
			return msgTxLen[idx];
		}

		private void CCT_Send(int idx)
		{
			if (sm != null)
			{
				int i = 0;
				CCTEventArgs cCTEventArgs = new CCTEventArgs(msgTxLen[idx]);
				sm.ResetRecIdx();
				if (bEchoEnabled)
				{
					bWaitForEcho = true;
					sm.SetRecThresh = msgTxLen[idx];
				}
				sm.Transmit(cct_tx[idx], msgTxLen[idx]);
				for (; i < msgTxLen[idx]; i++)
				{
					cCTEventArgs.buf[i] = cct_tx[idx][i];
				}
				cCTEventArgs.CCTEvType = 0;
				tTimeOut.Interval = 200.0;
				tTimeOut.Start();
				if (this.OnCCTTxData != null)
				{
					this.OnCCTTxData(this, cCTEventArgs);
				}
			}
		}

		public int CCT_Transmit(string str, bool bAddChecksum)
		{
			CCT_PrepareMsg(0, str, bAddChecksum);
			CCT_Send(0);
			return 0;
		}

		private unsafe ushort CCT_VerifyChecksum(int nBytes)
		{
			ushort num = 0;
			switch (bChecksumType)
			{
			case CHECKSUMTYPE.CHK8BIT:
			{
				for (int i = 0; i < nBytes; i++)
				{
					num = (ushort)(num + cct_rx[i]);
				}
				return num;
			}
			case CHECKSUMTYPE.CRC16BIT:
			{
				byte[] array = new byte[2]
				{
					cct_rx[nBytes - 1],
					cct_rx[2]
				};
				fixed (byte* pC = cct_rx)
				{
					fixed (byte* pC2 = &cct_rx[3])
					{
						fixed (byte* pC3 = array)
						{
							num = CCtalkUtilityDll.crcCalculation(2, pC, 0);
							num = CCtalkUtilityDll.crcCalculation(nBytes - 3, pC2, num);
							num = CCtalkUtilityDll.crcCalculation(2, pC3, num);
						}
					}
				}
				return num;
			}
			default:
				return num;
			}
		}

		public void Close()
		{
			if (sm != null)
			{
				Disable_All();
				Thread.Sleep(100);
				Poll();
				sm.ClosePort();
				sm = null;
			}
			OnLine = false;
		}

		private byte[] ConvertiLaStringa(string s)
		{
			byte[] array = new byte[s.Length + 2];
			char[] array2 = new char[s.Length + 2];
			int num = 1;
			array2 = s.ToCharArray();
			if (s[0] == '"' && s[s.Length - 1] == '"')
			{
				s = s.Substring(1, s.Length - 2);
				char[] array3 = s.ToCharArray();
				foreach (char c in array3)
				{
					array[num++] = (byte)c;
				}
				array[0] = (byte)s.Length;
				return array;
			}
			array[0] = 0;
			return array;
		}

		public void CreateCmdString(byte[] bySeq, bool send)
		{
			string text = "";
			for (int i = 0; i < bySeq.GetLength(0); i++)
			{
				text += $"{bySeq[i]:X2} ";
			}
			text = text.Trim();
			CCT_Transmit(text, bAddChecksum: true);
		}

		private bool CheckForAnswer(string s)
		{
			if (s.Contains("2A 2A 2A"))
			{
				return false;
			}
			return true;
		}

		private void cct_OnCCTRxData(object sender, CCTEventArgs e)
		{
			if (sm == null)
			{
				return;
			}
			string text = "";
			for (int i = 0; i < e.nData; i++)
			{
				text += $"{e.buf[i]:X} ";
			}
			if (e.nData >= 16)
			{
				TimeOutComs = 0;
				if (Ult_ser != e.buf[4])
				{
					Ult_ser = e.buf[4];
					if (e.buf[6] == 1)
					{
						bill = byte.MaxValue;
						if (e.buf[5] < 15)
						{
							CreateCmdString(new byte[5]
							{
								40,
								1,
								1,
								154,
								1
							}, send: true);
							confirm_OK = true;
						}
						else
						{
							CreateCmdString(new byte[5]
							{
								40,
								1,
								1,
								154,
								0
							}, send: true);
							confirm_OK = false;
						}
					}
					else if (e.buf[6] == 0 && e.buf[8] == 1 && confirm_OK)
					{
						CreateCmdString(new byte[5]
						{
							40,
							1,
							1,
							154,
							1
						}, send: true);
						confirm_OK = false;
						bill = e.buf[5];
						if (bill > 0 && bill < 15 && NoMoney == 0 && Canal[bill - 1] > 0)
						{
							m_Creditos = Canal[bill - 1];
						}
					}
				}
			}
			if (e.CCTEvType == 2 && !bFoundAcceptor && CheckForAnswer(text) && text != "28 0 31 9F 5D " && text[0] == '1' && text[1] == ' ' && text[2] == 'B')
			{
				bFoundAcceptor = true;
			}
		}

		private void cct_OnCCTTxData(object sender, CCTEventArgs e)
		{
			if (sm != null)
			{
			}
		}

		public bool Open()
		{
			if (sm != null)
			{
				sm.ClosePort();
				sm = null;
			}
			_f_ports = SerialPort.GetPortNames();
			int num = 0;
			if (_f_ports != null)
			{
				for (int i = 0; i < _f_ports.Length; i++)
				{
					if (_f_ports[i].ToLower() == port.ToLower())
					{
						num = 1;
					}
				}
			}
			if (port == "-" || port == "?" || num == 0)
			{
				return false;
			}
			if (sm == null)
			{
				sm = new SerialManager();
			}
			sm.BAUDRATE = 9600;
			sm.PORTNAME = port;
			sm.OnReceivedData += sm_OnReceivedData;
			OnCCTRxData += cct_OnCCTRxData;
			OnCCTTxData += cct_OnCCTTxData;
			ECHO = false;
			CHKTYPE = CHECKSUMTYPE.CRC16BIT;
			OnLine = false;
			try
			{
				sm.OpenPort();
			}
			catch (Exception)
			{
				return false;
			}
			OnLine = true;
			NoMoney = 1;
			bool flag = false;
			CCT_PrepareMsg(1, "28 0 1 9F", bAddChecksum: true);
			for (int i = 0; i < 5; i++)
			{
				Poll();
				Thread.Sleep(100);
			}
			Enable();
			for (int i = 0; i < 10; i++)
			{
				Poll();
				Thread.Sleep(100);
			}
			TimeOutComs = 0;
			return true;
		}

		public string Find_Device()
		{
			string[] portNames = SerialPort.GetPortNames();
			ECHO = false;
			CHKTYPE = CHECKSUMTYPE.CRC16BIT;
			bool flag = false;
			CCT_PrepareMsg(1, "28 0 1 9F", bAddChecksum: true);
			_f_resp_scom = "-";
			respuesta = false;
			if (portNames == null)
			{
				return "-";
			}
			string[] array = portNames;
			foreach (string text in array)
			{
				try
				{
					bFoundAcceptor = false;
					if (sm == null)
					{
						sm = new SerialManager();
					}
					sm.OnReceivedData += sm_OnReceivedData;
					OnCCTRxData += cct_OnCCTRxData;
					OnCCTTxData += cct_OnCCTTxData;
					sm.BAUDRATE = 9600;
					port = text;
					sm.PORTNAME = port;
					sm.OpenPort();
					bFoundAcceptor = false;
					flag = false;
					CCT_PrepareMsg(1, "28 0 1 9F", bAddChecksum: true);
					Poll();
					Thread.Sleep(100);
					Enable();
					Poll();
					Thread.Sleep(100);
					for (int j = 0; j < 20; j++)
					{
						Poll();
						Thread.Sleep(100);
						if (bFoundAcceptor)
						{
							Disable();
							if (sm != null)
							{
								sm.ClosePort();
								sm = null;
							}
							respuesta = true;
							_f_resp_scom = text;
							return text;
						}
					}
					if (sm != null)
					{
						sm.ClosePort();
						sm = null;
					}
				}
				catch
				{
					if (sm != null)
					{
						sm.ClosePort();
						sm = null;
					}
				}
			}
			return "-";
		}

		public bool Enable_All()
		{
			try
			{
				CreateCmdString(new byte[5]
				{
					40,
					1,
					234,
					228,
					1
				}, send: true);
				NoMoney = 0;
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool Enable()
		{
			try
			{
				byte[] array = new byte[6]
				{
					40,
					2,
					110,
					231,
					0,
					0
				};
				array[4] = byte.MaxValue;
				array[5] = byte.MaxValue;
				CreateCmdString(array, send: true);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool Disable()
		{
			try
			{
				byte[] array = new byte[6]
				{
					40,
					2,
					110,
					231,
					0,
					0
				};
				array[4] = 0;
				array[5] = 0;
				CreateCmdString(array, send: true);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool Disable_All()
		{
			try
			{
				CreateCmdString(new byte[5]
				{
					40,
					1,
					234,
					228,
					0
				}, send: true);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		private int ParseTransmittingData(ref byte[] buff, string str)
		{
			int num = 0;
			byte[] array = new byte[100];
			bool flag = false;
			string text = str.Trim();
			int num2 = 0;
			int num3 = 0;
			for (num3 = text.IndexOf("  ", num2); num3 != -1; num3 = text.IndexOf("  ", num3))
			{
				text = text.Remove(num3, 1);
			}
			while (!flag)
			{
				num3 = text.IndexOf(" ", num2);
				string text2;
				if (num3 != -1)
				{
					text2 = text.Substring(num2, num3 - num2);
				}
				else
				{
					text2 = text.Substring(num2);
					flag = true;
				}
				if (string.IsNullOrEmpty(text2))
				{
					continue;
				}
				if (text2[0] == '"')
				{
					num3 = text.IndexOf('"', num2 + 1);
					if (num3 != -1)
					{
						text2 = text.Substring(num2, num3 - num2 + 1);
					}
					else
					{
						text2 = text.Substring(num2);
						flag = true;
					}
					array = ConvertiLaStringa(text2);
					for (int i = 0; i < array[0]; i++)
					{
						buff[num++] = array[i + 1];
					}
					num2 = num3 + 1;
				}
				else
				{
					int num4 = text2.IndexOf("0x");
					if (num4 != -1)
					{
						text2 = text2.Substring(num4 + 2);
					}
					try
					{
						if (nNumericFormat == NUMERICFORMATTYPE.HEX)
						{
							buff[num] = byte.Parse(text2, NumberStyles.HexNumber);
						}
						else if (nNumericFormat == NUMERICFORMATTYPE.DEC)
						{
							buff[num] = byte.Parse(text2, NumberStyles.Integer);
						}
						num2 = num3 + 1;
						num++;
					}
					catch (Exception)
					{
						return 0;
					}
				}
			}
			return num;
		}

		private void sm_OnReceivedData(object sender, CCTEventArgs e)
		{
			for (int i = e.idxStart; i < e.idxEnd; i++)
			{
				cct_rx[i] = sm.GetRxByte(i);
			}
			if (bWaitForEcho)
			{
				bWaitForEcho = false;
				e.msgChk = CCT_VerifyChecksum(e.nData);
				for (int i = 0; i < e.idxEnd; i++)
				{
					e.buf[i] = cct_rx[i];
				}
				e.CCTEvType = 1;
				if (this.OnCCTRxData != null)
				{
					this.OnCCTRxData(this, e);
				}
				sm.ResetRecIdx();
				return;
			}
			e.CCTEvType = 2;
			if (e.idxStart == 0)
			{
				if (cct_rx[1] == 0)
				{
					e.msgChk = CCT_VerifyChecksum(e.nData);
					for (int i = 0; i < e.idxEnd; i++)
					{
						e.buf[i] = cct_rx[i];
					}
					tTimeOut.Stop();
					if (this.OnCCTRxData != null)
					{
						this.OnCCTRxData(this, e);
					}
					sm.ResetRecIdx();
					return;
				}
				int num = cct_rx[1] - sm.SaveBufferContent();
				if (num > 0)
				{
					sm.SetRecThresh = num;
					return;
				}
				for (int i = e.idxEnd; i < e.idxEnd + cct_rx[1]; i++)
				{
					cct_rx[i] = sm.GetRxByte(i);
				}
				e.idxEnd += cct_rx[1];
				e.nData = e.idxEnd;
				e.msgChk = CCT_VerifyChecksum(e.nData);
				for (int i = 0; i < e.idxEnd; i++)
				{
					e.buf[i] = cct_rx[i];
				}
				tTimeOut.Stop();
				if (this.OnCCTRxData != null)
				{
					this.OnCCTRxData(this, e);
				}
				sm.ResetRecIdx();
			}
			else
			{
				for (int i = 0; i < e.idxEnd; i++)
				{
					cct_rx[i] = sm.GetRxByte(i);
				}
				e.msgChk = CCT_VerifyChecksum(e.idxEnd);
				for (int i = 0; i < e.idxEnd; i++)
				{
					e.buf[i] = cct_rx[i];
				}
				e.nData = e.idxEnd;
				tTimeOut.Stop();
				if (this.OnCCTRxData != null)
				{
					this.OnCCTRxData(this, e);
				}
				sm.ResetRecIdx();
			}
		}

		private void tTimeOut_Elapsed(object sender, ElapsedEventArgs e)
		{
			CCTEventArgs cCTEventArgs = new CCTEventArgs(3);
			tTimeOut.Stop();
			int num = 0;
			while (num < 3)
			{
				cCTEventArgs.buf[num++] = 42;
			}
			cCTEventArgs.CCTEvType = 2;
			if (this.OnCCTRxData != null)
			{
				this.OnCCTRxData(this, cCTEventArgs);
			}
		}
	}
}
