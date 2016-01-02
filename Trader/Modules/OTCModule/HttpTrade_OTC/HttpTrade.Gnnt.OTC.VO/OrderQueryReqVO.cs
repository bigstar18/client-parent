using HttpTrade.Gnnt.OTC.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
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
		private string SORTFLD;
		private string ISDESC;
		private string A_N;
		private string P_P;
		private string MARKET_ID;
		public string AgencyNo
		{
			get
			{
				return this.A_N;
			}
			set
			{
				this.A_N = value;
			}
		}
		public string AgencyPhonePassword
		{
			get
			{
				return this.P_P;
			}
			set
			{
				this.P_P = value;
			}
		}
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
		public long StartNum
		{
			get
			{
				return Tools.StrToLong(this.STARTNUM);
			}
			set
			{
				this.STARTNUM = value.ToString();
			}
		}
		public long RecordCount
		{
			get
			{
				return Tools.StrToLong(this.RECCNT);
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
		public bool IsDesc
		{
			get
			{
				return Tools.StrToBool(this.ISDESC);
			}
			set
			{
				this.ISDESC = Convert.ToInt16(value).ToString();
			}
		}
		public OrderQueryReqVO()
		{
			base.Name = ProtocolName.my_order_query;
		}
	}
}
