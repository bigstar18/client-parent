using System;
using System.Collections.Generic;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class TradeMessageList
	{
		private List<M_TradeMessage> TDS;
		public List<M_TradeMessage> M_TradeMessage
		{
			get
			{
				return this.TDS;
			}
			set
			{
				this.TDS = value;
			}
		}
	}
}
