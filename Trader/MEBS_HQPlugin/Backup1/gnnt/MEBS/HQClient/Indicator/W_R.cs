// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.W_R
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
  internal class W_R : IndicatorBase
  {
    private readonly int[] m_iParam = new int[2]
    {
      14,
      6
    };

    public W_R(IndicatorPos pos, int Precision)
      : base(pos, Precision)
    {
      this.m_strIndicatorName = "W%R";
      this.m_strIndicatorName += "(";
      for (int index = 0; index < this.m_iParam.Length; ++index)
      {
        if (index > 0)
          this.m_strIndicatorName += ",";
        this.m_strIndicatorName += (string) (object) this.m_iParam[index];
      }
      this.m_strIndicatorName += ")";
      this.m_strParamName = new string[this.m_iParam.Length];
      for (int index = 0; index < this.m_iParam.Length; ++index)
        this.m_strParamName[index] = "WR" + (object) (index + 1);
      this.m_iPrecision = 2;
    }

    public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
    {
      base.Paint(g, rc, data);
      this.Calculate();
      this.m_max = -1E+38f;
      this.m_min = 1E+38f;
      for (int index = 0; index < this.m_iParam.Length; ++index)
        this.GetValueMaxMin(this.m_data[index], this.m_iParam[index]);
      this.DrawCoordinate(g, 2);
      for (int index = 0; index < this.m_iParam.Length; ++index)
        this.DrawLine(g, this.m_data[index], this.m_iParam[index], SetInfo.RHColor.clIndicator[index]);
    }

    public override void Calculate()
    {
      this.m_data = new float[this.m_iParam.Length][];
      if (this.m_kData == null || this.m_kData.Length <= 0)
        return;
      for (int index = 0; index < this.m_iParam.Length; ++index)
      {
        this.m_data[index] = new float[this.m_kData.Length];
        this.GetW_R(this.m_iParam[index], this.m_data[index]);
      }
    }

    private void GetW_R(int n, float[] wms)
    {
      if (n > this.m_kData.Length || n < 1)
        return;
      for (int index1 = n - 1; index1 < this.m_kData.Length; ++index1)
      {
        float val1_1 = this.m_kData[index1].highPrice;
        float val1_2 = this.m_kData[index1].lowPrice;
        for (int index2 = index1 - 1; index2 > index1 - n; --index2)
        {
          val1_1 = Math.Max(val1_1, this.m_kData[index2].highPrice);
          val1_2 = Math.Min(val1_2, this.m_kData[index2].lowPrice);
        }
        wms[index1] = (double) val1_1 - (double) val1_2 != 0.0 ? (float) (-((double) val1_1 - (double) this.m_kData[index1].closePrice) / ((double) val1_1 - (double) val1_2) * 100.0) : (index1 - 1 != 0 ? wms[index1 - 1] : -50f);
      }
    }
  }
}
