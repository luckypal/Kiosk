using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace Kiosk.SerQuiosc2
{
	[DesignerCategory("code")]
	[GeneratedCode("System.Web.Services", "4.6.1586.0")]
	[DebuggerStepThrough]
	public class SaveFileCompletedEventArgs : AsyncCompletedEventArgs
	{
		private object[] results;

		public bool Result
		{
			get
			{
				RaiseExceptionIfNecessary();
				return (bool)results[0];
			}
		}

		internal SaveFileCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}
	}
}
