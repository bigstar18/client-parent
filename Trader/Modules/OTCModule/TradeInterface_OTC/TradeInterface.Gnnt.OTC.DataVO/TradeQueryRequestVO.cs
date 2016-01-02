using System;
namespace TradeInterface.Gnnt.OTC.DataVO
{
	public class TradeQueryRequestVO
	{
		public string UserID = string.Empty;
		public string MarketID = string.Empty;
		public int StartNum;
		public int RecordCount;
		public string SortField = string.Empty;
		public bool IsDesc;
		public string AgencyNo = string.Empty;
		public string AgencyPhonePassword = string.Empty;
	}
}
