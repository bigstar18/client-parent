using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace YrdceClient.Yrdce.Common.Library
{
	public class DrawOverflowButtonRenderer : ToolStripProfessionalRenderer
	{
		private Color ofPressedColor1;
		public DrawOverflowButtonRenderer()
		{
			this.ofPressedColor1 = Color.FromArgb(0, 125, 155);
		}
		protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
		{
			if (e.ToolStrip.OverflowButton.Enabled)
			{
				if (e.ToolStrip.OverflowButton.Pressed)
				{
					LinearGradientBrush linearGradientBrush = new LinearGradientBrush(e.ToolStrip.OverflowButton.Bounds, this.ofPressedColor1, this.ofPressedColor1, 0f);
					e.Graphics.FillRectangle(linearGradientBrush, e.Graphics.ClipBounds);
					linearGradientBrush.Dispose();
				}
				else if (e.ToolStrip.OverflowButton.Selected)
				{
					LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush(e.ToolStrip.OverflowButton.Bounds, this.ofPressedColor1, this.ofPressedColor1, 0f);
					e.Graphics.FillRectangle(linearGradientBrush2, e.Graphics.ClipBounds);
					linearGradientBrush2.Dispose();
				}
			}
			e.Graphics.DrawString(">>", new Font("宋体", 12f), Brushes.White, new RectangleF(0f, (float)e.ToolStrip.OverflowButton.Bounds.Height - e.Graphics.MeasureString(">>", new Font("宋体", 12f)).Height - 4f, 50f, (float)e.ToolStrip.OverflowButton.Bounds.Height));
		}
	}
}
