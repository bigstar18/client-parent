using System;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class ConditionRevokeRepVO : RepVO
	{
		private RecallConditionOrderRepResult RESULT;
		public RecallConditionOrderRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
	}
}
