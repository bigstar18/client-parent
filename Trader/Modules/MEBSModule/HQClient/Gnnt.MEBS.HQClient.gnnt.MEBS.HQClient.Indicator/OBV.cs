using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class OBV : IndicatorBase
	{
		private readonly int[] m_iParam = new int[]
		{
			12
		};
		public OBV(IndicatorPos pos, int Precision) : base(pos, Precision)
		{
			this.m_strIndicatorName = "OBV";
			this.m_strParamName = new string[]
			{
				"",
				"MA" + this.m_iParam[0]
			};
			this.m_iPrecision = 2;
		}
		public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
		{
			base.Paint(g, rc, data);
			this.Calculate();
			this.m_max = -1E+38f;
			this.m_min = 1E+38f;
			base.GetValueMaxMin(this.m_data[0], 0);
			base.DrawCoordinate(g, 2);
			base.DrawLine(g, this.m_data[0], 0, SetInfo.RHColor.clIndicator[0]);
			base.DrawLine(g, this.m_data[1], this.m_iParam[0], SetInfo.RHColor.clIndicator[1]);
		}
		public override void Calculate()
		{
			this.m_data = new float[2][];
			if (this.m_kData == null || this.m_kData.Length <= 0)
			{
				return;
			}
			int n = this.m_iParam[0];
			this.m_data[0] = new float[this.m_kData.Length];
			this.m_data[1] = new float[this.m_kData.Length];
			float[] array = this.m_data[0];
			array[0] = 0f;
			for (int i = 1; i < this.m_kData.Length; i++)
			{
				if (this.m_kData[i].closePrice > this.m_kData[i - 1].closePrice)
				{
					array[i] = array[i - 1] + (float)(this.m_kData[i].totalAmount / 1000L);
				}
				else if (this.m_kData[i].closePrice < this.m_kData[i - 1].closePrice)
				{
					array[i] = array[i - 1] - (float)(this.m_kData[i].totalAmount / 1000L);
				}
				else
				{
					array[i] = array[i - 1];
				}
			}
			IndicatorBase.Average(1, this.m_kData.Length, n, array, this.m_data[1]);
		}
	}
}
