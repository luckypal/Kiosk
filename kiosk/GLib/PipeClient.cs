using System;
using System.IO.Pipes;
using System.Text;

namespace GLib
{
	internal class PipeClient
	{
		public void Send(string SendStr, string PipeName, int TimeOut = 1000)
		{
			try
			{
				NamedPipeClientStream namedPipeClientStream = new NamedPipeClientStream(".", PipeName, PipeDirection.Out, PipeOptions.Asynchronous);
				namedPipeClientStream.Connect(TimeOut);
				byte[] bytes = Encoding.UTF8.GetBytes(SendStr);
				namedPipeClientStream.BeginWrite(bytes, 0, bytes.Length, AsyncSend, namedPipeClientStream);
			}
			catch
			{
			}
		}

		private void AsyncSend(IAsyncResult iar)
		{
			try
			{
				NamedPipeClientStream namedPipeClientStream = (NamedPipeClientStream)iar.AsyncState;
				namedPipeClientStream.EndWrite(iar);
				namedPipeClientStream.Flush();
				namedPipeClientStream.Close();
				namedPipeClientStream.Dispose();
			}
			catch (Exception)
			{
			}
		}
	}
}
