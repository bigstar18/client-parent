using HttpTrade.Gnnt.ISSUE.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class CheckUserReqVO : ReqVO
	{
		private string USER_ID;
		private string SESSION_ID;
		private string MODULE_ID;
		private string F_LOGONTYPE;
		private string LOGONTYPE;
		public string FromLogonType
		{
			get
			{
				return this.F_LOGONTYPE;
			}
			set
			{
				this.F_LOGONTYPE = value;
			}
		}
		public string LogonType
		{
			get
			{
				return this.LOGONTYPE;
			}
			set
			{
				this.LOGONTYPE = value;
			}
		}
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
		public string ModuleID
		{
			get
			{
				return this.MODULE_ID;
			}
			set
			{
				this.MODULE_ID = value;
			}
		}
		public CheckUserReqVO()
		{
			base.Name = ProtocolName.check_user;
		}
	}
}
