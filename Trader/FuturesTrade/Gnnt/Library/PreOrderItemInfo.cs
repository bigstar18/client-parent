namespace FuturesTrade.Gnnt.Library
{
    using System;
    using System.Collections;

    public class PreOrderItemInfo
    {
        public Hashtable m_htItemInfo;
        public string m_strItems;

        public PreOrderItemInfo()
        {
            this.initAllItem();
        }

        private void initAllItem()
        {
            this.m_strItems = "SelectFlag;ID;TransactionsCode;CommodityCode;B_S;O_L;Price;Qty;MarKet;LPrice;TodayPosition;";
            this.m_htItemInfo = new Hashtable();
            this.m_htItemInfo.Add("SelectFlag", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_SelectFlag"), 6, "", 0));
            this.m_htItemInfo.Add("ID", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_ID"), 9, "", 1));
            this.m_htItemInfo.Add("TransactionsCode", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_TransactionsCode"), 10, "", 0));
            this.m_htItemInfo.Add("CommodityCode", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_CommodityID"), 11, "", 1));
            this.m_htItemInfo.Add("B_S", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_B_S"), 9, "", 1));
            this.m_htItemInfo.Add("O_L", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_O_L"), 13, "", 1));
            this.m_htItemInfo.Add("Price", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Price"), 11, Global.formatMoney, 1));
            this.m_htItemInfo.Add("Qty", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Qty"), 10, "", 1));
            this.m_htItemInfo.Add("MarKet", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_MarKet"), 11, "", 1));
            this.m_htItemInfo.Add("LPrice", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_ZDPrice"), 11, Global.formatMoney, 0));
            this.m_htItemInfo.Add("TodayPosition", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_TodayPosition"), 11, "", 0));
            this.m_htItemInfo.Add("CloseMode", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_CloseMode"), 11, "", 0));
            this.m_htItemInfo.Add("TimeFlag", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_TimeFlag"), 11, "", 0));
            string[] strArray = ((string)Global.HTConfig["PreOrderItemName"]).Split(new char[] { ';' });
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
            string str2 = (string)Global.HTConfig["PreOrderItems"];
            if (str2.Length > 0)
            {
                this.m_strItems = str2;
            }
        }
    }
}
