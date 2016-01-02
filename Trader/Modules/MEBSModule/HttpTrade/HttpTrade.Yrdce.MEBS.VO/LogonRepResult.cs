using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class LogonRepResult
	{
		private string RETCODE = string.Empty;
		private string MESSAGE = string.Empty;
		private string MODULE_ID = string.Empty;
		private string LAST_TIME = string.Empty;
		private string LAST_IP = string.Empty;
		private string CHG_PWD = string.Empty;
		private string RANDOM_KEY = string.Empty;
		private string USERID = string.Empty;
		public long RetCode
		{
			get
			{
				return Tools.StrToLong(this.RETCODE);
			}
		}
		public string RetMessage
		{
			get
			{
				return this.MESSAGE;
			}
		}
		public string ModuleID
		{
			get
			{
				return this.MODULE_ID;
			}
		}
		public string LastTime
		{
			get
			{
				return this.LAST_TIME;
			}
		}
		public string LastIP
		{
			get
			{
				return this.LAST_IP;
			}
		}
		public string ChgPWD
		{
			get
			{
				return this.CHG_PWD;
			}
		}
		public string RandomKey
		{
			get
			{
				return this.RANDOM_KEY;
			}
		}
		public string UserID
		{
			get
			{
				return this.USERID;
			}
		}
	}
}
