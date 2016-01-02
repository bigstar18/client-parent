using System;
using System.Data;
namespace FuturesTrade.Gnnt.DBService.Query.QueryTrade
{
	public interface IQueryTrade
	{
		DataSet QueryTradeOrderDataSet(object queryTradeOrderInfoReqVO);
		DataSet QueryTradeDataSet(object queryTradeInfoReqVO);
	}
}
