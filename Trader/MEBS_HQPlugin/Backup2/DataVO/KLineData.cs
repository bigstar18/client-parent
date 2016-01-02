// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.DataVO.KLineData
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

namespace Gnnt.MEBS.HQModel.DataVO
{
  public class KLineData
  {
    public long date;
    public float openPrice;
    public float closePrice;
    public float highPrice;
    public float lowPrice;
    public float balancePrice;
    public long totalAmount;
    public double totalMoney;
    public int reserveCount;

    public override string ToString()
    {
      return "\r\ndate:" + (object) this.date + "\r\nopenPrice:" + (string) (object) this.openPrice + "\r\nhighPrice:" + (string) (object) this.highPrice + "\r\nlowPrice:" + (string) (object) this.lowPrice + "\r\nclosePrice:" + (string) (object) this.closePrice + "\r\ntotalAmount:" + (string) (object) this.totalAmount + "\r\ntotalMoney:" + (string) (object) this.totalMoney;
    }
  }
}
