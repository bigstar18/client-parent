using System;
using System.Collections.Generic;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class FirmHoldSumResultList
	{
		private List<M_FirmHoldSum> REC;
		public List<M_FirmHoldSum> FirmHoldSumList
		{
			get
			{
				return this.REC;
			}
		}
	}
}
