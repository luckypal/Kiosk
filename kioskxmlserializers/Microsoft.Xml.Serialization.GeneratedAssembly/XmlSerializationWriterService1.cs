using System.Xml.Serialization;

namespace Microsoft.Xml.Serialization.GeneratedAssembly
{
	public class XmlSerializationWriterService1 : XmlSerializationWriter
	{
		public void Write1_Version(object[] p)
		{
			WriteStartDocument();
			TopLevelElement();
			_ = p.Length;
			WriteStartElement("Version", "http://tempuri.org/", null, writePrefixed: false);
			WriteEndElement();
		}

		public void Write2_VersionInHeaders(object[] p)
		{
			WriteStartDocument();
			TopLevelElement();
			_ = p.Length;
		}

		public void Write3_SaveFile(object[] p)
		{
			WriteStartDocument();
			TopLevelElement();
			int num = p.Length;
			WriteStartElement("SaveFile", "http://tempuri.org/", null, writePrefixed: false);
			if (num > 0)
			{
				WriteElementString("fileID", "http://tempuri.org/", (string)p[0]);
			}
			if (num > 1)
			{
				WriteElementString("name", "http://tempuri.org/", (string)p[1]);
			}
			if (num > 2)
			{
				WriteElementString("UserName", "http://tempuri.org/", (string)p[2]);
			}
			if (num > 3)
			{
				WriteElementStringRaw("arraytoinsert", "http://tempuri.org/", XmlSerializationWriter.FromByteArrayBase64((byte[])p[3]));
			}
			WriteEndElement();
		}

		public void Write4_SaveFileInHeaders(object[] p)
		{
			WriteStartDocument();
			TopLevelElement();
			_ = p.Length;
		}

		public void Write5_loadFile(object[] p)
		{
			WriteStartDocument();
			TopLevelElement();
			int num = p.Length;
			WriteStartElement("loadFile", "http://tempuri.org/", null, writePrefixed: false);
			if (num > 0)
			{
				WriteElementString("fileID", "http://tempuri.org/", (string)p[0]);
			}
			if (num > 1)
			{
				WriteElementString("name", "http://tempuri.org/", (string)p[1]);
			}
			WriteEndElement();
		}

		public void Write6_loadFileInHeaders(object[] p)
		{
			WriteStartDocument();
			TopLevelElement();
			_ = p.Length;
		}

		protected override void InitCallbacks()
		{
		}
	}
}
