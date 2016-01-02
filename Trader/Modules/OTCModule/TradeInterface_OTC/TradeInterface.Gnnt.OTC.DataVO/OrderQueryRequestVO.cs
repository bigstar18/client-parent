using System;
namespace TradeInterface.Gnnt.OTC.DataVO
{
	public class OrderQueryRequestVO
	{
		public string UserID = string.Empty;
		public string MarketID = string.Empty;
		public short BuySell;
		public long OrderNO;
		public string CommodityID = string.Empty;
		public int StartNum;
		public int RecordCount;
		public long UpdateTime;
		public string SortField = string.Empty;
		public bool IsDesc;
		public string AgencyNo = string.Empty;
		public string AgencyPhonePassword = string.Empty;
	}
}
