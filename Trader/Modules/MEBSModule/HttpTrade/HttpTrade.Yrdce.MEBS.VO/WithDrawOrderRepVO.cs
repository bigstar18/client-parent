using System;
namespace HttpTrade.Gnnt.MEBS.VO
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
