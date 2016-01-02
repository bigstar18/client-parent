namespace FuturesTrade.Gnnt.DBService.Query.QueryOrder
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using TradeInterface.Gnnt.DataVO;

    public interface IQueryOrder
    {
        void CalculateTotalData(long quantity);
        DataSet QueryAllOrderDataSet(object QueryAllOrderReqVO);
        DataSet QueryUnOrderDataSet(object QueryUnOrderReqVO);
        void UpdateOrderInfo(List<OrderInfo> orderInfoList);
        void UpdateOrderStatus(long orderNo, short status);
    }
}
