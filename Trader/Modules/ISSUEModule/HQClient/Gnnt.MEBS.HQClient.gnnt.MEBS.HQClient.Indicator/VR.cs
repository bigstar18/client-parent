using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class VR : IndicatorBase
	{
		private readonly int[] m_iParam = new int[]
		{
			25,
			5
		};
		public VR(IndicatorPos pos, int Precision) : base(pos, Precision)
		{
			this.m_strIndicatorName = "VR";
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
				"MA"
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
			base.DrawCoordinate(g, 2);
			base.DrawLine(g, this.m_data[0], this.m_iParam[0], SetInfo.RHColor.clIndicator[0]);
			base.DrawLine(g, this.m_data[1], this.m_iParam[0] + this.m_iParam[1] - 1, SetInfo.RHColor.clIndicator[1]);
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
			int num = this.m_iParam[0];
			int n = this.m_iParam[1];
			float[] array = this.m_data[0];
			float[] destination = this.m_data[1];
			float num4;
			float num3;
			float num2 = num3 = (num4 = 0f);
			if (this.m_kData.Length < num)
			{
				return;
			}
			array[num - 2] = 100f;
			for (int i = 1; i < num; i++)
			{
				if (this.m_kData[i].closePrice == this.m_kData[i - 1].closePrice)
				{
					num4 += (float)this.m_kData[i].totalAmount;
				}
				else if (this.m_kData[i].closePrice > this.m_kData[i - 1].closePrice)
				{
					num3 += (float)this.m_kData[i].totalAmount;
				}
				else
				{
					num2 += (float)this.m_kData[i].totalAmount;
				}
			}
			if (num2 + num4 / 2f == 0f)
			{
				array[num - 1] = array[num - 2];
			}
			else
			{
				array[num - 1] = (num3 + num4 / 2f) / (num2 + num4 / 2f);
			}
			for (int j = num; j < this.m_kData.Length; j++)
			{
				if (this.m_kData[j].closePrice == this.m_kData[j - 1].closePrice)
				{
					num4 += (float)this.m_kData[j].totalAmount;
				}
				else if (this.m_kData[j].closePrice > this.m_kData[j - 1].closePrice)
				{
					num3 += (float)this.m_kData[j].totalAmount;
				}
				else
				{
					num2 += (float)this.m_kData[j].totalAmount;
				}
				if (num2 + num4 / 2f == 0f)
				{
					array[j] = array[j - 1];
				}
				else
				{
					array[j] = (num3 + num4 / 2f) / (num2 + num4 / 2f) * 100f;
				}
				if (this.m_kData[j - num + 1].closePrice == this.m_kData[j - num].closePrice)
				{
					num4 -= (float)this.m_kData[j - num + 1].totalAmount;
				}
				else if (this.m_kData[j - num + 1].closePrice > this.m_kData[j - num].closePrice)
				{
					num3 -= (float)this.m_kData[j - num + 1].totalAmount;
				}
				else
				{
					num2 -= (float)this.m_kData[j - num + 1].totalAmount;
				}
			}
			IndicatorBase.Average(num, this.m_kData.Length, n, array, destination);
		}
	}
}
