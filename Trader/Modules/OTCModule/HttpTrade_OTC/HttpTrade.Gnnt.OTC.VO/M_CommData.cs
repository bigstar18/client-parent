using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class M_CommData
	{
		private string C;
		private string H;
		private string L;
		private string LT;
		private string T;
		private string S;
		private string B;
		public string CommodityID
		{
			get
			{
				return this.C;
			}
			set
			{
				this.C = value;
			}
		}
		public double High
		{
			get
			{
				return Tools.StrToDouble(this.H);
			}
			set
			{
				this.H = value.ToString();
			}
		}
		public double Low
		{
			get
			{
				return Tools.StrToDouble(this.L);
			}
			set
			{
				this.L = value.ToString();
			}
		}
		public double Last
		{
			get
			{
				return Tools.StrToDouble(this.LT);
			}
			set
			{
				this.LT = value.ToString();
			}
		}
		public double BuyPrice
		{
			get
			{
				return Tools.StrToDouble(this.B);
			}
			set
			{
				this.B = value.ToString();
			}
		}
		public double SellPrice
		{
			get
			{
				return Tools.StrToDouble(this.S);
			}
			set
			{
				this.S = value.ToString();
			}
		}
		public string UpdateTime
		{
			get
			{
				return this.T;
			}
			set
			{
				this.T = value.ToString();
			}
		}
	}
}
