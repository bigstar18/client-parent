using System;
namespace TradeInterface.Gnnt.ISSUE.DataVO
{
	public class OrderQueryRequestVO
	{
		public string UserID = string.Empty;
		public string MarketID = string.Empty;
		public short BuySell;
		public long OrderNO;
		public string CommodityID = string.Empty;
		public int StartNum;
		public int RecordCount;
		public long UpdateTime;
	}
}
