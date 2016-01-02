using System;
namespace TradeInterface.Gnnt.DataVO
{
	public class TradeQueryPagingRequestVO
	{
		public string UserID = string.Empty;
		public string MarketID = string.Empty;
		public int StartNum;
		public int RecordCount;
		public string SortFld;
		public short IsDesc;
		public int CurrentPagNum;
		public string Pri;
		public short Type;
		public short Se_f;
	}
}
