using System;
namespace TradeInterface.Gnnt.DataVO
{
	public class OrderQueryPagingRequestVO
	{
		public string UserID = string.Empty;
		public string MarketID = string.Empty;
		public short BuySell;
		public long OrderNO;
		public string CommodityID = string.Empty;
		public int StartNum;
		public int RecordCount;
		public long UpdateTime;
		public string SortFld;
		public short IsDesc;
		public int CurrentPagNum;
		public short IsQueryAll;
		public string Pri;
		public short Type;
		public short Sta;
	}
}
