using System;
namespace HttpTrade.Gnnt.OTC.VO
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
