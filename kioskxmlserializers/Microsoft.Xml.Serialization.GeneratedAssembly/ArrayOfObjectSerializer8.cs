using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Xml.Serialization.GeneratedAssembly
{
	public sealed class ArrayOfObjectSerializer8 : XmlSerializer1
	{
		public override bool CanDeserialize(XmlReader xmlReader)
		{
			return xmlReader.IsStartElement("loadFile", "http://tempuri.org/");
		}

		protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
		{
			((XmlSerializationWriterService1)writer).Write5_loadFile((object[])objectToSerialize);
		}
	}
}
