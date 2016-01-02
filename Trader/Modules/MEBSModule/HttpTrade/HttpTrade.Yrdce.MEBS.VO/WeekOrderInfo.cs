using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
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
			set
			{
				this.OR_N = value.ToString();
			}
		}
		public string Time
		{
			get
			{
				return this.TIME;
			}
			set
			{
				this.TIME = value;
			}
		}
		public short State
		{
			get
			{
				return Tools.StrToShort(this.STA);
			}
			set
			{
				this.STA = value.ToString();
			}
		}
		public short BuySell
		{
			get
			{
				return Tools.StrToShort(this.TYPE);
			}
			set
			{
				this.TYPE = value.ToString();
			}
		}
		public short SettleBasis
		{
			get
			{
				return Tools.StrToShort(this.SE_F);
			}
			set
			{
				this.SE_F = value.ToString();
			}
		}
		public string TraderID
		{
			get
			{
				return this.TR_I;
			}
			set
			{
				this.TR_I = value;
			}
		}
		public string FirmID
		{
			get
			{
				return this.FI_I;
			}
			set
			{
				this.FI_I = value;
			}
		}
		public string CustomerID
		{
			get
			{
				return this.CU_I;
			}
			set
			{
				this.CU_I = value;
			}
		}
		public string CommodityID
		{
			get
			{
				return this.CO_I;
			}
			set
			{
				this.CO_I = value;
			}
		}
		public double OrderPrice
		{
			get
			{
				return Tools.StrToDouble(this.PRI);
			}
			set
			{
				this.PRI = value.ToString();
			}
		}
		public long OrderQuantity
		{
			get
			{
				return Tools.StrToLong(this.QTY);
			}
			set
			{
				this.QTY = value.ToString();
			}
		}
		public double Balance
		{
			get
			{
				return Tools.StrToDouble(this.BAL);
			}
			set
			{
				this.BAL = value.ToString();
			}
		}
		public double LPrice
		{
			get
			{
				return Tools.StrToDouble(this.L_P);
			}
			set
			{
				this.L_P = value.ToString();
			}
		}
		public string WithDrawTime
		{
			get
			{
				return this.WD_T;
			}
			set
			{
				this.WD_T = value;
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
				this.C_F = value.ToString();
			}
		}
		public short BillTradeType
		{
			get
			{
				return Tools.StrToShort(this.B_T_T);
			}
			set
			{
				this.B_T_T = value.ToString();
			}
		}
	}
}
