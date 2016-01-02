using System;
namespace TradeInterface.Gnnt.ISSUE.DataVO
{
	public class FirmInfoResponseVO : ResponseVO
	{
		public string FirmID = string.Empty;
		public string FirmName = string.Empty;
		public short FirmType;
		public double InitFund;
		public double InFund;
		public double OutFund;
		public double HKSell;
		public double HKBuy;
		public double CurFreezeFund;
		public double CurUnfreezeFund;
		public double IssuanceFee;
		public double SGFreezeFund;
		public double OrderFrozenFund;
		public double OtherFrozenFund;
		public double Fee;
		public double WareHouseRegFee;
		public double WareHouseCancelFee;
		public double TransferFee;
		public double DistributionFee;
		public double OtherFee;
		public double OtherChange;
		public double MarketValue;
		public double UsableFund;
		public double DesirableFund;
		public double CurrentRight;
	}
}
