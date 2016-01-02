using System;
using System.Collections.Generic;
namespace HttpTrade.Gnnt.OTC.VO
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
			set
			{
				this.REC = value;
			}
		}
	}
}
