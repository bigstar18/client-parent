using System;
namespace TradeInterface.Gnnt.OTC.DataVO
{
	public class TradeMessage
	{
		public long OrderNO;
		public string MarketID = string.Empty;
		public string CommodityID = string.Empty;
		public long TradeQuatity;
		public short BuySell;
		public short SettleBasis;
		public short TradeType;
	}
}
