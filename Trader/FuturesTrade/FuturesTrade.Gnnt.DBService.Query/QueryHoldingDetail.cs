using FuturesTrade.Gnnt.Library;
using System;
using System.Data;
using TPME.Log;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.DBService.Query
{
	public class QueryHoldingDetail
	{
		public DataSet QueryHoldingDetailInfo(object queryHoldingDetailRequestVO)
		{
			HoldingDetailRequestVO holdingDetailRequestVO = (HoldingDetailRequestVO)queryHoldingDetailRequestVO;
			HoldingDetailResponseVO holdingDetailResponseVO = null;
			if (holdingDetailRequestVO != null)
			{
				holdingDetailResponseVO = Global.TradeLibrary.HoldPtByPrice(holdingDetailRequestVO);
				TradeDataInfo.holdingDetailInfoList = holdingDetailResponseVO.HoldingDetailInfoList;
			}
			DataSet dataSet = new DataSet("holdingDetailDataSet");
			DataTable dataTable = new DataTable("HoldingDetail");
			DataColumn column = new DataColumn("CommodityID");
			DataColumn column2 = new DataColumn("TransactionsCode");
			DataColumn column3 = new DataColumn("B_S");
			DataColumn column4 = new DataColumn("Cur_Open", typeof(int));
			DataColumn column5 = new DataColumn("Price", typeof(double));
			DataColumn column6 = new DataColumn("Margin", typeof(double));
			DataColumn column7 = new DataColumn("GoodsQty", typeof(int));
			DataColumn column8 = new DataColumn("DeadLine");
			DataColumn column9 = new DataColumn("RemainDay");
			DataColumn column10 = new DataColumn("holddate");
			dataTable.Columns.Add(column);
			dataTable.Columns.Add(column2);
			dataTable.Columns.Add(column4);
			dataTable.Columns.Add(column3);
			dataTable.Columns.Add(column5);
			dataTable.Columns.Add(column6);
			dataTable.Columns.Add(column7);
			dataTable.Columns.Add(column8);
			dataTable.Columns.Add(column9);
			dataTable.Columns.Add(column10);
			dataSet.Tables.Add(dataTable);
			if (holdingDetailResponseVO != null && holdingDetailResponseVO.RetCode != 0L)
			{
				Logger.wirte(MsgType.Error, "订货明细查询错误：" + holdingDetailResponseVO.RetMessage);
				return dataSet;
			}
			for (int i = 0; i < TradeDataInfo.holdingDetailInfoList.Count; i++)
			{
				HoldingDetailInfo holdingDetailInfo = TradeDataInfo.holdingDetailInfoList[i];
				DataRow dataRow = dataTable.NewRow();
				dataRow["CommodityID"] = holdingDetailInfo.CommodityID;
				dataRow["TransactionsCode"] = holdingDetailInfo.CustomerID;
				dataRow["Price"] = holdingDetailInfo.Price;
				dataRow["B_S"] = Global.BuySellStrArr[(int)holdingDetailInfo.BuySell];
				dataRow["Margin"] = holdingDetailInfo.Bail;
				dataRow["Cur_Open"] = holdingDetailInfo.AmountOnOrder;
				dataRow["GoodsQty"] = holdingDetailInfo.GOQuantity;
				dataRow["DeadLine"] = holdingDetailInfo.DeadLine;
				dataRow["RemainDay"] = holdingDetailInfo.RemainDay;
				dataRow["holddate"] = holdingDetailInfo.holddate;
				dataTable.Rows.Add(dataRow);
			}
			return dataSet;
		}
	}
}
