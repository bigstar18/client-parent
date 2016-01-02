using System;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class CommodityQueryRepVO : RepVO
	{
		private CommodityQueryRepResult RESULT;
		private CommodityQueryResultList RESULTLIST;
		public CommodityQueryRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
		public CommodityQueryResultList ResultList
		{
			get
			{
				return this.RESULTLIST;
			}
		}
	}
}
