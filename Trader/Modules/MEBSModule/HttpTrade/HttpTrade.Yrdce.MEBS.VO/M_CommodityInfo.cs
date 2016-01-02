using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class M_CommodityInfo
	{
		private string MA_I;
		private string CO_I;
		private string CO_N;
		private string L_SET;
		private string STA;
		private string CT_S;
		private string SPREAD;
		private string SP_U;
		private string SP_D;
		private string MA_A;
		private string BMA;
		private string SMA;
		private string BAS;
		private string SAS;
		private string PR_C;
		private string FE_A;
		private string TE_T;
		private string STE_T;
		private string BCHFE;
		private string SCHFE;
		private string BCTFE;
		private string SCTFE;
		private string BCFFE;
		private string SCFFE;
		private string SFE_A;
		private string TM_SET;
		private string STM_SET;
		private string BRDID;
		private string B_T_M;
		private string MQ;
		private string MAXHOLDDAYS;
		private string TR;
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
		public string DeliveryDate
		{
			get
			{
				return this.L_SET;
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
		public double BMargin
		{
			get
			{
				return Tools.StrToDouble(this.BMA);
			}
		}
		public double SMargin
		{
			get
			{
				return Tools.StrToDouble(this.SMA);
			}
		}
		public double BMargin_g
		{
			get
			{
				return Tools.StrToDouble(this.BAS);
			}
		}
		public double SMargin_g
		{
			get
			{
				return Tools.StrToDouble(this.SAS);
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
		public double BOpenComm
		{
			get
			{
				return Tools.StrToDouble(this.TE_T);
			}
		}
		public double SOpenComm
		{
			get
			{
				return Tools.StrToDouble(this.STE_T);
			}
		}
		public double BTHHComm
		{
			get
			{
				return Tools.StrToDouble(this.BCHFE);
			}
		}
		public double STHHComm
		{
			get
			{
				return Tools.StrToDouble(this.SCHFE);
			}
		}
		public double BTTHComm
		{
			get
			{
				return Tools.StrToDouble(this.BCTFE);
			}
		}
		public double STTHComm
		{
			get
			{
				return Tools.StrToDouble(this.SCTFE);
			}
		}
		public double BFTComm
		{
			get
			{
				return Tools.StrToDouble(this.BCFFE);
			}
		}
		public double SFTComm
		{
			get
			{
				return Tools.StrToDouble(this.SCFFE);
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
		public short TradeMode
		{
			get
			{
				return Tools.StrToShort(this.B_T_M);
			}
		}
		public double MinQty
		{
			get
			{
				return Tools.StrToDouble(this.MQ);
			}
		}
		public string MaxHoldDays
		{
			get
			{
				return this.MAXHOLDDAYS;
			}
		}
		public double TaxRate
		{
			get
			{
				return Tools.StrToDouble(this.TR);
			}
		}
	}
}
