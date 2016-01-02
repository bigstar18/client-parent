using System;
namespace TradeClientApp.Gnnt.OTC
{
	public class CloseCommodityInfo
	{
		private long _HoldingID;
		private string _OriginBuySell = string.Empty;
		private string _CommodityID = string.Empty;
		private long _CloseMaxBuyQty;
		private long _CloseMaxSellQty;
		private double _ZS;
		private double _ZY;
		public long HoldingID
		{
			get
			{
				return this._HoldingID;
			}
			set
			{
				this._HoldingID = value;
			}
		}
		public string OriginBuySell
		{
			get
			{
				return this._OriginBuySell;
			}
			set
			{
				this._OriginBuySell = value;
			}
		}
		public string CommodityID
		{
			get
			{
				return this._CommodityID;
			}
			set
			{
				this._CommodityID = value;
			}
		}
		public long CloseMaxBuyQty
		{
			get
			{
				return this._CloseMaxBuyQty;
			}
			set
			{
				this._CloseMaxBuyQty = value;
			}
		}
		public long CloseMaxSellQty
		{
			get
			{
				return this._CloseMaxSellQty;
			}
			set
			{
				this._CloseMaxSellQty = value;
			}
		}
		public double ZS
		{
			get
			{
				return this._ZS;
			}
			set
			{
				this._ZS = value;
			}
		}
		public double ZY
		{
			get
			{
				return this._ZY;
			}
			set
			{
				this._ZY = value;
			}
		}
	}
}
