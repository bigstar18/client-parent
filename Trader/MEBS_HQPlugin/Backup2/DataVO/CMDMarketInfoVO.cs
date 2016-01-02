// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.DataVO.CMDMarketInfoVO
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

using Gnnt.MEBS.HQModel.Service.IO;

namespace Gnnt.MEBS.HQModel.DataVO
{
  public class CMDMarketInfoVO : CMDVO
  {
    public int date;
    public int time;

    public CMDMarketInfoVO()
    {
      this.cmd = (byte) 17;
    }

    public static MarketInfoListVO getObj(InputStreamConvert input)
    {
      MarketInfoListVO marketInfoListVo = new MarketInfoListVO();
      MarketInfoVO[] marketInfoVoArray = new MarketInfoVO[input.ReadJavaInt()];
      for (int index = 0; index < marketInfoVoArray.Length; ++index)
      {
        marketInfoVoArray[index] = new MarketInfoVO();
        marketInfoVoArray[index].marketID = input.ReadJavaUTF();
        marketInfoVoArray[index].marketName = input.ReadJavaUTF();
      }
      marketInfoListVo.marketInfos = marketInfoVoArray;
      input = (InputStreamConvert) null;
      return marketInfoListVo;
    }
  }
}
