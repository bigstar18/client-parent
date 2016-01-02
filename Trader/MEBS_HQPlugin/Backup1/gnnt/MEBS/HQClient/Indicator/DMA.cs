// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.DMA
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
  internal class DMA : IndicatorBase
  {
    private readonly int[] m_iParam = new int[2]
    {
      10,
      50
    };

    public DMA(IndicatorPos pos, int Precision)
      : base(pos, Precision)
    {
      this.m_strIndicatorName = "DMA";
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
        "DMA",
        "AMA"
      };
      this.m_iPrecision = 2;
    }

    public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
    {
      base.Paint(g, rc, data);
      this.Calculate();
      this.m_max = -10000f;
      this.m_min = 10000f;
      this.GetValueMaxMin(this.m_data[0], this.m_iParam[1] - 1);
      this.GetValueMaxMin(this.m_data[1], this.m_iParam[0] + this.m_iParam[1] - 2);
      this.DrawCoordinate(g, 2);
      this.DrawLine(g, this.m_data[0], this.m_iParam[1] - 1, SetInfo.RHColor.clIndicator[0]);
      this.DrawLine(g, this.m_data[1], this.m_iParam[0] + this.m_iParam[1] - 2, SetInfo.RHColor.clIndicator[1]);
    }

    public override void Calculate()
    {
      this.m_data = new float[2][];
      if (this.m_kData == null || this.m_kData.Length <= 0 || this.m_kData.Length < Math.Max(this.m_iParam[0], this.m_iParam[1]))
        return;
      for (int index = 0; index < 2; ++index)
        this.m_data[index] = new float[this.m_kData.Length];
      float[] numArray1 = this.m_data[0];
      float[] numArray2 = this.m_data[1];
      this.AverageClose(this.m_iParam[0], numArray1);
      this.AverageClose(this.m_iParam[1], numArray2);
      for (int index = this.m_iParam[1] - 1; index < this.m_kData.Length; ++index)
        numArray1[index] -= numArray2[index];
      IndicatorBase.Average(this.m_iParam[1] - 1, this.m_kData.Length, this.m_iParam[0], numArray1, numArray2);
    }
  }
}
