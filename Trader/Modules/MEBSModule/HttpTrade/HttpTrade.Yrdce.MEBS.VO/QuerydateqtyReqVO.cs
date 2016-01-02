using HttpTrade.Gnnt.MEBS.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class QuerydateqtyReqVO : ReqVO
	{
		private string USER_ID;
		private string COMMODITY_ID;
		private string SESSION_ID;
		private string QUERYNAME;
		private string PARAMETER;
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
		public string QueryName
		{
			get
			{
				return this.QUERYNAME;
			}
			set
			{
				this.QUERYNAME = value;
			}
		}
		public string Parameter
		{
			get
			{
				return this.PARAMETER;
			}
			set
			{
				this.PARAMETER = value;
			}
		}
		public QuerydateqtyReqVO()
		{
			base.Name = ProtocolName.querydateqty;
		}
	}
}
