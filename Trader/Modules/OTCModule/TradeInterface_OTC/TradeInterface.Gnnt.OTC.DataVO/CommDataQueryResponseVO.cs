using System;
using System.Collections.Generic;
namespace TradeInterface.Gnnt.OTC.DataVO
{
	public class CommDataQueryResponseVO : ResponseVO
	{
		public Dictionary<string, CommData> CommDataList = new Dictionary<string, CommData>();
	}
}
