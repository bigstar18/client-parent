using System;
using System.Collections.Generic;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class HoldingDetailInfoResultList
	{
		private List<M_HoldingDetailInfo> REC;
		public List<M_HoldingDetailInfo> HoldingDetailInfoList
		{
			get
			{
				return this.REC;
			}
		}
	}
}
