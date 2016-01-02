using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.ISSUE.VO
{
	public class M_HoldingDetailInfo
	{
		private string CO_I;
		private string CU_I;
		private string QTY;
		private string BS;
		private string PRC;
		private string GO_Q;
		private string MAR;
		public string CommodityID
		{
			get
			{
				return this.CO_I;
			}
		}
		public string CustomerID
		{
			get
			{
				return this.CU_I;
			}
		}
		public short BuySell
		{
			get
			{
				return Tools.StrToShort(this.BS);
			}
		}
		public long AmountOnOrder
		{
			get
			{
				return Tools.StrToLong(this.QTY);
			}
		}
		public double Price
		{
			get
			{
				return Tools.StrToDouble(this.PRC);
			}
		}
		public long GOQuantity
		{
			get
			{
				return Tools.StrToLong(this.GO_Q);
			}
		}
		public double Bail
		{
			get
			{
				return Tools.StrToDouble(this.MAR);
			}
		}
	}
}
