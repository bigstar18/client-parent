using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TPME.Log;
namespace ToolsLibrary.util
{
	public class BitmapRegion
	{
		public static void CreateControlRegion(Control control, Bitmap bitmap)
		{
			try
			{
				if (control != null && bitmap != null)
				{
					control.Width = bitmap.Width;
					control.Height = bitmap.Height;
					if (control is Form)
					{
						Form form = (Form)control;
						form.Width = control.Width;
						form.Height = control.Height;
						form.FormBorderStyle = FormBorderStyle.None;
						form.BackgroundImage = bitmap;
						GraphicsPath path = BitmapRegion.CalculateControlGraphicsPath(bitmap);
						form.Region = new Region(path);
					}
					else if (control is Button)
					{
						Button button = (Button)control;
						button.Text = "";
						button.Cursor = Cursors.Hand;
						button.BackgroundImage = bitmap;
						GraphicsPath path2 = BitmapRegion.CalculateControlGraphicsPath(bitmap);
						button.Region = new Region(path2);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.Message);
			}
		}
		private static GraphicsPath CalculateControlGraphicsPath(Bitmap bitmap)
		{
			GraphicsPath graphicsPath = null;
			try
			{
				graphicsPath = new GraphicsPath();
				Color pixel = bitmap.GetPixel(0, 0);
				for (int i = 0; i < bitmap.Height; i++)
				{
					for (int j = 0; j < bitmap.Width; j++)
					{
						if (bitmap.GetPixel(j, i) != pixel)
						{
							int num = j;
							int num2 = num;
							while (num2 < bitmap.Width && !(bitmap.GetPixel(num2, i) == pixel))
							{
								num2++;
							}
							graphicsPath.AddRectangle(new Rectangle(num, i, num2 - num, 1));
							j = num2;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.Message);
			}
			return graphicsPath;
		}
	}
}
