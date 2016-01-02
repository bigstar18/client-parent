using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class CheckUserRepResult
	{
		private string RETCODE;
		private string MESSAGE;
		private string MODULE_ID;
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
	}
}
