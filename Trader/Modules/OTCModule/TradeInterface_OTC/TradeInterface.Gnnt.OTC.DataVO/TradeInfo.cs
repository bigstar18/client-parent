using System;
namespace TradeInterface.Gnnt.OTC.DataVO
{
	public class TradeInfo
	{
		public long TradeNO;
		public long OrderNO;
		public long HoldingNO;
		public string TradeTime = string.Empty;
		public short BuySell;
		public short SettleBasis;
		public string TraderID = string.Empty;
		public string FirmID = string.Empty;
		public string MarketID = string.Empty;
		public string CommodityID = string.Empty;
		public double TradePrice;
		public double OpenPrice;
		public long TradeQuantity;
		public double TransferPL;
		public double Comm;
		public double HoldingPrice;
		public string OrderTime = string.Empty;
		public short TradeType;
		public string OtherID;
	}
}
