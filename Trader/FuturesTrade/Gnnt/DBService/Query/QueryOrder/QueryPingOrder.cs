namespace FuturesTrade.Gnnt.DBService.Query.QueryOrder
{
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading;
    using ToolsLibrary.util;
    using TPME.Log;
    using TradeInterface.Gnnt.DataVO;

    public class QueryPingOrder : IQueryOrder
    {
        private int allOrderAllNum;
        private int allOrderCurPageNum;
        private List<OrderInfo> allOrderListBuffer = new List<OrderInfo>();
        private int allOrderQty;
        private int allOrderUnTradeNum;
        private int orderAllNum;
        private int orderQty;
        private int orderUnTradeNum;
        private int unOrderCurPageNum;
        private List<OrderInfo> unOrderListBuffer = new List<OrderInfo>();
        private object weekOrderLock = new object();
        private long weekOrderPagingUpdateTime;
        private bool weekOrderQueryFirst = true;

        public void CalculateTotalData(long quantity)
        {
            this.orderAllNum++;
            this.allOrderAllNum++;
            this.orderQty += Tools.StrToInt(quantity.ToString(), 0);
            this.allOrderQty += Tools.StrToInt(quantity.ToString(), 0);
            this.orderUnTradeNum += Tools.StrToInt(quantity.ToString(), 0);
            this.allOrderUnTradeNum += Tools.StrToInt(quantity.ToString(), 0);
        }

        public DataSet QueryAllOrderDataSet(object QueryAllOrderReqVO)
        {
            OrderQueryPagingResponseVO evo = this.QueryPagingOrder(QueryAllOrderReqVO);
            List<OrderInfo> allOrderListBuffer = this.allOrderListBuffer;
            DataSet set = new DataSet("orderDataSet");
            DataTable table = new DataTable("Order");
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
            table.Columns.Add(column);
            table.Columns.Add(column2);
            table.Columns.Add(column3);
            table.Columns.Add(column4);
            table.Columns.Add(column5);
            table.Columns.Add(column6);
            table.Columns.Add(column7);
            table.Columns.Add(column8);
            table.Columns.Add(column9);
            table.Columns.Add(column10);
            table.Columns.Add(column11);
            table.Columns.Add(column12);
            table.Columns.Add(new DataColumn("AutoID", typeof(int)));
            table.Columns["AutoID"].ColumnMapping = MappingType.Hidden;
            table.Columns.Add(column13);
            set.Tables.Add(table);
            if ((evo != null) && (evo.RetCode != 0L))
            {
                Logger.wirte(MsgType.Error, "全部委托分页查询QueryAllOrderDataSet错误：" + evo.RetMessage);
                return set;
            }
            for (int i = 0; i < allOrderListBuffer.Count; i++)
            {
                OrderInfo info = allOrderListBuffer[i];
                DataRow row = table.NewRow();
                row["OrderNo"] = info.OrderNO;
                row["Time"] = Global.toTime(info.Time);
                row["TransactionsCode"] = info.CustomerID;
                row["CommodityID"] = info.CommodityID;
                row["B_S"] = Global.BuySellStrArr[info.BuySell];
                row["O_L"] = Global.SettleBasisStrArr[info.SettleBasis];
                row["Price"] = info.OrderPrice;
                row["Qty"] = info.OrderQuantity;
                row["Balance"] = info.Balance;
                row["Status"] = Global.OrderStatusStrArr[info.State];
                row["Market"] = info.MarketID;
                row["CBasis"] = Global.CBasisStrArr[info.CBasis];
                row["BillTradeType"] = Global.BillTradeTypeStrArr[info.BillTradeType];
                table.Rows.Add(row);
            }
            DataRow row2 = table.NewRow();
            row2["Time"] = "合计";
            row2["TransactionsCode"] = "共" + this.allOrderAllNum.ToString() + "条";
            row2["Qty"] = this.allOrderQty;
            row2["Balance"] = this.allOrderUnTradeNum;
            row2["AutoID"] = 0x186a0;
            table.Rows.Add(row2);
            return set;
        }

        private OrderQueryPagingResponseVO QueryPagingOrder(object QueryPagingOrderReqVO)
        {
            OrderQueryPagingRequestVO req = (OrderQueryPagingRequestVO)QueryPagingOrderReqVO;
            OrderQueryPagingResponseVO evo = null;
            if (req != null)
            {
                if (req.CurrentPagNum > 0)
                {
                    if (req.IsQueryAll == 0)
                    {
                        this.allOrderCurPageNum = req.CurrentPagNum;
                        this.allOrderListBuffer.Clear();
                    }
                    else if (req.IsQueryAll == 1)
                    {
                        this.unOrderCurPageNum = req.CurrentPagNum;
                        this.unOrderListBuffer.Clear();
                    }
                }
                if (req.CurrentPagNum == 0)
                {
                    req.UpdateTime = this.weekOrderPagingUpdateTime;
                    if ((req.IsQueryAll == 0) && this.weekOrderQueryFirst)
                    {
                        req.UpdateTime = 0L;
                        this.weekOrderQueryFirst = false;
                    }
                }
                if ((req.UpdateTime == 0L) && (req.CurrentPagNum == 0))
                {
                    req.CurrentPagNum = 1;
                }
                evo = Global.TradeLibrary.AllOrderQueryPaging(req);
                this.UpdateOrderInfo(evo.OrderInfoList);
                if (req.IsQueryAll == 1)
                {
                    this.orderAllNum = evo.TotalRow.TotalNum;
                    this.orderQty = evo.TotalRow.TotalQty;
                    this.orderUnTradeNum = evo.TotalRow.UnTradeQty;
                    if (this.unOrderCurPageNum == 0)
                    {
                        this.weekOrderPagingUpdateTime = evo.UpdateTime;
                    }
                    return evo;
                }
                if (req.IsQueryAll == 0)
                {
                    this.allOrderAllNum = evo.TotalRow.TotalNum;
                    this.allOrderQty = evo.TotalRow.TotalQty;
                    this.allOrderUnTradeNum = evo.TotalRow.UnTradeQty;
                    if (this.allOrderCurPageNum == 0)
                    {
                        this.weekOrderPagingUpdateTime = evo.UpdateTime;
                    }
                }
            }
            return evo;
        }

        public DataSet QueryUnOrderDataSet(object QueryUnOrderReqVO)
        {
            OrderQueryPagingResponseVO evo = this.QueryPagingOrder(QueryUnOrderReqVO);
            List<OrderInfo> list = this.unOrderListBuffer.FindAll(delegate (OrderInfo o) {
                if (o.State != 1)
                {
                    return o.State == 2;
                }
                return true;
            });
            DataSet set = new DataSet("orderDataSet");
            DataTable table = new DataTable("Order");
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
            table.Columns.Add(column);
            table.Columns.Add(column2);
            table.Columns.Add(column3);
            table.Columns.Add(column4);
            table.Columns.Add(column5);
            table.Columns.Add(column6);
            table.Columns.Add(column7);
            table.Columns.Add(column8);
            table.Columns.Add(column9);
            table.Columns.Add(column10);
            table.Columns.Add(column11);
            table.Columns.Add(column12);
            table.Columns.Add(new DataColumn("AutoID", typeof(int)));
            table.Columns["AutoID"].ColumnMapping = MappingType.Hidden;
            table.Columns.Add(column13);
            set.Tables.Add(table);
            if ((evo != null) && (evo.RetCode != 0L))
            {
                Logger.wirte(MsgType.Error, "未成交委托分页查询QueryUnOrderDataSet错误：" + evo.RetMessage);
                return set;
            }
            for (int i = 0; i < list.Count; i++)
            {
                OrderInfo info = list[i];
                DataRow row = table.NewRow();
                row["OrderNo"] = info.OrderNO;
                row["Time"] = Global.toTime(info.Time);
                row["TransactionsCode"] = info.CustomerID;
                row["CommodityID"] = info.CommodityID;
                row["B_S"] = Global.BuySellStrArr[info.BuySell];
                row["O_L"] = Global.SettleBasisStrArr[info.SettleBasis];
                row["Price"] = info.OrderPrice;
                row["Qty"] = info.OrderQuantity;
                row["Balance"] = info.Balance;
                row["Status"] = Global.OrderStatusStrArr[info.State];
                row["Market"] = info.MarketID;
                row["CBasis"] = Global.CBasisStrArr[info.CBasis];
                row["BillTradeType"] = Global.BillTradeTypeStrArr[info.BillTradeType];
                table.Rows.Add(row);
            }
            DataRow row2 = table.NewRow();
            row2["Time"] = "合计";
            row2["TransactionsCode"] = "共" + this.orderAllNum.ToString() + "条";
            row2["Qty"] = this.orderQty;
            row2["Balance"] = this.orderUnTradeNum;
            row2["AutoID"] = 0x186a0;
            table.Rows.Add(row2);
            return set;
        }

        private void ReplaceOrderInfo(List<OrderInfo> orderInfoList)
        {
            int count = this.unOrderListBuffer.Count;
            int num2 = this.allOrderListBuffer.Count;
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
                    if (((orderInfoList[i].State == 1) || (orderInfoList[i].State == 2)) && (this.unOrderCurPageNum <= 1))
                    {
                        this.unOrderListBuffer.Add(orderInfoList[i]);
                    }
                }
                else
                {
                    flag = false;
                }
                for (int k = 0; k < num2; k++)
                {
                    if (orderInfoList[i].OrderNO == this.allOrderListBuffer[k].OrderNO)
                    {
                        this.allOrderListBuffer[k] = orderInfoList[i];
                        flag = true;
                        break;
                    }
                }
                if (!flag && (this.allOrderCurPageNum <= 1))
                {
                    this.allOrderListBuffer.Add(orderInfoList[i]);
                }
            }
        }

        public void UpdateOrderInfo(List<OrderInfo> orderInfoList)
        {
            object obj2;
            Monitor.Enter(obj2 = this.weekOrderLock);
            try
            {
                if (this.unOrderListBuffer.Count == 0)
                {
                    this.unOrderListBuffer.AddRange(orderInfoList);
                }
                else if (this.allOrderListBuffer.Count == 0)
                {
                    this.allOrderListBuffer.AddRange(orderInfoList);
                }
                else
                {
                    this.ReplaceOrderInfo(orderInfoList);
                }
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.StackTrace + exception.Message);
            }
            finally
            {
                Monitor.Exit(obj2);
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
            int num3 = this.allOrderListBuffer.Count;
            for (int j = 0; j < num3; j++)
            {
                if (this.allOrderListBuffer[j].OrderNO == orderNo)
                {
                    this.allOrderListBuffer[j].State = status;
                    return;
                }
            }
        }

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
    }
}
