using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class M_CommData
	{
		private string CO_I;
		private string CO_N;
		private string L_SET;
		private string PR_C;
		private string BID;
		private string BI_D;
		private string OFFER;
		private string OF_D;
		private string HIGH;
		private string LOW;
		private string LAST;
		private string AVG;
		private string CHA;
		private string VO_T;
		private string TT_O;
		public string CommodityID
		{
			get
			{
				return this.CO_I;
			}
		}
		public string CommodityName
		{
			get
			{
				return this.CO_N;
			}
		}
		public string DeliveryDate
		{
			get
			{
				return this.L_SET;
			}
		}
		public double PrevClear
		{
			get
			{
				return Tools.StrToDouble(this.PR_C);
			}
		}
		public double Bid
		{
			get
			{
				return Tools.StrToDouble(this.BID);
			}
		}
		public long BidVol
		{
			get
			{
				return Tools.StrToLong(this.BI_D);
			}
		}
		public double Offer
		{
			get
			{
				return Tools.StrToDouble(this.OFFER);
			}
		}
		public long OfferVol
		{
			get
			{
				return Tools.StrToLong(this.OF_D);
			}
		}
		public double High
		{
			get
			{
				return Tools.StrToDouble(this.HIGH);
			}
		}
		public double Low
		{
			get
			{
				return Tools.StrToDouble(this.LOW);
			}
		}
		public double Last
		{
			get
			{
				return Tools.StrToDouble(this.LAST);
			}
		}
		public double Avg
		{
			get
			{
				return Tools.StrToDouble(this.AVG);
			}
		}
		public double Change
		{
			get
			{
				return Tools.StrToDouble(this.CHA);
			}
		}
		public double VolToday
		{
			get
			{
				return Tools.StrToDouble(this.VO_T);
			}
		}
		public double TTOpen
		{
			get
			{
				return Tools.StrToDouble(this.TT_O);
			}
		}
	}
}
