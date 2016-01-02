using FuturesTrade.Gnnt.Library;
using System;
using System.Collections.Generic;
using TPME.Log;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.DBService.Query
{
	public class QueryCommData
	{
		public CommData QueryGNCommodityInfo(string MarketID, string CommodityID)
		{
			CommDataQueryRequestVO commDataQueryRequestVO = new CommDataQueryRequestVO();
			commDataQueryRequestVO.UserID = Global.UserID;
			commDataQueryRequestVO.MarketID = MarketID;
			commDataQueryRequestVO.CommodityID = CommodityID;
			CommDataQueryResponseVO commDataQueryResponseVO = Global.TradeLibrary.CommDataQuery(commDataQueryRequestVO);
			if (commDataQueryResponseVO.RetCode != 0L)
			{
				Logger.wirte(MsgType.Error, "查询商品行情错误：" + commDataQueryResponseVO.RetMessage);
				return null;
			}
			List<CommData> commDataList = commDataQueryResponseVO.CommDataList;
			if (commDataList.Count > 0)
			{
				return commDataList[0];
			}
			return null;
		}
	}
}
