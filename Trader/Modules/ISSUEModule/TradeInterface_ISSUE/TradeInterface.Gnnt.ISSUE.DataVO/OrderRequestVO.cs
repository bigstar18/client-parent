using System;
namespace TradeInterface.Gnnt.ISSUE.DataVO
{
	public class OrderRequestVO
	{
		public string UserID = string.Empty;
		public string CustomerID = string.Empty;
		public short BuySell;
		public string MarketID = string.Empty;
		public string CommodityID = string.Empty;
		public double Price;
		public long Quantity;
		public short SettleBasis;
		public short CloseMode;
		public short TimeFlag;
		public double LPrice;
		public short BillType;
	}
}
