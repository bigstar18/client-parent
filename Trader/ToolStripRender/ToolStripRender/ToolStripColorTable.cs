using System;
using System.Drawing;
namespace ToolStripRender
{
	public class ToolStripColorTable
	{
		private static Color _base = Color.FromArgb(105, 200, 254);
		private static Color _border = Color.FromArgb(194, 169, 120);
		private static Color _backNormal = Color.FromArgb(250, 250, 250);
		private static Color _backHover = Color.FromArgb(255, 201, 15);
		private static Color _backPressed = Color.FromArgb(226, 176, 0);
		private static Color _fore = Color.FromArgb(21, 66, 139);
		private static Color _dropDownImageBack = Color.FromArgb(233, 238, 238);
		private static Color _dropDownImageSeparator = Color.FromArgb(197, 197, 197);
		public virtual Color Base
		{
			get
			{
				return ToolStripColorTable._base;
			}
			set
			{
				ToolStripColorTable._base = value;
			}
		}
		public virtual Color Border
		{
			get
			{
				return ToolStripColorTable._border;
			}
			set
			{
				ToolStripColorTable._border = value;
			}
		}
		public virtual Color BackNormal
		{
			get
			{
				return ToolStripColorTable._backNormal;
			}
			set
			{
				ToolStripColorTable._backNormal = value;
			}
		}
		public virtual Color BackHover
		{
			get
			{
				return ToolStripColorTable._backHover;
			}
			set
			{
				ToolStripColorTable._backHover = value;
			}
		}
		public virtual Color BackPressed
		{
			get
			{
				return ToolStripColorTable._backPressed;
			}
			set
			{
				ToolStripColorTable._backPressed = value;
			}
		}
		public virtual Color Fore
		{
			get
			{
				return ToolStripColorTable._fore;
			}
			set
			{
				ToolStripColorTable._fore = value;
			}
		}
		public virtual Color DropDownImageBack
		{
			get
			{
				return ToolStripColorTable._dropDownImageBack;
			}
			set
			{
				ToolStripColorTable._dropDownImageBack = value;
			}
		}
		public virtual Color DropDownImageSeparator
		{
			get
			{
				return ToolStripColorTable._dropDownImageSeparator;
			}
			set
			{
				ToolStripColorTable._dropDownImageSeparator = value;
			}
		}
	}
}
