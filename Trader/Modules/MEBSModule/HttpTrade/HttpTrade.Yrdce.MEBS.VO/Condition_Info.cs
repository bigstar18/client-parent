using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class Condition_Info
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
		private string EMDATE;
		private string CONEXPIRE;
		private string CONCO_I;
		private string CONPRICE;
		private string CONTYPE;
		private string CONOPERATOR;
		private string SESSIONID;
		public long Surplus
		{
			get
			{
				return Tools.StrToLong(this.BAL);
			}
		}
		public long LPrice
		{
			get
			{
				return Tools.StrToLong(this.L_P);
			}
		}
		public string RevokeTime
		{
			get
			{
				return this.WD_T;
			}
		}
		public short CFlag
		{
			get
			{
				return Tools.StrToShort(this.C_F);
			}
		}
		public short BillTradeType
		{
			get
			{
				return Tools.StrToShort(this.B_T_T);
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
		public long OrderNO
		{
			get
			{
				return Tools.StrToLong(this.OR_N);
			}
		}
		public string CommodityID
		{
			get
			{
				return this.CO_I;
			}
		}
		public string Codition_CommodityID
		{
			get
			{
				return this.CONCO_I;
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
		public short ConditionType
		{
			get
			{
				return Tools.StrToShort(this.CONTYPE);
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
		public double ConditionPrice
		{
			get
			{
				return Tools.StrToDouble(this.CONPRICE);
			}
		}
		public string PrePareTime
		{
			get
			{
				return this.EMDATE;
			}
		}
		public string OrderTime
		{
			get
			{
				return this.TIME;
			}
		}
		public string MatureTime
		{
			get
			{
				return this.CONEXPIRE;
			}
		}
		public string OrderState
		{
			get
			{
				return this.STA;
			}
		}
		public long SessionID
		{
			get
			{
				return Tools.StrToLong(this.SESSIONID);
			}
		}
		public short ConOperator
		{
			get
			{
				return Tools.StrToShort(this.CONOPERATOR);
			}
		}
	}
}
