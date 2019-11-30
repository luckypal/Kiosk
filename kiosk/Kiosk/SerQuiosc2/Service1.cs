// Decompiled with JetBrains decompiler
// Type: Kiosk.SerQuiosc2.Service1
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace Kiosk.SerQuiosc2
{
  [DesignerCategory("code")]
  [GeneratedCode("System.Web.Services", "4.6.1586.0")]
  [DebuggerStepThrough]
  [WebServiceBinding(Name = "Service1Soap", Namespace = "http://tempuri.org/")]
  public class Service1 : SoapHttpClientProtocol
  {
    private SendOrPostCallback VersionOperationCompleted;
    private SendOrPostCallback SaveFileOperationCompleted;
    private SendOrPostCallback loadFileOperationCompleted;
    private bool useDefaultCredentialsSetExplicitly;

    public Service1(string _url)
    {
      this.Url = _url;
      if (this.IsLocalFileSystemWebService(this.Url))
      {
        this.UseDefaultCredentials = true;
        this.useDefaultCredentialsSetExplicitly = false;
      }
      else
        this.useDefaultCredentialsSetExplicitly = true;
    }

    public new string Url
    {
      get
      {
        return base.Url;
      }
      set
      {
        if (this.IsLocalFileSystemWebService(base.Url) && !this.useDefaultCredentialsSetExplicitly && !this.IsLocalFileSystemWebService(value))
          base.UseDefaultCredentials = false;
        base.Url = value;
      }
    }

    public new bool UseDefaultCredentials
    {
      get
      {
        return base.UseDefaultCredentials;
      }
      set
      {
        base.UseDefaultCredentials = value;
        this.useDefaultCredentialsSetExplicitly = true;
      }
    }

    public event VersionCompletedEventHandler VersionCompleted;

    public event SaveFileCompletedEventHandler SaveFileCompleted;

    public event loadFileCompletedEventHandler loadFileCompleted;

    [SoapDocumentMethod("http://tempuri.org/Version", ParameterStyle = SoapParameterStyle.Wrapped, RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = SoapBindingUse.Literal)]
    public string Version()
    {
      return (string) this.Invoke(nameof (Version), new object[0])[0];
    }

    public void VersionAsync()
    {
      this.VersionAsync((object) null);
    }

    public void VersionAsync(object userState)
    {
      if (this.VersionOperationCompleted == null)
        this.VersionOperationCompleted = new SendOrPostCallback(this.OnVersionOperationCompleted);
      this.InvokeAsync("Version", new object[0], this.VersionOperationCompleted, userState);
    }

    private void OnVersionOperationCompleted(object arg)
    {
      if (this.VersionCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.VersionCompleted((object) this, new VersionCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://tempuri.org/SaveFile", ParameterStyle = SoapParameterStyle.Wrapped, RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = SoapBindingUse.Literal)]
    public bool SaveFile(string fileID, string name, string UserName, [XmlElement(DataType = "base64Binary")] byte[] arraytoinsert)
    {
      return (bool) this.Invoke(nameof (SaveFile), new object[4]
      {
        (object) fileID,
        (object) name,
        (object) UserName,
        (object) arraytoinsert
      })[0];
    }

    public void SaveFileAsync(string fileID, string name, string UserName, byte[] arraytoinsert)
    {
      this.SaveFileAsync(fileID, name, UserName, arraytoinsert, (object) null);
    }

    public void SaveFileAsync(
      string fileID,
      string name,
      string UserName,
      byte[] arraytoinsert,
      object userState)
    {
      if (this.SaveFileOperationCompleted == null)
        this.SaveFileOperationCompleted = new SendOrPostCallback(this.OnSaveFileOperationCompleted);
      this.InvokeAsync("SaveFile", new object[4]
      {
        (object) fileID,
        (object) name,
        (object) UserName,
        (object) arraytoinsert
      }, this.SaveFileOperationCompleted, userState);
    }

    private void OnSaveFileOperationCompleted(object arg)
    {
      if (this.SaveFileCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.SaveFileCompleted((object) this, new SaveFileCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://tempuri.org/loadFile", ParameterStyle = SoapParameterStyle.Wrapped, RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = SoapBindingUse.Literal)]
    [return: XmlElement(DataType = "base64Binary")]
    public byte[] loadFile(string fileID, string name)
    {
      return (byte[]) this.Invoke(nameof (loadFile), new object[2]
      {
        (object) fileID,
        (object) name
      })[0];
    }

    public void loadFileAsync(string fileID, string name)
    {
      this.loadFileAsync(fileID, name, (object) null);
    }

    public void loadFileAsync(string fileID, string name, object userState)
    {
      if (this.loadFileOperationCompleted == null)
        this.loadFileOperationCompleted = new SendOrPostCallback(this.OnloadFileOperationCompleted);
      this.InvokeAsync("loadFile", new object[2]
      {
        (object) fileID,
        (object) name
      }, this.loadFileOperationCompleted, userState);
    }

    private void OnloadFileOperationCompleted(object arg)
    {
      if (this.loadFileCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.loadFileCompleted((object) this, new loadFileCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    public new void CancelAsync(object userState)
    {
      base.CancelAsync(userState);
    }

    private bool IsLocalFileSystemWebService(string url)
    {
      if (url == null || url == string.Empty)
        return false;
      Uri uri = new Uri(url);
      return uri.Port >= 1024 && string.Compare(uri.Host, "localHost", StringComparison.OrdinalIgnoreCase) == 0;
    }
  }
}
