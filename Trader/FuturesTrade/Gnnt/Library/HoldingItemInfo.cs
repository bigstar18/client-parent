namespace FuturesTrade.Gnnt.Library
{
    using System;
    using System.Collections;

    public class HoldingItemInfo
    {
        public Hashtable m_htItemInfo;
        public string m_strItems;

        public HoldingItemInfo()
        {
            this.initAllItem();
        }

        private void initAllItem()
        {
            this.m_strItems = "CommodityID;TransactionsCode;BuyHolding;BuyAvg;SellHolding;SellAvg;Margin;Floatpl;BuyVHolding;SellVHolding;GoodsQty;NewPriceLP;Market;NetHolding;AllHolding";
            this.m_htItemInfo = new Hashtable();
            this.m_htItemInfo.Add("CommodityID", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_CommodityID"), 10, "", 0));
            this.m_htItemInfo.Add("TransactionsCode", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_TransactionsCode"), 10, "", 0));
            this.m_htItemInfo.Add("BuyHolding", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_BuyHolding"), 7, "", 0));
            this.m_htItemInfo.Add("BuyAvg", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_BuyAvg"), 10, Global.formatMoney, 0));
            this.m_htItemInfo.Add("SellHolding", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_SellHolding"), 7, "", 0));
            this.m_htItemInfo.Add("SellAvg", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_SellAvg"), 10, Global.formatMoney, 0));
            this.m_htItemInfo.Add("Margin", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Margin"), 12, Global.formatMoney, 0));
            this.m_htItemInfo.Add("Floatpl", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Floatpl"), 11, Global.formatMoney, 0));
            this.m_htItemInfo.Add("BuyVHolding", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_BuyVHolding"), 7, "", 0));
            this.m_htItemInfo.Add("SellVHolding", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_SellVHolding"), 7, "", 0));
            this.m_htItemInfo.Add("GoodsQty", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_GoodsQty"), 8, "", 0));
            this.m_htItemInfo.Add("NewPriceLP", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_NewPriceLP"), 8, Global.formatMoney, 0));
            this.m_htItemInfo.Add("Market", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_MarKet"), 2, "", 0));
            this.m_htItemInfo.Add("NetHolding", new ColItemInfo("净订货", 7, "", 0));
            this.m_htItemInfo.Add("AllHolding", new ColItemInfo("总订货", 7, "", 0));
            string[] strArray = ((string)Global.HTConfig["HoldingName"]).Split(new char[] { ';' });
            for (int i = 0; i < strArray.Length; i++)
            {
                string[] strArray2 = strArray[i].Split(new char[] { ':' });
                if ((strArray2.Length == 2) && (strArray2[1].Length > 0))
                {
                    ColItemInfo info = (ColItemInfo)this.m_htItemInfo[strArray2[0]];
                    if (info != null)
                    {
                        info.name = strArray2[1];
                    }
                }
            }
            string str2 = (string)Global.HTConfig["HoldingItems"];
            if (str2.Length > 0)
            {
                this.m_strItems = str2;
            }
        }
    }
}
