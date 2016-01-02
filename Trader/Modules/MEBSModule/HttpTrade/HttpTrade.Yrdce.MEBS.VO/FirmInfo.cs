using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
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
		private string OT_F;
		private string UF;
		private string FEE;
		private string PL;
		private CodeList CDRS;
		private string EQT;
		private string IOF;
		private string HPF;
		private string OC;
		private string PGF;
		private string STA;
		private string AP_C;
		private string VI_O;
		private string CU_O;
		private string TE_C;
		private string TE_A;
		private string CU_I;
		private string CU_N;
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
		public CodeList CodeList
		{
			get
			{
				return this.CDRS;
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
		public short Status
		{
			get
			{
				return Tools.StrToShort(this.STA);
			}
		}
		public long AMHolding
		{
			get
			{
				return Tools.StrToLong(this.AP_C);
			}
		}
		public long CurMHolding
		{
			get
			{
				return Tools.StrToLong(this.VI_O);
			}
		}
		public long CurrentOpen
		{
			get
			{
				return Tools.StrToLong(this.CU_O);
			}
		}
		public double MinFund
		{
			get
			{
				return Tools.StrToDouble(this.TE_C);
			}
		}
		public double SFund
		{
			get
			{
				return Tools.StrToDouble(this.TE_A);
			}
		}
		public string CustomerID
		{
			get
			{
				return this.CU_I;
			}
		}
		public string CustomerName
		{
			get
			{
				return this.CU_N;
			}
		}
	}
}
