using System;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class TradeSumQueryRepVO : RepVO
	{
		private TradeSumQueryRepResult RESULT;
		private TradeSumInfoResultList RESULTLIST;
		public TradeSumQueryRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
		public TradeSumInfoResultList ResultList
		{
			get
			{
				return this.RESULTLIST;
			}
		}
	}
}
