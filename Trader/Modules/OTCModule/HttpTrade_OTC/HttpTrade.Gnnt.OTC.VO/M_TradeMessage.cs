using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class M_TradeMessage
	{
		private string OR_N;
		private string CO_I;
		private string TDQTY;
		private string BUY_SELL;
		private string SE_F;
		private string T_T;
		public long OrderNO
		{
			get
			{
				return Tools.StrToLong(this.OR_N);
			}
			set
			{
				this.OR_N = value.ToString();
			}
		}
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
		public long TradeQuatity
		{
			get
			{
				return Tools.StrToLong(this.TDQTY);
			}
			set
			{
				this.TDQTY = value.ToString();
			}
		}
		public short BuySell
		{
			get
			{
				return Tools.StrToShort(this.BUY_SELL);
			}
			set
			{
				this.BUY_SELL = value.ToString();
			}
		}
		public short SettleBasis
		{
			get
			{
				return Tools.StrToShort(this.SE_F);
			}
			set
			{
				this.SE_F = value.ToString();
			}
		}
		public short TradeType
		{
			get
			{
				return Tools.StrToShort(this.T_T);
			}
			set
			{
				this.T_T = value.ToString();
			}
		}
	}
}
