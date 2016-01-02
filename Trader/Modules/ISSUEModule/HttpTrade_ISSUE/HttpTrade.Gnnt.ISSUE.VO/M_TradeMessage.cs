using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class M_TradeMessage
	{
		private string OR_N;
		private string CO_I;
		private string TDQTY;
		public long OrderNO
		{
			get
			{
				return Tools.StrToLong(this.OR_N);
			}
		}
		public string CommodityID
		{
			get
			{
				return this.CO_I;
			}
		}
		public long TradeQuatity
		{
			get
			{
				return Tools.StrToLong(this.TDQTY);
			}
		}
	}
}
