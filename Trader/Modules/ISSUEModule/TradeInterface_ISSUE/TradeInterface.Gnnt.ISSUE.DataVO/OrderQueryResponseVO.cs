using System;
using System.Collections.Generic;
namespace TradeInterface.Gnnt.ISSUE.DataVO
{
	public class OrderQueryResponseVO : RecResponseVO
	{
		public long UpdateTime;
		public List<OrderInfo> OrderInfoList = new List<OrderInfo>();
	}
}
