using System;
namespace TradeInterface.Gnnt.DataVO
{
	public class TradeInfo
	{
		public long TradeNO;
		public long OrderNO;
		public string TradeTime = string.Empty;
		public short BuySell;
		public short SettleBasis;
		public string TraderID = string.Empty;
		public string FirmID = string.Empty;
		public string CustomerID = string.Empty;
		public string MarketID = string.Empty;
		public string CommodityID = string.Empty;
		public double TradePrice;
		public long TradeQuantity;
		public double TransferPrice;
		public double TransferPL;
		public double Comm;
		public long STradeNO;
		public long ATradeNO;
		public short TradeType;
	}
}
