using System;
using System.Collections;
using System.Collections.Generic;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.Library
{
	public class TradeDataInfo
	{
		public static Hashtable ht_TradeMode = new Hashtable();
		public static Hashtable ht_Variety = new Hashtable();
		public static List<OrderInfo> OrderInfoList = new List<OrderInfo>();
		public static List<ConditionQueryOrderInfo> ConditionOrderInfoList = new List<ConditionQueryOrderInfo>();
		public static List<TradeInfo> tradeInfoList = new List<TradeInfo>();
		public static List<HoldingInfo> holdingInfoList = new List<HoldingInfo>();
		public static List<HoldingDetailInfo> holdingDetailInfoList = new List<HoldingDetailInfo>();
		public static Hashtable CommodityHashtable = new Hashtable();
		public static List<FirmBreedInfo> FirmBreedInfoList = new List<FirmBreedInfo>();
		public static void ClearMemoryData()
		{
			TradeDataInfo.ht_TradeMode.Clear();
			TradeDataInfo.ht_Variety.Clear();
			TradeDataInfo.OrderInfoList.Clear();
			TradeDataInfo.tradeInfoList.Clear();
			TradeDataInfo.holdingInfoList.Clear();
			TradeDataInfo.CommodityHashtable.Clear();
			TradeDataInfo.FirmBreedInfoList.Clear();
		}
	}
}
