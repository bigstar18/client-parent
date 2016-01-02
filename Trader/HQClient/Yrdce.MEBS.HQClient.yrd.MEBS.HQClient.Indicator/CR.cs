using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class CR : IndicatorBase
	{
		private readonly int[] m_iParam = new int[]
		{
			26,
			10,
			20,
			40
		};
		public CR(IndicatorPos pos, int Precision) : base(pos, Precision)
		{
			this.m_strIndicatorName = "CR";
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
				"CR",
				"a",
				"b",
				"c"
			};
			this.m_iPrecision = 2;
		}
		public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
		{
			base.Paint(g, rc, data);
			this.Calculate();
			this.m_max = -10000f;
			this.m_min = 10000f;
			base.GetValueMaxMin(this.m_data[0], this.m_iParam[0]);
			base.GetValueMaxMin(this.m_data[1], this.m_iParam[0] + this.m_iParam[1]);
			base.GetValueMaxMin(this.m_data[2], this.m_iParam[0] + this.m_iParam[2]);
			base.GetValueMaxMin(this.m_data[3], this.m_iParam[0] + this.m_iParam[3]);
			base.DrawCoordinate(g, 2);
			base.DrawLine(g, this.m_data[0], this.m_iParam[0], SetInfo.RHColor.clIndicator[0]);
			base.DrawLine(g, this.m_data[1], this.m_iParam[0] + this.m_iParam[1], SetInfo.RHColor.clIndicator[1]);
			base.DrawLine(g, this.m_data[2], this.m_iParam[0] + this.m_iParam[2], SetInfo.RHColor.clIndicator[2]);
			base.DrawLine(g, this.m_data[3], this.m_iParam[0] + this.m_iParam[3], SetInfo.RHColor.clIndicator[3]);
		}
		public override void Calculate()
		{
			this.m_data = new float[4][];
			if (this.m_kData == null || this.m_kData.Length <= 0)
			{
				return;
			}
			int n = this.m_iParam[0];
			for (int i = 0; i < 4; i++)
			{
				this.m_data[i] = new float[this.m_kData.Length];
			}
			this.GetCR(n, this.m_data[0]);
			IndicatorBase.Average(this.m_iParam[0], this.m_kData.Length, this.m_iParam[1], this.m_data[0], this.m_data[1]);
			IndicatorBase.Average(this.m_iParam[0], this.m_kData.Length, this.m_iParam[2], this.m_data[0], this.m_data[2]);
			IndicatorBase.Average(this.m_iParam[0], this.m_kData.Length, this.m_iParam[3], this.m_data[0], this.m_data[3]);
		}
		private void GetCR(int n, float[] cr)
		{
			if (this.m_kData.Length < n)
			{
				return;
			}
			float num2;
			float num = num2 = 0f;
			for (int i = 1; i < n; i++)
			{
				float num3 = (this.m_kData[i - 1].highPrice + this.m_kData[i - 1].lowPrice) / 2f;
				num2 += ((this.m_kData[i].highPrice - num3 > 0f) ? (this.m_kData[i].highPrice - num3) : 0f);
				num += ((num3 - this.m_kData[i].lowPrice > 0f) ? (num3 - this.m_kData[i].lowPrice) : 0f);
			}
			float num4 = 0f;
			for (int i = n; i < this.m_kData.Length; i++)
			{
				float num3 = (this.m_kData[i - 1].highPrice + this.m_kData[i - 1].lowPrice) / 2f;
				num2 += ((this.m_kData[i].highPrice - num3 > 0f) ? (this.m_kData[i].highPrice - num3) : 0f);
				num += ((num3 - this.m_kData[i].lowPrice > 0f) ? (num3 - this.m_kData[i].lowPrice) : 0f);
				cr[i] = num4;
				if (num != 0f)
				{
					cr[i] = num2 / num * 100f;
				}
				num4 = cr[i];
				int num5 = i - n + 1;
				num3 = (this.m_kData[num5 - 1].highPrice + this.m_kData[num5 - 1].lowPrice) / 2f;
				num2 -= ((this.m_kData[num5].highPrice - num3 > 0f) ? (this.m_kData[num5].highPrice - num3) : 0f);
				num -= ((num3 - this.m_kData[num5].lowPrice > 0f) ? (num3 - this.m_kData[num5].lowPrice) : 0f);
			}
		}
	}
}
