using System;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class CheckUserRepVO : RepVO
	{
		private CheckUserRepResult RESULT;
		public CheckUserRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
	}
}
