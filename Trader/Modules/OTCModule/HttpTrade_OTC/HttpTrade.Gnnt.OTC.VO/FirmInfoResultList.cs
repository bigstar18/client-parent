using System;
using System.Collections.Generic;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class FirmInfoResultList
	{
		private List<FirmInfo> REC;
		public List<FirmInfo> FirmInfoList
		{
			get
			{
				return this.REC;
			}
		}
	}
}
