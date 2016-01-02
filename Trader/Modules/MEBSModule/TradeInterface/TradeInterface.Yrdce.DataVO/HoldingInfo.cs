using System;
namespace TradeInterface.Gnnt.DataVO
{
	public class HoldingInfo
	{
		public string MarketID = string.Empty;
		public string CommodityID = string.Empty;
		public string CustomerID = string.Empty;
		public long BuyHolding;
		public long SellHolding;
		public long BuyVHolding;
		public long SellVHolding;
		public double BuyAverage;
		public double SellAverage;
		public long GOQuantity;
		public double FloatingLP;
		public double Bail;
		public double NewPriceLP;
	}
}
