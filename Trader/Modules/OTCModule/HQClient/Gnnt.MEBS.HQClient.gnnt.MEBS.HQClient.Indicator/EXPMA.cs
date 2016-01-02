using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class EXPMA : KLine
	{
		private readonly int[] m_iParam = new int[]
		{
			5,
			20,
			50
		};
		public EXPMA(IndicatorPos pos, int iPrecision, HQForm hqForm) : base(pos, 0, iPrecision, hqForm)
		{
			this.m_strIndicatorName = "EXPMA";
			this.m_strParamName = new string[this.m_iParam.Length];
			for (int i = 0; i < this.m_iParam.Length; i++)
			{
				this.m_strParamName[i] = "MA" + this.m_iParam[i];
			}
		}
		public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
		{
			if (data == null || data.Length == 0)
			{
				return;
			}
			this.m_kData = data;
			this.Calculate();
			base.Paint(g, rc, data);
			for (int i = 0; i < this.m_iParam.Length; i++)
			{
				base.DrawLine(g, this.m_data[i], 0, SetInfo.RHColor.clIndicator[i]);
			}
		}
		public override void Calculate()
		{
			this.m_data = new float[this.m_iParam.Length][];
			for (int i = 0; i < this.m_iParam.Length; i++)
			{
				this.m_data[i] = new float[this.m_kData.Length];
				this.GetEXPMA(this.m_iParam[i], this.m_data[i]);
			}
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
		protected override void GetMaxMin()
		{
			base.GetMaxMin();
			for (int i = 0; i < this.m_iParam.Length; i++)
			{
				base.GetValueMaxMin(this.m_data[i], this.m_iParam[i] - 1);
			}
		}
	}
}
