using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class M_YanQi
	{
		private string STEP_V;
		private string STEP_L;
		private string YA_V;
		public long StepLow
		{
			get
			{
				return Tools.StrToLong(this.STEP_L);
			}
		}
		public long StepValue
		{
			get
			{
				return Tools.StrToLong(this.STEP_V);
			}
		}
		public double YanValue
		{
			get
			{
				return Tools.StrToDouble(this.YA_V);
			}
		}
	}
}
