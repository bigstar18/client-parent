namespace FuturesTrade.Gnnt.DBService.Query
{
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Data;
    using TPME.Log;
    using TradeInterface.Gnnt.DataVO;

    public class QueryHoldingDetail
    {
        public DataSet QueryHoldingDetailInfo(object queryHoldingDetailRequestVO)
        {
            HoldingDetailRequestVO req = (HoldingDetailRequestVO)queryHoldingDetailRequestVO;
            HoldingDetailResponseVO evo = null;
            if (req != null)
            {
                evo = Global.TradeLibrary.HoldPtByPrice(req);
                TradeDataInfo.holdingDetailInfoList = evo.HoldingDetailInfoList;
            }
            DataSet set = new DataSet("holdingDetailDataSet");
            DataTable table = new DataTable("HoldingDetail");
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
            table.Columns.Add(column);
            table.Columns.Add(column2);
            table.Columns.Add(column4);
            table.Columns.Add(column3);
            table.Columns.Add(column5);
            table.Columns.Add(column6);
            table.Columns.Add(column7);
            table.Columns.Add(column8);
            table.Columns.Add(column9);
            table.Columns.Add(column10);
            set.Tables.Add(table);
            if ((evo != null) && (evo.RetCode != 0L))
            {
                Logger.wirte(MsgType.Error, "订货明细查询错误：" + evo.RetMessage);
                return set;
            }
            for (int i = 0; i < TradeDataInfo.holdingDetailInfoList.Count; i++)
            {
                HoldingDetailInfo info = TradeDataInfo.holdingDetailInfoList[i];
                DataRow row = table.NewRow();
                row["CommodityID"] = info.CommodityID;
                row["TransactionsCode"] = info.CustomerID;
                row["Price"] = info.Price;
                row["B_S"] = Global.BuySellStrArr[info.BuySell];
                row["Margin"] = info.Bail;
                row["Cur_Open"] = info.AmountOnOrder;
                row["GoodsQty"] = info.GOQuantity;
                row["DeadLine"] = info.DeadLine;
                row["RemainDay"] = info.RemainDay;
                row["holddate"] = info.holddate;
                table.Rows.Add(row);
            }
            return set;
        }
    }
}
