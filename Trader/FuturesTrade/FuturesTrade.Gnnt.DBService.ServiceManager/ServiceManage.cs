using FuturesTrade.Gnnt.DBService.Order;
using FuturesTrade.Gnnt.DBService.Query;
using FuturesTrade.Gnnt.DBService.Query.QueryOrder;
using FuturesTrade.Gnnt.DBService.Query.QueryTrade;
using System;
using System.Threading;
namespace FuturesTrade.Gnnt.DBService.ServiceManager
{
	public class ServiceManage
	{
		private ConditionOrder conditionOrder;
		private EntrustOrder entrustOrder;
		private RevokeOrder revokeOrder;
		public QueryConOrder queryConOrder;
		private IQueryOrder queryOrder;
		private IQueryTrade queryTrade;
		private QueryCommData queryCommData;
		private QueryHolding queryHolding;
		private QueryHoldingDetail queryHoldingDetail;
		private QueryInitData queryInitData;
		private QuerySysTime querySysTime;
		private static volatile ServiceManage serviceManager;
		public static ServiceManage GetInstance()
		{
			if (ServiceManage.serviceManager == null)
			{
				Type typeFromHandle;
				Monitor.Enter(typeFromHandle = typeof(ServiceManage));
				try
				{
					if (ServiceManage.serviceManager == null)
					{
						try
						{
							ServiceManage.serviceManager = new ServiceManage();
						}
						catch (Exception ex)
						{
							throw ex;
						}
					}
				}
				finally
				{
					Monitor.Exit(typeFromHandle);
				}
			}
			return ServiceManage.serviceManager;
		}
		public ConditionOrder CreateConditionOrder()
		{
			if (this.conditionOrder == null)
			{
				this.conditionOrder = new ConditionOrder();
			}
			return this.conditionOrder;
		}
		public EntrustOrder CreateEntrustOrder()
		{
			if (this.entrustOrder == null)
			{
				this.entrustOrder = new EntrustOrder();
				this.entrustOrder.InsertOrder = new EntrustOrder.InsertOrderCallBack(this.queryOrder.UpdateOrderInfo);
				this.entrustOrder.CalculateSumOrder = new EntrustOrder.CalculateSumOrderCallBack(this.queryOrder.CalculateTotalData);
			}
			return this.entrustOrder;
		}
		public RevokeOrder CreateRevokeOrder()
		{
			if (this.revokeOrder == null)
			{
				this.revokeOrder = new RevokeOrder();
				this.revokeOrder.UpdateOrderStatus = new RevokeOrder.UpdateOrderStatusCallBack(this.queryOrder.UpdateOrderStatus);
			}
			return this.revokeOrder;
		}
		public QueryConOrder CreateQueryConOrder()
		{
			if (this.queryConOrder == null)
			{
				this.queryConOrder = new QueryConOrder();
			}
			return this.queryConOrder;
		}
		public IQueryOrder CreateIQueryOrder(bool isPagingQuery)
		{
			if (this.queryOrder == null)
			{
				if (isPagingQuery)
				{
					this.queryOrder = new QueryPingOrder();
				}
				else
				{
					this.queryOrder = new QueryOrder();
				}
			}
			return this.queryOrder;
		}
		public IQueryTrade CreateIQueryTrade(bool isPagingQuery)
		{
			if (this.queryTrade == null)
			{
				if (isPagingQuery)
				{
					this.queryTrade = new QueryPingTrade();
				}
				else
				{
					this.queryTrade = new QueryTrade();
				}
			}
			return this.queryTrade;
		}
		public QueryCommData CreateQueryCommData()
		{
			if (this.queryCommData == null)
			{
				this.queryCommData = new QueryCommData();
			}
			return this.queryCommData;
		}
		public QueryHolding CreateQueryHolding()
		{
			if (this.queryHolding == null)
			{
				this.queryHolding = new QueryHolding();
			}
			return this.queryHolding;
		}
		public QueryHoldingDetail CreateQueryHoldingDetail()
		{
			if (this.queryHoldingDetail == null)
			{
				this.queryHoldingDetail = new QueryHoldingDetail();
			}
			return this.queryHoldingDetail;
		}
		public QueryInitData CreateQueryInitData()
		{
			if (this.queryInitData == null)
			{
				this.queryInitData = new QueryInitData();
			}
			return this.queryInitData;
		}
		public QuerySysTime CreateQuerySysTime()
		{
			if (this.querySysTime == null)
			{
				this.querySysTime = new QuerySysTime();
			}
			return this.querySysTime;
		}
		public void DisposeServiceManage()
		{
			if (ServiceManage.serviceManager != null)
			{
				ServiceManage.serviceManager = null;
			}
			GC.SuppressFinalize(this);
		}
	}
}
