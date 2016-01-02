using System;
using System.Collections;
using TPME.Log;
namespace TradeClientApp.Gnnt.OTC.Library
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
			try
			{
				this.m_strItems = "FirmID;RealFunds;Margin;Lfpl;Jysqy;AddFund;Status;Cur_Open;Virtual_Open;HoldGain;DynRight;FreezFund;FreezComm;Comm;TransLiqpl;Zjaql;Code";
				this.m_htItemInfo = new Hashtable();
				this.m_htItemInfo.Add("FirmID", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_FirmID"), 0, "", 0));
				this.m_htItemInfo.Add("RealFunds", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_RealFunds"), 0, "F2", 0));
				this.m_htItemInfo.Add("Margin", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_DayMargin"), 0, "F2", 0));
				this.m_htItemInfo.Add("Lfpl", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_Lfpl"), 0, "F2", 0));
				this.m_htItemInfo.Add("Jysqy", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_Jysqy"), 0, "F2", 0));
				this.m_htItemInfo.Add("AddFund", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_AddFund"), 0, "F2", 0));
				this.m_htItemInfo.Add("Status", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_Status"), 0, "F2", 0));
				this.m_htItemInfo.Add("Cur_Open", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_Cur_Open"), 0, "F2", 0));
				this.m_htItemInfo.Add("Virtual_Open", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_Virtual_Open"), 0, "F2", 0));
				this.m_htItemInfo.Add("HoldGain", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_HoldGain"), 0, "F2", 0));
				this.m_htItemInfo.Add("DynRight", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_DynRight"), 0, "F2", 0));
				this.m_htItemInfo.Add("FreezFund", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_FreezFund"), 0, "F2", 0));
				this.m_htItemInfo.Add("FreezComm", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_FreezComm"), 0, "F2", 0));
				this.m_htItemInfo.Add("Comm", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_FirmComm"), 0, "F2", 0));
				this.m_htItemInfo.Add("TransLiqpl", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_TransLiqpl"), 0, "F2", 0));
				this.m_htItemInfo.Add("Ioamt", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_Ioamt"), 0, "F2", 0));
				this.m_htItemInfo.Add("MarginAmt", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_MarginAmt"), 0, "F2", 0));
				this.m_htItemInfo.Add("ChkFund", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_ChkFund"), 0, "F2", 0));
				this.m_htItemInfo.Add("TTlMargin", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_TTlMargin"), 0, "F2", 0));
				this.m_htItemInfo.Add("Zjaql", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_Zjaql"), 0, "F2", 0));
				this.m_htItemInfo.Add("Code", new ColItemInfo(Global.m_PMESResourceManager.GetString("TradeStr_Code"), 0, "", 0));
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
			catch (Exception ex)
			{
				Logger.wirte(ex);
			}
		}
	}
}
