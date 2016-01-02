using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace FrmObject
{
    public class CommonClass
    {
        public const int WS_SYSMENU = 524288;
        public const int WS_MINIMIZEBOX = 131072;
        public const int WM_SYSCOMMAND = 274;
        public const int SC_MOVE = 61456;
        public const int HTCAPTION = 2;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowLong(HandleRef hWnd, int nIndex);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SetWindowLong(HandleRef hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public static void SetTaskMenu(Form form)
        {
            int windowLong = CommonClass.GetWindowLong(new HandleRef(form, form.Handle), -16);
            CommonClass.SetWindowLong(new HandleRef(form, form.Handle), -16, windowLong | 524288 | 131072);
        }
    }
}
