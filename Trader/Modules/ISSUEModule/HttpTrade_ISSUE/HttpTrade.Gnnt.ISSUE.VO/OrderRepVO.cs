using System;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class OrderRepVO : RepVO
	{
		private OrderRepResult RESULT;
		public OrderRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
	}
}
