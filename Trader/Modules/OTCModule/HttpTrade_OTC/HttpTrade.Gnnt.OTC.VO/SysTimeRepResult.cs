using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class SysTimeRepResult
	{
		private string RETCODE;
		private string MESSAGE;
		private string CU_T;
		private string CU_D;
		private string TV_U;
		private string L_ID;
		private string NEW_T;
		private string TD_TTL;
		private TradeMessageList TDRP;
		private string DAY;
		private string U_D;
		private string S_S;
		public long RetCode
		{
			get
			{
				return Tools.StrToLong(this.RETCODE);
			}
			set
			{
				this.RETCODE = value.ToString();
			}
		}
		public string RetMessage
		{
			get
			{
				return this.MESSAGE;
			}
			set
			{
				this.MESSAGE = value;
			}
		}
		public string CurrentTime
		{
			get
			{
				return this.CU_T;
			}
			set
			{
				this.CU_T = value;
			}
		}
		public string CurrentDate
		{
			get
			{
				return this.CU_D;
			}
			set
			{
				this.CU_D = value;
			}
		}
		public string MicroSecond
		{
			get
			{
				return this.TV_U;
			}
			set
			{
				this.TV_U = value;
			}
		}
		public long LastID
		{
			get
			{
				return Tools.StrToLong(this.L_ID);
			}
			set
			{
				this.L_ID = value.ToString();
			}
		}
		public short NewTrade
		{
			get
			{
				return Tools.StrToShort(this.NEW_T);
			}
			set
			{
				this.NEW_T = value.ToString();
			}
		}
		public long TradeTotal
		{
			get
			{
				return Tools.StrToLong(this.TD_TTL);
			}
			set
			{
				this.TD_TTL = value.ToString();
			}
		}
		public TradeMessageList TradeMessageList
		{
			get
			{
				return this.TDRP;
			}
			set
			{
				this.TDRP = value;
			}
		}
		public string TradeDay
		{
			get
			{
				return this.DAY;
			}
			set
			{
				this.DAY = value;
			}
		}
		public string UpdateData
		{
			get
			{
				return this.U_D;
			}
			set
			{
				this.U_D = value;
			}
		}
		public short SystemStatus
		{
			get
			{
				return Tools.StrToShort(this.S_S);
			}
			set
			{
				this.S_S = value.ToString();
			}
		}
	}
}
