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
		public byte[] Send_Command;

		public readonly string ID = "CCTALK";

		public bool OnLine;

		public CommPort comm;

		public string port;

		public int[] Canal;

		public int[] eCanal;

		public int[] iCanal;

		public readonly int MaxCanales = 32;

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

		public bool EnablePoll = false;

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

		private static int MIDA = 16384;

		private int m_Creditos;

		private int m_Respuesta;

		private bool m_Test = false;

		private byte poll_mask;

		private byte last_cmd;

		private bool version;

		private int lastPoll = 0;

		private int CntTimeOut = 0;

		public string _f_resp_scom;

		public string _f_com;

		private string[] _f_ports;

		private int _f_cnt;

		private long TimeoutRead;

		private long CoinTimeoutPoll;

		private long BillTimeoutPoll;

		private int m_Respuesta_Point;

		private int lockCommand;

		[Description("Creditos")]
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

		public Control_CCTALK_COIN()
		{
			port = "COM1";
			comm = null;
			OnLine = false;
			Enabled = false;
			inici = 0;
			final = 0;
			bytes = 0;
			lockCommand = 0;
			ProtocolVersion = 0;
			Device_ID = 0;
			Creditos = 0;
			poll_mask = 0;
			m_Respuesta_Point = 0;
			m_Respuesta = 0;
			Multiplier = 1;
			CoinConnected = false;
			Connected = false;
			Base = 100;
			m_Test = false;
			Canales = MaxCanales;
			Currency = "EUR";
			buffer = new byte[MIDA];
			Canal = new int[MaxCanales];
			eCanal = new int[MaxCanales];
			iCanal = new int[MaxCanales];
			for (int i = 0; i < MaxCanales; i++)
			{
				Canal[i] = 0;
				eCanal[i] = 0;
				iCanal[i] = 0;
			}
		}

		public byte GetCRC(ref byte[] _buffer, int _len)
		{
			byte b = 0;
			for (int i = 0; i < _len; i++)
			{
				b = (byte)(b + _buffer[i]);
			}
			return (byte)(-b);
		}

		public byte GetCRC(ref byte[] _buffer)
		{
			return GetCRC(ref _buffer, _buffer.Length);
		}

		public bool IsCRCValid(ref byte[] _buffer)
		{
			if (_buffer.Length < 5)
			{
				return false;
			}
			byte cRC = GetCRC(ref _buffer, _buffer.Length - 1);
			return _buffer[_buffer.Length - 1] == cRC;
		}

		public bool Open()
		{
			OnLine = false;
			if (comm != null)
			{
				comm.Close();
				comm = null;
			}
			if (!port.ToUpper().Contains("COM"))
			{
				return false;
			}
			comm = null;
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
			Send_Command = null;
			Connected = false;
			EnablePoll = false;
			CoinConnected = false;
			lockCommand = 0;
			comm = new CommPort();
			comm.Open(port, 9600, Parity.None, 8, StopBits.One);
			Enabled = false;
			_f_resp_scom = port;
			return Startup();
		}

		public string GetIDString(int _dev, byte[] _cmd)
		{
			string result = "?";
			for (int i = 0; i < 4; i++)
			{
				Command(_dev, new byte[1]
				{
					254
				});
				if (Last_Cmd_Text == "ACK")
				{
					Command(_dev, _cmd);
					if (Wait_Resposta_Ack() && Last_Cmd_Text != "ACK")
					{
						result = Last_Cmd_Text;
						break;
					}
				}
			}
			return result;
		}

		public string GetIDSerial(int _dev, byte[] _cmd)
		{
			string result = "?";
			for (int i = 0; i < 4; i++)
			{
				Command(_dev, new byte[1]
				{
					254
				});
				if (!(Last_Cmd_Text == "ACK"))
				{
					continue;
				}
				Command(_dev, _cmd);
				if (Wait_Resposta_Ack() && Last_Cmd_Text != "ACK")
				{
					int num = 0;
					if (Last_Data.Length == 1)
					{
						num = Last_Data[0];
					}
					if (Last_Data.Length == 2)
					{
						num = Last_Data[0] + Last_Data[1] * 256;
					}
					if (Last_Data.Length == 3)
					{
						num = Last_Data[0] + Last_Data[1] * 256 + Last_Data[2] * 65536;
					}
					if (Last_Data.Length == 4)
					{
						num = Last_Data[0] + Last_Data[1] * 256 + Last_Data[2] * 65536 + Last_Data[3] * 16777216;
					}
					result = string.Concat(num);
					break;
				}
			}
			return result;
		}

		public int GetIDInt(int _dev, byte[] _cmd)
		{
			int result = 0;
			for (int i = 0; i < 4; i++)
			{
				Command(_dev, new byte[1]
				{
					254
				});
				if (Last_Cmd_Text == "ACK")
				{
					Command(_dev, _cmd);
					if (Wait_Resposta_Ack() && Last_Cmd_Text != "ACK")
					{
						result = Last_Data[0] + Last_Data[1] * 256;
						break;
					}
				}
			}
			return result;
		}

		public string GetIDSInt(int _dev, byte[] _cmd)
		{
			string result = "?";
			for (int i = 0; i < 4; i++)
			{
				Command(_dev, new byte[1]
				{
					254
				});
				if (Last_Cmd_Text == "ACK")
				{
					Command(_dev, _cmd);
					if (Wait_Resposta_Ack() && Last_Cmd_Text != "ACK")
					{
						result = string.Concat(Last_Data[0], Last_Data[1] * 256);
						break;
					}
				}
			}
			return result;
		}

		public int GetIDLong(int _dev, byte[] _cmd)
		{
			int result = 0;
			for (int i = 0; i < 4; i++)
			{
				Command(_dev, new byte[1]
				{
					254
				});
				if (Last_Cmd_Text == "ACK")
				{
					Command(_dev, _cmd);
					if (Wait_Resposta_Ack() && Last_Cmd_Text != "ACK")
					{
						result = Last_Data[0] + Last_Data[1] * 256 + Last_Data[2] * 65536 + Last_Data[3] * 16777216;
						break;
					}
				}
			}
			return result;
		}

		public string GetIDSLong(int _dev, byte[] _cmd, int _x)
		{
			string result = "?";
			for (int i = 0; i < 4; i++)
			{
				Command(_dev, new byte[1]
				{
					254
				});
				if (Last_Cmd_Text == "ACK")
				{
					Command(_dev, _cmd);
					if (Wait_Resposta_Ack() && Last_Cmd_Text != "ACK")
					{
						int num = Last_Data[0] + Last_Data[1] * 256 + Last_Data[2] * 65536 + Last_Data[3] * 16777216;
						result = ((_x != 16) ? $"{num}" : $"{num:X08}");
						break;
					}
				}
			}
			return result;
		}

		public string GetIDVer(int _dev, byte[] _cmd)
		{
			string result = "?";
			for (int i = 0; i < 4; i++)
			{
				Command(_dev, new byte[1]
				{
					254
				});
				if (Last_Cmd_Text == "ACK")
				{
					Command(_dev, _cmd);
					if (Wait_Resposta_Ack() && Last_Cmd_Text != "ACK")
					{
						result = Last_Data[0] + "." + Last_Data[1] + "." + Last_Data[2];
						break;
					}
				}
			}
			return result;
		}

		public string GetIDTest(int _dev, byte[] _cmd)
		{
			string result = "";
			for (int i = 0; i < 4; i++)
			{
				Command(_dev, new byte[1]
				{
					254
				});
				if (!(Last_Cmd_Text == "ACK"))
				{
					continue;
				}
				Command(_dev, _cmd);
				if (Wait_Resposta_Ack() && Last_Cmd_Text != "ACK")
				{
					result = ((Last_Data[0] != 0) ? ("ERROR " + Last_Data[0]) : "OK");
					if (Last_Data.Length <= 1)
					{
					}
					break;
				}
			}
			return result;
		}

		public string GetIDStorage(int _dev, byte[] _cmd)
		{
			string result = "";
			for (int i = 0; i < 4; i++)
			{
				Command(_dev, new byte[1]
				{
					254
				});
				if (!(Last_Cmd_Text == "ACK"))
				{
					continue;
				}
				Command(_dev, _cmd);
				if (Wait_Resposta_Ack() && Last_Cmd_Text != "ACK")
				{
					switch (Last_Data[0])
					{
					case 0:
					case 1:
						result = "TYPE RAM";
						break;
					case 2:
						result = "TYPE EEPROM";
						break;
					case 3:
						result = "TYPE FRAM / BATTERY RAM";
						break;
					}
					break;
				}
			}
			return result;
		}

		public string GetIDSpeed(int _dev, byte[] _cmd)
		{
			string result = "";
			for (int i = 0; i < 4; i++)
			{
				Command(_dev, new byte[1]
				{
					254
				});
				if (!(Last_Cmd_Text == "ACK"))
				{
					continue;
				}
				Command(_dev, _cmd);
				if (Wait_Resposta_Ack() && Last_Cmd_Text != "ACK")
				{
					switch (Last_Data[0])
					{
					case 0:
						result = "?";
						break;
					case 1:
						result = Last_Data[1] + "ms";
						break;
					case 2:
						result = Last_Data[1] * 10 + "ms";
						break;
					case 3:
						result = Last_Data[1] + "seconds";
						break;
					case 4:
						result = Last_Data[1] + "min";
						break;
					case 5:
						result = Last_Data[1] + "hours";
						break;
					case 6:
						result = Last_Data[1] + "days";
						break;
					case 7:
						result = Last_Data[1] + "months";
						break;
					case 8:
						result = Last_Data[1] + "years";
						break;
					}
					break;
				}
			}
			return result;
		}

		public string GetIDSByte(int _dev, byte[] _cmd)
		{
			string result = "?";
			for (int i = 0; i < 4; i++)
			{
				Command(_dev, new byte[1]
				{
					254
				});
				if (Last_Cmd_Text == "ACK")
				{
					Command(_dev, _cmd);
					if (Wait_Resposta_Ack() && Last_Cmd_Text != "ACK")
					{
						result = string.Concat(Last_Data[0]);
						break;
					}
				}
			}
			return result;
		}

		public byte GetIDByte(int _dev, byte[] _cmd)
		{
			byte result = 0;
			for (int i = 0; i < 4; i++)
			{
				Command(_dev, new byte[1]
				{
					254
				});
				if (Last_Cmd_Text == "ACK")
				{
					Command(_dev, _cmd);
					if (Wait_Resposta_Ack() && Last_Cmd_Text != "ACK")
					{
						result = Last_Data[0];
						break;
					}
				}
			}
			return result;
		}

		public bool GetIDBool(int _dev, byte[] _cmd)
		{
			bool result = false;
			for (int i = 0; i < 4; i++)
			{
				Command(_dev, new byte[1]
				{
					254
				});
				if (Last_Cmd_Text == "ACK")
				{
					Command(_dev, _cmd);
					if (Wait_Resposta_Ack() && Last_Cmd_Text != "ACK")
					{
						result = ((Last_Data[0] & 1) == 1);
						break;
					}
				}
			}
			return result;
		}

		public string GetIDSBool(int _dev, byte[] _cmd)
		{
			string result = "?";
			for (int i = 0; i < 4; i++)
			{
				Command(_dev, new byte[1]
				{
					254
				});
				if (Last_Cmd_Text == "ACK")
				{
					Command(_dev, _cmd);
					if (Wait_Resposta_Ack() && Last_Cmd_Text != "ACK")
					{
						result = string.Concat((Last_Data[0] & 1) == 1);
						break;
					}
				}
			}
			return result;
		}

		public string GetCoinAcceptorInfo()
		{
			return Coin_Acceptor_Product + "\r\n" + Coin_Acceptor_Manufacturer + "\r\n" + Coin_Acceptor_Currency;
		}

		public bool IsAscii(char _c)
		{
			if ((_c >= 'a' && _c <= 'z') || (_c >= 'A' && _c <= 'Z'))
			{
				return true;
			}
			return false;
		}

		public bool IsAsciiNumbers(char _c)
		{
			if ((_c >= 'a' && _c <= 'z') || (_c >= 'A' && _c <= 'Z') || (_c >= '0' && _c <= '1'))
			{
				return true;
			}
			return false;
		}

		public int CVF_Credits(string _cvf)
		{
			char c = _cvf[2];
			char c2 = _cvf[3];
			char c3 = _cvf[4];
			if (c == '.')
			{
				c = '0';
			}
			if (c2 == '.')
			{
				c2 = '0';
			}
			if (c3 == '.')
			{
				c3 = '0';
			}
			string s = string.Concat(c, c2, c3);
			int result = 0;
			try
			{
				result = int.Parse(s);
			}
			catch
			{
			}
			return result;
		}

		public string CVF_Credits_Currency(int _c)
		{
			decimal num = (decimal)_c / 100m;
			return $"{num:0.00}";
		}

		public string GetChannelDescription(int _c)
		{
			int num = 0;
			if (_c <= Canales)
			{
				num = Canal[_c - 1];
			}
			return $"{num:0.00} {GetChannelCurrency(_c)}";
		}

		public string CVF_Currency(string _cvf)
		{
			string text = string.Concat(_cvf[0], _cvf[1]);
			switch (text.ToLower())
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
				Command(_dev, new byte[1]
				{
					245
				});
				return true;
			case "PRODUCT_COIN":
				Coin_Acceptor_Product = GetIDString(_dev, new byte[1]
				{
					244
				});
				logs = logs + "Product: " + Coin_Acceptor_Product + "\r\n";
				return true;
			case "BUILD_COIN":
				Coin_Acceptor_Build = GetIDString(_dev, new byte[1]
				{
					192
				});
				logs = logs + "Build: " + Coin_Acceptor_Build + "\r\n";
				return true;
			case "MANUFACTURER_COIN":
				Coin_Acceptor_Manufacturer = GetIDString(_dev, new byte[1]
				{
					246
				});
				logs = logs + "Manufacturer: " + Coin_Acceptor_Manufacturer + "\r\n";
				return true;
			case "SERIAL_COIN":
				Coin_Acceptor_Serial = GetIDSerial(_dev, new byte[1]
				{
					242
				});
				logs = logs + "Serial: " + Coin_Acceptor_Serial + "\r\n";
				return true;
			case "SOFT_COIN":
				Coin_Acceptor_Software = GetIDString(_dev, new byte[1]
				{
					241
				});
				logs = logs + "Software: " + Coin_Acceptor_Software + "\r\n";
				return true;
			case "COMMS_COIN":
				Coin_Acceptor_Comms = GetIDVer(_dev, new byte[1]
				{
					4
				});
				logs = logs + "Comms: " + Coin_Acceptor_Comms + "\r\n";
				return true;
			case "ENABLED_COIN":
			{
				Coin_Acceptor_Enabled = GetIDBool(_dev, new byte[1]
				{
					227
				});
				object obj = logs;
				logs = obj + "Enabled: " + Coin_Acceptor_Enabled + "\r\n";
				return true;
			}
			case "INHIBIT_COIN":
			{
				Coin_Acceptor_Inhibit = GetIDInt(_dev, new byte[1]
				{
					230
				});
				object obj = logs;
				logs = obj + "Inhibits: " + Coin_Acceptor_Inhibit + "\r\n";
				return true;
			}
			case "CHECKSUM_COIN":
				Coin_Acceptor_CheckSum = GetIDSLong(_dev, new byte[1]
				{
					197
				}, 16);
				logs = logs + "Checksum: " + Coin_Acceptor_CheckSum + "\r\n";
				return true;
			case "TEST_COIN":
				Coin_Acceptor_Test = GetIDTest(_dev, new byte[1]
				{
					232
				});
				logs = logs + "Status: " + Coin_Acceptor_Test + "\r\n";
				return true;
			case "CLEAR_COIN":
				Command(_dev, new byte[1]
				{
					3
				});
				return true;
			case "STORAGE_COIN":
				Coin_Acceptor_Storage = GetIDStorage(_dev, new byte[1]
				{
					216
				});
				logs = logs + "Storage: " + Coin_Acceptor_Storage + "\r\n";
				return true;
			case "STATUS_COIN":
				Coin_Acceptor_Status = GetIDTest(_dev, new byte[1]
				{
					248
				});
				logs = logs + "Status: " + Coin_Acceptor_Status + "\r\n";
				return true;
			case "SPEED_COIN":
				Coin_Acceptor_Speed = GetIDSpeed(_dev, new byte[1]
				{
					249
				});
				logs = logs + "Speed: " + Coin_Acceptor_Speed + "\r\n";
				return true;
			case "INSERTED_COIN":
				Coin_Acceptor_Inserted = GetIDSerial(_dev, new byte[1]
				{
					226
				});
				logs = logs + "Inserted coins: " + Coin_Acceptor_Inserted + "\r\n";
				return true;
			case "ACCEPTED_COIN":
				Coin_Acceptor_Accepted = GetIDSerial(_dev, new byte[1]
				{
					225
				});
				logs = logs + "Accepted coins: " + Coin_Acceptor_Accepted + "\r\n";
				return true;
			case "FRAUD_COIN":
				Coin_Acceptor_Fraud = GetIDSerial(_dev, new byte[1]
				{
					193
				});
				logs = logs + "Fraud coins: " + Coin_Acceptor_Fraud + "\r\n";
				return true;
			case "REJECTED_COIN":
				Coin_Acceptor_Rejected = GetIDSerial(_dev, new byte[1]
				{
					194
				});
				logs = logs + "Rejected coins: " + Coin_Acceptor_Rejected + "\r\n";
				return true;
			case "RESET":
				Command(_dev, new byte[1]
				{
					1
				});
				logs += "!RESET\r\n";
				lastPoll = 0;
				return true;
			case "POLL":
				Command(_dev, new byte[1]
				{
					254
				});
				return true;
			case "CURRENCY_COIN":
			{
				Coin_Acceptor_Currency = "";
				int num = 0;
				int num2 = 1;
				while (num2 < 8)
				{
					Command(_dev, new byte[1]
					{
						254
					});
					if (!(Last_Cmd_Text == "ACK"))
					{
						continue;
					}
					Command(_dev, new byte[2]
					{
						184,
						(byte)num2
					});
					if (Wait_Resposta_Ack() && Last_Cmd_Text != "ACK")
					{
						if (IsAscii(Last_Cmd_Text[0]))
						{
							eCanal[num2 - 1] = 1;
							iCanal[num2 - 1] = 1;
							Canal[num2 - 1] = CVF_Credits(Last_Cmd_Text);
							string coin_Acceptor_Currency = Coin_Acceptor_Currency;
							Coin_Acceptor_Currency = coin_Acceptor_Currency + CVF_Credits_Currency(Canal[num2 - 1]) + " " + CVF_Currency(Last_Cmd_Text) + " (" + Last_Cmd_Text + ")\r\n";
							num = 1;
						}
						else
						{
							eCanal[num2 - 1] = 0;
						}
						num2++;
					}
				}
				if (num == 0)
				{
					Coin_Acceptor_Currency = "";
				}
				logs = logs + "Currencies:\r\n" + Coin_Acceptor_Currency;
				return true;
			}
			default:
				return false;
			}
		}

		public bool Startup()
		{
			lockCommand++;
			SerialNumber = "";
			Thread.Sleep(200);
			Reset(Coin_Acceptor_ID);
			Thread.Sleep(200);
			GetInfo(Coin_Acceptor_ID, "TEST_COIN");
			Coin_Acceptor_ID = 2;
			GetInfo(Coin_Acceptor_ID, "TYPE");
			if (_IsCoinPresent)
			{
				GetInfo(Coin_Acceptor_ID, "TEST_COIN");
				GetInfo(Coin_Acceptor_ID, "POLL");
				GetInfo(Coin_Acceptor_ID, "RESET_COIN");
				GetInfo(Coin_Acceptor_ID, "POLL");
				GetInfo(Coin_Acceptor_ID, "STATUS_COIN");
				GetInfo(Coin_Acceptor_ID, "CURRENCY_COIN");
				GetInfo(Coin_Acceptor_ID, "INHIBIT_COIN");
				if (Coin_Acceptor_Currency == "")
				{
					_IsCoinPresent = false;
					EnablePoll = false;
				}
				else
				{
					EnablePoll = true;
				}
			}
			if (_IsCoinPresent)
			{
				OnLine = true;
			}
			lockCommand--;
			if (OnLine)
			{
				Inhibits(Coin_Acceptor_ID, new int[4]
				{
					20,
					50,
					100,
					200
				});
			}
			return OnLine;
		}

		public bool Wait_Resposta()
		{
			Thread.Sleep(150);
			DateTime t = DateTime.Now.AddMilliseconds(1050.0);
			while (m_Respuesta == m_Respuesta_Point)
			{
				Poll();
				if (DateTime.Now > t)
				{
					break;
				}
			}
			return m_Respuesta != m_Respuesta_Point;
		}

		public bool Wait_Resposta_Ack()
		{
			Thread.Sleep(150);
			DateTime t = DateTime.Now.AddMilliseconds(1050.0);
			while (m_Respuesta == m_Respuesta_Point || Last_Cmd_Text == "ACK")
			{
				Poll();
				if (Last_Cmd_Text == "ACK")
				{
					m_Respuesta_Point = m_Respuesta;
				}
				if (DateTime.Now > t)
				{
					break;
				}
			}
			return m_Respuesta != m_Respuesta_Point;
		}

		public bool Close()
		{
			OnLine = false;
			if (comm != null)
			{
				comm.Close();
			}
			return true;
		}

		public static byte[] InsertByteStuff(byte[] inData)
		{
			List<byte> list = new List<byte>(10);
			for (int i = 0; i < inData.Length; i++)
			{
				if (inData[i] == 127)
				{
					list.Add(inData[i]);
					list.Add(inData[i]);
				}
				else
				{
					list.Add(inData[i]);
				}
			}
			return list.ToArray();
		}

		public byte[] Build_Command(int _id, byte[] cmd)
		{
			int num = 0;
			byte[] _buffer = new byte[cmd.Length + 3];
			_buffer[num++] = (byte)_id;
			_buffer[num++] = (byte)(cmd.Length - 1);
			_buffer[num++] = 1;
			for (int i = 0; i < cmd.Length; i++)
			{
				_buffer[num++] = cmd[i];
			}
			ushort cRC = GetCRC(ref _buffer);
			byte[] array = new byte[cmd.Length + 4];
			num = 0;
			for (int i = 0; i < _buffer.Length; i++)
			{
				array[num++] = _buffer[i];
			}
			array[num] = (byte)cRC;
			return array;
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
			if (comm != null)
			{
				comm.Close();
				comm = null;
			}
			if (_f_ports == null)
			{
				_f_resp_scom = "-";
				return true;
			}
			if (_f_cnt < _f_ports.Length)
			{
				_f_resp_scom = "-";
				_f_com = _f_ports[_f_cnt];
				port = _f_com;
				_f_cnt++;
				return Open();
			}
			_f_resp_scom = "-";
			return true;
		}

		public bool Poll_Find_Device()
		{
			return Next_Find_Device();
		}

		public void Stop_Find_Device()
		{
			if (comm != null)
			{
				comm.Close();
				comm = null;
			}
		}

		private byte[] CopyFlat(int len)
		{
			byte[] array = new byte[len];
			for (int i = 0; i < len; i++)
			{
				int num = (inici + i) & (MIDA - 1);
				array[i] = buffer[num];
			}
			return array;
		}

		private byte[] CopyFlat(byte[] _buff, int _pos, int len)
		{
			byte[] array = new byte[len];
			for (int i = 0; i < len; i++)
			{
				array[i] = _buff[_pos + i];
			}
			return array;
		}

		private void Purge(int len)
		{
			for (int i = 0; i < len; i++)
			{
				int num = (inici + i) & (MIDA - 1);
				buffer[num] = 0;
			}
			inici = ((inici + len) & (MIDA - 1));
			bytes -= len;
			if (bytes <= 0)
			{
				Clear_Buffer();
			}
		}

		private void Clear_Buffer()
		{
			bytes = 0;
			inici = 0;
			final = 0;
			for (int i = 0; i < MIDA; i++)
			{
				buffer[i] = 0;
			}
		}

		public void Reset(int _dev)
		{
			logs = "";
			Command(_dev, new byte[1]
			{
				1
			});
		}

		private void Parse_Command(byte[] _data)
		{
			if (_data[3] != 0)
			{
				return;
			}
			int num = _data[1];
			if (num > 0)
			{
				Last_Data = CopyFlat(_data, 4, _data[1]);
				if (SendPoll)
				{
					if (OnLine)
					{
						int num2 = _data[4] - lastPoll;
						if (_data[4] != lastPoll)
						{
							int num3 = 0;
							if (_data[2] == Coin_Acceptor_ID && CoinConnected)
							{
								num3 = 1;
							}
							if (num2 < 5)
							{
								for (int i = 0; i < num2 * 2; i += 2)
								{
									int num4 = _data[5 + i];
									if (num4 > 0 && num4 < 8 && num3 == 1)
									{
										Last_Cmd_Text = "COIN " + CVF_Credits_Currency(Canal[num4 - 1]);
										logs = logs + Last_Cmd_Text + "\r\n";
										Creditos = Canal[num4 - 1];
									}
									else
									{
										Last_Cmd_Text = "ERROR COIN";
									}
								}
							}
						}
						lastPoll = _data[4];
					}
				}
				else
				{
					string @string = Encoding.Default.GetString(Last_Data);
					if (IsAsciiNumbers(@string[0]))
					{
						Last_Cmd_Text = @string;
					}
					else
					{
						Last_Cmd_Text = "!";
					}
					if (Last_Cmd_Text.ToLower() == "Coin Acceptor".ToLower())
					{
						_IsCoinPresent = true;
						Coin_Acceptor = Last_Cmd_Text;
						Coin_Acceptor_ID = _data[2];
					}
				}
			}
			else
			{
				Last_Cmd_Text = "ACK";
			}
			SendPoll = false;
		}

		public void Decode()
		{
			bool flag = false;
			while (bytes >= 5 && !flag)
			{
				int num = 1;
				byte[] array = CopyFlat(5);
				byte b = (byte)(array[1] + 5);
				if (bytes >= 5 && array[0] != 1 && array[2] != 1)
				{
					num = 0;
				}
				if (bytes >= b)
				{
					byte[] _buffer = CopyFlat(b);
					if (IsCRCValid(ref _buffer))
					{
						if (array[0] == 1)
						{
							if (_buffer[2] == Coin_Acceptor_ID)
							{
								CoinTimeoutPoll = DateTime.Now.Ticks / 10000000;
							}
							m_Respuesta++;
							Last_Packet = "";
							for (int i = 0; i < _buffer.Length; i++)
							{
								Last_Packet = Last_Packet + Convert.ToString(_buffer[i], 16) + " ";
							}
							Parse_Command(_buffer);
						}
						Purge(b);
					}
					else
					{
						num = 0;
					}
				}
				else
				{
					if (DateTime.Now.Ticks / 10000 > TimeoutRead)
					{
						num = 0;
					}
					flag = true;
				}
				if (num == 0)
				{
					Purge(1);
				}
			}
		}

		public bool Parser()
		{
			if (comm != null)
			{
				int num = 0;
				do
				{
					num = comm.Bytes;
					if (num > 0)
					{
						TimeoutRead = DateTime.Now.Ticks / 10000 + 250;
						byte[] array = comm.Read(num);
						for (int i = 0; i < array.Length; i++)
						{
							buffer[final] = array[i];
							final = (++final & (MIDA - 1));
							bytes++;
							if (bytes >= MIDA)
							{
								inici &= (++inici & (MIDA - 1));
								bytes = MIDA - 1;
							}
						}
					}
					Decode();
				}
				while (num > 0);
				return true;
			}
			return false;
		}

		public bool Poll()
		{
			if (comm != null)
			{
				if (Parser())
				{
					if (Send_Command != null)
					{
						comm.Write(Send_Command);
						Send_Command = null;
					}
					else
					{
						if (lockCommand > 0)
						{
							return true;
						}
						if (EnablePoll)
						{
							if (_IsCoinPresent)
							{
								long num = DateTime.Now.Ticks / 10000000 - CoinTimeoutPoll;
								if (num > 5 && CoinConnected)
								{
									Clear_Buffer();
									SendPoll = false;
									CoinConnected = false;
									byte[] data = Build_Command(Coin_Acceptor_ID, new byte[2]
									{
										228,
										0
									});
									comm.Write(data);
								}
								else
								{
									Clear_Buffer();
									SendPoll = true;
									byte[] data = Build_Command(Coin_Acceptor_ID, new byte[1]
									{
										229
									});
									comm.Write(data);
								}
							}
							Connected = CoinConnected;
						}
					}
				}
				return true;
			}
			return false;
		}

		public bool Command(int _id, byte[] _cmd)
		{
			if (comm == null)
			{
				return false;
			}
			DateTime t = DateTime.Now.AddMilliseconds(1000.0);
			Poll();
			while (Send_Command != null)
			{
				Poll();
				if (DateTime.Now > t)
				{
					return false;
				}
			}
			m_Respuesta_Point = m_Respuesta;
			Last_Cmd_Text = null;
			Send_Command = Build_Command(_id, _cmd);
			t = DateTime.Now.AddMilliseconds(1050.0);
			while (m_Respuesta == m_Respuesta_Point)
			{
				Poll();
				if (DateTime.Now > t)
				{
					break;
				}
			}
			return m_Respuesta != m_Respuesta_Point;
		}

		private bool IsChannelEnabled(int _v, int[] _list)
		{
			for (int i = 0; i < _list.Length; i++)
			{
				if (_v == _list[i])
				{
					return true;
				}
			}
			return false;
		}

		public bool Inhibits(int _dev, int[] _enabled)
		{
			if (comm == null)
			{
				return false;
			}
			lockCommand++;
			bool result = false;
			byte b = 1;
			byte b2 = 0;
			int num = 0;
			while (num < 8)
			{
				if (IsChannelEnabled(Canal[num], _enabled))
				{
					b2 = (byte)(b2 | b);
				}
				num++;
				b = (byte)(b << 1);
			}
			b = 1;
			byte b3 = 0;
			num = 0;
			while (num < 8)
			{
				if (IsChannelEnabled(Canal[num + 8], _enabled))
				{
					b3 = (byte)(b3 | b);
				}
				num++;
				b = (byte)(b << 1);
			}
			int num2 = b2 + b3 * 256;
			if (_dev == Coin_Acceptor_ID)
			{
				if (num2 != Coin_Acceptor_Inhibit)
				{
					result = Command(_dev, new byte[3]
					{
						231,
						b2,
						b3
					});
				}
				GetInfo(_dev, "INHIBIT_COIN");
				if (num2 != Coin_Acceptor_Inhibit)
				{
					result = false;
				}
			}
			lockCommand--;
			return result;
		}

		public bool Enable(int _dev)
		{
			if (comm == null)
			{
				return false;
			}
			lockCommand++;
			bool result = Command(_dev, new byte[2]
			{
				228,
				1
			});
			SendPoll = false;
			EnablePoll = true;
			if (_dev == Coin_Acceptor_ID)
			{
				CoinConnected = true;
				CoinTimeoutPoll = DateTime.Now.Ticks / 10000000;
			}
			Connected = CoinConnected;
			lockCommand--;
			return result;
		}

		public bool GetChannelInhibit(int _c)
		{
			if (_c <= Canales)
			{
				return (iCanal[_c - 1] == 0) ? true : false;
			}
			return true;
		}

		public decimal GetChannelValue(int _c)
		{
			if (_c <= Canales)
			{
				return Canal[_c - 1] * Multiplier * Base;
			}
			return 0m;
		}

		public bool GetChannelEnabled(int _c)
		{
			if (Canal[_c - 1] <= 0 || eCanal[_c - 1] == 4)
			{
				return false;
			}
			return true;
		}

		public string GetChannelCurrency(int _c)
		{
			return Currency;
		}

		public bool Disable(int _dev)
		{
			if (comm == null)
			{
				return false;
			}
			lockCommand++;
			if (_dev == Coin_Acceptor_ID)
			{
				CoinConnected = false;
			}
			bool result = Command(_dev, new byte[2]
			{
				228,
				0
			});
			lockCommand--;
			return result;
		}
	}
}
