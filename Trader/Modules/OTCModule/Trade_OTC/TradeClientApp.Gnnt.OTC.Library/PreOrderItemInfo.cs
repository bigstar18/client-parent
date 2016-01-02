using System;
using System.Collections;
using TPME.Log;
namespace TradeClientApp.Gnnt.OTC.Library
{
	public class PreOrderItemInfo
	{
		public string m_strItems;
		public Hashtable m_htItemInfo;
		public PreOrderItemInfo()
		{
			this.initAllItem();
		}
		private void initAllItem()
		{
			try
			{
				this.m_strItems = "SelectFlag;ID;TransactionsCode;CommodityCode;B_S;O_L;Price;Qty;MarKet;LPrice;TodayPosition;";
				this.m_htItemInfo = new Hashtable();
				this.m_htItemInfo.Add("SelectFlag", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_SelectFlag"), 6, "", 0));
				this.m_htItemInfo.Add("ID", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_ID"), 9, "", 1));
				this.m_htItemInfo.Add("TransactionsCode", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_TransactionsCode"), 10, "", 0));
				this.m_htItemInfo.Add("CommodityCode", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_CommodityID"), 11, "", 1));
				this.m_htItemInfo.Add("B_S", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_B_S"), 9, "", 1));
				this.m_htItemInfo.Add("O_L", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_O_L"), 13, "", 1));
				this.m_htItemInfo.Add("Price", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_Price"), 11, Global.formatMoney, 1));
				this.m_htItemInfo.Add("Qty", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_Qty"), 10, "", 1));
				this.m_htItemInfo.Add("MarKet", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_MarKet"), 11, "", 1));
				this.m_htItemInfo.Add("LPrice", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_ZDPrice"), 11, "", 0));
				this.m_htItemInfo.Add("TodayPosition", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_TodayPosition"), 11, "", 0));
				this.m_htItemInfo.Add("CloseMode", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_CloseMode"), 11, "", 0));
				this.m_htItemInfo.Add("TimeFlag", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_TimeFlag"), 11, "", 0));
				string text = (string)Global.HTConfig["PreOrderItemName"];
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
				string text2 = (string)Global.HTConfig["PreOrderItems"];
				if (text2.Length > 0)
				{
					this.m_strItems = text2;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
	}
}
