using System;
namespace TradeInterface.Gnnt.DataVO
{
	public class TradeQueryRequestVO
	{
		public string UserID = string.Empty;
		public string MarketID = string.Empty;
		public int StartNum;
		public int RecordCount;
	}
}
