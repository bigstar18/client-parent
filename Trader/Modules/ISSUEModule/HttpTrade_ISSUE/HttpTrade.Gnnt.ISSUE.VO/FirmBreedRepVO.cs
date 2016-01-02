using System;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class FirmBreedRepVO : RepVO
	{
		private FirmBreedRepResult RESULT;
		private FirmBreedResultList RESULTLIST;
		public FirmBreedRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
		public FirmBreedResultList ResultList
		{
			get
			{
				return this.RESULTLIST;
			}
		}
	}
}
