using System;
using TradeInterface.Gnnt.ISSUE.DataVO;
namespace TradeInterface.Gnnt.ISSUE.Interface
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
		TradeSumQueryResponseVO TradeSumQuery(TradeSumQueryRequestVO req);
		SysTimeQueryResponseVO GetSysTime(SysTimeQueryRequestVO req);
		OrderQueryResponseVO OrderQuery(OrderQueryRequestVO req);
		HoldingQueryResponseVO HoldingQuery(HoldingQueryRequestVO req);
		HoldingDetailResponseVO HoldPtByPrice(HoldingDetailRequestVO req);
		OrderQueryResponseVO AllOrderQuery(OrderQueryRequestVO req);
		CommDataQueryResponseVO CommDataQuery(CommDataQueryRequestVO req);
		CommodityQueryResponseVO CommodityQuery(CommodityQueryRequestVO req);
		MarketQueryResponseVO MarketQuery(MarketQueryRequestVO req);
		FirmbreedQueryResponseVO FirmbreedQuery(string UserID);
		InvestorInfoResponseVO GetInvestorInfo(string UserID);
		void Dispose();
	}
}
