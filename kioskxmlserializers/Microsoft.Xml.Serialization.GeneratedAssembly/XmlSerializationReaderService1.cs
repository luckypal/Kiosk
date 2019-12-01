using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Xml.Serialization.GeneratedAssembly
{
	public class XmlSerializationReaderService1 : XmlSerializationReader
	{
		private string id2_httptempuriorg;

		private string id7_loadFileResult;

		private string id4_SaveFileResponse;

		private string id1_VersionResponse;

		private string id6_loadFileResponse;

		private string id5_SaveFileResult;

		private string id3_VersionResult;

		public object[] Read1_VersionResponse()
		{
			base.Reader.MoveToContent();
			object[] array = new object[1];
			base.Reader.MoveToContent();
			int whileIterations = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != 0)
			{
				if (base.Reader.IsStartElement(id1_VersionResponse, id2_httptempuriorg))
				{
					bool[] array2 = new bool[1];
					if (base.Reader.IsEmptyElement)
					{
						base.Reader.Skip();
						base.Reader.MoveToContent();
						continue;
					}
					base.Reader.ReadStartElement();
					base.Reader.MoveToContent();
					int whileIterations2 = 0;
					int readerCount2 = base.ReaderCount;
					while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != 0)
					{
						if (base.Reader.NodeType == XmlNodeType.Element)
						{
							if (!array2[0] && (object)base.Reader.LocalName == id3_VersionResult && (object)base.Reader.NamespaceURI == id2_httptempuriorg)
							{
								array[0] = base.Reader.ReadElementString();
								array2[0] = true;
							}
							else
							{
								UnknownNode(array, "http://tempuri.org/:VersionResult");
							}
						}
						else
						{
							UnknownNode(array, "http://tempuri.org/:VersionResult");
						}
						base.Reader.MoveToContent();
						CheckReaderCount(ref whileIterations2, ref readerCount2);
					}
					ReadEndElement();
				}
				else
				{
					UnknownNode(null, "http://tempuri.org/:VersionResponse");
				}
				base.Reader.MoveToContent();
				CheckReaderCount(ref whileIterations, ref readerCount);
			}
			return array;
		}

		public object[] Read2_VersionResponseOutHeaders()
		{
			base.Reader.MoveToContent();
			object[] array = new object[0];
			base.Reader.MoveToContent();
			int whileIterations = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != 0)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					UnknownNode(array, "");
				}
				else
				{
					UnknownNode(array, "");
				}
				base.Reader.MoveToContent();
				CheckReaderCount(ref whileIterations, ref readerCount);
			}
			return array;
		}

		public object[] Read3_SaveFileResponse()
		{
			base.Reader.MoveToContent();
			object[] array = new object[1]
			{
				false
			};
			base.Reader.MoveToContent();
			int whileIterations = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != 0)
			{
				if (base.Reader.IsStartElement(id4_SaveFileResponse, id2_httptempuriorg))
				{
					bool[] array2 = new bool[1];
					if (base.Reader.IsEmptyElement)
					{
						base.Reader.Skip();
						base.Reader.MoveToContent();
						continue;
					}
					base.Reader.ReadStartElement();
					base.Reader.MoveToContent();
					int whileIterations2 = 0;
					int readerCount2 = base.ReaderCount;
					while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != 0)
					{
						if (base.Reader.NodeType == XmlNodeType.Element)
						{
							if (!array2[0] && (object)base.Reader.LocalName == id5_SaveFileResult && (object)base.Reader.NamespaceURI == id2_httptempuriorg)
							{
								array[0] = XmlConvert.ToBoolean(base.Reader.ReadElementString());
								array2[0] = true;
							}
							else
							{
								UnknownNode(array, "http://tempuri.org/:SaveFileResult");
							}
						}
						else
						{
							UnknownNode(array, "http://tempuri.org/:SaveFileResult");
						}
						base.Reader.MoveToContent();
						CheckReaderCount(ref whileIterations2, ref readerCount2);
					}
					ReadEndElement();
				}
				else
				{
					UnknownNode(null, "http://tempuri.org/:SaveFileResponse");
				}
				base.Reader.MoveToContent();
				CheckReaderCount(ref whileIterations, ref readerCount);
			}
			return array;
		}

		public object[] Read4_SaveFileResponseOutHeaders()
		{
			base.Reader.MoveToContent();
			object[] array = new object[0];
			base.Reader.MoveToContent();
			int whileIterations = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != 0)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					UnknownNode(array, "");
				}
				else
				{
					UnknownNode(array, "");
				}
				base.Reader.MoveToContent();
				CheckReaderCount(ref whileIterations, ref readerCount);
			}
			return array;
		}

		public object[] Read5_loadFileResponse()
		{
			base.Reader.MoveToContent();
			object[] array = new object[1];
			base.Reader.MoveToContent();
			int whileIterations = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != 0)
			{
				if (base.Reader.IsStartElement(id6_loadFileResponse, id2_httptempuriorg))
				{
					bool[] array2 = new bool[1];
					if (base.Reader.IsEmptyElement)
					{
						base.Reader.Skip();
						base.Reader.MoveToContent();
						continue;
					}
					base.Reader.ReadStartElement();
					base.Reader.MoveToContent();
					int whileIterations2 = 0;
					int readerCount2 = base.ReaderCount;
					while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != 0)
					{
						if (base.Reader.NodeType == XmlNodeType.Element)
						{
							if (!array2[0] && (object)base.Reader.LocalName == id7_loadFileResult && (object)base.Reader.NamespaceURI == id2_httptempuriorg)
							{
								array[0] = ToByteArrayBase64(isNull: false);
								array2[0] = true;
							}
							else
							{
								UnknownNode(array, "http://tempuri.org/:loadFileResult");
							}
						}
						else
						{
							UnknownNode(array, "http://tempuri.org/:loadFileResult");
						}
						base.Reader.MoveToContent();
						CheckReaderCount(ref whileIterations2, ref readerCount2);
					}
					ReadEndElement();
				}
				else
				{
					UnknownNode(null, "http://tempuri.org/:loadFileResponse");
				}
				base.Reader.MoveToContent();
				CheckReaderCount(ref whileIterations, ref readerCount);
			}
			return array;
		}

		public object[] Read6_loadFileResponseOutHeaders()
		{
			base.Reader.MoveToContent();
			object[] array = new object[0];
			base.Reader.MoveToContent();
			int whileIterations = 0;
			int readerCount = base.ReaderCount;
			while (base.Reader.NodeType != XmlNodeType.EndElement && base.Reader.NodeType != 0)
			{
				if (base.Reader.NodeType == XmlNodeType.Element)
				{
					UnknownNode(array, "");
				}
				else
				{
					UnknownNode(array, "");
				}
				base.Reader.MoveToContent();
				CheckReaderCount(ref whileIterations, ref readerCount);
			}
			return array;
		}

		protected override void InitCallbacks()
		{
		}

		protected override void InitIDs()
		{
			id2_httptempuriorg = base.Reader.NameTable.Add("http://tempuri.org/");
			id7_loadFileResult = base.Reader.NameTable.Add("loadFileResult");
			id4_SaveFileResponse = base.Reader.NameTable.Add("SaveFileResponse");
			id1_VersionResponse = base.Reader.NameTable.Add("VersionResponse");
			id6_loadFileResponse = base.Reader.NameTable.Add("loadFileResponse");
			id5_SaveFileResult = base.Reader.NameTable.Add("SaveFileResult");
			id3_VersionResult = base.Reader.NameTable.Add("VersionResult");
		}
	}
}
