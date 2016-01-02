using HttpTrade.Gnnt.MEBS.Lib;
using System;
namespace HttpTrade.Gnnt.MEBS.VO
{
	internal class GetMappingUserReqVO : ReqVO
	{
		private string USER_ID = string.Empty;
		private string MDID = string.Empty;
		private string PASSWORD = string.Empty;
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
		public string ModuleID
		{
			get
			{
				return this.MDID;
			}
			set
			{
				this.MDID = value;
			}
		}
		public string Password
		{
			get
			{
				return this.PASSWORD;
			}
			set
			{
				this.PASSWORD = value;
			}
		}
		public GetMappingUserReqVO()
		{
			base.Name = ProtocolName.get_mapping_user;
		}
	}
}
