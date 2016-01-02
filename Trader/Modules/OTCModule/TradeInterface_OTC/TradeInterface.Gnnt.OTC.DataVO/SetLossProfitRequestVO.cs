using System;
namespace TradeInterface.Gnnt.OTC.DataVO
{
	public class SetLossProfitRequestVO
	{
		public string UserID = string.Empty;
		public long HoldingID;
		public double StopLoss;
		public double StopProfit;
		public long SessionID;
		public string CommodityID;
		public string BuySellType;
		public string AgencyNo = string.Empty;
		public string AgencyPhonePassword = string.Empty;
	}
}
