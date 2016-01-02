namespace FuturesTrade.Gnnt.Library
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal class MessageBoxEx
    {
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, string[] buttonTitles, MessageBoxIcon icon)
        {
            DummyForm owner = new DummyForm(buttons, buttonTitles)
            {
                TopMost = true
            };
            owner.Show();
            owner.WatchForActivate = true;
            DialogResult result = MessageBox.Show(owner, text, caption, buttons, icon);
            owner.Close();
            return result;
        }

        public static void test()
        {
            Show("提示消息", "提示标题", MessageBoxButtons.YesNoCancel, new string[] { "按钮一(&O)", "按钮三(&H)" }, MessageBoxIcon.Asterisk);
        }

        private class DummyForm : Form
        {
            private MessageBoxButtons _buttons;
            private string[] _buttonTitles;
            private IntPtr _handle;
            private bool _watchForActivate;

            public DummyForm(MessageBoxButtons buttons, string[] buttonTitles)
            {
                this._buttons = buttons;
                this._buttonTitles = buttonTitles;
                this.Text = "";
                base.StartPosition = FormStartPosition.Manual;
                base.Location = new Point(-32000, -32000);
                base.ShowInTaskbar = false;
            }

            private void CheckMsgbox()
            {
                if ((this._buttonTitles != null) && (this._buttonTitles.Length != 0))
                {
                    int index = 0;
                    for (IntPtr ptr = NativeWin32API.GetWindow(this._handle, 5); ptr != IntPtr.Zero; ptr = NativeWin32API.GetWindow(ptr, 2))
                    {
                        if (NativeWin32API.GetWindowClassName(ptr).Equals("Button") && (this._buttonTitles.Length > index))
                        {
                            NativeWin32API.SetWindowText(ptr, this._buttonTitles[index]);
                            index++;
                        }
                    }
                }
            }

            protected override void OnShown(EventArgs e)
            {
                base.OnShown(e);
                NativeWin32API.SetWindowPos(base.Handle, IntPtr.Zero, 0, 0, 0, 0, 0x293);
            }

            protected override void WndProc(ref Message m)
            {
                if (this._watchForActivate && (m.Msg == 6))
                {
                    this._watchForActivate = false;
                    this._handle = m.LParam;
                    this.CheckMsgbox();
                }
                base.WndProc(ref m);
            }

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
        }
    }
}
