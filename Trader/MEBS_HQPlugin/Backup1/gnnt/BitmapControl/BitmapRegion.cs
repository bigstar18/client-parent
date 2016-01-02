// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.BitmapControl.BitmapRegion
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Gnnt.MEBS.HQClient.gnnt.BitmapControl
{
  public class BitmapRegion
  {
    public static void CreateControlRegion(Control control, Bitmap bitmap)
    {
      if (control == null || bitmap == null)
        return;
      control.Width = bitmap.Width;
      control.Height = bitmap.Height;
      if (control is Form)
      {
        Form form = (Form) control;
        form.Width = control.Width;
        form.Height = control.Height;
        form.FormBorderStyle = FormBorderStyle.None;
        form.BackgroundImage = (Image) bitmap;
        GraphicsPath path = BitmapRegion.CalculateControlGraphicsPath(bitmap);
        form.Region = new Region(path);
      }
      else
      {
        if (!(control is Button))
          return;
        Button button = (Button) control;
        button.Text = "";
        button.Cursor = Cursors.Hand;
        button.BackgroundImage = (Image) bitmap;
        GraphicsPath path = BitmapRegion.CalculateControlGraphicsPath(bitmap);
        button.Region = new Region(path);
      }
    }

    private static GraphicsPath CalculateControlGraphicsPath(Bitmap bitmap)
    {
      GraphicsPath graphicsPath = new GraphicsPath();
      Color pixel = bitmap.GetPixel(0, 0);
      int num = 0;
      for (int y = 0; y < bitmap.Height; ++y)
      {
        num = 0;
        for (int x1 = 0; x1 < bitmap.Width; ++x1)
        {
          if (bitmap.GetPixel(x1, y) != pixel)
          {
            int x2 = x1;
            int x3 = x2;
            while (x3 < bitmap.Width && !(bitmap.GetPixel(x3, y) == pixel))
              ++x3;
            graphicsPath.AddRectangle(new Rectangle(x2, y, x3 - x2, 1));
            x1 = x3;
          }
        }
      }
      return graphicsPath;
    }
  }
}
