using System;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class MarketStatusVO
	{
		public string marketID = string.Empty;
		public string code = string.Empty;
		public float cur;
		public byte status;
		public float value;
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"\r\ncode:",
				this.code,
				"\r\ncur:",
				this.cur,
				"\r\nstatus:",
				this.status,
				"\r\nvalue:",
				this.value
			});
		}
	}
}
