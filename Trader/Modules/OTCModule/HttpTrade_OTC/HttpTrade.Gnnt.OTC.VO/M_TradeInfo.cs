using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
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
		private string CO_I;
		private string PR;
		private string QTY;
		private string LIQPL;
		private string COMM;
		private string H_P;
		private string OR_T;
		private string O_PR;
		private string TR_T;
		private string HL_N;
		private string OTHER_ID;
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
		public long HoldingNO
		{
			get
			{
				return Tools.StrToLong(this.HL_N);
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
		public string CommodityID
		{
			get
			{
				return this.CO_I;
			}
		}
		public double OpenPrice
		{
			get
			{
				return Tools.StrToDouble(this.O_PR);
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
		public double HoldingPrice
		{
			get
			{
				return Tools.StrToDouble(this.H_P);
			}
		}
		public string OrderTime
		{
			get
			{
				return this.OR_T;
			}
		}
		public short TradeType
		{
			get
			{
				return Tools.StrToShort(this.TR_T);
			}
		}
		public string OtherID
		{
			get
			{
				return this.OTHER_ID;
			}
		}
	}
}
