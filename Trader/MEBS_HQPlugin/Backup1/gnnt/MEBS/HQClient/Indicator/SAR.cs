// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.SAR
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
  internal class SAR : KLine
  {
    private readonly int[] m_iParam = new int[1]
    {
      5
    };
    private readonly int SAR_DOWN = 1;
    private readonly int SAR_CUP = 16;
    private readonly int SAR_CDOWN = 17;
    private readonly int SAR_UP;

    public SAR(IndicatorPos pos, int iPrecision, HQForm hqForm)
      : base(pos, 0, iPrecision, hqForm)
    {
      this.m_strIndicatorName = "SAR(" + (object) this.m_iParam[0] + ")";
      this.m_strParamName = new string[1]
      {
        ""
      };
    }

    public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
    {
      this.m_kData = data;
      this.Calculate();
      base.Paint(g, rc, data);
      Color[] color = new Color[3]
      {
        SetInfo.RHColor.clIncrease,
        SetInfo.RHColor.clDecrease,
        SetInfo.RHColor.clEqual
      };
      this.DrawSAR(g, this.m_iParam[0] - 1, this.m_data[0], this.m_data[1], color);
    }

    public override void Calculate()
    {
      int num1 = this.m_iParam[0];
      this.m_data = new float[2][];
      if (this.m_kData == null || this.m_kData.Length <= 0 || (num1 > this.m_kData.Length || num1 < 3))
        return;
      for (int index = 0; index < 2; ++index)
        this.m_data[index] = new float[this.m_kData.Length];
      float[] numArray1 = this.m_data[0];
      float[] numArray2 = this.m_data[1];
      float num2 = 0.02f;
      numArray2[num1 - 1] = (double) this.m_kData[num1 - 1].closePrice >= (double) this.m_kData[num1 - 2].closePrice ? ((double) this.m_kData[num1 - 1].closePrice <= (double) this.m_kData[num1 - 2].closePrice ? ((double) this.m_kData[num1 - 2].closePrice >= (double) this.m_kData[num1 - 3].closePrice ? ((double) this.m_kData[num1 - 2].closePrice <= (double) this.m_kData[num1 - 3].closePrice ? (float) this.SAR_CUP : (float) this.SAR_UP) : (float) this.SAR_DOWN) : ((double) this.m_kData[num1 - 2].closePrice < (double) this.m_kData[num1 - 3].closePrice ? (float) this.SAR_CUP : (float) this.SAR_UP)) : ((double) this.m_kData[num1 - 2].closePrice > (double) this.m_kData[num1 - 3].closePrice ? (float) this.SAR_CDOWN : (float) this.SAR_DOWN);
      if ((double) numArray2[num1 - 1] == (double) this.SAR_DOWN || (double) numArray2[num1 - 1] == (double) this.SAR_CDOWN)
      {
        numArray1[num1 - 1] = -1E+36f;
        for (int index = num1 - 1; index >= 0; --index)
          numArray1[num1 - 1] = Math.Max(numArray1[num1 - 1], this.m_kData[index].highPrice);
      }
      else
      {
        numArray1[num1 - 1] = 1E+36f;
        for (int index = num1 - 1; index >= 0; --index)
          numArray1[num1 - 1] = Math.Min(numArray1[num1 - 1], this.m_kData[index].lowPrice);
      }
      for (int index1 = num1; index1 < this.m_kData.Length; ++index1)
      {
        if ((double) numArray2[index1 - 1] == (double) this.SAR_UP || (double) numArray2[index1 - 1] == (double) this.SAR_CUP)
        {
          if ((double) this.m_kData[index1].closePrice < (double) numArray1[index1 - 1])
          {
            numArray1[index1] = -1E+36f;
            for (int index2 = index1; index2 > index1 - num1; --index2)
              numArray1[index1] = Math.Max(numArray1[index1], this.m_kData[index2].highPrice);
            numArray2[index1] = (float) this.SAR_CDOWN;
            num2 = 0.02f;
          }
          else
          {
            numArray1[index1] = numArray1[index1 - 1] + num2 * (this.m_kData[index1 - 1].highPrice - numArray1[index1 - 1]);
            num2 = (double) num2 < 0.200000002980232 ? num2 + 0.02f : num2;
            numArray2[index1] = (float) this.SAR_UP;
          }
        }
        else if ((double) this.m_kData[index1].closePrice > (double) numArray1[index1 - 1])
        {
          numArray1[index1] = 1E+36f;
          for (int index2 = index1; index2 > index1 - num1; --index2)
            numArray1[index1] = Math.Min(numArray1[index1], this.m_kData[index2].lowPrice);
          numArray2[index1] = (float) this.SAR_CUP;
          num2 = 0.02f;
        }
        else
        {
          numArray1[index1] = numArray1[index1 - 1] + num2 * (this.m_kData[index1 - 1].lowPrice - numArray1[index1 - 1]);
          num2 = (double) num2 < 0.200000002980232 ? num2 + 0.02f : num2;
          numArray2[index1] = (float) this.SAR_DOWN;
        }
      }
    }

    protected override void GetMaxMin()
    {
      base.GetMaxMin();
      this.GetValueMaxMin(this.m_data[0], this.m_iParam[0]);
      if (this.m_rc.Height <= this.m_iTextH)
        return;
      float num = this.m_max - this.m_min;
      this.m_max += (float) ((double) this.m_pos.m_Ratio / 2.0 * (double) num / (double) (this.m_rc.Height - this.m_iTextH));
      this.m_min -= (float) ((double) this.m_pos.m_Ratio / 2.0 * (double) num / (double) (this.m_rc.Height - this.m_iTextH));
    }

    private void DrawSAR(Graphics g, int iBegin, float[] data, float[] sign, Color[] color)
    {
      if (data == null || sign == null)
        return;
      Rectangle rectangle = new Rectangle(this.m_rc.X, this.m_rc.Y + this.m_iTextH, this.m_rc.Width, this.m_rc.Height - this.m_iTextH);
      if ((double) this.m_max - (double) this.m_min == 0.0 || rectangle.Height <= 0)
        return;
      int num1 = this.m_pos.m_Begin < iBegin ? iBegin : this.m_pos.m_Begin;
      float num2 = (float) rectangle.X + (float) (num1 - this.m_pos.m_Begin) * this.m_pos.m_Ratio;
      float num3 = (this.m_max - this.m_min) / (float) rectangle.Height;
      for (int index = num1; index <= this.m_pos.m_End; ++index)
      {
        float num4 = (float) rectangle.Y + (this.m_max - data[index]) / num3;
        if ((double) sign[index] == (double) this.SAR_DOWN)
          this.pen.Color = color[1];
        else if ((double) sign[index] == (double) this.SAR_UP)
          this.pen.Color = color[0];
        else
          this.pen.Color = color[2];
        int x = (int) num2;
        int y = (int) ((double) num4 - (double) this.m_pos.m_Ratio / 2.0);
        int num5 = (int) ((double) num2 + (double) this.m_pos.m_Ratio);
        int num6 = (int) ((double) num4 + (double) this.m_pos.m_Ratio / 2.0);
        if (num5 > x && num6 > y)
          g.DrawArc(this.pen, x, y, num5 - x, num6 - y, 0, 360);
        num2 += this.m_pos.m_Ratio;
      }
    }
  }
}
