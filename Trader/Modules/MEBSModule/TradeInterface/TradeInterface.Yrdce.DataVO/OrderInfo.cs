using System;
namespace TradeInterface.Gnnt.DataVO
{
	public class OrderInfo
	{
		public long OrderNO;
		public string Time = string.Empty;
		public short State;
		public short BuySell;
		public short SettleBasis;
		public string TraderID = string.Empty;
		public string FirmID = string.Empty;
		public string CustomerID = string.Empty;
		public string MarketID = string.Empty;
		public string CommodityID = string.Empty;
		public double OrderPrice;
		public long OrderQuantity;
		public double Balance;
		public double LPrice;
		public string WithDrawTime = string.Empty;
		public short CBasis;
		public short BillTradeType;
	}
}
