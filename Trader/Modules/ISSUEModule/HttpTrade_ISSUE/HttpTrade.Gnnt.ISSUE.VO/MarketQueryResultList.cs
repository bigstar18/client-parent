using System;
using System.Collections.Generic;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class MarketQueryResultList
	{
		private List<M_MarkeInfo> REC;
		public List<M_MarkeInfo> MarkeInfoList
		{
			get
			{
				return this.REC;
			}
		}
	}
}
