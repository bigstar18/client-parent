using System;
using System.Collections.Generic;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class WeekOrderInfoResultList
	{
		private List<WeekOrderInfo> REC;
		public List<WeekOrderInfo> WeekOrderList
		{
			get
			{
				return this.REC;
			}
		}
	}
}
