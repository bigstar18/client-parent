using System;
namespace TradeInterface.Gnnt.DataVO
{
	public class ConditionRevokeRequestVO
	{
		public string UserID = string.Empty;
		public string CustomerID = string.Empty;
		public long SessionID;
		public long ConditionOrderNo;
	}
}
