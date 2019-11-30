using System;
using System.IO;

namespace wyUpdate
{
	public class Logger
	{
		private readonly string Filename;

		public Logger(string file)
		{
			Filename = file;
		}

		public void Write(string message)
		{
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(Filename, append: true))
				{
					streamWriter.Write(DateTime.Now.ToString("M/d/yyyy HH:mm:ss:fff tt") + ": ");
					streamWriter.WriteLine(message);
				}
			}
			catch
			{
			}
		}
	}
}
