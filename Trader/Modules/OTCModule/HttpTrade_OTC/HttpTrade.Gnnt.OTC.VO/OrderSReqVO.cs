using HttpTrade.Gnnt.OTC.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class OrderSReqVO : ReqVO
	{
		private string USER_ID;
		private string BUY_SELL;
		private string COMMODITY_ID;
		private string PRICE;
		private string QTY;
		private string SETTLE_BASIS;
		private string DOT_DIFF;
		private string CLOSEMODE;
		private string HOLDING_ID;
		private string OTHER_ID;
		private string SESSION_ID;
		private string STOP_LOSS;
		private string STOP_PROFIT;
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
		public short BuySell
		{
			get
			{
				return Tools.StrToShort(this.BUY_SELL);
			}
			set
			{
				this.BUY_SELL = value.ToString();
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
		public double Price
		{
			get
			{
				return Tools.StrToDouble(this.PRICE);
			}
			set
			{
				this.PRICE = value.ToString();
			}
		}
		public long Quantity
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
		public short SettleBasis
		{
			get
			{
				return Tools.StrToShort(this.SETTLE_BASIS);
			}
			set
			{
				this.SETTLE_BASIS = value.ToString();
			}
		}
		public short DotDiff
		{
			get
			{
				return Tools.StrToShort(this.DOT_DIFF);
			}
			set
			{
				this.DOT_DIFF = value.ToString();
			}
		}
		public short CloseMode
		{
			get
			{
				return Tools.StrToShort(this.CLOSEMODE);
			}
			set
			{
				this.CLOSEMODE = value.ToString();
			}
		}
		public long HoldingID
		{
			get
			{
				return Tools.StrToLong(this.HOLDING_ID);
			}
			set
			{
				this.HOLDING_ID = value.ToString();
			}
		}
		public string OtherID
		{
			get
			{
				return this.OTHER_ID;
			}
			set
			{
				this.OTHER_ID = value;
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
		public OrderSReqVO()
		{
			base.Name = ProtocolName.order_s;
		}
	}
}
