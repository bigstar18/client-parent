namespace FuturesTrade.Gnnt.DBService.ServiceManager
{
    using FuturesTrade.Gnnt.DBService.Order;
    using FuturesTrade.Gnnt.DBService.Query;
    using FuturesTrade.Gnnt.DBService.Query.QueryOrder;
    using FuturesTrade.Gnnt.DBService.Query.QueryTrade;
    using System;
    using System.Runtime.CompilerServices;

    public class ServiceManage
    {
        private ConditionOrder conditionOrder;
        private EntrustOrder entrustOrder;
        private QueryCommData queryCommData;
        public QueryConOrder queryConOrder;
        private QueryHolding queryHolding;
        private QueryHoldingDetail queryHoldingDetail;
        private QueryInitData queryInitData;
        private IQueryOrder queryOrder;
        private QuerySysTime querySysTime;
        private IQueryTrade queryTrade;
        private RevokeOrder revokeOrder;
        private static volatile ServiceManage serviceManager;

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
                    this.queryOrder = new FuturesTrade.Gnnt.DBService.Query.QueryOrder.QueryOrder();
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
                    this.queryTrade = new FuturesTrade.Gnnt.DBService.Query.QueryTrade.QueryTrade();
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

        public QueryConOrder CreateQueryConOrder()
        {
            if (this.queryConOrder == null)
            {
                this.queryConOrder = new QueryConOrder();
            }
            return this.queryConOrder;
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

        public RevokeOrder CreateRevokeOrder()
        {
            if (this.revokeOrder == null)
            {
                this.revokeOrder = new RevokeOrder();
                this.revokeOrder.UpdateOrderStatus = new RevokeOrder.UpdateOrderStatusCallBack(this.queryOrder.UpdateOrderStatus);
            }
            return this.revokeOrder;
        }

        public void DisposeServiceManage()
        {
            if (serviceManager != null)
            {
                serviceManager = null;
            }
            GC.SuppressFinalize(this);
        }

        public static ServiceManage GetInstance()
        {
            if (serviceManager == null)
            {
                lock (typeof(ServiceManage))
                {
                    if (serviceManager == null)
                    {
                        try
                        {
                            serviceManager = new ServiceManage();
                        }
                        catch (Exception exception)
                        {
                            throw exception;
                        }
                    }
                }
            }
            return serviceManager;
        }
    }
}
