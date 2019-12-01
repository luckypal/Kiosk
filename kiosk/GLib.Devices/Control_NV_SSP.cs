using ITLlib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace GLib.Devices
{
	public class Control_NV_SSP
	{
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

		public static int MaxCanales = 16;

		public bool respuesta;

		public System.Windows.Forms.Timer reconnectionTimer;

		private int reconnectionInterval;

		public int TimeOutComs = 0;

		public bool IsEnable;

		private decimal _Creditos = 0m;

		public int ReStartNeed = 0;

		[Description("Creditos")]
		public decimal Creditos
		{
			get
			{
				return _Creditos;
			}
			set
			{
				_Creditos = value;
			}
		}

		[Description("Multiplicador")]
		public int Multiplicador
		{
			get
			{
				return m_ValueMultiplier;
			}
			set
			{
				m_ValueMultiplier = value;
			}
		}

		[Description("Canales")]
		public int Canales
		{
			get
			{
				return m_NumberOfChannels;
			}
			set
			{
				m_NumberOfChannels = value;
			}
		}

		public SSP_COMMAND CommandStructure
		{
			get
			{
				return cmd;
			}
			set
			{
				cmd = value;
			}
		}

		public SSP_COMMAND_INFO InfoStructure
		{
			get
			{
				return info;
			}
			set
			{
				info = value;
			}
		}

		public char UnitType => m_UnitType;

		public int NumberOfChannels
		{
			get
			{
				return m_NumberOfChannels;
			}
			set
			{
				m_NumberOfChannels = value;
			}
		}

		public int NumberOfNotesStacked
		{
			get
			{
				return m_NumStackedNotes;
			}
			set
			{
				m_NumStackedNotes = value;
			}
		}

		public int Multiplier
		{
			get
			{
				return m_ValueMultiplier;
			}
			set
			{
				m_ValueMultiplier = value;
			}
		}

		public Control_NV_SSP()
		{
			eSSP = new SSPComms();
			cmd = new SSP_COMMAND();
			storedCmd = new SSP_COMMAND();
			keys = new SSP_KEYS();
			sspKey = new SSP_FULL_KEY();
			info = new SSP_COMMAND_INFO();
			m_NumberOfChannels = 0;
			m_ValueMultiplier = 1;
			m_UnitType = 'ÿ';
			m_UnitDataList = new List<ChannelData>();
			reconnectionInterval = 2;
			reconnectionTimer = new System.Windows.Forms.Timer();
			respuesta = false;
			port = "COM3";
			Creditos = 0m;
			Canal = new int[MaxCanales];
			eCanal = new int[MaxCanales];
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
			Canal[6] = 50000;
			OnLine = false;
		}

		public string Find_Device()
		{
			return "-";
		}

		public void Start_Find_Device()
		{
			_f_resp_scom = "-";
			_f_ports = SerialPort.GetPortNames();
			_f_cnt = 0;
		}

		public bool Last_Find_Device()
		{
			if (_f_ports == null)
			{
				return false;
			}
			return _f_cnt < _f_ports.Length;
		}

		public bool Next_Find_Device()
		{
			if (eSSP != null)
			{
				eSSP.CloseComPort();
				eSSP = null;
			}
			if (_f_ports == null)
			{
				respuesta = false;
				_f_resp_scom = "-";
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
			return true;
		}

		public bool Poll_Find_Device()
		{
			respuesta = false;
			return Next_Find_Device();
		}

		public void Stop_Find_Device()
		{
			if (eSSP != null)
			{
				eSSP.CloseComPort();
				eSSP = null;
			}
		}

		public bool Open()
		{
			IsEnable = false;
			ReStartNeed = 0;
			TimeOutComs = 0;
			m_log = "";
			if (OnLine)
			{
				OnLine = false;
				DisableValidator(ref m_log);
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
			CommandStructure.ComPort = port;
			CommandStructure.SSPAddress = 0;
			CommandStructure.Timeout = 3000u;
			if (!ConnectToValidator())
			{
				m_log += "fail";
				respuesta = false;
				return false;
			}
			OnLine = true;
			_f_resp_scom = port;
			respuesta = true;
			return true;
		}

		public bool Poll()
		{
			if (OnLine)
			{
				m_log = "";
				return DoPoll(ref m_log);
			}
			return false;
		}

		public void Close()
		{
			IsEnable = false;
			if (eSSP != null)
			{
				Disable();
				Thread.Sleep(100);
				Poll();
			}
			if (eSSP != null)
			{
				eSSP.CloseComPort();
				eSSP = null;
			}
		}

		public bool Enable()
		{
			IsEnable = true;
			if (OnLine)
			{
				TimeOutComs = 0;
				m_log = "";
				EnableValidator(ref m_log);
				return true;
			}
			return false;
		}

		public bool Disable()
		{
			IsEnable = false;
			if (OnLine)
			{
				TimeOutComs = 0;
				m_log = "";
				DisableValidator(ref m_log);
				return true;
			}
			return false;
		}

		private byte FindMaxProtocolVersion()
		{
			m_log = "";
			byte b = 1;
			byte result = 0;
			for (b = 1; b < 10; b = (byte)(b + 1))
			{
				SetProtocolVersion(b, ref m_log);
				Thread.Sleep(200);
				if (CommandStructure.ResponseData[0] == 240)
				{
					result = b;
				}
			}
			return result;
		}

		private bool IsUnitTypeSupported(char type)
		{
			if (type == '\0')
			{
				return true;
			}
			return false;
		}

		private bool ConnectToValidator()
		{
			m_log = "";
			reconnectionTimer.Interval = reconnectionInterval * 1000;
			reconnectionTimer.Enabled = true;
			if (eSSP != null)
			{
				eSSP.CloseComPort();
				eSSP = null;
			}
			if (eSSP == null)
			{
				eSSP = new SSPComms();
			}
			CommandStructure.EncryptionStatus = false;
			if (OpenComPort(ref m_log) && NegotiateKeys(ref m_log))
			{
				CommandStructure.EncryptionStatus = true;
				byte b = FindMaxProtocolVersion();
				if (b >= 6)
				{
					SetProtocolVersion(6, ref m_log);
					SetupRequest(ref m_log);
					if (!IsUnitTypeSupported(UnitType))
					{
						return false;
					}
					SetInhibits(ref m_log);
					EnableValidator(ref m_log);
					return true;
				}
				return false;
			}
			return false;
		}

		public bool GetChannelInhibit(int channelNum)
		{
			if (channelNum >= 1 && channelNum <= m_NumberOfChannels)
			{
				foreach (ChannelData unitData in m_UnitDataList)
				{
					if (unitData.Channel == channelNum)
					{
						return unitData.Inhibit;
					}
				}
			}
			return true;
		}

		public bool GetChannelEnabled(int channelNum)
		{
			if (channelNum >= 1 && channelNum <= m_NumberOfChannels)
			{
				foreach (ChannelData unitData in m_UnitDataList)
				{
					if (unitData.Channel == channelNum)
					{
						return unitData.Enabled;
					}
				}
			}
			return false;
		}

		public int GetChannelValue(int channelNum)
		{
			if (channelNum >= 1 && channelNum <= m_NumberOfChannels)
			{
				foreach (ChannelData unitData in m_UnitDataList)
				{
					if (unitData.Channel == channelNum)
					{
						return unitData.Value;
					}
				}
			}
			return -1;
		}

		public string GetChannelCurrency(int channelNum)
		{
			if (channelNum >= 1 && channelNum <= m_NumberOfChannels)
			{
				foreach (ChannelData unitData in m_UnitDataList)
				{
					if (unitData.Channel == channelNum)
					{
						return new string(unitData.Currency);
					}
				}
			}
			return "";
		}

		public void EnableValidator(ref string log)
		{
			cmd.CommandData[0] = 10;
			cmd.CommandDataLength = 1;
			if (SendCommand(ref log) && CheckGenericResponses(ref log) && log != null)
			{
				log += "Unit enabled\r\n";
			}
		}

		public void DisableValidator(ref string log)
		{
			cmd.CommandData[0] = 9;
			cmd.CommandDataLength = 1;
			if (SendCommand(ref log) && CheckGenericResponses(ref log) && log != null)
			{
				log += "Unit disabled\r\n";
			}
		}

		public void Reset(ref string log)
		{
			cmd.CommandData[0] = 1;
			cmd.CommandDataLength = 1;
			if (SendCommand(ref log) && CheckGenericResponses(ref log))
			{
				log += "Resetting unit\r\n";
			}
		}

		public bool SendSync(string log = null)
		{
			cmd.CommandData[0] = 17;
			cmd.CommandDataLength = 1;
			if (!SendCommand(ref log))
			{
				return false;
			}
			if (CheckGenericResponses(ref log))
			{
				log += "Successfully sent sync\r\n";
			}
			return true;
		}

		public void SetProtocolVersion(byte pVersion, ref string log)
		{
			cmd.CommandData[0] = 6;
			cmd.CommandData[1] = pVersion;
			cmd.CommandDataLength = 2;
			if (SendCommand(ref log))
			{
			}
		}

		public void QueryRejection(ref string log)
		{
			cmd.CommandData[0] = 23;
			cmd.CommandDataLength = 1;
			if (SendCommand(ref log) && CheckGenericResponses(ref log) && log != null)
			{
				switch (cmd.ResponseData[1])
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
		}

		public bool NegotiateKeys(ref string log)
		{
			cmd.EncryptionStatus = false;
			if (log != null)
			{
				log += "Syncing... \r\n";
			}
			cmd.CommandData[0] = 17;
			cmd.CommandDataLength = 1;
			if (!SendCommand(ref log))
			{
				return false;
			}
			if (log != null)
			{
				log += "Success\r\n";
			}
			eSSP.InitiateSSPHostKeys(keys, cmd);
			cmd.CommandData[0] = 74;
			cmd.CommandDataLength = 9;
			if (log != null)
			{
				log += "Setting generator... \r\n";
			}
			for (byte b = 0; b < 8; b = (byte)(b + 1))
			{
				cmd.CommandData[b + 1] = (byte)(keys.Generator >> 8 * b);
			}
			if (!SendCommand(ref log))
			{
				return false;
			}
			if (log != null)
			{
				log += "Success\r\n";
			}
			cmd.CommandData[0] = 75;
			cmd.CommandDataLength = 9;
			if (log != null)
			{
				log += "Sending modulus... \r\n";
			}
			for (byte b = 0; b < 8; b = (byte)(b + 1))
			{
				cmd.CommandData[b + 1] = (byte)(keys.Modulus >> 8 * b);
			}
			if (!SendCommand(ref log))
			{
				return false;
			}
			if (log != null)
			{
				log += "Success\r\n";
			}
			cmd.CommandData[0] = 76;
			cmd.CommandDataLength = 9;
			if (log != null)
			{
				log += "Exchanging keys... ";
			}
			for (byte b = 0; b < 8; b = (byte)(b + 1))
			{
				cmd.CommandData[b + 1] = (byte)(keys.HostInter >> 8 * b);
			}
			if (!SendCommand(ref log))
			{
				return false;
			}
			if (log != null)
			{
				log += "Success\r\n";
			}
			keys.SlaveInterKey = 0uL;
			for (byte b = 0; b < 8; b = (byte)(b + 1))
			{
				keys.SlaveInterKey += (ulong)cmd.ResponseData[1 + b] << 8 * b;
			}
			eSSP.CreateSSPHostEncryptionKey(keys);
			cmd.Key.FixedKey = 81985526925837671uL;
			cmd.Key.VariableKey = keys.KeyHost;
			if (log != null)
			{
				log += "Keys successfully negotiated\r\n";
			}
			return true;
		}

		public void SetupRequest(ref string log)
		{
			cmd.CommandData[0] = 5;
			cmd.CommandDataLength = 1;
			if (!SendCommand(ref log))
			{
				return;
			}
			string str = "Unit Type: ";
			int num = 1;
			m_UnitType = (char)cmd.ResponseData[num++];
			switch (m_UnitType)
			{
			case '\0':
				str += "Validator";
				break;
			case '\u0003':
				str += "SMART Hopper";
				break;
			case '\u0006':
				str += "SMART Payout";
				break;
			case '\a':
				str += "NV11";
				break;
			default:
				str += "Unknown Type";
				break;
			}
			str += "\r\nFirmware: ";
			while (num <= 5)
			{
				str += (char)cmd.ResponseData[num++];
				if (num == 4)
				{
					str += ".";
				}
			}
			num = 9;
			num = 12;
			str += "\r\nNumber of Channels: ";
			str = str + (m_NumberOfChannels = cmd.ResponseData[num++]) + "\r\n";
			num = 13 + m_NumberOfChannels;
			int[] array = new int[m_NumberOfChannels];
			for (int i = 0; i < m_NumberOfChannels; i++)
			{
				array[i] = cmd.ResponseData[13 + m_NumberOfChannels + i];
			}
			num = 13 + m_NumberOfChannels * 2;
			str += "Real Value Multiplier: ";
			m_ValueMultiplier = cmd.ResponseData[num + 2];
			m_ValueMultiplier += cmd.ResponseData[num + 1] << 8;
			m_ValueMultiplier += cmd.ResponseData[num] << 16;
			str = str + m_ValueMultiplier + "\r\nProtocol Version: ";
			num += 3;
			num = 16 + m_NumberOfChannels * 2;
			int num2 = cmd.ResponseData[num++];
			str = str + num2 + "\r\n";
			num = 17 + m_NumberOfChannels * 2;
			int num3 = 17 + m_NumberOfChannels * 5;
			int num4 = 0;
			byte[] array2 = new byte[3 * m_NumberOfChannels];
			while (num < num3)
			{
				object obj = str;
				str = obj + "Channel " + (num4 / 3 + 1) + ", currency: ";
				array2[num4] = cmd.ResponseData[num++];
				str += (char)array2[num4++];
				array2[num4] = cmd.ResponseData[num++];
				str += (char)array2[num4++];
				array2[num4] = cmd.ResponseData[num++];
				str += (char)array2[num4++];
				str += "\r\n";
			}
			num = num3;
			str += "Expanded channel values:\r\n";
			num3 = 17 + m_NumberOfChannels * 9;
			int num5 = 0;
			num4 = 0;
			int[] array3 = new int[m_NumberOfChannels];
			while (num < num3)
			{
				num5 = (array3[num4] = CHelpers.ConvertBytesToInt32(cmd.ResponseData, num));
				num += 4;
				object obj = str;
				str = obj + "Channel " + ++num4 + ", value = " + num5 + "\r\n";
			}
			m_UnitDataList.Clear();
			for (byte b = 0; b < m_NumberOfChannels; b = (byte)(b + 1))
			{
				ChannelData channelData = new ChannelData();
				channelData.Channel = b;
				channelData.Channel++;
				channelData.Value = array3[b] * Multiplier;
				channelData.Currency[0] = (char)array2[b * 3];
				channelData.Currency[1] = (char)array2[1 + b * 3];
				channelData.Currency[2] = (char)array2[2 + b * 3];
				channelData.Level = 0;
				channelData.Recycling = false;
				channelData.Enabled = ((array[b] != 4) ? true : false);
				channelData.Inhibit = false;
				m_UnitDataList.Add(channelData);
			}
			m_UnitDataList.Sort((ChannelData d1, ChannelData d2) => d1.Value.CompareTo(d2.Value));
			if (log != null)
			{
				log += str;
			}
		}

		public void SetInhibits(ref string log)
		{
			cmd.CommandData[0] = 2;
			cmd.CommandData[1] = byte.MaxValue;
			cmd.CommandData[2] = byte.MaxValue;
			cmd.CommandDataLength = 3;
			if (SendCommand(ref log) && CheckGenericResponses(ref log) && log != null)
			{
				log += "Inhibits set\r\n";
			}
		}

		public void SetEnableds(int _mask, ref string log)
		{
			cmd.CommandData[0] = 2;
			cmd.CommandData[1] = (byte)(_mask & 0xFF);
			cmd.CommandData[2] = (byte)((_mask >> 8) & 0xFF);
			cmd.CommandDataLength = 3;
			if (SendCommand(ref log) && CheckGenericResponses(ref log) && log != null)
			{
				log += "Enableds set\r\n";
			}
		}

		public bool DoPoll(ref string log)
		{
			cmd.CommandData[0] = 7;
			cmd.CommandDataLength = 1;
			cmd.ResponseDataLength = 0;
			if (!SendCommand(ref log))
			{
				return false;
			}
			int num = 0;
			ReStartNeed = 0;
			if (IsEnable)
			{
				TimeOutComs++;
			}
			if (cmd.ResponseDataLength == 1 && cmd.ResponseData[0] == 250)
			{
				TimeOutComs = 1000;
			}
			for (byte b = 1; b < cmd.ResponseDataLength; b = (byte)(b + 1))
			{
				TimeOutComs = 0;
				switch (cmd.ResponseData[b])
				{
				case 241:
					log += "Unit reset\r\n";
					break;
				case 239:
					if (cmd.ResponseData[b + 1] > 0)
					{
						num = GetChannelValue(cmd.ResponseData[b + 1]);
						log = log + "Note in escrow, amount: " + CHelpers.FormatToCurrency(num) + "\r\n";
					}
					else
					{
						log += "Reading note...\r\n";
					}
					b = (byte)(b + 1);
					break;
				case 238:
					num = GetChannelValue(cmd.ResponseData[b + 1]);
					log = log + "Credit " + CHelpers.FormatToCurrency(num) + "\r\n";
					Creditos = num;
					m_NumStackedNotes++;
					b = (byte)(b + 1);
					break;
				case 237:
					log += "Rejecting note...\r\n";
					break;
				case 236:
					log += "Note rejected\r\n";
					QueryRejection(ref log);
					break;
				case 204:
					log += "Stacking note...\r\n";
					break;
				case 235:
					log += "Note stacked\r\n";
					break;
				case 234:
					log += "Safe jam\r\n";
					break;
				case 233:
					log += "Unsafe jam\r\n";
					break;
				case 232:
					TimeOutComs = 1000;
					break;
				case 230:
				{
					object obj = log;
					log = obj + "Fraud attempt, note type: " + GetChannelValue(cmd.ResponseData[b + 1]) + "\r\n";
					b = (byte)(b + 1);
					break;
				}
				case 231:
					log += "Stacker full\r\n";
					break;
				case 225:
					log = log + GetChannelValue(cmd.ResponseData[b + 1]) + " note cleared from front at reset.\r\n";
					b = (byte)(b + 1);
					break;
				case 226:
					log = log + GetChannelValue(cmd.ResponseData[b + 1]) + " note cleared to stacker at reset.\r\n";
					b = (byte)(b + 1);
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
				case 209:
					log += "Bar code ticket accepted\r\n";
					break;
				case 224:
					log += "Note path open\r\n";
					break;
				case 181:
					log += "All channels inhibited, unit disabled\r\n";
					break;
				default:
				{
					object obj = log;
					log = obj + "Unrecognised poll response detected " + (int)cmd.ResponseData[b] + "\r\n";
					break;
				}
				}
			}
			return true;
		}

		public bool OpenComPort(ref string log)
		{
			if (log != null)
			{
				log += "Opening com port\r\n";
			}
			if (eSSP == null)
			{
				eSSP = new SSPComms();
			}
			if (!eSSP.OpenSSPComPort(cmd))
			{
				return false;
			}
			return true;
		}

		private bool CheckGenericResponses(ref string log)
		{
			if (cmd.ResponseData[0] == 240)
			{
				return true;
			}
			if (log != null)
			{
				switch (cmd.ResponseData[0])
				{
				case 245:
					if (cmd.ResponseData[1] == 3)
					{
						log += "Validator has responded with \"Busy\", command cannot be processed at this time\r\n";
					}
					else
					{
						log = log + "Command response is CANNOT PROCESS COMMAND, error code - 0x" + BitConverter.ToString(cmd.ResponseData, 1, 1) + "\r\n";
					}
					return false;
				case 248:
					log += "Command response is FAIL\r\n";
					return false;
				case 250:
					log += "Command response is KEY NOT SET, Validator requires encryption on this command or there isa problem with the encryption on this request\r\n";
					return false;
				case 244:
					log += "Command response is PARAM OUT OF RANGE\r\n";
					return false;
				case 246:
					log += "Command response is SOFTWARE ERROR\r\n";
					return false;
				case 242:
					log += "Command response is UNKNOWN\r\n";
					return false;
				case 243:
					log += "Command response is WRONG PARAMETERS\r\n";
					return false;
				default:
					return false;
				}
			}
			return false;
		}

		public bool SendCommand(ref string log)
		{
			byte[] array = new byte[255];
			cmd.CommandData.CopyTo(array, 0);
			byte commandDataLength = cmd.CommandDataLength;
			if (eSSP == null)
			{
				eSSP = new SSPComms();
				Open();
			}
			if (!eSSP.SSPSendCommand(cmd, info))
			{
				eSSP.CloseComPort();
				if (log != null)
				{
					log = log + "Sending command failed\r\nPort status: " + cmd.ResponseStatus.ToString() + "\r\n";
				}
				return false;
			}
			return true;
		}
	}
}
