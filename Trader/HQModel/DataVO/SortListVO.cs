// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.DataVO.SortListVO
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

namespace Gnnt.MEBS.HQModel.DataVO
{
  public class SortListVO
  {
    public string[] codeList = new string[0];
    public const int SORT_KEY_MAX = 10;
    public const int SORT_BY_CODE = 0;
    public const int SORT_BY_PRICE = 1;
    public const int SORT_BY_UP = 2;
    public const int SORT_BY_UPRATE = 3;
    public const int SORT_BY_SHAKERATE = 4;
    public const int SORT_BY_AMOUNTRATE = 5;
    public const int SORT_BY_MONEY = 6;
    public const int SORT_BY_CONSIGNRATE = 7;
    public const int SORT_BY_MINRATE = 8;
    public const int SORT_BY_TOTALAMOUNT = 9;
    public int sortKey;

    public object clone()
    {
      SortListVO sortListVo = new SortListVO();
      sortListVo.sortKey = this.sortKey;
      if (this.codeList != null)
      {
        sortListVo.codeList = new string[this.codeList.Length];
        for (int index = 0; index < sortListVo.codeList.Length; ++index)
          sortListVo.codeList[index] = this.codeList[index];
      }
      return (object) sortListVo;
    }
  }
}
