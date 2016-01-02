using System;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class WeekOrderQueryRepVO : RepVO
	{
		private WeekOrderQueryRepResult RESULT;
		private WeekOrderInfoResultList RESULTLIST;
		public WeekOrderQueryRepResult Result
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
