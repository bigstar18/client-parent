// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.DataVO.CMDTradeTimeVO
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

using Gnnt.MEBS.HQModel.Service.IO;
using System.Collections;

namespace Gnnt.MEBS.HQModel.DataVO
{
  public class CMDTradeTimeVO : CMDVO
  {
    public CMDTradeTimeVO()
    {
      this.cmd = (byte) 8;
    }

    public static Hashtable getObj(InputStreamConvert input)
    {
      Hashtable hashtable = new Hashtable();
      int num = input.ReadJavaInt();
      for (int index1 = 0; index1 < num; ++index1)
      {
        string str = input.ReadJavaUTF();
        MarketDataVO marketDataVo = new MarketDataVO();
        marketDataVo.marketID = str;
        TradeTimeVO[] tradeTimeVoArray = new TradeTimeVO[input.ReadJavaInt()];
        for (int index2 = 0; index2 < tradeTimeVoArray.Length; ++index2)
        {
          tradeTimeVoArray[index2] = new TradeTimeVO();
          tradeTimeVoArray[index2].orderID = input.ReadJavaInt();
          tradeTimeVoArray[index2].beginDate = input.ReadJavaInt();
          tradeTimeVoArray[index2].beginTime = input.ReadJavaInt();
          tradeTimeVoArray[index2].endDate = input.ReadJavaInt();
          tradeTimeVoArray[index2].endTime = input.ReadJavaInt();
          tradeTimeVoArray[index2].tradeDate = input.ReadJavaInt();
          tradeTimeVoArray[index2].status = input.ReadJavaInt();
        }
        marketDataVo.m_timeRange = tradeTimeVoArray;
        hashtable.Add((object) str, (object) marketDataVo);
      }
      return hashtable;
    }
  }
}
