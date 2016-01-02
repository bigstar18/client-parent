using HttpTrade.Gnnt.MEBS.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	internal class ChgMappingPwdReqVO : ReqVO
	{
		private string USER_ID = string.Empty;
		private string MDID = string.Empty;
		private string OLD_PASSWORD;
		private string NEW_PASSWORD = string.Empty;
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
		public string ModuleID
		{
			get
			{
				return this.MDID;
			}
			set
			{
				this.MDID = value;
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
		public ChgMappingPwdReqVO()
		{
			base.Name = ProtocolName.change_mapping_password;
		}
	}
}
