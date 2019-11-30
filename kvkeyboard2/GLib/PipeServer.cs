using System;
using System.IO.Pipes;
using System.Text;

namespace GLib
{
	internal class PipeServer
	{
		private string _pipeName;

		public event DelegateMessage PipeMessage;

		public void Listen(string PipeName)
		{
			try
			{
				_pipeName = PipeName;
				NamedPipeServerStream namedPipeServerStream = new NamedPipeServerStream(PipeName, PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
				namedPipeServerStream.BeginWaitForConnection(WaitForConnectionCallBack, namedPipeServerStream);
			}
			catch (Exception)
			{
			}
		}

		private void WaitForConnectionCallBack(IAsyncResult iar)
		{
			try
			{
				NamedPipeServerStream namedPipeServerStream = (NamedPipeServerStream)iar.AsyncState;
				namedPipeServerStream.EndWaitForConnection(iar);
				byte[] array = new byte[255];
				namedPipeServerStream.Read(array, 0, 255);
				string @string = Encoding.UTF8.GetString(array, 0, array.Length);
				this.PipeMessage(@string);
				namedPipeServerStream.Close();
				namedPipeServerStream = null;
				namedPipeServerStream = new NamedPipeServerStream(_pipeName, PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
				namedPipeServerStream.BeginWaitForConnection(WaitForConnectionCallBack, namedPipeServerStream);
			}
			catch
			{
			}
		}
	}
}
