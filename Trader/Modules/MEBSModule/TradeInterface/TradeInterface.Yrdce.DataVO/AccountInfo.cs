using System;
namespace TradeInterface.Gnnt.DataVO
{
	public class AccountInfo
	{
		private string mWUserID = string.Empty;
		private string password = string.Empty;
		public string MWUserID
		{
			get
			{
				return this.mWUserID;
			}
			set
			{
				this.mWUserID = value;
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
