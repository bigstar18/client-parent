using System;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class HoldingDetailRepVO : RepVO
	{
		private HoldingDetailRepResult RESULT;
		private HoldingDetailInfoResultList RESULTLIST;
		public HoldingDetailRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
		public HoldingDetailInfoResultList ResultList
		{
			get
			{
				return this.RESULTLIST;
			}
		}
	}
}
