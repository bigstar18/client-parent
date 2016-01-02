using System;
using System.Collections;
namespace FuturesTrade.Gnnt.Library
{
	public class HoldingItemInfo
	{
		public string m_strItems;
		public Hashtable m_htItemInfo;
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
			string text = (string)Global.HTConfig["HoldingName"];
			string[] array = text.Split(new char[]
			{
				';'
			});
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new char[]
				{
					':'
				});
				if (array2.Length == 2 && array2[1].Length > 0)
				{
					ColItemInfo colItemInfo = (ColItemInfo)this.m_htItemInfo[array2[0]];
					if (colItemInfo != null)
					{
						colItemInfo.name = array2[1];
					}
				}
			}
			string text2 = (string)Global.HTConfig["HoldingItems"];
			if (text2.Length > 0)
			{
				this.m_strItems = text2;
			}
		}
	}
}
