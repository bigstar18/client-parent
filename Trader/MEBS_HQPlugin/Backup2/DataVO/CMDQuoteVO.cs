// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.DataVO.CMDQuoteVO
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

using Gnnt.MEBS.HQModel.Service.IO;

namespace Gnnt.MEBS.HQModel.DataVO
{
  public class CMDQuoteVO : CMDVO
  {
    public string[,] codeList = new string[0, 0];
    public byte isAll;
    public int count;

    public CMDQuoteVO()
    {
      this.cmd = (byte) 4;
    }

    public static ProductDataVO[] getObj(InputStreamConvert input)
    {
      byte num = input.ReadJavaByte();
      ProductDataVO[] productDataVoArray = new ProductDataVO[input.ReadJavaInt()];
      for (int index1 = 0; index1 < productDataVoArray.Length; ++index1)
      {
        productDataVoArray[index1] = new ProductDataVO();
        productDataVoArray[index1].marketID = input.ReadJavaUTF();
        productDataVoArray[index1].code = input.ReadJavaUTF();
        int yyyyMMdd = input.ReadJavaInt();
        int HHmmss = input.ReadJavaInt();
        productDataVoArray[index1].time = TradeTimeVO.HHmmssToDateTime(yyyyMMdd, HHmmss);
        productDataVoArray[index1].closePrice = input.ReadJavaFloat();
        productDataVoArray[index1].openPrice = input.ReadJavaFloat();
        productDataVoArray[index1].highPrice = input.ReadJavaFloat();
        productDataVoArray[index1].lowPrice = input.ReadJavaFloat();
        productDataVoArray[index1].curPrice = input.ReadJavaFloat();
        productDataVoArray[index1].totalAmount = input.ReadJavaLong();
        productDataVoArray[index1].totalMoney = input.ReadJavaDouble();
        productDataVoArray[index1].curAmount = input.ReadJavaInt();
        productDataVoArray[index1].consignRate = input.ReadJavaFloat();
        productDataVoArray[index1].amountRate = input.ReadJavaFloat();
        productDataVoArray[index1].balancePrice = input.ReadJavaFloat();
        productDataVoArray[index1].reserveCount = input.ReadJavaInt();
        productDataVoArray[index1].yesterBalancePrice = input.ReadJavaFloat();
        productDataVoArray[index1].reserveChange = input.ReadJavaInt();
        if ((int) num == 1)
        {
          productDataVoArray[index1].inAmount = input.ReadJavaInt();
          productDataVoArray[index1].outAmount = input.ReadJavaInt();
          productDataVoArray[index1].buyAmount = new int[input.ReadJavaInt()];
          for (int index2 = 0; index2 < productDataVoArray[index1].buyAmount.Length; ++index2)
            productDataVoArray[index1].buyAmount[index2] = input.ReadJavaInt();
          productDataVoArray[index1].sellAmount = new int[input.ReadJavaInt()];
          for (int index2 = 0; index2 < productDataVoArray[index1].sellAmount.Length; ++index2)
            productDataVoArray[index1].sellAmount[index2] = input.ReadJavaInt();
          productDataVoArray[index1].buyPrice = new float[input.ReadJavaInt()];
          for (int index2 = 0; index2 < productDataVoArray[index1].buyPrice.Length; ++index2)
            productDataVoArray[index1].buyPrice[index2] = input.ReadJavaFloat();
          productDataVoArray[index1].sellPrice = new float[input.ReadJavaInt()];
          for (int index2 = 0; index2 < productDataVoArray[index1].sellPrice.Length; ++index2)
            productDataVoArray[index1].sellPrice[index2] = input.ReadJavaFloat();
        }
        productDataVoArray[index1].expStr = input.ReadJavaUTF();
      }
      return productDataVoArray;
    }
  }
}
