using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class GetMappingUserRepResult
	{
		private string RETCODE;
		private string MESSAGE;
		private string MUSER;
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
		public string MUser
		{
			get
			{
				return this.MUSER;
			}
		}
	}
}
