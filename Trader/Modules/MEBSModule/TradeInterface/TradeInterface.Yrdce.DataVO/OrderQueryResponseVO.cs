using System;
using System.Collections.Generic;
namespace TradeInterface.Gnnt.DataVO
{
	public class OrderQueryResponseVO : RecResponseVO
	{
		public long UpdateTime;
		public List<OrderInfo> OrderInfoList = new List<OrderInfo>();
	}
}
