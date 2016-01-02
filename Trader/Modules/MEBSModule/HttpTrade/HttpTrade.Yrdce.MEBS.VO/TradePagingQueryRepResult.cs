using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class TradePagingQueryRepResult
	{
		private string RETCODE;
		private string MESSAGE;
		private string TTLREC;
		private TradeQueryTotalRow TOTALROW;
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
		public int TotalRecord
		{
			get
			{
				return Tools.StrToInt(this.TTLREC);
			}
		}
		public TradeQueryTotalRow TotalRow
		{
			get
			{
				return this.TOTALROW;
			}
		}
	}
}
