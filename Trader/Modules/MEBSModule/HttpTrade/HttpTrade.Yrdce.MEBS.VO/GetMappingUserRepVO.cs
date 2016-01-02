using System;
namespace HttpTrade.Gnnt.MEBS.VO
{
	internal class GetMappingUserRepVO : RepVO
	{
		private GetMappingUserRepResult RESULT;
		private GetMappingUserRepResultList RESULTLIST;
		public GetMappingUserRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
		public GetMappingUserRepResultList ResultList
		{
			get
			{
				return this.RESULTLIST;
			}
		}
	}
}
