using System;
namespace TradeInterface.Gnnt.DataVO
{
	public class HoldingDetailRequestVO
	{
		public string UserID = string.Empty;
		public string MarketID = string.Empty;
		public string CommodityID = string.Empty;
		public int StartNum;
		public int RecordCount;
	}
}
