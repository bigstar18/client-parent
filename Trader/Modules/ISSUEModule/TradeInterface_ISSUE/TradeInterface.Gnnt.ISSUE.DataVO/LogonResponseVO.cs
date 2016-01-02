using System;
namespace TradeInterface.Gnnt.ISSUE.DataVO
{
	public class LogonResponseVO : ResponseVO
	{
		private string mLastTime = string.Empty;
		private string mLastIP = string.Empty;
		private string mChgPWD = string.Empty;
		private string mRandomKey = string.Empty;
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
	}
}
