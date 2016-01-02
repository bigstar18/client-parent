using HttpTrade.Gnnt.OTC.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class OrderXReqVO : ReqVO
	{
		private string USER_ID;
		private string BUY_SELL;
		private string COMMODITY_ID;
		private string PRICE;
		private string QTY;
		private string STOP_LOSS;
		private string STOP_PROFIT;
		private string VALIDITY_TYPE;
		private string OTHER_ID;
		private string SESSION_ID;
		private string BILLTYPE;
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
		public string ValidityType
		{
			get
			{
				return this.VALIDITY_TYPE;
			}
			set
			{
				this.VALIDITY_TYPE = value;
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
		public short BillType
		{
			get
			{
				return Tools.StrToShort(this.BILLTYPE);
			}
			set
			{
				this.BILLTYPE = value.ToString();
			}
		}
		public OrderXReqVO()
		{
			base.Name = ProtocolName.order_x;
		}
	}
}
