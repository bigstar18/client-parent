using System;
namespace TradeInterface.Gnnt.OTC.DataVO
{
	public class OrderRequestVO
	{
		public string UserID = string.Empty;
		public short BuySell;
		public string MarketID = string.Empty;
		public string CommodityID = string.Empty;
		public double Price;
		public long Quantity;
		public short SettleBasis;
		public short DotDiff;
		public short CloseMode;
		public long HoldingID;
		public string OtherID = string.Empty;
		public short TradeType;
		public double StopLoss;
		public double StopProfit;
		public string ValidityType;
		public bool IsFSJC;
		public long FSJCQuantity;
		public string AgencyNo = string.Empty;
		public string AgencyPhonePassword = string.Empty;
	}
}
