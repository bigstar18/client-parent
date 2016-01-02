using System;
using TPME.Log;
using TradeInterface.Gnnt.OTC.DataVO;
namespace TradeClientApp.Gnnt.OTC.Library
{
	internal class IdentityStatus
	{
		private bool IsAgency;
		public IdentityStatus(bool _IsAgency)
		{
			this.IsAgency = _IsAgency;
		}
		public bool IdentityStatusToEnableMenu(ConnectStatus connectStatus, out string ErrorMessage)
		{
			bool flag = false;
			ErrorMessage = "";
			string value = string.Empty;
			bool result;
			try
			{
				if (connectStatus != ConnectStatus.Connect)
				{
					ErrorMessage = "您当前的连接状态异常,不能进行业务操作!";
					result = flag;
				}
				else
				{
					if (this.IsAgency)
					{
						lock (Global.AgencyHQCommDataLock)
						{
							if (Global.AgencyHQCommData == null)
							{
								ErrorMessage = "行情数据异常,不能进行业务操作!";
								result = flag;
								return result;
							}
						}
						if (Global.AgencyCommodityData == null)
						{
							ErrorMessage = "商品数据异常,不能进行业务操作!";
							result = flag;
							return result;
						}
						lock (Global.AgencyFirmInfoDataLock)
						{
							if (Global.AgencyFirmInfoData == null)
							{
								ErrorMessage = "账户信息异常,不能进行业务操作!";
								result = flag;
								return result;
							}
							goto IL_123;
						}
					}
					lock (Global.HQCommDataLock)
					{
						if (Global.HQCommData == null)
						{
							ErrorMessage = "行情数据异常,不能进行业务操作!";
							result = flag;
							return result;
						}
					}
					if (Global.CommodityData == null)
					{
						ErrorMessage = "商品数据异常,不能进行业务操作!";
						result = flag;
						return result;
					}
					lock (Global.FirmInfoDataLock)
					{
						if (Global.FirmInfoData == null)
						{
							ErrorMessage = "账户信息异常,不能进行业务操作!";
							result = flag;
							return result;
						}
					}
					if (Global.sMemberType == MemberType.B)
					{
						ErrorMessage = "经纪类会员不能进行业务操作!";
						result = flag;
						return result;
					}
					IL_123:
					if (this.IsAgency)
					{
						lock (Global.AgencyFirmInfoDataLock)
						{
							value = Global.AgencyFirmInfoData.CStatus;
							goto IL_16F;
						}
					}
					lock (Global.FirmInfoDataLock)
					{
						value = Global.FirmInfoData.CStatus;
					}
					IL_16F:
					switch ((MemberStatus)Enum.Parse(typeof(MemberStatus), value))
					{
					case MemberStatus.U:
						ErrorMessage = "您当前的状态为未激活,不能进行业务操作!";
						flag = false;
						break;
					case MemberStatus.N:
						flag = true;
						break;
					case MemberStatus.F:
						flag = true;
						break;
					case MemberStatus.D:
						ErrorMessage = "您当前的状态为终止,不能进行业务操作!";
						flag = false;
						break;
					default:
						ErrorMessage = "您当前为非正常状态,不能进行业务操作!";
						flag = false;
						break;
					}
					result = flag;
				}
			}
			catch (Exception ex)
			{
				ErrorMessage = ex.Message;
				Logger.wirte(ex);
				result = flag;
			}
			return result;
		}
	}
}
