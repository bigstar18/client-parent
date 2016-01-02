using HttpTrade.Gnnt.MEBS.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	internal class MixUserReqVO : ReqVO
	{
		private string USER_ID;
		private string MDID;
		private string MAPTYPE;
		private string MMUSER;
		private string SESSION_ID;
		private AccountInfoList REQUESTLIST;
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
		public long MappingType
		{
			get
			{
				return Tools.StrToLong(this.MAPTYPE);
			}
			set
			{
				this.MAPTYPE = value.ToString();
			}
		}
		public string MmUser
		{
			get
			{
				return this.MMUSER;
			}
			set
			{
				this.MMUSER = value;
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
		public AccountInfoList RequestList
		{
			get
			{
				return this.REQUESTLIST;
			}
			set
			{
				this.REQUESTLIST = value;
			}
		}
		public MixUserReqVO()
		{
			base.Name = ProtocolName.mix_user;
		}
	}
}
