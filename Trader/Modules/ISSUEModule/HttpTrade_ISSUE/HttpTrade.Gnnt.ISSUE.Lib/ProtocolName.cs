using System;
namespace HttpTrade.Gnnt.ISSUE.Lib
{
	public enum ProtocolName
	{
		logon,
		logoff,
		check_user,
		change_password,
		firm_info,
		order,
		order_wd,
		tradequery,
		trade_sum_query,
		sys_time_query,
		my_order_query,
		holdpositionbyprice,
		holding_query,
		my_weekorder_query,
		commodity_data_query,
		commodity_query,
		market_query,
		directfirmbreed_query,
		investor_info
	}
}
