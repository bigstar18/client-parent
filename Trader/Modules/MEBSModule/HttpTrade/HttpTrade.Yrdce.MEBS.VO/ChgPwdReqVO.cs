using HttpTrade.Gnnt.MEBS.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class ChgPwdReqVO : ReqVO
	{
		private string USER_ID;
		private string OLD_PASSWORD;
		private string NEW_PASSWORD;
		private string SESSION_ID;
		public string UserID
		{
			get
			{
				return this.USER_ID;
			}
			set
			{
				this.USER_ID = value;
			}
		}
		public string OldPassword
		{
			get
			{
				return this.OLD_PASSWORD;
			}
			set
			{
				this.OLD_PASSWORD = value;
			}
		}
		public string NewPassword
		{
			get
			{
				return this.NEW_PASSWORD;
			}
			set
			{
				this.NEW_PASSWORD = value;
			}
		}
		public long SessionID
		{
			get
			{
				return Tools.StrToLong(this.SESSION_ID);
			}
			set
			{
				this.SESSION_ID = value.ToString();
			}
		}
		public ChgPwdReqVO()
		{
			base.Name = ProtocolName.change_password;
		}
	}
}
