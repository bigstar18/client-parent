using System;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class ConditionOrderRepVO : RepVO
	{
		private ConditionOrderRepResult RESULT;
		public ConditionOrderRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
	}
}
