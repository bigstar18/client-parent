using HttpTrade.Gnnt.ISSUE.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class HoldingDetailReqVO : ReqVO
	{
		private string USER_ID;
		private string COMMODITY_ID;
		private string STARTNUM;
		private string RECCNT;
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
		public HoldingDetailReqVO()
		{
			base.Name = ProtocolName.holdpositionbyprice;
		}
	}
}
