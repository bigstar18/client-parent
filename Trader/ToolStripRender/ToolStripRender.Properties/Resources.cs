using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
namespace ToolStripRender.Properties
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
					ResourceManager resourceManager = new ResourceManager("ToolStripRender.Properties.Resources", typeof(Resources).Assembly);
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
		internal static Bitmap AlignTableCellMiddleLeftJustHS
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("AlignTableCellMiddleLeftJustHS", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}
		internal static Bitmap MoveToFolderHS
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("MoveToFolderHS", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}
		internal static Bitmap NewDocumentHS
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("NewDocumentHS", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}
		internal static Bitmap NewFolderHS
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("NewFolderHS", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}
		internal static Bitmap OpenHS
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("OpenHS", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}
		internal static Bitmap PrintHS
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("PrintHS", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}
		internal static Bitmap PrintSetupHS
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("PrintSetupHS", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}
		internal static Bitmap SaveAllHS
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("SaveAllHS", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}
		internal static Bitmap SaveHS
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("SaveHS", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}
		internal Resources()
		{
		}
	}
}
