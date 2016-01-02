using System;
using System.Drawing;
using System.Windows.Forms;
using TPME.Log;
namespace TradeClientApp.Gnnt.OTC.Library
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
				try
				{
					this._buttons = buttons;
					this._buttonTitles = buttonTitles;
					this.Text = "";
					base.StartPosition = FormStartPosition.Manual;
					base.Location = new Point(-32000, -32000);
					base.ShowInTaskbar = false;
				}
				catch (Exception ex)
				{
					Logger.wirte(ex);
				}
			}
			protected override void OnShown(EventArgs e)
			{
				try
				{
					base.OnShown(e);
					NativeWin32API.SetWindowPos(base.Handle, IntPtr.Zero, 0, 0, 0, 0, 659);
				}
				catch (Exception ex)
				{
					Logger.wirte(ex);
				}
			}
			protected override void WndProc(ref Message m)
			{
				try
				{
					if (this._watchForActivate && m.Msg == 6)
					{
						this._watchForActivate = false;
						this._handle = m.LParam;
						this.CheckMsgbox();
					}
					base.WndProc(ref m);
				}
				catch (Exception ex)
				{
					Logger.wirte(ex);
				}
			}
			private void CheckMsgbox()
			{
				try
				{
					if (this._buttonTitles != null && this._buttonTitles.Length != 0)
					{
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
				catch (Exception ex)
				{
					Logger.wirte(ex);
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
			DialogResult result = DialogResult.Cancel;
			try
			{
				MessageBoxEx.DummyForm dummyForm = new MessageBoxEx.DummyForm(buttons, buttonTitles);
				dummyForm.TopMost = true;
				dummyForm.Show();
				dummyForm.WatchForActivate = true;
				result = MessageBox.Show(dummyForm, text, caption, buttons, icon);
				dummyForm.Close();
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
			return result;
		}
	}
}
