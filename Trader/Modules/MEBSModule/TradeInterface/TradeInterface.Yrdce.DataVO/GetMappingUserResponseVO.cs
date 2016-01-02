using System;
using System.Collections.Generic;
namespace TradeInterface.Gnnt.DataVO
{
	public class GetMappingUserResponseVO : ResponseVO
	{
		private string mappingUser = string.Empty;
		public List<MappingUser_Info> MappingUser_InfoList = new List<MappingUser_Info>();
		public string MappingUser
		{
			get
			{
				return this.mappingUser;
			}
			set
			{
				this.mappingUser = value;
			}
		}
	}
}
