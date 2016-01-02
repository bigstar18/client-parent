// Decompiled with JetBrains decompiler
// Type: Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator.IndicatorBase
// Assembly: HQClient, Version=4.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 32B1E8FA-F247-4052-AB8C-B47D49C50CF5
// Assembly location: D:\test\长三角商品交易所(模拟)\Trader\Modules\MEBSModule\HQClient.dll

using Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;

namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
  internal abstract class IndicatorBase : IDisposable
  {
    public static string[,] INDICATOR_NAME = new string[23, 2]
    {
      {
        "ASI",
        "累计振荡指标"
      },
      {
        "BIAS",
        "乖离率"
      },
      {
        "BRAR",
        "BRAR能量指标"
      },
      {
        "BOLL",
        "布林线"
      },
      {
        "CCI",
        "顺势指标"
      },
      {
        "CR",
        "CR能量指标"
      },
      {
        "DMA",
        "平均线差"
      },
      {
        "DMI",
        "趋向指标"
      },
      {
        "EMV",
        "简易波动指标"
      },
      {
        "EXPMA",
        "指数平均数"
      },
      {
        "KDJ",
        "随机指标"
      },
      {
        "MACD",
        "平滑异同移动平均线"
      },
      {
        "MIKE",
        "麦克指标"
      },
      {
        "OBV",
        "能量潮"
      },
      {
        "PSY",
        "心理线"
      },
      {
        "ROC",
        "变动速率"
      },
      {
        "RSI",
        "相对强弱指标"
      },
      {
        "SAR",
        "抛物线指标"
      },
      {
        "TRIX",
        "三重指数平均"
      },
      {
        "VR",
        "成交量变异率"
      },
      {
        "W%R",
        "威廉指标"
      },
      {
        "WVAD",
        "威廉变异离散量"
      },
      {
        "ORDER",
        "订货量"
      }
    };
    private Font font = new Font("宋体", 9f, FontStyle.Regular);
    internal SolidBrush m_Brush = new SolidBrush(SetInfo.RHColor.clBackGround);
    internal Pen pen = new Pen(SetInfo.RHColor.clBackGround);
    protected int m_iTextH = 12;
    internal string m_strIndicatorName;
    protected Rectangle m_rc;
    protected KLineData[] m_kData;
    protected IndicatorPos m_pos;
    protected float m_max;
    protected float m_min;
    protected float[][] m_data;
    protected string[] m_strParamName;
    protected int m_iPrecision;

    public IndicatorBase(IndicatorPos pos, int iPrecision)
    {
      this.m_pos = pos;
      this.m_iPrecision = iPrecision;
    }

    public virtual void Paint(Graphics g, Rectangle rc, KLineData[] data)
    {
      this.m_rc = rc;
      this.m_kData = data;
    }

    public abstract void Calculate();

    public void DrawTitle(Graphics g, int iIndex)
    {
      this.m_Brush.Color = SetInfo.RHColor.clBackGround;
      g.FillRectangle((Brush) this.m_Brush, this.m_rc.X + 1, this.m_rc.Y + 1, this.m_rc.Width - 1, this.m_iTextH - 1);
      int num1 = this.m_rc.X + 1;
      int y = this.m_rc.Y;
      this.m_Brush.Color = SetInfo.RHColor.clItem;
      g.DrawString(this.m_strIndicatorName, this.font, (Brush) this.m_Brush, (float) num1, (float) y);
      int num2 = num1 + ((int) g.MeasureString(this.m_strIndicatorName, this.font).Width + this.m_iTextH);
      if (this.m_data[0] == null || this.m_data[0].Length == 0)
        return;
      if (iIndex >= this.m_data[0].Length)
        iIndex = this.m_data[0].Length - 1;
      for (int index = 0; index < this.m_strParamName.Length; ++index)
      {
        if (this.m_data[index] != null)
        {
          string str = M_Common.FloatToString((double) this.m_data[index][iIndex], this.m_iPrecision);
          if (this.m_strParamName[index].Length > 0)
            str = this.m_strParamName[index] + ":" + str;
          this.m_Brush.Color = SetInfo.RHColor.clIndicator[index];
          if (num2 + (int) g.MeasureString(str, this.font).Width > this.m_rc.X + this.m_rc.Width)
            break;
          g.DrawString(str, this.font, (Brush) this.m_Brush, (float) num2, (float) y);
          num2 += (int) g.MeasureString(str, this.font).Width + this.m_iTextH;
        }
      }
    }

    public virtual void DrawCursor(Graphics g, int iPos)
    {
    }

    internal void DrawCoordinate(Graphics g, int precision)
    {
      if ((double) this.m_max <= (double) this.m_min)
        return;
      this.m_iTextH = this.font.Height;
      int num1 = this.m_rc.Y + this.m_iTextH;
      if (num1 >= this.m_rc.Y + this.m_rc.Height)
        return;
      float num2;
      switch (precision)
      {
        case 2:
          num2 = 0.1f;
          break;
        case 3:
          num2 = 0.01f;
          break;
        default:
          num2 = 10f;
          break;
      }
      float num3 = num2;
      int[] numArray = new int[3]
      {
        2,
        5,
        2
      };
      int num4 = (int) ((double) num2 * (double) this.m_rc.Height / ((double) this.m_max - (double) this.m_min));
      int index = 0;
      while (num4 < this.m_iTextH * 2)
      {
        num2 *= (float) numArray[index];
        if (index == 1)
          num2 /= 2f;
        num4 = (int) ((double) num2 * (double) this.m_rc.Height / ((double) this.m_max - (double) this.m_min));
        index = (index + 1) % 3;
      }
      float num5 = (float) (int) ((double) this.m_max / (double) num3 / ((double) num2 / (double) num3)) * num2;
      while ((double) num5 <= (double) this.m_max && (double) num5 >= (double) this.m_min)
      {
        int num6 = (int) ((double) num1 + ((double) this.m_max - (double) num5) * (double) (this.m_rc.Height - this.m_iTextH) / ((double) this.m_max - (double) this.m_min));
        if (num6 < num1 + this.m_iTextH || num6 > this.m_rc.Y + this.m_rc.Height)
        {
          num5 -= num2;
        }
        else
        {
          this.pen.Color = SetInfo.RHColor.clGrid;
          g.DrawLine(this.pen, this.m_rc.X - 3, num6, this.m_rc.X, num6);
          M_Common.DrawDotLine(g, SetInfo.RHColor.clGrid, this.m_rc.X, num6, this.m_rc.X + this.m_rc.Width, num6);
          string str = M_Common.FloatToString((double) num5, precision);
          int num7 = this.m_rc.X - 3 - (int) g.MeasureString(str, this.font).Width;
          int num8 = num6 - this.font.Height / 2 + 2;
          this.m_Brush.Color = SetInfo.RHColor.clNumber;
          g.DrawString(str, this.font, (Brush) this.m_Brush, (float) num7, (float) num8);
          num5 -= num2;
        }
      }
    }

    protected void GetValueMaxMin(float[] data, int iFirst)
    {
      if (data == null)
        return;
      int num1 = this.m_pos.m_Begin <= iFirst ? iFirst : this.m_pos.m_Begin;
      int num2 = this.m_pos.m_End;
      for (int index = num1; index <= num2 && index < data.Length; ++index)
      {
        if ((double) data[index] > (double) this.m_max)
          this.m_max = data[index];
        if ((double) data[index] < (double) this.m_min)
          this.m_min = data[index];
      }
    }

    protected void DrawLine(Graphics g, float[] data, int iFirst, Color color)
    {
      if (data == null || iFirst > this.m_kData.Length || ((double) this.m_max - (double) this.m_min <= 0.0 || this.m_rc.Height - this.m_iTextH <= 0))
        return;
      int num1 = this.m_pos.m_Begin <= iFirst ? iFirst : this.m_pos.m_Begin;
      int num2 = this.m_pos.m_End;
      if (num1 > num2)
        return;
      float num3 = (float) ((double) this.m_rc.X + (double) this.m_pos.m_Ratio / 2.0 + (double) (num1 - this.m_pos.m_Begin) * (double) this.m_pos.m_Ratio);
      float num4 = (this.m_max - this.m_min) / (float) (this.m_rc.Height - this.m_iTextH);
      this.pen.Color = color;
      for (int index = num1 + 1; index <= num2 && index < data.Length; ++index)
      {
        if ((double) this.m_max >= (double) data[index - 1] && (double) data[index - 1] >= (double) this.m_min && ((double) this.m_max >= (double) data[index] && (double) data[index] >= (double) this.m_min))
          g.DrawLine(this.pen, (int) num3, this.m_rc.Y + this.m_iTextH + (int) (((double) this.m_max - (double) data[index - 1]) / (double) num4), (int) ((double) num3 + (double) this.m_pos.m_Ratio), this.m_rc.Y + this.m_iTextH + (int) (((double) this.m_max - (double) data[index]) / (double) num4));
        num3 += this.m_pos.m_Ratio;
      }
    }

    protected void AverageClose(int iParam, float[] data)
    {
      if (this.m_kData == null || this.m_kData.Length == 0)
        return;
      int num1 = iParam;
      if (num1 > this.m_kData.Length || num1 < 1)
        return;
      float num2 = 0.0f;
      double num3 = 0.0;
      for (int index = 0; index < num1 - 1; ++index)
      {
        num3 += (double) this.m_kData[index].closePrice;
        data[index] = (float) num3 / (float) (index + 1);
      }
      for (int index = num1 - 1; index < this.m_kData.Length; ++index)
      {
        num3 = num3 - (double) num2 + (double) this.m_kData[index].closePrice;
        data[index] = (float) num3 / (float) num1;
        num2 = this.m_kData[index - num1 + 1].closePrice;
      }
    }

    protected void AverageHigh(int iParam, float[] data)
    {
      if (this.m_kData == null || this.m_kData.Length == 0)
        return;
      int num1 = iParam;
      if (num1 > this.m_kData.Length || num1 < 1)
        return;
      float num2 = 0.0f;
      double num3 = 0.0;
      for (int index = 0; index < num1 - 1; ++index)
        num3 += (double) this.m_kData[index].highPrice;
      for (int index = num1 - 1; index < this.m_kData.Length; ++index)
      {
        num3 = num3 - (double) num2 + (double) this.m_kData[index].highPrice;
        data[index] = (float) num3 / (float) num1;
        num2 = this.m_kData[index - num1 + 1].highPrice;
      }
    }

    protected void AverageLow(int iParam, float[] data)
    {
      if (this.m_kData == null || this.m_kData.Length == 0)
        return;
      int num1 = iParam;
      if (num1 > this.m_kData.Length || num1 < 1)
        return;
      float num2 = 0.0f;
      double num3 = 0.0;
      for (int index = 0; index < num1 - 1; ++index)
        num3 += (double) this.m_kData[index].lowPrice;
      for (int index = num1 - 1; index < this.m_kData.Length; ++index)
      {
        num3 = num3 - (double) num2 + (double) this.m_kData[index].lowPrice;
        data[index] = (float) num3 / (float) num1;
        num2 = this.m_kData[index - num1 + 1].lowPrice;
      }
    }

    internal static void Average(int begin, int iCount, int n, float[] source, float[] destination)
    {
      if (source == null || destination == null || (n > iCount - begin || n < 1))
        return;
      float num1 = 0.0f;
      double num2 = 0.0;
      for (int index = iCount - 1; index > iCount - n; --index)
        num2 += (double) source[index];
      for (int index = iCount - 1; index >= begin + n - 1; --index)
      {
        num2 = num2 - (double) num1 + (double) source[index - n + 1];
        num1 = source[index];
        destination[index] = (float) num2 / (float) n;
      }
    }

    public void Dispose()
    {
      this.font.Dispose();
      this.m_Brush.Dispose();
      this.pen.Dispose();
    }
  }
}
