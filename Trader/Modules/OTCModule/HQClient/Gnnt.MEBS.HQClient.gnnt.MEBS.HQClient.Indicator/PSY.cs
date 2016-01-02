using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class PSY : IndicatorBase
	{
		private readonly int[] m_iParam = new int[]
		{
			12,
			24
		};
		public PSY(IndicatorPos pos, int Precision) : base(pos, Precision)
		{
			this.m_strIndicatorName = "PSY";
			this.m_strParamName = new string[this.m_iParam.Length];
			for (int i = 0; i < this.m_iParam.Length; i++)
			{
				this.m_strParamName[i] = "PSY" + this.m_iParam[i];
			}
			this.m_iPrecision = 2;
		}
		public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
		{
			base.Paint(g, rc, data);
			this.Calculate();
			this.m_max = -10000f;
			this.m_min = 10000f;
			for (int i = 0; i < this.m_iParam.Length; i++)
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
			this.m_data = new float[this.m_iParam.Length][];
			if (this.m_kData == null || this.m_kData.Length == 0)
			{
				return;
			}
			for (int i = 0; i < this.m_iParam.Length; i++)
			{
				if (this.m_iParam[i] <= this.m_kData.Length && this.m_iParam[i] > 0)
				{
					this.m_data[i] = new float[this.m_kData.Length];
					this.GetPSY(this.m_iParam[i], this.m_data[i]);
				}
			}
		}
		private void GetPSY(int n, float[] psy)
		{
			if (psy == null)
			{
				return;
			}
			double num = 0.0;
			for (int i = 1; i < n; i++)
			{
				if (this.m_kData[i].closePrice > this.m_kData[i - 1].closePrice)
				{
					num += 1.0;
				}
			}
			for (int i = n; i < this.m_kData.Length; i++)
			{
				if (this.m_kData[i].closePrice > this.m_kData[i - 1].closePrice)
				{
					num += 1.0;
				}
				psy[i] = (float)(num / (double)n * 100.0);
				int num2 = i - n + 1;
				if (this.m_kData[num2].closePrice > this.m_kData[num2 - 1].closePrice)
				{
					num -= 1.0;
				}
			}
		}
	}
}
