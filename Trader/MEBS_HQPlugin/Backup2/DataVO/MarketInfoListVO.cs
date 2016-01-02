// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.DataVO.MarketInfoListVO
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

using System.Text;

namespace Gnnt.MEBS.HQModel.DataVO
{
  public class MarketInfoListVO
  {
    public MarketInfoVO[] marketInfos = new MarketInfoVO[0];
    public int count;

    public override string ToString()
    {
      string str = "\n";
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("**MarketInfoListVO**" + str);
      stringBuilder.Append("count:" + (object) this.count + str);
      for (int index = 0; index < this.marketInfos.Length; ++index)
        stringBuilder.Append(this.marketInfos[index].ToString());
      stringBuilder.Append(str);
      return stringBuilder.ToString();
    }
  }
}
