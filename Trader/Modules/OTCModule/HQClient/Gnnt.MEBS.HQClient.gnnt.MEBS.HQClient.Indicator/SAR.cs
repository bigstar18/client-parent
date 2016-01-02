using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class SAR : KLine
	{
		private readonly int[] m_iParam = new int[]
		{
			5
		};
		private readonly int SAR_UP;
		private readonly int SAR_DOWN = 1;
		private readonly int SAR_CUP = 16;
		private readonly int SAR_CDOWN = 17;
		public SAR(IndicatorPos pos, int iPrecision, HQForm hqForm) : base(pos, 0, iPrecision, hqForm)
		{
			this.m_strIndicatorName = "SAR(" + this.m_iParam[0] + ")";
			this.m_strParamName = new string[]
			{
				""
			};
		}
		public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
		{
			this.m_kData = data;
			this.Calculate();
			base.Paint(g, rc, data);
			Color[] color = new Color[]
			{
				SetInfo.RHColor.clIncrease,
				SetInfo.RHColor.clDecrease,
				SetInfo.RHColor.clEqual
			};
			this.DrawSAR(g, this.m_iParam[0] - 1, this.m_data[0], this.m_data[1], color);
		}
		public override void Calculate()
		{
			int num = this.m_iParam[0];
			this.m_data = new float[2][];
			if (this.m_kData == null || this.m_kData.Length <= 0)
			{
				return;
			}
			if (num > this.m_kData.Length || num < 3)
			{
				return;
			}
			for (int i = 0; i < 2; i++)
			{
				this.m_data[i] = new float[this.m_kData.Length];
			}
			float[] array = this.m_data[0];
			float[] array2 = this.m_data[1];
			float num2 = 0.02f;
			if (this.m_kData[num - 1].closePrice < this.m_kData[num - 2].closePrice)
			{
				if (this.m_kData[num - 2].closePrice <= this.m_kData[num - 3].closePrice)
				{
					array2[num - 1] = (float)this.SAR_DOWN;
				}
				else
				{
					array2[num - 1] = (float)this.SAR_CDOWN;
				}
			}
			else if (this.m_kData[num - 1].closePrice > this.m_kData[num - 2].closePrice)
			{
				if (this.m_kData[num - 2].closePrice >= this.m_kData[num - 3].closePrice)
				{
					array2[num - 1] = (float)this.SAR_UP;
				}
				else
				{
					array2[num - 1] = (float)this.SAR_CUP;
				}
			}
			else if (this.m_kData[num - 2].closePrice < this.m_kData[num - 3].closePrice)
			{
				array2[num - 1] = (float)this.SAR_DOWN;
			}
			else if (this.m_kData[num - 2].closePrice > this.m_kData[num - 3].closePrice)
			{
				array2[num - 1] = (float)this.SAR_UP;
			}
			else
			{
				array2[num - 1] = (float)this.SAR_CUP;
			}
			if (array2[num - 1] == (float)this.SAR_DOWN || array2[num - 1] == (float)this.SAR_CDOWN)
			{
				array[num - 1] = -1E+36f;
				for (int j = num - 1; j >= 0; j--)
				{
					array[num - 1] = Math.Max(array[num - 1], this.m_kData[j].highPrice);
				}
			}
			else
			{
				array[num - 1] = 1E+36f;
				for (int j = num - 1; j >= 0; j--)
				{
					array[num - 1] = Math.Min(array[num - 1], this.m_kData[j].lowPrice);
				}
			}
			for (int i = num; i < this.m_kData.Length; i++)
			{
				if (array2[i - 1] == (float)this.SAR_UP || array2[i - 1] == (float)this.SAR_CUP)
				{
					if (this.m_kData[i].closePrice < array[i - 1])
					{
						array[i] = -1E+36f;
						for (int j = i; j > i - num; j--)
						{
							array[i] = Math.Max(array[i], this.m_kData[j].highPrice);
						}
						array2[i] = (float)this.SAR_CDOWN;
						num2 = 0.02f;
					}
					else
					{
						array[i] = array[i - 1] + num2 * (this.m_kData[i - 1].highPrice - array[i - 1]);
						num2 = ((num2 < 0.2f) ? (num2 + 0.02f) : num2);
						array2[i] = (float)this.SAR_UP;
					}
				}
				else if (this.m_kData[i].closePrice > array[i - 1])
				{
					array[i] = 1E+36f;
					for (int j = i; j > i - num; j--)
					{
						array[i] = Math.Min(array[i], this.m_kData[j].lowPrice);
					}
					array2[i] = (float)this.SAR_CUP;
					num2 = 0.02f;
				}
				else
				{
					array[i] = array[i - 1] + num2 * (this.m_kData[i - 1].lowPrice - array[i - 1]);
					num2 = ((num2 < 0.2f) ? (num2 + 0.02f) : num2);
					array2[i] = (float)this.SAR_DOWN;
				}
			}
		}
		protected override void GetMaxMin()
		{
			base.GetMaxMin();
			base.GetValueMaxMin(this.m_data[0], this.m_iParam[0]);
			if (this.m_rc.Height > this.m_iTextH)
			{
				float num = this.m_max - this.m_min;
				this.m_max += this.m_pos.m_Ratio / 2f * num / (float)(this.m_rc.Height - this.m_iTextH);
				this.m_min -= this.m_pos.m_Ratio / 2f * num / (float)(this.m_rc.Height - this.m_iTextH);
			}
		}
		private void DrawSAR(Graphics g, int iBegin, float[] data, float[] sign, Color[] color)
		{
			if (data == null || sign == null)
			{
				return;
			}
			Rectangle rectangle = new Rectangle(this.m_rc.X, this.m_rc.Y + this.m_iTextH, this.m_rc.Width, this.m_rc.Height - this.m_iTextH);
			if (this.m_max - this.m_min == 0f || rectangle.Height <= 0)
			{
				return;
			}
			int num = (this.m_pos.m_Begin < iBegin) ? iBegin : this.m_pos.m_Begin;
			float num2 = (float)rectangle.X + (float)(num - this.m_pos.m_Begin) * this.m_pos.m_Ratio;
			float num3 = (this.m_max - this.m_min) / (float)rectangle.Height;
			for (int i = num; i <= this.m_pos.m_End; i++)
			{
				float num4 = (float)rectangle.Y + (this.m_max - data[i]) / num3;
				if (sign[i] == (float)this.SAR_DOWN)
				{
					this.pen.Color = color[1];
				}
				else if (sign[i] == (float)this.SAR_UP)
				{
					this.pen.Color = color[0];
				}
				else
				{
					this.pen.Color = color[2];
				}
				int num5 = (int)num2;
				int num6 = (int)(num4 - this.m_pos.m_Ratio / 2f);
				int num7 = (int)(num2 + this.m_pos.m_Ratio);
				int num8 = (int)(num4 + this.m_pos.m_Ratio / 2f);
				if (num7 > num5 && num8 > num6)
				{
					g.DrawArc(this.pen, num5, num6, num7 - num5, num8 - num6, 0, 360);
				}
				num2 += this.m_pos.m_Ratio;
			}
		}
	}
}
