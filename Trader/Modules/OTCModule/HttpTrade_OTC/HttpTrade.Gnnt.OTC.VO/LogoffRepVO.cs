using System;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class LogoffRepVO : RepVO
	{
		private LogoffRepResult RESULT;
		public LogoffRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
	}
}
