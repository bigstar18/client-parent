using HttpTrade.Gnnt.MEBS.Lib;
using System;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class LogonReqVO : ReqVO
	{
		private string USER_ID;
		private string PASSWORD;
		private string REGISTER_WORD;
		private string VERSIONINFO;
		private string LA;
		private string L_M;
		private string LOGONTYPE;
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
		public string RegisterWord
		{
			get
			{
				return this.REGISTER_WORD;
			}
			set
			{
				this.REGISTER_WORD = value;
			}
		}
		public string VersionInfo
		{
			get
			{
				return this.VERSIONINFO;
			}
			set
			{
				this.VERSIONINFO = value;
			}
		}
		public string La
		{
			get
			{
				return this.LA;
			}
			set
			{
				this.LA = value;
			}
		}
		public string LoginMark
		{
			get
			{
				return this.L_M;
			}
			set
			{
				this.L_M = value;
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
		public LogonReqVO()
		{
			base.Name = ProtocolName.logon;
		}
	}
}
