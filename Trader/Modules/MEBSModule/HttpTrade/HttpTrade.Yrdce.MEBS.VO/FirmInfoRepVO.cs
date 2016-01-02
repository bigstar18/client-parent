using System;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class FirmInfoRepVO : RepVO
	{
		private FirmInfoRepResult RESULT;
		private FirmInfoResultList RESULTLIST;
		public FirmInfoRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
		public FirmInfoResultList ResultList
		{
			get
			{
				return this.RESULTLIST;
			}
		}
	}
}
