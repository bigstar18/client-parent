using System;
namespace TradeInterface.Gnnt.DataVO
{
	public class ConditionQueryRequestVO
	{
		public string UserID = string.Empty;
		public string CustomerID = string.Empty;
		public short BuySell;
		public long OrderNO;
		public string MarketID = string.Empty;
		public string CommodityID = string.Empty;
		public long Session_ID;
		public int StartNum;
		public int RecordCount;
		public string SortField;
		public short ISDesc;
		public long UpdateTime;
		public short SettleBasis;
		public string OrderStatus;
		public short ConditionType;
		public int PageNumber;
	}
}
