using System;
using System.Collections.Generic;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class EspecialMemberQueryResultList
	{
		private List<M_EspecialMemberQuery> REC;
		public List<M_EspecialMemberQuery> EspecialMemberQueryList
		{
			get
			{
				return this.REC;
			}
		}
	}
}
