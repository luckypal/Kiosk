using System;
using System.Linq;
using System.Windows.Forms;

namespace KVKeyboard
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			string text = "en";
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			if (commandLineArgs.Count() <= 1)
			{
				return;
			}
			string text2 = commandLineArgs[1].ToLower();
			if (!(text2.ToLower() == "/q".ToLower()))
			{
				switch (text2.ToLower())
				{
				case "es":
				case "en":
				case "lx":
				case "pt":
				case "br":
				case "de":
				case "fr":
					text = text2.ToLower();
					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(defaultValue: false);
					Application.Run(new VKeyboard_Form(text));
					break;
				}
			}
		}
	}
}
