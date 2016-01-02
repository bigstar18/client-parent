using System;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class VerifyVersionRepVO : RepVO
	{
		private VerifyVersionRepResult RESULT;
		public VerifyVersionRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
	}
}
