using System;
namespace TradeInterface.Gnnt.DataVO
{
	public class ConditionQueryOrderInfo
	{
		public long OrderNO;
		public string OrderStare;
		public short BuySell_Type;
		public short SettleBasis;
		public string TraderID = string.Empty;
		public string FirmID = string.Empty;
		public string CustomerID = string.Empty;
		public long SessionID;
		public string CommodityID = string.Empty;
		public string Condition_CommodityID = string.Empty;
		public double OrderPrice;
		public long OrderQuantity;
		public long Surplus;
		public long LPrice;
		public string RevokeTime = string.Empty;
		public short CFlag;
		public short BillTradeType;
		public string PrePareTime = string.Empty;
		public string OrderTime = string.Empty;
		public string MatureTime = string.Empty;
		public double ConditionPrice;
		public short ConditionType;
		public short ConditionOperator;
	}
}
