using FuturesTrade.Gnnt.Library;
using System;
using System.Data;
using TPME.Log;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.DBService.Query
{
	public class QueryHolding
	{
		public DataSet QueryHoldingInfo(object queryHoldingRequestVO)
		{
			HoldingQueryRequestVO holdingQueryRequestVO = (HoldingQueryRequestVO)queryHoldingRequestVO;
			HoldingQueryResponseVO holdingQueryResponseVO = null;
			if (holdingQueryRequestVO != null)
			{
				holdingQueryResponseVO = Global.TradeLibrary.HoldingQuery(holdingQueryRequestVO);
				TradeDataInfo.holdingInfoList = holdingQueryResponseVO.HoldingInfoList;
			}
			DataSet dataSet = new DataSet("holdingDataSet");
			DataTable dataTable = new DataTable("Holding");
			DataColumn column = new DataColumn("CommodityID");
			DataColumn column2 = new DataColumn("TransactionsCode");
			DataColumn column3 = new DataColumn("BuyHolding", typeof(int));
			DataColumn column4 = new DataColumn("BuyAvg", typeof(double));
			DataColumn column5 = new DataColumn("SellHolding", typeof(int));
			DataColumn column6 = new DataColumn("SellAvg", typeof(double));
			DataColumn column7 = new DataColumn("Margin", typeof(double));
			DataColumn column8 = new DataColumn("Floatpl", typeof(double));
			DataColumn column9 = new DataColumn("BuyVHolding", typeof(int));
			DataColumn column10 = new DataColumn("SellVHolding", typeof(int));
			DataColumn column11 = new DataColumn("GoodsQty", typeof(int));
			DataColumn column12 = new DataColumn("NewPriceLP", typeof(double));
			DataColumn column13 = new DataColumn("Market");
			DataColumn column14 = new DataColumn("NetHolding", typeof(int));
			DataColumn column15 = new DataColumn("AllHolding", typeof(int));
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
			dataTable.Columns.Add(column13);
			dataTable.Columns.Add(column14);
			dataTable.Columns.Add(column15);
			dataSet.Tables.Add(dataTable);
			if (holdingQueryResponseVO != null && holdingQueryResponseVO.RetCode != 0L)
			{
				Logger.wirte(MsgType.Error, "持仓查询错误：" + holdingQueryResponseVO.RetMessage);
				return dataSet;
			}
			for (int i = 0; i < TradeDataInfo.holdingInfoList.Count; i++)
			{
				HoldingInfo holdingInfo = TradeDataInfo.holdingInfoList[i];
				DataRow dataRow = dataTable.NewRow();
				dataRow["CommodityID"] = holdingInfo.CommodityID;
				dataRow["TransactionsCode"] = holdingInfo.CustomerID;
				dataRow["BuyHolding"] = holdingInfo.BuyHolding;
				dataRow["BuyAvg"] = holdingInfo.BuyAverage;
				dataRow["SellHolding"] = holdingInfo.SellHolding;
				dataRow["SellAvg"] = holdingInfo.SellAverage;
				dataRow["Margin"] = holdingInfo.Bail;
				dataRow["Floatpl"] = holdingInfo.FloatingLP;
				dataRow["BuyVHolding"] = holdingInfo.BuyVHolding;
				dataRow["SellVHolding"] = holdingInfo.SellVHolding;
				dataRow["GoodsQty"] = holdingInfo.GOQuantity;
				dataRow["NewPriceLP"] = holdingInfo.NewPriceLP;
				dataRow["Market"] = holdingInfo.MarketID;
				dataRow["NetHolding"] = holdingInfo.BuyHolding - holdingInfo.SellHolding;
				dataRow["AllHolding"] = holdingInfo.SellHolding + holdingInfo.BuyHolding;
				dataTable.Rows.Add(dataRow);
			}
			return dataSet;
		}
	}
}
