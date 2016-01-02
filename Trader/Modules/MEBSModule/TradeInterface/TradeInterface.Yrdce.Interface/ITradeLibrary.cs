using System;
using TradeInterface.Gnnt.DataVO;
namespace TradeInterface.Gnnt.Interface
{
	public interface ITradeLibrary
	{
		int ProtocolID
		{
			get;
			set;
		}
		bool IsWriteLog
		{
			get;
			set;
		}
		string IpAddress
		{
			get;
			set;
		}
		int Port
		{
			get;
			set;
		}
		string CommunicationUrl
		{
			get;
			set;
		}
		void Initialize();
		LogonResponseVO Logon(LogonRequestVO req);
		ResponseVO Logoff(string userID);
		ResponseVO CheckUser(CheckUserRequestVO req);
		ResponseVO ChangePwd(ChgPwdRequestVO req);
		FirmInfoResponseVO GetFirmInfo(string userID);
		ResponseVO Order(OrderRequestVO req);
		ResponseVO WithDrawOrder(WithDrawOrderRequestVO req);
		TradeQueryResponseVO TradeQuery(TradeQueryRequestVO req);
		SysTimeQueryResponseVO GetSysTime(SysTimeQueryRequestVO req);
		OrderQueryResponseVO OrderQuery(OrderQueryRequestVO req);
		OrderQueryResponseVO OrderQuery();
		HoldingQueryResponseVO HoldingQuery(HoldingQueryRequestVO req);
		HoldingDetailResponseVO HoldPtByPrice(HoldingDetailRequestVO req);
		OrderQueryResponseVO AllOrderQuery(OrderQueryRequestVO req);
		OrderQueryResponseVO AllOrderQuery();
		CommDataQueryResponseVO CommDataQuery(CommDataQueryRequestVO req);
		CommodityQueryResponseVO CommodityQuery(CommodityQueryRequestVO req);
		MarketQueryResponseVO MarketQuery(MarketQueryRequestVO req);
		FirmbreedQueryResponseVO FirmbreedQuery(string UserID);
		QuerydateqtyResponseVO Querydateqty(QuerydateqtyRequestVO req);
		OrderQueryPagingResponseVO AllOrderQueryPaging(OrderQueryPagingRequestVO req);
		TradeQueryPagingResponseVO TradeQueryPaging(TradeQueryPagingRequestVO req);
		VerifyVersionResponseVO VerifyVersion(VerifyVersionRequestVO req);
		ConditionOrderResponseVO ConditionOrder(ConditionOrderRequestVO req);
		ConditionQueryResponseVO ConditionQuery(ConditionQueryRequestVO req);
		ConditionRevokeResponseVO ConditionRevoke(ConditionRevokeRequestVO req);
		ResponseVO ChgMappingPwd(ChgMappingPwdRequestVO req);
		ResponseVO MixUser(MixUserRequestVO req);
		GetMappingUserResponseVO GetMappingUser(GetMappingUserRequestVO req);
		ResponseVO CheckMappingUser(CheckMappingUserRequestVO req);
		void Dispose();
	}
}
