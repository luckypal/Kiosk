using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Kiosk.Properties
{
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
	[CompilerGenerated]
	internal sealed class Settings : ApplicationSettingsBase
	{
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

		public static Settings Default => defaultInstance;

		[ApplicationScopedSetting]
		[DefaultSettingValue("http://wsquioscv2.tbtlp.com/ServiceQuiosc.asmx")]
		[DebuggerNonUserCode]
		[SpecialSetting(SpecialSetting.WebServiceUrl)]
		public string Kiosk_SerQuiosc2_Service1 => (string)this["Kiosk_SerQuiosc2_Service1"];
	}
}
