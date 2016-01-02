using AppUpdate;
using System;
using System.Windows.Forms;
namespace AppUpdaterCom
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			int arg_0F_0 = Environment.OSVersion.Version.Major;
			CheckForUpdate checkForUpdate = new CheckForUpdate("UpdateList.xml");
			if (checkForUpdate.StartCheck() && MessageBox.Show("检查到新的更新，是否更新程序！", "程序更新", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
			{
				checkForUpdate.StartUpdate();
			}
		}
	}
}
