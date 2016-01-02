using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal abstract class IndicatorBase : IDisposable
	{
		private Font font = new Font("宋体", 9f, FontStyle.Regular);
		internal SolidBrush m_Brush = new SolidBrush(SetInfo.RHColor.clBackGround);
		internal Pen pen = new Pen(SetInfo.RHColor.clBackGround);
		internal string m_strIndicatorName;
		protected Rectangle m_rc;
		protected KLineData[] m_kData;
		protected IndicatorPos m_pos;
		protected float m_max;
		protected float m_min;
		protected int m_iTextH = 12;
		protected float[][] m_data;
		protected string[] m_strParamName;
		protected int m_iPrecision;
		public static string[,] INDICATOR_NAME;
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
			g.FillRectangle(this.m_Brush, this.m_rc.X + 1, this.m_rc.Y + 1, this.m_rc.Width - 1, this.m_iTextH - 1);
			int num = this.m_rc.X + 1;
			int y = this.m_rc.Y;
			this.m_Brush.Color = SetInfo.RHColor.clItem;
			g.DrawString(this.m_strIndicatorName, this.font, this.m_Brush, (float)num, (float)y);
			num += (int)g.MeasureString(this.m_strIndicatorName, this.font).Width + this.m_iTextH;
			if (this.m_data[0] == null || this.m_data[0].Length == 0)
			{
				return;
			}
			if (iIndex >= this.m_data[0].Length)
			{
				iIndex = this.m_data[0].Length - 1;
			}
			for (int i = 0; i < this.m_strParamName.Length; i++)
			{
				if (this.m_data[i] != null)
				{
					string text = M_Common.FloatToString((double)this.m_data[i][iIndex], this.m_iPrecision);
					if (this.m_strParamName[i].Length > 0)
					{
						text = this.m_strParamName[i] + ":" + text;
					}
					this.m_Brush.Color = SetInfo.RHColor.clIndicator[i];
					if (num + (int)g.MeasureString(text, this.font).Width > this.m_rc.X + this.m_rc.Width)
					{
						return;
					}
					g.DrawString(text, this.font, this.m_Brush, (float)num, (float)y);
					num += (int)g.MeasureString(text, this.font).Width + this.m_iTextH;
				}
			}
		}
		public virtual void DrawCursor(Graphics g, int iPos)
		{
		}
		internal void DrawCoordinate(Graphics g, int precision)
		{
			if (this.m_max <= this.m_min)
			{
				return;
			}
			this.m_iTextH = this.font.Height;
			int num = this.m_rc.Y + this.m_iTextH;
			if (num >= this.m_rc.Y + this.m_rc.Height)
			{
				return;
			}
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
			int[] array = new int[]
			{
				2,
				5,
				2
			};
			int i = (int)(num2 * (float)this.m_rc.Height / (this.m_max - this.m_min));
			int num4 = 0;
			while (i < this.m_iTextH * 2)
			{
				num2 *= (float)array[num4];
				if (num4 == 1)
				{
					num2 /= 2f;
				}
				i = (int)(num2 * (float)this.m_rc.Height / (this.m_max - this.m_min));
				num4++;
				num4 %= 3;
			}
			float num5 = (float)((int)(this.m_max / num3 / (num2 / num3))) * num2;
			float num6 = num5;
			while (num6 <= this.m_max)
			{
				if (num6 < this.m_min)
				{
					return;
				}
				int num7 = (int)((float)num + (this.m_max - num6) * (float)(this.m_rc.Height - this.m_iTextH) / (this.m_max - this.m_min));
				if (num7 < num + this.m_iTextH || num7 > this.m_rc.Y + this.m_rc.Height)
				{
					num6 -= num2;
				}
				else
				{
					this.pen.Color = SetInfo.RHColor.clGrid;
					g.DrawLine(this.pen, this.m_rc.X - 3, num7, this.m_rc.X, num7);
					M_Common.DrawDotLine(g, SetInfo.RHColor.clGrid, this.m_rc.X, num7, this.m_rc.X + this.m_rc.Width, num7);
					string text = M_Common.FloatToString((double)num6, precision);
					int num8 = this.m_rc.X - 3 - (int)g.MeasureString(text, this.font).Width;
					int num9 = num7 - this.font.Height / 2 + 2;
					this.m_Brush.Color = SetInfo.RHColor.clNumber;
					g.DrawString(text, this.font, this.m_Brush, (float)num8, (float)num9);
					num6 -= num2;
				}
			}
		}
		protected void GetValueMaxMin(float[] data, int iFirst)
		{
			if (data == null)
			{
				return;
			}
			int num = (this.m_pos.m_Begin <= iFirst) ? iFirst : this.m_pos.m_Begin;
			int end = this.m_pos.m_End;
			for (int i = num; i <= end; i++)
			{
				if (i >= data.Length)
				{
					return;
				}
				if (data[i] > this.m_max)
				{
					this.m_max = data[i];
				}
				if (data[i] < this.m_min)
				{
					this.m_min = data[i];
				}
			}
		}
		protected void DrawLine(Graphics g, float[] data, int iFirst, Color color)
		{
			if (data == null)
			{
				return;
			}
			if (iFirst > this.m_kData.Length)
			{
				return;
			}
			if (this.m_max - this.m_min <= 0f || this.m_rc.Height - this.m_iTextH <= 0)
			{
				return;
			}
			int num = (this.m_pos.m_Begin <= iFirst) ? iFirst : this.m_pos.m_Begin;
			int end = this.m_pos.m_End;
			if (num > end)
			{
				return;
			}
			float num2 = (float)this.m_rc.X + this.m_pos.m_Ratio / 2f + (float)(num - this.m_pos.m_Begin) * this.m_pos.m_Ratio;
			float num3 = (this.m_max - this.m_min) / (float)(this.m_rc.Height - this.m_iTextH);
			this.pen.Color = color;
			for (int i = num + 1; i <= end; i++)
			{
				if (i >= data.Length)
				{
					return;
				}
				if (this.m_max >= data[i - 1] && data[i - 1] >= this.m_min && this.m_max >= data[i] && data[i] >= this.m_min)
				{
					g.DrawLine(this.pen, (int)num2, this.m_rc.Y + this.m_iTextH + (int)((this.m_max - data[i - 1]) / num3), (int)(num2 + this.m_pos.m_Ratio), this.m_rc.Y + this.m_iTextH + (int)((this.m_max - data[i]) / num3));
				}
				num2 += this.m_pos.m_Ratio;
			}
		}
		protected void AverageClose(int iParam, float[] data)
		{
			if (this.m_kData == null || this.m_kData.Length == 0)
			{
				return;
			}
			if (iParam > this.m_kData.Length || iParam < 1)
			{
				return;
			}
			float num = 0f;
			double num2 = 0.0;
			for (int i = 0; i < iParam - 1; i++)
			{
				num2 += (double)this.m_kData[i].closePrice;
				data[i] = (float)(num2 / (double)(i + 1));
			}
			for (int i = iParam - 1; i < this.m_kData.Length; i++)
			{
				num2 -= (double)num;
				num2 += (double)this.m_kData[i].closePrice;
				data[i] = (float)(num2 / (double)iParam);
				num = this.m_kData[i - iParam + 1].closePrice;
			}
		}
		protected void AverageHigh(int iParam, float[] data)
		{
			if (this.m_kData == null || this.m_kData.Length == 0)
			{
				return;
			}
			if (iParam > this.m_kData.Length || iParam < 1)
			{
				return;
			}
			float num = 0f;
			double num2 = 0.0;
			for (int i = 0; i < iParam - 1; i++)
			{
				num2 += (double)this.m_kData[i].highPrice;
			}
			for (int i = iParam - 1; i < this.m_kData.Length; i++)
			{
				num2 -= (double)num;
				num2 += (double)this.m_kData[i].highPrice;
				data[i] = (float)(num2 / (double)iParam);
				num = this.m_kData[i - iParam + 1].highPrice;
			}
		}
		protected void AverageLow(int iParam, float[] data)
		{
			if (this.m_kData == null || this.m_kData.Length == 0)
			{
				return;
			}
			if (iParam > this.m_kData.Length || iParam < 1)
			{
				return;
			}
			float num = 0f;
			double num2 = 0.0;
			for (int i = 0; i < iParam - 1; i++)
			{
				num2 += (double)this.m_kData[i].lowPrice;
			}
			for (int i = iParam - 1; i < this.m_kData.Length; i++)
			{
				num2 -= (double)num;
				num2 += (double)this.m_kData[i].lowPrice;
				data[i] = (float)(num2 / (double)iParam);
				num = this.m_kData[i - iParam + 1].lowPrice;
			}
		}
		internal static void Average(int begin, int iCount, int n, float[] source, float[] destination)
		{
			if (source == null || destination == null)
			{
				return;
			}
			if (n > iCount - begin || n < 1)
			{
				return;
			}
			float num = 0f;
			double num2 = 0.0;
			for (int i = iCount - 1; i > iCount - n; i--)
			{
				num2 += (double)source[i];
			}
			for (int i = iCount - 1; i >= begin + n - 1; i--)
			{
				num2 -= (double)num;
				num2 += (double)source[i - n + 1];
				num = source[i];
				destination[i] = (float)(num2 / (double)n);
			}
		}
		public void Dispose()
		{
			this.font.Dispose();
			this.m_Brush.Dispose();
			this.pen.Dispose();
		}
		static IndicatorBase()
		{
			// Note: this type is marked as 'beforefieldinit'.
			string[,] array = new string[23, 2];
			array[0, 0] = "ASI";
			array[0, 1] = "累计振荡指标";
			array[1, 0] = "BIAS";
			array[1, 1] = "乖离率";
			array[2, 0] = "BRAR";
			array[2, 1] = "BRAR能量指标";
			array[3, 0] = "BOLL";
			array[3, 1] = "布林线";
			array[4, 0] = "CCI";
			array[4, 1] = "顺势指标";
			array[5, 0] = "CR";
			array[5, 1] = "CR能量指标";
			array[6, 0] = "DMA";
			array[6, 1] = "平均线差";
			array[7, 0] = "DMI";
			array[7, 1] = "趋向指标";
			array[8, 0] = "EMV";
			array[8, 1] = "简易波动指标";
			array[9, 0] = "EXPMA";
			array[9, 1] = "指数平均数";
			array[10, 0] = "KDJ";
			array[10, 1] = "随机指标";
			array[11, 0] = "MACD";
			array[11, 1] = "平滑异同移动平均线";
			array[12, 0] = "MIKE";
			array[12, 1] = "麦克指标";
			array[13, 0] = "OBV";
			array[13, 1] = "能量潮";
			array[14, 0] = "PSY";
			array[14, 1] = "心理线";
			array[15, 0] = "ROC";
			array[15, 1] = "变动速率";
			array[16, 0] = "RSI";
			array[16, 1] = "相对强弱指标";
			array[17, 0] = "SAR";
			array[17, 1] = "抛物线指标";
			array[18, 0] = "TRIX";
			array[18, 1] = "三重指数平均";
			array[19, 0] = "VR";
			array[19, 1] = "成交量变异率";
			array[20, 0] = "W%R";
			array[20, 1] = "威廉指标";
			array[21, 0] = "WVAD";
			array[21, 1] = "威廉变异离散量";
			array[22, 0] = "ORDER";
			array[22, 1] = "订货量";
			IndicatorBase.INDICATOR_NAME = array;
		}
	}
}
