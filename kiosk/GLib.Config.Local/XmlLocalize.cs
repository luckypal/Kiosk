using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace GLib.Config.Local
{
	public class XmlLocalize
	{
		public class GLocalizeItem
		{
			public enum Tipos
			{
				Desconocido,
				Texto,
				Imagen,
				Link,
				Audio,
				Video,
				Datos
			}

			public string Loc;

			public string Original;

			public string Translation;

			public string IdEnum;

			public int Id;

			public Tipos Tipo;

			public bool Translated;

			public object Clone()
			{
				return MemberwiseClone();
			}

			public GLocalizeItem()
			{
				Loc = "enEN";
				Original = "";
				Translation = "";
				IdEnum = "IDT_";
				Id = 0;
				Tipo = Tipos.Desconocido;
				Translated = false;
			}
		}

		public class GLocalizeCountry
		{
			public string BaseLoc;

			public string BaseFile;

			public string Loc;

			public string File;

			public GLocalizeItem[] Items;

			public string[] Locs;

			public string[] LocsFile;

			public GLocalizeCountry()
			{
				Items = new GLocalizeItem[0];
				BaseLoc = "";
				Loc = "";
				File = "";
				Locs = new string[0];
				LocsFile = new string[0];
			}

			public int Unique_Key()
			{
				int num = 0;
				bool flag = false;
				num = Items.Length;
				while (!flag)
				{
					flag = true;
					num++;
					for (int i = 0; i < Items.Length; i++)
					{
						if (Items[i].Id == num)
						{
							flag = false;
							break;
						}
					}
				}
				return num;
			}

			public int AddItem(string _txt, string _loc)
			{
				GLocalizeItem gLocalizeItem = new GLocalizeItem();
				Array.Resize(ref Items, Items.Length + 1);
				Items[Items.Length - 1] = gLocalizeItem;
				int num = Unique_Key();
				Items[Items.Length - 1].Id = num;
				Items[Items.Length - 1].Original = _txt;
				Items[Items.Length - 1].Translation = _txt;
				Items[Items.Length - 1].IdEnum = "IDT_" + num;
				Items[Items.Length - 1].Loc = _loc;
				Items[Items.Length - 1].Tipo = GLocalizeItem.Tipos.Desconocido;
				return num;
			}

			public int CopyItem(GLocalizeItem _i)
			{
				Array.Resize(ref Items, Items.Length + 1);
				Items[Items.Length - 1] = _i;
				return _i.Id;
			}

			public int EditTranslation(string _id, string _txt)
			{
				int num = 0;
				try
				{
					num = Convert.ToInt32(_id);
				}
				catch
				{
					return -1;
				}
				for (int i = 0; i < Items.Length; i++)
				{
					if (Items[i].Id == num)
					{
						Items[i].Translation = _txt;
						Items[i].Translated = true;
						return i;
					}
				}
				return -1;
			}
		}

		public string Path;

		public string BaseFile;

		public GLocalizeCountry Translation;

		private string error;

		public string Localize
		{
			get
			{
				return Translation.Loc;
			}
			set
			{
				Localize_Set(value);
			}
		}

		public XmlLocalize(string _path, string _locs)
		{
			BaseFile = _locs;
			Path = _path;
			Translation = Load_Locs(_locs);
			if (Translation == null)
			{
				Translation = new GLocalizeCountry();
			}
		}

		public static string Ver()
		{
			return "1.0.0";
		}

		public static string Ver_Build()
		{
			return Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}

		public bool Localize_Set(string _loc)
		{
			if (Translation == null)
			{
				return false;
			}
			if (Translation.Locs == null)
			{
				return false;
			}
			for (int i = 0; i < Translation.Locs.Length; i++)
			{
				if (Translation.Locs[i] == _loc)
				{
					Localize_Country(ref Translation, Translation.LocsFile[i]);
					return true;
				}
			}
			return false;
		}

		private GLocalizeCountry Load_Locs(string _file)
		{
			int num = 0;
			string text = "";
			string text2 = "";
			GLocalizeCountry gLocalizeCountry = null;
			try
			{
				XmlTextReader xmlTextReader = new XmlTextReader(Path + _file);
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
								for (int i = 0; i < xmlTextReader.AttributeCount; i++)
								{
									xmlTextReader.MoveToAttribute(i);
									if (xmlTextReader.Name.ToLower() == "iso")
									{
										text2 = xmlTextReader.Value.ToString();
									}
									else if (xmlTextReader.Name.ToLower() == "file")
									{
										text = xmlTextReader.Value.ToString();
									}
								}
							}
							gLocalizeCountry = Localize_Base(text);
							gLocalizeCountry.BaseFile = text;
							gLocalizeCountry.BaseLoc = text2;
						}
						if (xmlTextReader.Name.ToLower() == "country" && num == 1 && xmlTextReader.HasAttributes)
						{
							xmlTextReader.MoveToFirstAttribute();
							for (int i = 0; i < xmlTextReader.AttributeCount; i++)
							{
								xmlTextReader.MoveToAttribute(i);
								if (xmlTextReader.Name.ToLower() == "iso")
								{
									text2 = xmlTextReader.Value.ToString();
								}
								else if (xmlTextReader.Name.ToLower() == "file")
								{
									text = xmlTextReader.Value.ToString();
								}
								else
								{
									xmlTextReader.MoveToElement();
								}
							}
							Array.Resize(ref gLocalizeCountry.Locs, gLocalizeCountry.Locs.Length + 1);
							gLocalizeCountry.Locs[gLocalizeCountry.Locs.Length - 1] = text2;
							Array.Resize(ref gLocalizeCountry.LocsFile, gLocalizeCountry.LocsFile.Length + 1);
							gLocalizeCountry.LocsFile[gLocalizeCountry.LocsFile.Length - 1] = text;
						}
					}
					if (xmlTextReader.NodeType == XmlNodeType.EndElement && xmlTextReader.Name.ToLower() == "countries")
					{
						num = 0;
					}
				}
				xmlTextReader.Close();
			}
			catch (Exception ex)
			{
				error = ex.Message;
				return null;
			}
			return gLocalizeCountry;
		}

		public bool Localize_Country(ref GLocalizeCountry _l, string _file)
		{
			int num = 0;
			int num2 = 0;
			string text = "";
			string id = "0";
			try
			{
				XmlTextReader xmlTextReader = new XmlTextReader(Path + _file);
				while (xmlTextReader.Read())
				{
					if (xmlTextReader.NodeType == XmlNodeType.Text && num == 1)
					{
						text = xmlTextReader.Value.ToString();
						GLocalizeItem gLocalizeItem = new GLocalizeItem();
						_l.EditTranslation(id, text);
						num = 0;
					}
					if (xmlTextReader.NodeType == XmlNodeType.Element)
					{
						if (xmlTextReader.Name.ToLower() == "localize")
						{
							num2 = 1;
							if (xmlTextReader.HasAttributes)
							{
								xmlTextReader.MoveToFirstAttribute();
								for (int i = 0; i < xmlTextReader.AttributeCount; i++)
								{
									xmlTextReader.MoveToAttribute(i);
									if (xmlTextReader.Name.ToLower() == "iso")
									{
										_l.Loc = xmlTextReader.Value.ToString();
									}
								}
							}
						}
						if (xmlTextReader.Name.ToLower() == "text" && num2 == 1)
						{
							num = 1;
							if (xmlTextReader.HasAttributes)
							{
								xmlTextReader.MoveToFirstAttribute();
								for (int i = 0; i < xmlTextReader.AttributeCount; i++)
								{
									xmlTextReader.MoveToAttribute(i);
									if (xmlTextReader.Name.ToLower() == "id")
									{
										id = xmlTextReader.Value.ToString();
									}
									else
									{
										xmlTextReader.MoveToElement();
									}
								}
							}
						}
						if (xmlTextReader.NodeType == XmlNodeType.EndElement && xmlTextReader.Name.ToLower() == "localize")
						{
							num2 = 0;
						}
					}
				}
				xmlTextReader.Close();
			}
			catch (Exception ex)
			{
				error = ex.Message;
				return false;
			}
			return true;
		}

		public GLocalizeCountry Localize_Base(string _file)
		{
			int num = 0;
			int num2 = 0;
			string idEnum = "";
			string loc = "";
			string text = "";
			int id = 0;
			GLocalizeItem.Tipos tipo = GLocalizeItem.Tipos.Desconocido;
			GLocalizeCountry gLocalizeCountry = new GLocalizeCountry();
			try
			{
				XmlTextReader xmlTextReader = new XmlTextReader(Path + _file);
				while (xmlTextReader.Read())
				{
					if (xmlTextReader.NodeType == XmlNodeType.Text && num == 1)
					{
						text = xmlTextReader.Value.ToString();
						GLocalizeItem gLocalizeItem = new GLocalizeItem();
						gLocalizeItem.Translated = true;
						gLocalizeItem.Tipo = tipo;
						gLocalizeItem.Loc = loc;
						gLocalizeItem.Id = id;
						gLocalizeItem.IdEnum = idEnum;
						gLocalizeItem.Original = text;
						gLocalizeItem.Translation = text;
						gLocalizeCountry.CopyItem(gLocalizeItem);
						num = 0;
					}
					if (xmlTextReader.NodeType == XmlNodeType.Element)
					{
						if (xmlTextReader.Name.ToLower() == "localize")
						{
							num2 = 1;
							if (xmlTextReader.HasAttributes)
							{
								xmlTextReader.MoveToFirstAttribute();
								for (int i = 0; i < xmlTextReader.AttributeCount; i++)
								{
									xmlTextReader.MoveToAttribute(i);
									if (xmlTextReader.Name.ToLower() == "iso")
									{
										gLocalizeCountry.BaseLoc = (gLocalizeCountry.Loc = xmlTextReader.Value.ToString());
									}
								}
							}
						}
						if (xmlTextReader.Name.ToLower() == "text" && num2 == 1)
						{
							num = 1;
							if (xmlTextReader.HasAttributes)
							{
								xmlTextReader.MoveToFirstAttribute();
								for (int i = 0; i < xmlTextReader.AttributeCount; i++)
								{
									xmlTextReader.MoveToAttribute(i);
									if (xmlTextReader.Name.ToLower() == "iso")
									{
										loc = xmlTextReader.Value.ToString();
									}
									else if (xmlTextReader.Name.ToLower() == "id")
									{
										id = Convert.ToInt32(xmlTextReader.Value.ToString());
									}
									else if (xmlTextReader.Name.ToLower() == "enum")
									{
										idEnum = xmlTextReader.Value.ToString();
									}
									else if (xmlTextReader.Name.ToLower() == "type")
									{
										string text2 = xmlTextReader.Value.ToString();
										switch (text2.ToLower())
										{
										case "imagen":
											tipo = GLocalizeItem.Tipos.Imagen;
											break;
										case "link":
											tipo = GLocalizeItem.Tipos.Link;
											break;
										case "texto":
											tipo = GLocalizeItem.Tipos.Texto;
											break;
										case "video":
											tipo = GLocalizeItem.Tipos.Video;
											break;
										case "datos":
											tipo = GLocalizeItem.Tipos.Datos;
											break;
										case "audio":
											tipo = GLocalizeItem.Tipos.Audio;
											break;
										default:
											tipo = GLocalizeItem.Tipos.Desconocido;
											break;
										}
									}
									else
									{
										xmlTextReader.MoveToElement();
									}
								}
							}
						}
						if (xmlTextReader.NodeType == XmlNodeType.EndElement && xmlTextReader.Name.ToLower() == "localize")
						{
							num2 = 0;
						}
					}
				}
				xmlTextReader.Close();
			}
			catch (Exception ex)
			{
				error = ex.Message;
				return null;
			}
			return gLocalizeCountry;
		}

		public bool Save_New()
		{
			string name = Path + BaseFile + ".new";
			return Save_New(name);
		}

		public bool Save_New(string _name)
		{
			XmlTextWriter xmlTextWriter = new XmlTextWriter(_name + ".tmp", Encoding.ASCII);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.WriteStartDocument();
			try
			{
				xmlTextWriter.WriteStartElement("localize");
				xmlTextWriter.WriteAttributeString("iso", Translation.BaseLoc);
				for (int i = 0; i < Translation.Items.Length; i++)
				{
					if (!Translation.Items[i].Translated)
					{
						xmlTextWriter.WriteStartElement("text");
						xmlTextWriter.WriteAttributeString("id", Translation.Items[i].Id.ToString());
						xmlTextWriter.WriteAttributeString("iso", Translation.Items[i].Loc);
						xmlTextWriter.WriteAttributeString("enum", Translation.Items[i].IdEnum);
						xmlTextWriter.WriteAttributeString("type", Translation.Items[i].Tipo.ToString());
						xmlTextWriter.WriteString(Translation.Items[i].Original);
						xmlTextWriter.WriteEndElement();
					}
				}
				xmlTextWriter.WriteEndElement();
			}
			catch (Exception)
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
			for (int i = 0; i < Translation.Items.Length; i++)
			{
				if (Translation.Items[i].Original == text)
				{
					return Translation.Items[i].Translation;
				}
			}
			Translation.AddItem(text, Translation.BaseLoc);
			return text;
		}

		public string Text(int _id)
		{
			for (int i = 0; i < Translation.Items.Length; i++)
			{
				if (Translation.Items[i].Id == _id)
				{
					return Translation.Items[i].Translation;
				}
			}
			return Translation.Loc;
		}
	}
}
