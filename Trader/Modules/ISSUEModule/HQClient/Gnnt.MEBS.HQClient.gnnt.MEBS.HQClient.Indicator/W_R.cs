using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class W_R : IndicatorBase
	{
		private readonly int[] m_iParam = new int[]
		{
			14,
			6
		};
		public W_R(IndicatorPos pos, int Precision) : base(pos, Precision)
		{
			this.m_strIndicatorName = "W%R";
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
			this.m_strParamName = new string[this.m_iParam.Length];
			for (int j = 0; j < this.m_iParam.Length; j++)
			{
				this.m_strParamName[j] = "WR" + (j + 1);
			}
			this.m_iPrecision = 2;
		}
		public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
		{
			base.Paint(g, rc, data);
			this.Calculate();
			this.m_max = -1E+38f;
			this.m_min = 1E+38f;
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
			if (this.m_kData == null || this.m_kData.Length <= 0)
			{
				return;
			}
			for (int i = 0; i < this.m_iParam.Length; i++)
			{
				this.m_data[i] = new float[this.m_kData.Length];
				this.GetW_R(this.m_iParam[i], this.m_data[i]);
			}
		}
		private void GetW_R(int n, float[] wms)
		{
			if (n > this.m_kData.Length || n < 1)
			{
				return;
			}
			for (int i = n - 1; i < this.m_kData.Length; i++)
			{
				float num = this.m_kData[i].highPrice;
				float num2 = this.m_kData[i].lowPrice;
				for (int j = i - 1; j > i - n; j--)
				{
					num = Math.Max(num, this.m_kData[j].highPrice);
					num2 = Math.Min(num2, this.m_kData[j].lowPrice);
				}
				if (num - num2 == 0f)
				{
					if (i - 1 == 0)
					{
						wms[i] = -50f;
					}
					else
					{
						wms[i] = wms[i - 1];
					}
				}
				else
				{
					wms[i] = -(num - this.m_kData[i].closePrice) / (num - num2) * 100f;
				}
			}
		}
	}
}
