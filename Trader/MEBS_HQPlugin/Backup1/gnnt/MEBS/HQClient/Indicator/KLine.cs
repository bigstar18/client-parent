// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.KLine
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQClient.gnnt.util;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System.Drawing;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
  internal class KLine : IndicatorBase
  {
    public const int LineType_K = 0;
    public const int LineType_USA = 1;
    public const int LineType_POLY = 2;
    private int m_iLineType;
    private HQForm m_hqForm;

    public KLine(IndicatorPos pos, int iLineType, int Precision, HQForm hqForm)
      : base(pos, Precision)
    {
      this.m_strIndicatorName = "KLine";
      this.m_iLineType = iLineType;
      this.m_hqForm = hqForm;
    }

    public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
    {
      base.Paint(g, rc, data);
      this.GetMaxMin();
      this.DrawCoordinate(g, this.m_iPrecision);
      switch (this.m_iLineType)
      {
        case 1:
          this.drawUSA(g);
          break;
        case 2:
          this.DrawPolyLine(g);
          break;
        default:
          this.DrawKLine(g);
          break;
      }
    }

    public override void Calculate()
    {
    }

    protected virtual void GetMaxMin()
    {
      if (this.m_pos.m_Begin > this.m_pos.m_End)
      {
        this.m_max = 0.0f;
        this.m_min = 0.0f;
      }
      else
      {
        this.m_max = 0.0f;
        this.m_min = 1E+08f;
        for (int index = this.m_pos.m_Begin; index <= this.m_pos.m_End; ++index)
        {
          if (this.m_iLineType == 0 || this.m_iLineType == 1)
          {
            if ((double) this.m_kData[index].highPrice > 0.0)
            {
              if ((double) this.m_kData[index].highPrice > (double) this.m_max)
                this.m_max = this.m_kData[index].highPrice;
              if ((double) this.m_kData[index].lowPrice < (double) this.m_min)
                this.m_min = this.m_kData[index].lowPrice;
            }
          }
          else if ((double) this.m_kData[index].closePrice > 0.0)
          {
            if ((double) this.m_kData[index].closePrice > (double) this.m_max)
              this.m_max = this.m_kData[index].closePrice;
            if ((double) this.m_kData[index].closePrice < (double) this.m_min)
              this.m_min = this.m_kData[index].closePrice;
          }
        }
      }
    }

    private void DrawKLine(Graphics g)
    {
      int num1 = this.m_pos.m_Begin;
      int num2 = this.m_pos.m_End;
      if ((double) this.m_max - (double) this.m_min == 0.0 || this.m_rc.Height - this.m_iTextH <= 0)
        return;
      int num3 = (double) this.m_pos.m_Ratio < 3.0 ? 0 : (int) (((double) this.m_pos.m_Ratio + 1.0) / 3.0);
      if (num3 % 2 == 0 && num3 > 0)
        --num3;
      float num4 = (float) this.m_rc.X + this.m_pos.m_Ratio / 2f;
      float num5 = (this.m_max - this.m_min) / (float) (this.m_rc.Height - this.m_iTextH);
      int num6 = 0;
      int num7 = 0;
      for (int index = num1; index <= num2; ++index)
      {
        int num8 = this.m_rc.Y + this.m_iTextH + (int) (((double) this.m_max - (double) this.m_kData[index].openPrice) / (double) num5);
        int y1 = this.m_rc.Y + this.m_iTextH + (int) (((double) this.m_max - (double) this.m_kData[index].highPrice) / (double) num5);
        int num9 = this.m_rc.Y + this.m_iTextH + (int) (((double) this.m_max - (double) this.m_kData[index].lowPrice) / (double) num5);
        int num10 = this.m_rc.Y + this.m_iTextH + (int) (((double) this.m_max - (double) this.m_kData[index].closePrice) / (double) num5);
        if ((double) this.m_kData[index].openPrice == (double) this.m_kData[index].closePrice)
        {
          this.pen.Color = SetInfo.RHColor.clKLineEqual;
          g.DrawLine(this.pen, (int) num4 - num3, num8, (int) num4 + num3, num10);
          g.DrawLine(this.pen, (int) num4, y1, (int) num4, num9);
        }
        else if ((double) this.m_kData[index].openPrice > (double) this.m_kData[index].closePrice)
        {
          this.pen.Color = SetInfo.RHColor.clKLineDown;
          this.m_Brush.Color = SetInfo.RHColor.clKLineDown;
          g.DrawLine(this.pen, (int) num4, y1, (int) num4, num9);
          int height = num10 - num8;
          if (height == 0)
            height = 1;
          g.FillRectangle((Brush) this.m_Brush, (int) num4 - num3, num8, 2 * num3 + 1, height);
        }
        else
        {
          this.pen.Color = SetInfo.RHColor.clKLineUp;
          g.DrawLine(this.pen, (int) num4, y1, (int) num4, num10);
          g.DrawLine(this.pen, (int) num4, num8, (int) num4, num9);
          int height = num8 - num10;
          if (height == 0)
            height = 1;
          g.DrawRectangle(this.pen, (int) num4 - num3, num10, 2 * num3, height);
        }
        if ((double) this.m_max == (double) this.m_kData[index].highPrice)
        {
          if (num6 == 0)
          {
            this.pen.Color = SetInfo.RHColor.clKLineEqual;
            g.DrawLine(this.pen, (int) num4, y1, (int) num4 + 15, y1 + 5);
            g.DrawString(this.m_max.ToString(), new Font("宋体", 10f, FontStyle.Bold), (Brush) new SolidBrush(Color.White), new PointF(num4 + 15f, (float) y1));
          }
          ++num6;
        }
        if ((double) this.m_min == (double) this.m_kData[index].lowPrice)
        {
          if (num7 == 0)
          {
            this.pen.Color = SetInfo.RHColor.clKLineEqual;
            g.DrawLine(this.pen, (int) num4, num9, (int) num4 + 15, num9 - 5);
            g.DrawString(this.m_min.ToString(), new Font("宋体", 10f, FontStyle.Bold), (Brush) new SolidBrush(Color.White), new PointF(num4 + 15f, (float) (num9 - 10)));
          }
          ++num7;
        }
        num4 += this.m_pos.m_Ratio;
      }
    }

    private void drawUSA(Graphics g)
    {
      int num1 = this.m_pos.m_Begin;
      int num2 = this.m_pos.m_End;
      if ((double) this.m_max - (double) this.m_min == 0.0 || this.m_rc.Height - this.m_iTextH <= 0)
        return;
      int num3 = (double) this.m_pos.m_Ratio < 3.0 ? 0 : (int) (((double) this.m_pos.m_Ratio + 1.0) / 3.0);
      if (num3 % 2 == 0 && num3 > 0)
        --num3;
      float num4 = (float) this.m_rc.X + this.m_pos.m_Ratio / 2f;
      float num5 = (this.m_max - this.m_min) / (float) (this.m_rc.Height - this.m_iTextH);
      for (int index = num1; index <= num2; ++index)
      {
        int num6 = this.m_rc.Y + this.m_iTextH + (int) (((double) this.m_max - (double) this.m_kData[index].openPrice) / (double) num5);
        int y1 = this.m_rc.Y + this.m_iTextH + (int) (((double) this.m_max - (double) this.m_kData[index].highPrice) / (double) num5);
        int y2 = this.m_rc.Y + this.m_iTextH + (int) (((double) this.m_max - (double) this.m_kData[index].lowPrice) / (double) num5);
        int num7 = this.m_rc.Y + this.m_iTextH + (int) (((double) this.m_max - (double) this.m_kData[index].closePrice) / (double) num5);
        this.pen.Color = SetInfo.RHColor.clUSALine;
        g.DrawLine(this.pen, (int) num4, y1, (int) num4, y2);
        g.DrawLine(this.pen, (int) num4 - num3, num6, (int) num4, num6);
        g.DrawLine(this.pen, (int) num4 + num3 + 1, num7, (int) num4, num7);
        num4 += this.m_pos.m_Ratio;
      }
    }

    private void DrawPolyLine(Graphics g)
    {
      int num1 = this.m_pos.m_Begin;
      int num2 = this.m_pos.m_End;
      if ((double) this.m_max - (double) this.m_min == 0.0 || this.m_rc.Height - this.m_iTextH <= 0)
        return;
      int num3 = (double) this.m_pos.m_Ratio < 3.0 ? 0 : (int) (((double) this.m_pos.m_Ratio + 1.0) / 3.0);
      if (num3 % 2 == 0 && num3 > 0)
      {
        int num4 = num3 - 1;
      }
      float num5 = (float) this.m_rc.X + this.m_pos.m_Ratio / 2f;
      float num6 = (this.m_max - this.m_min) / (float) (this.m_rc.Height - this.m_iTextH);
      this.pen.Color = SetInfo.RHColor.clPolyLine;
      int x1 = -1;
      int y1 = -1;
      for (int index = num1; index <= num2; ++index)
      {
        int y2 = this.m_rc.Y + this.m_iTextH + (int) (((double) this.m_max - (double) this.m_kData[index].closePrice) / (double) num6);
        if (x1 != -1 && y1 != -1)
        {
          g.DrawLine(this.pen, x1, y1, (int) num5, y2);
          if ((double) (y1 - y2) > (double) num5 - (double) x1)
          {
            g.DrawLine(this.pen, x1 - 1, y1, (int) num5 - 1, y2);
            g.DrawLine(this.pen, x1 + 1, y1, (int) num5 + 1, y2);
          }
          else if ((double) (y2 - y1) > (double) num5 - (double) x1)
          {
            g.DrawLine(this.pen, x1 - 1, y1, (int) num5 - 1, y2);
            g.DrawLine(this.pen, x1 + 1, y1, (int) num5 + 1, y2);
          }
          else
          {
            g.DrawLine(this.pen, x1, y1 - 1, (int) num5, y2 - 1);
            g.DrawLine(this.pen, x1, y1 + 1, (int) num5, y2 + 1);
          }
        }
        x1 = (int) num5;
        y1 = y2;
        num5 += this.m_pos.m_Ratio;
      }
    }

    public override void DrawCursor(Graphics g, int iPos)
    {
      int index = this.m_pos.m_Begin + iPos;
      if (index >= this.m_kData.Length)
        return;
      int num = (int) ((double) (this.m_rc.Y + this.m_iTextH) + ((double) this.m_max - (double) this.m_kData[index].closePrice) * (double) (this.m_rc.Height - this.m_iTextH) / ((double) this.m_max - (double) this.m_min));
      GDIDraw.XorLine(g, this.m_rc.X, num, this.m_rc.X + this.m_rc.Width, num, SetInfo.RHColor.clCursor, this.m_hqForm.ScrollOffset);
    }
  }
}
