using System;
using System.Collections.Generic;
namespace HttpTrade.Gnnt.OTC.VO
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
