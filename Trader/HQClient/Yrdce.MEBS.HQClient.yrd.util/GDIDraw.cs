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
			if (g != null)
			{
				IntPtr hdc = g.GetHdc();
				IntPtr hObject = GDIDraw.CreateSolidBrush(ColorTranslator.ToWin32(m_Color));
				GDIDraw.SelectObject(hdc, hObject);
				m_rc.X += scrollOffset.X;
				m_rc.Y += scrollOffset.Y;
				GDIDraw.SetROP2(hdc, drawingMode.R2_XORPEN);
				GDIDraw.MoveToEx(hdc, m_rc.X, m_rc.Y, IntPtr.Zero);
				GDIDraw.Rectangle(hdc, m_rc.X, m_rc.Y, m_rc.Width + m_rc.X, m_rc.Height + m_rc.Y);
				GDIDraw.DeleteObject(hObject);
				g.ReleaseHdc();
			}
		}
		public static void XorLine(Graphics g, int X1, int Y1, int X2, int Y2, Color m_Color, Point scrollOffset)
		{
			IntPtr hdc = g.GetHdc();
			IntPtr hObject = GDIDraw.CreatePen(0, 1, ColorTranslator.ToWin32(m_Color));
			GDIDraw.SelectObject(hdc, hObject);
			X1 += scrollOffset.X;
			Y1 += scrollOffset.Y;
			X2 += scrollOffset.X;
			Y2 += scrollOffset.Y;
			GDIDraw.SetROP2(hdc, drawingMode.R2_XORPEN);
			GDIDraw.MoveToEx(hdc, X1, Y1, IntPtr.Zero);
			GDIDraw.LineTo(hdc, X2, Y2);
			GDIDraw.DeleteObject(hObject);
			g.ReleaseHdc();
		}
	}
}
