using System.ComponentModel;
using System.IO.Ports;
using System.Threading;

namespace GLib.Devices
{
	public class Control_Trilogy
	{
		public decimal Base = 100m;

		public bool OnLine;

		public CommPort comm;

		public string port;

		public int[] Canal;

		public int[] eCanal;

		public int Canales = 8;

		private int inici;

		private int final;

		private int bytes;

		private byte[] buffer;

		private static int MIDA = 16384;

		private int m_Creditos;

		private bool respuesta;

		private byte Canales_On;

		private byte XorPoll;

		public string _f_resp_scom;

		public string _f_com;

		private string[] _f_ports;

		private int _f_cnt;

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

		public Control_Trilogy()
		{
			comm = null;
			inici = 0;
			final = 0;
			bytes = 0;
			Base = 100m;
			respuesta = false;
			buffer = new byte[MIDA];
			port = "COM1";
			Creditos = 0;
			XorPoll = 16;
			Canales_On = 0;
			Canal = new int[Canales];
			eCanal = new int[Canales];
			Set_Euro();
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
			Canal[0] = 500;
			Canal[1] = 1000;
			Canal[2] = 2000;
			Canal[3] = 5000;
			Canal[4] = 10000;
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
			eCanal[6] = 1;
			Canal[0] = 0;
			Canal[1] = 0;
			Canal[2] = 500;
			Canal[3] = 1000;
			Canal[4] = 2000;
			Canal[5] = 5000;
			Canal[6] = 10000;
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

		public bool Open()
		{
			if (comm != null)
			{
				OnLine = false;
				comm.Close();
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
			comm.Open(port, 9600, Parity.Even, 7, StopBits.One);
			OnLine = true;
			Enable();
			return true;
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

		public string Find_Device()
		{
			string[] portNames = SerialPort.GetPortNames();
			if (portNames == null)
			{
				return "-";
			}
			comm = new CommPort();
			string[] array = portNames;
			foreach (string result in array)
			{
				try
				{
					port = result;
					comm.Open(result, 9600, Parity.Even, 7, StopBits.One);
					Disable();
					respuesta = false;
					for (int j = 0; j < 5; j++)
					{
						if (ResetCom(0))
						{
							Poll();
							Thread.Sleep(200);
							Parser();
							if (respuesta)
							{
								_f_resp_scom = port;
								comm.Close();
								return result;
							}
						}
					}
					comm.Close();
				}
				catch
				{
				}
			}
			return "-";
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
			while (bytes >= 4 && !flag)
			{
				byte[] array = CopyFlat(2);
				int num = array[1];
				if (array[0] == 2 && array.Length >= 2 && num >= 2)
				{
					respuesta = true;
					if (bytes >= num)
					{
						byte[] _pck = CopyFlat(num);
						if (ChecksumV(ref _pck))
						{
							if ((_pck[3] & 2) == 2)
							{
							}
							if ((_pck[3] & 0x10) == 16)
							{
								int num2 = (_pck[5] >> 3) & 0x1F;
								if (Canal[num2 - 1] > 0)
								{
									m_Creditos = Canal[num2 - 1];
								}
							}
							Purge(num);
						}
						else
						{
							Purge(1);
						}
					}
					else
					{
						flag = true;
					}
				}
				else
				{
					Purge(1);
				}
			}
		}

		public bool Parser()
		{
			if (comm != null)
			{
				int num = comm.Bytes;
				if (num > 4)
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
				Command(0);
				return true;
			}
			return false;
		}

		private void Checksum(ref byte[] _pck)
		{
			byte b = 0;
			try
			{
				int num = _pck[1];
				for (int i = 1; i < num - 2; i++)
				{
					b = (byte)(b ^ _pck[i]);
				}
				_pck[num - 1] = b;
			}
			catch
			{
			}
		}

		private bool ChecksumV(ref byte[] _pck)
		{
			byte b = 0;
			int num = _pck[1];
			try
			{
				for (int i = 1; i < num - 2; i++)
				{
					b = (byte)(b ^ _pck[i]);
				}
				if (_pck[num - 1] == b)
				{
					return true;
				}
			}
			catch
			{
				return false;
			}
			return false;
		}

		public void Start_Find_Device()
		{
			_f_resp_scom = "-";
			_f_ports = SerialPort.GetPortNames();
			_f_cnt = 0;
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
			if (comm != null)
			{
				comm.Close();
				comm = null;
			}
		}

		public bool Command(byte _val)
		{
			if (comm != null)
			{
				XorPoll ^= 1;
				byte[] _pck = new byte[8]
				{
					2,
					8,
					XorPoll,
					Canales_On,
					0,
					0,
					3,
					103
				};
				Checksum(ref _pck);
				comm.Write(_pck);
				return true;
			}
			return false;
		}

		public bool ResetCom(byte _val)
		{
			if (comm != null)
			{
				XorPoll ^= 1;
				byte[] _pck = new byte[9]
				{
					63,
					63,
					63,
					63,
					63,
					63,
					63,
					63,
					63
				};
				Checksum(ref _pck);
				comm.Write(_pck);
				Thread.Sleep(300);
				int mida = comm.Bytes;
				byte[] array = comm.Read(mida);
				if (array.Length == 9 && array[0] == 63 && array[1] == 63 && array[2] == 63 && array[3] == 63 && array[4] == 63 && array[5] == 63 && array[6] == 63 && array[7] == 63)
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public bool Enable()
		{
			if (comm != null)
			{
				Canales_On = 0;
				for (int i = 0; i < Canal.Length; i++)
				{
					if (Canal[i] > 0)
					{
						Canales_On |= (byte)(1 << i);
					}
				}
				return true;
			}
			return false;
		}

		public bool Disable()
		{
			if (comm != null)
			{
				Canales_On = 0;
				return true;
			}
			return false;
		}
	}
}
