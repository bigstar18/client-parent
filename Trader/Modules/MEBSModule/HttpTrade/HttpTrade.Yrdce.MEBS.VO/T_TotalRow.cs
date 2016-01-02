using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class T_TotalRow
	{
		private string RESPONSENAME;
		private string TOTALNUM;
		private string TOTALQTY;
		private string UNTRADEQTY;
		private string TOTALLIQPL;
		private string TOTALCOMM;
		public string ResponseName
		{
			get
			{
				return this.RESPONSENAME;
			}
		}
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
		public int UnTradeQty
		{
			get
			{
				return Tools.StrToInt(this.UNTRADEQTY);
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
