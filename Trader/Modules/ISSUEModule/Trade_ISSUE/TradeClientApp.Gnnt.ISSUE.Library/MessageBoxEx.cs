using System;
using System.Drawing;
using System.Windows.Forms;
namespace TradeClientApp.Gnnt.ISSUE.Library
{
	internal class MessageBoxEx
	{
		private class DummyForm : Form
		{
			private IntPtr _handle;
			private MessageBoxButtons _buttons;
			private string[] _buttonTitles;
			private bool _watchForActivate;
			public bool WatchForActivate
			{
				get
				{
					return this._watchForActivate;
				}
				set
				{
					this._watchForActivate = value;
				}
			}
			public DummyForm(MessageBoxButtons buttons, string[] buttonTitles)
			{
				this._buttons = buttons;
				this._buttonTitles = buttonTitles;
				this.Text = "";
				base.StartPosition = FormStartPosition.Manual;
				base.Location = new Point(-32000, -32000);
				base.ShowInTaskbar = false;
			}
			protected override void OnShown(EventArgs e)
			{
				base.OnShown(e);
				NativeWin32API.SetWindowPos(base.Handle, IntPtr.Zero, 0, 0, 0, 0, 659);
			}
			protected override void WndProc(ref Message m)
			{
				if (this._watchForActivate && m.Msg == 6)
				{
					this._watchForActivate = false;
					this._handle = m.LParam;
					this.CheckMsgbox();
				}
				base.WndProc(ref m);
			}
			private void CheckMsgbox()
			{
				if (this._buttonTitles == null || this._buttonTitles.Length == 0)
				{
					return;
				}
				int num = 0;
				IntPtr window = NativeWin32API.GetWindow(this._handle, 5);
				while (window != IntPtr.Zero)
				{
					if (NativeWin32API.GetWindowClassName(window).Equals("Button") && this._buttonTitles.Length > num)
					{
						NativeWin32API.SetWindowText(window, this._buttonTitles[num]);
						num++;
					}
					window = NativeWin32API.GetWindow(window, 2);
				}
			}
		}
		public static void test()
		{
			MessageBoxEx.Show("提示消息", "提示标题", MessageBoxButtons.YesNoCancel, new string[]
			{
				"按钮一(&O)",
				"按钮三(&H)"
			}, MessageBoxIcon.Asterisk);
		}
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, string[] buttonTitles, MessageBoxIcon icon)
		{
			MessageBoxEx.DummyForm dummyForm = new MessageBoxEx.DummyForm(buttons, buttonTitles);
			dummyForm.TopMost = true;
			dummyForm.Show();
			dummyForm.WatchForActivate = true;
			DialogResult result = MessageBox.Show(dummyForm, text, caption, buttons, icon);
			dummyForm.Close();
			return result;
		}
	}
}
