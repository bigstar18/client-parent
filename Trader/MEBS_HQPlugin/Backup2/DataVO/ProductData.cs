// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.DataVO.ProductData
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

using System.Collections;

namespace Gnnt.MEBS.HQModel.DataVO
{
  public class ProductData
  {
    public ArrayList aBill = ArrayList.Synchronized(new ArrayList());
    public ArrayList lastBill = ArrayList.Synchronized(new ArrayList());
    public CommodityInfo commodityInfo;
    public ProductDataVO realData;
    public ArrayList aMinLine;
    public KLineData[] dayKLine;
    public KLineData[] min5KLine;
    public KLineData[] min1KLine;
    public KLineData[] hrKLine;
  }
}
