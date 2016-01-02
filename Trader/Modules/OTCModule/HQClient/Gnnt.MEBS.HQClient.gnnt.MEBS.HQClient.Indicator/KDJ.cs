using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class KDJ : IndicatorBase
	{
		private readonly int[] m_iParam = new int[]
		{
			9,
			3,
			3
		};
		public KDJ(IndicatorPos pos, int Precision) : base(pos, Precision)
		{
			this.m_strIndicatorName = string.Concat(new object[]
			{
				"KDJ(",
				this.m_iParam[0],
				",",
				this.m_iParam[1],
				",",
				this.m_iParam[2],
				")"
			});
			this.m_strParamName = new string[]
			{
				"K",
				"D",
				"J"
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
			for (int i = 0; i < 3; i++)
			{
				base.GetValueMaxMin(this.m_data[i], this.m_iParam[i]);
			}
			base.DrawCoordinate(g, 2);
			for (int j = 0; j < this.m_iParam.Length; j++)
			{
				base.DrawLine(g, this.m_data[j], this.m_iParam[j], SetInfo.RHColor.clIndicator[j]);
			}
		}
		public override void Calculate()
		{
			this.m_data = new float[3][];
			int num = this.m_iParam[0];
			int num2 = this.m_iParam[1];
			int num3 = this.m_iParam[2];
			this.m_data[0] = new float[this.m_kData.Length];
			this.m_data[1] = new float[this.m_kData.Length];
			this.m_data[2] = new float[this.m_kData.Length];
			if (this.m_kData == null || num > this.m_kData.Length || num < 1)
			{
				return;
			}
			float[] array = this.m_data[0];
			float[] array2 = this.m_data[1];
			float[] array3 = this.m_data[2];
			num2 = ((num2 <= 0) ? 3 : num2);
			num3 = ((num3 <= 0) ? 3 : num3);
			float highPrice = this.m_kData[num - 1].highPrice;
			float lowPrice = this.m_kData[num - 1].lowPrice;
			for (int i = num - 1; i >= 0; i--)
			{
				if (highPrice < this.m_kData[i].highPrice)
				{
					highPrice = this.m_kData[i].highPrice;
				}
				if (lowPrice < this.m_kData[i].lowPrice)
				{
					lowPrice = this.m_kData[i].lowPrice;
				}
			}
			float num4;
			if (highPrice <= lowPrice)
			{
				num4 = 50f;
			}
			else
			{
				num4 = (this.m_kData[num - 1].closePrice - lowPrice) / (highPrice - lowPrice) * 100f;
			}
			float num5;
			array[num - 1] = (array2[num - 1] = (array3[num - 1] = (num5 = num4)));
			for (int j = 0; j < num; j++)
			{
				array[j] = 0f;
				array2[j] = 0f;
				array3[j] = 0f;
			}
			for (int j = num; j < this.m_kData.Length; j++)
			{
				highPrice = this.m_kData[j].highPrice;
				lowPrice = this.m_kData[j].lowPrice;
				for (int i = j - 1; i > j - num; i--)
				{
					if (highPrice < this.m_kData[i].highPrice)
					{
						highPrice = this.m_kData[i].highPrice;
					}
					if (lowPrice > this.m_kData[i].lowPrice)
					{
						lowPrice = this.m_kData[i].lowPrice;
					}
				}
				if (highPrice <= lowPrice)
				{
					num4 = num5;
				}
				else
				{
					num5 = num4;
					num4 = (this.m_kData[j].closePrice - lowPrice) / (highPrice - lowPrice) * 100f;
				}
				array[j] = array[j - 1] * (float)(num2 - 1) / (float)num2 + num4 / (float)num2;
				array2[j] = array[j] / (float)num3 + array2[j - 1] * (float)(num3 - 1) / (float)num3;
				array3[j] = 3f * array[j] - 2f * array2[j];
			}
		}
	}
}
