using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class FirmFundsInfoRepResult
	{
		private string RETCODE;
		private string MESSAGE;
		public long RetCode
		{
			get
			{
				return Tools.StrToLong(this.RETCODE);
			}
		}
		public string RetMessage
		{
			get
			{
				return this.MESSAGE;
			}
		}
	}
}
