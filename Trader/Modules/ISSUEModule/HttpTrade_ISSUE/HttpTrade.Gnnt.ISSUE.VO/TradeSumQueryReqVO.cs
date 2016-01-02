using HttpTrade.Gnnt.ISSUE.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class TradeSumQueryReqVO : ReqVO
	{
		private string USER_ID;
		private string SESSION_ID;
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
		public TradeSumQueryReqVO()
		{
			base.Name = ProtocolName.trade_sum_query;
		}
	}
}
