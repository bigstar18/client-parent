using System;
using System.Drawing;
using System.Windows.Forms;
namespace ToolsLibrary.util
{
	public class ScaleForm
	{
		private Graphics Graphic;
		public static void ScaleForms(Form form)
		{
			Graphics graphics = Graphics.FromHwnd(form.Handle);
			double num = (double)(graphics.DpiX - 96f) * 1.1;
			float emSize = Tools.StrToFloat(((double)form.Font.Size / (1.0 + num / 100.0)).ToString());
			Font font = new Font("宋体", emSize);
			form.Font = font;
		}
		public Size ScaleSize(Size size)
		{
			int height = size.Height;
			int width = size.Width;
			float num = (float)(height / 15) * (1440f / this.Graphic.DpiX);
			float num2 = (float)(width / 15) * (1440f / this.Graphic.DpiX);
			int width2 = Tools.StrToInt(num.ToString());
			int height2 = Tools.StrToInt(num2.ToString());
			Size result = new Size(width2, height2);
			return result;
		}
		public Point ScalePoint(Point p)
		{
			float num = (float)(p.X / 15) * (1440f / this.Graphic.DpiX);
			float num2 = (float)(p.Y / 15) * (1440f / this.Graphic.DpiX);
			Point result = new Point((int)num, (int)num2);
			return result;
		}
	}
}
