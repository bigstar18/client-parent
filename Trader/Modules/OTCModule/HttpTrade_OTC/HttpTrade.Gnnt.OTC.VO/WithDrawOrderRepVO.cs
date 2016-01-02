using System;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class WithDrawOrderRepVO : RepVO
	{
		private WithDrawOrderRepResult RESULT;
		public WithDrawOrderRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
	}
}
