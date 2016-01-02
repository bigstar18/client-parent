using FuturesTrade.Gnnt.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using ToolsLibrary.util;
using TPME.Log;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.DBService.Query.QueryOrder
{
	public class QueryPingOrder : IQueryOrder
	{
		private long weekOrderPagingUpdateTime;
		private List<OrderInfo> unOrderListBuffer = new List<OrderInfo>();
		private int orderAllNum;
		private int orderQty;
		private int orderUnTradeNum;
		private int unOrderCurPageNum;
		private List<OrderInfo> allOrderListBuffer = new List<OrderInfo>();
		private int allOrderAllNum;
		private int allOrderQty;
		private int allOrderUnTradeNum;
		private int allOrderCurPageNum;
		private bool weekOrderQueryFirst = true;
		private object weekOrderLock = new object();
		public long WeekOrderUpdateTime
		{
			get
			{
				return this.weekOrderPagingUpdateTime;
			}
			set
			{
				this.weekOrderPagingUpdateTime = value;
			}
		}
		public void UpdateOrderInfo(List<OrderInfo> orderInfoList)
		{
			object obj;
			Monitor.Enter(obj = this.weekOrderLock);
			try
			{
				if (this.unOrderListBuffer.Count == 0)
				{
					this.unOrderListBuffer.AddRange(orderInfoList);
				}
				else
				{
					if (this.allOrderListBuffer.Count == 0)
					{
						this.allOrderListBuffer.AddRange(orderInfoList);
					}
					else
					{
						this.ReplaceOrderInfo(orderInfoList);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(MsgType.Error, ex.StackTrace + ex.Message);
			}
			finally
			{
				Monitor.Exit(obj);
			}
		}
		private void ReplaceOrderInfo(List<OrderInfo> orderInfoList)
		{
			int count = this.unOrderListBuffer.Count;
			int count2 = this.allOrderListBuffer.Count;
			for (int i = 0; i < orderInfoList.Count; i++)
			{
				bool flag = false;
				for (int j = 0; j < count; j++)
				{
					if (orderInfoList[i].OrderNO == this.unOrderListBuffer[j].OrderNO)
					{
						this.unOrderListBuffer[j] = orderInfoList[i];
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					if ((orderInfoList[i].State == 1 || orderInfoList[i].State == 2) && this.unOrderCurPageNum <= 1)
					{
						this.unOrderListBuffer.Add(orderInfoList[i]);
					}
				}
				else
				{
					flag = false;
				}
				for (int k = 0; k < count2; k++)
				{
					if (orderInfoList[i].OrderNO == this.allOrderListBuffer[k].OrderNO)
					{
						this.allOrderListBuffer[k] = orderInfoList[i];
						flag = true;
						break;
					}
				}
				if (!flag && this.allOrderCurPageNum <= 1)
				{
					this.allOrderListBuffer.Add(orderInfoList[i]);
				}
			}
		}
		public void UpdateOrderStatus(long orderNo, short status)
		{
			int count = this.unOrderListBuffer.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.unOrderListBuffer[i].OrderNO == orderNo)
				{
					this.unOrderListBuffer[i].State = status;
					break;
				}
			}
			int count2 = this.allOrderListBuffer.Count;
			for (int j = 0; j < count2; j++)
			{
				if (this.allOrderListBuffer[j].OrderNO == orderNo)
				{
					this.allOrderListBuffer[j].State = status;
					return;
				}
			}
		}
		public void CalculateTotalData(long quantity)
		{
			this.orderAllNum++;
			this.allOrderAllNum++;
			this.orderQty += Tools.StrToInt(quantity.ToString(), 0);
			this.allOrderQty += Tools.StrToInt(quantity.ToString(), 0);
			this.orderUnTradeNum += Tools.StrToInt(quantity.ToString(), 0);
			this.allOrderUnTradeNum += Tools.StrToInt(quantity.ToString(), 0);
		}
		public DataSet QueryUnOrderDataSet(object QueryUnOrderReqVO)
		{
			OrderQueryPagingResponseVO orderQueryPagingResponseVO = this.QueryPagingOrder(QueryUnOrderReqVO);
			List<OrderInfo> list = this.unOrderListBuffer.FindAll((OrderInfo o) => o.State == 1 || o.State == 2);
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
			DataColumn column9 = new DataColumn("Balance", typeof(int));
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
			dataTable.Columns.Add(new DataColumn("AutoID", typeof(int)));
			dataTable.Columns["AutoID"].ColumnMapping = MappingType.Hidden;
			dataTable.Columns.Add(column13);
			dataSet.Tables.Add(dataTable);
			if (orderQueryPagingResponseVO != null && orderQueryPagingResponseVO.RetCode != 0L)
			{
				Logger.wirte(MsgType.Error, "未成交委托分页查询QueryUnOrderDataSet错误：" + orderQueryPagingResponseVO.RetMessage);
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
			DataRow dataRow2 = dataTable.NewRow();
			dataRow2["Time"] = "合计";
			dataRow2["TransactionsCode"] = "共" + this.orderAllNum.ToString() + "条";
			dataRow2["Qty"] = this.orderQty;
			dataRow2["Balance"] = this.orderUnTradeNum;
			dataRow2["AutoID"] = 100000;
			dataTable.Rows.Add(dataRow2);
			return dataSet;
		}
		public DataSet QueryAllOrderDataSet(object QueryAllOrderReqVO)
		{
			OrderQueryPagingResponseVO orderQueryPagingResponseVO = this.QueryPagingOrder(QueryAllOrderReqVO);
			List<OrderInfo> list = this.allOrderListBuffer;
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
			DataColumn column9 = new DataColumn("Balance", typeof(int));
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
			dataTable.Columns.Add(new DataColumn("AutoID", typeof(int)));
			dataTable.Columns["AutoID"].ColumnMapping = MappingType.Hidden;
			dataTable.Columns.Add(column13);
			dataSet.Tables.Add(dataTable);
			if (orderQueryPagingResponseVO != null && orderQueryPagingResponseVO.RetCode != 0L)
			{
				Logger.wirte(MsgType.Error, "全部委托分页查询QueryAllOrderDataSet错误：" + orderQueryPagingResponseVO.RetMessage);
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
			DataRow dataRow2 = dataTable.NewRow();
			dataRow2["Time"] = "合计";
			dataRow2["TransactionsCode"] = "共" + this.allOrderAllNum.ToString() + "条";
			dataRow2["Qty"] = this.allOrderQty;
			dataRow2["Balance"] = this.allOrderUnTradeNum;
			dataRow2["AutoID"] = 100000;
			dataTable.Rows.Add(dataRow2);
			return dataSet;
		}
		private OrderQueryPagingResponseVO QueryPagingOrder(object QueryPagingOrderReqVO)
		{
			OrderQueryPagingRequestVO orderQueryPagingRequestVO = (OrderQueryPagingRequestVO)QueryPagingOrderReqVO;
			OrderQueryPagingResponseVO orderQueryPagingResponseVO = null;
			if (orderQueryPagingRequestVO != null)
			{
				if (orderQueryPagingRequestVO.CurrentPagNum > 0)
				{
					if (orderQueryPagingRequestVO.IsQueryAll == 0)
					{
						this.allOrderCurPageNum = orderQueryPagingRequestVO.CurrentPagNum;
						this.allOrderListBuffer.Clear();
					}
					else
					{
						if (orderQueryPagingRequestVO.IsQueryAll == 1)
						{
							this.unOrderCurPageNum = orderQueryPagingRequestVO.CurrentPagNum;
							this.unOrderListBuffer.Clear();
						}
					}
				}
				if (orderQueryPagingRequestVO.CurrentPagNum == 0)
				{
					orderQueryPagingRequestVO.UpdateTime = this.weekOrderPagingUpdateTime;
					if (orderQueryPagingRequestVO.IsQueryAll == 0 && this.weekOrderQueryFirst)
					{
						orderQueryPagingRequestVO.UpdateTime = 0L;
						this.weekOrderQueryFirst = false;
					}
				}
				if (orderQueryPagingRequestVO.UpdateTime == 0L && orderQueryPagingRequestVO.CurrentPagNum == 0)
				{
					orderQueryPagingRequestVO.CurrentPagNum = 1;
				}
				orderQueryPagingResponseVO = Global.TradeLibrary.AllOrderQueryPaging(orderQueryPagingRequestVO);
				this.UpdateOrderInfo(orderQueryPagingResponseVO.OrderInfoList);
				if (orderQueryPagingRequestVO.IsQueryAll == 1)
				{
					this.orderAllNum = orderQueryPagingResponseVO.TotalRow.TotalNum;
					this.orderQty = orderQueryPagingResponseVO.TotalRow.TotalQty;
					this.orderUnTradeNum = orderQueryPagingResponseVO.TotalRow.UnTradeQty;
					if (this.unOrderCurPageNum == 0)
					{
						this.weekOrderPagingUpdateTime = orderQueryPagingResponseVO.UpdateTime;
					}
				}
				else
				{
					if (orderQueryPagingRequestVO.IsQueryAll == 0)
					{
						this.allOrderAllNum = orderQueryPagingResponseVO.TotalRow.TotalNum;
						this.allOrderQty = orderQueryPagingResponseVO.TotalRow.TotalQty;
						this.allOrderUnTradeNum = orderQueryPagingResponseVO.TotalRow.UnTradeQty;
						if (this.allOrderCurPageNum == 0)
						{
							this.weekOrderPagingUpdateTime = orderQueryPagingResponseVO.UpdateTime;
						}
					}
				}
			}
			return orderQueryPagingResponseVO;
		}
	}
}
