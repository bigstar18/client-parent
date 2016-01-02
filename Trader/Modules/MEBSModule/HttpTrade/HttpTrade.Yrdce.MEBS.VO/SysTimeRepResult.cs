using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class SysTimeRepResult
	{
		private string RETCODE;
		private string MESSAGE;
		private string CU_T;
		private string CU_D;
		private string TV_U;
		private string L_ID;
		private BroadcastList BCRS;
		private string NEW_T;
		private string TD_TTL;
		private TradeMessageList TDRP;
		private string DAY;
		private string MARK;
		public long RetCode
		{
			get
			{
				return Tools.StrToLong(this.RETCODE);
			}
		}
		public string RetMessage
		{
			get
			{
				return this.MESSAGE;
			}
		}
		public string CurrentTime
		{
			get
			{
				return this.CU_T;
			}
		}
		public string CurrentDate
		{
			get
			{
				return this.CU_D;
			}
		}
		public string MicroSecond
		{
			get
			{
				return this.TV_U;
			}
		}
		public long LastID
		{
			get
			{
				return Tools.StrToLong(this.L_ID);
			}
		}
		public BroadcastList BroadcastList
		{
			get
			{
				return this.BCRS;
			}
		}
		public short NewTrade
		{
			get
			{
				return Tools.StrToShort(this.NEW_T);
			}
		}
		public long TradeTotal
		{
			get
			{
				return Tools.StrToLong(this.TD_TTL);
			}
		}
		public TradeMessageList TradeMessageList
		{
			get
			{
				return this.TDRP;
			}
		}
		public string TradeDay
		{
			get
			{
				return this.DAY;
			}
		}
		public short RefreshMark
		{
			get
			{
				return Tools.StrToShort(this.MARK);
			}
		}
	}
}
