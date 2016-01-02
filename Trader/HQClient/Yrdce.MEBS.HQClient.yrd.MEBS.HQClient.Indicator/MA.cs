using Gnnt.MEBS.HQClient.gnnt.ClientForms;
using Gnnt.MEBS.HQModel;
using Gnnt.MEBS.HQModel.DataVO;
using System;
using System.Drawing;
namespace Gnnt.MEBS.HQClient.gnnt.MEBS.HQClient.Indicator
{
	internal class MA : KLine
	{
		private readonly int[] m_iParam = new int[]
		{
			5,
			10,
			20,
			30,
			60
		};
		public MA(IndicatorPos pos, int iLineType, int iPrecision, HQForm hqForm) : base(pos, iLineType, iPrecision, hqForm)
		{
			this.m_strIndicatorName = "MA";
			this.m_strParamName = new string[this.m_iParam.Length];
			for (int i = 0; i < this.m_iParam.Length; i++)
			{
				this.m_strParamName[i] = "MA" + this.m_iParam[i];
			}
		}
		public override void Paint(Graphics g, Rectangle rc, KLineData[] data)
		{
			this.m_kData = data;
			this.Calculate();
			base.Paint(g, rc, data);
			for (int i = 0; i < this.m_iParam.Length; i++)
			{
				base.DrawLine(g, this.m_data[i], this.m_iParam[i] - 1, SetInfo.RHColor.clIndicator[i]);
			}
		}
		public override void Calculate()
		{
			this.m_data = new float[this.m_iParam.Length][];
			if (this.m_kData == null || this.m_kData.Length == 0)
			{
				return;
			}
			for (int i = 0; i < this.m_iParam.Length; i++)
			{
				this.m_data[i] = new float[this.m_kData.Length];
				base.AverageClose(this.m_iParam[i], this.m_data[i]);
			}
		}
		protected override void GetMaxMin()
		{
			base.GetMaxMin();
		}
	}
}
