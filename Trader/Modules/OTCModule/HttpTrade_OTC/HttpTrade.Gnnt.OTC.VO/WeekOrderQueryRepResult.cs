using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class WeekOrderQueryRepResult
	{
		private string RETCODE;
		private string MESSAGE;
		private string TTLREC;
		private string UT;
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
		public long UpdateTime
		{
			get
			{
				return Tools.StrToLong(this.UT);
			}
		}
	}
}
