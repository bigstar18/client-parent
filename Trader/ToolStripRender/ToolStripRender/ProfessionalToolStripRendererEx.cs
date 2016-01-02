using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace ToolStripRender
{
	public class ProfessionalToolStripRendererEx : ToolStripRenderer
	{
		private static readonly int OffsetMargin = 24;
		private string _menuLogoString = "更多精彩尽在 www.ToolStripRender.com";
		private ToolStripColorTable _colorTable;
		public string MenuLogoString
		{
			get
			{
				return this._menuLogoString;
			}
			set
			{
				this._menuLogoString = value;
			}
		}
		protected virtual ToolStripColorTable ColorTable
		{
			get
			{
				if (this._colorTable == null)
				{
					this._colorTable = new ToolStripColorTable();
				}
				return this._colorTable;
			}
		}
		public ProfessionalToolStripRendererEx()
		{
		}
		public ProfessionalToolStripRendererEx(ToolStripColorTable colorTable)
		{
			this._colorTable = colorTable;
		}
		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			ToolStrip toolStrip = e.ToolStrip;
			Graphics graphics = e.Graphics;
			Rectangle affectedBounds = e.AffectedBounds;
			if (toolStrip is ToolStripDropDown)
			{
				RegionHelper.CreateRegion(toolStrip, affectedBounds);
				using (SolidBrush solidBrush = new SolidBrush(this.ColorTable.BackNormal))
				{
					graphics.FillRectangle(solidBrush, affectedBounds);
					return;
				}
			}
			LinearGradientMode mode = (toolStrip.Orientation == Orientation.Horizontal) ? LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
			RenderHelper.RenderBackgroundInternal(graphics, affectedBounds, this.ColorTable.Base, this.ColorTable.Border, this.ColorTable.BackNormal, RoundStyle.None, 0, 0.35f, false, false, mode);
		}
		protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
		{
			ToolStrip toolStrip = e.ToolStrip;
			Graphics graphics = e.Graphics;
			Rectangle affectedBounds = e.AffectedBounds;
			if (toolStrip is ToolStripDropDown)
			{
				bool flag = this.NeedDrawLogo(toolStrip);
				bool flag2 = toolStrip.RightToLeft == RightToLeft.Yes;
				Rectangle rect = affectedBounds;
				rect.Width = ProfessionalToolStripRendererEx.OffsetMargin;
				if (flag)
				{
					Rectangle rect2 = affectedBounds;
					rect2.Width = ProfessionalToolStripRendererEx.OffsetMargin;
					if (flag2)
					{
						rect2.X -= 2;
						rect.X = rect2.X - ProfessionalToolStripRendererEx.OffsetMargin;
					}
					else
					{
						rect2.X += 2;
						rect.X = rect2.Right;
					}
					rect2.Y++;
					rect2.Height -= 2;
					using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rect2, this.ColorTable.BackHover, this.ColorTable.BackNormal, 90f))
					{
						linearGradientBrush.Blend = new Blend
						{
							Positions = new float[]
							{
								0f,
								0.2f,
								1f
							},
							Factors = new float[]
							{
								0f,
								0.1f,
								0.9f
							}
						};
						rect2.Y++;
						rect2.Height -= 2;
						using (GraphicsPath graphicsPath = GraphicsPathHelper.CreatePath(rect2, 8, RoundStyle.All, false))
						{
							using (new SmoothingModeGraphics(graphics))
							{
								graphics.FillPath(linearGradientBrush, graphicsPath);
							}
						}
					}
					StringFormat stringFormat = new StringFormat(StringFormatFlags.NoWrap);
					Font font = new Font(toolStrip.Font.FontFamily, 11f, FontStyle.Bold);
					stringFormat.Alignment = StringAlignment.Near;
					stringFormat.LineAlignment = StringAlignment.Center;
					stringFormat.Trimming = StringTrimming.EllipsisCharacter;
					graphics.TranslateTransform((float)rect2.X, (float)rect2.Bottom);
					graphics.RotateTransform(270f);
					if (!string.IsNullOrEmpty(this.MenuLogoString))
					{
						Rectangle r = new Rectangle(0, 0, rect2.Height, rect2.Width);
						using (Brush brush = new SolidBrush(this.ColorTable.Fore))
						{
							using (new TextRenderingHintGraphics(graphics))
							{
								graphics.DrawString(this.MenuLogoString, font, brush, r, stringFormat);
							}
						}
					}
					graphics.ResetTransform();
				}
				else if (flag2)
				{
					rect.X -= 3;
				}
				else
				{
					rect.X += 3;
				}
				rect.Y += 2;
				rect.Height -= 4;
				using (SolidBrush solidBrush = new SolidBrush(this.ColorTable.DropDownImageBack))
				{
					graphics.FillRectangle(solidBrush, rect);
				}
				Point pt;
				Point pt2;
				if (flag2)
				{
					pt = new Point(rect.X, rect.Y);
					pt2 = new Point(rect.X, rect.Bottom);
				}
				else
				{
					pt = new Point(rect.Right - 1, rect.Y);
					pt2 = new Point(rect.Right - 1, rect.Bottom);
				}
				using (Pen pen = new Pen(this.ColorTable.DropDownImageSeparator))
				{
					graphics.DrawLine(pen, pt, pt2);
					return;
				}
			}
			base.OnRenderImageMargin(e);
		}
		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			ToolStrip toolStrip = e.ToolStrip;
			Graphics graphics = e.Graphics;
			Rectangle affectedBounds = e.AffectedBounds;
			if (toolStrip is ToolStripDropDown)
			{
				using (new SmoothingModeGraphics(graphics))
				{
					using (GraphicsPath graphicsPath = GraphicsPathHelper.CreatePath(affectedBounds, 8, RoundStyle.All, true))
					{
						using (Pen pen = new Pen(this.ColorTable.DropDownImageSeparator))
						{
							graphicsPath.Widen(pen);
							graphics.DrawPath(pen, graphicsPath);
						}
					}
				}
				if (toolStrip is ToolStripOverflow)
				{
					return;
				}
				affectedBounds.Inflate(-1, -1);
				using (GraphicsPath graphicsPath2 = GraphicsPathHelper.CreatePath(affectedBounds, 8, RoundStyle.All, true))
				{
					using (Pen pen2 = new Pen(this.ColorTable.BackNormal))
					{
						graphics.DrawPath(pen2, graphicsPath2);
					}
					return;
				}
			}
			base.OnRenderToolStripBorder(e);
		}
		protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
		{
			ToolStrip toolStrip = e.ToolStrip;
			ToolStripItem item = e.Item;
			if (!item.Enabled)
			{
				return;
			}
			Graphics graphics = e.Graphics;
			Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);
			if (toolStrip is MenuStrip)
			{
				LinearGradientMode mode = (toolStrip.Orientation == Orientation.Horizontal) ? LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
				if (item.Selected)
				{
					RenderHelper.RenderBackgroundInternal(graphics, rect, this.ColorTable.BackHover, this.ColorTable.Border, this.ColorTable.BackNormal, RoundStyle.All, true, true, mode);
					return;
				}
				if (item.Pressed)
				{
					RenderHelper.RenderBackgroundInternal(graphics, rect, this.ColorTable.BackPressed, this.ColorTable.Border, this.ColorTable.BackNormal, RoundStyle.All, true, true, mode);
					return;
				}
				base.OnRenderMenuItemBackground(e);
				return;
			}
			else
			{
				if (!(toolStrip is ToolStripDropDown))
				{
					base.OnRenderMenuItemBackground(e);
					return;
				}
				int num = this.NeedDrawLogo(toolStrip) ? ProfessionalToolStripRendererEx.OffsetMargin : 0;
				if (item.RightToLeft == RightToLeft.Yes)
				{
					rect.X += 4;
				}
				else
				{
					rect.X += num + 4;
				}
				rect.Width -= num + 8;
				rect.Height--;
				if (item.Selected)
				{
					RenderHelper.RenderBackgroundInternal(graphics, rect, this.ColorTable.BackHover, this.ColorTable.Border, this.ColorTable.BackNormal, RoundStyle.All, true, true, LinearGradientMode.Vertical);
					return;
				}
				base.OnRenderMenuItemBackground(e);
				return;
			}
		}
		protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
		{
			ToolStrip toolStrip = e.ToolStrip;
			Graphics graphics = e.Graphics;
			if (toolStrip is ToolStripDropDown && e.Item is ToolStripMenuItem)
			{
				int num = this.NeedDrawLogo(toolStrip) ? ProfessionalToolStripRendererEx.OffsetMargin : 0;
				ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)e.Item;
				if (toolStripMenuItem.Checked)
				{
					return;
				}
				Rectangle imageRectangle = e.ImageRectangle;
				if (e.Item.RightToLeft == RightToLeft.Yes)
				{
					imageRectangle.X -= num + 2;
				}
				else
				{
					imageRectangle.X += num + 2;
				}
				using (new InterpolationModeGraphics(graphics))
				{
					ToolStripItemImageRenderEventArgs e2 = new ToolStripItemImageRenderEventArgs(graphics, e.Item, e.Image, imageRectangle);
					base.OnRenderItemImage(e2);
					return;
				}
			}
			base.OnRenderItemImage(e);
		}
		protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
		{
			ToolStrip toolStrip = e.ToolStrip;
			e.TextColor = this.ColorTable.Fore;
			if (toolStrip is ToolStripDropDown && e.Item is ToolStripMenuItem)
			{
				int num = this.NeedDrawLogo(toolStrip) ? 18 : 0;
				Rectangle textRectangle = e.TextRectangle;
				if (e.Item.RightToLeft == RightToLeft.Yes)
				{
					textRectangle.X -= num;
				}
				else
				{
					textRectangle.X += num;
				}
				e.TextRectangle = textRectangle;
			}
			base.OnRenderItemText(e);
		}
		protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
		{
			ToolStrip toolStrip = e.ToolStrip;
			Graphics graphics = e.Graphics;
			if (toolStrip is ToolStripDropDown && e.Item is ToolStripMenuItem)
			{
				int num = this.NeedDrawLogo(toolStrip) ? ProfessionalToolStripRendererEx.OffsetMargin : 0;
				Rectangle imageRectangle = e.ImageRectangle;
				if (e.Item.RightToLeft == RightToLeft.Yes)
				{
					imageRectangle.X -= num + 2;
				}
				else
				{
					imageRectangle.X += num + 2;
				}
				imageRectangle.Width = 13;
				imageRectangle.Y++;
				imageRectangle.Height -= 3;
				using (new SmoothingModeGraphics(graphics))
				{
					using (GraphicsPath graphicsPath = new GraphicsPath())
					{
						graphicsPath.AddRectangle(imageRectangle);
						using (PathGradientBrush pathGradientBrush = new PathGradientBrush(graphicsPath))
						{
							pathGradientBrush.CenterColor = this.ColorTable.DropDownImageBack;
							pathGradientBrush.SurroundColors = new Color[]
							{
								this.ColorTable.BackPressed
							};
							pathGradientBrush.Blend = new Blend
							{
								Positions = new float[]
								{
									0f,
									0.3f,
									1f
								},
								Factors = new float[]
								{
									0f,
									0.5f,
									1f
								}
							};
							graphics.FillRectangle(pathGradientBrush, imageRectangle);
						}
					}
					using (Pen pen = new Pen(this.ColorTable.BackPressed))
					{
						graphics.DrawRectangle(pen, imageRectangle);
					}
					ControlPaintEx.DrawCheckedFlag(graphics, imageRectangle, this.ColorTable.Fore);
					return;
				}
			}
			base.OnRenderItemCheck(e);
		}
		protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
		{
			if (e.Item.Enabled)
			{
				e.ArrowColor = this.ColorTable.Fore;
			}
			ToolStrip owner = e.Item.Owner;
			if (owner is ToolStripDropDown && e.Item is ToolStripMenuItem)
			{
				int num = this.NeedDrawLogo(owner) ? 3 : 0;
				Rectangle arrowRectangle = e.ArrowRectangle;
				if (e.Item.RightToLeft == RightToLeft.Yes)
				{
					arrowRectangle.X -= num;
				}
				else
				{
					arrowRectangle.X += num;
				}
				e.ArrowRectangle = arrowRectangle;
			}
			base.OnRenderArrow(e);
		}
		protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
		{
			ToolStrip toolStrip = e.ToolStrip;
			Rectangle contentRectangle = e.Item.ContentRectangle;
			Graphics graphics = e.Graphics;
			Color baseColor = this.ColorTable.Base;
			if (toolStrip is ToolStripDropDown)
			{
				int num = this.NeedDrawLogo(toolStrip) ? (ProfessionalToolStripRendererEx.OffsetMargin * 2) : ProfessionalToolStripRendererEx.OffsetMargin;
				if (e.Item.RightToLeft != RightToLeft.Yes)
				{
					contentRectangle.X += num + 2;
				}
				contentRectangle.Width -= num + 4;
				baseColor = this.ColorTable.DropDownImageSeparator;
			}
			this.RenderSeparatorLine(graphics, contentRectangle, baseColor, this.ColorTable.BackNormal, Color.Snow, e.Vertical);
		}
		protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
		{
			ToolStrip toolStrip = e.ToolStrip;
			ToolStripButton toolStripButton = e.Item as ToolStripButton;
			Graphics graphics = e.Graphics;
			if (toolStripButton != null)
			{
				LinearGradientMode mode = (toolStrip.Orientation == Orientation.Horizontal) ? LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
				SmoothingModeGraphics smoothingModeGraphics = new SmoothingModeGraphics(graphics);
				Rectangle rectangle = new Rectangle(Point.Empty, toolStripButton.Size);
				if (toolStripButton.BackgroundImage != null)
				{
					Rectangle clipRect = toolStripButton.Selected ? toolStripButton.ContentRectangle : rectangle;
					ControlPaintEx.DrawBackgroundImage(graphics, toolStripButton.BackgroundImage, this.ColorTable.BackNormal, toolStripButton.BackgroundImageLayout, rectangle, clipRect);
				}
				if (toolStripButton.CheckState == CheckState.Unchecked)
				{
					if (toolStripButton.Selected)
					{
						Color baseColor = this.ColorTable.BackHover;
						if (toolStripButton.Pressed)
						{
							baseColor = this.ColorTable.BackPressed;
						}
						RenderHelper.RenderBackgroundInternal(graphics, rectangle, baseColor, this.ColorTable.Border, this.ColorTable.BackNormal, RoundStyle.All, true, true, mode);
						goto IL_177;
					}
					if (!(toolStrip is ToolStripOverflow))
					{
						goto IL_177;
					}
					using (Brush brush = new SolidBrush(this.ColorTable.BackHover))
					{
						graphics.FillRectangle(brush, rectangle);
						goto IL_177;
					}
				}
				Color baseColor2 = ControlPaint.Light(this.ColorTable.BackHover);
				if (toolStripButton.Selected)
				{
					baseColor2 = this.ColorTable.BackHover;
				}
				if (toolStripButton.Pressed)
				{
					baseColor2 = this.ColorTable.BackPressed;
				}
				RenderHelper.RenderBackgroundInternal(graphics, rectangle, baseColor2, this.ColorTable.Border, this.ColorTable.BackNormal, RoundStyle.All, true, true, mode);
				IL_177:
				smoothingModeGraphics.Dispose();
			}
		}
		protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
		{
			ToolStrip toolStrip = e.ToolStrip;
			ToolStripDropDownItem toolStripDropDownItem = e.Item as ToolStripDropDownItem;
			if (toolStripDropDownItem != null)
			{
				LinearGradientMode mode = (toolStrip.Orientation == Orientation.Horizontal) ? LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
				Graphics graphics = e.Graphics;
				SmoothingModeGraphics smoothingModeGraphics = new SmoothingModeGraphics(graphics);
				Rectangle rect = new Rectangle(Point.Empty, toolStripDropDownItem.Size);
				if (toolStripDropDownItem.Pressed && toolStripDropDownItem.HasDropDownItems)
				{
					RenderHelper.RenderBackgroundInternal(graphics, rect, this.ColorTable.BackPressed, this.ColorTable.Border, this.ColorTable.BackNormal, RoundStyle.All, true, true, mode);
				}
				else if (toolStripDropDownItem.Selected)
				{
					RenderHelper.RenderBackgroundInternal(graphics, rect, this.ColorTable.BackHover, this.ColorTable.Border, this.ColorTable.BackNormal, RoundStyle.All, true, true, mode);
				}
				else
				{
					if (toolStrip is ToolStripOverflow)
					{
						using (Brush brush = new SolidBrush(this.ColorTable.BackNormal))
						{
							graphics.FillRectangle(brush, rect);
							goto IL_F6;
						}
					}
					base.OnRenderDropDownButtonBackground(e);
				}
				IL_F6:
				smoothingModeGraphics.Dispose();
			}
		}
		protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
		{
			ToolStrip toolStrip = e.ToolStrip;
			ToolStripSplitButton toolStripSplitButton = e.Item as ToolStripSplitButton;
			if (toolStripSplitButton == null)
			{
				base.OnRenderSplitButtonBackground(e);
				return;
			}
			Graphics graphics = e.Graphics;
			LinearGradientMode mode = (toolStrip.Orientation == Orientation.Horizontal) ? LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
			Rectangle rectangle = new Rectangle(Point.Empty, toolStripSplitButton.Size);
			new SmoothingModeGraphics(graphics);
			Color arrowColor = toolStrip.Enabled ? this.ColorTable.Fore : SystemColors.ControlDark;
			if (toolStripSplitButton.BackgroundImage != null)
			{
				Rectangle clipRect = toolStripSplitButton.Selected ? toolStripSplitButton.ContentRectangle : rectangle;
				ControlPaintEx.DrawBackgroundImage(graphics, toolStripSplitButton.BackgroundImage, this.ColorTable.BackNormal, toolStripSplitButton.BackgroundImageLayout, rectangle, clipRect);
			}
			if (toolStripSplitButton.ButtonPressed)
			{
				Rectangle rectangle2 = toolStripSplitButton.ButtonBounds;
				Padding padding = (toolStripSplitButton.RightToLeft == RightToLeft.Yes) ? new Padding(0, 1, 1, 1) : new Padding(1, 1, 0, 1);
				rectangle2 = LayoutUtils.DeflateRect(rectangle2, padding);
				RenderHelper.RenderBackgroundInternal(graphics, rectangle, this.ColorTable.BackHover, this.ColorTable.Border, this.ColorTable.BackNormal, RoundStyle.All, true, true, mode);
				rectangle2.Inflate(-1, -1);
				graphics.SetClip(rectangle2);
				RenderHelper.RenderBackgroundInternal(graphics, rectangle2, this.ColorTable.BackPressed, this.ColorTable.Border, this.ColorTable.BackNormal, RoundStyle.Left, false, true, mode);
				graphics.ResetClip();
				using (Pen pen = new Pen(this.ColorTable.Border))
				{
					graphics.DrawLine(pen, toolStripSplitButton.SplitterBounds.Left, toolStripSplitButton.SplitterBounds.Top, toolStripSplitButton.SplitterBounds.Left, toolStripSplitButton.SplitterBounds.Bottom);
				}
				base.DrawArrow(new ToolStripArrowRenderEventArgs(graphics, toolStripSplitButton, toolStripSplitButton.DropDownButtonBounds, arrowColor, ArrowDirection.Down));
				return;
			}
			if (toolStripSplitButton.Pressed || toolStripSplitButton.DropDownButtonPressed)
			{
				RenderHelper.RenderBackgroundInternal(graphics, rectangle, this.ColorTable.BackPressed, this.ColorTable.Border, this.ColorTable.BackNormal, RoundStyle.All, true, true, mode);
				base.DrawArrow(new ToolStripArrowRenderEventArgs(graphics, toolStripSplitButton, toolStripSplitButton.DropDownButtonBounds, arrowColor, ArrowDirection.Down));
				return;
			}
			if (toolStripSplitButton.Selected)
			{
				RenderHelper.RenderBackgroundInternal(graphics, rectangle, this.ColorTable.BackHover, this.ColorTable.Border, this.ColorTable.BackNormal, RoundStyle.All, true, true, mode);
				using (Pen pen2 = new Pen(this.ColorTable.Border))
				{
					graphics.DrawLine(pen2, toolStripSplitButton.SplitterBounds.Left, toolStripSplitButton.SplitterBounds.Top, toolStripSplitButton.SplitterBounds.Left, toolStripSplitButton.SplitterBounds.Bottom);
				}
				base.DrawArrow(new ToolStripArrowRenderEventArgs(graphics, toolStripSplitButton, toolStripSplitButton.DropDownButtonBounds, arrowColor, ArrowDirection.Down));
				return;
			}
			base.DrawArrow(new ToolStripArrowRenderEventArgs(graphics, toolStripSplitButton, toolStripSplitButton.DropDownButtonBounds, arrowColor, ArrowDirection.Down));
		}
		protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
		{
			ToolStripItem item = e.Item;
			ToolStrip toolStrip = e.ToolStrip;
			Graphics graphics = e.Graphics;
			bool flag = item.RightToLeft == RightToLeft.Yes;
			new SmoothingModeGraphics(graphics);
			this.RenderOverflowBackground(e, flag);
			bool flag2 = toolStrip.Orientation == Orientation.Horizontal;
			Rectangle empty = Rectangle.Empty;
			if (flag)
			{
				empty = new Rectangle(0, item.Height - 8, 10, 5);
			}
			else
			{
				empty = new Rectangle(item.Width - 12, item.Height - 8, 10, 5);
			}
			ArrowDirection direction = flag2 ? ArrowDirection.Down : ArrowDirection.Right;
			int x = (flag && flag2) ? -1 : 1;
			empty.Offset(x, 1);
			Color color = toolStrip.Enabled ? this.ColorTable.Fore : SystemColors.ControlDark;
			using (Brush brush = new SolidBrush(color))
			{
				RenderHelper.RenderArrowInternal(graphics, empty, direction, brush);
			}
			if (flag2)
			{
				using (Pen pen = new Pen(color))
				{
					graphics.DrawLine(pen, empty.Right - 8, empty.Y - 2, empty.Right - 2, empty.Y - 2);
					graphics.DrawLine(pen, empty.Right - 8, empty.Y - 1, empty.Right - 2, empty.Y - 1);
					return;
				}
			}
			using (Pen pen2 = new Pen(color))
			{
				graphics.DrawLine(pen2, empty.X, empty.Y, empty.X, empty.Bottom - 1);
				graphics.DrawLine(pen2, empty.X, empty.Y + 1, empty.X, empty.Bottom);
			}
		}
		protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
		{
			if (e.GripStyle == ToolStripGripStyle.Visible)
			{
				Rectangle gripBounds = e.GripBounds;
				bool flag = e.GripDisplayStyle == ToolStripGripDisplayStyle.Vertical;
				ToolStrip toolStrip = e.ToolStrip;
				Graphics graphics = e.Graphics;
				if (flag)
				{
					gripBounds.X = e.AffectedBounds.X;
					gripBounds.Width = e.AffectedBounds.Width;
					if (toolStrip is MenuStrip)
					{
						if (e.AffectedBounds.Height > e.AffectedBounds.Width)
						{
							flag = false;
							gripBounds.Y = e.AffectedBounds.Y;
						}
						else
						{
							toolStrip.GripMargin = new Padding(0, 2, 0, 2);
							gripBounds.Y = e.AffectedBounds.Y;
							gripBounds.Height = e.AffectedBounds.Height;
						}
					}
					else
					{
						toolStrip.GripMargin = new Padding(2, 2, 4, 2);
						gripBounds.X++;
						gripBounds.Width++;
					}
				}
				else
				{
					gripBounds.Y = e.AffectedBounds.Y;
					gripBounds.Height = e.AffectedBounds.Height;
				}
				this.DrawDottedGrip(graphics, gripBounds, flag, false, this.ColorTable.BackNormal, ControlPaint.Dark(this.ColorTable.Base, 0.3f));
			}
		}
		protected override void OnRenderStatusStripSizingGrip(ToolStripRenderEventArgs e)
		{
			this.DrawSolidStatusGrip(e.Graphics, e.AffectedBounds, this.ColorTable.BackNormal, ControlPaint.Dark(this.ColorTable.Base, 0.3f));
		}
		internal void RenderSeparatorLine(Graphics g, Rectangle rect, Color baseColor, Color backColor, Color shadowColor, bool vertical)
		{
			if (vertical)
			{
				rect.Y += 2;
				rect.Height -= 4;
				using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rect, baseColor, backColor, LinearGradientMode.Vertical))
				{
					using (Pen pen = new Pen(linearGradientBrush))
					{
						g.DrawLine(pen, rect.X, rect.Y, rect.X, rect.Bottom);
					}
					return;
				}
			}
			using (LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush(rect, baseColor, backColor, 180f))
			{
				linearGradientBrush2.Blend = new Blend
				{
					Positions = new float[]
					{
						0f,
						0.2f,
						0.5f,
						0.8f,
						1f
					},
					Factors = new float[]
					{
						1f,
						0.3f,
						0f,
						0.3f,
						1f
					}
				};
				using (Pen pen2 = new Pen(linearGradientBrush2))
				{
					g.DrawLine(pen2, rect.X, rect.Y, rect.Right, rect.Y);
					linearGradientBrush2.LinearColors = new Color[]
					{
						shadowColor,
						backColor
					};
					pen2.Brush = linearGradientBrush2;
					g.DrawLine(pen2, rect.X, rect.Y + 1, rect.Right, rect.Y + 1);
				}
			}
		}
		internal void RenderOverflowBackground(ToolStripItemRenderEventArgs e, bool rightToLeft)
		{
			Color baseColor = Color.Empty;
			Graphics graphics = e.Graphics;
			ToolStrip toolStrip = e.ToolStrip;
			ToolStripOverflowButton toolStripOverflowButton = e.Item as ToolStripOverflowButton;
			Rectangle rectangle = new Rectangle(Point.Empty, toolStripOverflowButton.Size);
			Rectangle withinBounds = rectangle;
			bool flag = !(toolStripOverflowButton.GetCurrentParent() is MenuStrip);
			bool flag2 = toolStrip.Orientation == Orientation.Horizontal;
			if (flag2)
			{
				rectangle.X += rectangle.Width - 12 + 1;
				rectangle.Width = 12;
				if (rightToLeft)
				{
					rectangle = LayoutUtils.RTLTranslate(rectangle, withinBounds);
				}
			}
			else
			{
				rectangle.Y = rectangle.Height - 12 + 1;
				rectangle.Height = 12;
			}
			if (toolStripOverflowButton.Pressed)
			{
				baseColor = this.ColorTable.BackPressed;
			}
			else if (toolStripOverflowButton.Selected)
			{
				baseColor = this.ColorTable.BackHover;
			}
			else
			{
				baseColor = this.ColorTable.Base;
			}
			if (flag)
			{
				using (Pen pen = new Pen(this.ColorTable.Base))
				{
					Point pt = new Point(rectangle.Left - 1, rectangle.Height - 2);
					Point pt2 = new Point(rectangle.Left, rectangle.Height - 2);
					if (rightToLeft)
					{
						pt.X = rectangle.Right + 1;
						pt2.X = rectangle.Right;
					}
					graphics.DrawLine(pen, pt, pt2);
				}
			}
			LinearGradientMode mode = flag2 ? LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
			RenderHelper.RenderBackgroundInternal(graphics, rectangle, baseColor, this.ColorTable.Border, this.ColorTable.BackNormal, RoundStyle.None, 0, 0.35f, false, false, mode);
			if (flag)
			{
				using (Brush brush = new SolidBrush(this.ColorTable.Base))
				{
					if (flag2)
					{
						Point point = new Point(rectangle.X - 2, 0);
						Point point2 = new Point(rectangle.X - 1, 1);
						if (rightToLeft)
						{
							point.X = rectangle.Right + 1;
							point2.X = rectangle.Right;
						}
						graphics.FillRectangle(brush, point.X, point.Y, 1, 1);
						graphics.FillRectangle(brush, point2.X, point2.Y, 1, 1);
					}
					else
					{
						graphics.FillRectangle(brush, rectangle.Width - 3, rectangle.Top - 1, 1, 1);
						graphics.FillRectangle(brush, rectangle.Width - 2, rectangle.Top - 2, 1, 1);
					}
				}
				using (Brush brush2 = new SolidBrush(this.ColorTable.Base))
				{
					if (flag2)
					{
						Rectangle rect = new Rectangle(rectangle.X - 1, 0, 1, 1);
						if (rightToLeft)
						{
							rect.X = rectangle.Right;
						}
						graphics.FillRectangle(brush2, rect);
					}
					else
					{
						graphics.FillRectangle(brush2, rectangle.X, rectangle.Top - 1, 1, 1);
					}
				}
			}
		}
		private void DrawDottedGrip(Graphics g, Rectangle bounds, bool vertical, bool largeDot, Color innerColor, Color outerColor)
		{
			bounds.Height -= 3;
			Point location = new Point(bounds.X, bounds.Y);
			Rectangle bounds2 = new Rectangle(0, 0, 2, 2);
			using (new SmoothingModeGraphics(g))
			{
				if (vertical)
				{
					int num = bounds.Height;
					location.Y += 8;
					int num2 = 0;
					while (location.Y > 4)
					{
						location.Y = num - (2 + num2);
						if (largeDot)
						{
							bounds2.Location = location;
							this.DrawCircle(g, bounds2, outerColor, innerColor);
						}
						else
						{
							int crColor = ColorTranslator.ToWin32(innerColor);
							int crColor2 = ColorTranslator.ToWin32(outerColor);
							IntPtr hdc = g.GetHdc();
							ProfessionalToolStripRendererEx.SetPixel(hdc, location.X, location.Y, crColor);
							ProfessionalToolStripRendererEx.SetPixel(hdc, location.X + 1, location.Y, crColor2);
							ProfessionalToolStripRendererEx.SetPixel(hdc, location.X, location.Y + 1, crColor2);
							ProfessionalToolStripRendererEx.SetPixel(hdc, location.X + 3, location.Y, crColor);
							ProfessionalToolStripRendererEx.SetPixel(hdc, location.X + 4, location.Y, crColor2);
							ProfessionalToolStripRendererEx.SetPixel(hdc, location.X + 3, location.Y + 1, crColor2);
							g.ReleaseHdc(hdc);
						}
						num2 += 4;
					}
				}
				else
				{
					bounds.Inflate(-2, 0);
					int num = bounds.Width;
					location.X += 2;
					int num3 = 1;
					while (location.X > 0)
					{
						location.X = num - (2 + num3);
						if (largeDot)
						{
							bounds2.Location = location;
							this.DrawCircle(g, bounds2, outerColor, innerColor);
						}
						else
						{
							int crColor3 = ColorTranslator.ToWin32(innerColor);
							int crColor4 = ColorTranslator.ToWin32(outerColor);
							IntPtr hdc = g.GetHdc();
							ProfessionalToolStripRendererEx.SetPixel(hdc, location.X, location.Y, crColor3);
							ProfessionalToolStripRendererEx.SetPixel(hdc, location.X + 1, location.Y, crColor4);
							ProfessionalToolStripRendererEx.SetPixel(hdc, location.X, location.Y + 1, crColor4);
							ProfessionalToolStripRendererEx.SetPixel(hdc, location.X + 3, location.Y, crColor3);
							ProfessionalToolStripRendererEx.SetPixel(hdc, location.X + 4, location.Y, crColor4);
							ProfessionalToolStripRendererEx.SetPixel(hdc, location.X + 3, location.Y + 1, crColor4);
							g.ReleaseHdc(hdc);
						}
						num3 += 4;
					}
				}
			}
		}
		private void DrawCircle(Graphics g, Rectangle bounds, Color borderColor, Color fillColor)
		{
			using (GraphicsPath graphicsPath = new GraphicsPath())
			{
				graphicsPath.AddEllipse(bounds);
				graphicsPath.CloseFigure();
				using (Pen pen = new Pen(borderColor))
				{
					g.DrawPath(pen, graphicsPath);
				}
				using (Brush brush = new SolidBrush(fillColor))
				{
					g.FillPath(brush, graphicsPath);
				}
			}
		}
		private void DrawDottedStatusGrip(Graphics g, Rectangle bounds, Color innerColor, Color outerColor)
		{
			Rectangle bounds2 = new Rectangle(0, 0, 2, 2);
			bounds2.X = bounds.Width - 17;
			bounds2.Y = bounds.Height - 8;
			using (new SmoothingModeGraphics(g))
			{
				this.DrawCircle(g, bounds2, outerColor, innerColor);
				bounds2.X = bounds.Width - 12;
				this.DrawCircle(g, bounds2, outerColor, innerColor);
				bounds2.X = bounds.Width - 7;
				this.DrawCircle(g, bounds2, outerColor, innerColor);
				bounds2.Y = bounds.Height - 13;
				this.DrawCircle(g, bounds2, outerColor, innerColor);
				bounds2.Y = bounds.Height - 18;
				this.DrawCircle(g, bounds2, outerColor, innerColor);
				bounds2.Y = bounds.Height - 13;
				bounds2.X = bounds.Width - 12;
				this.DrawCircle(g, bounds2, outerColor, innerColor);
			}
		}
		private void DrawSolidStatusGrip(Graphics g, Rectangle bounds, Color innerColor, Color outerColor)
		{
			using (new SmoothingModeGraphics(g))
			{
				using (Pen pen = new Pen(innerColor))
				{
					using (Pen pen2 = new Pen(outerColor))
					{
						g.DrawLine(pen2, new Point(bounds.Width - 14, bounds.Height - 6), new Point(bounds.Width - 4, bounds.Height - 16));
						g.DrawLine(pen, new Point(bounds.Width - 13, bounds.Height - 6), new Point(bounds.Width - 4, bounds.Height - 15));
						g.DrawLine(pen2, new Point(bounds.Width - 12, bounds.Height - 6), new Point(bounds.Width - 4, bounds.Height - 14));
						g.DrawLine(pen, new Point(bounds.Width - 11, bounds.Height - 6), new Point(bounds.Width - 4, bounds.Height - 13));
						g.DrawLine(pen2, new Point(bounds.Width - 10, bounds.Height - 6), new Point(bounds.Width - 4, bounds.Height - 12));
						g.DrawLine(pen, new Point(bounds.Width - 9, bounds.Height - 6), new Point(bounds.Width - 4, bounds.Height - 11));
						g.DrawLine(pen2, new Point(bounds.Width - 8, bounds.Height - 6), new Point(bounds.Width - 4, bounds.Height - 10));
						g.DrawLine(pen, new Point(bounds.Width - 7, bounds.Height - 6), new Point(bounds.Width - 4, bounds.Height - 9));
						g.DrawLine(pen2, new Point(bounds.Width - 6, bounds.Height - 6), new Point(bounds.Width - 4, bounds.Height - 8));
						g.DrawLine(pen, new Point(bounds.Width - 5, bounds.Height - 6), new Point(bounds.Width - 4, bounds.Height - 7));
					}
				}
			}
		}
		internal bool NeedDrawLogo(ToolStrip toolStrip)
		{
			ToolStripDropDown toolStripDropDown = toolStrip as ToolStripDropDown;
			return (toolStripDropDown.OwnerItem != null && toolStripDropDown.OwnerItem.Owner is MenuStrip) || toolStrip is ContextMenuStrip;
		}
		[DllImport("gdi32.dll")]
		private static extern uint SetPixel(IntPtr hdc, int X, int Y, int crColor);
	}
}
