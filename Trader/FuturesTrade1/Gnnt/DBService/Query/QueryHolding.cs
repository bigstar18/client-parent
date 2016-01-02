namespace FuturesTrade.Gnnt.DBService.Query
{
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Data;
    using TPME.Log;
    using TradeInterface.Gnnt.DataVO;

    public class QueryHolding
    {
        public DataSet QueryHoldingInfo(object queryHoldingRequestVO)
        {
            HoldingQueryRequestVO req = (HoldingQueryRequestVO)queryHoldingRequestVO;
            HoldingQueryResponseVO evo = null;
            if (req != null)
            {
                evo = Global.TradeLibrary.HoldingQuery(req);
                TradeDataInfo.holdingInfoList = evo.HoldingInfoList;
            }
            DataSet set = new DataSet("holdingDataSet");
            DataTable table = new DataTable("Holding");
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
            table.Columns.Add(column14);
            table.Columns.Add(column15);
            set.Tables.Add(table);
            if ((evo != null) && (evo.RetCode != 0L))
            {
                Logger.wirte(MsgType.Error, "持仓查询错误：" + evo.RetMessage);
                return set;
            }
            for (int i = 0; i < TradeDataInfo.holdingInfoList.Count; i++)
            {
                HoldingInfo info = TradeDataInfo.holdingInfoList[i];
                DataRow row = table.NewRow();
                row["CommodityID"] = info.CommodityID;
                row["TransactionsCode"] = info.CustomerID;
                row["BuyHolding"] = info.BuyHolding;
                row["BuyAvg"] = info.BuyAverage;
                row["SellHolding"] = info.SellHolding;
                row["SellAvg"] = info.SellAverage;
                row["Margin"] = info.Bail;
                row["Floatpl"] = info.FloatingLP;
                row["BuyVHolding"] = info.BuyVHolding;
                row["SellVHolding"] = info.SellVHolding;
                row["GoodsQty"] = info.GOQuantity;
                row["NewPriceLP"] = info.NewPriceLP;
                row["Market"] = info.MarketID;
                row["NetHolding"] = info.BuyHolding - info.SellHolding;
                row["AllHolding"] = info.SellHolding + info.BuyHolding;
                table.Rows.Add(row);
            }
            return set;
        }
    }
}
