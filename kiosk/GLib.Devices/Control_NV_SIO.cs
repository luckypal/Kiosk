using System.ComponentModel;
using System.IO.Ports;
using System.Threading;

namespace GLib.Devices
{
	public class Control_NV_SIO
	{
		private string[] _f_ports;

		public string _f_resp_scom;

		public bool OnLine;

		public CommPort comm;

		public string port;

		public int[] Canal;

		public int[] eCanal;

		public int Canales = 16;

		public decimal Base = 100m;

		public int CriticalJamp;

		private int inici;

		private int final;

		private int bytes;

		private byte[] buffer;

		private static int MIDA = 16384;

		private int m_Creditos;

		private bool respuesta;

		private bool test = false;

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

		public Control_NV_SIO()
		{
			comm = null;
			inici = 0;
			final = 0;
			bytes = 0;
			CriticalJamp = 0;
			respuesta = false;
			buffer = new byte[MIDA];
			port = "COM1";
			_f_resp_scom = "-";
			Creditos = 0;
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
			Canal[0] = 20;
			Canal[1] = 50;
			Canal[2] = 100;
			Canal[3] = 200;
			Canal[4] = 500;
			Canal[5] = 1000;
			Canal[6] = 2000;
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
			comm.Open(port, 300, Parity.None, 8, StopBits.Two);
			OnLine = true;
			CriticalJamp = 0;
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
			string text = "-";
			_f_resp_scom = "-";
			comm = new CommPort();
			for (int i = 1; i < 16; i++)
			{
				text = "COM" + i;
				test = true;
				try
				{
					comm.Open(text, 300, Parity.None, 8, StopBits.Two);
					respuesta = false;
					for (int j = 0; j < 5; j++)
					{
						Command(182);
						Thread.Sleep(200);
						Parser();
						if (respuesta)
						{
							comm.Close();
							_f_resp_scom = text;
							test = false;
							return text;
						}
						Purge(bytes);
					}
					comm.Close();
				}
				catch
				{
					text = "-";
				}
			}
			test = false;
			return "-";
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
			while (bytes >= 1 && !flag)
			{
				if (test)
				{
					byte[] array = CopyFlat(bytes);
					if ((array[0] == 182 && bytes == 1) || (array[1] == 0 && bytes == 2) || (array[2] == 0 && bytes == 3))
					{
						break;
					}
					if (array[0] == 182 && array[1] == 0 && array[2] == 0 && array[3] == 0 && bytes >= 4)
					{
						respuesta = true;
					}
				}
				byte[] array2 = CopyFlat(1);
				switch (array2[0])
				{
				case byte.MaxValue:
					respuesta = true;
					break;
				case 121:
					respuesta = true;
					break;
				case 120:
					respuesta = true;
					break;
				case 80:
					respuesta = true;
					break;
				case 70:
					respuesta = true;
					break;
				case 60:
					respuesta = true;
					break;
				case 50:
					respuesta = true;
					break;
				case 40:
					respuesta = true;
					break;
				case 30:
					respuesta = true;
					break;
				case 20:
					respuesta = true;
					break;
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
				case 12:
				case 13:
				case 14:
				case 15:
				case 16:
				{
					int num = array2[0] - 1;
					if (Canal[num] > 0)
					{
						m_Creditos = Canal[num];
					}
					if (Canal[num] <= 0)
					{
						if (CriticalJamp == 0)
						{
							CriticalJamp = 1;
						}
						m_Creditos = 0;
					}
					break;
				}
				}
				Purge(1);
			}
		}

		public bool Parser()
		{
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
				return true;
			}
			return false;
		}

		public bool Command(byte _val)
		{
			if (comm != null)
			{
				byte[] data = new byte[1]
				{
					_val
				};
				comm.Write(data);
				return true;
			}
			return false;
		}

		public bool Enable()
		{
			Command(184);
			if (comm != null)
			{
				for (int i = 0; i < Canal.Length; i++)
				{
					Thread.Sleep(40);
					Poll();
					Parser();
					if (Canal[i] <= 0)
					{
						Command((byte)(131 + i));
					}
					else
					{
						Command((byte)(151 + i));
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
				return Command(185);
			}
			return false;
		}
	}
}
