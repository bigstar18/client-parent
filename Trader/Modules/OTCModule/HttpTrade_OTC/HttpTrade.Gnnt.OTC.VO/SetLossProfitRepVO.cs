using System;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class SetLossProfitRepVO : RepVO
	{
		private SetLossProfitRepResult RESULT;
		public SetLossProfitRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
	}
}
