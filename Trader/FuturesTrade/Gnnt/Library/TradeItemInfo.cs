namespace FuturesTrade.Gnnt.Library
{
    using System;
    using System.Collections;

    public class TradeItemInfo
    {
        public Hashtable m_htItemInfo;
        public string m_strItems;

        public TradeItemInfo()
        {
            this.initAllItem();
        }

        private void initAllItem()
        {
            this.m_strItems = "TradeNo;Time;TransactionsCode;CommodityID;B_S;O_L;Price;Qty;Liqpl;LPrice;Comm;";
            this.m_htItemInfo = new Hashtable();
            this.m_htItemInfo.Add("TradeNo", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_TradeNo"), 12, "", 0));
            this.m_htItemInfo.Add("Time", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Time"), 8, "Time", 0));
            this.m_htItemInfo.Add("TransactionsCode", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_TransactionsCode"), 9, "", 0));
            this.m_htItemInfo.Add("CommodityID", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_CommodityID"), 8, "", 0));
            this.m_htItemInfo.Add("B_S", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_B_S"), 6, "", 0));
            this.m_htItemInfo.Add("O_L", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_O_L"), 10, "", 0));
            this.m_htItemInfo.Add("Price", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Price"), 9, Global.formatMoney, 0));
            this.m_htItemInfo.Add("Qty", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Qty"), 7, "", 0));
            this.m_htItemInfo.Add("Liqpl", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Liqpl"), 10, Global.formatMoney, 0));
            this.m_htItemInfo.Add("LPrice", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_LPrice"), 8, Global.formatMoney, 0));
            this.m_htItemInfo.Add("Comm", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Comm"), 8, Global.formatMoney, 0));
            this.m_htItemInfo.Add("Market", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_MarKet"), 8, "", 0));
            string[] strArray = ((string)Global.HTConfig["TradeItemName"]).Split(new char[] { ';' });
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
            string str2 = (string)Global.HTConfig["TradeItems"];
            if (str2.Length > 0)
            {
                this.m_strItems = str2;
            }
        }
    }
}
