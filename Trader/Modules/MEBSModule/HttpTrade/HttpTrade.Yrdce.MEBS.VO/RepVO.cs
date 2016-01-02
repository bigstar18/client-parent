using HttpTrade.Gnnt.MEBS.Lib;
using System;
namespace HttpTrade.Gnnt.MEBS.VO
{
	public class RepVO
	{
		public static RepVO GetResponseVO(ProtocolName name)
		{
			RepVO result = null;
			switch (name)
			{
			case ProtocolName.logon:
				result = new LogonRepVO();
				break;
			case ProtocolName.logoff:
				result = new LogoffRepVO();
				break;
			case ProtocolName.check_user:
				result = new CheckUserRepVO();
				break;
			case ProtocolName.change_password:
				result = new ChgPwdRepVO();
				break;
			case ProtocolName.firm_info:
				result = new FirmInfoRepVO();
				break;
			case ProtocolName.order:
				result = new OrderRepVO();
				break;
			case ProtocolName.order_wd:
				result = new WithDrawOrderRepVO();
				break;
			case ProtocolName.tradequery:
				result = new TradeQueryRepVO();
				break;
			case ProtocolName.sys_time_query:
				result = new SysTimeQueryRepVO();
				break;
			case ProtocolName.my_order_query:
				result = new OrderQueryRepVO();
				break;
			case ProtocolName.holdpositionbyprice:
				result = new HoldingDetailRepVO();
				break;
			case ProtocolName.holding_query:
				result = new HoldingQueryRepVO();
				break;
			case ProtocolName.my_weekorder_query:
				result = new WeekOrderQueryRepVO();
				break;
			case ProtocolName.commodity_data_query:
				result = new CommDataQueryRepVO();
				break;
			case ProtocolName.commodity_query:
				result = new CommodityQueryRepVO();
				break;
			case ProtocolName.market_query:
				result = new MarketQueryRepVO();
				break;
			case ProtocolName.directfirmbreed_query:
				result = new FirmBreedRepVO();
				break;
			case ProtocolName.querydateqty:
				result = new QuerydateqtyRepVO();
				break;
			case ProtocolName.my_weekorder_pagingquery:
				result = new WeekOrderPagingQueryRepVO();
				break;
			case ProtocolName.tradepagingquery:
				result = new TradePagingQueryRepVO();
				break;
			case ProtocolName.verify_version:
				result = new VerifyVersionRepVO();
				break;
			case ProtocolName.conditionorder:
				result = new ConditionOrderRepVO();
				break;
			case ProtocolName.conditionorder_query:
				result = new ConditionQueryRepVO();
				break;
			case ProtocolName.conditionorder_wd:
				result = new ConditionRevokeRepVO();
				break;
			case ProtocolName.change_mapping_password:
				result = new ChgMappingPwdRepVO();
				break;
			case ProtocolName.mix_user:
				result = new MixUserRepVO();
				break;
			case ProtocolName.get_mapping_user:
				result = new GetMappingUserRepVO();
				break;
			case ProtocolName.check_mapping_user:
				result = new CheckMappingUserRepVO();
				break;
			}
			return result;
		}
	}
}
