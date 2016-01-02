using HttpTrade.Gnnt.OTC.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class SetLossProfitReqVO : ReqVO
	{
		private string USER_ID;
		private string H_ID;
		private string STOP_LOSS;
		private string STOP_PROFIT;
		private string SESSION_ID;
		private string COMMODITY_ID;
		private string TY;
		private string A_N;
		private string P_P;
		public string AgencyNo
		{
			get
			{
				return this.A_N;
			}
			set
			{
				this.A_N = value;
			}
		}
		public string AgencyPhonePassword
		{
			get
			{
				return this.P_P;
			}
			set
			{
				this.P_P = value;
			}
		}
		public string CommodityID
		{
			get
			{
				return this.COMMODITY_ID;
			}
			set
			{
				this.COMMODITY_ID = value;
			}
		}
		public string BuySellType
		{
			get
			{
				return this.TY;
			}
			set
			{
				this.TY = value;
			}
		}
		public string UserID
		{
			get
			{
				return this.USER_ID;
			}
			set
			{
				this.USER_ID = value;
			}
		}
		public long Holding_ID
		{
			get
			{
				return Tools.StrToLong(this.H_ID);
			}
			set
			{
				this.H_ID = value.ToString();
			}
		}
		public double StopLoss
		{
			get
			{
				return Tools.StrToDouble(this.STOP_LOSS);
			}
			set
			{
				this.STOP_LOSS = value.ToString();
			}
		}
		public double StopProfit
		{
			get
			{
				return Tools.StrToDouble(this.STOP_PROFIT);
			}
			set
			{
				this.STOP_PROFIT = value.ToString();
			}
		}
		public long SessionID
		{
			get
			{
				return Tools.StrToLong(this.SESSION_ID);
			}
			set
			{
				this.SESSION_ID = value.ToString();
			}
		}
		public SetLossProfitReqVO()
		{
			base.Name = ProtocolName.set_loss_profit;
		}
	}
}
