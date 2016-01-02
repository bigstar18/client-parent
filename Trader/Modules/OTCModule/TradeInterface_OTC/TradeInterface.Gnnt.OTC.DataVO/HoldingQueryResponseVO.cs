using System;
using System.Collections.Generic;
namespace TradeInterface.Gnnt.OTC.DataVO
{
	public class HoldingQueryResponseVO : RecResponseVO
	{
		public List<HoldingInfo> HoldingInfoList = new List<HoldingInfo>();
	}
}
