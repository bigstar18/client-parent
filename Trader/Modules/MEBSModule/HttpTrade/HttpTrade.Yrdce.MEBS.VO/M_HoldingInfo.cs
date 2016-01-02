using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class M_HoldingInfo
	{
		private string CO_I;
		private string CU_I;
		private string BU_H;
		private string SE_H;
		private string B_V_H;
		private string S_V_H;
		private string BU_A;
		private string SE_A;
		private string GO_Q;
		private string FL_P;
		private string MAR;
		private string NP_PF;
		public string CommodityID
		{
			get
			{
				return this.CO_I;
			}
		}
		public string CustomerID
		{
			get
			{
				return this.CU_I;
			}
		}
		public long BuyHolding
		{
			get
			{
				return Tools.StrToLong(this.BU_H);
			}
		}
		public long SellHolding
		{
			get
			{
				return Tools.StrToLong(this.SE_H);
			}
		}
		public long BuyVHolding
		{
			get
			{
				return Tools.StrToLong(this.B_V_H);
			}
		}
		public long SellVHolding
		{
			get
			{
				return Tools.StrToLong(this.S_V_H);
			}
		}
		public double BuyAverage
		{
			get
			{
				return Tools.StrToDouble(this.BU_A);
			}
		}
		public double SellAverage
		{
			get
			{
				return Tools.StrToDouble(this.SE_A);
			}
		}
		public long GOQuantity
		{
			get
			{
				return Tools.StrToLong(this.GO_Q);
			}
		}
		public double FloatingLP
		{
			get
			{
				return Tools.StrToDouble(this.FL_P);
			}
		}
		public double Bail
		{
			get
			{
				return Tools.StrToDouble(this.MAR);
			}
		}
		public double NewPriceLP
		{
			get
			{
				return Tools.StrToDouble(this.NP_PF);
			}
		}
	}
}
