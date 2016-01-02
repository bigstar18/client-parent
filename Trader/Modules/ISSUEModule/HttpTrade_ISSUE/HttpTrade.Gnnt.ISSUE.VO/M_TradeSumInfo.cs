using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class M_TradeSumInfo
	{
		private string CO_I;
		private string B_Q;
		private string B_COMM;
		private string S_Q;
		private string S_COMM;
		public string CommodityID
		{
			get
			{
				return this.CO_I;
			}
		}
		public long BuyQty
		{
			get
			{
				return Tools.StrToLong(this.B_Q);
			}
		}
		public double BuyComm
		{
			get
			{
				return Tools.StrToDouble(this.B_COMM);
			}
		}
		public long SellQty
		{
			get
			{
				return Tools.StrToLong(this.S_Q);
			}
		}
		public double SellComm
		{
			get
			{
				return Tools.StrToDouble(this.S_COMM);
			}
		}
	}
}
