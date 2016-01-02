using FuturesTrade.Gnnt.Library;
using System;
using System.Collections.Generic;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.DBService.Order
{
	public class EntrustOrder
	{
		public delegate void CalculateSumOrderCallBack(long quantity);
		public delegate void InsertOrderCallBack(List<OrderInfo> orderInfo);
		public EntrustOrder.CalculateSumOrderCallBack CalculateSumOrder;
		public EntrustOrder.InsertOrderCallBack InsertOrder;
		public ResponseVO Order(OrderRequestVO req)
		{
			ResponseVO responseVO = Global.TradeLibrary.Order(req);
			if (responseVO.RetCode == 0L && responseVO.OrderNo > 0L)
			{
				List<OrderInfo> list = new List<OrderInfo>();
				list.Add(new OrderInfo
				{
					OrderNO = responseVO.OrderNo,
					Time = responseVO.orderTime,
					State = 1,
					BuySell = req.BuySell,
					SettleBasis = req.SettleBasis,
					TraderID = req.UserID,
					FirmID = req.UserID,
					CustomerID = req.CustomerID,
					CBasis = 0,
					BillTradeType = 0,
					MarketID = req.MarketID,
					CommodityID = req.CommodityID,
					OrderPrice = req.Price,
					OrderQuantity = req.Quantity,
					Balance = (double)req.Quantity,
					LPrice = req.LPrice,
					WithDrawTime = string.Empty
				});
				if (this.CalculateSumOrder != null)
				{
					this.CalculateSumOrder(req.Quantity);
				}
				if (this.InsertOrder != null)
				{
					this.InsertOrder(list);
				}
			}
			return responseVO;
		}
	}
}
