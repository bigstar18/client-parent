using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class CCI : IndicatorBase
	{
		private readonly int[] m_iParam = new int[]
		{
			21
		};
		public CCI(IndicatorPos pos, int Precision) : base(pos, Precision)
		{
			this.m_strIndicatorName = "CCI";
			this.m_strParamName = new string[]
			{
				""
			};
			this.m_iPrecision = 2;
		}
		public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
		{
			base.Paint(g, rc, data);
			this.Calculate();
			this.m_max = -10000f;
			this.m_min = 10000f;
			base.GetValueMaxMin(this.m_data[0], this.m_iParam[0] - 1);
			base.DrawCoordinate(g, 2);
			base.DrawLine(g, this.m_data[0], this.m_iParam[0] - 1, SetInfo.RHColor.clIndicator[0]);
		}
		public override void Calculate()
		{
			this.m_data = new float[2][];
			if (this.m_kData == null || this.m_kData.Length <= 0)
			{
				return;
			}
			int num = this.m_iParam[0];
			if (num > this.m_kData.Length || num < 2)
			{
				return;
			}
			this.m_data[0] = new float[this.m_kData.Length];
			this.m_data[1] = new float[this.m_kData.Length];
			float[] array = this.m_data[0];
			float[] array2 = this.m_data[1];
			double num2 = 0.0;
			for (int i = 0; i < num - 1; i++)
			{
				num2 += (double)((this.m_kData[i].highPrice + this.m_kData[i].lowPrice + this.m_kData[i].closePrice) / 3f);
			}
			float num3 = 0f;
			for (int j = num - 1; j < this.m_kData.Length; j++)
			{
				num2 -= (double)num3;
				num2 += (double)((this.m_kData[j].highPrice + this.m_kData[j].lowPrice + this.m_kData[j].closePrice) / 3f);
				array2[j] = (float)(num2 / (double)num);
				num3 = (this.m_kData[j - num + 1].highPrice + this.m_kData[j - num + 1].lowPrice + this.m_kData[j - num + 1].closePrice) / 3f;
			}
			array[num - 2] = 0f;
			for (int k = num - 1; k < this.m_kData.Length; k++)
			{
				num2 = 0.0;
				for (int l = k - num + 1; l <= k; l++)
				{
					num2 += (double)Math.Abs((this.m_kData[l].highPrice + this.m_kData[l].lowPrice + this.m_kData[l].closePrice) / 3f - array2[k]);
				}
				if (num2 == 0.0)
				{
					array[k] = array[k - 1];
				}
				else
				{
					array[k] = (float)((double)((this.m_kData[k].highPrice + this.m_kData[k].lowPrice + this.m_kData[k].closePrice) / 3f - array2[k]) / (0.015 * num2 / (double)num));
				}
			}
		}
	}
}
