using System;
namespace FuturesTrade.Gnnt.BLL.Order
{
	public class SubmitOrderInfo
	{
		public string customerID = string.Empty;
		public string commodityID = string.Empty;
		public short B_SType = 1;
		public short O_LType = 1;
		public double price;
		public int qty;
		public short closeMode;
		public short timeFlag;
		public double lPrice;
		public short billType;
	}
}
