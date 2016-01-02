using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class M_HoldingDetailInfo
	{
		private string H_ID;
		private string CO_I;
		private string TY;
		private string O_QTY;
		private string C_QTY;
		private string PR;
		private string H_P;
		private string OR_T;
		private string FL_P;
		private string MAR;
		private string COMM;
		private string STOP_LOSS;
		private string STOP_PROFIT;
		private string OTHER_ID;
		private string CO_ID = string.Empty;
		public long HoldingID
		{
			get
			{
				return Tools.StrToLong(this.H_ID);
			}
		}
		public string CommodityID
		{
			get
			{
				return this.CO_I;
			}
		}
		public short BuySell
		{
			get
			{
				return Tools.StrToShort(this.TY);
			}
		}
		public long OpenQuantity
		{
			get
			{
				return Tools.StrToLong(this.O_QTY);
			}
		}
		public long HoldingQuantity
		{
			get
			{
				return Tools.StrToLong(this.C_QTY);
			}
		}
		public double Price
		{
			get
			{
				return Tools.StrToDouble(this.PR);
			}
		}
		public double HoldingPrice
		{
			get
			{
				return Tools.StrToDouble(this.H_P);
			}
		}
		public string OrderTime
		{
			get
			{
				return this.OR_T;
			}
		}
		public double FloatProfit
		{
			get
			{
				return Tools.StrToDouble(this.FL_P);
			}
		}
		public double Bail
		{
			get
			{
				return Tools.StrToDouble(this.MAR);
			}
		}
		public double Comm
		{
			get
			{
				return Tools.StrToDouble(this.COMM);
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
		public string OtherID
		{
			get
			{
				return this.OTHER_ID;
			}
		}
		public string AgentID
		{
			get
			{
				return this.CO_ID;
			}
		}
	}
}
