using System;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class ResultListREC
	{
		private T_TotalRow TOTALROW;
		public T_TotalRow TotalRowList
		{
			get
			{
				return this.TOTALROW;
			}
			set
			{
				this.TOTALROW = value;
			}
		}
	}
}
