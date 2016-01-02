using System;
using System.Collections.Generic;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class ConditionOrder_QueryResultList
	{
		private List<Condition_Info> REC;
		public List<Condition_Info> Condition_InfoList
		{
			get
			{
				return this.REC;
			}
		}
	}
}
