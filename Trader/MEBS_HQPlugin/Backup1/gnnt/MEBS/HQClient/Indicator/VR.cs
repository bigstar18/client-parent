// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.VR
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System.Drawing;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
  internal class VR : IndicatorBase
  {
    private readonly int[] m_iParam = new int[2]
    {
      25,
      5
    };

    public VR(IndicatorPos pos, int Precision)
      : base(pos, Precision)
    {
      this.m_strIndicatorName = "VR";
      this.m_strIndicatorName += "(";
      for (int index = 0; index < this.m_iParam.Length; ++index)
      {
        if (index > 0)
          this.m_strIndicatorName += ",";
        this.m_strIndicatorName += (string) (object) this.m_iParam[index];
      }
      this.m_strIndicatorName += ")";
      this.m_strParamName = new string[2]
      {
        "",
        "MA"
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
      this.DrawCoordinate(g, 2);
      this.DrawLine(g, this.m_data[0], this.m_iParam[0], SetInfo.RHColor.clIndicator[0]);
      this.DrawLine(g, this.m_data[1], this.m_iParam[0] + this.m_iParam[1] - 1, SetInfo.RHColor.clIndicator[1]);
    }

    public override void Calculate()
    {
      this.m_data = new float[2][];
      if (this.m_kData == null || this.m_kData.Length <= 0)
        return;
      this.m_data[0] = new float[this.m_kData.Length];
      this.m_data[1] = new float[this.m_kData.Length];
      int begin = this.m_iParam[0];
      int n = this.m_iParam[1];
      float[] source = this.m_data[0];
      float[] destination = this.m_data[1];
      double num1;
      float num2 = (float) (num1 = 0.0);
      float num3 = (float) num1;
      float num4 = (float) num1;
      if (this.m_kData.Length < begin)
        return;
      source[begin - 2] = 100f;
      for (int index = 1; index < begin; ++index)
      {
        if ((double) this.m_kData[index].closePrice == (double) this.m_kData[index - 1].closePrice)
          num2 += (float) this.m_kData[index].totalAmount;
        else if ((double) this.m_kData[index].closePrice > (double) this.m_kData[index - 1].closePrice)
          num4 += (float) this.m_kData[index].totalAmount;
        else
          num3 += (float) this.m_kData[index].totalAmount;
      }
      source[begin - 1] = (double) num3 + (double) num2 / 2.0 != 0.0 ? (float) (((double) num4 + (double) num2 / 2.0) / ((double) num3 + (double) num2 / 2.0)) : source[begin - 2];
      for (int index = begin; index < this.m_kData.Length; ++index)
      {
        if ((double) this.m_kData[index].closePrice == (double) this.m_kData[index - 1].closePrice)
          num2 += (float) this.m_kData[index].totalAmount;
        else if ((double) this.m_kData[index].closePrice > (double) this.m_kData[index - 1].closePrice)
          num4 += (float) this.m_kData[index].totalAmount;
        else
          num3 += (float) this.m_kData[index].totalAmount;
        source[index] = (double) num3 + (double) num2 / 2.0 != 0.0 ? (float) (((double) num4 + (double) num2 / 2.0) / ((double) num3 + (double) num2 / 2.0) * 100.0) : source[index - 1];
        if ((double) this.m_kData[index - begin + 1].closePrice == (double) this.m_kData[index - begin].closePrice)
          num2 -= (float) this.m_kData[index - begin + 1].totalAmount;
        else if ((double) this.m_kData[index - begin + 1].closePrice > (double) this.m_kData[index - begin].closePrice)
          num4 -= (float) this.m_kData[index - begin + 1].totalAmount;
        else
          num3 -= (float) this.m_kData[index - begin + 1].totalAmount;
      }
      IndicatorBase.Average(begin, this.m_kData.Length, n, source, destination);
    }
  }
}
