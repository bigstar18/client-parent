namespace FuturesTrade.Gnnt.Library
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class LocalHook
    {
        private static int hKeyboardHook;
        private static int hMouseHook;
        private HookProc KeyboardHookProcedure;
        private HookProc MouseHookProcedure;
        public const int WH_KEYBOARD = 2;
        public const int WH_KEYBOARD_LL = 13;
        public const int WH_MOUSE = 7;
        public const int WH_MOUSE_LL = 14;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_LBUTTONDBLCLK = 0x203;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_MBUTTONDBLCLK = 0x209;
        private const int WM_MBUTTONDOWN = 0x207;
        private const int WM_MBUTTONUP = 520;
        private const int WM_MOUSEMOVE = 0x200;
        private const int WM_RBUTTONDBLCLK = 0x206;
        private const int WM_RBUTTONDOWN = 0x204;
        private const int WM_RBUTTONUP = 0x205;
        private const int WM_SYSKEYDOWN = 260;
        private const int WM_SYSKEYUP = 0x105;

        public event KeyEventHandler KeyDown;

        public event KeyEventHandler KeyLeft;

        public event KeyPressEventHandler KeyPress;

        public event KeyEventHandler KeyUp;

        public event MouseEventHandler OnMouseActivity;

        public LocalHook()
        {
            this.Start();
        }

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);
        ~LocalHook()
        {
            this.Stop();
        }

        [DllImport("user32")]
        public static extern int GetKeyboardState(byte[] pbKeyState);
        private int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if ((nCode >= 0) && (((this.KeyDown != null) || (this.KeyUp != null)) || ((this.KeyPress != null) || (this.KeyLeft != null))))
            {
                Keys keyData = (Keys)wParam;
                KeyEventArgs e = new KeyEventArgs(keyData);
                if (this.KeyDown != null)
                {
                    this.KeyDown(this, e);
                }
                else if (this.KeyUp != null)
                {
                    this.KeyUp(this, e);
                }
                else if (this.KeyLeft != null)
                {
                    this.KeyLeft(this, e);
                }
            }
            return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
        }

        private int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if ((nCode >= 0) && (this.OnMouseActivity != null))
            {
                MouseButtons none = MouseButtons.None;
                switch (wParam)
                {
                    case 0x201:
                        none = MouseButtons.Left;
                        break;

                    case 0x204:
                        none = MouseButtons.Right;
                        break;
                }
                int clicks = 0;
                if (none != MouseButtons.None)
                {
                    if ((wParam == 0x203) || (wParam == 0x206))
                    {
                        clicks = 2;
                    }
                    else
                    {
                        clicks = 1;
                    }
                }
                MouseHookStruct struct2 = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
                MouseEventArgs e = new MouseEventArgs(none, clicks, struct2.pt.x, struct2.pt.y, 0);
                this.OnMouseActivity(this, e);
            }
            return CallNextHookEx(hMouseHook, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        public void Start()
        {
            if (hMouseHook == 0)
            {
                this.MouseHookProcedure = new HookProc(this.MouseHookProc);
                hMouseHook = SetWindowsHookEx(7, this.MouseHookProcedure, IntPtr.Zero, AppDomain.GetCurrentThreadId());
                if (hMouseHook == 0)
                {
                    this.Stop();
                    throw new Exception("SetWindowsHookEx failed.");
                }
            }
            if (hKeyboardHook == 0)
            {
                this.KeyboardHookProcedure = new HookProc(this.KeyboardHookProc);
                hKeyboardHook = SetWindowsHookEx(2, this.KeyboardHookProcedure, IntPtr.Zero, AppDomain.GetCurrentThreadId());
                if (hKeyboardHook == 0)
                {
                    this.Stop();
                    throw new Exception("SetWindowsHookEx ist failed.");
                }
            }
        }

        public void Stop()
        {
            bool flag = true;
            bool flag2 = true;
            if (hMouseHook != 0)
            {
                flag = UnhookWindowsHookEx(hMouseHook);
                hMouseHook = 0;
            }
            if (hKeyboardHook != 0)
            {
                flag2 = UnhookWindowsHookEx(hKeyboardHook);
                hKeyboardHook = 0;
            }
            if (!flag || !flag2)
            {
                throw new Exception("UnhookWindowsHookEx failed.");
            }
        }

        [DllImport("user32")]
        public static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        public delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public class KeyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public LocalHook.POINT pt;
            public int hwnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;
        }
    }
}
