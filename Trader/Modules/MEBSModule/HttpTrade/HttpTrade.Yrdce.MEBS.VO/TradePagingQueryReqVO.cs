using HttpTrade.Gnnt.MEBS.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class TradePagingQueryReqVO : ReqVO
	{
		private string USER_ID;
		private string LAST_TRADE_ID;
		private string RECCNT;
		private string SESSION_ID;
		private string MARKET_ID;
		private string SORTFLD;
		private string ISDESC;
		private string PAGENUM;
		private string PRI;
		private string TYPE;
		private string SE_F;
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
		public int CurrentPageNum
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
		public short Se_t
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
		public TradePagingQueryReqVO()
		{
			base.Name = ProtocolName.tradepagingquery;
		}
	}
}
