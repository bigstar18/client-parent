using System;
namespace HttpTrade.Gnnt.ISSUE.VO
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
