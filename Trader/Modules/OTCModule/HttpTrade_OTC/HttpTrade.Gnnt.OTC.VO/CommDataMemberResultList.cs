using System;
using System.Collections.Generic;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class CommDataMemberResultList
	{
		private List<M_CommDataMember> REC;
		public List<M_CommDataMember> CommDataList
		{
			get
			{
				return this.REC;
			}
			set
			{
				this.REC = value;
			}
		}
	}
}
