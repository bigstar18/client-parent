using System;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class MarketQueryRepVO : RepVO
	{
		private MarketQueryRepResult RESULT;
		private MarketQueryResultList RESULTLIST;
		public MarketQueryRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
		public MarketQueryResultList ResultList
		{
			get
			{
				return this.RESULTLIST;
			}
		}
	}
}
