// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.ASI
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
  internal class ASI : IndicatorBase
  {
    public ASI(IndicatorPos pos, int Precision)
      : base(pos, Precision)
    {
      this.m_strIndicatorName = "ASI";
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
      this.m_max = -1E+38f;
      this.m_min = 1E+38f;
      this.GetValueMaxMin(this.m_data[0], 1);
      this.DrawCoordinate(g, 2);
      this.DrawLine(g, this.m_data[0], 0, SetInfo.RHColor.clIndicator[0]);
    }

    public override void Calculate()
    {
      this.m_data = new float[1][];
      if (this.m_kData == null || this.m_kData.Length <= 0)
        return;
      this.m_data[0] = new float[this.m_kData.Length];
      float[] numArray = this.m_data[0];
      numArray[0] = 0.0f;
      float num1 = 0.0f;
      for (int index = 1; index < this.m_kData.Length; ++index)
      {
        float val1 = Math.Abs(this.m_kData[index].highPrice - this.m_kData[index - 1].closePrice);
        float val2 = Math.Abs(this.m_kData[index].lowPrice - this.m_kData[index - 1].closePrice);
        float num2 = Math.Abs(this.m_kData[index].highPrice - this.m_kData[index - 1].lowPrice);
        float num3 = Math.Abs(this.m_kData[index - 1].closePrice - this.m_kData[index - 1].openPrice);
        float num4 = this.m_kData[index].closePrice - this.m_kData[index - 1].closePrice + (this.m_kData[index].closePrice - this.m_kData[index].openPrice) / 2f + (this.m_kData[index - 1].closePrice - this.m_kData[index - 1].openPrice);
        float num5 = 0.0f;
        if ((double) val1 >= (double) val2 && (double) val1 >= (double) num2)
          num5 = (float) ((double) val1 + (double) val2 / 2.0 + (double) num3 / 4.0);
        if ((double) val2 >= (double) val1 && (double) val2 >= (double) num2)
          num5 = (float) ((double) val2 + (double) val1 / 2.0 + (double) num3 / 4.0);
        if ((double) num2 >= (double) val1 && (double) num2 >= (double) val2)
          num5 = num2 + num3 / 4f;
        float num6 = Math.Max(val1, val2);
        if ((double) num6 != 0.0)
          num1 = (float) (50.0 * (double) num4 / (double) num5 * (double) num6 / 3.0);
        numArray[index] = numArray[index - 1] + num1;
      }
    }
  }
}
