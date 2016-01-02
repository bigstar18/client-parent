using System;
namespace TradeInterface.Gnnt.OTC.DataVO
{
	public class FirmHoldSumQuery
	{
		public string CommodityID;
		public string MaxHolding;
		public string MemberJingTouCun;
		public string CustomerJingTouCun;
		public string DuiChongJingTouCun;
		public double HoldingNetFloating;
		public double CustomerTradeFloating;
		public double DuiChongFloating;
	}
}
