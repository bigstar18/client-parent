namespace FuturesTrade.Gnnt.DBService.Order
{
    using FuturesTrade.Gnnt.Library;
    using System;
    using TradeInterface.Gnnt.DataVO;

    public class ConditionOrder
    {
        public ConditionOrderResponseVO SetConditionOrder(ConditionOrderRequestVO req)
        {
            return Global.TradeLibrary.ConditionOrder(req);
        }

        public ConditionRevokeResponseVO WithDrawConditionOrder(ConditionRevokeRequestVO req)
        {
            return Global.TradeLibrary.ConditionRevoke(req);
        }
    }
}
