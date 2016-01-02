using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class FirmInfo
	{
		private string FI;
		private string FN;
		private string TP;
		private string IF;
		private string IN_F;
		private string OU_F;
		private string HK_S;
		private string HK_B;
		private string IC;
		private string UC;
		private string IS;
		private string SG_F;
		private string OR_F;
		private string OT_F;
		private string FEE;
		private string BC_R;
		private string BC_U;
		private string BC_C;
		private string BC_D;
		private string SAF;
		private string OC;
		private string MV;
		private string UF;
		private string DQ;
		private string JYSQY;
		public string FirmID
		{
			get
			{
				return this.FI;
			}
		}
		public string FirmName
		{
			get
			{
				return this.FN;
			}
		}
		public short FirmTpye
		{
			get
			{
				return Tools.StrToShort(this.TP);
			}
		}
		public double InitFund
		{
			get
			{
				return Tools.StrToDouble(this.IF);
			}
		}
		public double InFund
		{
			get
			{
				return Tools.StrToDouble(this.IN_F);
			}
		}
		public double OutFund
		{
			get
			{
				return Tools.StrToDouble(this.OU_F);
			}
		}
		public double HKSell
		{
			get
			{
				return Tools.StrToDouble(this.HK_S);
			}
		}
		public double HKBuy
		{
			get
			{
				return Tools.StrToDouble(this.HK_B);
			}
		}
		public double CurFreezeFund
		{
			get
			{
				return Tools.StrToDouble(this.IC);
			}
		}
		public double CurUnfreezeFund
		{
			get
			{
				return Tools.StrToDouble(this.UC);
			}
		}
		public double IssuanceFee
		{
			get
			{
				return Tools.StrToDouble(this.IS);
			}
		}
		public double SGFreezeFund
		{
			get
			{
				return Tools.StrToDouble(this.SG_F);
			}
		}
		public double OrderFrozenFund
		{
			get
			{
				return Tools.StrToDouble(this.OR_F);
			}
		}
		public double OtherFrozenFund
		{
			get
			{
				return Tools.StrToDouble(this.OT_F);
			}
		}
		public double Fee
		{
			get
			{
				return Tools.StrToDouble(this.FEE);
			}
		}
		public double WareHouseRegFee
		{
			get
			{
				return Tools.StrToDouble(this.BC_R);
			}
		}
		public double WareHouseCancelFee
		{
			get
			{
				return Tools.StrToDouble(this.BC_U);
			}
		}
		public double TransferFee
		{
			get
			{
				return Tools.StrToDouble(this.BC_C);
			}
		}
		public double DistributionFee
		{
			get
			{
				return Tools.StrToDouble(this.BC_D);
			}
		}
		public double OtherFee
		{
			get
			{
				return Tools.StrToDouble(this.SAF);
			}
		}
		public double OtherChange
		{
			get
			{
				return Tools.StrToDouble(this.OC);
			}
		}
		public double MarketValue
		{
			get
			{
				return Tools.StrToDouble(this.MV);
			}
		}
		public double UsableFund
		{
			get
			{
				return Tools.StrToDouble(this.UF);
			}
		}
		public double DesirableFund
		{
			get
			{
				return Tools.StrToDouble(this.DQ);
			}
		}
		public double CurrentRight
		{
			get
			{
				return Tools.StrToDouble(this.JYSQY);
			}
		}
	}
}
