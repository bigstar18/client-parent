using System;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class LogonVO
	{
		public int sessionID;
		public string message = string.Empty;
		public int gmlevel;
		public string lastIP = string.Empty;
		public string lastTime = string.Empty;
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"\r\nsessionID:",
				this.sessionID,
				"\r\nmessage:",
				this.message,
				"\r\ngmlevel:",
				this.gmlevel,
				"\r\nlastIP:",
				this.lastIP,
				"\r\nlastTime:",
				this.lastTime
			});
		}
	}
}
