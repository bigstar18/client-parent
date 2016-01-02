using HttpTrade.Gnnt.MEBS.Lib;
using System;
namespace HttpTrade.Gnnt.MEBS.VO
{
	internal class CheckMappingUserReqVO : ReqVO
	{
		private string USER_ID;
		private string MDID;
		private string PASSWORD;
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
		public CheckMappingUserReqVO()
		{
			base.Name = ProtocolName.check_mapping_user;
		}
	}
}
