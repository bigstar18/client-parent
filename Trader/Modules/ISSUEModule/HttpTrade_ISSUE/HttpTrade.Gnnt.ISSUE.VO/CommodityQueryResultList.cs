using System;
using System.Collections.Generic;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class CommodityQueryResultList
	{
		private List<M_CommodityInfo> REC;
		public List<M_CommodityInfo> CommodityList
		{
			get
			{
				return this.REC;
			}
		}
	}
}
