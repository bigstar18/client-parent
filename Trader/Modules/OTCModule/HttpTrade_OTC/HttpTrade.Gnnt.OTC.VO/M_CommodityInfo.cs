using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class M_CommodityInfo
	{
		private string MA_I;
		private string CO_I;
		private string CO_N;
		private string STA;
		private string CT_S;
		private string SPREAD;
		private string SP_U;
		private string SP_D;
		private string MA_A;
		private YanQiList YARS;
		private string MA_V;
		private string PR_C;
		private string FE_A;
		private string SFE_A;
		private string TM_SET;
		private string STM_SET;
		private string BRDID;
		private string B_T_M;
		private string YA_A;
		private string FEE_V;
		private string FEE_T;
		private string P_MIN_H;
		private string P_M_H;
		private string M_H;
		private string W_D_T_P;
		private string W_D_S_L_P;
		private string W_D_S_P_P;
		private string B_O_P;
		private string B_L_P;
		private string B_X_O_P;
		private string B_S_L;
		private string B_S_P;
		private string S_O_P;
		private string S_L_P;
		private string S_X_O_P;
		private string S_S_L;
		private string S_S_P;
		private string B_P_D_D;
		private string S_P_D_D;
		private string X_O_B_D_D;
		private string X_O_S_D_D;
		private string U_O_D_D_MIN;
		private string U_O_D_D_MAX;
		private string U_O_D_D_DF;
		private string ORDER_NUM;
		private string B_J_H;
		private string STOP_L_P;
		private string STOP_P_P;
		private string CON_U;
		public short DeferAmountType
		{
			get
			{
				return Tools.StrToShort(this.YA_A);
			}
		}
		public double FEE__V
		{
			get
			{
				return Tools.StrToDouble(this.FEE_V);
			}
		}
		public string FEE__T
		{
			get
			{
				return this.FEE_T;
			}
		}
		public long P__MIN__H
		{
			get
			{
				return Tools.StrToLong(this.P_MIN_H);
			}
		}
		public long P__M__H
		{
			get
			{
				return Tools.StrToLong(this.P_M_H);
			}
		}
		public long MaxHolding
		{
			get
			{
				return Tools.StrToLong(this.M_H);
			}
		}
		public bool W__D__T__P
		{
			get
			{
				return Convert.ToBoolean(Convert.ToInt32(this.W_D_T_P));
			}
		}
		public bool W__D__S__L__P
		{
			get
			{
				return Convert.ToBoolean(Convert.ToInt32(this.W_D_S_L_P));
			}
		}
		public bool W__D__S__P__P
		{
			get
			{
				return Convert.ToBoolean(Convert.ToInt32(this.W_D_S_P_P));
			}
		}
		public bool B__O__P
		{
			get
			{
				return Convert.ToBoolean(Convert.ToInt32(this.B_O_P));
			}
		}
		public bool B__L__P
		{
			get
			{
				return Convert.ToBoolean(Convert.ToInt32(this.B_L_P));
			}
		}
		public bool B__X__O__P
		{
			get
			{
				return Convert.ToBoolean(Convert.ToInt32(this.B_X_O_P));
			}
		}
		public bool B__S__L
		{
			get
			{
				return Convert.ToBoolean(Convert.ToInt32(this.B_S_L));
			}
		}
		public bool B__S__P
		{
			get
			{
				return Convert.ToBoolean(Convert.ToInt32(this.B_S_P));
			}
		}
		public bool S__O__P
		{
			get
			{
				return Convert.ToBoolean(Convert.ToInt32(this.S_O_P));
			}
		}
		public bool S__L__P
		{
			get
			{
				return Convert.ToBoolean(Convert.ToInt32(this.S_L_P));
			}
		}
		public bool S__X__O__P
		{
			get
			{
				return Convert.ToBoolean(Convert.ToInt32(this.S_X_O_P));
			}
		}
		public bool S__S__L
		{
			get
			{
				return Convert.ToBoolean(Convert.ToInt32(this.S_S_L));
			}
		}
		public bool S__S__P
		{
			get
			{
				return Convert.ToBoolean(Convert.ToInt32(this.S_S_P));
			}
		}
		public double B__P__D__D
		{
			get
			{
				return Tools.StrToDouble(this.B_P_D_D);
			}
		}
		public double S__P__D__D
		{
			get
			{
				return Tools.StrToDouble(this.S_P_D_D);
			}
		}
		public double X__O__B__D__D
		{
			get
			{
				return Tools.StrToDouble(this.X_O_B_D_D);
			}
		}
		public double X__O__S__D__D
		{
			get
			{
				return Tools.StrToDouble(this.X_O_S_D_D);
			}
		}
		public double U__O__D__D__MIN
		{
			get
			{
				return Tools.StrToDouble(this.U_O_D_D_MIN);
			}
		}
		public double U__O__D__D__MAX
		{
			get
			{
				return Tools.StrToDouble(this.U_O_D_D_MAX);
			}
		}
		public double U__O__D__D__DF
		{
			get
			{
				return Tools.StrToDouble(this.U_O_D_D_DF);
			}
		}
		public long OrderNum
		{
			get
			{
				return Tools.StrToLong(this.ORDER_NUM);
			}
		}
		public double B__J__H
		{
			get
			{
				return Tools.StrToDouble(this.B_J_H);
			}
		}
		public string MarketID
		{
			get
			{
				return this.MA_I;
			}
		}
		public string CommodityID
		{
			get
			{
				return this.CO_I;
			}
		}
		public string CommodityName
		{
			get
			{
				return this.CO_N;
			}
		}
		public short Status
		{
			get
			{
				return Tools.StrToShort(this.STA);
			}
		}
		public double CtrtSize
		{
			get
			{
				return Tools.StrToDouble(this.CT_S);
			}
		}
		public double Spread
		{
			get
			{
				return Tools.StrToDouble(this.SPREAD);
			}
		}
		public double SpreadUp
		{
			get
			{
				return Tools.StrToDouble(this.SP_U);
			}
		}
		public double SpreadDown
		{
			get
			{
				return Tools.StrToDouble(this.SP_D);
			}
		}
		public short MarginType
		{
			get
			{
				return Tools.StrToShort(this.MA_A);
			}
		}
		public double MarginValue
		{
			get
			{
				return Tools.StrToDouble(this.MA_V);
			}
		}
		public YanQiList YanQiList
		{
			get
			{
				return this.YARS;
			}
		}
		public double PrevClear
		{
			get
			{
				return Tools.StrToDouble(this.PR_C);
			}
		}
		public short CommType
		{
			get
			{
				return Tools.StrToShort(this.FE_A);
			}
		}
		public short DeliveryCommType
		{
			get
			{
				return Tools.StrToShort(this.SFE_A);
			}
		}
		public double DeliveryBComm
		{
			get
			{
				return Tools.StrToDouble(this.TM_SET);
			}
		}
		public double DeliverySComm
		{
			get
			{
				return Tools.StrToDouble(this.STM_SET);
			}
		}
		public string VarietyID
		{
			get
			{
				return this.BRDID;
			}
		}
		public string CommodityUnit
		{
			get
			{
				return this.CON_U;
			}
		}
		public short TradeMode
		{
			get
			{
				return Tools.StrToShort(this.B_T_M);
			}
		}
		public double STOP__L__P
		{
			get
			{
				return Tools.StrToDouble(this.STOP_L_P);
			}
		}
		public double STOP__P__P
		{
			get
			{
				return Tools.StrToDouble(this.STOP_P_P);
			}
		}
	}
}
