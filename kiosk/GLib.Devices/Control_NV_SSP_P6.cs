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

		public readonly string ID = "SSP3";

		public bool OnLine;

		public CommPort comm;

		public string port;

		public int[] Canal;

		public int[] eCanal;

		public int[] iCanal;

		public readonly int MaxCanales = 16;

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

		private static int MIDA = 16384;

		private int m_Creditos;

		public bool m_Respuesta;

		private bool m_Test = false;

		private byte poll_mask;

		private byte last_cmd;

		private bool version;

		public string _f_resp_scom;

		public string _f_com;

		private string[] _f_ports;

		private int _f_cnt;

		public int TimeOutComs = 0;

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

		public Control_NV_SSP_P6()
		{
			port = "COM1";
			comm = null;
			OnLine = false;
			Enabled = false;
			inici = 0;
			final = 0;
			bytes = 0;
			ProtocolVersion = 0;
			Device_ID = 0;
			Creditos = 0;
			poll_mask = 0;
			Multiplier = 1;
			Base = 100;
			m_Respuesta = false;
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

		public static byte[] GetCRC(byte[] inData)
		{
			byte b = byte.MaxValue;
			byte b2 = byte.MaxValue;
			for (int i = 0; i < inData.Length; i++)
			{
				int num = inData[i] ^ b2;
				b2 = (byte)((CRC_Table[num] >> 8) ^ b);
				b = (byte)(CRC_Table[num] & 0xFF);
			}
			return new byte[2]
			{
				b,
				b2
			};
		}

		public static bool IsCRCValid(byte[] inData)
		{
			if (inData.Length < 5)
			{
				return false;
			}
			byte[] array = new byte[inData.Length - 3];
			Array.Copy(inData, 1, array, 0, inData.Length - 3);
			byte[] cRC = GetCRC(array);
			return cRC[0] == inData[inData.Length - 2] && cRC[1] == inData[inData.Length - 1];
		}

		public bool Open()
		{
			if (comm != null)
			{
				OnLine = false;
				comm.Close();
				comm = null;
			}
			TimeOutComs = 0;
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
			comm = new CommPort();
			comm.Open(port, 9600, Parity.None, 8, StopBits.Two);
			OnLine = false;
			Enabled = false;
			_f_resp_scom = port;
			return Startup();
		}

		public bool Startup()
		{
			SerialNumber = "";
			int num = 2;
			m_Respuesta = false;
			while (SerialNumber == "" && !m_Respuesta && num > 0)
			{
				num--;
				poll_mask = 128;
				Command(new byte[1]
				{
					17
				});
				if (!m_Respuesta)
				{
					continue;
				}
				Command(new byte[1]
				{
					9
				});
				int num2 = 3;
				do
				{
					version = true;
					Command(new byte[2]
					{
						6,
						(byte)num2
					});
					if (version)
					{
						num2++;
					}
				}
				while (version);
				num2--;
				if (num2 > 3)
				{
					comm.Close();
					return false;
				}
				Command(new byte[2]
				{
					6,
					3
				});
				Command(new byte[1]
				{
					12
				});
				Command(new byte[1]
				{
					5
				});
				Inhibits();
			}
			if (SerialNumber == "")
			{
				comm.Close();
				OnLine = false;
				return false;
			}
			OnLine = true;
			return true;
		}

		public void Wait_Resposta()
		{
			Thread.Sleep(100);
			DateTime t = DateTime.Now.AddMilliseconds(500.0);
			m_Respuesta = false;
			do
			{
				if (!m_Respuesta)
				{
					Parser();
					continue;
				}
				return;
			}
			while (!(DateTime.Now > t));
			version = false;
		}

		public bool Close()
		{
			if (comm != null)
			{
				Disable();
				Thread.Sleep(100);
				Poll();
			}
			if (comm != null)
			{
				comm.Close();
			}
			OnLine = false;
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

		public byte[] Build_Command(byte[] cmd)
		{
			int num = 0;
			byte[] array = new byte[cmd.Length + 2];
			array[num++] = (byte)(Device_ID | poll_mask);
			array[num++] = (byte)cmd.Length;
			last_cmd = cmd[0];
			for (int i = 0; i < cmd.Length; i++)
			{
				array[num++] = cmd[i];
			}
			byte[] cRC = GetCRC(array);
			byte[] array2 = new byte[cmd.Length + 5];
			array2[0] = 0;
			for (int i = 0; i < array.Length; i++)
			{
				array2[i + 1] = array[i];
			}
			array2[array2.Length - 2] = cRC[0];
			array2[array2.Length - 1] = cRC[1];
			array2 = InsertByteStuff(array2);
			array2[0] = 127;
			poll_mask ^= 128;
			return array2;
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
				m_Respuesta = false;
				_f_resp_scom = "-";
				return true;
			}
			if (_f_cnt < _f_ports.Length)
			{
				m_Respuesta = false;
				_f_resp_scom = "-";
				_f_com = _f_ports[_f_cnt];
				port = _f_com;
				_f_cnt++;
				return Open();
			}
			m_Respuesta = false;
			_f_resp_scom = "-";
			return true;
		}

		public bool Poll_Find_Device()
		{
			m_Respuesta = false;
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

		private void Purge(int len)
		{
			inici = ((inici + len) & (MIDA - 1));
			bytes -= len;
			if (bytes < 0)
			{
				bytes = 0;
				inici = 0;
				final = 0;
			}
		}

		public void Reset()
		{
			Command(new byte[1]
			{
				1
			});
			poll_mask = 128;
			byte[] data = Build_Command(new byte[1]
			{
				17
			});
			comm.Write(data);
			Wait_Resposta();
		}

		public void Decode()
		{
			bool flag = false;
			int num = 0;
			while (bytes >= 6 && !flag)
			{
				int num2 = 0;
				byte[] array = CopyFlat(3);
				if (array[0] == 127 && array[1] == (byte)(Device_ID | (poll_mask ^ 0x80)))
				{
					byte b = (byte)(5 + array[2]);
					if (bytes >= b)
					{
						byte[] array2 = CopyFlat(b);
						if (IsCRCValid(array2))
						{
							num2 = 1;
							switch (array2[3])
							{
							case 242:
							case 243:
							case 244:
							case 245:
							case 246:
							case 250:
								Error = array2[3];
								m_Respuesta = true;
								break;
							case 248:
							{
								Error = array2[3];
								byte b2 = last_cmd;
								if (b2 == 6)
								{
									version = false;
								}
								break;
							}
							case 240:
								m_Respuesta = true;
								if (b >= 7)
								{
									switch (array2[4])
									{
									case 204:
									case 235:
										if (array2[5] == 238)
										{
											Creditos = Canal[array2[6] - 1] * Multiplier * Base;
										}
										break;
									case 239:
										if (array2[6] == 238)
										{
											Creditos = Canal[array2[6] - 1] * Multiplier * Base;
										}
										break;
									case 238:
										Creditos = Canal[array2[5] - 1] * Multiplier * Base;
										break;
									case 232:
										num = 1;
										break;
									}
								}
								switch (last_cmd)
								{
								case 5:
								{
									byte[] destinationArray = new byte[4];
									Array.Copy(array2, 5, destinationArray, 0, 4);
									Firmware = Encoding.UTF8.GetString(destinationArray);
									byte[] destinationArray2 = new byte[3];
									Array.Copy(array2, 9, destinationArray2, 0, 3);
									Currency = Encoding.UTF8.GetString(destinationArray2);
									Multiplier = array2[12] * 65536 + array2[13] * 256 + array2[14];
									Canales = array2[15];
									int num4 = 16;
									for (int i = 0; i < MaxCanales; i++)
									{
										Canal[i] = 0;
										eCanal[i] = 0;
										iCanal[i] = 0;
									}
									for (int i = 0; i < Canales; i++)
									{
										Canal[i] = array2[num4 + i];
										eCanal[i] = array2[num4 + i + Canales];
										iCanal[i] = ((eCanal[i] != 4) ? 1 : 0);
									}
									ProtocolVersion = array2[num4 + Canales * 2 + 3];
									break;
								}
								case 12:
								{
									int num3 = array2[4] * 16777216 + array2[5] * 65536 + array2[6] * 256 + array2[7];
									SerialNumber = string.Concat(num3);
									break;
								}
								}
								break;
							}
							Purge(b);
						}
					}
					if (num == 0)
					{
						TimeOutComs = 0;
					}
				}
				if (num2 == 0)
				{
					Purge(1);
				}
			}
		}

		public bool Parser()
		{
			TimeOutComs++;
			if (comm != null)
			{
				int num = comm.Bytes;
				if (num > 0)
				{
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
				return true;
			}
			return false;
		}

		public bool Poll()
		{
			if (comm != null)
			{
				Parser();
				byte[] data = Build_Command(new byte[1]
				{
					7
				});
				comm.Write(data);
				if (TimeOutComs > 100)
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public bool Inhibits()
		{
			if (comm != null)
			{
				byte b = 1;
				byte b2 = 0;
				int num = 0;
				while (num < 8)
				{
					if (!GetChannelInhibit(num + 1))
					{
						b2 = (byte)(b2 | b);
					}
					num++;
					b = (byte)(b << 1);
				}
				b = 1;
				byte b3 = 0;
				num = 8;
				while (num < 16)
				{
					if (!GetChannelInhibit(num + 1))
					{
						b3 = (byte)(b3 | b);
					}
					num++;
					b = (byte)(b << 1);
				}
				Command(new byte[3]
				{
					2,
					b2,
					b3
				});
				return true;
			}
			return false;
		}

		public bool Enable()
		{
			bool result = Inhibits();
			Command(new byte[1]
			{
				10
			});
			return result;
		}

		public void Command(byte[] _cmd)
		{
			comm.Write(Build_Command(_cmd));
			Wait_Resposta();
		}

		public bool Disable()
		{
			if (comm != null)
			{
				Command(new byte[3]
				{
					2,
					0,
					0
				});
				return true;
			}
			return false;
		}
	}
}
