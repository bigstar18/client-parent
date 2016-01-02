using System;
namespace TradeInterface.Gnnt.OTC.DataVO
{
	public class HoldingInfo
	{
		public string MarketID = string.Empty;
		public string CommodityID = string.Empty;
		public double FloatingLP;
		public double Bail;
		public double CommPrice;
		public short TradeType;
		public long Qty;
		public double OpenAveragePrice;
		public double HoldingAveragePrice;
		public long FreezeQty;
	}
}
