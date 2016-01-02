using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TPME.Log;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class CommDataQueryMemberRepVO : RepVO
	{
		private CommDataQueryMemberRepResult RESULT;
		private CommDataMemberResultList RESULTLIST;
		public CommDataQueryMemberRepResult Result
		{
			get
			{
				return this.RESULT;
			}
		}
		public CommDataMemberResultList ResultList
		{
			get
			{
				return this.RESULTLIST;
			}
		}
		public CommDataQueryMemberRepVO()
		{
		}
		public CommDataQueryMemberRepVO(BinaryReader data)
		{
			try
			{
				Logger.wirte(1, "二进制（会员行情数据）返回包");
				this.RESULT = new CommDataQueryMemberRepResult();
				this.RESULTLIST = new CommDataMemberResultList();
				this.RESULTLIST.CommDataList = new List<M_CommDataMember>();
				base.ID = data.ReadInt16();
				this.RESULT.RetCode = data.ReadInt64();
				short count = data.ReadInt16();
				this.RESULT.RetMessage = Encoding.UTF8.GetString(data.ReadBytes((int)count));
				int num = Convert.ToInt32(data.ReadByte());
				for (int i = 0; i < num; i++)
				{
					M_CommDataMember m_CommDataMember = new M_CommDataMember();
					count = data.ReadInt16();
					m_CommDataMember.CommodityID = Encoding.UTF8.GetString(data.ReadBytes((int)count));
					m_CommDataMember.High = data.ReadDouble();
					m_CommDataMember.Low = data.ReadDouble();
					m_CommDataMember.Last = data.ReadDouble();
					m_CommDataMember.BasePrice = data.ReadDouble();
					m_CommDataMember.MemberBuyDianCha = data.ReadDouble();
					m_CommDataMember.MemberSellDianCha = data.ReadDouble();
					m_CommDataMember.SMemberBuyDianCha = data.ReadDouble();
					m_CommDataMember.SMemberSellDianCha = data.ReadDouble();
					count = data.ReadInt16();
					m_CommDataMember.UpdateTime = Encoding.UTF8.GetString(data.ReadBytes((int)count));
					this.RESULTLIST.CommDataList.Add(m_CommDataMember);
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
