using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class WVAD : IndicatorBase
	{
		private readonly int[] m_iParam = new int[]
		{
			6
		};
		public WVAD(IndicatorPos pos, int Precision) : base(pos, Precision)
		{
			this.m_strIndicatorName = "WVAD";
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
				""
			};
			this.m_iPrecision = 2;
		}
		public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
		{
			base.Paint(g, rc, data);
			this.Calculate();
			this.m_max = -1E+38f;
			this.m_min = 1E+38f;
			base.GetValueMaxMin(this.m_data[0], this.m_iParam[0]);
			base.DrawCoordinate(g, 2);
			base.DrawLine(g, this.m_data[0], this.m_iParam[0], SetInfo.RHColor.clIndicator[0]);
		}
		public override void Calculate()
		{
			this.m_data = new float[1][];
			if (this.m_kData == null || this.m_kData.Length <= 0)
			{
				return;
			}
			int num = this.m_iParam[0];
			if (num > this.m_kData.Length || num < 1)
			{
				return;
			}
			this.m_data[0] = new float[this.m_kData.Length];
			float[] array = this.m_data[0];
			for (int i = 0; i < this.m_kData.Length; i++)
			{
				float num2 = this.m_kData[i].highPrice - this.m_kData[i].lowPrice;
				if (num2 > 0f)
				{
					array[i] = (this.m_kData[i].closePrice - this.m_kData[i].openPrice) / num2 * (float)this.m_kData[i].totalAmount / 1000f;
				}
				else
				{
					array[i] = (float)this.m_kData[i].totalAmount / 1000f;
				}
			}
			IndicatorBase.Average(0, this.m_kData.Length, num, this.m_data[0], this.m_data[0]);
			for (int j = 0; j < this.m_kData.Length; j++)
			{
				this.m_data[0][j] *= (float)num;
			}
		}
	}
}
