using HttpTrade.Gnnt.OTC.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class WithdrawLossProfitReqVO : ReqVO
	{
		private string USER_ID;
		private string H_ID;
		private string TYPE;
		private string SESSION_ID;
		private string COMMODITY_ID;
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
		public short WithdrawType
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
		public WithdrawLossProfitReqVO()
		{
			base.Name = ProtocolName.withdraw_loss_profit;
		}
	}
}
