using HttpTrade.Gnnt.OTC.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class EspecialMemberQueryReqVO : ReqVO
	{
		private string USER_ID;
		private string IS_D;
		private string SESSION_ID;
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
		public short IsGetDefault
		{
			get
			{
				return Tools.StrToShort(this.IS_D);
			}
			set
			{
				this.IS_D = value.ToString();
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
		public EspecialMemberQueryReqVO()
		{
			base.Name = ProtocolName.other_firm_query;
		}
	}
}
