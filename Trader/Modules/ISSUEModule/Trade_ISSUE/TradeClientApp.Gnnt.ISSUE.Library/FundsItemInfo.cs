using System;
using System.Collections;
namespace TradeClientApp.Gnnt.ISSUE.Library
{
	public class FundsItemInfo
	{
		public string m_strItems;
		public Hashtable m_htItemInfo;
		public FundsItemInfo()
		{
			this.initAllItem();
		}
		private void initAllItem()
		{
			this.m_htItemInfo = new Hashtable();
			this.m_htItemInfo.Add("FirmID", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_FirmID"), 0, "", 0));
			this.m_htItemInfo.Add("FirmName", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_FirmName"), 0, "", 0));
			this.m_htItemInfo.Add("FirmType", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_FirmType"), 0, "F2", 0));
			this.m_htItemInfo.Add("InitFund", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_InitFund"), 0, "F2", 0));
			this.m_htItemInfo.Add("InFund", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_InFund"), 0, "F2", 0));
			this.m_htItemInfo.Add("AddFund", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_AddFund"), 0, "F2", 0));
			this.m_htItemInfo.Add("OutFund", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_OutFund"), 0, "F2", 0));
			this.m_htItemInfo.Add("HKSell", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_HKSell"), 0, "F2", 0));
			this.m_htItemInfo.Add("HKBuy", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_HKBuy"), 0, "F2", 0));
			this.m_htItemInfo.Add("CurFreezeFund", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_CurFreezeFund"), 0, "F2", 0));
			this.m_htItemInfo.Add("CurUnfreezeFund", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_CurUnfreezeFund"), 0, "F2", 0));
			this.m_htItemInfo.Add("IssuanceFee", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_IssuanceFee"), 0, "F2", 0));
			this.m_htItemInfo.Add("SGFreezeFund", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_SGFreezeFund"), 0, "F2", 0));
			this.m_htItemInfo.Add("OrderFrozenFund", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_OrderFrozenFund"), 0, "F2", 0));
			this.m_htItemInfo.Add("OtherFrozenFund", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_OtherFrozenFund"), 0, "F2", 0));
			this.m_htItemInfo.Add("Fee", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_Fee"), 0, "F2", 0));
			this.m_htItemInfo.Add("WareHouseRegFee", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_WareHouseRegFee"), 0, "F2", 0));
			this.m_htItemInfo.Add("WareHouseCancelFee", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_WareHouseCancelFee"), 0, "F2", 0));
			this.m_htItemInfo.Add("TransferFee", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_TransferFee"), 0, "F2", 0));
			this.m_htItemInfo.Add("DistributionFee", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_DistributionFee"), 0, "F2", 0));
			this.m_htItemInfo.Add("OtherFee", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_OtherFee"), 0, "F2", 0));
			this.m_htItemInfo.Add("OtherChange", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_OtherChange"), 0, "F2", 0));
			this.m_htItemInfo.Add("MarketValue", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_MarketValue"), 0, "F2", 0));
			this.m_htItemInfo.Add("UsableFund", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_UsableFund"), 0, "F2", 0));
			this.m_htItemInfo.Add("DesirableFund", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_DesirableFund"), 0, "F2", 0));
			this.m_htItemInfo.Add("CurrentRight", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_CurrentRight"), 0, "F2", 0));
			this.m_htItemInfo.Add("HKSellMinus", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_HKSellMinus"), 0, "F2", 0));
			this.m_htItemInfo.Add("CurFreezeFundMinus", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_CurFreezeFundMinus"), 0, "F2", 0));
			this.m_htItemInfo.Add("OrderFrozenFundMinus", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_OrderFrozenFundMinus"), 0, "F2", 0));
			this.m_htItemInfo.Add("UsableFundAdd", new ColItemInfo(Global.M_ResourceManager.GetString("TradeStr_UsableFundAdd"), 0, "F2", 0));
			string text = (string)Global.HTConfig["FundsName"];
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
			string text2 = (string)Global.HTConfig["FundsItems"];
			if (text2.Length > 0)
			{
				this.m_strItems = text2;
			}
		}
	}
}
