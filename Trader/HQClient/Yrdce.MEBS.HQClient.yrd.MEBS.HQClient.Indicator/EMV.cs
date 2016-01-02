using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class EMV : IndicatorBase
	{
		private readonly int[] m_iParam = new int[]
		{
			14,
			9
		};
		public EMV(IndicatorPos pos, int Precision) : base(pos, Precision)
		{
			this.m_strIndicatorName = "EMV";
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
				"EMVMA"
			};
			this.m_iPrecision = 2;
		}
		public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
		{
			if (data == null || data.Length == 0)
			{
				return;
			}
			base.Paint(g, rc, data);
			this.Calculate();
			this.m_max = -10000f;
			this.m_min = 10000f;
			base.GetValueMaxMin(this.m_data[0], this.m_iParam[0]);
			if (this.m_iParam[1] > 0 && this.m_iParam[1] <= this.m_kData.Length)
			{
				base.GetValueMaxMin(this.m_data[1], this.m_iParam[0] + this.m_iParam[1] - 1);
			}
			base.DrawCoordinate(g, 2);
			base.DrawLine(g, this.m_data[0], this.m_iParam[0], SetInfo.RHColor.clIndicator[0]);
			if (this.m_iParam[1] > 0 && this.m_iParam[1] <= this.m_kData.Length)
			{
				base.DrawLine(g, this.m_data[1], this.m_iParam[0] + this.m_iParam[1] - 1, SetInfo.RHColor.clIndicator[1]);
			}
		}
		public override void Calculate()
		{
			this.m_data = new float[2][];
			int num = this.m_iParam[0];
			int num2 = this.m_iParam[1];
			if (this.m_kData == null || num > this.m_kData.Length || num < 1)
			{
				return;
			}
			this.m_data[0] = new float[this.m_kData.Length];
			this.m_data[1] = new float[this.m_kData.Length];
			float[] array = this.m_data[0];
			float[] destination = this.m_data[1];
			array[num - 1] = 0f;
			for (int i = num; i < this.m_kData.Length; i++)
			{
				array[i] = 0f;
				if (this.m_kData[i].totalAmount > 0L)
				{
					array[i] = (this.m_kData[i].highPrice + this.m_kData[i].lowPrice - this.m_kData[i - num].highPrice - this.m_kData[i - num].lowPrice) / 2f * (this.m_kData[i].highPrice - this.m_kData[i].lowPrice);
				}
			}
			if (num2 <= this.m_kData.Length && num2 > 0)
			{
				IndicatorBase.Average(num, this.m_kData.Length, num2, array, destination);
			}
		}
	}
}
