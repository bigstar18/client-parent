using System;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class M_EspecialMemberQuery
	{
		private string E_M_ID;
		private string E_M_N;
		public string EspecialMemberID
		{
			get
			{
				return this.E_M_ID;
			}
			set
			{
				this.E_M_ID = value;
			}
		}
		public string EspecialMemberName
		{
			get
			{
				return this.E_M_N;
			}
			set
			{
				this.E_M_N = value;
			}
		}
	}
}
