using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class DMA : IndicatorBase
	{
		private readonly int[] m_iParam = new int[]
		{
			10,
			50
		};
		public DMA(IndicatorPos pos, int Precision) : base(pos, Precision)
		{
			this.m_strIndicatorName = "DMA";
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
				"DMA",
				"AMA"
			};
			this.m_iPrecision = 2;
		}
		public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
		{
			base.Paint(g, rc, data);
			this.Calculate();
			this.m_max = -10000f;
			this.m_min = 10000f;
			base.GetValueMaxMin(this.m_data[0], this.m_iParam[1] - 1);
			base.GetValueMaxMin(this.m_data[1], this.m_iParam[0] + this.m_iParam[1] - 2);
			base.DrawCoordinate(g, 2);
			base.DrawLine(g, this.m_data[0], this.m_iParam[1] - 1, SetInfo.RHColor.clIndicator[0]);
			base.DrawLine(g, this.m_data[1], this.m_iParam[0] + this.m_iParam[1] - 2, SetInfo.RHColor.clIndicator[1]);
		}
		public override void Calculate()
		{
			this.m_data = new float[2][];
			if (this.m_kData == null || this.m_kData.Length <= 0)
			{
				return;
			}
			int val = this.m_iParam[0];
			int val2 = this.m_iParam[1];
			if (this.m_kData.Length < Math.Max(val, val2))
			{
				return;
			}
			for (int i = 0; i < 2; i++)
			{
				this.m_data[i] = new float[this.m_kData.Length];
			}
			float[] array = this.m_data[0];
			float[] array2 = this.m_data[1];
			base.AverageClose(this.m_iParam[0], array);
			base.AverageClose(this.m_iParam[1], array2);
			for (int i = this.m_iParam[1] - 1; i < this.m_kData.Length; i++)
			{
				array[i] -= array2[i];
			}
			IndicatorBase.Average(this.m_iParam[1] - 1, this.m_kData.Length, this.m_iParam[0], array, array2);
		}
	}
}
