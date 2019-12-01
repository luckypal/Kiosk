using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace Kiosk.SerQuiosc2
{
	[GeneratedCode("System.Web.Services", "4.6.1586.0")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	public class VersionCompletedEventArgs : AsyncCompletedEventArgs
	{
		private object[] results;

		public string Result
		{
			get
			{
				RaiseExceptionIfNecessary();
				return (string)results[0];
			}
		}

		internal VersionCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}
	}
}
