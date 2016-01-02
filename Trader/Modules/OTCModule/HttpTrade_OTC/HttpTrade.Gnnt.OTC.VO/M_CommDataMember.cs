using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class M_CommDataMember
	{
		private string C;
		private string H;
		private string L;
		private string LT;
		private string T;
		private string S;
		private string B;
		private string _BasePrice;
		private string _MemberBuyDianCha;
		private string _MemberSellDianCha;
		private string _SMemberBuyDianCha;
		private string _SMemberSellDianCha;
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
		public double BasePrice
		{
			get
			{
				return Tools.StrToDouble(this._BasePrice);
			}
			set
			{
				this._BasePrice = value.ToString();
			}
		}
		public double MemberBuyDianCha
		{
			get
			{
				return Tools.StrToDouble(this._MemberBuyDianCha);
			}
			set
			{
				this._MemberBuyDianCha = value.ToString();
			}
		}
		public double MemberSellDianCha
		{
			get
			{
				return Tools.StrToDouble(this._MemberSellDianCha);
			}
			set
			{
				this._MemberSellDianCha = value.ToString();
			}
		}
		public double SMemberBuyDianCha
		{
			get
			{
				return Tools.StrToDouble(this._SMemberBuyDianCha);
			}
			set
			{
				this._SMemberBuyDianCha = value.ToString();
			}
		}
		public double SMemberSellDianCha
		{
			get
			{
				return Tools.StrToDouble(this._SMemberSellDianCha);
			}
			set
			{
				this._SMemberSellDianCha = value.ToString();
			}
		}
	}
}
