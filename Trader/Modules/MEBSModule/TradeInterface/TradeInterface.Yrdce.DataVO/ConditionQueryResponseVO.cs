using System;
using System.Collections.Generic;
namespace TradeInterface.Gnnt.DataVO
{
	public class ConditionQueryResponseVO : RecResponseVO
	{
		public long UpdateTime;
		public ConditionTotalInfo TotalInfo = new ConditionTotalInfo();
		public List<ConditionQueryOrderInfo> ConditionQueryInfoList = new List<ConditionQueryOrderInfo>();
	}
}
