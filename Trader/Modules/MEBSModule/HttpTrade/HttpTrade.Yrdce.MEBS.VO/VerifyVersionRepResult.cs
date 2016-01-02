using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class VerifyVersionRepResult
	{
		private string RETCODE = string.Empty;
		private string MESSAGE = string.Empty;
		private string MODULE_ID = string.Empty;
		private string SERVERVERSION = string.Empty;
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
		public string ServerVersion
		{
			get
			{
				return this.SERVERVERSION;
			}
		}
	}
}
