// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.Reserve
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System.Drawing;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
  internal class Reserve : IndicatorBase
  {
    private readonly int[] m_iParam = new int[2]
    {
      5,
      10
    };

    public Reserve(IndicatorPos pos, int Precision)
      : base(pos, Precision)
    {
      this.m_strIndicatorName = "ORDER";
      this.m_iPrecision = 0;
      this.m_strParamName = new string[this.m_iParam.Length];
      for (int index = 0; index < this.m_iParam.Length; ++index)
        this.m_strParamName[index] = "MA" + (object) this.m_iParam[index];
    }

    public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
    {
      base.Paint(g, rc, data);
      this.Calculate();
      this.GetMaxMin();
      for (int index = 0; index < this.m_iParam.Length; ++index)
        this.GetValueMaxMin(this.m_data[index], this.m_iParam[index] - 1);
      this.DrawCoordinate(g, 0);
      this.DrawVolume(g);
      for (int index = 0; index < this.m_iParam.Length; ++index)
        this.DrawLine(g, this.m_data[index], this.m_iParam[index] - 1, SetInfo.RHColor.clIndicator[index]);
    }

    public override void Calculate()
    {
      this.m_data = new float[this.m_iParam.Length][];
      for (int iIndex = 0; iIndex < this.m_iParam.Length; ++iIndex)
        this.AverageVolume(iIndex);
      this.m_data = new float[this.m_iParam.Length][];
      for (int iIndex = 0; iIndex < this.m_iParam.Length; ++iIndex)
        this.AverageVolume(iIndex);
    }

    private void GetMaxMin()
    {
      this.m_max = 0.0f;
      this.m_min = 0.0f;
      for (int index = this.m_pos.m_Begin; index <= this.m_pos.m_End; ++index)
      {
        if ((double) this.m_kData[index].reserveCount > (double) this.m_max)
          this.m_max = (float) this.m_kData[index].reserveCount;
      }
    }

    private void DrawVolume(Graphics g)
    {
      if ((double) this.m_max - (double) this.m_min == 0.0 || this.m_rc.Height - this.m_iTextH <= 0)
        return;
      int num1 = (double) this.m_pos.m_Ratio < 3.0 ? 0 : (int) (((double) this.m_pos.m_Ratio + 1.0) / 3.0);
      if (num1 % 2 == 0 && num1 > 0)
        --num1;
      float num2 = (float) this.m_rc.X + this.m_pos.m_Ratio / 2f;
      float num3 = (this.m_max - this.m_min) / (float) (this.m_rc.Height - this.m_iTextH);
      for (int index = this.m_pos.m_Begin; index <= this.m_pos.m_End; ++index)
      {
        int y = this.m_rc.Y + this.m_iTextH + (int) (((double) this.m_max - (double) this.m_kData[index].reserveCount) / (double) num3);
        if ((double) this.m_kData[index].openPrice > (double) this.m_kData[index].closePrice)
        {
          this.m_Brush.Color = SetInfo.RHColor.clKLineDown;
          g.FillRectangle((Brush) this.m_Brush, (int) num2 - num1, y, 2 * num1 + 1, this.m_rc.Y + this.m_rc.Height - y - 1);
        }
        else if ((double) this.m_kData[index].openPrice < (double) this.m_kData[index].closePrice)
        {
          this.pen.Color = SetInfo.RHColor.clKLineUp;
          g.DrawRectangle(this.pen, (int) num2 - num1, y, 2 * num1 + 1, this.m_rc.Y + this.m_rc.Height - y - 1);
        }
        else
        {
          this.pen.Color = SetInfo.RHColor.clKLineEqual;
          g.DrawRectangle(this.pen, (int) num2 - num1, y, 2 * num1 + 1, this.m_rc.Y + this.m_rc.Height - y - 1);
        }
        num2 += this.m_pos.m_Ratio;
      }
    }

    private void AverageVolume(int iIndex)
    {
      if (this.m_kData == null || this.m_kData.Length == 0)
        return;
      int num1 = this.m_iParam[iIndex];
      if (num1 > this.m_kData.Length || num1 < 1)
        return;
      this.m_data[iIndex] = new float[this.m_kData.Length];
      float[] numArray = this.m_data[iIndex];
      float num2 = 0.0f;
      double num3 = 0.0;
      for (int index = 0; index < num1 - 1; ++index)
        num3 += (double) this.m_kData[index].reserveCount;
      for (int index = num1 - 1; index < this.m_kData.Length; ++index)
      {
        num3 = num3 - (double) num2 + (double) this.m_kData[index].reserveCount;
        numArray[index] = (float) num3 / (float) num1;
        num2 = (float) this.m_kData[index - num1 + 1].reserveCount;
      }
    }
  }
}
