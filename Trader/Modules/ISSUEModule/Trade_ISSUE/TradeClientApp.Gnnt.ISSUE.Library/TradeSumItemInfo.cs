using System;
using System.Collections;
namespace TradeClientApp.Gnnt.ISSUE.Library
{
	public class TradeSumItemInfo
	{
		public string m_strItems;
		public Hashtable m_htItemInfo;
		public TradeSumItemInfo()
		{
			this.initAllItem();
		}
		private void initAllItem()
		{
			this.m_strItems = "CommodityID;CommodityID;BuyQty;BuyComm;SellQty;SellComm;TotalQty;TotalComm;IndeWareHouse;";
			this.m_htItemInfo = new Hashtable();
			this.m_htItemInfo.Add("CommodityID", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_CommodityID"), 8, "", 0));
			this.m_htItemInfo.Add("CommodityName", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_CommodityName"), 8, "", 0));
			this.m_htItemInfo.Add("BuyQty", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_B_S"), 6, "", 0));
			this.m_htItemInfo.Add("BuyComm", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_O_L"), 9, "", 0));
			this.m_htItemInfo.Add("SellQty", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Price"), 6, "", 0));
			this.m_htItemInfo.Add("SellComm", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Qty"), 10, "", 0));
			this.m_htItemInfo.Add("TotalQty", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Liqpl"), 7, "", 0));
			this.m_htItemInfo.Add("TotalComm", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_LPrice"), 10, "", 0));
			this.m_htItemInfo.Add("IndeWareHouse", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Comm"), 9, "", 0));
			string text = (string)Global.HTConfig["TradeSumItemName"];
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
			string text2 = (string)Global.HTConfig["TradeSumItems"];
			if (text2.Length > 0)
			{
				this.m_strItems = text2;
			}
		}
	}
}
