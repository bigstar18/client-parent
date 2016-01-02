using System;
using System.Collections;
namespace FuturesTrade.Gnnt.Library
{
	public class OrderItemInfo
	{
		public string m_strItems;
		public Hashtable m_htItemInfo;
		public OrderItemInfo()
		{
			this.initAllItem();
		}
		private void initAllItem()
		{
			this.m_strItems = "OrderNo;Time;TransactionsCode;CommodityID;B_S;O_L;Price;Qty;Balance;Status";
			this.m_htItemInfo = new Hashtable();
			this.m_htItemInfo.Add("OrderNo", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_OrderNo"), 11, "", 0));
			this.m_htItemInfo.Add("Time", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Time"), 9, "", 0));
			this.m_htItemInfo.Add("TransactionsCode", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_TransactionsCode"), 7, "", 0));
			this.m_htItemInfo.Add("CommodityID", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_CommodityID"), 7, "", 0));
			this.m_htItemInfo.Add("B_S", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_B_S"), 7, "", 0));
			this.m_htItemInfo.Add("O_L", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_O_L"), 9, "", 0));
			this.m_htItemInfo.Add("Price", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Price"), 9, Global.formatMoney, 0));
			this.m_htItemInfo.Add("Qty", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Qty"), 8, "", 0));
			this.m_htItemInfo.Add("Balance", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Balance"), 7, "", 0));
			this.m_htItemInfo.Add("Status", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Status"), 9, "", 0));
			this.m_htItemInfo.Add("Market", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_MarKet"), 4, "", 0));
			this.m_htItemInfo.Add("CBasis", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_CBasis"), 10, "", 0));
			this.m_htItemInfo.Add("BillTradeType", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_BillTradeType"), 8, "", 0));
			string text = (string)Global.HTConfig["OrderItemName"];
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
			string text2 = (string)Global.HTConfig["OrderItems"];
			if (text2.Length > 0)
			{
				this.m_strItems = text2;
			}
		}
	}
}
