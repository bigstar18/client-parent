using System;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class QuerydateqtyRepVO : RepVO
	{
		private QuerydateqtyRepResult RESULT;
		private QuerydateqtyRepResultList RESULTLIST;
		public QuerydateqtyRepResult Result
		{
			get
			{
				return this.RESULT;
			}
			set
			{
				this.RESULT = value;
			}
		}
		public QuerydateqtyRepResultList ResultList
		{
			get
			{
				return this.RESULTLIST;
			}
			set
			{
				this.RESULTLIST = value;
			}
		}
	}
}
