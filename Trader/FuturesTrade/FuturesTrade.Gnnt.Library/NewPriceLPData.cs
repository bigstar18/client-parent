using System;
namespace FuturesTrade.Gnnt.Library
{
	public class NewPriceLPData
	{
		private string marketId;
		private string commodityID;
		private double last;
		private double avg;
		private double sellAvg;
		private double sellHolding;
		private double buyAvg;
		private double buyHolding;
		private double ctrtSize;
		private double newPriceLP;
		public string MarketId
		{
			get
			{
				return this.marketId;
			}
			set
			{
				this.marketId = value;
			}
		}
		public string CommodityID
		{
			get
			{
				return this.commodityID;
			}
			set
			{
				this.commodityID = value;
			}
		}
		public double Last
		{
			get
			{
				return this.last;
			}
			set
			{
				this.last = value;
			}
		}
		public double Avg
		{
			get
			{
				return this.avg;
			}
			set
			{
				this.avg = value;
			}
		}
		public double SellAvg
		{
			get
			{
				return this.sellAvg;
			}
			set
			{
				this.sellAvg = value;
			}
		}
		public double SellHolding
		{
			get
			{
				return this.sellHolding;
			}
			set
			{
				this.sellHolding = value;
			}
		}
		public double BuyAvg
		{
			get
			{
				return this.buyAvg;
			}
			set
			{
				this.buyAvg = value;
			}
		}
		public double BuyHolding
		{
			get
			{
				return this.buyHolding;
			}
			set
			{
				this.buyHolding = value;
			}
		}
		public double CtrtSize
		{
			get
			{
				return this.ctrtSize;
			}
			set
			{
				this.ctrtSize = value;
			}
		}
		public double NewPriceLP
		{
			get
			{
				return this.newPriceLP;
			}
			set
			{
				this.newPriceLP = value;
			}
		}
	}
}
