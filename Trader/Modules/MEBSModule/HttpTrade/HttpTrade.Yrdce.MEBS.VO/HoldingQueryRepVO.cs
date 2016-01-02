using System;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class HoldingQueryRepVO : RepVO
	{
		private HoldingQueryRepResult RESULT;
		private HoldingInfoResultList RESULTLIST;
		public HoldingQueryRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
		public HoldingInfoResultList ResultList
		{
			get
			{
				return this.RESULTLIST;
			}
		}
	}
}
