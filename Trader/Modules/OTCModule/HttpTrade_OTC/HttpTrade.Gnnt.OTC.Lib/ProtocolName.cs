using System;
namespace HttpTrade.Gnnt.OTC.Lib
{
	public enum ProtocolName
	{
		logon,
		logoff,
		check_user,
		change_password,
		firm_info,
		order_s,
		order_x,
		order_wd,
		my_order_query,
		tradequery,
		holding_query,
		holding_detail_query,
		c_d_q,
		c_d_q_m,
		commodity_query,
		sys_time_query,
		set_loss_profit,
		withdraw_loss_profit,
		firm_funds_info,
		customer_order_query,
		other_firm_query,
		agency_logon,
		holdpositionbyprice,
		my_weekorder_query,
		market_query,
		directfirmbreed_query,
		firm_holdsum
	}
}
