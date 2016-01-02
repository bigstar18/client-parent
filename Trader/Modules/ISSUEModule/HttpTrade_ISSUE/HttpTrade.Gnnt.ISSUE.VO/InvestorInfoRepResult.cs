using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class InvestorInfoRepResult
	{
		private string RETCODE;
		private string MESSAGE;
		private string ACCOUNT;
		private string NAME;
		private string BANK;
		private string PHONE;
		private string IDNUM;
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
		public string Account
		{
			get
			{
				return this.ACCOUNT;
			}
		}
		public string Name
		{
			get
			{
				return this.NAME;
			}
		}
		public string Bank
		{
			get
			{
				return this.BANK;
			}
		}
		public string Phone
		{
			get
			{
				return this.PHONE;
			}
		}
		public string IDNum
		{
			get
			{
				return this.IDNUM;
			}
		}
	}
}
