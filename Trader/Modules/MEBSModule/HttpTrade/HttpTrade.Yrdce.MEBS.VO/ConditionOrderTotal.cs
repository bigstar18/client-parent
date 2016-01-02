using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class ConditionOrderTotal
	{
		private string TOTALNUM;
		private string TOTALQTY;
		public long TotalNum
		{
			get
			{
				return Tools.StrToLong(this.TOTALNUM);
			}
		}
		public long TotalQty
		{
			get
			{
				return Tools.StrToLong(this.TOTALQTY);
			}
		}
	}
}
