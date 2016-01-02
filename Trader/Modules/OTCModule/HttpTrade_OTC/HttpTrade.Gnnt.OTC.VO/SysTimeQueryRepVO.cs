using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TPME.Log;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class SysTimeQueryRepVO : RepVO
	{
		private SysTimeRepResult RESULT;
		public SysTimeRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
		public SysTimeQueryRepVO()
		{
		}
		public SysTimeQueryRepVO(BinaryReader data)
		{
			try
			{
				Logger.wirte(1, "二进制（服务器时间）返回包");
				this.RESULT = new SysTimeRepResult();
				this.RESULT.TradeMessageList = new TradeMessageList();
				this.RESULT.TradeMessageList.M_TradeMessage = new List<M_TradeMessage>();
				base.ID = data.ReadInt16();
				this.RESULT.RetCode = data.ReadInt64();
				short count = data.ReadInt16();
				this.RESULT.RetMessage = Encoding.UTF8.GetString(data.ReadBytes((int)count));
				count = data.ReadInt16();
				this.RESULT.CurrentTime = Encoding.UTF8.GetString(data.ReadBytes((int)count));
				count = data.ReadInt16();
				this.RESULT.CurrentDate = Encoding.UTF8.GetString(data.ReadBytes((int)count));
				this.RESULT.MicroSecond = data.ReadInt64().ToString();
				this.RESULT.LastID = data.ReadInt64();
				this.RESULT.NewTrade = Convert.ToInt16(data.ReadByte());
				this.RESULT.TradeTotal = data.ReadInt64();
				count = data.ReadInt16();
				this.RESULT.TradeDay = Encoding.UTF8.GetString(data.ReadBytes((int)count));
				int num = data.ReadInt32();
				for (int i = 0; i < num; i++)
				{
					M_TradeMessage m_TradeMessage = new M_TradeMessage();
					m_TradeMessage.OrderNO = data.ReadInt64();
					count = data.ReadInt16();
					m_TradeMessage.CommodityID = Encoding.UTF8.GetString(data.ReadBytes((int)count));
					m_TradeMessage.TradeQuatity = data.ReadInt64();
					m_TradeMessage.SettleBasis = data.ReadInt16();
					m_TradeMessage.BuySell = data.ReadInt16();
					m_TradeMessage.TradeType = data.ReadInt16();
					this.RESULT.TradeMessageList.M_TradeMessage.Add(m_TradeMessage);
				}
				this.RESULT.SystemStatus = Convert.ToInt16(data.ReadByte());
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				throw;
			}
		}
	}
}
