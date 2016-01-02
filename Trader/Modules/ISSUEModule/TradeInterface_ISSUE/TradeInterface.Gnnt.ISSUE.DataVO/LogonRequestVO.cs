using System;
namespace TradeInterface.Gnnt.ISSUE.DataVO
{
	public class LogonRequestVO
	{
		private string mUserID = string.Empty;
		private string mPassword = string.Empty;
		private string mRegisterWord = string.Empty;
		private string mVersionInfo = string.Empty;
		private string mLogonType = string.Empty;
		public string UserID
		{
			get
			{
				return this.mUserID;
			}
			set
			{
				this.mUserID = value;
			}
		}
		public string Password
		{
			get
			{
				return this.mPassword;
			}
			set
			{
				this.mPassword = value;
			}
		}
		public string RegisterWord
		{
			get
			{
				return this.mRegisterWord;
			}
			set
			{
				this.mRegisterWord = value;
			}
		}
		public string VersionInfo
		{
			get
			{
				return this.mVersionInfo;
			}
			set
			{
				this.mVersionInfo = value;
			}
		}
		public string LogonType
		{
			get
			{
				return this.mLogonType;
			}
			set
			{
				this.mLogonType = value;
			}
		}
	}
}
