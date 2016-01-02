using FuturesTrade.Gnnt.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using TPME.Log;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.DBService.Query.QueryOrder
{
	public class QueryOrder : IQueryOrder
	{
		private object weekOrderLock = new object();
		public void UpdateOrderInfo(List<OrderInfo> orderInfoList)
		{
			int count = TradeDataInfo.OrderInfoList.Count;
			object obj;
			Monitor.Enter(obj = this.weekOrderLock);
			try
			{
				for (int i = 0; i < orderInfoList.Count; i++)
				{
					bool flag = false;
					for (int j = 0; j < count; j++)
					{
						if (orderInfoList[i].OrderNO == TradeDataInfo.OrderInfoList[j].OrderNO)
						{
							TradeDataInfo.OrderInfoList[j] = orderInfoList[i];
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						TradeDataInfo.OrderInfoList.Add(orderInfoList[i]);
					}
				}
			}
			finally
			{
				Monitor.Exit(obj);
			}
		}
		public void UpdateOrderStatus(long orderNo, short status)
		{
			int count = TradeDataInfo.OrderInfoList.Count;
			for (int i = 0; i < count; i++)
			{
				if (TradeDataInfo.OrderInfoList[i].OrderNO == orderNo)
				{
					TradeDataInfo.OrderInfoList[i].State = status;
					return;
				}
			}
		}
		public DataSet QueryUnOrderDataSet(object QueryUnOrderReqVO)
		{
			OrderQueryRequestVO orderQueryRequestVO = (OrderQueryRequestVO)QueryUnOrderReqVO;
			OrderQueryResponseVO orderQueryResponseVO = null;
			if (orderQueryRequestVO != null)
			{
				orderQueryResponseVO = Global.TradeLibrary.AllOrderQuery(orderQueryRequestVO);
				this.UpdateOrderInfo(orderQueryResponseVO.OrderInfoList);
			}
			List<OrderInfo> list = TradeDataInfo.OrderInfoList.FindAll((OrderInfo o) => o.State == 1 || o.State == 2);
			DataSet dataSet = new DataSet("orderDataSet");
			DataTable dataTable = new DataTable("Order");
			DataColumn column = new DataColumn("OrderNo", typeof(long));
			DataColumn column2 = new DataColumn("Time");
			DataColumn column3 = new DataColumn("TransactionsCode");
			DataColumn column4 = new DataColumn("CommodityID");
			DataColumn column5 = new DataColumn("B_S");
			DataColumn column6 = new DataColumn("O_L");
			DataColumn column7 = new DataColumn("Price", typeof(double));
			DataColumn column8 = new DataColumn("Qty", typeof(int));
			DataColumn column9 = new DataColumn("Balance", typeof(double));
			DataColumn column10 = new DataColumn("Status");
			DataColumn column11 = new DataColumn("Market");
			DataColumn column12 = new DataColumn("CBasis");
			DataColumn column13 = new DataColumn("BillTradeType");
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
			dataSet.Tables.Add(dataTable);
			if (TradeDataInfo.OrderInfoList.Count == 0 && orderQueryResponseVO != null && orderQueryResponseVO.RetCode != 0L)
			{
				Logger.wirte(MsgType.Error, "未成交委托查询QueryUnOrderDataSet错误：" + orderQueryResponseVO.RetMessage);
				return dataSet;
			}
			for (int i = 0; i < list.Count; i++)
			{
				OrderInfo orderInfo = list[i];
				DataRow dataRow = dataTable.NewRow();
				dataRow["OrderNo"] = orderInfo.OrderNO;
				dataRow["Time"] = Global.toTime(orderInfo.Time);
				dataRow["TransactionsCode"] = orderInfo.CustomerID;
				dataRow["CommodityID"] = orderInfo.CommodityID;
				dataRow["B_S"] = Global.BuySellStrArr[(int)orderInfo.BuySell];
				dataRow["O_L"] = Global.SettleBasisStrArr[(int)orderInfo.SettleBasis];
				dataRow["Price"] = orderInfo.OrderPrice;
				dataRow["Qty"] = orderInfo.OrderQuantity;
				dataRow["Balance"] = orderInfo.Balance;
				dataRow["Status"] = Global.OrderStatusStrArr[(int)orderInfo.State];
				dataRow["Market"] = orderInfo.MarketID;
				dataRow["CBasis"] = Global.CBasisStrArr[(int)orderInfo.CBasis];
				dataRow["BillTradeType"] = Global.BillTradeTypeStrArr[(int)orderInfo.BillTradeType];
				dataTable.Rows.Add(dataRow);
			}
			return dataSet;
		}
		public DataSet QueryAllOrderDataSet(object QueryAllOrderReqVO)
		{
			OrderQueryRequestVO orderQueryRequestVO = (OrderQueryRequestVO)QueryAllOrderReqVO;
			OrderQueryResponseVO orderQueryResponseVO = null;
			if (orderQueryRequestVO != null)
			{
				orderQueryResponseVO = Global.TradeLibrary.AllOrderQuery(orderQueryRequestVO);
				this.UpdateOrderInfo(orderQueryResponseVO.OrderInfoList);
			}
			List<OrderInfo> orderInfoList = TradeDataInfo.OrderInfoList;
			DataSet dataSet = new DataSet("orderDataSet");
			DataTable dataTable = new DataTable("Order");
			DataColumn column = new DataColumn("OrderNo", typeof(long));
			DataColumn column2 = new DataColumn("Time");
			DataColumn column3 = new DataColumn("TransactionsCode");
			DataColumn column4 = new DataColumn("CommodityID");
			DataColumn column5 = new DataColumn("B_S");
			DataColumn column6 = new DataColumn("O_L");
			DataColumn column7 = new DataColumn("Price", typeof(double));
			DataColumn column8 = new DataColumn("Qty", typeof(int));
			DataColumn column9 = new DataColumn("Balance", typeof(double));
			DataColumn column10 = new DataColumn("Status");
			DataColumn column11 = new DataColumn("Market");
			DataColumn column12 = new DataColumn("CBasis");
			DataColumn column13 = new DataColumn("BillTradeType");
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
			dataSet.Tables.Add(dataTable);
			if (TradeDataInfo.OrderInfoList.Count == 0 && orderQueryResponseVO != null && orderQueryResponseVO.RetCode != 0L)
			{
				Logger.wirte(MsgType.Error, "所有委托查询QueryAllOrderDataSet错误：" + orderQueryResponseVO.RetMessage);
				return dataSet;
			}
			for (int i = 0; i < orderInfoList.Count; i++)
			{
				OrderInfo orderInfo = orderInfoList[i];
				DataRow dataRow = dataTable.NewRow();
				dataRow["OrderNo"] = orderInfo.OrderNO;
				dataRow["Time"] = Global.toTime(orderInfo.Time);
				dataRow["TransactionsCode"] = orderInfo.CustomerID;
				dataRow["CommodityID"] = orderInfo.CommodityID;
				dataRow["B_S"] = Global.BuySellStrArr[(int)orderInfo.BuySell];
				dataRow["O_L"] = Global.SettleBasisStrArr[(int)orderInfo.SettleBasis];
				dataRow["Price"] = orderInfo.OrderPrice;
				dataRow["Qty"] = orderInfo.OrderQuantity;
				dataRow["Balance"] = orderInfo.Balance;
				dataRow["Status"] = Global.OrderStatusStrArr[(int)orderInfo.State];
				dataRow["Market"] = orderInfo.MarketID;
				dataRow["CBasis"] = Global.CBasisStrArr[(int)orderInfo.CBasis];
				dataRow["BillTradeType"] = Global.BillTradeTypeStrArr[(int)orderInfo.BillTradeType];
				dataTable.Rows.Add(dataRow);
			}
			return dataSet;
		}
		public void CalculateTotalData(long quantity)
		{
		}
	}
}
