using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class M_HoldingInfo
	{
		private string CO_I;
		private string FL_P;
		private string MAR;
		private string COMM;
		private string TY;
		private string QTY;
		private string O_A;
		private string A_H;
		private string F_QTY;
		public string CommodityID
		{
			get
			{
				return this.CO_I;
			}
		}
		public double FloatingLP
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
		public short TradeType
		{
			get
			{
				return Tools.StrToShort(this.TY);
			}
		}
		public long Qty
		{
			get
			{
				return Tools.StrToLong(this.QTY);
			}
		}
		public double OpenAveragePrice
		{
			get
			{
				return Tools.StrToDouble(this.O_A);
			}
		}
		public double HoldingAveragePrice
		{
			get
			{
				return Tools.StrToDouble(this.A_H);
			}
		}
		public long FreezeQty
		{
			get
			{
				return Tools.StrToLong(this.F_QTY);
			}
		}
	}
}
