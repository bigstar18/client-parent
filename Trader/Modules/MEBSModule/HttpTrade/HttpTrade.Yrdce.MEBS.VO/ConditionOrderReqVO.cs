using HttpTrade.Gnnt.MEBS.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class ConditionOrderReqVO : ReqVO
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
		private string SO;
		private string CONCOMMODITYID;
		private string CONTYPE;
		private string CONOPERATOR;
		private string CONPRICE;
		private string CONEXPIRE;
		private string FIRM_ID;
		private string TRADER_ID;
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
		public long LPrice
		{
			get
			{
				return Tools.StrToLong(this.L_PRICE);
			}
			set
			{
				this.L_PRICE = value.ToString();
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
		public short So
		{
			get
			{
				return Tools.StrToShort(this.SO);
			}
			set
			{
				this.SO = value.ToString();
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
		public string ConcommodityID
		{
			get
			{
				return this.CONCOMMODITYID;
			}
			set
			{
				this.CONCOMMODITYID = value;
			}
		}
		public string FirmID
		{
			get
			{
				return this.FIRM_ID;
			}
			set
			{
				this.FIRM_ID = value;
			}
		}
		public string TraderID
		{
			get
			{
				return this.TRADER_ID;
			}
			set
			{
				this.TRADER_ID = value;
			}
		}
		public string ConmmodityID
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
		public short ConType
		{
			get
			{
				return Tools.StrToShort(this.CONTYPE);
			}
			set
			{
				this.CONTYPE = value.ToString();
			}
		}
		public short Buy_Sell
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
		public short ConOperator
		{
			get
			{
				return Tools.StrToShort(this.CONOPERATOR);
			}
			set
			{
				this.CONOPERATOR = value.ToString();
			}
		}
		public double ConPrice
		{
			get
			{
				return Tools.StrToDouble(this.CONPRICE);
			}
			set
			{
				this.CONPRICE = value.ToString();
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
		public string ConexPire
		{
			get
			{
				return this.CONEXPIRE;
			}
			set
			{
				this.CONEXPIRE = value;
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
		public ConditionOrderReqVO()
		{
			base.Name = ProtocolName.conditionorder;
		}
	}
}
