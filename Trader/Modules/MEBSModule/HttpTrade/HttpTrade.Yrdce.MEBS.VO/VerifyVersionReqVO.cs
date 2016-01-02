using HttpTrade.Gnnt.MEBS.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class VerifyVersionReqVO : ReqVO
	{
		private string USER_ID;
		private string SESSION_ID;
		private string MODULE_ID;
		private string ClientVERSION;
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
		public string ClientVersion
		{
			get
			{
				return this.ClientVERSION;
			}
			set
			{
				this.ClientVERSION = value;
			}
		}
		public VerifyVersionReqVO()
		{
			base.Name = ProtocolName.verify_version;
		}
	}
}
