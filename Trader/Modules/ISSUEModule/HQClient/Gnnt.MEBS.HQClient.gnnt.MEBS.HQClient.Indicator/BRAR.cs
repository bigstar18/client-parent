using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class BRAR : IndicatorBase
	{
		private readonly int[] m_iParam = new int[]
		{
			26
		};
		public BRAR(IndicatorPos pos, int Precision) : base(pos, Precision)
		{
			this.m_strIndicatorName = "BRAR";
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
				"AR",
				"BR"
			};
			this.m_iPrecision = 2;
		}
		public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
		{
			base.Paint(g, rc, data);
			this.Calculate();
			this.m_max = -10000f;
			this.m_min = 10000f;
			base.GetValueMaxMin(this.m_data[0], this.m_iParam[0] + 1);
			base.GetValueMaxMin(this.m_data[1], this.m_iParam[0] + 1);
			base.DrawCoordinate(g, 2);
			base.DrawLine(g, this.m_data[0], this.m_iParam[0] + 1, SetInfo.RHColor.clIndicator[0]);
			base.DrawLine(g, this.m_data[1], this.m_iParam[0] + 1, SetInfo.RHColor.clIndicator[1]);
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
			this.GetAR(this.m_iParam[0], this.m_data[0]);
			this.GetBR(this.m_iParam[0], this.m_data[1]);
		}
		private void GetAR(int n, float[] ar)
		{
			if (this.m_kData.Length < n)
			{
				return;
			}
			float num2;
			float num = num2 = 0f;
			for (int i = 1; i < n; i++)
			{
				num2 += this.m_kData[i].highPrice - this.m_kData[i].openPrice;
				num += this.m_kData[i].openPrice - this.m_kData[i].lowPrice;
			}
			float num3 = 0f;
			for (int i = n; i < this.m_kData.Length; i++)
			{
				num2 += this.m_kData[i].highPrice - this.m_kData[i].openPrice;
				num += this.m_kData[i].openPrice - this.m_kData[i].lowPrice;
				ar[i] = num3;
				if (num != 0f)
				{
					ar[i] = num2 / num * 100f;
				}
				num3 = ar[i];
				int num4 = i - n + 1;
				num2 -= this.m_kData[num4].highPrice - this.m_kData[num4].openPrice;
				num -= this.m_kData[num4].openPrice - this.m_kData[num4].lowPrice;
			}
		}
		private void GetBR(int n, float[] br)
		{
			if (this.m_kData.Length < n)
			{
				return;
			}
			float num2;
			float num = num2 = 0f;
			for (int i = 1; i < n; i++)
			{
				float num3 = this.m_kData[i].highPrice - this.m_kData[i - 1].closePrice;
				num2 += ((num3 <= 0f) ? 0f : num3);
				num3 = this.m_kData[i - 1].closePrice - this.m_kData[i].lowPrice;
				num += ((num3 <= 0f) ? 0f : num3);
			}
			float num4 = 0f;
			for (int i = n; i < this.m_kData.Length; i++)
			{
				float num3 = this.m_kData[i].highPrice - this.m_kData[i - 1].closePrice;
				num2 += ((num3 <= 0f) ? 0f : num3);
				num3 = this.m_kData[i - 1].closePrice - this.m_kData[i].lowPrice;
				num += ((num3 <= 0f) ? 0f : num3);
				br[i] = num4;
				if (num != 0f)
				{
					br[i] = num2 / num * 100f;
				}
				num4 = br[i];
				int num5 = i - n + 1;
				num3 = this.m_kData[num5].highPrice - this.m_kData[num5 - 1].closePrice;
				num2 -= ((num3 <= 0f) ? 0f : num3);
				num3 = this.m_kData[num5 - 1].closePrice - this.m_kData[num5].lowPrice;
				num -= ((num3 <= 0f) ? 0f : num3);
			}
		}
	}
}
