using System;
using System.Collections;
namespace TradeClientApp.Gnnt.ISSUE.Library
{
	public class InvestorItemInfo
	{
		public string m_strItems;
		public Hashtable m_htItemInfo;
		public InvestorItemInfo()
		{
			this.initAllItem();
		}
		private void initAllItem()
		{
			this.m_strItems = "Account;Name;Bank;Phone;IDNum";
			this.m_htItemInfo = new Hashtable();
			this.m_htItemInfo.Add("Account", new ColItemInfo("", 0, "", 0));
			this.m_htItemInfo.Add("Name", new ColItemInfo("", 0, "", 0));
			this.m_htItemInfo.Add("Bank", new ColItemInfo("", 0, "", 0));
			this.m_htItemInfo.Add("Phone", new ColItemInfo("", 0, "", 0));
			this.m_htItemInfo.Add("IDNum", new ColItemInfo("", 0, "", 0));
			string text = (string)Global.HTConfig["InvestorName"];
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
			string text2 = (string)Global.HTConfig["InvestorItems"];
			if (text2.Length > 0)
			{
				this.m_strItems = text2;
			}
		}
	}
}
