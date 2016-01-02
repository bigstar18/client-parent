using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class OrderRepResult
	{
		private string RETCODE;
		private string MESSAGE;
		private string OR_N;
		private string TIME;
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
		public long OrderNo
		{
			get
			{
				return Tools.StrToLong(this.OR_N);
			}
		}
		public string Time
		{
			get
			{
				return this.TIME;
			}
		}
	}
}
