namespace FuturesTrade.Gnnt.Library
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using TradeInterface.Gnnt.DataVO;

    public class TradeDataInfo
    {
        public static Hashtable CommodityHashtable = new Hashtable();
        public static List<ConditionQueryOrderInfo> ConditionOrderInfoList = new List<ConditionQueryOrderInfo>();
        public static List<FirmBreedInfo> FirmBreedInfoList = new List<FirmBreedInfo>();
        public static List<HoldingDetailInfo> holdingDetailInfoList = new List<HoldingDetailInfo>();
        public static List<HoldingInfo> holdingInfoList = new List<HoldingInfo>();
        public static Hashtable ht_TradeMode = new Hashtable();
        public static Hashtable ht_Variety = new Hashtable();
        public static List<OrderInfo> OrderInfoList = new List<OrderInfo>();
        public static List<TradeInfo> tradeInfoList = new List<TradeInfo>();

        public static void ClearMemoryData()
        {
            ht_TradeMode.Clear();
            ht_Variety.Clear();
            OrderInfoList.Clear();
            tradeInfoList.Clear();
            holdingInfoList.Clear();
            CommodityHashtable.Clear();
            FirmBreedInfoList.Clear();
        }
    }
}
