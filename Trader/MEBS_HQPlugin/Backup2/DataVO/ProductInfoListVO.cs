// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.DataVO.ProductInfoListVO
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

using System.Text;

namespace Gnnt.MEBS.HQModel.DataVO
{
  public class ProductInfoListVO
  {
    public ProductInfoVO[] productInfos = new ProductInfoVO[0];
    public int date;
    public int time;
    public int count;

    public override string ToString()
    {
      string str = "\n";
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("**ProductInfoListVO**" + str);
      stringBuilder.Append("date:" + (object) this.date + str);
      stringBuilder.Append("time:" + (object) this.time + str);
      stringBuilder.Append("count:" + (object) this.count + str);
      for (int index = 0; index < this.productInfos.Length; ++index)
        stringBuilder.Append(this.productInfos[index].ToString());
      stringBuilder.Append(str);
      return stringBuilder.ToString();
    }
  }
}
