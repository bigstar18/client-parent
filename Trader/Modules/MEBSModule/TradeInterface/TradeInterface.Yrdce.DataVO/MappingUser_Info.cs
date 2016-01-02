using System;
namespace TradeInterface.Gnnt.DataVO
{
	public class MappingUser_Info
	{
		private string moduleID = string.Empty;
		private string mappingUserID = string.Empty;
		public string ModuleID
		{
			get
			{
				return this.moduleID;
			}
			set
			{
				this.moduleID = value;
			}
		}
		public string MappingUserID
		{
			get
			{
				return this.mappingUserID;
			}
			set
			{
				this.mappingUserID = value;
			}
		}
	}
}
