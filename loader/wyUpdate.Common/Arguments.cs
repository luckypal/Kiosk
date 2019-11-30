using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace wyUpdate.Common
{
	public sealed class Arguments
	{
		private StringDictionary Parameters;

		public string this[string Param] => Parameters[Param];

		public Arguments(string Args)
		{
			Regex regex = new Regex("(['\"][^\"]+['\"])\\s*|([^\\s]+)\\s*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			MatchCollection matchCollection = regex.Matches(Args);
			string[] array = new string[matchCollection.Count - 1];
			for (int i = 1; i < matchCollection.Count; i++)
			{
				array[i - 1] = matchCollection[i].Value.Trim();
			}
			Extract(array);
		}

		public Arguments(string[] Args)
		{
			Extract(Args);
		}

		private void Extract(string[] Args)
		{
			Parameters = new StringDictionary();
			Regex regex = new Regex("^([/-]|--){1}(?<name>\\w+)([:=])?(?<value>.+)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			char[] trimChars = new char[2]
			{
				'"',
				'\''
			};
			string text = null;
			foreach (string text2 in Args)
			{
				Match match = regex.Match(text2);
				if (!match.Success)
				{
					if (text != null)
					{
						Parameters[text] = text2.Trim(trimChars);
					}
				}
				else
				{
					text = match.Groups["name"].Value;
					Parameters.Add(text, match.Groups["value"].Value.Trim(trimChars));
				}
			}
		}
	}
}
