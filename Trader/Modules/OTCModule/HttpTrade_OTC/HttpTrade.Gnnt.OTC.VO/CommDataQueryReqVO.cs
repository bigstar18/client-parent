using HttpTrade.Gnnt.OTC.Lib;
using System;
using System.IO;
using System.Text;
using ToolsLibrary.util;
using TPME.Log;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class CommDataQueryReqVO : ReqVO
	{
		private string USER_ID;
		private string COMMODITY_ID;
		private string SESSION_ID;
		private string A_N;
		private string P_P;
		public string AgencyNo
		{
			get
			{
				return this.A_N;
			}
			set
			{
				this.A_N = value;
			}
		}
		public string AgencyPhonePassword
		{
			get
			{
				return this.P_P;
			}
			set
			{
				this.P_P = value;
			}
		}
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
		public string CommodityID
		{
			get
			{
				return this.COMMODITY_ID;
			}
			set
			{
				this.COMMODITY_ID = value;
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
		public CommDataQueryReqVO()
		{
			base.Name = ProtocolName.c_d_q;
			base.ID = 1;
		}
		public override MemoryStream DeSerializationobj()
		{
			MemoryStream memoryStream = null;
			try
			{
				Logger.wirte(1, "开始启动二进制（行情数据）请求");
				memoryStream = new MemoryStream();
				BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
				binaryWriter.Write(base.ID);
				binaryWriter.Write(Convert.ToInt16(this.USER_ID.Length));
				binaryWriter.Write(Encoding.UTF8.GetBytes(this.USER_ID));
				binaryWriter.Write(Convert.ToInt16(this.COMMODITY_ID.Length));
				binaryWriter.Write(Encoding.UTF8.GetBytes(this.COMMODITY_ID));
				binaryWriter.Write(Convert.ToInt64(this.SESSION_ID));
				binaryWriter.Write(Convert.ToInt16(this.A_N.Length));
				binaryWriter.Write(Encoding.UTF8.GetBytes(this.A_N));
				binaryWriter.Write(Convert.ToInt16(this.P_P.Length));
				binaryWriter.Write(Encoding.UTF8.GetBytes(this.P_P));
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				throw;
			}
			return memoryStream;
		}
	}
}
