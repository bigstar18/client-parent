using AxGNNTKEYLib;
using PluginInterface;
using System;
using System.Drawing;
namespace SysFrame.Gnnt.Common.Operation
{
	public class LogonOperationInfo
	{
		public string username;
		public string password;
		public short loginmark;
		public bool isMemoryChecked;
		public bool isProtectChecked;
		public string verifycode;
		public Image verifyCodeImage;
		public string verifyCodeString;
		public AxGnntKey axgnntkey;
		public IPlugin myPlugin;
	}
}
