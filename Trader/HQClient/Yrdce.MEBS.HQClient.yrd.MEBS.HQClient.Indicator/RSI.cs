using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class RSI : IndicatorBase
	{
		private readonly int[] m_iParam = new int[]
		{
			6,
			12,
			24
		};
		public RSI(IndicatorPos pos, int Precision) : base(pos, Precision)
		{
			this.m_strIndicatorName = string.Concat(new object[]
			{
				"RSI(",
				this.m_iParam[0],
				",",
				this.m_iParam[1],
				",",
				this.m_iParam[2],
				")"
			});
			this.m_strParamName = new string[]
			{
				"RSI1",
				"RSI2",
				"RSI3"
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
			this.m_max = 0f;
			this.m_min = 10000f;
			for (int i = 0; i < 3; i++)
			{
				base.GetValueMaxMin(this.m_data[i], this.m_iParam[i]);
			}
			base.DrawCoordinate(g, 2);
			for (int j = 0; j < 3; j++)
			{
				base.DrawLine(g, this.m_data[j], this.m_iParam[j], SetInfo.RHColor.clIndicator[j]);
			}
		}
		public override void Calculate()
		{
			this.m_data = new float[3][];
			this.m_data[0] = new float[this.m_kData.Length];
			this.m_data[1] = new float[this.m_kData.Length];
			this.m_data[2] = new float[this.m_kData.Length];
			if (this.m_kData == null || this.m_kData.Length == 0)
			{
				return;
			}
			for (int i = 0; i < 3; i++)
			{
				this.GetRSI(this.m_iParam[i], this.m_data[i]);
			}
		}
		private void GetRSI(int n, float[] rsi)
		{
			if (n > this.m_kData.Length || n < 1)
			{
				return;
			}
			float num = 0f;
			float num2 = 0f;
			for (int i = 1; i < n; i++)
			{
				if (this.m_kData[i].closePrice > this.m_kData[i - 1].closePrice)
				{
					num += this.m_kData[i].closePrice - this.m_kData[i - 1].closePrice;
				}
				else
				{
					num2 += this.m_kData[i - 1].closePrice - this.m_kData[i].closePrice;
				}
			}
			if (num + num2 == 0f)
			{
				rsi[n - 1] = 50f;
			}
			else
			{
				rsi[n - 1] = num / (num + num2) * 100f;
			}
			float num4;
			float num3 = num4 = 0f;
			for (int i = n; i < this.m_kData.Length; i++)
			{
				num -= num4;
				num2 -= num3;
				if (this.m_kData[i].closePrice > this.m_kData[i - 1].closePrice)
				{
					num += this.m_kData[i].closePrice - this.m_kData[i - 1].closePrice;
				}
				else
				{
					num2 += this.m_kData[i - 1].closePrice - this.m_kData[i].closePrice;
				}
				if (num + num2 == 0f)
				{
					rsi[i] = rsi[i - 1];
				}
				else
				{
					rsi[i] = num / (num + num2) * 100f;
				}
				num3 = (num4 = 0f);
				if (this.m_kData[i - n + 1].closePrice > this.m_kData[i - n].closePrice)
				{
					num4 = this.m_kData[i - n + 1].closePrice - this.m_kData[i - n].closePrice;
				}
				else
				{
					num3 = this.m_kData[i - n].closePrice - this.m_kData[i - n + 1].closePrice;
				}
			}
		}
	}
}
