using System;
namespace TradeInterface.Gnnt.OTC.DataVO
{
	public class HoldingDetailInfo
	{
		public long HoldingID;
		public string MarketID = string.Empty;
		public string CommodityID = string.Empty;
		public short BuySell;
		public long OpenQuantity;
		public long HoldingQuantity;
		public double OpenPrice;
		public double HoldPrice;
		public string OrderTime;
		public double TotalFloatingPrice;
		public double CommPrice;
		public double Bail;
		public double StopLoss;
		public double StopProfit;
		public string OtherID;
		public string AgentID;
	}
}
