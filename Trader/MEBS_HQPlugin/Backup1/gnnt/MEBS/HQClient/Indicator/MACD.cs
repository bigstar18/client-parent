// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.MACD
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System.Drawing;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
  internal class MACD : IndicatorBase
  {
    private readonly int[] m_iParam = new int[3]
    {
      12,
      26,
      9
    };

    public MACD(IndicatorPos pos, int Precision)
      : base(pos, Precision)
    {
      this.m_strIndicatorName = "MACD(" + (object) this.m_iParam[0] + "," + (string) (object) this.m_iParam[1] + "," + (string) (object) this.m_iParam[2] + ")";
      this.m_strParamName = new string[3]
      {
        "DIF",
        "DEA",
        "MACD"
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
      this.GetValueMaxMin(this.m_data[0], 0);
      this.GetValueMaxMin(this.m_data[1], 0);
      this.GetValueMaxMin(this.m_data[2], 0);
      this.DrawCoordinate(g, 2);
      this.DrawLine(g, this.m_data[0], 0, SetInfo.RHColor.clIndicator[0]);
      this.DrawLine(g, this.m_data[1], 0, SetInfo.RHColor.clIndicator[1]);
      this.DrawVertLine(g, this.m_data[2], 0, SetInfo.RHColor.clIncrease, SetInfo.RHColor.clDecrease);
    }

    public override void Calculate()
    {
      this.m_data = new float[3][];
      this.m_data[0] = new float[this.m_kData.Length];
      this.m_data[1] = new float[this.m_kData.Length];
      this.m_data[2] = new float[this.m_kData.Length];
      float[] numArray1 = this.m_data[0];
      float[] numArray2 = this.m_data[1];
      float[] numArray3 = this.m_data[2];
      float num1 = 0.0f;
      float num2 = 0.0f;
      float[] numArray4 = new float[3];
      float[] numArray5 = new float[3];
      int[] numArray6 = new int[3];
      for (int index = 0; index < 3; ++index)
      {
        numArray6[index] = this.m_iParam[index];
        numArray4[index] = 2f / (float) (numArray6[index] + 1);
        numArray5[index] = 0.0f;
      }
      for (int index = 0; index < this.m_kData.Length; ++index)
      {
        float num3 = this.m_kData[index].closePrice;
        if (index == 0)
        {
          num1 = num3;
          num2 = num3;
        }
        num1 = (num3 - num1) * numArray4[0] + num1;
        num2 = (num3 - num2) * numArray4[1] + num2;
        numArray1[index] = index < numArray6[0] - 1 || index < numArray6[1] ? num1 - num2 : num1 - num2;
        numArray2[index] = index != 0 ? (float) (((double) numArray1[index] - (double) numArray2[index - 1]) * 0.2) + numArray2[index - 1] : numArray1[index];
        numArray3[index] = numArray1[index] - numArray2[index];
      }
    }

    private void DrawVertLine(Graphics g, float[] data, int iFirst, Color color1, Color color2)
    {
      if (data == null || iFirst > this.m_kData.Length || ((double) this.m_max - (double) this.m_min <= 0.0 || this.m_rc.Height - this.m_iTextH <= 0))
        return;
      int num1 = this.m_pos.m_Begin <= iFirst ? iFirst : this.m_pos.m_Begin;
      int num2 = this.m_pos.m_End;
      if (num1 > num2)
        return;
      float num3 = (float) ((double) this.m_rc.X + (double) this.m_pos.m_Ratio / 2.0 + (double) (num1 - this.m_pos.m_Begin) * (double) this.m_pos.m_Ratio);
      float num4 = (this.m_max - this.m_min) / (float) (this.m_rc.Height - this.m_iTextH);
      int y1 = this.m_rc.Y + this.m_iTextH + (int) (((double) this.m_max - 0.0) / (double) num4);
      this.pen.Color = color1;
      for (int index = num1; index <= num2; ++index)
      {
        if ((double) data[index] > 0.0)
          this.pen.Color = color1;
        else
          this.pen.Color = color2;
        g.DrawLine(this.pen, (int) num3, y1, (int) num3, this.m_rc.Y + this.m_iTextH + (int) (((double) this.m_max - (double) data[index]) / (double) num4));
        num3 += this.m_pos.m_Ratio;
      }
    }
  }
}
