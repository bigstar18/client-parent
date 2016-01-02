using System;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class Account_Info
	{
		private string MWUSER;
		private string PASSWORD;
		public string MwUser
		{
			get
			{
				return this.MWUSER;
			}
			set
			{
				this.MWUSER = value;
			}
		}
		public string Password
		{
			get
			{
				return this.PASSWORD;
			}
			set
			{
				this.PASSWORD = value;
			}
		}
	}
}
