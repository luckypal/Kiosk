using Kiosk.SerQuiosc2;
using System;
using System.Collections;
using System.Xml.Serialization;

namespace Microsoft.Xml.Serialization.GeneratedAssembly
{
	public class XmlSerializerContract : XmlSerializerImplementation
	{
		private Hashtable readMethods;

		private Hashtable writeMethods;

		private Hashtable typedSerializers;

		public override XmlSerializationReader Reader => new XmlSerializationReaderService1();

		public override XmlSerializationWriter Writer => new XmlSerializationWriterService1();

		public override Hashtable ReadMethods
		{
			get
			{
				if (readMethods == null)
				{
					Hashtable hashtable = new Hashtable();
					hashtable["Kiosk.SerQuiosc2.Service1:System.String Version():Response"] = "Read1_VersionResponse";
					hashtable["Kiosk.SerQuiosc2.Service1:System.String Version():OutHeaders"] = "Read2_VersionResponseOutHeaders";
					hashtable["Kiosk.SerQuiosc2.Service1:Boolean SaveFile(System.String, System.String, System.String, Byte[]):Response"] = "Read3_SaveFileResponse";
					hashtable["Kiosk.SerQuiosc2.Service1:Boolean SaveFile(System.String, System.String, System.String, Byte[]):OutHeaders"] = "Read4_SaveFileResponseOutHeaders";
					hashtable["Kiosk.SerQuiosc2.Service1:Byte[] loadFile(System.String, System.String):Response"] = "Read5_loadFileResponse";
					hashtable["Kiosk.SerQuiosc2.Service1:Byte[] loadFile(System.String, System.String):OutHeaders"] = "Read6_loadFileResponseOutHeaders";
					if (readMethods == null)
					{
						readMethods = hashtable;
					}
				}
				return readMethods;
			}
		}

		public override Hashtable WriteMethods
		{
			get
			{
				if (writeMethods == null)
				{
					Hashtable hashtable = new Hashtable();
					hashtable["Kiosk.SerQuiosc2.Service1:System.String Version()"] = "Write1_Version";
					hashtable["Kiosk.SerQuiosc2.Service1:System.String Version():InHeaders"] = "Write2_VersionInHeaders";
					hashtable["Kiosk.SerQuiosc2.Service1:Boolean SaveFile(System.String, System.String, System.String, Byte[])"] = "Write3_SaveFile";
					hashtable["Kiosk.SerQuiosc2.Service1:Boolean SaveFile(System.String, System.String, System.String, Byte[]):InHeaders"] = "Write4_SaveFileInHeaders";
					hashtable["Kiosk.SerQuiosc2.Service1:Byte[] loadFile(System.String, System.String)"] = "Write5_loadFile";
					hashtable["Kiosk.SerQuiosc2.Service1:Byte[] loadFile(System.String, System.String):InHeaders"] = "Write6_loadFileInHeaders";
					if (writeMethods == null)
					{
						writeMethods = hashtable;
					}
				}
				return writeMethods;
			}
		}

		public override Hashtable TypedSerializers
		{
			get
			{
				if (typedSerializers == null)
				{
					Hashtable hashtable = new Hashtable();
					hashtable.Add("Kiosk.SerQuiosc2.Service1:System.String Version():InHeaders", new ArrayOfObjectSerializer2());
					hashtable.Add("Kiosk.SerQuiosc2.Service1:Boolean SaveFile(System.String, System.String, System.String, Byte[]):Response", new ArrayOfObjectSerializer5());
					hashtable.Add("Kiosk.SerQuiosc2.Service1:System.String Version():OutHeaders", new ArrayOfObjectSerializer3());
					hashtable.Add("Kiosk.SerQuiosc2.Service1:Byte[] loadFile(System.String, System.String)", new ArrayOfObjectSerializer8());
					hashtable.Add("Kiosk.SerQuiosc2.Service1:Boolean SaveFile(System.String, System.String, System.String, Byte[]):OutHeaders", new ArrayOfObjectSerializer7());
					hashtable.Add("Kiosk.SerQuiosc2.Service1:Byte[] loadFile(System.String, System.String):InHeaders", new ArrayOfObjectSerializer10());
					hashtable.Add("Kiosk.SerQuiosc2.Service1:Boolean SaveFile(System.String, System.String, System.String, Byte[])", new ArrayOfObjectSerializer4());
					hashtable.Add("Kiosk.SerQuiosc2.Service1:System.String Version():Response", new ArrayOfObjectSerializer1());
					hashtable.Add("Kiosk.SerQuiosc2.Service1:Boolean SaveFile(System.String, System.String, System.String, Byte[]):InHeaders", new ArrayOfObjectSerializer6());
					hashtable.Add("Kiosk.SerQuiosc2.Service1:Byte[] loadFile(System.String, System.String):Response", new ArrayOfObjectSerializer9());
					hashtable.Add("Kiosk.SerQuiosc2.Service1:Byte[] loadFile(System.String, System.String):OutHeaders", new ArrayOfObjectSerializer11());
					hashtable.Add("Kiosk.SerQuiosc2.Service1:System.String Version()", new ArrayOfObjectSerializer());
					if (typedSerializers == null)
					{
						typedSerializers = hashtable;
					}
				}
				return typedSerializers;
			}
		}

		public override bool CanSerialize(Type type)
		{
			if ((object)type == typeof(Service1))
			{
				return true;
			}
			return false;
		}

		public override XmlSerializer GetSerializer(Type type)
		{
			return null;
		}
	}
}
