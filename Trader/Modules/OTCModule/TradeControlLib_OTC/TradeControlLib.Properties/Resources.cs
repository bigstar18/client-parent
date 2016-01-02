using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
namespace TradeControlLib.Properties
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
	internal class Resources
	{
		private static ResourceManager resourceMan;
		private static CultureInfo resourceCulture;
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("TradeControlLib.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}
		internal static Bitmap _1
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("1", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}
		internal static Bitmap _2
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("2", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}
		internal static Bitmap bg
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("bg", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}
		internal static string buytext
		{
			get
			{
				return Resources.ResourceManager.GetString("buytext", Resources.resourceCulture);
			}
		}
		internal Resources()
		{
		}
	}
}
