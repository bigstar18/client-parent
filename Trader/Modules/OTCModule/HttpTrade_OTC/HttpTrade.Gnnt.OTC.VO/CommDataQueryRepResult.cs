using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class CommDataQueryRepResult
	{
		private string RETCODE;
		private string MESSAGE;
		public long RetCode
		{
			get
			{
				return Tools.StrToLong(this.RETCODE);
			}
			set
			{
				this.RETCODE = value.ToString();
			}
		}
		public string RetMessage
		{
			get
			{
				return this.MESSAGE;
			}
			set
			{
				this.MESSAGE = value;
			}
		}
	}
}
