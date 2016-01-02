using System;
namespace FuturesTrade.Gnnt.BLL.Order
{
	public class SubmitConOrderInfo
	{
		public string customerID = string.Empty;
		public string commodityID = string.Empty;
		public string datetime = string.Empty;
		public short contype;
		public short conoperator;
		public double conprice;
		public short B_SType = 1;
		public short O_LType = 1;
		public double price;
		public int qty;
	}
}
