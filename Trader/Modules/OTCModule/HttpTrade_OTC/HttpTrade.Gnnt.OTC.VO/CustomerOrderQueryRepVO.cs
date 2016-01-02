using System;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class CustomerOrderQueryRepVO : RepVO
	{
		private CustomerOrderQueryRepResult RESULT;
		private CustomerOrderQueryResultList RESULTLIST;
		public CustomerOrderQueryRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
		public CustomerOrderQueryResultList ResultList
		{
			get
			{
				return this.RESULTLIST;
			}
		}
	}
}
