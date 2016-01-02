// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.WVAD
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System.Drawing;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
  internal class WVAD : IndicatorBase
  {
    private readonly int[] m_iParam = new int[1]
    {
      6
    };

    public WVAD(IndicatorPos pos, int Precision)
      : base(pos, Precision)
    {
      this.m_strIndicatorName = "WVAD";
      this.m_strIndicatorName += "(";
      for (int index = 0; index < this.m_iParam.Length; ++index)
      {
        if (index > 0)
          this.m_strIndicatorName += ",";
        this.m_strIndicatorName += (string) (object) this.m_iParam[index];
      }
      this.m_strIndicatorName += ")";
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
      this.GetValueMaxMin(this.m_data[0], this.m_iParam[0]);
      this.DrawCoordinate(g, 2);
      this.DrawLine(g, this.m_data[0], this.m_iParam[0], SetInfo.RHColor.clIndicator[0]);
    }

    public override void Calculate()
    {
      this.m_data = new float[1][];
      if (this.m_kData == null || this.m_kData.Length <= 0)
        return;
      int n = this.m_iParam[0];
      if (n > this.m_kData.Length || n < 1)
        return;
      this.m_data[0] = new float[this.m_kData.Length];
      float[] numArray = this.m_data[0];
      for (int index = 0; index < this.m_kData.Length; ++index)
      {
        float num = this.m_kData[index].highPrice - this.m_kData[index].lowPrice;
        numArray[index] = (double) num <= 0.0 ? (float) this.m_kData[index].totalAmount / 1000f : (float) (((double) this.m_kData[index].closePrice - (double) this.m_kData[index].openPrice) / (double) num * (double) this.m_kData[index].totalAmount / 1000.0);
      }
      IndicatorBase.Average(0, this.m_kData.Length, n, this.m_data[0], this.m_data[0]);
      for (int index = 0; index < this.m_kData.Length; ++index)
        this.m_data[0][index] *= (float) n;
    }
  }
}
