using System;
using TradeInterface.Gnnt.OTC.DataVO;
namespace TradeInterface.Gnnt.OTC.Interface
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
		ResponseVO CheckUser(CheckUserRequestVO userID);
		ResponseVO ChangePwd(ChgPwdRequestVO req);
		FirmInfoResponseVO GetFirmInfo(FirmInfoRequestVO req);
		ResponseVO Order(OrderRequestVO req);
		ResponseVO WithDrawOrder(WithDrawOrderRequestVO req);
		OrderQueryResponseVO OrderQuery(OrderQueryRequestVO req);
		OrderQueryResponseVO AllOrderQuery(OrderQueryRequestVO req);
		TradeQueryResponseVO TradeQuery(TradeQueryRequestVO req);
		HoldingQueryResponseVO HoldingQuery(HoldingQueryRequestVO req);
		HoldingDetailResponseVO HoldPtByPrice(HoldingDetailRequestVO req);
		CommDataQueryResponseVO CommDataQuery(CommDataQueryRequestVO req);
		CommodityQueryResponseVO CommodityQuery(CommodityQueryRequestVO req);
		SysTimeQueryResponseVO GetSysTime(SysTimeQueryRequestVO req);
		ResponseVO SetLossProfit(SetLossProfitRequestVO req);
		ResponseVO WithdrawLossProfit(WithdrawLossProfitRequestVO req);
		FirmFundsInfoResponseVO GetFirmFundsInfo(string userID);
		CustomerOrderQueryResponseVO GetCustomerOrderQuery(CustomerOrderQueryRequestVO req);
		FirmHoldSumResponseVO GetFirmHoldSumQuery(string userID);
		EspecialMemberQueryResponseVO GetEspecialMemberQuery(string userID, bool isGetDefault);
		ResponseVO AgencyLogon(AgencyLogonRequestVO req);
		void Dispose();
	}
}
