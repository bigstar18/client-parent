using System;
using TradeClientApp.Gnnt.OTC.Library;
namespace TradeClientApp.Gnnt.OTC
{
	internal class MainForm
	{
		private OrderItemInfo orderItemInfo;
		private TradeOrderItemInfo tradeOrderItemInfo;
		private AllOrderItemInfo allOrderItemInfo;
		private TradeItemInfo tradeItemInfo;
		private HoldingItemInfo holdingItemInfo;
		private HoldingDetailItemInfo holdingDetailItemInfo;
		private FundsItemInfo fundsItemInfo;
		private CommodityItemInfo commodityItemInfo;
		private PreOrderItemInfo preOrderItemInfo;
		private void InitFieldInfo()
		{
			this.orderItemInfo = new OrderItemInfo();
			this.tradeOrderItemInfo = new TradeOrderItemInfo();
			this.allOrderItemInfo = new AllOrderItemInfo();
			this.tradeItemInfo = new TradeItemInfo();
			this.holdingItemInfo = new HoldingItemInfo();
			this.holdingDetailItemInfo = new HoldingDetailItemInfo();
			this.fundsItemInfo = new FundsItemInfo();
			this.commodityItemInfo = new CommodityItemInfo();
			this.preOrderItemInfo = new PreOrderItemInfo();
		}
	}
}
