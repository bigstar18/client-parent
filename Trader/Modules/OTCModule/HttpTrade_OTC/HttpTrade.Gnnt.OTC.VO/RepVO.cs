using HttpTrade.Gnnt.OTC.Lib;
using System;
using System.IO;
using TPME.Log;
namespace HttpTrade.Gnnt.OTC.VO
{
	public class RepVO
	{
		private short id;
		public short ID
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}
		public static RepVO GetResponseVO(ProtocolName name, BinaryReader Reader)
		{
			RepVO result = null;
			try
			{
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
				case ProtocolName.order_s:
					result = new OrderSRepVO();
					break;
				case ProtocolName.order_x:
					result = new OrderXRepVO();
					break;
				case ProtocolName.order_wd:
					result = new WithDrawOrderRepVO();
					break;
				case ProtocolName.my_order_query:
					result = new OrderQueryRepVO();
					break;
				case ProtocolName.tradequery:
					result = new TradeQueryRepVO();
					break;
				case ProtocolName.holding_query:
					result = new HoldingQueryRepVO();
					break;
				case ProtocolName.holding_detail_query:
					result = new HoldingDetailRepVO();
					break;
				case ProtocolName.c_d_q:
					result = new CommDataQueryRepVO(Reader);
					break;
				case ProtocolName.c_d_q_m:
					result = new CommDataQueryMemberRepVO(Reader);
					break;
				case ProtocolName.commodity_query:
					result = new CommodityQueryRepVO();
					break;
				case ProtocolName.sys_time_query:
					result = new SysTimeQueryRepVO(Reader);
					break;
				case ProtocolName.set_loss_profit:
					result = new SetLossProfitRepVO();
					break;
				case ProtocolName.withdraw_loss_profit:
					result = new WithdrawLossProfitRepVO();
					break;
				case ProtocolName.firm_funds_info:
					result = new FirmFundsInfoRepVO();
					break;
				case ProtocolName.customer_order_query:
					result = new CustomerOrderQueryRepVO();
					break;
				case ProtocolName.other_firm_query:
					result = new EspecialMemberQueryRepVO();
					break;
				case ProtocolName.agency_logon:
					result = new AgencyLogonRepVO();
					break;
				case ProtocolName.firm_holdsum:
					result = new FirmHoldSumRepVO();
					break;
				}
			}
			catch (Exception ex)
			{
				Logger.wirte(3, ex.Message.ToString());
			}
			return result;
		}
	}
}
