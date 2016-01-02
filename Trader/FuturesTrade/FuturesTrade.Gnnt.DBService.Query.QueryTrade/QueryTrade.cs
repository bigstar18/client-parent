using FuturesTrade.Gnnt.Library;
using System;
using System.Collections.Generic;
using System.Data;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.DBService.Query.QueryTrade
{
	internal class QueryTrade : IQueryTrade
	{
		private void UpdateTradeInfo(List<TradeInfo> orderInfoList)
		{
			if (TradeDataInfo.tradeInfoList.Count > 0)
			{
				while (orderInfoList.Count > 0 && orderInfoList[0].TradeNO <= TradeDataInfo.tradeInfoList[TradeDataInfo.tradeInfoList.Count - 1].TradeNO)
				{
					orderInfoList.RemoveAt(0);
				}
			}
			TradeDataInfo.tradeInfoList.AddRange(orderInfoList);
		}
		public DataSet QueryTradeOrderDataSet(object queryTradeOrderInfoReqVO)
		{
			TradeQueryRequestVO tradeQueryRequestVO = (TradeQueryRequestVO)queryTradeOrderInfoReqVO;
			if (tradeQueryRequestVO != null)
			{
				TradeQueryResponseVO tradeQueryResponseVO = Global.TradeLibrary.TradeQuery(tradeQueryRequestVO);
				this.UpdateTradeInfo(tradeQueryResponseVO.TradeInfoList);
			}
			List<TradeInfo> tradeInfoList = TradeDataInfo.tradeInfoList;
			DataSet dataSet = new DataSet("tradeDataSet");
			DataTable dataTable = new DataTable("Trade");
			DataColumn column = new DataColumn("TradeNo", typeof(long));
			DataColumn column2 = new DataColumn("OrderNo", typeof(long));
			DataColumn column3 = new DataColumn("Time");
			DataColumn column4 = new DataColumn("TransactionsCode");
			DataColumn column5 = new DataColumn("CommodityID");
			DataColumn column6 = new DataColumn("B_S");
			DataColumn column7 = new DataColumn("O_L");
			DataColumn column8 = new DataColumn("Price", typeof(double));
			DataColumn column9 = new DataColumn("Qty", typeof(int));
			DataColumn column10 = new DataColumn("LPrice", typeof(double));
			DataColumn column11 = new DataColumn("Comm", typeof(double));
			DataColumn column12 = new DataColumn("Market");
			dataTable.Columns.Add(column);
			dataTable.Columns.Add(column2);
			dataTable.Columns.Add(column3);
			dataTable.Columns.Add(column4);
			dataTable.Columns.Add(column5);
			dataTable.Columns.Add(column6);
			dataTable.Columns.Add(column7);
			dataTable.Columns.Add(column8);
			dataTable.Columns.Add(column9);
			dataTable.Columns.Add(column10);
			dataTable.Columns.Add(column11);
			dataTable.Columns.Add(column12);
			dataSet.Tables.Add(dataTable);
			for (int i = 0; i < tradeInfoList.Count; i++)
			{
				TradeInfo tradeInfo = tradeInfoList[i];
				DataRow dataRow = dataTable.NewRow();
				dataRow["TradeNo"] = tradeInfo.TradeNO;
				dataRow["OrderNo"] = tradeInfo.OrderNO;
				dataRow["Time"] = Global.toTime(tradeInfo.TradeTime);
				dataRow["TransactionsCode"] = tradeInfo.CustomerID;
				dataRow["CommodityID"] = tradeInfo.CommodityID;
				dataRow["B_S"] = Global.BuySellStrArr[(int)tradeInfo.BuySell];
				dataRow["O_L"] = Global.SettleBasisStrArr[(int)tradeInfo.SettleBasis];
				dataRow["Price"] = tradeInfo.TradePrice;
				dataRow["Qty"] = tradeInfo.TradeQuantity;
				dataRow["LPrice"] = tradeInfo.TransferPrice;
				dataRow["Comm"] = tradeInfo.Comm;
				dataRow["Market"] = tradeInfo.MarketID;
				dataTable.Rows.Add(dataRow);
			}
			return dataSet;
		}
		public DataSet QueryTradeDataSet(object queryTradeInfoReqVO)
		{
			TradeQueryRequestVO tradeQueryRequestVO = (TradeQueryRequestVO)queryTradeInfoReqVO;
			if (tradeQueryRequestVO != null)
			{
				TradeQueryResponseVO tradeQueryResponseVO = Global.TradeLibrary.TradeQuery(tradeQueryRequestVO);
				this.UpdateTradeInfo(tradeQueryResponseVO.TradeInfoList);
			}
			List<TradeInfo> tradeInfoList = TradeDataInfo.tradeInfoList;
			DataSet dataSet = new DataSet("tradeDataSet");
			DataTable dataTable = new DataTable("Trade");
			DataColumn column = new DataColumn("TradeNo", typeof(long));
			DataColumn column2 = new DataColumn("Time");
			DataColumn column3 = new DataColumn("TransactionsCode");
			DataColumn column4 = new DataColumn("CommodityID");
			DataColumn column5 = new DataColumn("B_S");
			DataColumn column6 = new DataColumn("O_L");
			DataColumn column7 = new DataColumn("Price", typeof(double));
			DataColumn column8 = new DataColumn("Qty", typeof(int));
			DataColumn column9 = new DataColumn("Liqpl", typeof(double));
			DataColumn column10 = new DataColumn("LPrice", typeof(double));
			DataColumn column11 = new DataColumn("Comm", typeof(double));
			DataColumn column12 = new DataColumn("Market");
			dataTable.Columns.Add(column);
			dataTable.Columns.Add(column2);
			dataTable.Columns.Add(column3);
			dataTable.Columns.Add(column4);
			dataTable.Columns.Add(column5);
			dataTable.Columns.Add(column6);
			dataTable.Columns.Add(column7);
			dataTable.Columns.Add(column8);
			dataTable.Columns.Add(column9);
			dataTable.Columns.Add(column10);
			dataTable.Columns.Add(column11);
			dataTable.Columns.Add(column12);
			dataSet.Tables.Add(dataTable);
			for (int i = 0; i < tradeInfoList.Count; i++)
			{
				TradeInfo tradeInfo = tradeInfoList[i];
				DataRow dataRow = dataTable.NewRow();
				dataRow["TradeNo"] = tradeInfo.TradeNO;
				dataRow["Time"] = Global.toTime(tradeInfo.TradeTime);
				dataRow["TransactionsCode"] = tradeInfo.CustomerID;
				dataRow["CommodityID"] = tradeInfo.CommodityID;
				dataRow["B_S"] = Global.BuySellStrArr[(int)tradeInfo.BuySell];
				dataRow["O_L"] = Global.SettleBasisStrArr[(int)tradeInfo.SettleBasis];
				dataRow["Price"] = tradeInfo.TradePrice;
				dataRow["Qty"] = tradeInfo.TradeQuantity;
				dataRow["Liqpl"] = tradeInfo.TransferPL.ToString();
				dataRow["LPrice"] = tradeInfo.TransferPrice;
				dataRow["Comm"] = tradeInfo.Comm;
				dataRow["Market"] = tradeInfo.MarketID;
				dataTable.Rows.Add(dataRow);
			}
			return dataSet;
		}
	}
}
