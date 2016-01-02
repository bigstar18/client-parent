using System;
using System.Collections.Generic;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class FirmFundsInfoResultList
	{
		private List<M_FirmFundsInfo> REC;
		public List<M_FirmFundsInfo> FirmFundsInfoList
		{
			get
			{
				return this.REC;
			}
		}
	}
}
