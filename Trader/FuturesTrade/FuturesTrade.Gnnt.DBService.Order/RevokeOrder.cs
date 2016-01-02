using FuturesTrade.Gnnt.Library;
using System;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.DBService.Order
{
	public class RevokeOrder
	{
		public delegate void UpdateOrderStatusCallBack(long orderNo, short status);
		private short status = 5;
		public RevokeOrder.UpdateOrderStatusCallBack UpdateOrderStatus;
		public ResponseVO WithDrawOrder(WithDrawOrderRequestVO req)
		{
			ResponseVO responseVO = Global.TradeLibrary.WithDrawOrder(req);
			if (responseVO != null && responseVO.RetCode == 0L && this.UpdateOrderStatus != null)
			{
				this.UpdateOrderStatus(req.OrderNo, this.status);
			}
			return responseVO;
		}
	}
}
