namespace FuturesTrade.Gnnt.DBService.Order
{
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Runtime.CompilerServices;
    using TradeInterface.Gnnt.DataVO;

    public class RevokeOrder
    {
        private short status = 5;
        public UpdateOrderStatusCallBack UpdateOrderStatus;

        public ResponseVO WithDrawOrder(WithDrawOrderRequestVO req)
        {
            ResponseVO evo = Global.TradeLibrary.WithDrawOrder(req);
            if (((evo != null) && (evo.RetCode == 0L)) && (this.UpdateOrderStatus != null))
            {
                this.UpdateOrderStatus(req.OrderNo, this.status);
            }
            return evo;
        }

        public delegate void UpdateOrderStatusCallBack(long orderNo, short status);
    }
}
