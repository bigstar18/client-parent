// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.util.GDIDraw
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Gnnt.MEBS.HQClient.gnnt.util
{
  internal class GDIDraw
  {
    public const int ROP_SrcCopy = 13369376;

    [DllImport("gdi32")]
    public static extern int SetROP2(IntPtr hdc, drawingMode fnDrawMode);

    [DllImport("gdi32")]
    public static extern bool LineTo(IntPtr hdc, int nXEnd, int nYEnd);

    [DllImport("gdi32")]
    public static extern bool MoveToEx(IntPtr hdc, int X, int Y, IntPtr lpPoint);

    [DllImport("gdi32.dll")]
    public static extern IntPtr CreatePen(int nPenStyle, int nWidth, int crColor);

    [DllImport("gdi32.dll")]
    public static extern long SelectObject(IntPtr hdc, IntPtr hObject);

    [DllImport("gdi32.dll")]
    public static extern long DeleteObject(IntPtr hObject);

    [DllImport("gdi32.dll")]
    public static extern int Rectangle(IntPtr hdc, int X1, int Y1, int X2, int Y2);

    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateSolidBrush(int crColor);

    [DllImport("gdi32.dll")]
    public static extern int BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

    public static void XorRectangle(Graphics g, Rectangle m_rc, Color m_Color, Point scrollOffset)
    {
      if (g == null)
        return;
      IntPtr hdc = g.GetHdc();
      IntPtr solidBrush = GDIDraw.CreateSolidBrush(ColorTranslator.ToWin32(m_Color));
      GDIDraw.SelectObject(hdc, solidBrush);
      m_rc.X += scrollOffset.X;
      m_rc.Y += scrollOffset.Y;
      GDIDraw.SetROP2(hdc, drawingMode.R2_XORPEN);
      GDIDraw.MoveToEx(hdc, m_rc.X, m_rc.Y, IntPtr.Zero);
      GDIDraw.Rectangle(hdc, m_rc.X, m_rc.Y, m_rc.Width + m_rc.X, m_rc.Height + m_rc.Y);
      GDIDraw.DeleteObject(solidBrush);
      g.ReleaseHdc();
    }

    public static void XorLine(Graphics g, int X1, int Y1, int X2, int Y2, Color m_Color, Point scrollOffset)
    {
      IntPtr hdc = g.GetHdc();
      IntPtr pen = GDIDraw.CreatePen(0, 1, ColorTranslator.ToWin32(m_Color));
      GDIDraw.SelectObject(hdc, pen);
      X1 += scrollOffset.X;
      Y1 += scrollOffset.Y;
      X2 += scrollOffset.X;
      Y2 += scrollOffset.Y;
      GDIDraw.SetROP2(hdc, drawingMode.R2_XORPEN);
      GDIDraw.MoveToEx(hdc, X1, Y1, IntPtr.Zero);
      GDIDraw.LineTo(hdc, X2, Y2);
      GDIDraw.DeleteObject(pen);
      g.ReleaseHdc();
    }
  }
}
