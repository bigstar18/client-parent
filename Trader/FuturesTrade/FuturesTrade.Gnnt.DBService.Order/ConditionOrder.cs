using FuturesTrade.Gnnt.Library;
using System;
using TradeInterface.Gnnt.DataVO;
namespace FuturesTrade.Gnnt.DBService.Order
{
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
