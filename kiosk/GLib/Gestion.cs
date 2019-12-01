using System;
using System.Text.RegularExpressions;

namespace GLib
{
	public class Gestion
	{
		private enum TiposCodigosEnum
		{
			NIF,
			NIE,
			CIF,
			CPF,
			PIS,
			CNPJ
		}

		private string numero;

		private TiposCodigosEnum tipo;

		public string CodigoIntracomunitario
		{
			get;
			internal set;
		}

		internal bool EsIntraComunitario
		{
			get;
			set;
		}

		public string LetraInicial
		{
			get;
			internal set;
		}

		public int Numero
		{
			get;
			internal set;
		}

		public string DigitoControl
		{
			get;
			internal set;
		}

		public bool EsCorrecto
		{
			get;
			internal set;
		}

		public string TipoNif => tipo.ToString();

		public static bool IsCnpj(string cnpj)
		{
			int[] array = new int[12]
			{
				5,
				4,
				3,
				2,
				9,
				8,
				7,
				6,
				5,
				4,
				3,
				2
			};
			int[] array2 = new int[13]
			{
				6,
				5,
				4,
				3,
				2,
				9,
				8,
				7,
				6,
				5,
				4,
				3,
				2
			};
			cnpj = cnpj.Trim();
			cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
			if (cnpj.Length != 14)
			{
				return false;
			}
			string text = cnpj.Substring(0, 12);
			int num = 0;
			for (int i = 0; i < 12; i++)
			{
				num += int.Parse(text[i].ToString()) * array[i];
			}
			int num2 = num % 11;
			string text2 = ((num2 >= 2) ? (11 - num2) : 0).ToString();
			text += text2;
			num = 0;
			for (int i = 0; i < 13; i++)
			{
				num += int.Parse(text[i].ToString()) * array2[i];
			}
			num2 = num % 11;
			text2 += ((num2 >= 2) ? (11 - num2) : 0).ToString();
			return cnpj.EndsWith(text2);
		}

		public static bool IsPis(string pis)
		{
			int[] array = new int[10]
			{
				3,
				2,
				9,
				8,
				7,
				6,
				5,
				4,
				3,
				2
			};
			if (pis.Trim().Length != 11)
			{
				return false;
			}
			pis = pis.Trim();
			pis = pis.Replace("-", "").Replace(".", "").PadLeft(11, '0');
			int num = 0;
			for (int i = 0; i < 10; i++)
			{
				num += int.Parse(pis[i].ToString()) * array[i];
			}
			int num2 = num % 11;
			return pis.EndsWith(((num2 >= 2) ? (11 - num2) : 0).ToString());
		}

		public static bool IsCpf(string cpf)
		{
			int[] array = new int[9]
			{
				10,
				9,
				8,
				7,
				6,
				5,
				4,
				3,
				2
			};
			int[] array2 = new int[10]
			{
				11,
				10,
				9,
				8,
				7,
				6,
				5,
				4,
				3,
				2
			};
			cpf = cpf.Trim();
			cpf = cpf.Replace(".", "").Replace("-", "");
			if (cpf.Length != 11)
			{
				return false;
			}
			string text = cpf.Substring(0, 9);
			int num = 0;
			for (int i = 0; i < 9; i++)
			{
				num += int.Parse(text[i].ToString()) * array[i];
			}
			int num2 = num % 11;
			string text2 = ((num2 >= 2) ? (11 - num2) : 0).ToString();
			text += text2;
			num = 0;
			for (int i = 0; i < 10; i++)
			{
				num += int.Parse(text[i].ToString()) * array2[i];
			}
			num2 = num % 11;
			text2 += ((num2 >= 2) ? (11 - num2) : 0).ToString();
			return cpf.EndsWith(text2);
		}

		private Gestion(string numero)
		{
			numero = EliminaCaracteres(numero);
			numero = numero.ToUpper();
			if (numero.Length != 9 && numero.Length != 11)
			{
				throw new ArgumentException("El NIF no tiene un número de caracteres válidos");
			}
			this.numero = numero;
			Desglosa();
			switch (tipo)
			{
			case TiposCodigosEnum.NIF:
			case TiposCodigosEnum.NIE:
				EsCorrecto = IsNif();
				break;
			case TiposCodigosEnum.CIF:
				EsCorrecto = IsCif();
				break;
			}
		}

		private void Desglosa()
		{
			int result;
			if (numero.Length == 11)
			{
				EsIntraComunitario = true;
				CodigoIntracomunitario = numero.Substring(0, 2);
				LetraInicial = numero.Substring(2, 1);
				int.TryParse(numero.Substring(3, 7), out result);
				DigitoControl = numero.Substring(10, 1);
				tipo = GetTipoDocumento(LetraInicial[0]);
			}
			else
			{
				tipo = GetTipoDocumento(numero[0]);
				EsIntraComunitario = false;
				if (tipo == TiposCodigosEnum.NIF)
				{
					LetraInicial = string.Empty;
					int.TryParse(numero.Substring(0, 8), out result);
				}
				else
				{
					LetraInicial = numero.Substring(0, 1);
					int.TryParse(numero.Substring(1, 7), out result);
				}
				DigitoControl = numero.Substring(8, 1);
			}
			Numero = result;
		}

		private TiposCodigosEnum GetTipoDocumento(char letra)
		{
			Regex regex = new Regex("[0-9]");
			if (regex.IsMatch(letra.ToString()))
			{
				return TiposCodigosEnum.NIF;
			}
			Regex regex2 = new Regex("[XYZ]");
			if (regex2.IsMatch(letra.ToString()))
			{
				return TiposCodigosEnum.NIE;
			}
			Regex regex3 = new Regex("[ABCDEFGHJPQRSUVNW]");
			if (regex3.IsMatch(letra.ToString()))
			{
				return TiposCodigosEnum.CIF;
			}
			throw new ApplicationException("El código no es reconocible");
		}

		private string EliminaCaracteres(string numero)
		{
			string pattern = "[^\\w]";
			Regex regex = new Regex(pattern);
			return regex.Replace(numero, "");
		}

		private bool IsNif()
		{
			return DigitoControl == GetLetraNif();
		}

		private bool IsCif()
		{
			string[] array = new string[10]
			{
				"J",
				"A",
				"B",
				"C",
				"D",
				"E",
				"F",
				"G",
				"H",
				"I"
			};
			string text = Numero.ToString("0000000");
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			bool flag = false;
			for (num4 = 0; num4 < text.Length; num4++)
			{
				int.TryParse(text[num4].ToString(), out int result);
				if ((num4 + 1) % 2 == 0)
				{
					num += result;
					continue;
				}
				result *= 2;
				num2 += SumaDigitos(result);
			}
			num3 += num + num2;
			int num5 = num3 % 10;
			if (num5 != 0)
			{
				num5 = 10 - num5;
			}
			switch (LetraInicial)
			{
			case "A":
			case "B":
			case "E":
			case "H":
				return DigitoControl == num5.ToString();
			case "K":
			case "P":
			case "Q":
			case "S":
				return DigitoControl == array[num5];
			default:
				return DigitoControl == num5.ToString() || DigitoControl == array[num5];
			}
		}

		private int SumaDigitos(int digitos)
		{
			string text = digitos.ToString();
			int num = 0;
			for (int i = 0; i < text.Length; i++)
			{
				int.TryParse(text[i].ToString(), out int result);
				num += result;
			}
			return num;
		}

		private string GetLetraNif()
		{
			int index = Numero % 23;
			return "TRWAGMYFPDXBNJZSQVHLCKET"[index].ToString();
		}

		public override string ToString()
		{
			string format = "{0:0000000}";
			if (tipo == TiposCodigosEnum.CIF && LetraInicial == "")
			{
				format = "{0:00000000}";
			}
			if (tipo == TiposCodigosEnum.NIF)
			{
				format = "{0:00000000}";
			}
			return EsIntraComunitario ? CodigoIntracomunitario : (string.Empty + LetraInicial + string.Format(format, Numero) + DigitoControl);
		}

		public static Gestion IsNif(string numero)
		{
			return new Gestion(numero);
		}

		public static int Mod10(string data)
		{
			int num = 0;
			bool flag = true;
			for (int num2 = data.Length - 1; num2 >= 0; num2--)
			{
				if (flag)
				{
					int num3 = Convert.ToInt32(data[num2].ToString()) * 2;
					if (num3 >= 10)
					{
						string text = num3.ToString();
						num3 = Convert.ToInt32(text[0].ToString()) + Convert.ToInt32(text[1].ToString());
					}
					num += num3;
				}
				else
				{
					num += Convert.ToInt32(data[num2].ToString());
				}
				flag = !flag;
			}
			int num4 = (num / 10 + 1) * 10 - num;
			return (num4 != 10) ? num4 : 0;
		}

		public static string Build_Mod10(string _user, int _ticket, int _id, int _opc)
		{
			string text;
			int num2;
			if (_opc == 0)
			{
				ushort num = (ushort)_id;
				text = $"{num:00000}{_ticket:000000}";
				num2 = Mod10(text);
				return "4" + text + num2 + "6";
			}
			text = $"{_id:00000000000}";
			num2 = Mod10(text);
			return "7" + text + num2 + "3";
		}

		public static bool Decode_Mod10(string _bar, out int _ticket, out int _id)
		{
			_ticket = 0;
			_id = 0;
			string value = _bar.Substring(1, 5);
			try
			{
				_id = Convert.ToInt32(value);
			}
			catch
			{
				return false;
			}
			string value2 = _bar.Substring(6, 6);
			try
			{
				_ticket = Convert.ToInt32(value2);
			}
			catch
			{
				return false;
			}
			return true;
		}

		public static int Check_Mod10(string _user, string _data)
		{
			string data = _data.Substring(1, 11);
			if ((_data[0] != '4' || _data[13] != '6') && (_data[0] != '7' || _data[13] != '3'))
			{
				return 0;
			}
			int num = Mod10(data);
			int num2 = Convert.ToInt32(_data[12]) - Convert.ToInt32('0');
			if (num == num2)
			{
				if (_data[0] == '4')
				{
					return 1;
				}
				if (_data[0] == '7')
				{
					return 2;
				}
			}
			return 0;
		}
	}
}
