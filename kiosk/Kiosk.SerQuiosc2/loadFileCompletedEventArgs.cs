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

		public byte[] Result
		{
			get
			{
				RaiseExceptionIfNecessary();
				return (byte[])results[0];
			}
		}

		internal loadFileCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}
	}
}
