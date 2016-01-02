namespace FuturesTrade.Gnnt.DBService.Query
{
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections.Generic;
    using TPME.Log;
    using TradeInterface.Gnnt.DataVO;

    public class QueryCommData
    {
        public CommData QueryGNCommodityInfo(string MarketID, string CommodityID)
        {
            CommDataQueryRequestVO req = new CommDataQueryRequestVO
            {
                UserID = Global.UserID,
                MarketID = MarketID,
                CommodityID = CommodityID
            };
            CommDataQueryResponseVO evo = Global.TradeLibrary.CommDataQuery(req);
            if (evo.RetCode != 0L)
            {
                Logger.wirte(MsgType.Error, "查询商品行情错误：" + evo.RetMessage);
                return null;
            }
            List<CommData> commDataList = evo.CommDataList;
            if (commDataList.Count > 0)
            {
                return commDataList[0];
            }
            return null;
        }
    }
}
