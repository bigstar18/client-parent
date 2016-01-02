using System;
using System.Runtime.InteropServices;
using System.Text;
using TPME.Log;
namespace TradeClientApp.Gnnt.OTC.Library
{
	public class NativeWin32API
	{
		public const int GW_CHILD = 5;
		public const int GW_HWNDNEXT = 2;
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int Width, int Height, int flags);
		[DllImport("user32.dll")]
		public static extern IntPtr GetWindow(IntPtr hWnd, int wCmd);
		[DllImport("user32.dll")]
		public static extern bool SetWindowText(IntPtr hWnd, string lpString);
		[DllImport("user32.dll")]
		public static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);
		public static string GetWindowClassName(IntPtr handle)
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			try
			{
				NativeWin32API.GetClassNameW(handle, stringBuilder, stringBuilder.Capacity);
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return stringBuilder.ToString();
		}
	}
}
