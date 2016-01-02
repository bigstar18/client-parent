using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class M_FirmHoldSum
	{
		private string CO_I;
		private string M_H;
		private string F_H;
		private string C_H;
		private string H_F_H;
		private string F_F;
		private string C_F;
		private string H_F;
		public string CommodityID
		{
			get
			{
				return this.CO_I;
			}
			set
			{
				this.CO_I = value.ToString();
			}
		}
		public string MaxHolding
		{
			get
			{
				return this.M_H;
			}
			set
			{
				this.M_H = value.ToString();
			}
		}
		public string MemberJingTouCun
		{
			get
			{
				return this.F_H;
			}
			set
			{
				this.F_H = value.ToString();
			}
		}
		public string CustomerJingTouCun
		{
			get
			{
				return this.C_H;
			}
			set
			{
				this.C_H = value.ToString();
			}
		}
		public string DuiChongJingTouCun
		{
			get
			{
				return this.H_F_H;
			}
			set
			{
				this.H_F_H = value.ToString();
			}
		}
		public double HoldingNetFloating
		{
			get
			{
				return Tools.StrToDouble(this.F_F);
			}
			set
			{
				this.F_F = value.ToString();
			}
		}
		public double CustomerTradeFloating
		{
			get
			{
				return Tools.StrToDouble(this.C_F);
			}
			set
			{
				this.C_F = value.ToString();
			}
		}
		public double DuiChongFloating
		{
			get
			{
				return Tools.StrToDouble(this.H_F);
			}
			set
			{
				this.H_F = value.ToString();
			}
		}
	}
}
