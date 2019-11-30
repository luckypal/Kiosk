// Decompiled with JetBrains decompiler
// Type: Kiosk.SerQuiosc2.loadFileCompletedEventArgs
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace Kiosk.SerQuiosc2
{
  [GeneratedCode("System.Web.Services", "4.6.1586.0")]
  [DesignerCategory("code")]
  [DebuggerStepThrough]
  public class loadFileCompletedEventArgs : AsyncCompletedEventArgs
  {
    private object[] results;

    internal loadFileCompletedEventArgs(
      object[] results,
      Exception exception,
      bool cancelled,
      object userState)
      : base(exception, cancelled, userState)
    {
      this.results = results;
    }

    public byte[] Result
    {
      get
      {
        this.RaiseExceptionIfNecessary();
        return (byte[]) this.results[0];
      }
    }
  }
}
