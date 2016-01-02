using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class BOLL : KLine
	{
		private readonly int[] m_iParam = new int[]
		{
			10
		};
		public BOLL(IndicatorPos pos, int iPrecision, HQForm hqForm) : base(pos, 1, iPrecision, hqForm)
		{
			this.m_strIndicatorName = "BOLL";
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
				"MID",
				"UPPER",
				"LOWER"
			};
			this.m_iPrecision = 2;
		}
		public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
		{
			this.m_kData = data;
			this.Calculate();
			base.Paint(g, rc, data);
			for (int i = 0; i < 3; i++)
			{
				base.DrawLine(g, this.m_data[i], this.m_iParam[0] * 2 - 2, SetInfo.RHColor.clIndicator[i]);
			}
		}
		protected override void GetMaxMin()
		{
			base.GetMaxMin();
			for (int i = 0; i < 3; i++)
			{
				base.GetValueMaxMin(this.m_data[i], this.m_iParam[0] * 2 - 2);
			}
		}
		public override void Calculate()
		{
			this.m_data = new float[3][];
			if (this.m_kData == null || this.m_kData.Length <= 0)
			{
				return;
			}
			int num = this.m_iParam[0];
			if (num > this.m_kData.Length || num < 1 || num + num - 2 >= this.m_kData.Length)
			{
				return;
			}
			for (int i = 0; i < 3; i++)
			{
				this.m_data[i] = new float[this.m_kData.Length];
			}
			float[] array = this.m_data[0];
			float[] array2 = this.m_data[1];
			float[] array3 = this.m_data[2];
			float num2 = 0f;
			base.AverageClose(num, array);
			for (int i = num - 1; i < num + num - 2; i++)
			{
				float num3 = this.m_kData[i].closePrice - array[i];
				num2 += num3 * num3;
			}
			float num4 = 0f;
			for (int i = num + num - 2; i < this.m_kData.Length; i++)
			{
				num2 -= num4;
				float num3 = this.m_kData[i].closePrice - array[i];
				num2 += num3 * num3;
				num3 = (float)Math.Sqrt((double)(num2 / (float)num)) * 1.805f;
				array2[i] = array[i] + num3;
				array3[i] = array[i] - num3;
				num3 = this.m_kData[i - num + 1].closePrice - array[i - num + 1];
				num4 = num3 * num3;
			}
		}
	}
}
