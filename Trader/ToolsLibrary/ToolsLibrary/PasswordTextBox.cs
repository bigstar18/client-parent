using System;
using System.Windows.Forms;
namespace ToolsLibrary
{
	public class PasswordTextBox : TextBox
	{
		private const int WM_SETTEXT = 12;
		private const int WM_GETTEXT = 13;
		private bool checkpass;
		public bool CheckPass
		{
			get
			{
				return this.checkpass;
			}
			set
			{
				this.checkpass = value;
			}
		}
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 13 && !this.checkpass)
			{
				return;
			}
			base.WndProc(ref m);
		}
	}
}
