using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TPME.Log;
namespace TradeClientApp.Gnnt.OTC.Library
{
	public class LocalHook
	{
		public delegate int HookProc(int nCode, int wParam, IntPtr lParam);
		public delegate void KeyProc();
		[StructLayout(LayoutKind.Sequential)]
		public class POINT
		{
			public int x;
			public int y;
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
		public class KeyboardHookStruct
		{
			public int vkCode;
			public int scanCode;
			public int flags;
			public int time;
			public int dwExtraInfo;
		}
		public const int WH_MOUSE_LL = 14;
		public const int WH_KEYBOARD_LL = 13;
		public const int WH_MOUSE = 7;
		public const int WH_KEYBOARD = 2;
		private const int WM_MOUSEMOVE = 512;
		private const int WM_LBUTTONDOWN = 513;
		private const int WM_RBUTTONDOWN = 516;
		private const int WM_MBUTTONDOWN = 519;
		private const int WM_LBUTTONUP = 514;
		private const int WM_RBUTTONUP = 517;
		private const int WM_MBUTTONUP = 520;
		private const int WM_LBUTTONDBLCLK = 515;
		private const int WM_RBUTTONDBLCLK = 518;
		private const int WM_MBUTTONDBLCLK = 521;
		private const int WM_MOUSEWHEEL = 522;
		private const int WM_KEYDOWN = 256;
		private const int WM_KEYUP = 257;
		private const int WM_SYSKEYDOWN = 260;
		private const int WM_SYSKEYUP = 261;
		private static int hMouseHook;
		private static int hKeyboardHook;
		private LocalHook.HookProc MouseHookProcedure;
		private LocalHook.HookProc KeyboardHookProcedure;
		public LocalHook.KeyProc keyDown;
		private LocalHook.POINT pt = new LocalHook.POINT();
		public event MouseEventHandler OnMouseActivity;
		public event KeyEventHandler KeyDown;
		public event KeyPressEventHandler KeyPress;
		public event KeyEventHandler KeyUp;
		public LocalHook()
		{
			this.Start();
		}
		~LocalHook()
		{
			this.Stop();
		}
		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
		public static extern int SetWindowsHookEx(int idHook, LocalHook.HookProc lpfn, IntPtr hInstance, int threadId);
		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
		public static extern bool UnhookWindowsHookEx(int idHook);
		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
		public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);
		public void Start()
		{
			try
			{
				if (LocalHook.hMouseHook == 0)
				{
					this.MouseHookProcedure = new LocalHook.HookProc(this.MouseHookProc);
					LocalHook.hMouseHook = LocalHook.SetWindowsHookEx(7, this.MouseHookProcedure, (IntPtr)0, AppDomain.GetCurrentThreadId());
					if (LocalHook.hMouseHook == 0)
					{
						this.Stop();
						throw new Exception("SetWindowsHookEx failed.");
					}
				}
				if (LocalHook.hKeyboardHook == 0)
				{
					this.KeyboardHookProcedure = new LocalHook.HookProc(this.KeyboardHookProc);
					LocalHook.hKeyboardHook = LocalHook.SetWindowsHookEx(2, this.KeyboardHookProcedure, (IntPtr)0, AppDomain.GetCurrentThreadId());
					if (LocalHook.hKeyboardHook == 0)
					{
						this.Stop();
						throw new Exception("SetWindowsHookEx ist failed.");
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		public void Stop()
		{
			try
			{
				bool flag = true;
				bool flag2 = true;
				if (LocalHook.hMouseHook != 0)
				{
					flag = LocalHook.UnhookWindowsHookEx(LocalHook.hMouseHook);
					LocalHook.hMouseHook = 0;
				}
				if (LocalHook.hKeyboardHook != 0)
				{
					flag2 = LocalHook.UnhookWindowsHookEx(LocalHook.hKeyboardHook);
					LocalHook.hKeyboardHook = 0;
				}
				if (!flag || !flag2)
				{
					throw new Exception("UnhookWindowsHookEx failed.");
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
		private int MouseHookProc(int nCode, int wParam, IntPtr lParam)
		{
			bool flag = false;
			LocalHook.MouseHookStruct mouseHookStruct = (LocalHook.MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(LocalHook.MouseHookStruct));
			try
			{
				if (nCode >= 0 && this.OnMouseActivity != null)
				{
					MouseButtons mouseButtons = MouseButtons.None;
					switch (wParam)
					{
					case 512:
						if (mouseHookStruct.pt.x != this.pt.x || mouseHookStruct.pt.y != this.pt.y)
						{
							this.pt = mouseHookStruct.pt;
							flag = true;
						}
						break;
					case 513:
						mouseButtons = MouseButtons.Left;
						flag = true;
						break;
					case 514:
					case 515:
						break;
					case 516:
						mouseButtons = MouseButtons.Right;
						flag = true;
						break;
					default:
						if (wParam == 522)
						{
							flag = true;
						}
						break;
					}
					int clicks = 0;
					if (mouseButtons != MouseButtons.None)
					{
						if (wParam == 515 || wParam == 518)
						{
							clicks = 2;
						}
						else
						{
							clicks = 1;
						}
					}
					MouseEventArgs e = new MouseEventArgs(mouseButtons, clicks, mouseHookStruct.pt.x, mouseHookStruct.pt.y, 0);
					if (flag)
					{
						this.OnMouseActivity(this, e);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return LocalHook.CallNextHookEx(LocalHook.hMouseHook, nCode, wParam, lParam);
		}
		[DllImport("user32")]
		public static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);
		[DllImport("user32")]
		public static extern int GetKeyboardState(byte[] pbKeyState);
		private int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
		{
			try
			{
				if (nCode >= 0 && (this.KeyDown != null || this.KeyUp != null || this.KeyPress != null))
				{
					KeyEventArgs e = new KeyEventArgs((Keys)wParam);
					if (this.KeyDown != null)
					{
						this.KeyDown(this, e);
					}
					else if (this.KeyUp != null)
					{
						this.KeyUp(this, e);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.Message.ToString());
			}
			return LocalHook.CallNextHookEx(LocalHook.hKeyboardHook, nCode, wParam, lParam);
		}
	}
}
