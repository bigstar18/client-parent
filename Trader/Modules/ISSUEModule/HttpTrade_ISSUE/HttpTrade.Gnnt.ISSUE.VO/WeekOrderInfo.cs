using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class WeekOrderInfo
	{
		private string OR_N;
		private string TIME;
		private string STA;
		private string TYPE;
		private string SE_F;
		private string TR_I;
		private string FI_I;
		private string CU_I;
		private string CO_I;
		private string PRI;
		private string QTY;
		private string BAL;
		private string L_P;
		private string WD_T;
		private string C_F;
		private string B_T_T;
		public long OrderNO
		{
			get
			{
				return Tools.StrToLong(this.OR_N);
			}
		}
		public string Time
		{
			get
			{
				return this.TIME;
			}
		}
		public short State
		{
			get
			{
				return Tools.StrToShort(this.STA);
			}
		}
		public short BuySell
		{
			get
			{
				return Tools.StrToShort(this.TYPE);
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
		public double OrderPrice
		{
			get
			{
				return Tools.StrToDouble(this.PRI);
			}
		}
		public long OrderQuantity
		{
			get
			{
				return Tools.StrToLong(this.QTY);
			}
		}
		public double Balance
		{
			get
			{
				return Tools.StrToDouble(this.BAL);
			}
		}
		public double LPrice
		{
			get
			{
				return Tools.StrToDouble(this.L_P);
			}
		}
		public string WithDrawTime
		{
			get
			{
				return this.WD_T;
			}
		}
		public short CBasis
		{
			get
			{
				return Tools.StrToShort(this.C_F);
			}
			set
			{
			}
		}
		public short BillTradeType
		{
			get
			{
				return Tools.StrToShort(this.B_T_T);
			}
		}
	}
}
