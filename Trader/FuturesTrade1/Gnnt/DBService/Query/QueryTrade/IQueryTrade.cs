namespace FuturesTrade.Gnnt.DBService.Query.QueryTrade
{
    using System;
    using System.Data;

    public interface IQueryTrade
    {
        DataSet QueryTradeDataSet(object queryTradeInfoReqVO);
        DataSet QueryTradeOrderDataSet(object queryTradeOrderInfoReqVO);
    }
}
