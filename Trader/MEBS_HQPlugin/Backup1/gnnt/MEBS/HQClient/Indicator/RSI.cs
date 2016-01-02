// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.RSI
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System.Drawing;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
  internal class RSI : IndicatorBase
  {
    private readonly int[] m_iParam = new int[3]
    {
      6,
      12,
      24
    };

    public RSI(IndicatorPos pos, int Precision)
      : base(pos, Precision)
    {
      this.m_strIndicatorName = "RSI(" + (object) this.m_iParam[0] + "," + (string) (object) this.m_iParam[1] + "," + (string) (object) this.m_iParam[2] + ")";
      this.m_strParamName = new string[3]
      {
        "RSI1",
        "RSI2",
        "RSI3"
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
      for (int index = 0; index < 3; ++index)
        this.DrawLine(g, this.m_data[index], this.m_iParam[index], SetInfo.RHColor.clIndicator[index]);
    }

    public override void Calculate()
    {
      this.m_data = new float[3][];
      this.m_data[0] = new float[this.m_kData.Length];
      this.m_data[1] = new float[this.m_kData.Length];
      this.m_data[2] = new float[this.m_kData.Length];
      if (this.m_kData == null || this.m_kData.Length == 0)
        return;
      for (int index = 0; index < 3; ++index)
        this.GetRSI(this.m_iParam[index], this.m_data[index]);
    }

    private void GetRSI(int n, float[] rsi)
    {
      if (n > this.m_kData.Length || n < 1)
        return;
      float num1 = 0.0f;
      float num2 = 0.0f;
      for (int index = 1; index < n; ++index)
      {
        if ((double) this.m_kData[index].closePrice > (double) this.m_kData[index - 1].closePrice)
          num1 += this.m_kData[index].closePrice - this.m_kData[index - 1].closePrice;
        else
          num2 += this.m_kData[index - 1].closePrice - this.m_kData[index].closePrice;
      }
      rsi[n - 1] = (double) num1 + (double) num2 != 0.0 ? (float) ((double) num1 / ((double) num1 + (double) num2) * 100.0) : 50f;
      float num3;
      float num4 = num3 = 0.0f;
      for (int index = n; index < this.m_kData.Length; ++index)
      {
        num1 -= num4;
        num2 -= num3;
        if ((double) this.m_kData[index].closePrice > (double) this.m_kData[index - 1].closePrice)
          num1 += this.m_kData[index].closePrice - this.m_kData[index - 1].closePrice;
        else
          num2 += this.m_kData[index - 1].closePrice - this.m_kData[index].closePrice;
        rsi[index] = (double) num1 + (double) num2 != 0.0 ? (float) ((double) num1 / ((double) num1 + (double) num2) * 100.0) : rsi[index - 1];
        num4 = num3 = 0.0f;
        if ((double) this.m_kData[index - n + 1].closePrice > (double) this.m_kData[index - n].closePrice)
          num4 = this.m_kData[index - n + 1].closePrice - this.m_kData[index - n].closePrice;
        else
          num3 = this.m_kData[index - n].closePrice - this.m_kData[index - n + 1].closePrice;
      }
    }
  }
}
