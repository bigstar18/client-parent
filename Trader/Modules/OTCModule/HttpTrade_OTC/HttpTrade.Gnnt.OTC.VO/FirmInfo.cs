using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class FirmInfo
	{
		private string FI;
		private string FN;
		private string IF;
		private string CM;
		private string CF;
		private string RM;
		private string RF;
		private string OR_F;
		private string OR_F_M;
		private string OR_F_F;
		private string OT_F;
		private string UF;
		private string FEE;
		private string PL;
		private string EQT;
		private string IOF;
		private string HPF;
		private string FUNDS_RISK;
		private string OC;
		private string PGF;
		private string C_STA;
		private string CD;
		private string ZT_IOF;
		public string FirmID
		{
			get
			{
				return this.FI;
			}
		}
		public string FirmName
		{
			get
			{
				return this.FN;
			}
		}
		public double InitFund
		{
			get
			{
				return Tools.StrToDouble(this.IF);
			}
		}
		public double YesterdayBail
		{
			get
			{
				return Tools.StrToDouble(this.CM);
			}
		}
		public double YesterdayFL
		{
			get
			{
				return Tools.StrToDouble(this.CF);
			}
		}
		public double CurrentBail
		{
			get
			{
				return Tools.StrToDouble(this.RM);
			}
		}
		public double CurrentFL
		{
			get
			{
				return Tools.StrToDouble(this.RF);
			}
		}
		public double OrderFrozenFund
		{
			get
			{
				return Tools.StrToDouble(this.OR_F);
			}
		}
		public double OrderFrozenMargin
		{
			get
			{
				return Tools.StrToDouble(this.OR_F_M);
			}
		}
		public double OrderFrozenFee
		{
			get
			{
				return Tools.StrToDouble(this.OR_F_F);
			}
		}
		public double OtherFrozenFund
		{
			get
			{
				return Tools.StrToDouble(this.OT_F);
			}
		}
		public double RealFund
		{
			get
			{
				return Tools.StrToDouble(this.UF);
			}
		}
		public double Fee
		{
			get
			{
				return Tools.StrToDouble(this.FEE);
			}
		}
		public double TransferPL
		{
			get
			{
				return Tools.StrToDouble(this.PL);
			}
		}
		public double CurrentRight
		{
			get
			{
				return Tools.StrToDouble(this.EQT);
			}
		}
		public double InOutFund
		{
			get
			{
				return Tools.StrToDouble(this.IOF);
			}
		}
		public double HoldingPL
		{
			get
			{
				return Tools.StrToDouble(this.HPF);
			}
		}
		public double FundRisk
		{
			get
			{
				return Tools.StrToDouble(this.FUNDS_RISK);
			}
		}
		public double OtherChange
		{
			get
			{
				return Tools.StrToDouble(this.OC);
			}
		}
		public double ImpawnFund
		{
			get
			{
				return Tools.StrToDouble(this.PGF);
			}
		}
		public double ClearDelay
		{
			get
			{
				return Tools.StrToDouble(this.CD);
			}
		}
		public double UsingFund
		{
			get
			{
				return Tools.StrToDouble(this.ZT_IOF);
			}
		}
		public string CStatus
		{
			get
			{
				return this.C_STA;
			}
		}
	}
}
