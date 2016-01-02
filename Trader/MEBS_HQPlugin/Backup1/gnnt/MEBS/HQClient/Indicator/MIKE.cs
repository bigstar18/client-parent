// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.MIKE
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System.Drawing;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
  internal class MIKE : KLine
  {
    private readonly int[] m_iParam = new int[1]
    {
      12
    };

    public MIKE(IndicatorPos pos, int iPrecision, HQForm hqForm)
      : base(pos, 1, iPrecision, hqForm)
    {
      this.m_strIndicatorName = "MIKE";
      this.m_strIndicatorName += "(";
      for (int index = 0; index < this.m_iParam.Length; ++index)
      {
        if (index > 0)
          this.m_strIndicatorName += ",";
        this.m_strIndicatorName += (string) (object) this.m_iParam[index];
      }
      this.m_strIndicatorName += ")";
      this.m_strParamName = new string[6]
      {
        "WR",
        "MR",
        "SR",
        "WS",
        "MS",
        "SS"
      };
      this.m_iPrecision = 2;
    }

    public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
    {
      this.m_kData = data;
      this.Calculate();
      base.Paint(g, rc, data);
      for (int index = 0; index < 6; ++index)
        this.DrawLine(g, this.m_data[index], this.m_iParam[0], SetInfo.RHColor.clIndicator[index / 1]);
    }

    protected override void GetMaxMin()
    {
      base.GetMaxMin();
      for (int index = 0; index < 6; ++index)
        this.GetValueMaxMin(this.m_data[index], this.m_iParam[0]);
    }

    public override void Calculate()
    {
      this.m_data = new float[6][];
      if (this.m_kData == null || this.m_kData.Length <= 0)
        return;
      int iParam = this.m_iParam[0];
      if (iParam > this.m_kData.Length || iParam < 1)
        return;
      for (int index = 0; index < 6; ++index)
        this.m_data[index] = new float[this.m_kData.Length];
      this.getN_DayLow(iParam, this.m_data[0]);
      this.getN_DayHigh(iParam, this.m_data[1]);
      for (int index = iParam - 1; index < this.m_kData.Length; ++index)
      {
        float num1 = this.m_kData[index].closePrice;
        float num2 = this.m_data[0][index];
        float num3 = this.m_data[1][index];
        float num4 = (float) (((double) num1 + (double) num3 + (double) num2) / 3.0);
        this.m_data[0][index] = num4 + (num4 - num2);
        this.m_data[1][index] = num4 + (num3 - num2);
        this.m_data[2][index] = 2f * num3 - num2;
        this.m_data[3][index] = num4 - (num3 - num4);
        this.m_data[4][index] = num4 - (num3 - num2);
        this.m_data[5][index] = 2f * num2 - num3;
      }
    }

    private void getN_DayLow(int iParam, float[] data)
    {
      if (this.m_kData == null || this.m_kData.Length == 0)
        return;
      int num1 = iParam;
      if (num1 > this.m_kData.Length || num1 < 1)
        return;
      for (int index1 = num1 - 1; index1 < this.m_kData.Length; ++index1)
      {
        double num2 = (double) this.m_kData[index1 - num1 + 1].lowPrice;
        for (int index2 = index1 - num1 + 2; index2 <= index1; ++index2)
        {
          if (num2 > (double) this.m_kData[index2].lowPrice)
            num2 = (double) this.m_kData[index2].lowPrice;
        }
        data[index1] = (float) num2;
      }
    }

    private void getN_DayHigh(int iParam, float[] data)
    {
      if (this.m_kData == null || this.m_kData.Length == 0)
        return;
      int num1 = iParam;
      if (num1 > this.m_kData.Length || num1 < 1)
        return;
      for (int index1 = num1 - 1; index1 < this.m_kData.Length; ++index1)
      {
        double num2 = (double) this.m_kData[index1 - num1 + 1].highPrice;
        for (int index2 = index1 - num1 + 2; index2 <= index1; ++index2)
        {
          if (num2 < (double) this.m_kData[index2].lowPrice)
            num2 = (double) this.m_kData[index2].lowPrice;
        }
        data[index1] = (float) num2;
      }
    }
  }
}
