using System;
namespace Gnnt.MixAccountPlugin
{
	public class UserInfo
	{
		private string userID;
		private string passWord;
		private string moduleID = string.Empty;
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
		public string PassWord
		{
			get
			{
				return this.passWord;
			}
			set
			{
				this.passWord = value;
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
	}
}
