using System;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class WeekOrderPagingQueryRepVO : RepVO
	{
		private WeekOrderPagingQueryRepResult RESULT;
		private WeekOrderInfoResultList RESULTLIST;
		public WeekOrderPagingQueryRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
		public WeekOrderInfoResultList ResultList
		{
			get
			{
				return this.RESULTLIST;
			}
		}
	}
}
