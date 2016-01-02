using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class M_CustomerOrderQuery
	{
		private string CO_I;
		private string BU_A;
		private string BU_F;
		private string BU_Q;
		private string BU_H;
		private string SE_A;
		private string SE_F;
		private string SE_Q;
		private string SE_H;
		private string JTC;
		private string F;
		public string CommodityID
		{
			get
			{
				return this.CO_I;
			}
			set
			{
				this.CO_I = value;
			}
		}
		public double BuyAveragePrice
		{
			get
			{
				return Tools.StrToDouble(this.BU_A);
			}
			set
			{
				this.BU_A = value.ToString();
			}
		}
		public double BuyHoldingAmount
		{
			get
			{
				return Tools.StrToDouble(this.BU_H);
			}
			set
			{
				this.BU_H = value.ToString();
			}
		}
		public long BuyQuantity
		{
			get
			{
				return Tools.StrToLong(this.BU_Q);
			}
			set
			{
				this.BU_Q = value.ToString();
			}
		}
		public double BuyFloat
		{
			get
			{
				return Tools.StrToDouble(this.BU_F);
			}
			set
			{
				this.BU_F = value.ToString();
			}
		}
		public double SellAveragePrice
		{
			get
			{
				return Tools.StrToDouble(this.SE_A);
			}
			set
			{
				this.SE_A = value.ToString();
			}
		}
		public double SellHoldingAmount
		{
			get
			{
				return Tools.StrToDouble(this.SE_H);
			}
			set
			{
				this.SE_H = value.ToString();
			}
		}
		public long SellQuantity
		{
			get
			{
				return Tools.StrToLong(this.SE_Q);
			}
			set
			{
				this.SE_Q = value.ToString();
			}
		}
		public double SellFloat
		{
			get
			{
				return Tools.StrToDouble(this.SE_F);
			}
			set
			{
				this.SE_F = value.ToString();
			}
		}
		public long JingTouCun
		{
			get
			{
				return Tools.StrToLong(this.JTC);
			}
			set
			{
				this.JTC = value.ToString();
			}
		}
		public double Float
		{
			get
			{
				return Tools.StrToDouble(this.F);
			}
			set
			{
				this.F = value.ToString();
			}
		}
	}
}
