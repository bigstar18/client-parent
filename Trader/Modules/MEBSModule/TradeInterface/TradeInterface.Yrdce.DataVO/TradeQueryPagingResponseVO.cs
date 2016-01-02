using System;
using System.Collections.Generic;
namespace TradeInterface.Gnnt.DataVO
{
	public class TradeQueryPagingResponseVO : RecResponseVO
	{
		public List<TradeInfo> TradeInfoList = new List<TradeInfo>();
		public TradeInfoTotalRow TradeTotalRow = new TradeInfoTotalRow();
	}
}
