using System;
using System.Collections.Generic;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class CommDataResultList
	{
		private List<M_CommData> REC;
		public List<M_CommData> CommDataList
		{
			get
			{
				return this.REC;
			}
		}
	}
}
