using System;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class EspecialMemberQueryRepVO : RepVO
	{
		private EspecialMemberQueryRepResult RESULT;
		private EspecialMemberQueryResultList RESULTLIST;
		public EspecialMemberQueryRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
		public EspecialMemberQueryResultList ResultList
		{
			get
			{
				return this.RESULTLIST;
			}
		}
	}
}
