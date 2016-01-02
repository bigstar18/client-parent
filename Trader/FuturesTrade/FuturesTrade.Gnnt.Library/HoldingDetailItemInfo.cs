using System;
using System.Collections;
namespace FuturesTrade.Gnnt.Library
{
	public class HoldingDetailItemInfo
	{
		public string m_strItems;
		public Hashtable m_htItemInfo;
		public HoldingDetailItemInfo()
		{
			this.initAllItem();
		}
		private void initAllItem()
		{
			this.m_strItems = "CommodityID;TransactionsCode;B_S;Price;Cur_Open;GoodsQty;Margin;DeadLine;RemainDay";
			this.m_htItemInfo = new Hashtable();
			this.m_htItemInfo.Add("CommodityID", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_CommodityID"), 17, "", 0));
			this.m_htItemInfo.Add("TransactionsCode", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_TransactionsCode"), 18, "", 0));
			this.m_htItemInfo.Add("B_S", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_B_S"), 10, "", 0));
			this.m_htItemInfo.Add("Price", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Price"), 15, Global.formatMoney, 0));
			this.m_htItemInfo.Add("Cur_Open", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Cur_Open"), 15, "", 0));
			this.m_htItemInfo.Add("GoodsQty", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_GoodsQty"), 15, "", 0));
			this.m_htItemInfo.Add("Margin", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Margin"), 10, Global.formatMoney, 0));
			this.m_htItemInfo.Add("DeadLine", new ColItemInfo("到期日期", 10, "", 0));
			this.m_htItemInfo.Add("RemainDay", new ColItemInfo("到期天数", 10, "", 0));
			this.m_htItemInfo.Add("holddate", new ColItemInfo("订货日期", 10, "", 0));
			string text = (string)Global.HTConfig["HoldingDetailName"];
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
			string text2 = (string)Global.HTConfig["HoldingDetailItems"];
			if (text2.Length > 0)
			{
				this.m_strItems = text2;
			}
		}
	}
}
