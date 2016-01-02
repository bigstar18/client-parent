// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.CCI
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
  internal class CCI : IndicatorBase
  {
    private readonly int[] m_iParam = new int[1]
    {
      21
    };

    public CCI(IndicatorPos pos, int Precision)
      : base(pos, Precision)
    {
      this.m_strIndicatorName = "CCI";
      this.m_strParamName = new string[1]
      {
        ""
      };
      this.m_iPrecision = 2;
    }

    public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
    {
      base.Paint(g, rc, data);
      this.Calculate();
      this.m_max = -10000f;
      this.m_min = 10000f;
      this.GetValueMaxMin(this.m_data[0], this.m_iParam[0] - 1);
      this.DrawCoordinate(g, 2);
      this.DrawLine(g, this.m_data[0], this.m_iParam[0] - 1, SetInfo.RHColor.clIndicator[0]);
    }

    public override void Calculate()
    {
      this.m_data = new float[2][];
      if (this.m_kData == null || this.m_kData.Length <= 0)
        return;
      int num1 = this.m_iParam[0];
      if (num1 > this.m_kData.Length || num1 < 2)
        return;
      this.m_data[0] = new float[this.m_kData.Length];
      this.m_data[1] = new float[this.m_kData.Length];
      float[] numArray1 = this.m_data[0];
      float[] numArray2 = this.m_data[1];
      double num2 = 0.0;
      for (int index = 0; index < num1 - 1; ++index)
        num2 += ((double) this.m_kData[index].highPrice + (double) this.m_kData[index].lowPrice + (double) this.m_kData[index].closePrice) / 3.0;
      float num3 = 0.0f;
      for (int index = num1 - 1; index < this.m_kData.Length; ++index)
      {
        num2 = num2 - (double) num3 + ((double) this.m_kData[index].highPrice + (double) this.m_kData[index].lowPrice + (double) this.m_kData[index].closePrice) / 3.0;
        numArray2[index] = (float) num2 / (float) num1;
        num3 = (float) (((double) this.m_kData[index - num1 + 1].highPrice + (double) this.m_kData[index - num1 + 1].lowPrice + (double) this.m_kData[index - num1 + 1].closePrice) / 3.0);
      }
      numArray1[num1 - 2] = 0.0f;
      for (int index1 = num1 - 1; index1 < this.m_kData.Length; ++index1)
      {
        double num4 = 0.0;
        for (int index2 = index1 - num1 + 1; index2 <= index1; ++index2)
          num4 += (double) Math.Abs((float) (((double) this.m_kData[index2].highPrice + (double) this.m_kData[index2].lowPrice + (double) this.m_kData[index2].closePrice) / 3.0) - numArray2[index1]);
        numArray1[index1] = num4 != 0.0 ? (float) ((((double) this.m_kData[index1].highPrice + (double) this.m_kData[index1].lowPrice + (double) this.m_kData[index1].closePrice) / 3.0 - (double) numArray2[index1]) / (0.015 * num4 / (double) num1)) : numArray1[index1 - 1];
      }
    }
  }
}
