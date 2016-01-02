using System;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class CommodityInfo
	{
		public string marketID = string.Empty;
		public string region;
		public string industry;
		public string commodityCode = string.Empty;
		public CommodityInfo(string _marketID, string _commodityCode)
		{
			this.marketID = _marketID;
			this.commodityCode = _commodityCode;
		}
		public CommodityInfo(string _marketID, string _commodityCode, string _region, string _industry)
		{
			this.marketID = _marketID;
			this.commodityCode = _commodityCode;
			this.region = _region;
			this.industry = _industry;
		}
		public bool Compare(CommodityInfo commodityInfo)
		{
			return commodityInfo.marketID.Equals(this.marketID) && commodityInfo.commodityCode.Equals(this.commodityCode);
		}
		public bool Compare(object obj)
		{
			CommodityInfo commodityInfo = (CommodityInfo)obj;
			return this.Compare(commodityInfo);
		}
		public static CommodityInfo DealCode(string dealCode)
		{
			string text = "00";
			string text2 = dealCode;
			int num = dealCode.IndexOf("_");
			if (num != -1)
			{
				text = dealCode.Substring(0, num);
				text2 = dealCode.Substring(num + 1);
			}
			if (text.Length == 0)
			{
				text = "00";
			}
			return new CommodityInfo(text, text2);
		}
	}
}
