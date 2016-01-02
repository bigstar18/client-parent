using HttpTrade.Gnnt.MEBS.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class ConditionRevokeReqVO : ReqVO
	{
		private string USER_ID;
		private string ORDER_NO;
		private string SESSION_ID;
		private string CUSTOMER_ID;
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
		public long ConditionOrderNo
		{
			get
			{
				return Tools.StrToLong(this.ORDER_NO);
			}
			set
			{
				this.ORDER_NO = value.ToString();
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
		public ConditionRevokeReqVO()
		{
			base.Name = ProtocolName.conditionorder_wd;
		}
	}
}
