using System;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class LogonRepVO : RepVO
	{
		private LogonRepResult RESULT;
		public LogonRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
	}
}
