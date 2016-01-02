using System;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class FirmHoldSumRepVO : RepVO
	{
		private FirmHoldSumRepResult RESULT;
		private FirmHoldSumResultList RESULTLIST;
		public FirmHoldSumRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
		public FirmHoldSumResultList ResultList
		{
			get
			{
				return this.RESULTLIST;
			}
		}
	}
}
