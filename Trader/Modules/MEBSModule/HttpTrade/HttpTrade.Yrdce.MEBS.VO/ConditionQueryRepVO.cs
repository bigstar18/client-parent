using System;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class ConditionQueryRepVO : RepVO
	{
		private ConditionOrder_QueryRepResult RESULT;
		private ConditionOrder_QueryResultList RESULTLIST;
		public ConditionOrder_QueryRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
		public ConditionOrder_QueryResultList ResultList
		{
			get
			{
				return this.RESULTLIST;
			}
		}
	}
}
