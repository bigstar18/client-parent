using System;
namespace TradeInterface.Gnnt.OTC.DataVO
{
	public class CommData
	{
		public string MarketID = string.Empty;
		public string CommodityID = string.Empty;
		public double High;
		public double Low;
		public double BuyPrice;
		public double SellPrice;
		public string UpdateTime;
		public double CustomerBuyPrice;
		public double CustomerSellPrice;
		public CommData(CommData _cd)
		{
			this.CommodityID = _cd.CommodityID;
			this.BuyPrice = _cd.BuyPrice;
			this.High = _cd.High;
			this.Low = _cd.Low;
			this.SellPrice = _cd.SellPrice;
			this.UpdateTime = _cd.UpdateTime;
			this.MarketID = _cd.MarketID;
			this.CustomerBuyPrice = _cd.CustomerBuyPrice;
			this.CustomerSellPrice = _cd.CustomerSellPrice;
		}
		public CommData()
		{
		}
		public object Clone()
		{
			return new CommData(this);
		}
	}
}
