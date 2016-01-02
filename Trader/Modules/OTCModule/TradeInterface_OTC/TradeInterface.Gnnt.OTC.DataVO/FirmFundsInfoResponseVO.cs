using System;
namespace TradeInterface.Gnnt.OTC.DataVO
{
	public class FirmFundsInfoResponseVO : ResponseVO
	{
		public double MemberPureFloating;
		public double CustomerTradeFloating;
		public double CustomerCloseProfit;
		public double DuiChongFloating;
		public double RiskMargin;
		public string MemberJingTouCun;
		public double MemberFundAlertLimit;
		public double CustomerFundAlertLimit;
		public double MemberFreezeLimit;
		public long JingTouCunAlertLimit;
		public long JingTouCunMaxAlertLimit;
		public string Status;
		public string MemberType;
		public double MinRiskFund;
	}
}
