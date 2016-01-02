using System;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class CodeTable
	{
		public string marketID;
		public string sName;
		public string[] sPinyin;
		public int status;
		public int[] tradeSecNo = new int[0];
		public float mPrice;
		public float fUnit;
	}
}
