// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.BOLL
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
  internal class BOLL : KLine
  {
    private readonly int[] m_iParam = new int[1]
    {
      10
    };

    public BOLL(IndicatorPos pos, int iPrecision, HQForm hqForm)
      : base(pos, 1, iPrecision, hqForm)
    {
      this.m_strIndicatorName = "BOLL";
      this.m_strIndicatorName += "(";
      for (int index = 0; index < this.m_iParam.Length; ++index)
      {
        if (index > 0)
          this.m_strIndicatorName += ",";
        this.m_strIndicatorName += (string) (object) this.m_iParam[index];
      }
      this.m_strIndicatorName += ")";
      this.m_strParamName = new string[3]
      {
        "MID",
        "UPPER",
        "LOWER"
      };
      this.m_iPrecision = 2;
    }

    public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
    {
      this.m_kData = data;
      this.Calculate();
      base.Paint(g, rc, data);
      for (int index = 0; index < 3; ++index)
        this.DrawLine(g, this.m_data[index], this.m_iParam[0] * 2 - 2, SetInfo.RHColor.clIndicator[index]);
    }

    protected override void GetMaxMin()
    {
      base.GetMaxMin();
      for (int index = 0; index < 3; ++index)
        this.GetValueMaxMin(this.m_data[index], this.m_iParam[0] * 2 - 2);
    }

    public override void Calculate()
    {
      this.m_data = new float[3][];
      if (this.m_kData == null || this.m_kData.Length <= 0)
        return;
      int iParam = this.m_iParam[0];
      if (iParam > this.m_kData.Length || iParam < 1 || iParam + iParam - 2 >= this.m_kData.Length)
        return;
      for (int index = 0; index < 3; ++index)
        this.m_data[index] = new float[this.m_kData.Length];
      float[] data = this.m_data[0];
      float[] numArray1 = this.m_data[1];
      float[] numArray2 = this.m_data[2];
      float num1 = 0.0f;
      this.AverageClose(iParam, data);
      for (int index = iParam - 1; index < iParam + iParam - 2; ++index)
      {
        float num2 = this.m_kData[index].closePrice - data[index];
        num1 += num2 * num2;
      }
      float num3 = 0.0f;
      for (int index = iParam + iParam - 2; index < this.m_kData.Length; ++index)
      {
        float num2 = num1 - num3;
        float num4 = this.m_kData[index].closePrice - data[index];
        num1 = num2 + num4 * num4;
        float num5 = (float) Math.Sqrt((double) num1 / (double) iParam) * 1.805f;
        numArray1[index] = data[index] + num5;
        numArray2[index] = data[index] - num5;
        float num6 = this.m_kData[index - iParam + 1].closePrice - data[index - iParam + 1];
        num3 = num6 * num6;
      }
    }
  }
}
