// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.DataVO.CMDQuoteListVO
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

using Gnnt.MEBS.HQModel.Service.IO;

namespace Gnnt.MEBS.HQModel.DataVO
{
  public class CMDQuoteListVO : CMDVO
  {
    public CMDQuoteListVO()
    {
      this.cmd = (byte) 5;
    }

    public static ProductDataVO[] getObj(InputStreamConvert input)
    {
      ProductDataVO[] productDataVoArray = new ProductDataVO[input.ReadJavaInt()];
      for (int index = 0; index < productDataVoArray.Length; ++index)
      {
        productDataVoArray[index] = new ProductDataVO();
        productDataVoArray[index].marketID = input.ReadJavaUTF();
        productDataVoArray[index].code = input.ReadJavaUTF();
        productDataVoArray[index].yesterBalancePrice = input.ReadJavaFloat();
        productDataVoArray[index].closePrice = input.ReadJavaFloat();
        productDataVoArray[index].openPrice = input.ReadJavaFloat();
        productDataVoArray[index].highPrice = input.ReadJavaFloat();
        productDataVoArray[index].lowPrice = input.ReadJavaFloat();
        productDataVoArray[index].curPrice = input.ReadJavaFloat();
        productDataVoArray[index].totalAmount = input.ReadJavaLong();
        productDataVoArray[index].totalMoney = input.ReadJavaDouble();
        productDataVoArray[index].curAmount = input.ReadJavaInt();
        productDataVoArray[index].consignRate = input.ReadJavaFloat();
        productDataVoArray[index].amountRate = input.ReadJavaFloat();
        productDataVoArray[index].balancePrice = input.ReadJavaFloat();
        productDataVoArray[index].reserveCount = input.ReadJavaInt();
        productDataVoArray[index].buyAmount = new int[1];
        productDataVoArray[index].buyAmount[0] = input.ReadJavaInt();
        productDataVoArray[index].sellAmount = new int[1];
        productDataVoArray[index].sellAmount[0] = input.ReadJavaInt();
        productDataVoArray[index].buyPrice = new float[1];
        productDataVoArray[index].buyPrice[0] = input.ReadJavaFloat();
        productDataVoArray[index].sellPrice = new float[1];
        productDataVoArray[index].sellPrice[0] = input.ReadJavaFloat();
        productDataVoArray[index].expStr = input.ReadJavaUTF();
      }
      return productDataVoArray;
    }
  }
}
