using System;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class WithdrawLossProfitRepVO : RepVO
	{
		private WithdrawLossProfitRepResult RESULT;
		public WithdrawLossProfitRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
	}
}
