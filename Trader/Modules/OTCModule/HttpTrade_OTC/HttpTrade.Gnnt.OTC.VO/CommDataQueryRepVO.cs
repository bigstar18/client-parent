using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TPME.Log;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class CommDataQueryRepVO : RepVO
	{
		private CommDataQueryRepResult RESULT;
		private CommDataResultList RESULTLIST;
		public CommDataQueryRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
		public CommDataResultList ResultList
		{
			get
			{
				return this.RESULTLIST;
			}
		}
		public CommDataQueryRepVO()
		{
		}
		public CommDataQueryRepVO(BinaryReader data)
		{
			try
			{
				Logger.wirte(1, "二进制（行情数据）返回包");
				this.RESULT = new CommDataQueryRepResult();
				this.RESULTLIST = new CommDataResultList();
				this.RESULTLIST.CommDataList = new List<M_CommData>();
				base.ID = data.ReadInt16();
				this.RESULT.RetCode = data.ReadInt64();
				short count = data.ReadInt16();
				this.RESULT.RetMessage = Encoding.UTF8.GetString(data.ReadBytes((int)count));
				int num = Convert.ToInt32(data.ReadByte());
				for (int i = 0; i < num; i++)
				{
					M_CommData m_CommData = new M_CommData();
					count = data.ReadInt16();
					m_CommData.CommodityID = Encoding.UTF8.GetString(data.ReadBytes((int)count));
					m_CommData.High = data.ReadDouble();
					m_CommData.Low = data.ReadDouble();
					m_CommData.Last = data.ReadDouble();
					m_CommData.BuyPrice = data.ReadDouble();
					m_CommData.SellPrice = data.ReadDouble();
					count = data.ReadInt16();
					m_CommData.UpdateTime = Encoding.UTF8.GetString(data.ReadBytes((int)count));
					this.RESULTLIST.CommDataList.Add(m_CommData);
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(ex);
				throw;
			}
		}
	}
}
