using System;
namespace TradeInterface.Gnnt.OTC.DataVO
{
	public class WithdrawLossProfitRequestVO
	{
		public string UserID = string.Empty;
		public long HoldingID;
		public short Type;
		public string CommodityID;
		public string AgencyNo = string.Empty;
		public string AgencyPhonePassword = string.Empty;
	}
}
