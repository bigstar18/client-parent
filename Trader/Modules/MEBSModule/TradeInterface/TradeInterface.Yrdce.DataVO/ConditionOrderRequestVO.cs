using System;
namespace TradeInterface.Gnnt.DataVO
{
	public class ConditionOrderRequestVO
	{
		public string UserID = string.Empty;
		public string CustomerID = string.Empty;
		public string TraderID = string.Empty;
		public string FirmID = string.Empty;
		public short BuySell;
		public string CommodityID = string.Empty;
		public double Price;
		public long Quantity;
		public short SettleBasis;
		public short CloseMode;
		public short TimeFlag;
		public long LPrice;
		public long SessionID;
		public short BillType;
		public short SO;
		public string Concommodity;
		public short Contype;
		public short Conoperator;
		public double ConPrice;
		public string ConExpire;
	}
}
