using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class ROC : IndicatorBase
	{
		private readonly int[] m_iParam = new int[]
		{
			12,
			6
		};
		public ROC(IndicatorPos pos, int Precision) : base(pos, Precision)
		{
			this.m_strIndicatorName = "ROC";
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
				"",
				"ROCMA"
			};
			this.m_iPrecision = 2;
		}
		public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
		{
			base.Paint(g, rc, data);
			this.Calculate();
			this.m_max = -10000f;
			this.m_min = 10000f;
			base.GetValueMaxMin(this.m_data[0], this.m_iParam[0] + 1);
			base.DrawCoordinate(g, 2);
			base.DrawLine(g, this.m_data[0], this.m_iParam[1] + 1, SetInfo.RHColor.clIndicator[0]);
			base.DrawLine(g, this.m_data[1], this.m_iParam[0] + this.m_iParam[1], SetInfo.RHColor.clIndicator[1]);
		}
		public override void Calculate()
		{
			this.m_data = new float[2][];
			if (this.m_kData == null || this.m_kData.Length <= 0)
			{
				return;
			}
			int num = this.m_iParam[0];
			int n = this.m_iParam[1];
			if (this.m_kData.Length < num || num < 1)
			{
				return;
			}
			this.m_data[0] = new float[this.m_kData.Length];
			this.m_data[1] = new float[this.m_kData.Length];
			float[] array = this.m_data[0];
			array[num - 1] = 0f;
			for (int i = num; i < this.m_kData.Length; i++)
			{
				if (this.m_kData[i - num].closePrice == 0f)
				{
					array[i] = array[i - 1];
				}
				else
				{
					array[i] = (this.m_kData[i].closePrice / this.m_kData[i - num].closePrice - 1f) * 100f;
				}
			}
			IndicatorBase.Average(1, this.m_kData.Length, n, array, this.m_data[1]);
		}
	}
}
