using HttpTrade.Gnnt.ISSUE.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class OrderReqVO : ReqVO
	{
		private string USER_ID;
		private string CUSTOMER_ID;
		private string BUY_SELL;
		private string COMMODITY_ID;
		private string PRICE;
		private string QTY;
		private string SETTLE_BASIS;
		private string CLOSEMODE;
		private string TIMEFLAG;
		private string L_PRICE;
		private string SESSION_ID;
		private string BILLTYPE;
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
		public string CustomerID
		{
			get
			{
				return this.CUSTOMER_ID;
			}
			set
			{
				this.CUSTOMER_ID = value;
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
		public short TimeFlag
		{
			get
			{
				return Tools.StrToShort(this.TIMEFLAG);
			}
			set
			{
				this.TIMEFLAG = value.ToString();
			}
		}
		public double LPrice
		{
			get
			{
				return Tools.StrToDouble(this.L_PRICE);
			}
			set
			{
				this.L_PRICE = value.ToString();
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
		public OrderReqVO()
		{
			base.Name = ProtocolName.order;
		}
	}
}
