using System;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class TradeQueryRepVO : RepVO
	{
		private TradeQueryRepResult RESULT;
		private TradeInfoResultList RESULTLIST;
		public TradeQueryRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
		public TradeInfoResultList ResultList
		{
			get
			{
				return this.RESULTLIST;
			}
		}
	}
}
