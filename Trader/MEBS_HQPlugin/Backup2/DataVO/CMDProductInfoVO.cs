// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQModel.DataVO.CMDProductInfoVO
// Assembly: HQModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FD1CB918-942E-47F7-BED2-EBD1E7FF35B7
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQModel.dll

using Gnnt.MEBS.HQModel.Service.IO;

namespace Gnnt.MEBS.HQModel.DataVO
{
  public class CMDProductInfoVO : CMDVO
  {
    public int date;
    public int time;

    public CMDProductInfoVO()
    {
      this.cmd = (byte) 3;
    }

    public static ProductInfoListVO getObj(InputStreamConvert input)
    {
      ProductInfoListVO productInfoListVo = new ProductInfoListVO();
      productInfoListVo.date = input.ReadJavaInt();
      productInfoListVo.time = input.ReadJavaInt();
      ProductInfoVO[] productInfoVoArray = new ProductInfoVO[input.ReadJavaInt()];
      for (int index1 = 0; index1 < productInfoVoArray.Length; ++index1)
      {
        productInfoVoArray[index1] = new ProductInfoVO();
        productInfoVoArray[index1].code = input.ReadJavaUTF();
        productInfoVoArray[index1].marketID = input.ReadJavaUTF();
        productInfoVoArray[index1].fUnit = input.ReadJavaFloat();
        productInfoVoArray[index1].name = input.ReadJavaUTF();
        productInfoVoArray[index1].status = input.ReadJavaInt();
        productInfoVoArray[index1].industry = input.ReadJavaUTF();
        productInfoVoArray[index1].region = input.ReadJavaUTF();
        productInfoVoArray[index1].pyName = new string[input.ReadJavaInt()];
        for (int index2 = 0; index2 < productInfoVoArray[index1].pyName.Length; ++index2)
          productInfoVoArray[index1].pyName[index2] = input.ReadJavaUTF();
        productInfoVoArray[index1].tradeSecNo = new int[input.ReadJavaInt()];
        for (int index2 = 0; index2 < productInfoVoArray[index1].tradeSecNo.Length; ++index2)
          productInfoVoArray[index1].tradeSecNo[index2] = input.ReadJavaInt();
        productInfoVoArray[index1].mPrice = input.ReadJavaFloat();
        if ((double) productInfoVoArray[index1].fUnit <= 0.0)
          productInfoVoArray[index1].fUnit = 1f;
      }
      productInfoListVo.productInfos = productInfoVoArray;
      input = (InputStreamConvert) null;
      return productInfoListVo;
    }
  }
}
