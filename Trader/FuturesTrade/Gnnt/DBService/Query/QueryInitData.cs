namespace FuturesTrade.Gnnt.DBService.Query
{
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using TPME.Log;
    using TradeInterface.Gnnt.DataVO;

    public class QueryInitData
    {
        private string queryName = "my_order_pagingquery,my_weekorder_pagingquery,tradepagingquery";

        public void QueryCommodity(string marketID, string commodityID)
        {
            if (TradeDataInfo.CommodityHashtable.Count == 0)
            {
                CommodityQueryRequestVO req = new CommodityQueryRequestVO
                {
                    UserID = Global.UserID,
                    CommodityID = commodityID,
                    MarketID = marketID
                };
                CommodityQueryResponseVO evo = Global.TradeLibrary.CommodityQuery(req);
                if ((evo != null) && (evo.RetCode == 0L))
                {
                    for (int i = 0; i < evo.CommodityInfoList.Count; i++)
                    {
                        TradeDataInfo.CommodityHashtable.Add(evo.CommodityInfoList[i].CommodityID, evo.CommodityInfoList[i]);
                    }
                }
            }
        }

        public List<TotalRow> Querydateqty()
        {
            QuerydateqtyRequestVO req = new QuerydateqtyRequestVO
            {
                UserID = Global.UserID,
                QueryName = this.queryName
            };
            QuerydateqtyResponseVO evo = Global.TradeLibrary.Querydateqty(req);
            List<TotalRow> totalRowList = evo.totalRowList;
            if (evo.RetCode != 0L)
            {
                Logger.wirte(MsgType.Error, "总条数查询错误：" + evo.RetMessage);
                return null;
            }
            return totalRowList;
        }

        public void QueryFirmbreed()
        {
            FirmbreedQueryResponseVO evo = Global.TradeLibrary.FirmbreedQuery(Global.UserID);
            if (evo.RetCode == 0L)
            {
                TradeDataInfo.FirmBreedInfoList = evo.FirmBreedInfoList;
            }
        }

        public FirmInfoResponseVO QueryFundsInfo()
        {
            FirmInfoResponseVO firmInfo = Global.TradeLibrary.GetFirmInfo(Global.UserID);
            if (firmInfo.RetCode != 0L)
            {
                Logger.wirte(MsgType.Error, "会员信息查询错误：" + firmInfo.RetMessage);
            }
            return firmInfo;
        }

        public void QueryMarketInfo()
        {
            Hashtable hashtable = new Hashtable();
            MarketQueryRequestVO req = new MarketQueryRequestVO
            {
                UserID = Global.UserID
            };
            MarketQueryResponseVO evo = Global.TradeLibrary.MarketQuery(req);
            if (evo.RetCode != 0L)
            {
                List<MarkeInfo> markeInfoList = evo.MarkeInfoList;
                for (int i = 0; i < markeInfoList.Count; i++)
                {
                    hashtable.Add(markeInfoList[i].MarketID, markeInfoList[i]);
                }
            }
            Global.MarketHT = hashtable;
            if ((Global.MarketHT != null) && (Global.MarketHT.Count == 1))
            {
                foreach (DictionaryEntry entry in Global.MarketHT)
                {
                    MarkeInfo info = (MarkeInfo)entry.Value;
                    if (info != null)
                    {
                        Global.MarketID = info.MarketID;
                    }
                }
            }
        }
    }
}
