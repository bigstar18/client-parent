namespace FuturesTrade.Gnnt.Library
{
    using System;
    using System.Collections;

    public class CommodityItemInfo
    {
        public Hashtable m_htItemInfo;
        public string m_strItems;

        public CommodityItemInfo()
        {
            this.initAllItem();
        }

        private void initAllItem()
        {
            this.m_strItems = "CommodityID;CommodityName;SpreadUp;SpreadDown;BMargin;SMargin;LSettledate;TmpTradecomm;TmpSettlecomm;BHold_Max;SHold_Max;CtrtSize;MaxHoldDays;";
            this.m_htItemInfo = new Hashtable();
            this.m_htItemInfo.Add("CommodityID", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_CommodityID"), 0, "", 0));
            this.m_htItemInfo.Add("CommodityName", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_CommodityName"), 0, "", 0));
            this.m_htItemInfo.Add("SpreadUp", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_SpreadUp"), 0, Global.formatMoney, 0));
            this.m_htItemInfo.Add("SpreadDown", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_SpreadDown"), 0, Global.formatMoney, 0));
            this.m_htItemInfo.Add("BMargin", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_BMargin"), 0, "", 0));
            this.m_htItemInfo.Add("SMargin", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_SMargin"), 0, "", 0));
            this.m_htItemInfo.Add("LSettledate", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_LSettledate"), 0, "", 0));
            this.m_htItemInfo.Add("TmpTradecomm", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_TmpTradecomm"), 0, "", 0));
            this.m_htItemInfo.Add("TmpSettlecomm", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_TmpSettlecomm"), 0, "", 0));
            this.m_htItemInfo.Add("BHold_Max", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_BHold_Max"), 0, "", 0));
            this.m_htItemInfo.Add("SHold_Max", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_SHold_Max"), 0, "", 0));
            this.m_htItemInfo.Add("CtrtSize", new ColItemInfo("合约因子", 0, "", 0));
            this.m_htItemInfo.Add("MaxHoldDays", new ColItemInfo("最大持仓天数", 0, "", 0));
            string[] strArray = ((string)Global.HTConfig["CommodityName"]).Split(new char[] { ';' });
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
            string str2 = (string)Global.HTConfig["CommodityItems"];
            if (str2.Length > 0)
            {
                this.m_strItems = str2;
            }
        }
    }
}
