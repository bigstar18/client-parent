using System;
namespace TradeInterface.Gnnt.OTC.DataVO
{
	public class LogonResponseVO : ResponseVO
	{
		private string mLastTime = string.Empty;
		private string mLastIP = string.Empty;
		private string mChgPWD = string.Empty;
		private string mRandomKey = string.Empty;
		private string mName = string.Empty;
		private string mIdentity = string.Empty;
		public string LastTime
		{
			get
			{
				return this.mLastTime;
			}
			set
			{
				this.mLastTime = value;
			}
		}
		public string LastIP
		{
			get
			{
				return this.mLastIP;
			}
			set
			{
				this.mLastIP = value;
			}
		}
		public string ChgPWD
		{
			get
			{
				return this.mChgPWD;
			}
			set
			{
				this.mChgPWD = value;
			}
		}
		public string RandomKey
		{
			get
			{
				return this.mRandomKey;
			}
			set
			{
				this.mRandomKey = value;
			}
		}
		public string Name
		{
			get
			{
				return this.mName;
			}
			set
			{
				this.mName = value;
			}
		}
		public string Identity
		{
			get
			{
				return this.mIdentity;
			}
			set
			{
				this.mIdentity = value;
			}
		}
	}
}
