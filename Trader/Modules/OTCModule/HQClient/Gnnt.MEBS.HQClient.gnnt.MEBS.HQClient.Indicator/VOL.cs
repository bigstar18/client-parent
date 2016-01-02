using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class VOL : IndicatorBase
	{
		private readonly int[] m_iParam = new int[]
		{
			5,
			10
		};
		public VOL(IndicatorPos pos, int Precision) : base(pos, Precision)
		{
			this.m_strIndicatorName = "VOL";
			this.m_iPrecision = 0;
			this.m_strParamName = new string[this.m_iParam.Length];
			for (int i = 0; i < this.m_iParam.Length; i++)
			{
				this.m_strParamName[i] = "MA" + this.m_iParam[i];
			}
		}
		public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
		{
			base.Paint(g, rc, data);
			this.Calculate();
			this.GetMaxMin();
			for (int i = 0; i < this.m_iParam.Length; i++)
			{
				base.GetValueMaxMin(this.m_data[i], this.m_iParam[i] - 1);
			}
			base.DrawCoordinate(g, 0);
			this.DrawVolume(g);
			for (int j = 0; j < this.m_iParam.Length; j++)
			{
				base.DrawLine(g, this.m_data[j], this.m_iParam[j] - 1, SetInfo.RHColor.clIndicator[j]);
			}
		}
		public override void Calculate()
		{
			this.m_data = new float[this.m_iParam.Length][];
			for (int i = 0; i < this.m_iParam.Length; i++)
			{
				this.AverageVolume(i);
			}
			this.m_data = new float[this.m_iParam.Length][];
			for (int j = 0; j < this.m_iParam.Length; j++)
			{
				this.AverageVolume(j);
			}
		}
		private void GetMaxMin()
		{
			this.m_max = 0f;
			this.m_min = 0f;
			for (int i = this.m_pos.m_Begin; i <= this.m_pos.m_End; i++)
			{
				if ((float)this.m_kData[i].totalAmount > this.m_max)
				{
					this.m_max = (float)this.m_kData[i].totalAmount;
				}
			}
		}
		private void DrawVolume(Graphics g)
		{
			if (this.m_max - this.m_min == 0f || this.m_rc.Height - this.m_iTextH <= 0)
			{
				return;
			}
			int num = (this.m_pos.m_Ratio < 3f) ? 0 : ((int)((this.m_pos.m_Ratio + 1f) / 3f));
			if (num % 2 == 0 && num > 0)
			{
				num--;
			}
			float num2 = (float)this.m_rc.X + this.m_pos.m_Ratio / 2f;
			float num3 = (this.m_max - this.m_min) / (float)(this.m_rc.Height - this.m_iTextH);
			for (int i = this.m_pos.m_Begin; i <= this.m_pos.m_End; i++)
			{
				int num4 = this.m_rc.Y + this.m_iTextH + (int)((this.m_max - (float)this.m_kData[i].totalAmount) / num3);
				if (this.m_kData[i].openPrice > this.m_kData[i].closePrice)
				{
					this.m_Brush.Color = SetInfo.RHColor.clKLineDown;
					g.FillRectangle(this.m_Brush, (int)num2 - num, num4, 2 * num + 1, this.m_rc.Y + this.m_rc.Height - num4 - 1);
				}
				else if (this.m_kData[i].openPrice < this.m_kData[i].closePrice)
				{
					this.pen.Color = SetInfo.RHColor.clKLineUp;
					g.DrawRectangle(this.pen, (int)num2 - num, num4, 2 * num + 1, this.m_rc.Y + this.m_rc.Height - num4 - 1);
				}
				else
				{
					this.pen.Color = SetInfo.RHColor.clKLineEqual;
					g.DrawRectangle(this.pen, (int)num2 - num, num4, 2 * num + 1, this.m_rc.Y + this.m_rc.Height - num4 - 1);
				}
				num2 += this.m_pos.m_Ratio;
			}
		}
		private void AverageVolume(int iIndex)
		{
			if (this.m_kData == null || this.m_kData.Length == 0)
			{
				return;
			}
			int num = this.m_iParam[iIndex];
			if (num > this.m_kData.Length || num < 1)
			{
				return;
			}
			this.m_data[iIndex] = new float[this.m_kData.Length];
			float[] array = this.m_data[iIndex];
			float num2 = 0f;
			double num3 = 0.0;
			for (int i = 0; i < num - 1; i++)
			{
				num3 += (double)this.m_kData[i].totalAmount;
			}
			for (int i = num - 1; i < this.m_kData.Length; i++)
			{
				num3 -= (double)num2;
				num3 += (double)this.m_kData[i].totalAmount;
				array[i] = (float)(num3 / (double)num);
				num2 = (float)this.m_kData[i - num + 1].totalAmount;
			}
		}
	}
}
