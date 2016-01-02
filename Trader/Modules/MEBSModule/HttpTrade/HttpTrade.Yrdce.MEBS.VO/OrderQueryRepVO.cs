using System;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class OrderQueryRepVO : RepVO
	{
		private OderQueryRepResult RESULT;
		private OderInfoResultList RESULTLIST;
		public OderQueryRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
		public OderInfoResultList ResultList
		{
			get
			{
				return this.RESULTLIST;
			}
		}
	}
}
