using System;
namespace TradeInterface.Gnnt.DataVO
{
	public class CheckMappingUserRequestVO
	{
		private string userID = string.Empty;
		private string moduleID = string.Empty;
		private string password = string.Empty;
		public string UserID
		{
			get
			{
				return this.userID;
			}
			set
			{
				this.userID = value;
			}
		}
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
		public string Password
		{
			get
			{
				return this.password;
			}
			set
			{
				this.password = value;
			}
		}
	}
}
