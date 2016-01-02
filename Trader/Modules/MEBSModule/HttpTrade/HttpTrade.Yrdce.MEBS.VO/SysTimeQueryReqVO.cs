using HttpTrade.Gnnt.MEBS.Lib;
using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class SysTimeQueryReqVO : ReqVO
	{
		private string USER_ID;
		private string LAST_ID;
		private string TD_CNT;
		private string SESSION_ID;
		private string CU_LG;
		private string UT;
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
		public long LastID
		{
			get
			{
				return Tools.StrToLong(this.LAST_ID);
			}
			set
			{
				this.LAST_ID = value.ToString();
			}
		}
		public long TradeCount
		{
			get
			{
				return Tools.StrToLong(this.TD_CNT);
			}
			set
			{
				this.TD_CNT = value.ToString();
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
		public long CurLogon
		{
			get
			{
				return Tools.StrToLong(this.CU_LG);
			}
			set
			{
				this.CU_LG = value.ToString();
			}
		}
		public long UpdateTime
		{
			get
			{
				return Tools.StrToLong(this.UT);
			}
			set
			{
				this.UT = value.ToString();
			}
		}
		public SysTimeQueryReqVO()
		{
			base.Name = ProtocolName.sys_time_query;
		}
	}
}
