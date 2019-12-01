using System.ComponentModel;
using System.IO.Ports;
using System.Threading;

namespace GLib.Devices
{
	public class Control_Comestero
	{
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

		public int Canales = 8;

		public decimal Base = 100m;

		public int SwapControl = 0;

		public int RM5_Command = -1;

		public int RM5_OK = 0;

		private int inici;

		private int final;

		private int bytes;

		private byte[] buffer;

		private static int MIDA = 16384;

		private int m_Creditos;

		private bool respuesta;

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

		public Control_Comestero()
		{
			comm = null;
			inici = 0;
			final = 0;
			bytes = 0;
			respuesta = false;
			Base = 100m;
			buffer = new byte[MIDA];
			port = "COM2";
			Creditos = 0;
			Canal = new int[Canales];
			eCanal = new int[Canales];
			Set_Euro();
			SwapControl = 0;
			RM5_Command = -1;
			RM5_OK = 0;
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
			Canal[0] = 50;
			Canal[1] = 100;
			Canal[2] = 200;
		}

		public void Set_Brazil()
		{
			for (int i = 0; i < Canal.Length; i++)
			{
				Canal[i] = 0;
				eCanal[i] = 0;
			}
			eCanal[0] = 1;
			Canal[0] = 1;
		}

		public bool Open()
		{
			for (int i = 0; i < 4; i++)
			{
				if (comm != null)
				{
					try
					{
						comm.Close();
					}
					catch
					{
					}
				}
				OnLine = false;
				comm = null;
				RM5_OK = 0;
				RM5_Command = -1;
				SwapControl = 0;
				_f_ports = SerialPort.GetPortNames();
				int num = 0;
				if (_f_ports != null)
				{
					for (int j = 0; j < _f_ports.Length; j++)
					{
						if (_f_ports[j].ToLower() == port.ToLower())
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
				comm.Open(port, 9600, Parity.None, 8, StopBits.One);
				if (Init_Moneder())
				{
					OnLine = true;
					Enable();
					return true;
				}
				Thread.Sleep(1000);
			}
			if (comm != null)
			{
				comm.Close();
			}
			comm = null;
			return false;
		}

		private bool Init_Moneder()
		{
			if (comm == null)
			{
				return false;
			}
			RM5_OK = 0;
			byte[] array = new byte[1];
			byte[] data = array;
			byte[] data2 = new byte[2]
			{
				0,
				1
			};
			byte[] data3 = new byte[4]
			{
				0,
				1,
				0,
				10
			};
			RM5_Command = 0;
			comm.Write(data);
			Thread.Sleep(100);
			Parser();
			RM5_Command = 1;
			comm.Write(data2);
			Thread.Sleep(100);
			Parser();
			RM5_Command = 0;
			comm.Write(data);
			Thread.Sleep(100);
			Parser();
			RM5_Command = 10;
			comm.Write(data3);
			Thread.Sleep(100);
			Parser();
			RM5_Command = 10;
			comm.Write(data3);
			Thread.Sleep(100);
			Parser();
			RM5_Command = 10;
			comm.Write(data3);
			Thread.Sleep(100);
			Parser();
			RM5_Command = 0;
			comm.Write(data);
			Thread.Sleep(100);
			Parser();
			RM5_Command = 0;
			comm.Write(data);
			Thread.Sleep(100);
			Parser();
			if (RM5_OK == 1)
			{
				return true;
			}
			return false;
		}

		public bool Close()
		{
			if (comm != null)
			{
				comm.Close();
			}
			OnLine = false;
			return true;
		}

		public void Start_Find_Device()
		{
			_f_resp_scom = "-";
			_f_ports = SerialPort.GetPortNames();
			_f_cnt = 0;
			_f_try = 0;
			Next_Find_Device();
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
				respuesta = false;
				_f_try = 0;
				_f_resp_scom = "-";
				_f_com = _f_ports[_f_cnt];
				comm = new CommPort();
				comm.Open(_f_com, 9600, Parity.None, 8, StopBits.One);
				_f_cnt++;
				return false;
			}
			_f_resp_scom = "-";
			return true;
		}

		public bool Test_Device()
		{
			if (comm != null)
			{
				comm.Close();
				comm = null;
			}
			_f_ports = SerialPort.GetPortNames();
			respuesta = false;
			_f_try = 0;
			_f_resp_scom = "-";
			comm = new CommPort();
			comm.Open(_f_com, 9600, Parity.None, 8, StopBits.One);
			_f_cnt++;
			return Poll_Find_Device();
		}

		public bool Poll_Find_Device()
		{
			respuesta = false;
			if (Init_Moneder())
			{
				respuesta = true;
				if (comm != null)
				{
					comm.Close();
					comm = null;
				}
				_f_resp_scom = _f_com;
				return true;
			}
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

		public void Decode()
		{
			bool flag = false;
			if (RM5_Command == 29)
			{
				while (bytes >= 2 && !flag)
				{
					byte[] array = CopyFlat(2);
					if (array[1] == 128)
					{
						break;
					}
					int num = array[1] + (array[0] & 0x3F) * 256;
					switch ((array[0] & 0xC0) >> 6)
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
					{
						num = 0;
					}
					if (num > 0)
					{
						m_Creditos = num;
					}
					Purge(2);
				}
				return;
			}
			while (bytes >= 1 && !flag)
			{
				byte[] array = CopyFlat(bytes);
				switch (RM5_Command)
				{
				case -1:
					Purge(1);
					break;
				case 0:
					if (array[0] == 0)
					{
						RM5_Command = -1;
					}
					Purge(1);
					break;
				case 1:
					if (bytes >= 2 && array[0] == 0 && array[1] == 0)
					{
						RM5_Command = -1;
						Purge(1);
					}
					Purge(1);
					break;
				case 10:
					if (bytes >= 5)
					{
						if (array[0] == 0 && array[1] == 0 && (array[2] & 0xF) == 5)
						{
							Purge(4);
							RM5_OK = 1;
							RM5_Command = -1;
						}
						if (array[0] == 0 && array[1] == 0 && (array[2] & 0xF) == 6)
						{
							Purge(4);
							RM5_OK = 2;
							RM5_Command = -1;
						}
					}
					Purge(1);
					break;
				}
			}
		}

		public bool Parser()
		{
			if (comm != null)
			{
				int num = 0;
				try
				{
					num = comm.Bytes;
				}
				catch
				{
				}
				if (num > 0)
				{
					byte[] array = new byte[0];
					try
					{
						array = comm.Read(num);
					}
					catch
					{
					}
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
				return Command(29);
			}
			return false;
		}

		public bool Poll_Null()
		{
			if (comm != null)
			{
				return Command(1);
			}
			return false;
		}

		public bool Command(byte _val)
		{
			if (comm != null)
			{
				byte[] data = new byte[2]
				{
					0,
					_val
				};
				try
				{
					comm.Write(data);
					return true;
				}
				catch
				{
					return false;
				}
			}
			return false;
		}

		public bool Enable()
		{
			if (comm != null)
			{
				RM5_Command = 29;
				SwapControl = 0;
				return true;
			}
			return false;
		}

		public bool Disable()
		{
			if (comm != null)
			{
				RM5_Command = -1;
				SwapControl = 0;
				return true;
			}
			return false;
		}
	}
}
