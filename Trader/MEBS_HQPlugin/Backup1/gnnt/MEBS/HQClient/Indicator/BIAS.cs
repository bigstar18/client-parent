// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.BIAS
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System.Drawing;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
  internal class BIAS : IndicatorBase
  {
    private readonly int[] m_iParam = new int[2]
    {
      6,
      12
    };

    public BIAS(IndicatorPos pos, int iPrecision)
      : base(pos, iPrecision)
    {
      this.m_strIndicatorName = "BIAS";
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
        this.m_strParamName[index] = "BIAS" + (object) (index + 1);
      this.m_iPrecision = 2;
    }

    public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
    {
      base.Paint(g, rc, data);
      this.Calculate();
      this.m_max = -10000f;
      this.m_min = 10000f;
      for (int index = 0; index < this.m_iParam.Length; ++index)
        this.GetValueMaxMin(this.m_data[index], this.m_iParam[index] - 1);
      this.DrawCoordinate(g, 2);
      for (int index = 0; index < this.m_iParam.Length; ++index)
        this.DrawLine(g, this.m_data[index], this.m_iParam[index] - 1, SetInfo.RHColor.clIndicator[index]);
    }

    public override void Calculate()
    {
      this.m_data = new float[this.m_iParam.Length][];
      if (this.m_kData == null || this.m_kData.Length <= 0)
        return;
      for (int index = 0; index < 2; ++index)
      {
        if (this.m_iParam[index] <= this.m_kData.Length && this.m_iParam[index] > 0)
        {
          this.m_data[index] = new float[this.m_kData.Length];
          this.GetBIAS(this.m_iParam[index], this.m_data[index]);
        }
      }
    }

    private void GetBIAS(int n, float[] bias)
    {
      if (bias == null)
        return;
      this.AverageClose(n, bias);
      bias[n - 2] = 0.0f;
      for (int index = n - 1; index < this.m_kData.Length; ++index)
        bias[index] = (double) bias[index] == 0.0 ? bias[index - 1] : (float) (((double) this.m_kData[index].closePrice - (double) bias[index]) / (double) bias[index] * 100.0);
    }
  }
}
