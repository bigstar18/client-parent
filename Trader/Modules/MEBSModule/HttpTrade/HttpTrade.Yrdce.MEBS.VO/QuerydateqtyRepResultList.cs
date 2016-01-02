using System;
using System.Collections.Generic;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class QuerydateqtyRepResultList
	{
		private List<ResultListREC> REC;
		public List<ResultListREC> ResultListRec
		{
			get
			{
				return this.REC;
			}
			set
			{
				this.REC = value;
			}
		}
	}
}
