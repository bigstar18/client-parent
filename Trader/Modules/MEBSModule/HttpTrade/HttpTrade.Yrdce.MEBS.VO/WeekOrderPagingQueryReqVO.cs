using HttpTrade.Gnnt.MEBS.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class WeekOrderPagingQueryReqVO : ReqVO
	{
		private string USER_ID;
		private string BUY_SELL;
		private string ORDER_NO;
		private string MARKET_ID;
		private string COMMODITY_ID;
		private string SESSION_ID;
		private string STARTNUM;
		private string RECCNT;
		private string UT;
		private string SORTFLD;
		private string ISDESC;
		private string PAGENUM;
		private string ISQUERYALL;
		private string PRI;
		private string TYPE;
		private string STA;
		public string UserID
		{
			get
			{
				return this.USER_ID;
			}
			set
			{
				this.USER_ID = value;
			}
		}
		public short BuySell
		{
			get
			{
				return Tools.StrToShort(this.BUY_SELL);
			}
			set
			{
				this.BUY_SELL = value.ToString();
			}
		}
		public long OrderNO
		{
			get
			{
				return Tools.StrToLong(this.ORDER_NO);
			}
			set
			{
				this.ORDER_NO = value.ToString();
			}
		}
		public string CommodityID
		{
			get
			{
				return this.COMMODITY_ID;
			}
			set
			{
				this.COMMODITY_ID = value;
			}
		}
		public int StartNum
		{
			get
			{
				return Tools.StrToInt(this.STARTNUM);
			}
			set
			{
				this.STARTNUM = value.ToString();
			}
		}
		public int RecordCount
		{
			get
			{
				return Tools.StrToInt(this.RECCNT);
			}
			set
			{
				this.RECCNT = value.ToString();
			}
		}
		public long SessionID
		{
			get
			{
				return Tools.StrToLong(this.SESSION_ID);
			}
			set
			{
				this.SESSION_ID = value.ToString();
			}
		}
		public long UpdateTime
		{
			get
			{
				return Tools.StrToLong(this.UT);
			}
			set
			{
				this.UT = value.ToString();
			}
		}
		public string MarketID
		{
			get
			{
				return this.MARKET_ID;
			}
			set
			{
				this.MARKET_ID = value;
			}
		}
		public string SortFld
		{
			get
			{
				return this.SORTFLD;
			}
			set
			{
				this.SORTFLD = value;
			}
		}
		public short IsDesc
		{
			get
			{
				return Tools.StrToShort(this.ISDESC);
			}
			set
			{
				this.ISDESC = value.ToString();
			}
		}
		public int CurrentPagNum
		{
			get
			{
				return Tools.StrToInt(this.PAGENUM);
			}
			set
			{
				this.PAGENUM = value.ToString();
			}
		}
		public short IsQueryAll
		{
			get
			{
				return Tools.StrToShort(this.ISQUERYALL);
			}
			set
			{
				this.ISQUERYALL = value.ToString();
			}
		}
		public string Pri
		{
			get
			{
				return this.PRI;
			}
			set
			{
				this.PRI = value;
			}
		}
		public short Type
		{
			get
			{
				return Tools.StrToShort(this.TYPE);
			}
			set
			{
				this.TYPE = value.ToString();
			}
		}
		public short Sta
		{
			get
			{
				return Tools.StrToShort(this.STA);
			}
			set
			{
				this.STA = value.ToString();
			}
		}
		public WeekOrderPagingQueryReqVO()
		{
			base.Name = ProtocolName.my_weekorder_pagingquery;
		}
	}
}
