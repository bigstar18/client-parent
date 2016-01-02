namespace FuturesTrade.Gnnt.DBService.Query.QueryOrder
{
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using TPME.Log;
    using TradeInterface.Gnnt.DataVO;

    public class QueryOrder : IQueryOrder
    {
        private object weekOrderLock = new object();

        public void CalculateTotalData(long quantity)
        {
        }

        public DataSet QueryAllOrderDataSet(object QueryAllOrderReqVO)
        {
            OrderQueryRequestVO req = (OrderQueryRequestVO)QueryAllOrderReqVO;
            OrderQueryResponseVO evo = null;
            if (req != null)
            {
                evo = Global.TradeLibrary.AllOrderQuery(req);
                this.UpdateOrderInfo(evo.OrderInfoList);
            }
            List<OrderInfo> orderInfoList = TradeDataInfo.OrderInfoList;
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
            DataColumn column9 = new DataColumn("Balance", typeof(double));
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
            table.Columns.Add(column13);
            set.Tables.Add(table);
            if (((TradeDataInfo.OrderInfoList.Count == 0) && (evo != null)) && (evo.RetCode != 0L))
            {
                Logger.wirte(MsgType.Error, "所有委托查询QueryAllOrderDataSet错误：" + evo.RetMessage);
                return set;
            }
            for (int i = 0; i < orderInfoList.Count; i++)
            {
                OrderInfo info = orderInfoList[i];
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
            return set;
        }

        public DataSet QueryUnOrderDataSet(object QueryUnOrderReqVO)
        {
            OrderQueryRequestVO req = (OrderQueryRequestVO)QueryUnOrderReqVO;
            OrderQueryResponseVO evo = null;
            if (req != null)
            {
                evo = Global.TradeLibrary.AllOrderQuery(req);
                this.UpdateOrderInfo(evo.OrderInfoList);
            }
            List<OrderInfo> list = TradeDataInfo.OrderInfoList.FindAll(delegate (OrderInfo o) {
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
            DataColumn column9 = new DataColumn("Balance", typeof(double));
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
            table.Columns.Add(column13);
            set.Tables.Add(table);
            if (((TradeDataInfo.OrderInfoList.Count == 0) && (evo != null)) && (evo.RetCode != 0L))
            {
                Logger.wirte(MsgType.Error, "未成交委托查询QueryUnOrderDataSet错误：" + evo.RetMessage);
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
            return set;
        }

        public void UpdateOrderInfo(List<OrderInfo> orderInfoList)
        {
            int count = TradeDataInfo.OrderInfoList.Count;
            lock (this.weekOrderLock)
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
    }
}
