using System;
using System.Collections.Generic;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class AccountInfoList
	{
		private List<Account_Info> REC;
		public List<Account_Info> AccountList
		{
			get
			{
				return this.REC;
			}
			set
			{
				this.REC = value;
			}
		}
	}
}
