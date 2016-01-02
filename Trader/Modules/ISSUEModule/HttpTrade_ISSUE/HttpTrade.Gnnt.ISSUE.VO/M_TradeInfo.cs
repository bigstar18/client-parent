using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class M_TradeInfo
	{
		private string TR_N;
		private string OR_N;
		private string TI;
		private string TY;
		private string SE_F;
		private string TR_I;
		private string FI_I;
		private string CU_I;
		private string CO_I;
		private string PR;
		private string QTY;
		private string O_PR;
		private string LIQPL;
		private string COMM;
		private string S_TR_N;
		private string A_TR_N;
		private string TR_T;
		public long TradeNO
		{
			get
			{
				return Tools.StrToLong(this.TR_N);
			}
		}
		public long OrderNO
		{
			get
			{
				return Tools.StrToLong(this.OR_N);
			}
		}
		public string TradeTime
		{
			get
			{
				return this.TI;
			}
		}
		public short BuySell
		{
			get
			{
				return Tools.StrToShort(this.TY);
			}
		}
		public short SettleBasis
		{
			get
			{
				return Tools.StrToShort(this.SE_F);
			}
		}
		public string TraderID
		{
			get
			{
				return this.TR_I;
			}
		}
		public string FirmID
		{
			get
			{
				return this.FI_I;
			}
		}
		public string CustomerID
		{
			get
			{
				return this.CU_I;
			}
		}
		public string CommodityID
		{
			get
			{
				return this.CO_I;
			}
		}
		public double TradePrice
		{
			get
			{
				return Tools.StrToDouble(this.PR);
			}
		}
		public long TradeQuantity
		{
			get
			{
				return Tools.StrToLong(this.QTY);
			}
		}
		public double TransferPrice
		{
			get
			{
				return Tools.StrToDouble(this.O_PR);
			}
		}
		public double TransferPL
		{
			get
			{
				return Tools.StrToDouble(this.LIQPL);
			}
		}
		public double Comm
		{
			get
			{
				return Tools.StrToDouble(this.COMM);
			}
		}
		public long STradeNO
		{
			get
			{
				return Tools.StrToLong(this.S_TR_N);
			}
		}
		public long ATradeNO
		{
			get
			{
				return Tools.StrToLong(this.A_TR_N);
			}
		}
		public short TradeType
		{
			get
			{
				return Tools.StrToShort(this.TR_T);
			}
		}
	}
}
