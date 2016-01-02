using System;
using System.Collections.Generic;
namespace TradeInterface.Gnnt.DataVO
{
	public class MixUserRequestVO
	{
		private string userID = string.Empty;
		private string moduleID = string.Empty;
		private int mappingType;
		private string mMappingUserID = string.Empty;
		private long sessionID;
		public List<AccountInfo> AccountInfoList = new List<AccountInfo>();
		public string UserID
		{
			get
			{
				return this.userID;
			}
			set
			{
				this.userID = value;
			}
		}
		public string ModuleID
		{
			get
			{
				return this.moduleID;
			}
			set
			{
				this.moduleID = value;
			}
		}
		public int MappingType
		{
			get
			{
				return this.mappingType;
			}
			set
			{
				this.mappingType = value;
			}
		}
		public string MMappingUserID
		{
			get
			{
				return this.mMappingUserID;
			}
			set
			{
				this.mMappingUserID = value;
			}
		}
		public long SessionID
		{
			get
			{
				return this.sessionID;
			}
			set
			{
				this.sessionID = value;
			}
		}
	}
}
