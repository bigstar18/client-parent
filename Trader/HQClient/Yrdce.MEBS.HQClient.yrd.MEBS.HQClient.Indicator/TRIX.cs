using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class TRIX : IndicatorBase
	{
		private readonly int[] m_iParam = new int[]
		{
			12,
			9
		};
		public TRIX(IndicatorPos pos, int Precision) : base(pos, Precision)
		{
			this.m_strIndicatorName = "TRIX";
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
			base.GetValueMaxMin(this.m_data[0], this.m_iParam[0] * 3 - 3);
			base.GetValueMaxMin(this.m_data[1], this.m_iParam[0] * 3 - 3 + this.m_iParam[1] - 1);
			base.DrawCoordinate(g, 2);
			base.DrawLine(g, this.m_data[0], this.m_iParam[0] * 3 - 3, SetInfo.RHColor.clIndicator[0]);
			base.DrawLine(g, this.m_data[1], this.m_iParam[0] * 3 - 3 + this.m_iParam[1] - 1, SetInfo.RHColor.clIndicator[1]);
		}
		public override void Calculate()
		{
			this.m_data = new float[2][];
			if (this.m_kData == null || this.m_kData.Length <= 0)
			{
				return;
			}
			this.m_data[0] = new float[this.m_kData.Length];
			this.m_data[1] = new float[this.m_kData.Length];
			this.GetEXPMA(this.m_iParam[0], this.m_data[0]);
			IndicatorBase.Average(this.m_iParam[0] - 1, this.m_kData.Length, this.m_iParam[0], this.m_data[0], this.m_data[1]);
			IndicatorBase.Average(this.m_iParam[0] * 2 - 2, this.m_kData.Length, this.m_iParam[0], this.m_data[1], this.m_data[0]);
			IndicatorBase.Average(this.m_iParam[0] * 3 - 3, this.m_kData.Length, this.m_iParam[1], this.m_data[0], this.m_data[1]);
		}
		private void GetEXPMA(int n, float[] expma)
		{
			float num = 2f / (float)(n + 1);
			expma[0] = this.m_kData[0].closePrice;
			for (int i = 1; i < this.m_kData.Length; i++)
			{
				expma[i] = (this.m_kData[i].closePrice - expma[i - 1]) * num + expma[i - 1];
			}
		}
	}
}
