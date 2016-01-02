using System;
namespace TradeInterface.Gnnt.OTC.DataVO
{
	public enum SystemStatus
	{
		InitializationComplete,
		Settling = 2,
		SettlementComplete,
		TradePause,
		Trading,
		Rest,
		TradeClosed
	}
}
