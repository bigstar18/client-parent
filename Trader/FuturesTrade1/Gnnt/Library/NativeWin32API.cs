namespace FuturesTrade.Gnnt.Library
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public class NativeWin32API
    {
        public const int GW_CHILD = 5;
        public const int GW_HWNDNEXT = 2;

        [DllImport("user32.dll")]
        public static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, int wCmd);
        public static string GetWindowClassName(IntPtr handle)
        {
            StringBuilder lpString = new StringBuilder(0x100);
            GetClassNameW(handle, lpString, lpString.Capacity);
            return lpString.ToString();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int Width, int Height, int flags);
        [DllImport("user32.dll")]
        public static extern bool SetWindowText(IntPtr hWnd, string lpString);
    }
}
