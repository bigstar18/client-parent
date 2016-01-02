// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.EMV
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System.Drawing;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
  internal class EMV : IndicatorBase
  {
    private readonly int[] m_iParam = new int[2]
    {
      14,
      9
    };

    public EMV(IndicatorPos pos, int Precision)
      : base(pos, Precision)
    {
      this.m_strIndicatorName = "EMV";
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
        "",
        "EMVMA"
      };
      this.m_iPrecision = 2;
    }

    public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
    {
      if (data == null || data.Length == 0)
        return;
      base.Paint(g, rc, data);
      this.Calculate();
      this.m_max = -10000f;
      this.m_min = 10000f;
      this.GetValueMaxMin(this.m_data[0], this.m_iParam[0]);
      if (this.m_iParam[1] > 0 && this.m_iParam[1] <= this.m_kData.Length)
        this.GetValueMaxMin(this.m_data[1], this.m_iParam[0] + this.m_iParam[1] - 1);
      this.DrawCoordinate(g, 2);
      this.DrawLine(g, this.m_data[0], this.m_iParam[0], SetInfo.RHColor.clIndicator[0]);
      if (this.m_iParam[1] <= 0 || this.m_iParam[1] > this.m_kData.Length)
        return;
      this.DrawLine(g, this.m_data[1], this.m_iParam[0] + this.m_iParam[1] - 1, SetInfo.RHColor.clIndicator[1]);
    }

    public override void Calculate()
    {
      this.m_data = new float[2][];
      int begin = this.m_iParam[0];
      int n = this.m_iParam[1];
      if (this.m_kData == null || begin > this.m_kData.Length || begin < 1)
        return;
      this.m_data[0] = new float[this.m_kData.Length];
      this.m_data[1] = new float[this.m_kData.Length];
      float[] source = this.m_data[0];
      float[] destination = this.m_data[1];
      source[begin - 1] = 0.0f;
      for (int index = begin; index < this.m_kData.Length; ++index)
      {
        source[index] = 0.0f;
        if (this.m_kData[index].totalAmount > 0L)
          source[index] = (float) (((double) this.m_kData[index].highPrice + (double) this.m_kData[index].lowPrice - (double) this.m_kData[index - begin].highPrice - (double) this.m_kData[index - begin].lowPrice) / 2.0 * ((double) this.m_kData[index].highPrice - (double) this.m_kData[index].lowPrice));
      }
      if (n > this.m_kData.Length || n <= 0)
        return;
      IndicatorBase.Average(begin, this.m_kData.Length, n, source, destination);
    }
  }
}
