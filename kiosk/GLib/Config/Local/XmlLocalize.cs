// Decompiled with JetBrains decompiler
// Type: GLib.Config.Local.XmlLocalize
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace GLib.Config.Local
{
  public class XmlLocalize
  {
    public string Path;
    public string BaseFile;
    public XmlLocalize.GLocalizeCountry Translation;
    private string error;

    public XmlLocalize(string _path, string _locs)
    {
      this.BaseFile = _locs;
      this.Path = _path;
      this.Translation = this.Load_Locs(_locs);
      if (this.Translation != null)
        return;
      this.Translation = new XmlLocalize.GLocalizeCountry();
    }

    public static string Ver()
    {
      return "1.0.0";
    }

    public static string Ver_Build()
    {
      return Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }

    public string Localize
    {
      get
      {
        return this.Translation.Loc;
      }
      set
      {
        this.Localize_Set(value);
      }
    }

    public bool Localize_Set(string _loc)
    {
      if (this.Translation == null || this.Translation.Locs == null)
        return false;
      for (int index = 0; index < this.Translation.Locs.Length; ++index)
      {
        if (this.Translation.Locs[index] == _loc)
        {
          this.Localize_Country(ref this.Translation, this.Translation.LocsFile[index]);
          return true;
        }
      }
      return false;
    }

    private XmlLocalize.GLocalizeCountry Load_Locs(string _file)
    {
      int num = 0;
      string _file1 = "";
      string str = "";
      XmlLocalize.GLocalizeCountry glocalizeCountry = (XmlLocalize.GLocalizeCountry) null;
      try
      {
        XmlTextReader xmlTextReader = new XmlTextReader(this.Path + _file);
        while (xmlTextReader.Read())
        {
          if (xmlTextReader.NodeType == XmlNodeType.Element)
          {
            if (xmlTextReader.Name.ToLower() == "countries")
            {
              num = 1;
              if (xmlTextReader.HasAttributes)
              {
                xmlTextReader.MoveToFirstAttribute();
                for (int i = 0; i < xmlTextReader.AttributeCount; ++i)
                {
                  xmlTextReader.MoveToAttribute(i);
                  if (xmlTextReader.Name.ToLower() == "iso")
                    str = xmlTextReader.Value.ToString();
                  else if (xmlTextReader.Name.ToLower() == "file")
                    _file1 = xmlTextReader.Value.ToString();
                }
              }
              glocalizeCountry = this.Localize_Base(_file1);
              glocalizeCountry.BaseFile = _file1;
              glocalizeCountry.BaseLoc = str;
            }
            if (xmlTextReader.Name.ToLower() == "country" && num == 1 && xmlTextReader.HasAttributes)
            {
              xmlTextReader.MoveToFirstAttribute();
              for (int i = 0; i < xmlTextReader.AttributeCount; ++i)
              {
                xmlTextReader.MoveToAttribute(i);
                if (xmlTextReader.Name.ToLower() == "iso")
                  str = xmlTextReader.Value.ToString();
                else if (xmlTextReader.Name.ToLower() == "file")
                  _file1 = xmlTextReader.Value.ToString();
                else
                  xmlTextReader.MoveToElement();
              }
              Array.Resize<string>(ref glocalizeCountry.Locs, glocalizeCountry.Locs.Length + 1);
              glocalizeCountry.Locs[glocalizeCountry.Locs.Length - 1] = str;
              Array.Resize<string>(ref glocalizeCountry.LocsFile, glocalizeCountry.LocsFile.Length + 1);
              glocalizeCountry.LocsFile[glocalizeCountry.LocsFile.Length - 1] = _file1;
            }
          }
          if (xmlTextReader.NodeType == XmlNodeType.EndElement && xmlTextReader.Name.ToLower() == "countries")
            num = 0;
        }
        xmlTextReader.Close();
      }
      catch (Exception ex)
      {
        this.error = ex.Message;
        return (XmlLocalize.GLocalizeCountry) null;
      }
      return glocalizeCountry;
    }

    public bool Localize_Country(ref XmlLocalize.GLocalizeCountry _l, string _file)
    {
      int num1 = 0;
      int num2 = 0;
      string _id = "0";
      try
      {
        XmlTextReader xmlTextReader = new XmlTextReader(this.Path + _file);
        while (xmlTextReader.Read())
        {
          if (xmlTextReader.NodeType == XmlNodeType.Text && num1 == 1)
          {
            string _txt = xmlTextReader.Value.ToString();
            XmlLocalize.GLocalizeItem glocalizeItem = new XmlLocalize.GLocalizeItem();
            _l.EditTranslation(_id, _txt);
            num1 = 0;
          }
          if (xmlTextReader.NodeType == XmlNodeType.Element)
          {
            if (xmlTextReader.Name.ToLower() == "localize")
            {
              num2 = 1;
              if (xmlTextReader.HasAttributes)
              {
                xmlTextReader.MoveToFirstAttribute();
                for (int i = 0; i < xmlTextReader.AttributeCount; ++i)
                {
                  xmlTextReader.MoveToAttribute(i);
                  if (xmlTextReader.Name.ToLower() == "iso")
                    _l.Loc = xmlTextReader.Value.ToString();
                }
              }
            }
            if (xmlTextReader.Name.ToLower() == "text" && num2 == 1)
            {
              num1 = 1;
              if (xmlTextReader.HasAttributes)
              {
                xmlTextReader.MoveToFirstAttribute();
                for (int i = 0; i < xmlTextReader.AttributeCount; ++i)
                {
                  xmlTextReader.MoveToAttribute(i);
                  if (xmlTextReader.Name.ToLower() == "id")
                    _id = xmlTextReader.Value.ToString();
                  else
                    xmlTextReader.MoveToElement();
                }
              }
            }
            if (xmlTextReader.NodeType == XmlNodeType.EndElement && xmlTextReader.Name.ToLower() == "localize")
              num2 = 0;
          }
        }
        xmlTextReader.Close();
      }
      catch (Exception ex)
      {
        this.error = ex.Message;
        return false;
      }
      return true;
    }

    public XmlLocalize.GLocalizeCountry Localize_Base(string _file)
    {
      int num1 = 0;
      int num2 = 0;
      string str1 = "";
      string str2 = "";
      int num3 = 0;
      XmlLocalize.GLocalizeItem.Tipos tipos = XmlLocalize.GLocalizeItem.Tipos.Desconocido;
      XmlLocalize.GLocalizeCountry glocalizeCountry = new XmlLocalize.GLocalizeCountry();
      try
      {
        XmlTextReader xmlTextReader = new XmlTextReader(this.Path + _file);
        while (xmlTextReader.Read())
        {
          if (xmlTextReader.NodeType == XmlNodeType.Text && num1 == 1)
          {
            string str3 = xmlTextReader.Value.ToString();
            glocalizeCountry.CopyItem(new XmlLocalize.GLocalizeItem()
            {
              Translated = true,
              Tipo = tipos,
              Loc = str2,
              Id = num3,
              IdEnum = str1,
              Original = str3,
              Translation = str3
            });
            num1 = 0;
          }
          if (xmlTextReader.NodeType == XmlNodeType.Element)
          {
            if (xmlTextReader.Name.ToLower() == "localize")
            {
              num2 = 1;
              if (xmlTextReader.HasAttributes)
              {
                xmlTextReader.MoveToFirstAttribute();
                for (int i = 0; i < xmlTextReader.AttributeCount; ++i)
                {
                  xmlTextReader.MoveToAttribute(i);
                  if (xmlTextReader.Name.ToLower() == "iso")
                    glocalizeCountry.BaseLoc = glocalizeCountry.Loc = xmlTextReader.Value.ToString();
                }
              }
            }
            if (xmlTextReader.Name.ToLower() == "text" && num2 == 1)
            {
              num1 = 1;
              if (xmlTextReader.HasAttributes)
              {
                xmlTextReader.MoveToFirstAttribute();
                for (int i = 0; i < xmlTextReader.AttributeCount; ++i)
                {
                  xmlTextReader.MoveToAttribute(i);
                  if (xmlTextReader.Name.ToLower() == "iso")
                    str2 = xmlTextReader.Value.ToString();
                  else if (xmlTextReader.Name.ToLower() == "id")
                    num3 = Convert.ToInt32(xmlTextReader.Value.ToString());
                  else if (xmlTextReader.Name.ToLower() == "enum")
                    str1 = xmlTextReader.Value.ToString();
                  else if (xmlTextReader.Name.ToLower() == "type")
                  {
                    switch (xmlTextReader.Value.ToString().ToLower())
                    {
                      case "imagen":
                        tipos = XmlLocalize.GLocalizeItem.Tipos.Imagen;
                        continue;
                      case "link":
                        tipos = XmlLocalize.GLocalizeItem.Tipos.Link;
                        continue;
                      case "texto":
                        tipos = XmlLocalize.GLocalizeItem.Tipos.Texto;
                        continue;
                      case "video":
                        tipos = XmlLocalize.GLocalizeItem.Tipos.Video;
                        continue;
                      case "datos":
                        tipos = XmlLocalize.GLocalizeItem.Tipos.Datos;
                        continue;
                      case "audio":
                        tipos = XmlLocalize.GLocalizeItem.Tipos.Audio;
                        continue;
                      default:
                        tipos = XmlLocalize.GLocalizeItem.Tipos.Desconocido;
                        continue;
                    }
                  }
                  else
                    xmlTextReader.MoveToElement();
                }
              }
            }
            if (xmlTextReader.NodeType == XmlNodeType.EndElement && xmlTextReader.Name.ToLower() == "localize")
              num2 = 0;
          }
        }
        xmlTextReader.Close();
      }
      catch (Exception ex)
      {
        this.error = ex.Message;
        return (XmlLocalize.GLocalizeCountry) null;
      }
      return glocalizeCountry;
    }

    public bool Save_New()
    {
      return this.Save_New(this.Path + this.BaseFile + ".new");
    }

    public bool Save_New(string _name)
    {
      XmlTextWriter xmlTextWriter = new XmlTextWriter(_name + ".tmp", Encoding.ASCII);
      xmlTextWriter.Formatting = Formatting.Indented;
      xmlTextWriter.WriteStartDocument();
      try
      {
        xmlTextWriter.WriteStartElement("localize");
        xmlTextWriter.WriteAttributeString("iso", this.Translation.BaseLoc);
        for (int index = 0; index < this.Translation.Items.Length; ++index)
        {
          if (!this.Translation.Items[index].Translated)
          {
            xmlTextWriter.WriteStartElement("text");
            xmlTextWriter.WriteAttributeString("id", this.Translation.Items[index].Id.ToString());
            xmlTextWriter.WriteAttributeString("iso", this.Translation.Items[index].Loc);
            xmlTextWriter.WriteAttributeString("enum", this.Translation.Items[index].IdEnum);
            xmlTextWriter.WriteAttributeString("type", this.Translation.Items[index].Tipo.ToString());
            xmlTextWriter.WriteString(this.Translation.Items[index].Original);
            xmlTextWriter.WriteEndElement();
          }
        }
        xmlTextWriter.WriteEndElement();
      }
      catch (Exception ex)
      {
        xmlTextWriter.Flush();
        xmlTextWriter.Close();
        return false;
      }
      xmlTextWriter.Flush();
      xmlTextWriter.Close();
      File.Delete(_name);
      File.Copy(_name + ".tmp", _name);
      File.Delete(_name + ".tmp");
      return true;
    }

    public string Text(string text)
    {
      for (int index = 0; index < this.Translation.Items.Length; ++index)
      {
        if (this.Translation.Items[index].Original == text)
          return this.Translation.Items[index].Translation;
      }
      this.Translation.AddItem(text, this.Translation.BaseLoc);
      return text;
    }

    public string Text(int _id)
    {
      for (int index = 0; index < this.Translation.Items.Length; ++index)
      {
        if (this.Translation.Items[index].Id == _id)
          return this.Translation.Items[index].Translation;
      }
      return this.Translation.Loc;
    }

    public class GLocalizeItem
    {
      public string Loc;
      public string Original;
      public string Translation;
      public string IdEnum;
      public int Id;
      public XmlLocalize.GLocalizeItem.Tipos Tipo;
      public bool Translated;

      public object Clone()
      {
        return this.MemberwiseClone();
      }

      public GLocalizeItem()
      {
        this.Loc = "enEN";
        this.Original = "";
        this.Translation = "";
        this.IdEnum = "IDT_";
        this.Id = 0;
        this.Tipo = XmlLocalize.GLocalizeItem.Tipos.Desconocido;
        this.Translated = false;
      }

      public enum Tipos
      {
        Desconocido,
        Texto,
        Imagen,
        Link,
        Audio,
        Video,
        Datos,
      }
    }

    public class GLocalizeCountry
    {
      public string BaseLoc;
      public string BaseFile;
      public string Loc;
      public string File;
      public XmlLocalize.GLocalizeItem[] Items;
      public string[] Locs;
      public string[] LocsFile;

      public GLocalizeCountry()
      {
        this.Items = new XmlLocalize.GLocalizeItem[0];
        this.BaseLoc = "";
        this.Loc = "";
        this.File = "";
        this.Locs = new string[0];
        this.LocsFile = new string[0];
      }

      public int Unique_Key()
      {
        bool flag = false;
        int length = this.Items.Length;
        while (!flag)
        {
          flag = true;
          ++length;
          for (int index = 0; index < this.Items.Length; ++index)
          {
            if (this.Items[index].Id == length)
            {
              flag = false;
              break;
            }
          }
        }
        return length;
      }

      public int AddItem(string _txt, string _loc)
      {
        XmlLocalize.GLocalizeItem glocalizeItem = new XmlLocalize.GLocalizeItem();
        Array.Resize<XmlLocalize.GLocalizeItem>(ref this.Items, this.Items.Length + 1);
        this.Items[this.Items.Length - 1] = glocalizeItem;
        int num = this.Unique_Key();
        this.Items[this.Items.Length - 1].Id = num;
        this.Items[this.Items.Length - 1].Original = _txt;
        this.Items[this.Items.Length - 1].Translation = _txt;
        this.Items[this.Items.Length - 1].IdEnum = "IDT_" + (object) num;
        this.Items[this.Items.Length - 1].Loc = _loc;
        this.Items[this.Items.Length - 1].Tipo = XmlLocalize.GLocalizeItem.Tipos.Desconocido;
        return num;
      }

      public int CopyItem(XmlLocalize.GLocalizeItem _i)
      {
        Array.Resize<XmlLocalize.GLocalizeItem>(ref this.Items, this.Items.Length + 1);
        this.Items[this.Items.Length - 1] = _i;
        return _i.Id;
      }

      public int EditTranslation(string _id, string _txt)
      {
        int int32;
        try
        {
          int32 = Convert.ToInt32(_id);
        }
        catch
        {
          return -1;
        }
        for (int index = 0; index < this.Items.Length; ++index)
        {
          if (this.Items[index].Id == int32)
          {
            this.Items[index].Translation = _txt;
            this.Items[index].Translated = true;
            return index;
          }
        }
        return -1;
      }
    }
  }
}
