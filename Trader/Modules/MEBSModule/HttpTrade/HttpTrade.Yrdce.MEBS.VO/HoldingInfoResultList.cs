using System;
using System.Collections.Generic;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class HoldingInfoResultList
	{
		private List<M_HoldingInfo> REC;
		public List<M_HoldingInfo> HoldingInfoList
		{
			get
			{
				return this.REC;
			}
		}
	}
}
