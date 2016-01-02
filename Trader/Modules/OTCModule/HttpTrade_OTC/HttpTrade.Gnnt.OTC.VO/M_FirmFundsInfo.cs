using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class M_FirmFundsInfo
	{
		private string PURE_F;
		private string TRADE_F;
		private string D_F;
		private string R_M;
		private string JTC;
		private string T_F_F;
		private string C_F_F;
		private string T_T_F;
		private string T_JTC_F;
		private string T_M_JTC_F;
		private string STA;
		private string M_T;
		private string TRADE_CF;
		private string M_R_F;
		public double PureFloatProfit
		{
			get
			{
				return Tools.StrToDouble(this.PURE_F);
			}
			set
			{
				this.PURE_F = value.ToString();
			}
		}
		public double TradeFloatProfit
		{
			get
			{
				return Tools.StrToDouble(this.TRADE_F);
			}
			set
			{
				this.TRADE_F = value.ToString();
			}
		}
		public double CustomerCloseProfit
		{
			get
			{
				return Tools.StrToDouble(this.TRADE_CF);
			}
			set
			{
				this.TRADE_CF = value.ToString();
			}
		}
		public double DuiChongFloatProfit
		{
			get
			{
				return Tools.StrToDouble(this.D_F);
			}
			set
			{
				this.D_F = value.ToString();
			}
		}
		public double RiskMargin
		{
			get
			{
				return Tools.StrToDouble(this.R_M);
			}
			set
			{
				this.R_M = value.ToString();
			}
		}
		public string JingTouCun
		{
			get
			{
				return this.JTC;
			}
			set
			{
				this.JTC = value.ToString();
			}
		}
		public double MemberFundAlertLimit
		{
			get
			{
				return Tools.StrToDouble(this.T_F_F);
			}
			set
			{
				this.T_F_F = value.ToString();
			}
		}
		public double CustomerFundAlertLimit
		{
			get
			{
				return Tools.StrToDouble(this.C_F_F);
			}
			set
			{
				this.C_F_F = value.ToString();
			}
		}
		public double MemberFreezeLimit
		{
			get
			{
				return Tools.StrToDouble(this.T_T_F);
			}
			set
			{
				this.T_T_F = value.ToString();
			}
		}
		public long JingTouCunAlertLimit
		{
			get
			{
				return Tools.StrToLong(this.T_JTC_F);
			}
			set
			{
				this.T_JTC_F = value.ToString();
			}
		}
		public long JingTouCunMaxAlertLimit
		{
			get
			{
				return Tools.StrToLong(this.T_M_JTC_F);
			}
			set
			{
				this.T_M_JTC_F = value.ToString();
			}
		}
		public string Status
		{
			get
			{
				return this.STA;
			}
			set
			{
				this.STA = value.ToString();
			}
		}
		public string MemberType
		{
			get
			{
				return this.M_T;
			}
			set
			{
				this.M_T = value.ToString();
			}
		}
		public double MinRiskFund
		{
			get
			{
				return Tools.StrToDouble(this.M_R_F);
			}
			set
			{
				this.M_R_F = value.ToString();
			}
		}
	}
}
