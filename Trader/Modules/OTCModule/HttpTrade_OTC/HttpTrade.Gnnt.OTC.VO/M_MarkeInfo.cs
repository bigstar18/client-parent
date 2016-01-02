using System;
using ToolsLibrary.util;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class M_MarkeInfo
	{
		private string MA_I;
		private string MA_N;
		private string STA;
		private string FI_I;
		private string MAR;
		private string SH_N;
		public string MarketID
		{
			get
			{
				return this.MA_I;
			}
		}
		public string MarketName
		{
			get
			{
				return this.MA_N;
			}
		}
		public short Status
		{
			get
			{
				return Tools.StrToShort(this.STA);
			}
		}
		public string FirmID
		{
			get
			{
				return this.FI_I;
			}
		}
		public short MarginType
		{
			get
			{
				return Tools.StrToShort(this.MAR);
			}
		}
		public string ShortName
		{
			get
			{
				return this.SH_N;
			}
		}
	}
}
