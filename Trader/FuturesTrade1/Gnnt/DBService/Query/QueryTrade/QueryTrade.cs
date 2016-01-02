namespace FuturesTrade.Gnnt.DBService.Query.QueryTrade
{
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using TradeInterface.Gnnt.DataVO;

    internal class QueryTrade : IQueryTrade
    {
        public DataSet QueryTradeDataSet(object queryTradeInfoReqVO)
        {
            TradeQueryRequestVO req = (TradeQueryRequestVO)queryTradeInfoReqVO;
            TradeQueryResponseVO evo = null;
            if (req != null)
            {
                evo = Global.TradeLibrary.TradeQuery(req);
                this.UpdateTradeInfo(evo.TradeInfoList);
            }
            List<TradeInfo> tradeInfoList = TradeDataInfo.tradeInfoList;
            DataSet set = new DataSet("tradeDataSet");
            DataTable table = new DataTable("Trade");
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
            set.Tables.Add(table);
            for (int i = 0; i < tradeInfoList.Count; i++)
            {
                TradeInfo info = tradeInfoList[i];
                DataRow row = table.NewRow();
                row["TradeNo"] = info.TradeNO;
                row["Time"] = Global.toTime(info.TradeTime);
                row["TransactionsCode"] = info.CustomerID;
                row["CommodityID"] = info.CommodityID;
                row["B_S"] = Global.BuySellStrArr[info.BuySell];
                row["O_L"] = Global.SettleBasisStrArr[info.SettleBasis];
                row["Price"] = info.TradePrice;
                row["Qty"] = info.TradeQuantity;
                row["Liqpl"] = info.TransferPL.ToString();
                row["LPrice"] = info.TransferPrice;
                row["Comm"] = info.Comm;
                row["Market"] = info.MarketID;
                table.Rows.Add(row);
            }
            return set;
        }

        public DataSet QueryTradeOrderDataSet(object queryTradeOrderInfoReqVO)
        {
            TradeQueryRequestVO req = (TradeQueryRequestVO)queryTradeOrderInfoReqVO;
            TradeQueryResponseVO evo = null;
            if (req != null)
            {
                evo = Global.TradeLibrary.TradeQuery(req);
                this.UpdateTradeInfo(evo.TradeInfoList);
            }
            List<TradeInfo> tradeInfoList = TradeDataInfo.tradeInfoList;
            DataSet set = new DataSet("tradeDataSet");
            DataTable table = new DataTable("Trade");
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
            set.Tables.Add(table);
            for (int i = 0; i < tradeInfoList.Count; i++)
            {
                TradeInfo info = tradeInfoList[i];
                DataRow row = table.NewRow();
                row["TradeNo"] = info.TradeNO;
                row["OrderNo"] = info.OrderNO;
                row["Time"] = Global.toTime(info.TradeTime);
                row["TransactionsCode"] = info.CustomerID;
                row["CommodityID"] = info.CommodityID;
                row["B_S"] = Global.BuySellStrArr[info.BuySell];
                row["O_L"] = Global.SettleBasisStrArr[info.SettleBasis];
                row["Price"] = info.TradePrice;
                row["Qty"] = info.TradeQuantity;
                row["LPrice"] = info.TransferPrice;
                row["Comm"] = info.Comm;
                row["Market"] = info.MarketID;
                table.Rows.Add(row);
            }
            return set;
        }

        private void UpdateTradeInfo(List<TradeInfo> orderInfoList)
        {
            if (TradeDataInfo.tradeInfoList.Count > 0)
            {
                while ((orderInfoList.Count > 0) && (orderInfoList[0].TradeNO <= TradeDataInfo.tradeInfoList[TradeDataInfo.tradeInfoList.Count - 1].TradeNO))
                {
                    orderInfoList.RemoveAt(0);
                }
            }
            TradeDataInfo.tradeInfoList.AddRange(orderInfoList);
        }
    }
}
