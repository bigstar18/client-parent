using System;
using System.Collections.Generic;
using System.Data;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.DBService.Query.QueryOrder
{
	public interface IQueryOrder
	{
		DataSet QueryUnOrderDataSet(object QueryUnOrderReqVO);
		DataSet QueryAllOrderDataSet(object QueryAllOrderReqVO);
		void UpdateOrderStatus(long orderNo, short status);
		void CalculateTotalData(long quantity);
		void UpdateOrderInfo(List<OrderInfo> orderInfoList);
	}
}
