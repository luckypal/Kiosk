using System;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace wyUpdate.Common
{
	public static class VersionTools
	{
		private static readonly Hashtable greek_ltrs = new Hashtable
		{
			{
				"alpha",
				0
			},
			{
				"beta",
				1
			},
			{
				"gamma",
				2
			},
			{
				"delta",
				3
			},
			{
				"epsilon",
				4
			},
			{
				"zeta",
				5
			},
			{
				"eta",
				6
			},
			{
				"theta",
				7
			},
			{
				"iota",
				8
			},
			{
				"kappa",
				9
			},
			{
				"lambda",
				10
			},
			{
				"mu",
				11
			},
			{
				"nu",
				12
			},
			{
				"xi",
				13
			},
			{
				"omicron",
				14
			},
			{
				"pi",
				15
			},
			{
				"rho",
				16
			},
			{
				"sigma",
				17
			},
			{
				"tau",
				18
			},
			{
				"upsilon",
				19
			},
			{
				"phi",
				20
			},
			{
				"chi",
				21
			},
			{
				"psi",
				22
			},
			{
				"omega",
				23
			},
			{
				"rc",
				24
			}
		};

		private static string thisVersion;

		private static string selfLocation;

		public static string SelfLocation => selfLocation ?? (selfLocation = Application.ExecutablePath);

		public static int Compare(string versionA, string versionB)
		{
			if (versionA == null)
			{
				return -1;
			}
			if (versionB == null)
			{
				return 1;
			}
			versionA = Regex.Replace(versionA.ToLowerInvariant(), "release[\\s]+candidate", "rc");
			versionB = Regex.Replace(versionB.ToLowerInvariant(), "release[\\s]+candidate", "rc");
			int index = 0;
			int index2 = 0;
			bool lastWasLetter = true;
			bool lastWasLetter2 = true;
			string text;
			while (true)
			{
				int num = index;
				int num2 = index2;
				text = GetNextObject(versionA, ref index, ref lastWasLetter);
				string text2 = GetNextObject(versionB, ref index2, ref lastWasLetter2);
				if (!lastWasLetter2 && text2 != null && (text == null || lastWasLetter))
				{
					text = "0";
					index = num;
				}
				else if (!lastWasLetter && text != null && (text2 == null || lastWasLetter2))
				{
					text2 = "0";
					index2 = num2;
				}
				num = (lastWasLetter ? GetGreekIndex(text) : (-1));
				num2 = (lastWasLetter2 ? GetGreekIndex(text2) : (-1));
				if (text == null && text2 == null)
				{
					return 0;
				}
				if (text == null)
				{
					if (num2 != -1)
					{
						return 1;
					}
					return -1;
				}
				if (text2 == null)
				{
					if (num != -1)
					{
						return -1;
					}
					return 1;
				}
				if (char.IsDigit(text[0]) != char.IsDigit(text2[0]))
				{
					break;
				}
				if (char.IsDigit(text[0]))
				{
					int num3 = IntCompare(text, text2);
					if (num3 != 0)
					{
						return num3;
					}
					continue;
				}
				if (num == -1 && num2 == -1)
				{
					int num3 = string.Compare(text, text2, StringComparison.Ordinal);
					if (num3 != 0)
					{
						return num3;
					}
					continue;
				}
				if (num == -1)
				{
					return 1;
				}
				if (num2 == -1)
				{
					return -1;
				}
				if (num > num2)
				{
					return 1;
				}
				if (num2 > num)
				{
					return -1;
				}
			}
			if (char.IsDigit(text[0]))
			{
				return 1;
			}
			return -1;
		}

		private static string GetNextObject(string version, ref int index, ref bool lastWasLetter)
		{
			int num = -1;
			int num2 = index;
			while (version.Length != index)
			{
				if (num == -1)
				{
					if (char.IsLetter(version[index]))
					{
						num2 = index;
						num = 1;
					}
					else if (char.IsDigit(version[index]))
					{
						num2 = index;
						num = 2;
					}
					else if (lastWasLetter && !char.IsWhiteSpace(version[index]))
					{
						index++;
						lastWasLetter = false;
						return "0";
					}
				}
				else if ((num == 1 && !char.IsLetter(version[index])) || (num == 2 && !char.IsDigit(version[index])))
				{
					break;
				}
				index++;
			}
			lastWasLetter = (num == 1);
			if (num == 1 || num == 2)
			{
				return version.Substring(num2, index - num2);
			}
			return null;
		}

		private static int GetGreekIndex(object str)
		{
			object obj = greek_ltrs[str];
			if (obj == null)
			{
				return -1;
			}
			return (int)obj;
		}

		private static int IntCompare(string a, string b)
		{
			int num = -1;
			for (int i = 0; i < a.Length && a[i] == '0'; i++)
			{
				num = i;
			}
			if (num != -1)
			{
				a = a.Substring(num + 1, a.Length - (num + 1));
			}
			num = -1;
			for (int j = 0; j < b.Length && b[j] == '0'; j++)
			{
				num = j;
			}
			if (num != -1)
			{
				b = b.Substring(num + 1, b.Length - (num + 1));
			}
			if (a.Length > b.Length)
			{
				return 1;
			}
			if (a.Length < b.Length)
			{
				return -1;
			}
			return string.Compare(a, b, StringComparison.Ordinal);
		}

		public static string FromExecutingAssembly()
		{
			return thisVersion ?? (thisVersion = FileVersionInfo.GetVersionInfo(SelfLocation).FileVersion);
		}
	}
}
