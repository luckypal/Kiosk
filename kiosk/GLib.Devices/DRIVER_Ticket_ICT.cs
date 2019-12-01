using System;
using System.IO.Ports;
using System.Threading;

namespace GLib.Devices
{
	public class DRIVER_Ticket_ICT
	{
		private const int BLOCK = 6;

		public byte ID;

		public bool OnLine;

		public bool Ready;

		public bool Fail;

		public bool Check;

		public CommPort comm;

		public string port;

		public byte Tickets;

		public int Tickets_Out;

		public int Tickets_Fail;

		public int Tickets_Send;

		public int Tickets_Total;

		public int CntCmd;

		public bool EnError;

		private volatile int inici;

		private volatile int final;

		private volatile int bytes;

		private volatile byte[] buffer;

		private static int MIDA = 16384;

		private volatile bool respuesta;

		private int CntError = 0;

		private int UtimCnt = 0;

		private int UtimCntOK = 0;

		private DateTime Check_Timeout;

		public DRIVER_Ticket_ICT()
		{
			comm = null;
			ID = 0;
			inici = 0;
			final = 0;
			bytes = 0;
			CntCmd = 0;
			respuesta = false;
			Fail = false;
			Ready = false;
			Check = false;
			EnError = false;
			buffer = new byte[MIDA];
			port = "COM1";
			Tickets_Total = 0;
			Tickets = 0;
			Tickets_Out = 0;
			Tickets_Fail = 0;
			Tickets_Send = 0;
			Reset_Timeout();
		}

		public bool Open()
		{
			if (comm != null)
			{
				comm.Close();
			}
			OnLine = false;
			comm = null;
			if (port == "-" || port == "" || port == "?" || port == null)
			{
				return false;
			}
			string[] portNames = SerialPort.GetPortNames();
			int num = 0;
			if (portNames != null)
			{
				for (int i = 0; i < portNames.Length; i++)
				{
					if (portNames[i].ToLower() == port.ToLower())
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
			comm.Open(port, 9600, Parity.Even, 8, StopBits.One);
			OnLine = true;
			Fail = false;
			Ready = false;
			EnError = false;
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
			byte b = 0;
			while (bytes >= 6 && !flag)
			{
				byte[] _pck = CopyFlat(6);
				if (_pck[0] == 1 && _pck[1] == 1)
				{
					if (Checksum(ref _pck))
					{
						respuesta = true;
						CntCmd++;
						switch (_pck[3])
						{
						case 170:
							UtimCnt = (UtimCntOK = _pck[4]);
							CntError = 0;
							Tickets_Out++;
							Tickets--;
							Tickets_Send--;
							if (Tickets_Send <= 0)
							{
								Tickets_Send = 0;
								Fail = false;
								Ready = true;
								Check = true;
							}
							EnError = false;
							break;
						case 187:
							UtimCnt = _pck[4];
							if (UtimCnt == 0)
							{
								UtimCntOK = 0;
							}
							CntError = 0;
							Tickets_Fail++;
							Ready = false;
							Fail = true;
							Check = false;
							EnError = true;
							break;
						case 0:
						case 2:
						case 10:
							UtimCnt = _pck[4];
							UtimCntOK = _pck[4];
							Fail = false;
							Ready = true;
							Check = true;
							CntError = 0;
							break;
						case 9:
							UtimCnt = _pck[4];
							if (UtimCnt == 0)
							{
								UtimCntOK = 0;
							}
							CntError++;
							if (CntError > 10)
							{
								CntError = 0;
								Reset();
							}
							break;
						default:
							UtimCnt = _pck[4];
							if (UtimCnt == 0)
							{
								UtimCntOK = 0;
							}
							CntError = 0;
							Check = false;
							Ready = false;
							Fail = true;
							EnError = true;
							break;
						}
						Purge(6);
						CntCmd++;
					}
					else
					{
						Purge(1);
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

		private bool Checksum(ref byte[] _pck)
		{
			byte b = 0;
			try
			{
				for (int i = 0; i < 5; i++)
				{
					b = (byte)(b + _pck[i]);
				}
				if (_pck[5] == b)
				{
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		private byte[] CMD_Payout(byte _cnt)
		{
			byte[] array = new byte[6];
			array[0] = 1;
			array[1] = 16;
			array[2] = ID;
			array[3] = 16;
			array[4] = _cnt;
			array[5] = (byte)(array[0] + array[1] + array[2] + array[3] + array[4]);
			return array;
		}

		public bool Payout(byte val)
		{
			if (comm == null)
			{
				return false;
			}
			if (!OnLine)
			{
				Open();
			}
			Tickets += val;
			Tickets_Total += val;
			return true;
		}

		public bool Reset()
		{
			if (comm == null)
			{
				return false;
			}
			if (!OnLine)
			{
				Open();
			}
			if (!OnLine)
			{
				return false;
			}
			byte[] array = new byte[6];
			array[0] = 1;
			array[1] = 16;
			array[2] = ID;
			array[3] = 18;
			array[4] = 0;
			array[5] = (byte)(array[0] + array[1] + array[2] + array[3] + array[4]);
			EnError = false;
			Fail = false;
			Ready = false;
			Check = false;
			comm.Write(array);
			return true;
		}

		public bool Check_Status()
		{
			Reset_Timeout();
			if (comm == null)
			{
				return false;
			}
			if (!OnLine)
			{
				Open();
			}
			if (!OnLine)
			{
				return false;
			}
			byte[] array = new byte[6];
			array[0] = 1;
			array[1] = 16;
			array[2] = ID;
			array[3] = 17;
			array[4] = 0;
			array[5] = (byte)(array[0] + array[1] + array[2] + array[3] + array[4]);
			Fail = false;
			Ready = false;
			Check = false;
			comm.Write(array);
			Reset_Timeout();
			return true;
		}

		private void Reset_Timeout()
		{
			Check_Timeout = DateTime.Now;
		}

		public bool Poll()
		{
			if (comm != null)
			{
				Parser();
				if (Tickets > 0)
				{
					int num = (int)(DateTime.Now - Check_Timeout).TotalMilliseconds;
					if ((!Check || (Check && !Ready && !Fail)) && num > 2000)
					{
						if (UtimCnt == UtimCntOK)
						{
							Check_Status();
						}
						Check_Timeout = DateTime.Now;
						CntCmd++;
					}
				}
				if (Ready && !Fail && Check && Tickets > 0)
				{
					Ready = false;
					byte b = 5;
					if (b > Tickets)
					{
						b = Tickets;
					}
					Tickets_Send = b;
					CntCmd++;
					byte[] data = CMD_Payout(b);
					comm.Write(data);
				}
				return true;
			}
			return false;
		}

		public string Find_Device()
		{
			string text = port;
			string[] portNames = SerialPort.GetPortNames();
			if (comm != null)
			{
				comm.Close();
				comm = null;
			}
			if (portNames == null)
			{
				respuesta = false;
				return "-";
			}
			for (int i = 0; i < portNames.Length; i++)
			{
				try
				{
					respuesta = false;
					port = portNames[i];
					try
					{
						SerialPort serialPort = new SerialPort(port);
						if (serialPort.IsOpen)
						{
							continue;
						}
					}
					catch
					{
					}
					Open();
					Thread.Sleep(100);
					Reset();
					Thread.Sleep(100);
					Poll();
					Check_Status();
					Thread.Sleep(100);
					Poll();
					Close();
					if (respuesta)
					{
						return portNames[i];
					}
				}
				catch
				{
				}
			}
			port = text;
			return "-";
		}
	}
}
