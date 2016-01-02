using FuturesTrade.Gnnt.Library;
using System;
using System.Collections.Generic;
using System.Data;
using TPME.Log;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.DBService.Query
{
	public class QueryConOrder
	{
		private void UpdatetradeInfo(List<ConditionQueryOrderInfo> tradeInfoList)
		{
			int count = TradeDataInfo.ConditionOrderInfoList.Count;
			for (int i = 0; i < tradeInfoList.Count; i++)
			{
				bool flag = false;
				for (int j = 0; j < count; j++)
				{
					if (tradeInfoList[i].OrderNO == TradeDataInfo.ConditionOrderInfoList[j].OrderNO)
					{
						TradeDataInfo.ConditionOrderInfoList[j] = tradeInfoList[i];
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					TradeDataInfo.ConditionOrderInfoList.Add(tradeInfoList[i]);
				}
			}
		}
		public DataSet QueryConditionOrderInfo(object queryConditionOrderReqVO)
		{
			ConditionQueryRequestVO conditionQueryRequestVO = (ConditionQueryRequestVO)queryConditionOrderReqVO;
			ConditionQueryResponseVO conditionQueryResponseVO = null;
			if (conditionQueryRequestVO != null)
			{
				conditionQueryResponseVO = Global.TradeLibrary.ConditionQuery(conditionQueryRequestVO);
				List<ConditionQueryOrderInfo> conditionQueryInfoList = conditionQueryResponseVO.ConditionQueryInfoList;
				this.UpdatetradeInfo(conditionQueryInfoList);
			}
			DataSet dataSet = new DataSet("ConditionDataSet");
			DataTable dataTable = new DataTable("Corder");
			DataColumn column = new DataColumn("OrderNo", typeof(long));
			DataColumn column2 = new DataColumn("CommodityID");
			DataColumn column3 = new DataColumn("B_S");
			DataColumn column4 = new DataColumn("O_L");
			DataColumn column5 = new DataColumn("Price", typeof(double));
			DataColumn column6 = new DataColumn("Qty", typeof(long));
			DataColumn column7 = new DataColumn("ConditionCommodityID");
			DataColumn column8 = new DataColumn("CoditionType");
			DataColumn column9 = new DataColumn("ConditionSign");
			DataColumn column10 = new DataColumn("ConditionPrice", typeof(double));
			DataColumn column11 = new DataColumn("PrepareTime");
			DataColumn column12 = new DataColumn("MatureTime");
			DataColumn column13 = new DataColumn("OrderTime");
			DataColumn column14 = new DataColumn("OrderState");
			DataColumn column15 = new DataColumn("TransactionsCode");
			dataTable.Columns.Add(column);
			dataTable.Columns.Add(column2);
			dataTable.Columns.Add(column14);
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
			dataTable.Columns.Add(column15);
			dataSet.Tables.Add(dataTable);
			if (TradeDataInfo.ConditionOrderInfoList.Count == 0 && conditionQueryResponseVO != null && conditionQueryResponseVO.RetCode != 0L)
			{
				Logger.wirte(MsgType.Error, "条件下单查询错误：" + conditionQueryResponseVO.RetMessage);
				return dataSet;
			}
			for (int i = 0; i < TradeDataInfo.ConditionOrderInfoList.Count; i++)
			{
				ConditionQueryOrderInfo conditionQueryOrderInfo = TradeDataInfo.ConditionOrderInfoList[i];
				DataRow dataRow = dataTable.NewRow();
				dataRow["OrderNo"] = conditionQueryOrderInfo.OrderNO;
				dataRow["CommodityID"] = conditionQueryOrderInfo.CommodityID;
				dataRow["B_S"] = Global.BuySellStrArr[(int)conditionQueryOrderInfo.BuySell_Type];
				dataRow["O_L"] = Global.SettleBasisStrArr[(int)conditionQueryOrderInfo.SettleBasis];
				dataRow["Price"] = conditionQueryOrderInfo.OrderPrice;
				dataRow["Qty"] = conditionQueryOrderInfo.OrderQuantity;
				dataRow["ConditionCommodityID"] = conditionQueryOrderInfo.Condition_CommodityID;
				dataRow["CoditionType"] = Global.ConditionTypeStrArr[(int)conditionQueryOrderInfo.ConditionType];
				dataRow["ConditionSign"] = Global.ConditionSignStrArr[(int)(conditionQueryOrderInfo.ConditionOperator + 2)];
				dataRow["ConditionPrice"] = conditionQueryOrderInfo.ConditionPrice;
				dataRow["PrepareTime"] = conditionQueryOrderInfo.PrePareTime.Substring(0, 10);
				dataRow["MatureTime"] = conditionQueryOrderInfo.MatureTime;
				if (conditionQueryOrderInfo.OrderTime.Length > 1)
				{
					dataRow["OrderTime"] = Convert.ToDateTime(conditionQueryOrderInfo.OrderTime).ToString();
				}
				else
				{
					dataRow["OrderTime"] = conditionQueryOrderInfo.OrderTime;
				}
				dataRow["OrderState"] = conditionQueryOrderInfo.OrderStare;
				dataRow["TransactionsCode"] = conditionQueryOrderInfo.CustomerID;
				dataTable.Rows.Add(dataRow);
			}
			return dataSet;
		}
	}
}
