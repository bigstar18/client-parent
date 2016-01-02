using System;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class CommDataQueryRepVO : RepVO
	{
		private CommDataQueryRepResult RESULT;
		private CommDataResultList RESULTLIST;
		public CommDataQueryRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
		public CommDataResultList ResultList
		{
			get
			{
				return this.RESULTLIST;
			}
		}
	}
}
