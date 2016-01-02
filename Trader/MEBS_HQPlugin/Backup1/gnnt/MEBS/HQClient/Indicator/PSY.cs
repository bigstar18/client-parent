// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.PSY
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System.Drawing;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
  internal class PSY : IndicatorBase
  {
    private readonly int[] m_iParam = new int[2]
    {
      12,
      24
    };

    public PSY(IndicatorPos pos, int Precision)
      : base(pos, Precision)
    {
      this.m_strIndicatorName = "PSY";
      this.m_strParamName = new string[this.m_iParam.Length];
      for (int index = 0; index < this.m_iParam.Length; ++index)
        this.m_strParamName[index] = "PSY" + (object) this.m_iParam[index];
      this.m_iPrecision = 2;
    }

    public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
    {
      base.Paint(g, rc, data);
      this.Calculate();
      this.m_max = -10000f;
      this.m_min = 10000f;
      for (int index = 0; index < this.m_iParam.Length; ++index)
        this.GetValueMaxMin(this.m_data[index], this.m_iParam[index]);
      this.DrawCoordinate(g, 2);
      for (int index = 0; index < this.m_iParam.Length; ++index)
        this.DrawLine(g, this.m_data[index], this.m_iParam[index], SetInfo.RHColor.clIndicator[index]);
    }

    public override void Calculate()
    {
      this.m_data = new float[this.m_iParam.Length][];
      if (this.m_kData == null || this.m_kData.Length == 0)
        return;
      for (int index = 0; index < this.m_iParam.Length; ++index)
      {
        if (this.m_iParam[index] <= this.m_kData.Length && this.m_iParam[index] > 0)
        {
          this.m_data[index] = new float[this.m_kData.Length];
          this.GetPSY(this.m_iParam[index], this.m_data[index]);
        }
      }
    }

    private void GetPSY(int n, float[] psy)
    {
      if (psy == null)
        return;
      double num = 0.0;
      for (int index = 1; index < n; ++index)
      {
        if ((double) this.m_kData[index].closePrice > (double) this.m_kData[index - 1].closePrice)
          ++num;
      }
      for (int index1 = n; index1 < this.m_kData.Length; ++index1)
      {
        if ((double) this.m_kData[index1].closePrice > (double) this.m_kData[index1 - 1].closePrice)
          ++num;
        psy[index1] = (float) (num / (double) n * 100.0);
        int index2 = index1 - n + 1;
        if ((double) this.m_kData[index2].closePrice > (double) this.m_kData[index2 - 1].closePrice)
          --num;
      }
    }
  }
}
