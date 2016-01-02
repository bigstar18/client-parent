namespace FuturesTrade.Gnnt.BLL.Order
{
    using System;

    public class SubmitConOrderInfo
    {
        public short B_SType = 1;
        public string commodityID = string.Empty;
        public short conoperator;
        public double conprice;
        public short contype;
        public string customerID = string.Empty;
        public string datetime = string.Empty;
        public short O_LType = 1;
        public double price;
        public int qty;
    }
}
