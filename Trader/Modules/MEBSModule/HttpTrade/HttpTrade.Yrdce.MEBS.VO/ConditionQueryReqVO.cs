using HttpTrade.Gnnt.MEBS.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class ConditionQueryReqVO : ReqVO
	{
		private string USER_ID;
		private string CUSTOMER_ID;
		private string BUY_SELL;
		private string ORDER_NO;
		private string MARKET_ID;
		private string COMMODITY_ID;
		private string SESSION_ID;
		private string STARTNUM;
		private string RECCNT;
		private string SORTFLD;
		private string ISDESC;
		private string UT;
		private string SE_F;
		private string ORDER_S;
		private string CONTYPE;
		private string PAGENUM;
		public string Userid
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
		public string CustomerID
		{
			get
			{
				return this.CUSTOMER_ID;
			}
			set
			{
				this.CUSTOMER_ID = value;
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
		public short SettleBasis
		{
			get
			{
				return Tools.StrToShort(this.SE_F);
			}
			set
			{
				this.SE_F = value.ToString();
			}
		}
		public string OrderStatus
		{
			get
			{
				return this.ORDER_S;
			}
			set
			{
				this.ORDER_S = value;
			}
		}
		public short ConditionType
		{
			get
			{
				return Tools.StrToShort(this.CONTYPE);
			}
			set
			{
				this.CONTYPE = value.ToString();
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
		public string SortField
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
		public short isDesc
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
		public int PageNumber
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
		public ConditionQueryReqVO()
		{
			base.Name = ProtocolName.conditionorder_query;
		}
	}
}
