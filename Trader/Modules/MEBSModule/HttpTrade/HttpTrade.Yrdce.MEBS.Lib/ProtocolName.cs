using System;
namespace HttpTrade.Gnnt.MEBS.Lib
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
		sys_time_query,
		my_order_query,
		holdpositionbyprice,
		holding_query,
		my_weekorder_query,
		commodity_data_query,
		commodity_query,
		market_query,
		directfirmbreed_query,
		querydateqty,
		my_weekorder_pagingquery,
		tradepagingquery,
		verify_version,
		conditionorder,
		conditionorder_query,
		conditionorder_wd,
		change_mapping_password,
		mix_user,
		get_mapping_user,
		check_mapping_user
	}
}
