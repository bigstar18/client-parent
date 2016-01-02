using System;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class TradePagingQueryRepVO : RepVO
	{
		private TradePagingQueryRepResult RESULT;
		private TradeInfoResultList RESULTLIST;
		public TradePagingQueryRepResult Result
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
