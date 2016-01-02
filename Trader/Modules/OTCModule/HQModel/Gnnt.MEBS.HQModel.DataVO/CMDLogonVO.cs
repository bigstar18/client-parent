using Gnnt.MEBS.HQModel.Service.IO;
using System;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class CMDLogonVO : CMDVO
	{
		public string name = string.Empty;
		public string password = string.Empty;
		public string key = string.Empty;
		public CMDLogonVO()
		{
			this.cmd = 1;
		}
		public static LogonVO getObj(InputStreamConvert input)
		{
			return new LogonVO
			{
				sessionID = input.ReadJavaInt(),
				message = input.ReadJavaUTF(),
				gmlevel = input.ReadJavaInt(),
				lastIP = input.ReadJavaUTF(),
				lastTime = input.ReadJavaUTF()
			};
		}
	}
}
