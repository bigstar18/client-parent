using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class ASI : IndicatorBase
	{
		public ASI(IndicatorPos pos, int Precision) : base(pos, Precision)
		{
			this.m_strIndicatorName = "ASI";
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
			base.GetValueMaxMin(this.m_data[0], 1);
			base.DrawCoordinate(g, 2);
			base.DrawLine(g, this.m_data[0], 0, SetInfo.RHColor.clIndicator[0]);
		}
		public override void Calculate()
		{
			this.m_data = new float[1][];
			if (this.m_kData == null || this.m_kData.Length <= 0)
			{
				return;
			}
			this.m_data[0] = new float[this.m_kData.Length];
			float[] array = this.m_data[0];
			array[0] = 0f;
			float num = 0f;
			for (int i = 1; i < this.m_kData.Length; i++)
			{
				float num2 = Math.Abs(this.m_kData[i].highPrice - this.m_kData[i - 1].closePrice);
				float num3 = Math.Abs(this.m_kData[i].lowPrice - this.m_kData[i - 1].closePrice);
				float num4 = Math.Abs(this.m_kData[i].highPrice - this.m_kData[i - 1].lowPrice);
				float num5 = Math.Abs(this.m_kData[i - 1].closePrice - this.m_kData[i - 1].openPrice);
				float num6 = this.m_kData[i].closePrice - this.m_kData[i - 1].closePrice;
				float num7 = this.m_kData[i].closePrice - this.m_kData[i].openPrice;
				float num8 = this.m_kData[i - 1].closePrice - this.m_kData[i - 1].openPrice;
				float num9 = num6 + num7 / 2f + num8;
				float num10 = 0f;
				if (num2 >= num3 && num2 >= num4)
				{
					num10 = num2 + num3 / 2f + num5 / 4f;
				}
				if (num3 >= num2 && num3 >= num4)
				{
					num10 = num3 + num2 / 2f + num5 / 4f;
				}
				if (num4 >= num2 && num4 >= num3)
				{
					num10 = num4 + num5 / 4f;
				}
				float num11 = Math.Max(num2, num3);
				if (num11 != 0f)
				{
					num = 50f * num9 / num10 * num11 / 3f;
				}
				array[i] = array[i - 1] + num;
			}
		}
	}
}
