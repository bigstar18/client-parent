using HttpTrade.Gnnt.ISSUE.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class TradeQueryReqVO : ReqVO
	{
		private string USER_ID;
		private string LAST_TRADE_ID;
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
		public long LastTradeID
		{
			get
			{
				return Tools.StrToLong(this.LAST_TRADE_ID);
			}
			set
			{
				this.LAST_TRADE_ID = value.ToString();
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
		public TradeQueryReqVO()
		{
			base.Name = ProtocolName.tradequery;
		}
	}
}
