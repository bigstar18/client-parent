using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.ISSUE.VO
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
		private string BP_1;
		private string BP_2;
		private string BP_3;
		private string SP_1;
		private string SP_2;
		private string SP_3;
		private string BV_1;
		private string BV_2;
		private string BV_3;
		private string SV_1;
		private string SV_2;
		private string SV_3;
		private string COUNT;
		private string LIMIT_UP;
		private string LIMIT_DOWN;
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
		public double BPrice1
		{
			get
			{
				return Tools.StrToDouble(this.BP_1);
			}
		}
		public double BPrice2
		{
			get
			{
				return Tools.StrToDouble(this.BP_2);
			}
		}
		public double BPrice3
		{
			get
			{
				return Tools.StrToDouble(this.BP_3);
			}
		}
		public double SPrice1
		{
			get
			{
				return Tools.StrToDouble(this.SP_1);
			}
		}
		public double SPrice2
		{
			get
			{
				return Tools.StrToDouble(this.SP_2);
			}
		}
		public double SPrice3
		{
			get
			{
				return Tools.StrToDouble(this.SP_3);
			}
		}
		public double BValue1
		{
			get
			{
				return Tools.StrToDouble(this.BV_1);
			}
		}
		public double BValue2
		{
			get
			{
				return Tools.StrToDouble(this.BV_2);
			}
		}
		public double BValue3
		{
			get
			{
				return Tools.StrToDouble(this.BV_3);
			}
		}
		public double SValue1
		{
			get
			{
				return Tools.StrToDouble(this.SV_1);
			}
		}
		public double SValue2
		{
			get
			{
				return Tools.StrToDouble(this.SV_2);
			}
		}
		public double SValue3
		{
			get
			{
				return Tools.StrToDouble(this.SV_3);
			}
		}
		public double Count
		{
			get
			{
				return Tools.StrToDouble(this.COUNT);
			}
		}
	}
}
