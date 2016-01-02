using System;
namespace TradeInterface.Gnnt.OTC.DataVO
{
	public class FirmInfoResponseVO : ResponseVO
	{
		public string FirmID = string.Empty;
		public string FirmName = string.Empty;
		public double InitFund;
		public double YesterdayBail;
		public double YesterdayFL;
		public double CurrentBail;
		public double CurrentFL;
		public double OrderFrozenFund;
		public double OrderFrozenMargin;
		public double OrderFrozenFee;
		public double OtherFrozenFund;
		public double RealFund;
		public double Fee;
		public double TransferPL;
		public double CurrentRight;
		public double InOutFund;
		public double HoldingPL;
		public double FundRisk;
		public double OtherChange;
		public double ImpawnFund;
		public double ClearDelay;
		public double UsingFund;
		public string CStatus;
		public FirmInfoResponseVO()
		{
		}
		public FirmInfoResponseVO(FirmInfoResponseVO _FirmInfoResponseVO)
		{
			this.ClearDelay = _FirmInfoResponseVO.ClearDelay;
			this.CStatus = _FirmInfoResponseVO.CStatus;
			this.CurrentBail = _FirmInfoResponseVO.CurrentBail;
			this.CurrentFL = _FirmInfoResponseVO.CurrentFL;
			this.CurrentRight = _FirmInfoResponseVO.CurrentRight;
			this.Fee = _FirmInfoResponseVO.Fee;
			this.FirmID = _FirmInfoResponseVO.FirmID;
			this.FirmName = _FirmInfoResponseVO.FirmName;
			this.FundRisk = _FirmInfoResponseVO.FundRisk;
			this.HoldingPL = _FirmInfoResponseVO.HoldingPL;
			this.ImpawnFund = _FirmInfoResponseVO.ImpawnFund;
			this.InitFund = _FirmInfoResponseVO.InitFund;
			this.InOutFund = _FirmInfoResponseVO.InOutFund;
			this.OrderFrozenFee = _FirmInfoResponseVO.OrderFrozenFee;
			this.OrderFrozenFund = _FirmInfoResponseVO.OrderFrozenFund;
			this.OrderFrozenMargin = _FirmInfoResponseVO.OrderFrozenMargin;
			this.OtherChange = _FirmInfoResponseVO.OtherChange;
			this.OtherFrozenFund = _FirmInfoResponseVO.OtherFrozenFund;
			this.RealFund = _FirmInfoResponseVO.RealFund;
			this.RetCode = _FirmInfoResponseVO.RetCode;
			this.RetMessage = _FirmInfoResponseVO.RetMessage;
			this.YesterdayBail = _FirmInfoResponseVO.YesterdayBail;
			this.YesterdayFL = _FirmInfoResponseVO.YesterdayFL;
			this.TransferPL = _FirmInfoResponseVO.TransferPL;
			this.UsingFund = _FirmInfoResponseVO.UsingFund;
		}
		public object Clone()
		{
			return new FirmInfoResponseVO(this);
		}
	}
}
