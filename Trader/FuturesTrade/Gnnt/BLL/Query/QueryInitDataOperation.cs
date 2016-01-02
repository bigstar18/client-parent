namespace FuturesTrade.Gnnt.BLL.Query
{
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using TradeInterface.Gnnt.DataVO;

    public class QueryInitDataOperation : QueryOperation
    {
        public ComboCommodityLoadCallBack ComboCommodityLoad;
        public FundsFillCallBack FundsFill;
        public Hashtable ht_TradeMode;
        public Hashtable ht_Variety;
        public IsPagingQueryCallBack isPagingQueryCallBack;
        private bool QueryFundsFlag = true;
        private bool refreshFirmInfoFlag = true;
        public TransactionLoadCallBack TransactionLoad;

        public void GetCommodity()
        {
            this.ht_TradeMode = new Hashtable();
            this.ht_Variety = new Hashtable();
            string path = Global.ConfigPath + Global.CommCodeXml.Substring(0, Global.CommCodeXml.IndexOf(".")) + ".xsd";
            if (!File.Exists(Global.ConfigPath + Global.CommCodeXml) || !File.Exists(path))
            {
                throw new Exception("没有发现商品架构文件！");
            }
            XmlDataSet set = new XmlDataSet(Global.ConfigPath + Global.CommCodeXml);
            DataSet dataSetByXml = set.GetDataSetByXml();
            base.serviceManage.CreateQueryInitData().QueryCommodity("", "");
            Hashtable commodityHashtable = TradeDataInfo.CommodityHashtable;
            if ((commodityHashtable != null) && (commodityHashtable.Count > 0))
            {
                string str2 = string.Empty;
                foreach (DataRow row in dataSetByXml.Tables[0].Rows)
                {
                    if ((bool)row["Flag"])
                    {
                        str2 = str2 + row["commodityCode"] + "-";
                    }
                }
                set.DeleteXmlAllRows();
                string[] columns = new string[] { "Flag", "ID", "CommodityCode", "MarKet" };
                string[] columnValue = new string[4];
                int num = 0;
                foreach (DictionaryEntry entry in commodityHashtable)
                {
                    CommodityInfo info = (CommodityInfo)entry.Value;
                    columnValue[0] = "false";
                    columnValue[1] = num.ToString();
                    columnValue[2] = info.CommodityID.Trim();
                    columnValue[3] = info.MarketID;
                    set.WriteBoolXml(columns, columnValue);
                    if (info.TradeMode.ToString().Trim().Length > 0)
                    {
                        this.ht_TradeMode.Add(info.CommodityID.Trim(), info.TradeMode.ToString());
                        this.ht_Variety.Add(info.CommodityID.Trim(), info.VarietyID.ToString());
                    }
                    num++;
                }
                string[] strArray3 = new string[] { "Flag" };
                string[] strArray4 = new string[] { "true" };
                string[] strArray5 = str2.Split(new char[] { '-' });
                for (int i = 0; i < (strArray5.Length - 1); i++)
                {
                    set.UpdateXmlRow(strArray3, strArray4, "commodityCode", strArray5[i]);
                }
            }
            else
            {
                set.DeleteXmlAllRows();
            }
            if (this.ComboCommodityLoad != null)
            {
                this.ComboCommodityLoad();
            }
        }

        public void GetFirmInfoList()
        {
            string path = Global.ConfigPath + Global.TrancCodeXml.Substring(0, Global.TrancCodeXml.IndexOf(".")) + ".xsd";
            if (!File.Exists(Global.ConfigPath + Global.TrancCodeXml) || !File.Exists(path))
            {
                throw new Exception("没有发现交易员架构文件！");
            }
            FirmInfoResponseVO evo = base.serviceManage.CreateQueryInitData().QueryFundsInfo();
            XmlDataSet set = new XmlDataSet(Global.ConfigPath + Global.TrancCodeXml);
            DataSet dataSetByXml = set.GetDataSetByXml();
            Global.FirmID = evo.FirmID;
            Global.CustomerCount = evo.CDS.Count;
            if (evo.CDS.Count > 0)
            {
                string str2 = string.Empty;
                foreach (DataRow row in dataSetByXml.Tables[0].Rows)
                {
                    if ((bool)row["Flag"])
                    {
                        str2 = str2 + row["TransactionsCode"] + "-";
                    }
                }
                set.DeleteXmlAllRows();
                string[] columns = new string[] { "Flag", "ID", "TransactionsCode" };
                string[] columnValue = new string[3];
                for (int i = 0; i < evo.CDS.Count; i++)
                {
                    columnValue[0] = "false";
                    columnValue[1] = i.ToString();
                    Code code = evo.CDS[i];
                    columnValue[2] = evo.FirmID + code.CD;
                    set.WriteBoolXml(columns, columnValue);
                }
                string[] strArray3 = new string[] { "Flag" };
                string[] strArray4 = new string[] { "true" };
                string[] strArray5 = str2.Split(new char[] { '-' });
                for (int j = 0; j < (strArray5.Length - 1); j++)
                {
                    set.UpdateXmlRow(strArray3, strArray4, "TransactionsCode", strArray5[j]);
                }
            }
            else
            {
                set.DeleteXmlAllRows();
                string[] strArray6 = new string[] { "Flag", "ID", "TransactionsCode" };
                string[] strArray7 = new string[] { "false", "0", evo.FirmID };
                set.WriteBoolXml(strArray6, strArray7);
            }
            if (this.TransactionLoad != null)
            {
                this.TransactionLoad();
            }
        }

        public void InitData()
        {
            this.GetCommodity();
            this.GetFirmInfoList();
            base.serviceManage.CreateQueryInitData().QueryMarketInfo();
            base.serviceManage.CreateQueryInitData().QueryFirmbreed();
            this.QueryDataQty();
        }

        public void QueryDataQty()
        {
            List<TotalRow> list = base.serviceManage.CreateQueryInitData().Querydateqty();
            bool flag = false;
            bool flag2 = false;
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    TotalRow row = list[i];
                    if ((row.ResponseName == "my_order_pagingquery") || (row.ResponseName == "my_weekorder_pagingquery"))
                    {
                        if (row.TotalNum > Global.MaxCount)
                        {
                            flag = true;
                        }
                    }
                    else if ((row.ResponseName == "tradepagingquery") && (row.TotalNum > Global.MaxCount))
                    {
                        flag2 = true;
                    }
                }
            }
            if (this.isPagingQueryCallBack != null)
            {
                this.isPagingQueryCallBack(flag, flag2);
            }
        }

        private void QueryFirmInfo(object o)
        {
            if (this.refreshFirmInfoFlag)
            {
                this.refreshFirmInfoFlag = false;
                FirmInfoResponseVO firmInfoResponseVO = base.serviceManage.CreateQueryInitData().QueryFundsInfo();
                if (this.FundsFill != null)
                {
                    this.FundsFill(firmInfoResponseVO);
                }
                this.refreshFirmInfoFlag = true;
            }
        }

        public void QueryFirmInfoThread()
        {
            if (this.QueryFundsFlag || base.IsOutRefreshTime())
            {
                base.ButtonRefreshFlag = 0;
                WaitCallback callBack = new WaitCallback(this.QueryFirmInfo);
                ThreadPool.QueueUserWorkItem(callBack);
                this.QueryFundsFlag = false;
            }
        }

        public void SetQueryUnOrderFlag(bool flag)
        {
            this.QueryFundsFlag = flag;
        }

        public delegate void ComboCommodityLoadCallBack();

        public delegate void FundsFillCallBack(FirmInfoResponseVO firmInfoResponseVO);

        public delegate void IsPagingQueryCallBack(bool _isOrderNew, bool _isTradeNew);

        public delegate void TransactionLoadCallBack();
    }
}
