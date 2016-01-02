using System;
using System.Collections.Generic;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class OderInfoResultList
	{
		private List<M_OrderInfo> REC;
		public List<M_OrderInfo> OderInfoList
		{
			get
			{
				return this.REC;
			}
		}
	}
}
