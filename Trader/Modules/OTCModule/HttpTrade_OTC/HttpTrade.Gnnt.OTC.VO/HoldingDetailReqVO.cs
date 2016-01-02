using HttpTrade.Gnnt.OTC.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class HoldingDetailReqVO : ReqVO
	{
		private string USER_ID;
		private string COMMODITY_ID;
		private string STARTNUM;
		private string RECCNT;
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
		public HoldingDetailReqVO()
		{
			base.Name = ProtocolName.holding_detail_query;
		}
	}
}
