// Decompiled with JetBrains decompiler
// Type: GLib.Gestion
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.Text.RegularExpressions;

namespace GLib
{
  public class Gestion
  {
    private string numero;
    private Gestion.TiposCodigosEnum tipo;

    public static bool IsCnpj(string cnpj)
    {
      int[] numArray1 = new int[12]
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
      int[] numArray2 = new int[13]
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
        return false;
      string str1 = cnpj.Substring(0, 12);
      int num1 = 0;
      for (int index = 0; index < 12; ++index)
        num1 += int.Parse(str1[index].ToString()) * numArray1[index];
      int num2 = num1 % 11;
      int num3 = num2 >= 2 ? 11 - num2 : 0;
      string str2 = num3.ToString();
      string str3 = str1 + str2;
      int num4 = 0;
      for (int index = 0; index < 13; ++index)
        num4 += int.Parse(str3[index].ToString()) * numArray2[index];
      num3 = num4 % 11;
      num3 = num3 >= 2 ? 11 - num3 : 0;
      string str4 = str2 + num3.ToString();
      return cnpj.EndsWith(str4);
    }

    public static bool IsPis(string pis)
    {
      int[] numArray = new int[10]
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
        return false;
      pis = pis.Trim();
      pis = pis.Replace("-", "").Replace(".", "").PadLeft(11, '0');
      int num1 = 0;
      for (int index = 0; index < 10; ++index)
        num1 += int.Parse(pis[index].ToString()) * numArray[index];
      int num2 = num1 % 11;
      int num3 = num2 >= 2 ? 11 - num2 : 0;
      return pis.EndsWith(num3.ToString());
    }

    public static bool IsCpf(string cpf)
    {
      int[] numArray1 = new int[9]
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
      int[] numArray2 = new int[10]
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
        return false;
      string str1 = cpf.Substring(0, 9);
      int num1 = 0;
      for (int index = 0; index < 9; ++index)
        num1 += int.Parse(str1[index].ToString()) * numArray1[index];
      int num2 = num1 % 11;
      int num3 = num2 >= 2 ? 11 - num2 : 0;
      string str2 = num3.ToString();
      string str3 = str1 + str2;
      int num4 = 0;
      for (int index = 0; index < 10; ++index)
        num4 += int.Parse(str3[index].ToString()) * numArray2[index];
      num3 = num4 % 11;
      num3 = num3 >= 2 ? 11 - num3 : 0;
      string str4 = str2 + num3.ToString();
      return cpf.EndsWith(str4);
    }

    public string CodigoIntracomunitario { get; internal set; }

    internal bool EsIntraComunitario { get; set; }

    public string LetraInicial { get; internal set; }

    public int Numero { get; internal set; }

    public string DigitoControl { get; internal set; }

    public bool EsCorrecto { get; internal set; }

    public string TipoNif
    {
      get
      {
        return this.tipo.ToString();
      }
    }

    private Gestion(string numero)
    {
      numero = this.EliminaCaracteres(numero);
      numero = numero.ToUpper();
      if (numero.Length != 9 && numero.Length != 11)
        throw new ArgumentException("El NIF no tiene un número de caracteres válidos");
      this.numero = numero;
      this.Desglosa();
      switch (this.tipo)
      {
        case Gestion.TiposCodigosEnum.NIF:
        case Gestion.TiposCodigosEnum.NIE:
          this.EsCorrecto = this.IsNif();
          break;
        case Gestion.TiposCodigosEnum.CIF:
          this.EsCorrecto = this.IsCif();
          break;
      }
    }

    private void Desglosa()
    {
      int result;
      if (this.numero.Length == 11)
      {
        this.EsIntraComunitario = true;
        this.CodigoIntracomunitario = this.numero.Substring(0, 2);
        this.LetraInicial = this.numero.Substring(2, 1);
        int.TryParse(this.numero.Substring(3, 7), out result);
        this.DigitoControl = this.numero.Substring(10, 1);
        this.tipo = this.GetTipoDocumento(this.LetraInicial[0]);
      }
      else
      {
        this.tipo = this.GetTipoDocumento(this.numero[0]);
        this.EsIntraComunitario = false;
        if (this.tipo == Gestion.TiposCodigosEnum.NIF)
        {
          this.LetraInicial = string.Empty;
          int.TryParse(this.numero.Substring(0, 8), out result);
        }
        else
        {
          this.LetraInicial = this.numero.Substring(0, 1);
          int.TryParse(this.numero.Substring(1, 7), out result);
        }
        this.DigitoControl = this.numero.Substring(8, 1);
      }
      this.Numero = result;
    }

    private Gestion.TiposCodigosEnum GetTipoDocumento(char letra)
    {
      if (new Regex("[0-9]").IsMatch(letra.ToString()))
        return Gestion.TiposCodigosEnum.NIF;
      if (new Regex("[XYZ]").IsMatch(letra.ToString()))
        return Gestion.TiposCodigosEnum.NIE;
      if (new Regex("[ABCDEFGHJPQRSUVNW]").IsMatch(letra.ToString()))
        return Gestion.TiposCodigosEnum.CIF;
      throw new ApplicationException("El código no es reconocible");
    }

    private string EliminaCaracteres(string numero)
    {
      return new Regex("[^\\w]").Replace(numero, "");
    }

    private bool IsNif()
    {
      return this.DigitoControl == this.GetLetraNif();
    }

    private bool IsCif()
    {
      string[] strArray = new string[10]
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
      string str = this.Numero.ToString("0000000");
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      for (int index = 0; index < str.Length; ++index)
      {
        int result;
        int.TryParse(str[index].ToString(), out result);
        if ((index + 1) % 2 == 0)
        {
          num1 += result;
        }
        else
        {
          result *= 2;
          num2 += this.SumaDigitos(result);
        }
      }
      int index1 = (num3 + (num1 + num2)) % 10;
      if (index1 != 0)
        index1 = 10 - index1;
      bool flag;
      switch (this.LetraInicial)
      {
        case "A":
        case "B":
        case "E":
        case "H":
          flag = this.DigitoControl == index1.ToString();
          break;
        case "K":
        case "P":
        case "Q":
        case "S":
          flag = this.DigitoControl == strArray[index1];
          break;
        default:
          flag = this.DigitoControl == index1.ToString() || this.DigitoControl == strArray[index1];
          break;
      }
      return flag;
    }

    private int SumaDigitos(int digitos)
    {
      string str = digitos.ToString();
      int num = 0;
      for (int index = 0; index < str.Length; ++index)
      {
        int result;
        int.TryParse(str[index].ToString(), out result);
        num += result;
      }
      return num;
    }

    private string GetLetraNif()
    {
      return "TRWAGMYFPDXBNJZSQVHLCKET"[this.Numero % 23].ToString();
    }

    public override string ToString()
    {
      string format = "{0:0000000}";
      if (this.tipo == Gestion.TiposCodigosEnum.CIF && this.LetraInicial == "")
        format = "{0:00000000}";
      if (this.tipo == Gestion.TiposCodigosEnum.NIF)
        format = "{0:00000000}";
      return this.EsIntraComunitario ? this.CodigoIntracomunitario : string.Empty + this.LetraInicial + string.Format(format, (object) this.Numero) + this.DigitoControl;
    }

    public static Gestion IsNif(string numero)
    {
      return new Gestion(numero);
    }

    public static int Mod10(string data)
    {
      int num1 = 0;
      bool flag = true;
      for (int index = data.Length - 1; index >= 0; --index)
      {
        char ch;
        if (flag)
        {
          ch = data[index];
          int num2 = Convert.ToInt32(ch.ToString()) * 2;
          if (num2 >= 10)
          {
            string str = num2.ToString();
            ch = str[0];
            int int32_1 = Convert.ToInt32(ch.ToString());
            ch = str[1];
            int int32_2 = Convert.ToInt32(ch.ToString());
            num2 = int32_1 + int32_2;
          }
          num1 += num2;
        }
        else
        {
          int num2 = num1;
          ch = data[index];
          int int32 = Convert.ToInt32(ch.ToString());
          num1 = num2 + int32;
        }
        flag = !flag;
      }
      int num3 = (num1 / 10 + 1) * 10 - num1;
      return num3 == 10 ? 0 : num3;
    }

    public static string Build_Mod10(string _user, int _ticket, int _id, int _opc)
    {
      if (_opc == 0)
      {
        string data = string.Format("{0:00000}{1:000000}", (object) (ushort) _id, (object) _ticket);
        int num = Gestion.Mod10(data);
        return "4" + data + (object) num + "6";
      }
      string data1 = string.Format("{0:00000000000}", (object) _id);
      int num1 = Gestion.Mod10(data1);
      return "7" + data1 + (object) num1 + "3";
    }

    public static bool Decode_Mod10(string _bar, out int _ticket, out int _id)
    {
      _ticket = 0;
      _id = 0;
      string str1 = _bar.Substring(1, 5);
      try
      {
        _id = Convert.ToInt32(str1);
      }
      catch
      {
        return false;
      }
      string str2 = _bar.Substring(6, 6);
      try
      {
        _ticket = Convert.ToInt32(str2);
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
      if ((_data[0] != '4' || _data[13] != '6') && (_data[0] != '7' || _data[13] != '3') || Gestion.Mod10(data) != Convert.ToInt32(_data[12]) - Convert.ToInt32('0'))
        return 0;
      if (_data[0] == '4')
        return 1;
      return _data[0] == '7' ? 2 : 0;
    }

    private enum TiposCodigosEnum
    {
      NIF,
      NIE,
      CIF,
      CPF,
      PIS,
      CNPJ,
    }
  }
}
