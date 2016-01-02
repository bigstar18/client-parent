using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class M_OrderInfo
	{
		private string OR_N;
		private string TIME;
		private string STA;
		private string TYPE;
		private string SE_F;
		private string TR_I;
		private string FI_I;
		private string CO_I;
		private string PRI;
		private string QTY;
		private string WD_T;
		private string STOP_LOSS;
		private string STOP_PROFIT;
		private string O_T;
		private string O_F;
		private string H_NO;
		private string CO_ID = string.Empty;
		private string F_MAR;
		private string F_FEE;
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
				return Tools.StrToLong(this.H_NO);
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
		public string WithDrawTime
		{
			get
			{
				return this.WD_T;
			}
		}
		public double StopLoss
		{
			get
			{
				return Tools.StrToDouble(this.STOP_LOSS);
			}
		}
		public double StopProfit
		{
			get
			{
				return Tools.StrToDouble(this.STOP_PROFIT);
			}
		}
		public short OrderType
		{
			get
			{
				return Tools.StrToShort(this.O_T);
			}
		}
		public string OrderFirmID
		{
			get
			{
				return this.O_F;
			}
		}
		public string AgentID
		{
			get
			{
				return this.CO_ID;
			}
		}
		public double FrozenMargin
		{
			get
			{
				return Tools.StrToDouble(this.F_MAR);
			}
		}
		public double FrozenFee
		{
			get
			{
				return Tools.StrToDouble(this.F_FEE);
			}
		}
	}
}
