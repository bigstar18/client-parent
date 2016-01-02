// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.DMI
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
  internal class DMI : IndicatorBase
  {
    private readonly int[] m_iParam = new int[3]
    {
      7,
      6,
      5
    };

    public DMI(IndicatorPos pos, int Precision)
      : base(pos, Precision)
    {
      this.m_strIndicatorName = "DMI";
      this.m_strIndicatorName += "(";
      for (int index = 0; index < this.m_iParam.Length; ++index)
      {
        if (index > 0)
          this.m_strIndicatorName += ",";
        this.m_strIndicatorName += (string) (object) this.m_iParam[index];
      }
      this.m_strIndicatorName += ")";
      this.m_strParamName = new string[4]
      {
        "+DI",
        "-DI",
        "ADX",
        "ADXR"
      };
      this.m_iPrecision = 2;
    }

    public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
    {
      base.Paint(g, rc, data);
      this.Calculate();
      this.m_max = -10000f;
      this.m_min = 10000f;
      this.GetValueMaxMin(this.m_data[0], this.m_iParam[0]);
      this.GetValueMaxMin(this.m_data[1], this.m_iParam[0]);
      this.GetValueMaxMin(this.m_data[2], this.m_iParam[0] + this.m_iParam[1] - 1);
      this.GetValueMaxMin(this.m_data[3], this.m_iParam[0] + this.m_iParam[1] + this.m_iParam[2] - 1);
      this.DrawCoordinate(g, 2);
      this.DrawLine(g, this.m_data[0], this.m_iParam[0], SetInfo.RHColor.clIndicator[0]);
      this.DrawLine(g, this.m_data[1], this.m_iParam[0], SetInfo.RHColor.clIndicator[1]);
      this.DrawLine(g, this.m_data[2], this.m_iParam[0] + this.m_iParam[1] - 1, SetInfo.RHColor.clIndicator[2]);
      this.DrawLine(g, this.m_data[3], this.m_iParam[0] + this.m_iParam[1] + this.m_iParam[2] - 1, SetInfo.RHColor.clIndicator[3]);
    }

    public override void Calculate()
    {
      this.m_data = new float[5][];
      if (this.m_kData == null || this.m_kData.Length <= 0)
        return;
      int begin = this.m_iParam[0];
      int n = this.m_iParam[1];
      int num1 = this.m_iParam[2];
      for (int index = 0; index < 5; ++index)
        this.m_data[index] = new float[this.m_kData.Length];
      float[] numArray1 = this.m_data[0];
      float[] numArray2 = this.m_data[1];
      float[] numArray3 = this.m_data[2];
      float[] destination = this.m_data[2];
      float[] numArray4 = this.m_data[3];
      float[] numArray5 = this.m_data[3];
      float[] numArray6 = this.m_data[4];
      float[] source = this.m_data[4];
      if (this.m_kData.Length < begin)
        return;
      for (int index = 1; index < this.m_kData.Length; ++index)
      {
        float val1_1 = Math.Abs(this.m_kData[index].highPrice - this.m_kData[index].lowPrice);
        float val1_2 = Math.Abs(this.m_kData[index].highPrice - this.m_kData[index - 1].closePrice);
        float val2 = Math.Abs(this.m_kData[index].lowPrice - this.m_kData[index - 1].closePrice);
        numArray3[index] = Math.Max(val1_1, Math.Max(val1_2, val2));
        float num2 = this.m_kData[index].highPrice - this.m_kData[index - 1].highPrice;
        float num3 = this.m_kData[index - 1].lowPrice - this.m_kData[index].lowPrice;
        float num4 = (double) num2 <= 0.0 ? 0.0f : num2;
        float num5 = (double) num3 <= 0.0 ? 0.0f : num3;
        numArray5[index] = 0.0f;
        numArray6[index] = 0.0f;
        if ((double) num4 > (double) num5)
          numArray5[index] = num4;
        else if ((double) num4 < (double) num5)
          numArray6[index] = num5;
      }
      double num6;
      float num7 = (float) (num6 = 0.0);
      float num8 = (float) num6;
      float num9 = (float) num6;
      for (int index = 1; index < begin; ++index)
      {
        num9 += numArray3[index];
        num8 += numArray5[index];
        num7 += numArray6[index];
      }
      float num10;
      float num11 = num10 = 0.0f;
      for (int index1 = begin; index1 < this.m_kData.Length; ++index1)
      {
        float num2 = num9 + numArray3[index1];
        float num3 = num8 + numArray5[index1];
        float num4 = num7 + numArray6[index1];
        numArray1[index1] = num11;
        numArray2[index1] = num10;
        if ((double) num2 != 0.0)
        {
          numArray1[index1] = (float) ((double) num3 / (double) num2 * 100.0);
          numArray2[index1] = (float) ((double) num4 / (double) num2 * 100.0);
        }
        num11 = numArray1[index1];
        num10 = numArray2[index1];
        int index2 = index1 - begin + 1;
        num9 = num2 - numArray3[index2];
        num8 = num3 - numArray5[index2];
        num7 = num4 - numArray6[index2];
      }
      for (int index = begin; index < this.m_kData.Length; ++index)
        source[index] = (double) numArray1[index] + (double) numArray2[index] == 0.0 ? 0.0f : (float) ((double) Math.Abs(numArray1[index] - numArray2[index]) / (double) Math.Abs(numArray1[index] + numArray2[index]) * 100.0);
      IndicatorBase.Average(begin, this.m_kData.Length, n, source, destination);
      for (int index = begin + n + num1 - 1; index < this.m_kData.Length; ++index)
        numArray4[index] = (float) (((double) destination[index] + (double) destination[index - num1]) / 2.0);
    }
  }
}
