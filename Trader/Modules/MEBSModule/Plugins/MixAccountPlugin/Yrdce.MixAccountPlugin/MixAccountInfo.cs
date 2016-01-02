using System;
using System.Collections.Generic;
namespace Gnnt.MixAccountPlugin
{
	public class MixAccountInfo
	{
		private string userID = string.Empty;
		private string moduleID = string.Empty;
		private int mapType;
		private string mainAccount = string.Empty;
		private List<UserInfo> childAccounts = new List<UserInfo>();
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
		public int MapType
		{
			get
			{
				return this.mapType;
			}
			set
			{
				this.mapType = value;
			}
		}
		public string MainAccount
		{
			get
			{
				return this.mainAccount;
			}
			set
			{
				this.mainAccount = value;
			}
		}
		public List<UserInfo> ChildAccounts
		{
			get
			{
				return this.childAccounts;
			}
			set
			{
				this.childAccounts = value;
			}
		}
	}
}
