using HttpTrade.Gnnt.OTC.Lib;
using System;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class LogonReqVO : ReqVO
	{
		private string USER_ID;
		private string PASSWORD;
		private string REGISTER_WORD;
		private string VERSIONINFO;
		private string LOGONTYPE;
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
		public LogonReqVO()
		{
			base.Name = ProtocolName.logon;
		}
	}
}
