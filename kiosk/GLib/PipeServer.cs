// Decompiled with JetBrains decompiler
// Type: GLib.PipeServer
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.Diagnostics;
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
        this._pipeName = PipeName;
        NamedPipeServerStream pipeServerStream = new NamedPipeServerStream(PipeName, PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
        pipeServerStream.BeginWaitForConnection(new AsyncCallback(this.WaitForConnectionCallBack), (object) pipeServerStream);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
      }
    }

    private void WaitForConnectionCallBack(IAsyncResult iar)
    {
      try
      {
        NamedPipeServerStream asyncState = (NamedPipeServerStream) iar.AsyncState;
        asyncState.EndWaitForConnection(iar);
        byte[] numArray = new byte[(int) byte.MaxValue];
        asyncState.Read(numArray, 0, (int) byte.MaxValue);
        string Reply = Encoding.UTF8.GetString(numArray, 0, numArray.Length);
        Debug.WriteLine(Reply + Environment.NewLine);
        this.PipeMessage(Reply);
        asyncState.Close();
        NamedPipeServerStream pipeServerStream = new NamedPipeServerStream(this._pipeName, PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
        pipeServerStream.BeginWaitForConnection(new AsyncCallback(this.WaitForConnectionCallBack), (object) pipeServerStream);
      }
      catch
      {
      }
    }
  }
}
