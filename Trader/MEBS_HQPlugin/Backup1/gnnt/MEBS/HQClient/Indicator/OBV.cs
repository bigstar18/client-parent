// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.OBV
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System.Drawing;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
  internal class OBV : IndicatorBase
  {
    private readonly int[] m_iParam = new int[1]
    {
      12
    };

    public OBV(IndicatorPos pos, int Precision)
      : base(pos, Precision)
    {
      this.m_strIndicatorName = "OBV";
      this.m_strParamName = new string[2]
      {
        "",
        "MA" + (object) this.m_iParam[0]
      };
      this.m_iPrecision = 2;
    }

    public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
    {
      base.Paint(g, rc, data);
      this.Calculate();
      this.m_max = -1E+38f;
      this.m_min = 1E+38f;
      this.GetValueMaxMin(this.m_data[0], 0);
      this.DrawCoordinate(g, 2);
      this.DrawLine(g, this.m_data[0], 0, SetInfo.RHColor.clIndicator[0]);
      this.DrawLine(g, this.m_data[1], this.m_iParam[0], SetInfo.RHColor.clIndicator[1]);
    }

    public override void Calculate()
    {
      this.m_data = new float[2][];
      if (this.m_kData == null || this.m_kData.Length <= 0)
        return;
      int n = this.m_iParam[0];
      this.m_data[0] = new float[this.m_kData.Length];
      this.m_data[1] = new float[this.m_kData.Length];
      float[] source = this.m_data[0];
      source[0] = 0.0f;
      for (int index = 1; index < this.m_kData.Length; ++index)
        source[index] = (double) this.m_kData[index].closePrice <= (double) this.m_kData[index - 1].closePrice ? ((double) this.m_kData[index].closePrice >= (double) this.m_kData[index - 1].closePrice ? source[index - 1] : source[index - 1] - (float) (this.m_kData[index].totalAmount / 1000L)) : source[index - 1] + (float) (this.m_kData[index].totalAmount / 1000L);
      IndicatorBase.Average(1, this.m_kData.Length, n, source, this.m_data[1]);
    }
  }
}
