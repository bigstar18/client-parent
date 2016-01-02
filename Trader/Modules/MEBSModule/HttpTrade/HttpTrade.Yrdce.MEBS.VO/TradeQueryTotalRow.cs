using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class TradeQueryTotalRow
	{
		private string TOTALNUM;
		private string TOTALQTY;
		private string TOTALLIQPL;
		private string TOTALCOMM;
		public int TotalNum
		{
			get
			{
				return Tools.StrToInt(this.TOTALNUM);
			}
		}
		public int TotalQty
		{
			get
			{
				return Tools.StrToInt(this.TOTALQTY);
			}
		}
		public double TotalLiqpl
		{
			get
			{
				return Tools.StrToDouble(this.TOTALLIQPL);
			}
		}
		public double TotalComm
		{
			get
			{
				return Tools.StrToDouble(this.TOTALCOMM);
			}
		}
	}
}
