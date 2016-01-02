using System;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class OrderSRepVO : RepVO
	{
		private OrderSRepResult RESULT;
		public OrderSRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
	}
}
