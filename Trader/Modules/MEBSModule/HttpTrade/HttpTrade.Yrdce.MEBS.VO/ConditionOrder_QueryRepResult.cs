using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class ConditionOrder_QueryRepResult
	{
		private string RETCODE;
		private string MESSAGE;
		private ConditionOrderTotal TOTALROW;
		private string UT;
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
		public ConditionOrderTotal TotalRow
		{
			get
			{
				return this.TOTALROW;
			}
		}
		public long UpdateTime
		{
			get
			{
				return Tools.StrToLong(this.UT);
			}
		}
	}
}
