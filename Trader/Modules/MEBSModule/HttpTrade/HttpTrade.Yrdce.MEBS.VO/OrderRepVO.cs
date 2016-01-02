using System;
namespace HttpTrade.Gnnt.MEBS.VO
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
