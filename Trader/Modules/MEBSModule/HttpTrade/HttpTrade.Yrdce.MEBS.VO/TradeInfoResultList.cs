using System;
using System.Collections.Generic;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class TradeInfoResultList
	{
		private List<M_TradeInfo> REC;
		public List<M_TradeInfo> TradeInfoList
		{
			get
			{
				return this.REC;
			}
		}
	}
}
