using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class BIAS : IndicatorBase
	{
		private readonly int[] m_iParam = new int[]
		{
			6,
			12
		};
		public BIAS(IndicatorPos pos, int iPrecision) : base(pos, iPrecision)
		{
			this.m_strIndicatorName = "BIAS";
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
				this.m_strParamName[j] = "BIAS" + (j + 1);
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
				base.GetValueMaxMin(this.m_data[i], this.m_iParam[i] - 1);
			}
			base.DrawCoordinate(g, 2);
			for (int j = 0; j < this.m_iParam.Length; j++)
			{
				base.DrawLine(g, this.m_data[j], this.m_iParam[j] - 1, SetInfo.RHColor.clIndicator[j]);
			}
		}
		public override void Calculate()
		{
			this.m_data = new float[this.m_iParam.Length][];
			if (this.m_kData == null || this.m_kData.Length <= 0)
			{
				return;
			}
			for (int i = 0; i < 2; i++)
			{
				if (this.m_iParam[i] <= this.m_kData.Length && this.m_iParam[i] > 0)
				{
					this.m_data[i] = new float[this.m_kData.Length];
					this.GetBIAS(this.m_iParam[i], this.m_data[i]);
				}
			}
		}
		private void GetBIAS(int n, float[] bias)
		{
			if (bias == null)
			{
				return;
			}
			base.AverageClose(n, bias);
			bias[n - 2] = 0f;
			for (int i = n - 1; i < this.m_kData.Length; i++)
			{
				if (bias[i] != 0f)
				{
					bias[i] = (this.m_kData[i].closePrice - bias[i]) / bias[i] * 100f;
				}
				else
				{
					bias[i] = bias[i - 1];
				}
			}
		}
	}
}
