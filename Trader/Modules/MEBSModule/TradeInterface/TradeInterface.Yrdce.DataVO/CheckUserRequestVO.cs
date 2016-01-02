using System;
namespace TradeInterface.Gnnt.DataVO
{
	public class CheckUserRequestVO
	{
		private string userID;
		private string fromLogonType;
		private string logonType;
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
		public string FromLogonType
		{
			get
			{
				return this.fromLogonType;
			}
			set
			{
				this.fromLogonType = value;
			}
		}
		public string LogonType
		{
			get
			{
				return this.logonType;
			}
			set
			{
				this.logonType = value;
			}
		}
	}
}
