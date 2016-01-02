namespace FuturesTrade.Gnnt.DBService.Order
{
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using TradeInterface.Gnnt.DataVO;

    public class EntrustOrder
    {
        public CalculateSumOrderCallBack CalculateSumOrder;
        public InsertOrderCallBack InsertOrder;

        public ResponseVO Order(OrderRequestVO req)
        {
            ResponseVO evo = Global.TradeLibrary.Order(req);
            if ((evo.RetCode == 0L) && (evo.OrderNo > 0L))
            {
                List<OrderInfo> orderInfo = new List<OrderInfo>();
                OrderInfo item = new OrderInfo
                {
                    OrderNO = evo.OrderNo,
                    Time = evo.orderTime,
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
                    Balance = req.Quantity,
                    LPrice = req.LPrice,
                    WithDrawTime = string.Empty
                };
                orderInfo.Add(item);
                if (this.CalculateSumOrder != null)
                {
                    this.CalculateSumOrder(req.Quantity);
                }
                if (this.InsertOrder != null)
                {
                    this.InsertOrder(orderInfo);
                }
            }
            return evo;
        }

        public delegate void CalculateSumOrderCallBack(long quantity);

        public delegate void InsertOrderCallBack(List<OrderInfo> orderInfo);
    }
}
