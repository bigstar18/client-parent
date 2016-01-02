using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
namespace YrdceClient.Yrdce.Common.Library
{
	public class WinSendMessage
	{
		public const int WM_SYSCOMMAND = 274;
		public const int SC_MOVE = 61456;
		public const int HTCAPTION = 2;
		public const int WM_COPYDATA = 74;
		[DllImport("user32.dll")]
		private static extern bool ReleaseCapture();
		[DllImport("user32.dll")]
		private static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
		[DllImport("User32.dll")]
		private static extern int FindWindow(string lpClassName, string lpWindowName);
		[DllImport("User32.dll")]
		private static extern int SendMessage(int hWnd, int Msg, IntPtr wParam, ref CopyDataStruct lParam);
		public static bool SendMessageToMoveForm(IntPtr handle)
		{
			WinSendMessage.ReleaseCapture();
			return WinSendMessage.SendMessage(handle, 274, 61458, 0);
		}
		public static bool SendMessageByProcessName(string destProcessName, string tradeID, IntPtr fromWindowHandler)
		{
			if (tradeID == null)
			{
				return false;
			}
			bool result = false;
			Process[] processesByName = Process.GetProcessesByName(destProcessName);
			Process[] array = processesByName;
			for (int i = 0; i < array.Length; i++)
			{
				Process process = array[i];
				int num = process.MainWindowHandle.ToInt32();
				if (num != 0)
				{
					Process currentProcess = Process.GetCurrentProcess();
					if (num != currentProcess.MainWindowHandle.ToInt32())
					{
						CopyDataStruct copyDataStruct = default(CopyDataStruct);
						copyDataStruct.dwData = (IntPtr)1;
						copyDataStruct.str = tradeID;
						copyDataStruct.cbData = Encoding.Default.GetBytes(tradeID).Length + 1;
						int num2 = WinSendMessage.SendMessage(num, 74, fromWindowHandler, ref copyDataStruct);
						if (num2 == 1)
						{
							return true;
						}
					}
				}
			}
			return result;
		}
	}
}
