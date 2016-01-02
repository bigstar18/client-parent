namespace FuturesTrade.Gnnt.BLL.Order
{
    using System;

    public class SubmitOrderInfo
    {
        public short B_SType = 1;
        public short billType;
        public short closeMode;
        public string commodityID = string.Empty;
        public string customerID = string.Empty;
        public double lPrice;
        public short O_LType = 1;
        public double price;
        public int qty;
        public short timeFlag;
    }
}
