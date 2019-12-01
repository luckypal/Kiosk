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

		public new string Url
		{
			get
			{
				return base.Url;
			}
			set
			{
				if (IsLocalFileSystemWebService(base.Url) && !useDefaultCredentialsSetExplicitly && !IsLocalFileSystemWebService(value))
				{
					base.UseDefaultCredentials = false;
				}
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
				useDefaultCredentialsSetExplicitly = true;
			}
		}

		public event VersionCompletedEventHandler VersionCompleted;

		public event SaveFileCompletedEventHandler SaveFileCompleted;

		public event loadFileCompletedEventHandler loadFileCompleted;

		public Service1(string _url)
		{
			Url = _url;
			if (IsLocalFileSystemWebService(Url))
			{
				UseDefaultCredentials = true;
				useDefaultCredentialsSetExplicitly = false;
			}
			else
			{
				useDefaultCredentialsSetExplicitly = true;
			}
		}

		[SoapDocumentMethod("http://tempuri.org/Version", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public string Version()
		{
			object[] array = Invoke("Version", new object[0]);
			return (string)array[0];
		}

		public void VersionAsync()
		{
			VersionAsync(null);
		}

		public void VersionAsync(object userState)
		{
			if (VersionOperationCompleted == null)
			{
				VersionOperationCompleted = OnVersionOperationCompleted;
			}
			InvokeAsync("Version", new object[0], VersionOperationCompleted, userState);
		}

		private void OnVersionOperationCompleted(object arg)
		{
			if (this.VersionCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.VersionCompleted(this, new VersionCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		[SoapDocumentMethod("http://tempuri.org/SaveFile", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		public bool SaveFile(string fileID, string name, string UserName, [XmlElement(DataType = "base64Binary")] byte[] arraytoinsert)
		{
			object[] array = Invoke("SaveFile", new object[4]
			{
				fileID,
				name,
				UserName,
				arraytoinsert
			});
			return (bool)array[0];
		}

		public void SaveFileAsync(string fileID, string name, string UserName, byte[] arraytoinsert)
		{
			SaveFileAsync(fileID, name, UserName, arraytoinsert, null);
		}

		public void SaveFileAsync(string fileID, string name, string UserName, byte[] arraytoinsert, object userState)
		{
			if (SaveFileOperationCompleted == null)
			{
				SaveFileOperationCompleted = OnSaveFileOperationCompleted;
			}
			InvokeAsync("SaveFile", new object[4]
			{
				fileID,
				name,
				UserName,
				arraytoinsert
			}, SaveFileOperationCompleted, userState);
		}

		private void OnSaveFileOperationCompleted(object arg)
		{
			if (this.SaveFileCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.SaveFileCompleted(this, new SaveFileCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		[SoapDocumentMethod("http://tempuri.org/loadFile", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
		[return: XmlElement(DataType = "base64Binary")]
		public byte[] loadFile(string fileID, string name)
		{
			object[] array = Invoke("loadFile", new object[2]
			{
				fileID,
				name
			});
			return (byte[])array[0];
		}

		public void loadFileAsync(string fileID, string name)
		{
			loadFileAsync(fileID, name, null);
		}

		public void loadFileAsync(string fileID, string name, object userState)
		{
			if (loadFileOperationCompleted == null)
			{
				loadFileOperationCompleted = OnloadFileOperationCompleted;
			}
			InvokeAsync("loadFile", new object[2]
			{
				fileID,
				name
			}, loadFileOperationCompleted, userState);
		}

		private void OnloadFileOperationCompleted(object arg)
		{
			if (this.loadFileCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.loadFileCompleted(this, new loadFileCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		public new void CancelAsync(object userState)
		{
			base.CancelAsync(userState);
		}

		private bool IsLocalFileSystemWebService(string url)
		{
			if (url == null || url == string.Empty)
			{
				return false;
			}
			Uri uri = new Uri(url);
			if (uri.Port >= 1024 && string.Compare(uri.Host, "localHost", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return true;
			}
			return false;
		}
	}
}
