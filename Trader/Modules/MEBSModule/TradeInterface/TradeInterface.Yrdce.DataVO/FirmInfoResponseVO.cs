using System;
using System.Collections.Generic;
namespace TradeInterface.Gnnt.DataVO
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
		public double OtherFrozenFund;
		public double RealFund;
		public double Fee;
		public double TransferPL;
		public List<Code> CDS = new List<Code>();
		public double CurrentRight;
		public double InOutFund;
		public double HoldingPL;
		public double OtherChange;
		public double ImpawnFund;
		public short Status;
		public long AMHolding;
		public long CurMHolding;
		public long CurrentOpen;
		public double MinFund;
		public double SFund;
		public string CustomerID = string.Empty;
		public string CustomerName = string.Empty;
	}
}
