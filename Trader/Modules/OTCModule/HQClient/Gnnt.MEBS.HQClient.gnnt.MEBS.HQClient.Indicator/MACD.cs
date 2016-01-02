using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class MACD : IndicatorBase
	{
		private readonly int[] m_iParam = new int[]
		{
			12,
			26,
			9
		};
		public MACD(IndicatorPos pos, int Precision) : base(pos, Precision)
		{
			this.m_strIndicatorName = string.Concat(new object[]
			{
				"MACD(",
				this.m_iParam[0],
				",",
				this.m_iParam[1],
				",",
				this.m_iParam[2],
				")"
			});
			this.m_strParamName = new string[]
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
			{
				return;
			}
			base.Paint(g, rc, data);
			this.Calculate();
			this.m_max = 0f;
			this.m_min = 10000f;
			base.GetValueMaxMin(this.m_data[0], 0);
			base.GetValueMaxMin(this.m_data[1], 0);
			base.GetValueMaxMin(this.m_data[2], 0);
			base.DrawCoordinate(g, 2);
			base.DrawLine(g, this.m_data[0], 0, SetInfo.RHColor.clIndicator[0]);
			base.DrawLine(g, this.m_data[1], 0, SetInfo.RHColor.clIndicator[1]);
			this.DrawVertLine(g, this.m_data[2], 0, SetInfo.RHColor.clIncrease, SetInfo.RHColor.clDecrease);
		}
		public override void Calculate()
		{
			this.m_data = new float[3][];
			this.m_data[0] = new float[this.m_kData.Length];
			this.m_data[1] = new float[this.m_kData.Length];
			this.m_data[2] = new float[this.m_kData.Length];
			float[] array = this.m_data[0];
			float[] array2 = this.m_data[1];
			float[] array3 = this.m_data[2];
			float num = 0f;
			float num2 = 0f;
			float[] array4 = new float[3];
			float[] array5 = new float[3];
			int[] array6 = new int[3];
			for (int i = 0; i < 3; i++)
			{
				array6[i] = this.m_iParam[i];
				array4[i] = 2f / (float)(array6[i] + 1);
				array5[i] = 0f;
			}
			for (int j = 0; j < this.m_kData.Length; j++)
			{
				float closePrice = this.m_kData[j].closePrice;
				if (j == 0)
				{
					num = closePrice;
					num2 = closePrice;
				}
				num = (closePrice - num) * array4[0] + num;
				num2 = (closePrice - num2) * array4[1] + num2;
				array[j] = ((j >= array6[0] - 1 && j >= array6[1]) ? (num - num2) : (num - num2));
				if (j == 0)
				{
					array2[j] = array[j];
				}
				else
				{
					array2[j] = (float)((double)(array[j] - array2[j - 1]) * 0.2) + array2[j - 1];
				}
				array3[j] = array[j] - array2[j];
			}
		}
		private void DrawVertLine(Graphics g, float[] data, int iFirst, Color color1, Color color2)
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
			int y = this.m_rc.Y + this.m_iTextH + (int)((this.m_max - 0f) / num3);
			this.pen.Color = color1;
			for (int i = num; i <= end; i++)
			{
				if (data[i] > 0f)
				{
					this.pen.Color = color1;
				}
				else
				{
					this.pen.Color = color2;
				}
				g.DrawLine(this.pen, (int)num2, y, (int)num2, this.m_rc.Y + this.m_iTextH + (int)((this.m_max - data[i]) / num3));
				num2 += this.m_pos.m_Ratio;
			}
		}
	}
}
