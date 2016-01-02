using System;
using System.Collections.Generic;
namespace TradeInterface.Gnnt.ISSUE.DataVO
{
	public class SysTimeQueryResponseVO : ResponseVO
	{
		public string CurrentTime = string.Empty;
		public string CurrentDate = string.Empty;
		public string MicroSecond = string.Empty;
		public List<Broadcast> BroadcastList = new List<Broadcast>();
		public short NewTrade;
		public long TradeTotal;
		public string TradeDay = string.Empty;
		public List<TradeMessage> TradeMessageList = new List<TradeMessage>();
	}
}
