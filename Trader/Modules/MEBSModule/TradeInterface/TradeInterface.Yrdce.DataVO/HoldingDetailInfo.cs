using System;
namespace TradeInterface.Gnnt.DataVO
{
	public class HoldingDetailInfo
	{
		public string MarketID = string.Empty;
		public string CommodityID = string.Empty;
		public string CustomerID = string.Empty;
		public short BuySell;
		public long AmountOnOrder;
		public double Price;
		public long GOQuantity;
		public double Bail;
		public string DeadLine;
		public string RemainDay;
		public string holddate;
	}
}
