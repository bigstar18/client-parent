namespace FuturesTrade.Gnnt.DBService.Query.QueryTrade
{
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading;
    using TPME.Log;
    using TradeInterface.Gnnt.DataVO;

    internal class QueryPingTrade : IQueryTrade
    {
        private int tradeAllNum;
        private double tradeComm;
        private int tradeCurPageNum;
        private double tradeLiqpl;
        private List<TradeInfo> tradeListBufferF2 = new List<TradeInfo>();
        private List<TradeInfo> tradeListBufferF4 = new List<TradeInfo>();
        private object tradeLock = new object();
        private double tradeorderComm;
        private int tradeorderCurPageNum;
        private int tradeorderNum;
        private int tradeorderQty;
        private int tradeQty;

        private TradeQueryPagingResponseVO QueryPagingTrade(object queryTradeInfoReqVO, byte tradeFlag)
        {
            TradeQueryPagingRequestVO req = (TradeQueryPagingRequestVO)queryTradeInfoReqVO;
            TradeQueryPagingResponseVO evo = null;
            if (req != null)
            {
                if (req.CurrentPagNum > 0)
                {
                    if (tradeFlag == 0)
                    {
                        this.tradeorderCurPageNum = req.CurrentPagNum;
                        this.tradeListBufferF2.Clear();
                    }
                    else if (tradeFlag == 1)
                    {
                        this.tradeCurPageNum = req.CurrentPagNum;
                        this.tradeListBufferF4.Clear();
                    }
                }
                evo = Global.TradeLibrary.TradeQueryPaging(req);
                this.UpdateTradeInfo(evo.TradeInfoList);
                if (tradeFlag == 0)
                {
                    this.tradeorderNum = evo.TradeTotalRow.TotalNum;
                    this.tradeorderQty = evo.TradeTotalRow.TotalQty;
                    this.tradeorderComm = evo.TradeTotalRow.TotalComm;
                    return evo;
                }
                if (tradeFlag == 1)
                {
                    this.tradeAllNum = evo.TradeTotalRow.TotalNum;
                    this.tradeLiqpl = evo.TradeTotalRow.TotalLiqpl;
                    this.tradeQty = evo.TradeTotalRow.TotalQty;
                    this.tradeComm = evo.TradeTotalRow.TotalComm;
                }
            }
            return evo;
        }

        public DataSet QueryTradeDataSet(object queryTradeInfoReqVO)
        {
            TradeQueryPagingResponseVO evo = this.QueryPagingTrade(queryTradeInfoReqVO, 1);
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
            table.Columns.Add(new DataColumn("AutoID", typeof(int)));
            table.Columns["AutoID"].ColumnMapping = MappingType.Hidden;
            set.Tables.Add(table);
            if ((this.tradeListBufferF4.Count == 0) && (evo.RetCode != 0L))
            {
                Logger.wirte(MsgType.Error, "成交情况查询错误：" + evo.RetMessage);
                return set;
            }
            for (int i = 0; i < this.tradeListBufferF4.Count; i++)
            {
                TradeInfo info = this.tradeListBufferF4[i];
                DataRow row = table.NewRow();
                row["TradeNo"] = info.TradeNO;
                row["Time"] = Global.toTime(info.TradeTime);
                row["TransactionsCode"] = info.CustomerID;
                row["CommodityID"] = info.CommodityID;
                row["B_S"] = Global.BuySellStrArr[info.BuySell];
                row["O_L"] = Global.SettleBasisStrArr[info.SettleBasis];
                row["Price"] = info.TradePrice;
                row["Qty"] = info.TradeQuantity;
                row["Liqpl"] = info.TransferPL;
                row["LPrice"] = info.TransferPrice;
                row["Comm"] = info.Comm;
                row["Market"] = info.MarketID;
                table.Rows.Add(row);
            }
            DataRow row2 = table.NewRow();
            row2["Time"] = "合计";
            row2["TransactionsCode"] = "共" + this.tradeAllNum.ToString() + "条";
            row2["Qty"] = this.tradeQty;
            row2["Liqpl"] = this.tradeLiqpl;
            row2["Comm"] = this.tradeComm;
            row2["AutoID"] = 0x186a0;
            table.Rows.Add(row2);
            return set;
        }

        public DataSet QueryTradeOrderDataSet(object queryTradeOrderInfoReqVO)
        {
            TradeQueryPagingResponseVO evo = this.QueryPagingTrade(queryTradeOrderInfoReqVO, 0);
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
            table.Columns.Add(new DataColumn("AutoID", typeof(int)));
            table.Columns["AutoID"].ColumnMapping = MappingType.Hidden;
            set.Tables.Add(table);
            if ((this.tradeListBufferF2.Count == 0) && (evo.RetCode != 0L))
            {
                Logger.wirte(MsgType.Error, "成交情况查询错误：" + evo.RetMessage);
                return set;
            }
            for (int i = 0; i < this.tradeListBufferF2.Count; i++)
            {
                TradeInfo info = this.tradeListBufferF2[i];
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
            DataRow row2 = table.NewRow();
            row2["Time"] = "合计";
            row2["TransactionsCode"] = "共" + this.tradeorderNum.ToString() + "条";
            row2["Qty"] = this.tradeorderQty;
            row2["Comm"] = this.tradeorderComm;
            row2["AutoID"] = 0x186a0;
            table.Rows.Add(row2);
            return set;
        }

        private void ReplaceTradeInfo(List<TradeInfo> tradeInfoList)
        {
            int count = this.tradeListBufferF2.Count;
            int num2 = this.tradeListBufferF4.Count;
            for (int i = 0; i < tradeInfoList.Count; i++)
            {
                bool flag = false;
                for (int j = 0; j < count; j++)
                {
                    if (tradeInfoList[i].OrderNO == this.tradeListBufferF2[j].OrderNO)
                    {
                        this.tradeListBufferF2[j] = tradeInfoList[i];
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    if (this.tradeorderCurPageNum <= 1)
                    {
                        this.tradeListBufferF2.Add(tradeInfoList[i]);
                    }
                }
                else
                {
                    flag = false;
                }
                for (int k = 0; k < num2; k++)
                {
                    if (tradeInfoList[i].OrderNO == this.tradeListBufferF4[k].OrderNO)
                    {
                        this.tradeListBufferF4[k] = tradeInfoList[i];
                        flag = true;
                        break;
                    }
                }
                if (!flag && (this.tradeCurPageNum <= 1))
                {
                    this.tradeListBufferF4.Add(tradeInfoList[i]);
                }
            }
        }

        private void UpdateTradeInfo(List<TradeInfo> tradeInfoList)
        {
            object obj2;
            Monitor.Enter(obj2 = this.tradeLock);
            try
            {
                if (this.tradeListBufferF2.Count == 0)
                {
                    this.tradeListBufferF2.AddRange(tradeInfoList);
                }
                else if (this.tradeListBufferF4.Count == 0)
                {
                    this.tradeListBufferF4.AddRange(tradeInfoList);
                }
                else
                {
                    this.ReplaceTradeInfo(tradeInfoList);
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
    }
}
