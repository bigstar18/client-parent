using System;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class FirmFundsInfoRepVO : RepVO
	{
		private FirmFundsInfoRepResult RESULT;
		private FirmFundsInfoResultList RESULTLIST;
		public FirmFundsInfoRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
		public FirmFundsInfoResultList ResultList
		{
			get
			{
				return this.RESULTLIST;
			}
		}
	}
}
