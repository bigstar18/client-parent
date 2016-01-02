namespace FuturesTrade.Gnnt.Library
{
    using System;
    using System.Collections;

    public class HoldingDetailItemInfo
    {
        public Hashtable m_htItemInfo;
        public string m_strItems;

        public HoldingDetailItemInfo()
        {
            this.initAllItem();
        }

        private void initAllItem()
        {
            this.m_strItems = "CommodityID;TransactionsCode;B_S;Price;Cur_Open;GoodsQty;Margin;DeadLine;RemainDay";
            this.m_htItemInfo = new Hashtable();
            this.m_htItemInfo.Add("CommodityID", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_CommodityID"), 0x11, "", 0));
            this.m_htItemInfo.Add("TransactionsCode", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_TransactionsCode"), 0x12, "", 0));
            this.m_htItemInfo.Add("B_S", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_B_S"), 10, "", 0));
            this.m_htItemInfo.Add("Price", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Price"), 15, Global.formatMoney, 0));
            this.m_htItemInfo.Add("Cur_Open", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Cur_Open"), 15, "", 0));
            this.m_htItemInfo.Add("GoodsQty", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_GoodsQty"), 15, "", 0));
            this.m_htItemInfo.Add("Margin", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Margin"), 10, Global.formatMoney, 0));
            this.m_htItemInfo.Add("DeadLine", new ColItemInfo("到期日期", 10, "", 0));
            this.m_htItemInfo.Add("RemainDay", new ColItemInfo("到期天数", 10, "", 0));
            this.m_htItemInfo.Add("holddate", new ColItemInfo("订货日期", 10, "", 0));
            string[] strArray = ((string)Global.HTConfig["HoldingDetailName"]).Split(new char[] { ';' });
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
            string str2 = (string)Global.HTConfig["HoldingDetailItems"];
            if (str2.Length > 0)
            {
                this.m_strItems = str2;
            }
        }
    }
}
