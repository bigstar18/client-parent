using FuturesTrade.Gnnt.Library;
using System;
using System.Collections;
using System.Collections.Generic;
using TPME.Log;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.DBService.Query
{
	public class QueryInitData
	{
		private string queryName = "my_order_pagingquery,my_weekorder_pagingquery,tradepagingquery";
		public void QueryCommodity(string marketID, string commodityID)
		{
			if (TradeDataInfo.CommodityHashtable.Count == 0)
			{
				CommodityQueryRequestVO commodityQueryRequestVO = new CommodityQueryRequestVO();
				commodityQueryRequestVO.UserID = Global.UserID;
				commodityQueryRequestVO.CommodityID = commodityID;
				commodityQueryRequestVO.MarketID = marketID;
				CommodityQueryResponseVO commodityQueryResponseVO = Global.TradeLibrary.CommodityQuery(commodityQueryRequestVO);
				if (commodityQueryResponseVO != null && commodityQueryResponseVO.RetCode == 0L)
				{
					for (int i = 0; i < commodityQueryResponseVO.CommodityInfoList.Count; i++)
					{
						TradeDataInfo.CommodityHashtable.Add(commodityQueryResponseVO.CommodityInfoList[i].CommodityID, commodityQueryResponseVO.CommodityInfoList[i]);
					}
				}
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
			MarketQueryRequestVO marketQueryRequestVO = new MarketQueryRequestVO();
			marketQueryRequestVO.UserID = Global.UserID;
			MarketQueryResponseVO marketQueryResponseVO = Global.TradeLibrary.MarketQuery(marketQueryRequestVO);
			if (marketQueryResponseVO.RetCode != 0L)
			{
				List<MarkeInfo> markeInfoList = marketQueryResponseVO.MarkeInfoList;
				for (int i = 0; i < markeInfoList.Count; i++)
				{
					hashtable.Add(markeInfoList[i].MarketID, markeInfoList[i]);
				}
			}
			Global.MarketHT = hashtable;
			if (Global.MarketHT != null && Global.MarketHT.Count == 1)
			{
				foreach (DictionaryEntry dictionaryEntry in Global.MarketHT)
				{
					MarkeInfo markeInfo = (MarkeInfo)dictionaryEntry.Value;
					if (markeInfo != null)
					{
						Global.MarketID = markeInfo.MarketID;
					}
				}
			}
		}
		public void QueryFirmbreed()
		{
			FirmbreedQueryResponseVO firmbreedQueryResponseVO = Global.TradeLibrary.FirmbreedQuery(Global.UserID);
			if (firmbreedQueryResponseVO.RetCode == 0L)
			{
				TradeDataInfo.FirmBreedInfoList = firmbreedQueryResponseVO.FirmBreedInfoList;
			}
		}
		public List<TotalRow> Querydateqty()
		{
			QuerydateqtyRequestVO querydateqtyRequestVO = new QuerydateqtyRequestVO();
			querydateqtyRequestVO.UserID = Global.UserID;
			querydateqtyRequestVO.QueryName = this.queryName;
			QuerydateqtyResponseVO querydateqtyResponseVO = Global.TradeLibrary.Querydateqty(querydateqtyRequestVO);
			List<TotalRow> totalRowList = querydateqtyResponseVO.totalRowList;
			if (querydateqtyResponseVO.RetCode != 0L)
			{
				Logger.wirte(MsgType.Error, "总条数查询错误：" + querydateqtyResponseVO.RetMessage);
				return null;
			}
			return totalRowList;
		}
	}
}
