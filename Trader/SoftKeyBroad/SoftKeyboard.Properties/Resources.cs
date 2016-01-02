using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
namespace SoftKeyboard.Properties
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
					ResourceManager resourceManager = new ResourceManager("SoftKeyboard.Properties.Resources", typeof(Resources).Assembly);
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
		internal static Bitmap bgimage
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("bgimage", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}
		internal static Bitmap button
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("button", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}
		internal static Bitmap close
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("close", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}
		internal static Bitmap close1
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("close1", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}
		internal static Bitmap close2
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("close2", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}
		internal static Bitmap onfocus
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("onfocus", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}
		internal Resources()
		{
		}
	}
}
