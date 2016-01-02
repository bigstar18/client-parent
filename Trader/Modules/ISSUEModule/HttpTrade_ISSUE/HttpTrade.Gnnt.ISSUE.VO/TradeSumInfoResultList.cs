using System;
using System.Collections.Generic;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class TradeSumInfoResultList
	{
		private List<M_TradeSumInfo> REC;
		public List<M_TradeSumInfo> TradeSumInfoList
		{
			get
			{
				return this.REC;
			}
		}
	}
}
