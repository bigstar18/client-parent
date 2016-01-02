namespace FuturesTrade.Gnnt.Library
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;
    using TabTest;

    public class BitmapRegion
    {
        private static GraphicsPath CalculateControlGraphicsPath(Bitmap bitmap)
        {
            GraphicsPath path = new GraphicsPath();
            Color pixel = bitmap.GetPixel(0, 0);
            int x = 0;
            for (int i = 0; i < bitmap.Height; i++)
            {
                x = 0;
                for (int j = 0; j < bitmap.Width; j++)
                {
                    if (!(bitmap.GetPixel(j, i) != pixel))
                    {
                        continue;
                    }
                    x = j;
                    int num4 = j;
                    num4 = x;
                    while (num4 < bitmap.Width)
                    {
                        if (bitmap.GetPixel(num4, i) == pixel)
                        {
                            break;
                        }
                        num4++;
                    }
                    path.AddRectangle(new Rectangle(x, i, num4 - x, 1));
                    j = num4;
                }
            }
            return path;
        }

        public static void CreateControlRegion(Control control, Bitmap bitmap)
        {
            if ((control != null) && (bitmap != null))
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
                    GraphicsPath path = CalculateControlGraphicsPath(bitmap);
                    form.Region = new Region(path);
                }
                else if (control is MyButton)
                {
                    MyButton button = (MyButton)control;
                    button.Text = "";
                    button.Cursor = Cursors.Hand;
                    button.BackgroundImage = bitmap;
                    GraphicsPath path2 = CalculateControlGraphicsPath(bitmap);
                    button.Region = new Region(path2);
                }
            }
        }
    }
}
