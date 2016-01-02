// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.DataVO.CommodityInfo
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

namespace Gnnt.MEBS.HQModel.DataVO
{
  public class CommodityInfo
  {
    public string marketID = string.Empty;
    public string commodityCode = string.Empty;
    public string region;
    public string industry;

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
      return this.Compare((CommodityInfo) obj);
    }

    public static CommodityInfo DealCode(string dealCode)
    {
      string str = dealCode;
      string _marketID = "00";
      string _commodityCode = str;
      int length = str.IndexOf("_");
      if (length != -1)
      {
        _marketID = str.Substring(0, length);
        _commodityCode = str.Substring(length + 1);
      }
      if (_marketID.Length == 0)
        _marketID = "00";
      return new CommodityInfo(_marketID, _commodityCode);
    }
  }
}
