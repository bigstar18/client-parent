using System;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class AgencyLogonRepVO : RepVO
	{
		private AgencyLogonRepResult RESULT;
		public AgencyLogonRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
	}
}
