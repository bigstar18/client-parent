using System;
namespace Gnnt.MEBS.HQModel.DataVO
{
	public class MarketDataVO
	{
		public string marketID;
		public string marketName;
		public TradeTimeVO[] m_timeRange;
		public int date;
		public int time;
	}
}
