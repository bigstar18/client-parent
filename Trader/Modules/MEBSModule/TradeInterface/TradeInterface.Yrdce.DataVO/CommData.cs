using System;
namespace TradeInterface.Gnnt.DataVO
{
	public class CommData
	{
		public string MarketID = string.Empty;
		public string CommodityID = string.Empty;
		public string CommodityName = string.Empty;
		public string DeliveryDate = string.Empty;
		public double PrevClear;
		public double Bid;
		public long BidVol;
		public double Offer;
		public long OfferVol;
		public double High;
		public double Low;
		public double Last;
		public double Avg;
		public double Change;
		public double VolToday;
		public double TTOpen;
	}
}
