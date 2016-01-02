using System;
using System.Collections.Generic;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class GetMappingUserRepResultList
	{
		private List<MappingUserInfo> REC;
		public List<MappingUserInfo> MappingUser_InfoList
		{
			get
			{
				return this.REC;
			}
		}
	}
}
