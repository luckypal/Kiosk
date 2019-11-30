// Decompiled with JetBrains decompiler
// Type: GLib.PipeClient
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

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
        NamedPipeClientStream pipeClientStream = new NamedPipeClientStream(".", PipeName, PipeDirection.Out, PipeOptions.Asynchronous);
        pipeClientStream.Connect(TimeOut);
        byte[] bytes = Encoding.UTF8.GetBytes(SendStr);
        pipeClientStream.BeginWrite(bytes, 0, bytes.Length, new AsyncCallback(this.AsyncSend), (object) pipeClientStream);
      }
      catch
      {
      }
    }

    private void AsyncSend(IAsyncResult iar)
    {
      try
      {
        NamedPipeClientStream asyncState = (NamedPipeClientStream) iar.AsyncState;
        asyncState.EndWrite(iar);
        asyncState.Flush();
        asyncState.Close();
        asyncState.Dispose();
      }
      catch (Exception ex)
      {
      }
    }
  }
}
