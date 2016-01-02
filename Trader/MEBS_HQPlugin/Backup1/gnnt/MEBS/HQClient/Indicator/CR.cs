// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.CR
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System.Drawing;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
  internal class CR : IndicatorBase
  {
    private readonly int[] m_iParam = new int[4]
    {
      26,
      10,
      20,
      40
    };

    public CR(IndicatorPos pos, int Precision)
      : base(pos, Precision)
    {
      this.m_strIndicatorName = "CR";
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
        "CR",
        "a",
        "b",
        "c"
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
      this.GetValueMaxMin(this.m_data[1], this.m_iParam[0] + this.m_iParam[1]);
      this.GetValueMaxMin(this.m_data[2], this.m_iParam[0] + this.m_iParam[2]);
      this.GetValueMaxMin(this.m_data[3], this.m_iParam[0] + this.m_iParam[3]);
      this.DrawCoordinate(g, 2);
      this.DrawLine(g, this.m_data[0], this.m_iParam[0], SetInfo.RHColor.clIndicator[0]);
      this.DrawLine(g, this.m_data[1], this.m_iParam[0] + this.m_iParam[1], SetInfo.RHColor.clIndicator[1]);
      this.DrawLine(g, this.m_data[2], this.m_iParam[0] + this.m_iParam[2], SetInfo.RHColor.clIndicator[2]);
      this.DrawLine(g, this.m_data[3], this.m_iParam[0] + this.m_iParam[3], SetInfo.RHColor.clIndicator[3]);
    }

    public override void Calculate()
    {
      this.m_data = new float[4][];
      if (this.m_kData == null || this.m_kData.Length <= 0)
        return;
      int n = this.m_iParam[0];
      for (int index = 0; index < 4; ++index)
        this.m_data[index] = new float[this.m_kData.Length];
      this.GetCR(n, this.m_data[0]);
      IndicatorBase.Average(this.m_iParam[0], this.m_kData.Length, this.m_iParam[1], this.m_data[0], this.m_data[1]);
      IndicatorBase.Average(this.m_iParam[0], this.m_kData.Length, this.m_iParam[2], this.m_data[0], this.m_data[2]);
      IndicatorBase.Average(this.m_iParam[0], this.m_kData.Length, this.m_iParam[3], this.m_data[0], this.m_data[3]);
    }

    private void GetCR(int n, float[] cr)
    {
      if (this.m_kData.Length < n)
        return;
      float num1;
      float num2 = num1 = 0.0f;
      for (int index = 1; index < n; ++index)
      {
        float num3 = (float) (((double) this.m_kData[index - 1].highPrice + (double) this.m_kData[index - 1].lowPrice) / 2.0);
        num2 += (double) this.m_kData[index].highPrice - (double) num3 > 0.0 ? this.m_kData[index].highPrice - num3 : 0.0f;
        num1 += (double) num3 - (double) this.m_kData[index].lowPrice > 0.0 ? num3 - this.m_kData[index].lowPrice : 0.0f;
      }
      float num4 = 0.0f;
      for (int index1 = n; index1 < this.m_kData.Length; ++index1)
      {
        float num3 = (float) (((double) this.m_kData[index1 - 1].highPrice + (double) this.m_kData[index1 - 1].lowPrice) / 2.0);
        float num5 = num2 + ((double) this.m_kData[index1].highPrice - (double) num3 > 0.0 ? this.m_kData[index1].highPrice - num3 : 0.0f);
        float num6 = num1 + ((double) num3 - (double) this.m_kData[index1].lowPrice > 0.0 ? num3 - this.m_kData[index1].lowPrice : 0.0f);
        cr[index1] = num4;
        if ((double) num6 != 0.0)
          cr[index1] = (float) ((double) num5 / (double) num6 * 100.0);
        num4 = cr[index1];
        int index2 = index1 - n + 1;
        float num7 = (float) (((double) this.m_kData[index2 - 1].highPrice + (double) this.m_kData[index2 - 1].lowPrice) / 2.0);
        num2 = num5 - ((double) this.m_kData[index2].highPrice - (double) num7 > 0.0 ? this.m_kData[index2].highPrice - num7 : 0.0f);
        num1 = num6 - ((double) num7 - (double) this.m_kData[index2].lowPrice > 0.0 ? num7 - this.m_kData[index2].lowPrice : 0.0f);
      }
    }
  }
}
