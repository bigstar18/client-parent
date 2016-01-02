using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class DMI : IndicatorBase
	{
		private readonly int[] m_iParam = new int[]
		{
			7,
			6,
			5
		};
		public DMI(IndicatorPos pos, int Precision) : base(pos, Precision)
		{
			this.m_strIndicatorName = "DMI";
			this.m_strIndicatorName += "(";
			for (int i = 0; i < this.m_iParam.Length; i++)
			{
				if (i > 0)
				{
					this.m_strIndicatorName += ",";
				}
				this.m_strIndicatorName += this.m_iParam[i];
			}
			this.m_strIndicatorName += ")";
			this.m_strParamName = new string[]
			{
				"+DI",
				"-DI",
				"ADX",
				"ADXR"
			};
			this.m_iPrecision = 2;
		}
		public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
		{
			base.Paint(g, rc, data);
			this.Calculate();
			this.m_max = -10000f;
			this.m_min = 10000f;
			base.GetValueMaxMin(this.m_data[0], this.m_iParam[0]);
			base.GetValueMaxMin(this.m_data[1], this.m_iParam[0]);
			base.GetValueMaxMin(this.m_data[2], this.m_iParam[0] + this.m_iParam[1] - 1);
			base.GetValueMaxMin(this.m_data[3], this.m_iParam[0] + this.m_iParam[1] + this.m_iParam[2] - 1);
			base.DrawCoordinate(g, 2);
			base.DrawLine(g, this.m_data[0], this.m_iParam[0], SetInfo.RHColor.clIndicator[0]);
			base.DrawLine(g, this.m_data[1], this.m_iParam[0], SetInfo.RHColor.clIndicator[1]);
			base.DrawLine(g, this.m_data[2], this.m_iParam[0] + this.m_iParam[1] - 1, SetInfo.RHColor.clIndicator[2]);
			base.DrawLine(g, this.m_data[3], this.m_iParam[0] + this.m_iParam[1] + this.m_iParam[2] - 1, SetInfo.RHColor.clIndicator[3]);
		}
		public override void Calculate()
		{
			this.m_data = new float[5][];
			if (this.m_kData == null || this.m_kData.Length <= 0)
			{
				return;
			}
			int num = this.m_iParam[0];
			int num2 = this.m_iParam[1];
			int num3 = this.m_iParam[2];
			for (int i = 0; i < 5; i++)
			{
				this.m_data[i] = new float[this.m_kData.Length];
			}
			float[] array = this.m_data[0];
			float[] array2 = this.m_data[1];
			float[] array3 = this.m_data[2];
			float[] array4 = this.m_data[2];
			float[] array5 = this.m_data[3];
			float[] array6 = this.m_data[3];
			float[] array7 = this.m_data[4];
			float[] array8 = this.m_data[4];
			if (this.m_kData.Length < num)
			{
				return;
			}
			float num4;
			float num5;
			float num6;
			for (int i = 1; i < this.m_kData.Length; i++)
			{
				num4 = Math.Abs(this.m_kData[i].highPrice - this.m_kData[i].lowPrice);
				num5 = Math.Abs(this.m_kData[i].highPrice - this.m_kData[i - 1].closePrice);
				num6 = Math.Abs(this.m_kData[i].lowPrice - this.m_kData[i - 1].closePrice);
				array3[i] = Math.Max(num4, Math.Max(num5, num6));
				num4 = this.m_kData[i].highPrice - this.m_kData[i - 1].highPrice;
				num5 = this.m_kData[i - 1].lowPrice - this.m_kData[i].lowPrice;
				num4 = ((num4 <= 0f) ? 0f : num4);
				num5 = ((num5 <= 0f) ? 0f : num5);
				array6[i] = 0f;
				array7[i] = 0f;
				if (num4 > num5)
				{
					array6[i] = num4;
				}
				else
				{
					if (num4 < num5)
					{
						array7[i] = num5;
					}
				}
			}
			num5 = (num4 = (num6 = 0f));
			for (int i = 1; i < num; i++)
			{
				num4 += array3[i];
				num5 += array6[i];
				num6 += array7[i];
			}
			float num8;
			float num7 = num8 = 0f;
			for (int i = num; i < this.m_kData.Length; i++)
			{
				num4 += array3[i];
				num5 += array6[i];
				num6 += array7[i];
				array[i] = num8;
				array2[i] = num7;
				if (num4 != 0f)
				{
					array[i] = num5 / num4 * 100f;
					array2[i] = num6 / num4 * 100f;
				}
				num8 = array[i];
				num7 = array2[i];
				int num9 = i - num + 1;
				num4 -= array3[num9];
				num5 -= array6[num9];
				num6 -= array7[num9];
			}
			for (int i = num; i < this.m_kData.Length; i++)
			{
				if (array[i] + array2[i] != 0f)
				{
					array8[i] = Math.Abs(array[i] - array2[i]) / Math.Abs(array[i] + array2[i]) * 100f;
				}
				else
				{
					array8[i] = 0f;
				}
			}
			IndicatorBase.Average(num, this.m_kData.Length, num2, array8, array4);
			for (int i = num + num2 + num3 - 1; i < this.m_kData.Length; i++)
			{
				array5[i] = (array4[i] + array4[i - num3]) / 2f;
			}
		}
	}
}
