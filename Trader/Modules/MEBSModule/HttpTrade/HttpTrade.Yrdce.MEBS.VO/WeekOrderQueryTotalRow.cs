using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class WeekOrderQueryTotalRow
	{
		private string TOTALNUM;
		private string TOTALQTY;
		private string UNTRADEQTY;
		public int TotalNum
		{
			get
			{
				return Tools.StrToInt(this.TOTALNUM);
			}
		}
		public int TotalQty
		{
			get
			{
				return Tools.StrToInt(this.TOTALQTY);
			}
		}
		public int UnTradeQty
		{
			get
			{
				return Tools.StrToInt(this.UNTRADEQTY);
			}
		}
	}
}
