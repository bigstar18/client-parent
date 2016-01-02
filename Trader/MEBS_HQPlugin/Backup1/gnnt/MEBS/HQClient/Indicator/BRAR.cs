// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.BRAR
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System.Drawing;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
  internal class BRAR : IndicatorBase
  {
    private readonly int[] m_iParam = new int[1]
    {
      26
    };

    public BRAR(IndicatorPos pos, int Precision)
      : base(pos, Precision)
    {
      this.m_strIndicatorName = "BRAR";
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
        "AR",
        "BR"
      };
      this.m_iPrecision = 2;
    }

    public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
    {
      base.Paint(g, rc, data);
      this.Calculate();
      this.m_max = -10000f;
      this.m_min = 10000f;
      this.GetValueMaxMin(this.m_data[0], this.m_iParam[0] + 1);
      this.GetValueMaxMin(this.m_data[1], this.m_iParam[0] + 1);
      this.DrawCoordinate(g, 2);
      this.DrawLine(g, this.m_data[0], this.m_iParam[0] + 1, SetInfo.RHColor.clIndicator[0]);
      this.DrawLine(g, this.m_data[1], this.m_iParam[0] + 1, SetInfo.RHColor.clIndicator[1]);
    }

    public override void Calculate()
    {
      this.m_data = new float[2][];
      if (this.m_kData == null || this.m_kData.Length <= 0)
        return;
      this.m_data[0] = new float[this.m_kData.Length];
      this.m_data[1] = new float[this.m_kData.Length];
      this.GetAR(this.m_iParam[0], this.m_data[0]);
      this.GetBR(this.m_iParam[0], this.m_data[1]);
    }

    private void GetAR(int n, float[] ar)
    {
      if (this.m_kData.Length < n)
        return;
      float num1;
      float num2 = num1 = 0.0f;
      for (int index = 1; index < n; ++index)
      {
        num2 += this.m_kData[index].highPrice - this.m_kData[index].openPrice;
        num1 += this.m_kData[index].openPrice - this.m_kData[index].lowPrice;
      }
      float num3 = 0.0f;
      for (int index1 = n; index1 < this.m_kData.Length; ++index1)
      {
        float num4 = num2 + (this.m_kData[index1].highPrice - this.m_kData[index1].openPrice);
        float num5 = num1 + (this.m_kData[index1].openPrice - this.m_kData[index1].lowPrice);
        ar[index1] = num3;
        if ((double) num5 != 0.0)
          ar[index1] = (float) ((double) num4 / (double) num5 * 100.0);
        num3 = ar[index1];
        int index2 = index1 - n + 1;
        num2 = num4 - (this.m_kData[index2].highPrice - this.m_kData[index2].openPrice);
        num1 = num5 - (this.m_kData[index2].openPrice - this.m_kData[index2].lowPrice);
      }
    }

    private void GetBR(int n, float[] br)
    {
      if (this.m_kData.Length < n)
        return;
      float num1;
      float num2 = num1 = 0.0f;
      for (int index = 1; index < n; ++index)
      {
        float num3 = this.m_kData[index].highPrice - this.m_kData[index - 1].closePrice;
        num2 += (double) num3 <= 0.0 ? 0.0f : num3;
        float num4 = this.m_kData[index - 1].closePrice - this.m_kData[index].lowPrice;
        num1 += (double) num4 <= 0.0 ? 0.0f : num4;
      }
      float num5 = 0.0f;
      for (int index1 = n; index1 < this.m_kData.Length; ++index1)
      {
        float num3 = this.m_kData[index1].highPrice - this.m_kData[index1 - 1].closePrice;
        float num4 = num2 + ((double) num3 <= 0.0 ? 0.0f : num3);
        float num6 = this.m_kData[index1 - 1].closePrice - this.m_kData[index1].lowPrice;
        float num7 = num1 + ((double) num6 <= 0.0 ? 0.0f : num6);
        br[index1] = num5;
        if ((double) num7 != 0.0)
          br[index1] = (float) ((double) num4 / (double) num7 * 100.0);
        num5 = br[index1];
        int index2 = index1 - n + 1;
        float num8 = this.m_kData[index2].highPrice - this.m_kData[index2 - 1].closePrice;
        num2 = num4 - ((double) num8 <= 0.0 ? 0.0f : num8);
        float num9 = this.m_kData[index2 - 1].closePrice - this.m_kData[index2].lowPrice;
        num1 = num7 - ((double) num9 <= 0.0 ? 0.0f : num9);
      }
    }
  }
}
