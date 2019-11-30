using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace TurnKiosk.Properties
{
	[CompilerGenerated]
	[DebuggerNonUserCode]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	internal class Resources
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(resourceMan, null))
				{
					ResourceManager resourceManager = resourceMan = new ResourceManager("TurnKiosk.Properties.Resources", typeof(Resources).Assembly);
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		internal static Bitmap big_cancel
		{
			get
			{
				object @object = ResourceManager.GetObject("big_cancel", resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Bitmap big_ok
		{
			get
			{
				object @object = ResourceManager.GetObject("big_ok", resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Bitmap big_warnning
		{
			get
			{
				object @object = ResourceManager.GetObject("big_warnning", resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static string full => ResourceManager.GetString("full", resourceCulture);

		internal static string unlock => ResourceManager.GetString("unlock", resourceCulture);

		internal Resources()
		{
		}
	}
}
