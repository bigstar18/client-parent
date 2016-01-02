using System;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class OrderXRepVO : RepVO
	{
		private OrderXRepResult RESULT;
		public OrderXRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
	}
}
