// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.KDJ
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System.Drawing;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
  internal class KDJ : IndicatorBase
  {
    private readonly int[] m_iParam = new int[3]
    {
      9,
      3,
      3
    };

    public KDJ(IndicatorPos pos, int Precision)
      : base(pos, Precision)
    {
      this.m_strIndicatorName = "KDJ(" + (object) this.m_iParam[0] + "," + (string) (object) this.m_iParam[1] + "," + (string) (object) this.m_iParam[2] + ")";
      this.m_strParamName = new string[3]
      {
        "K",
        "D",
        "J"
      };
      this.m_iPrecision = 2;
    }

    public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
    {
      if (data == null || data.Length == 0)
        return;
      base.Paint(g, rc, data);
      this.Calculate();
      this.m_max = 0.0f;
      this.m_min = 10000f;
      for (int index = 0; index < 3; ++index)
        this.GetValueMaxMin(this.m_data[index], this.m_iParam[index]);
      this.DrawCoordinate(g, 2);
      for (int index = 0; index < this.m_iParam.Length; ++index)
        this.DrawLine(g, this.m_data[index], this.m_iParam[index], SetInfo.RHColor.clIndicator[index]);
    }

    public override void Calculate()
    {
      this.m_data = new float[3][];
      int num1 = this.m_iParam[0];
      int num2 = this.m_iParam[1];
      int num3 = this.m_iParam[2];
      this.m_data[0] = new float[this.m_kData.Length];
      this.m_data[1] = new float[this.m_kData.Length];
      this.m_data[2] = new float[this.m_kData.Length];
      if (this.m_kData == null || num1 > this.m_kData.Length || num1 < 1)
        return;
      float[] numArray1 = this.m_data[0];
      float[] numArray2 = this.m_data[1];
      float[] numArray3 = this.m_data[2];
      int num4 = num2 <= 0 ? 3 : num2;
      int num5 = num3 <= 0 ? 3 : num3;
      float num6 = this.m_kData[num1 - 1].highPrice;
      float num7 = this.m_kData[num1 - 1].lowPrice;
      for (int index = num1 - 1; index >= 0; --index)
      {
        if ((double) num6 < (double) this.m_kData[index].highPrice)
          num6 = this.m_kData[index].highPrice;
        if ((double) num7 < (double) this.m_kData[index].lowPrice)
          num7 = this.m_kData[index].lowPrice;
      }
      float num8 = (double) num6 > (double) num7 ? (float) (((double) this.m_kData[num1 - 1].closePrice - (double) num7) / ((double) num6 - (double) num7) * 100.0) : 50f;
      float[] numArray4 = numArray1;
      int index1 = num1 - 1;
      float[] numArray5 = numArray2;
      int index2 = num1 - 1;
      float[] numArray6 = numArray3;
      int index3 = num1 - 1;
      double num9;
      float num10 = (float) (num9 = (double) num8);
      float num11 = (float) num9;
      numArray6[index3] = (float) num9;
      double num12;
      float num13 = (float) (num12 = (double) num11);
      numArray5[index2] = (float) num12;
      double num14 = (double) num13;
      numArray4[index1] = (float) num14;
      for (int index4 = 0; index4 < num1; ++index4)
      {
        numArray1[index4] = 0.0f;
        numArray2[index4] = 0.0f;
        numArray3[index4] = 0.0f;
      }
      for (int index4 = num1; index4 < this.m_kData.Length; ++index4)
      {
        float num15 = this.m_kData[index4].highPrice;
        float num16 = this.m_kData[index4].lowPrice;
        for (int index5 = index4 - 1; index5 > index4 - num1; --index5)
        {
          if ((double) num15 < (double) this.m_kData[index5].highPrice)
            num15 = this.m_kData[index5].highPrice;
          if ((double) num16 > (double) this.m_kData[index5].lowPrice)
            num16 = this.m_kData[index5].lowPrice;
        }
        if ((double) num15 <= (double) num16)
        {
          num8 = num10;
        }
        else
        {
          num10 = num8;
          num8 = (float) (((double) this.m_kData[index4].closePrice - (double) num16) / ((double) num15 - (double) num16) * 100.0);
        }
        numArray1[index4] = (float) ((double) numArray1[index4 - 1] * (double) (num4 - 1) / (double) num4 + (double) num8 / (double) num4);
        numArray2[index4] = (float) ((double) numArray1[index4] / (double) num5 + (double) numArray2[index4 - 1] * (double) (num5 - 1) / (double) num5);
        numArray3[index4] = (float) (3.0 * (double) numArray1[index4] - 2.0 * (double) numArray2[index4]);
      }
    }
  }
}
