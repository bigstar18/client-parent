using HttpTrade.Gnnt.OTC.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class FirmInfoReqVO : ReqVO
	{
		private string USER_ID;
		private string SESSION_ID;
		private string A_N;
		private string P_P;
		public string AgencyNo
		{
			get
			{
				return this.A_N;
			}
			set
			{
				this.A_N = value;
			}
		}
		public string AgencyPhonePassword
		{
			get
			{
				return this.P_P;
			}
			set
			{
				this.P_P = value;
			}
		}
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
		public FirmInfoReqVO()
		{
			base.Name = ProtocolName.firm_info;
		}
	}
}
