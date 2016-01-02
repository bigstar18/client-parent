using System;
using System.Collections.Generic;
namespace TradeInterface.Gnnt.OTC.DataVO
{
	public class SysTimeQueryResponseVO : ResponseVO
	{
		public string CurrentTime = string.Empty;
		public string CurrentDate = string.Empty;
		public string MicroSecond = string.Empty;
		public short NewTrade;
		public long TradeTotal;
		public long LastID;
		public string TradeDay = string.Empty;
		public string UpdateData;
		public short SystemStatus;
		public List<TradeMessage> TradeMessageList = new List<TradeMessage>();
	}
}
