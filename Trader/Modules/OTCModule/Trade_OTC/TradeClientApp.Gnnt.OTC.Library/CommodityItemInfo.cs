using System;
using System.Collections;
using TPME.Log;
namespace TradeClientApp.Gnnt.OTC.Library
{
	public class CommodityItemInfo
	{
		public string m_strItems;
		public Hashtable m_htItemInfo;
		public CommodityItemInfo()
		{
			this.initAllItem();
		}
		private void initAllItem()
		{
			try
			{
				this.m_strItems = "CommodityID;CommodityName;SpreadUp;SpreadDown;BMargin;SMargin;LSettledate;TmpTradecomm;TmpSettlecomm;BHold_Max;SHold_Max;CtrtSize;";
				this.m_htItemInfo = new Hashtable();
				this.m_htItemInfo.Add("CommodityID", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_CommodityID"), 0, "", 0));
				this.m_htItemInfo.Add("CommodityName", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_CommodityName"), 0, "", 0));
				this.m_htItemInfo.Add("SpreadUp", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_SpreadUp"), 0, Global.formatMoney, 0));
				this.m_htItemInfo.Add("SpreadDown", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_SpreadDown"), 0, Global.formatMoney, 0));
				this.m_htItemInfo.Add("BMargin", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_BMargin"), 0, "", 0));
				this.m_htItemInfo.Add("SMargin", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_SMargin"), 0, "", 0));
				this.m_htItemInfo.Add("LSettledate", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_LSettledate"), 0, "", 0));
				this.m_htItemInfo.Add("TmpTradecomm", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_TmpTradecomm"), 0, "", 0));
				this.m_htItemInfo.Add("TmpSettlecomm", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_TmpSettlecomm"), 0, "", 0));
				this.m_htItemInfo.Add("BHold_Max", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_BHold_Max"), 0, "", 0));
				this.m_htItemInfo.Add("SHold_Max", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_SHold_Max"), 0, "", 0));
				this.m_htItemInfo.Add("CtrtSize", new ColItemInfo("合约因子", 0, "", 0));
				string text = (string)Global.HTConfig["CommodityName"];
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
				string text2 = (string)Global.HTConfig["CommodityItems"];
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
