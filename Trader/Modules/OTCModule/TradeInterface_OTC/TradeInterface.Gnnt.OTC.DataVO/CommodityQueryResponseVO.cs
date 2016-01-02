using System;
using System.Collections.Generic;
namespace TradeInterface.Gnnt.OTC.DataVO
{
	public class CommodityQueryResponseVO : ResponseVO
	{
		public Dictionary<string, CommodityInfo> CommodityInfoList = new Dictionary<string, CommodityInfo>();
	}
}
