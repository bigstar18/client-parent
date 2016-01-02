using System;
using System.Collections.Generic;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class CustomerOrderQueryResultList
	{
		private List<M_CustomerOrderQuery> REC;
		public List<M_CustomerOrderQuery> CustomerOrderQueryList
		{
			get
			{
				return this.REC;
			}
		}
	}
}
