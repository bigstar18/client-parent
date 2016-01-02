using System;
using System.Collections.Generic;
namespace TradeInterface.Gnnt.OTC.DataVO
{
	public class CommodityInfo
	{
		public string MarketID = string.Empty;
		public string CommodityID = string.Empty;
		public string CommodityName = string.Empty;
		public string DeliveryDate = string.Empty;
		public short Status;
		public double CtrtSize;
		public double Spread;
		public double SpreadUp;
		public double SpreadDown;
		public short MarginType;
		public double MarginValue;
		public short DeferType;
		public double PrevClear;
		public short CommType;
		public long P_MIN_H;
		public long P_MAX_H;
		public long MaxHolding;
		public bool W_D_T_P = true;
		public bool W_D_S_L_P = true;
		public bool W_D_S_P_P = true;
		public bool B_O_P = true;
		public bool B_L_P = true;
		public bool B_X_O_P = true;
		public bool B_S_L = true;
		public bool B_S_P = true;
		public bool S_O_P = true;
		public bool S_L_P = true;
		public bool S_X_O_P = true;
		public bool S_S_L = true;
		public bool S_S_P = true;
		public double B_P_D_D;
		public double S_P_D_D;
		public double X_O_B_D_D;
		public double X_O_S_D_D;
		public double U_O_D_D_MIN;
		public double U_O_D_D_MAX;
		public double U_O_D_D_DF;
		public long OrderNum;
		public double B_J_H;
		public short DeliveryCommType;
		public double DeliveryBComm;
		public double DeliverySComm;
		public string VarietyID = string.Empty;
		public short TradeMode;
		public double STOP_L_P;
		public double STOP_P_P;
		public List<YanQiFee> YanQiFeeList;
		public double FeeValue;
		public string FeeType = string.Empty;
		public string CommodityUnit;
	}
}
