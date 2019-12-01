using System;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace GLib.Devices
{
	public class CommPort
	{
		public static int buffer_size = 8192;

		public int buffer_inici;

		public int buffer_final;

		public int buffer_bytes;

		public byte[] buffer;

		private bool _keepReading;

		private SerialPort _serialPort;

		private Thread _readThread;

		public int Bytes => buffer_bytes;

		public bool IsOpen => _serialPort.IsOpen;

		public CommPort()
		{
			_serialPort = new SerialPort();
			_serialPort.ReadBufferSize = buffer_size;
			_readThread = null;
			_keepReading = false;
			buffer = new byte[buffer_size];
			buffer_inici = (buffer_final = (buffer_bytes = 0));
		}

		private bool StartUpReading()
		{
			if (!_keepReading)
			{
				_keepReading = true;
				_readThread = new Thread(ReadPort);
				_readThread.Start();
				return true;
			}
			return false;
		}

		private bool ShutDownReading()
		{
			if (_keepReading)
			{
				_keepReading = false;
				_readThread.Join();
				_readThread = null;
				return true;
			}
			return false;
		}

		private void ReadPort()
		{
			while (_keepReading)
			{
				if (_serialPort.IsOpen)
				{
					byte[] array = new byte[_serialPort.ReadBufferSize + 1];
					try
					{
						int num = 0;
						try
						{
							num = _serialPort.Read(array, 0, _serialPort.ReadBufferSize);
						}
						catch
						{
						}
						string @string = Encoding.ASCII.GetString(array, 0, num);
						for (int i = 0; i < num; i++)
						{
							buffer[buffer_final] = array[i];
							buffer_final++;
							buffer_final &= buffer_size - 1;
							buffer_bytes++;
							if (buffer_bytes > buffer_size)
							{
								buffer_inici = buffer_final;
								buffer_bytes--;
							}
						}
					}
					catch (TimeoutException)
					{
					}
				}
				else
				{
					TimeSpan timeout = new TimeSpan(0, 0, 0, 0, 50);
					Thread.Sleep(timeout);
				}
			}
		}

		public byte[] Read(int mida)
		{
			byte[] array;
			if (IsOpen)
			{
				array = new byte[mida];
				for (int i = 0; i < mida; i++)
				{
					array[i] = buffer[buffer_inici];
					buffer_inici++;
					buffer_inici &= buffer_size - 1;
					buffer_bytes--;
					if (buffer_bytes <= 0)
					{
						buffer_bytes = (buffer_inici = (buffer_final = 0));
					}
				}
			}
			else
			{
				array = new byte[0];
				buffer_bytes = (buffer_inici = (buffer_final = 0));
			}
			return array;
		}

		public byte Read()
		{
			byte result = 0;
			if (IsOpen)
			{
				result = buffer[buffer_inici];
				buffer_inici++;
				buffer_inici &= buffer_size - 1;
				buffer_bytes--;
				if (buffer_bytes < 0)
				{
					buffer_bytes = (buffer_inici = (buffer_final = 0));
				}
			}
			return result;
		}

		public bool Open(string port, int baudrate, Parity parity, int databits, StopBits stopbits)
		{
			if (IsOpen)
			{
				Close();
			}
			if (port == "-")
			{
				return false;
			}
			try
			{
				_serialPort.PortName = port;
				_serialPort.BaudRate = baudrate;
				_serialPort.Parity = parity;
				_serialPort.DataBits = databits;
				_serialPort.Handshake = Handshake.None;
				_serialPort.ReadTimeout = 50;
				_serialPort.WriteTimeout = 50;
				_serialPort.Open();
				StartUpReading();
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			if (_serialPort.IsOpen)
			{
				return true;
			}
			return false;
		}

		public void Close()
		{
			ShutDownReading();
			try
			{
				_serialPort.Close();
			}
			catch
			{
			}
		}

		public string[] Puertos()
		{
			return SerialPort.GetPortNames();
		}

		public int PorEnviar()
		{
			return _serialPort.BytesToWrite;
		}

		public bool Write(byte[] data)
		{
			if (IsOpen)
			{
				try
				{
					_serialPort.Write(data, 0, data.Length);
					return true;
				}
				catch
				{
					return false;
				}
			}
			return false;
		}

		public bool Write(string data)
		{
			if (IsOpen)
			{
				ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
				byte[] bytes = aSCIIEncoding.GetBytes(data);
				try
				{
					_serialPort.Write(bytes, 0, bytes.Length);
					return true;
				}
				catch
				{
				}
			}
			return false;
		}
	}
}
