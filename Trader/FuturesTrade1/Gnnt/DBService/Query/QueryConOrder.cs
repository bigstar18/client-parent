namespace FuturesTrade.Gnnt.DBService.Query
{
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using TPME.Log;
    using TradeInterface.Gnnt.DataVO;

    public class QueryConOrder
    {
        public DataSet QueryConditionOrderInfo(object queryConditionOrderReqVO)
        {
            ConditionQueryRequestVO req = (ConditionQueryRequestVO)queryConditionOrderReqVO;
            ConditionQueryResponseVO evo = null;
            if (req != null)
            {
                evo = Global.TradeLibrary.ConditionQuery(req);
                List<ConditionQueryOrderInfo> conditionQueryInfoList = evo.ConditionQueryInfoList;
                this.UpdatetradeInfo(conditionQueryInfoList);
            }
            DataSet set = new DataSet("ConditionDataSet");
            DataTable table = new DataTable("Corder");
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
            table.Columns.Add(column);
            table.Columns.Add(column2);
            table.Columns.Add(column14);
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
            table.Columns.Add(column15);
            set.Tables.Add(table);
            if (((TradeDataInfo.ConditionOrderInfoList.Count == 0) && (evo != null)) && (evo.RetCode != 0L))
            {
                Logger.wirte(MsgType.Error, "条件下单查询错误：" + evo.RetMessage);
                return set;
            }
            for (int i = 0; i < TradeDataInfo.ConditionOrderInfoList.Count; i++)
            {
                ConditionQueryOrderInfo info = TradeDataInfo.ConditionOrderInfoList[i];
                DataRow row = table.NewRow();
                row["OrderNo"] = info.OrderNO;
                row["CommodityID"] = info.CommodityID;
                row["B_S"] = Global.BuySellStrArr[info.BuySell_Type];
                row["O_L"] = Global.SettleBasisStrArr[info.SettleBasis];
                row["Price"] = info.OrderPrice;
                row["Qty"] = info.OrderQuantity;
                row["ConditionCommodityID"] = info.Condition_CommodityID;
                row["CoditionType"] = Global.ConditionTypeStrArr[info.ConditionType];
                row["ConditionSign"] = Global.ConditionSignStrArr[info.ConditionOperator + 2];
                row["ConditionPrice"] = info.ConditionPrice;
                row["PrepareTime"] = info.PrePareTime.Substring(0, 10);
                row["MatureTime"] = info.MatureTime;
                if (info.OrderTime.Length > 1)
                {
                    row["OrderTime"] = Convert.ToDateTime(info.OrderTime).ToString();
                }
                else
                {
                    row["OrderTime"] = info.OrderTime;
                }
                row["OrderState"] = info.OrderStare;
                row["TransactionsCode"] = info.CustomerID;
                table.Rows.Add(row);
            }
            return set;
        }

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
    }
}
