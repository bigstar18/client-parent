using System;
namespace TradeInterface.Gnnt.DataVO
{
	public class ChgMappingPwdRequestVO
	{
		private string userID = string.Empty;
		private string moduleID = string.Empty;
		private string oldPassword = string.Empty;
		private string newPassword = string.Empty;
		private long sessionID;
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
		public string OldPassword
		{
			get
			{
				return this.oldPassword;
			}
			set
			{
				this.oldPassword = value;
			}
		}
		public string NewPassword
		{
			get
			{
				return this.newPassword;
			}
			set
			{
				this.newPassword = value;
			}
		}
		public long SessionID
		{
			get
			{
				return this.sessionID;
			}
			set
			{
				this.sessionID = value;
			}
		}
	}
}
