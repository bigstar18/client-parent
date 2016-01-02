using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class MIKE : KLine
	{
		private readonly int[] m_iParam = new int[]
		{
			12
		};
		public MIKE(IndicatorPos pos, int iPrecision, HQForm hqForm) : base(pos, 1, iPrecision, hqForm)
		{
			this.m_strIndicatorName = "MIKE";
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
				"WR",
				"MR",
				"SR",
				"WS",
				"MS",
				"SS"
			};
			this.m_iPrecision = 2;
		}
		public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
		{
			this.m_kData = data;
			this.Calculate();
			base.Paint(g, rc, data);
			for (int i = 0; i < 6; i++)
			{
				base.DrawLine(g, this.m_data[i], this.m_iParam[0], SetInfo.RHColor.clIndicator[i / 1]);
			}
		}
		protected override void GetMaxMin()
		{
			base.GetMaxMin();
			for (int i = 0; i < 6; i++)
			{
				base.GetValueMaxMin(this.m_data[i], this.m_iParam[0]);
			}
		}
		public override void Calculate()
		{
			this.m_data = new float[6][];
			if (this.m_kData == null || this.m_kData.Length <= 0)
			{
				return;
			}
			int num = this.m_iParam[0];
			if (num > this.m_kData.Length || num < 1)
			{
				return;
			}
			for (int i = 0; i < 6; i++)
			{
				this.m_data[i] = new float[this.m_kData.Length];
			}
			this.getN_DayLow(num, this.m_data[0]);
			this.getN_DayHigh(num, this.m_data[1]);
			for (int i = num - 1; i < this.m_kData.Length; i++)
			{
				float closePrice = this.m_kData[i].closePrice;
				float num2 = this.m_data[0][i];
				float num3 = this.m_data[1][i];
				float num4 = (closePrice + num3 + num2) / 3f;
				this.m_data[0][i] = num4 + (num4 - num2);
				this.m_data[1][i] = num4 + (num3 - num2);
				this.m_data[2][i] = 2f * num3 - num2;
				this.m_data[3][i] = num4 - (num3 - num4);
				this.m_data[4][i] = num4 - (num3 - num2);
				this.m_data[5][i] = 2f * num2 - num3;
			}
		}
		private void getN_DayLow(int iParam, float[] data)
		{
			if (this.m_kData == null || this.m_kData.Length == 0)
			{
				return;
			}
			if (iParam > this.m_kData.Length || iParam < 1)
			{
				return;
			}
			for (int i = iParam - 1; i < this.m_kData.Length; i++)
			{
				double num = (double)this.m_kData[i - iParam + 1].lowPrice;
				for (int j = i - iParam + 2; j <= i; j++)
				{
					if (num > (double)this.m_kData[j].lowPrice)
					{
						num = (double)this.m_kData[j].lowPrice;
					}
				}
				data[i] = (float)num;
			}
		}
		private void getN_DayHigh(int iParam, float[] data)
		{
			if (this.m_kData == null || this.m_kData.Length == 0)
			{
				return;
			}
			if (iParam > this.m_kData.Length || iParam < 1)
			{
				return;
			}
			for (int i = iParam - 1; i < this.m_kData.Length; i++)
			{
				double num = (double)this.m_kData[i - iParam + 1].highPrice;
				for (int j = i - iParam + 2; j <= i; j++)
				{
					if (num < (double)this.m_kData[j].lowPrice)
					{
						num = (double)this.m_kData[j].lowPrice;
					}
				}
				data[i] = (float)num;
			}
		}
	}
}
