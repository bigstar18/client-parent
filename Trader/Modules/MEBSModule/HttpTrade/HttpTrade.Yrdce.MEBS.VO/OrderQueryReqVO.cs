using HttpTrade.Gnnt.MEBS.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class OrderQueryReqVO : ReqVO
	{
		private string USER_ID;
		private string BUY_SELL;
		private string ORDER_NO;
		private string COMMODITY_ID;
		private string STARTNUM;
		private string RECCNT;
		private string UT;
		private string SESSION_ID;
		private string MARKET_ID;
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
		public OrderQueryReqVO()
		{
			base.Name = ProtocolName.my_order_query;
		}
	}
}
