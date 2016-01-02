using System;
namespace TradeInterface.Gnnt.ISSUE.DataVO
{
	public class CommodityInfo
	{
		public string MarketID = string.Empty;
		public string CommodityID = string.Empty;
		public string CommodityName = string.Empty;
		public string DeliveryDate = string.Empty;
		public short Status;
		public short OnMarket;
		public double CtrtSize;
		public double Spread;
		public double SpreadUp;
		public double SpreadDown;
		public short MarginType;
		public double BMargin;
		public double SMargin;
		public double BMargin_g;
		public double SMargin_g;
		public double PrevClear;
		public short CommType;
		public double BOpenComm;
		public double SOpenComm;
		public double BTHHComm;
		public double STHHComm;
		public double BTTHComm;
		public double STTHComm;
		public double BFTComm;
		public double SFTComm;
		public short DeliveryCommType;
		public double DeliveryBComm;
		public double DeliverySComm;
		public string VarietyID = string.Empty;
		public short TradeMode;
		public double MinQty;
	}
}
