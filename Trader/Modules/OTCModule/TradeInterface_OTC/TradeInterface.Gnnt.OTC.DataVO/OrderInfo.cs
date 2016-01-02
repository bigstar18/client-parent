using System;
namespace TradeInterface.Gnnt.OTC.DataVO
{
	public class OrderInfo
	{
		public long OrderNO;
		public long HoldingNO;
		public string Time = string.Empty;
		public short State;
		public short BuySell;
		public short SettleBasis;
		public string TraderID = string.Empty;
		public string FirmID = string.Empty;
		public string MarketID = string.Empty;
		public string CommodityID = string.Empty;
		public double OrderPrice;
		public long OrderQuantity;
		public string WithDrawTime = string.Empty;
		public double StopLoss;
		public double StopProfit;
		public short BillTradeType;
		public short OrderType;
		public string OrderFirmID;
		public string AgentID;
		public double FrozenMargin;
		public double FrozenFee;
	}
}
