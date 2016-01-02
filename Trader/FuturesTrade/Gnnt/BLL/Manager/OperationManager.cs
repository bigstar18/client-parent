namespace FuturesTrade.Gnnt.BLL.Manager
{
    using FuturesTrade.Gnnt.BLL.Order;
    using FuturesTrade.Gnnt.BLL.Query;
    using FuturesTrade.Gnnt.BLL.Query.QueryOrderOperation;
    using FuturesTrade.Gnnt.BLL.Query.QueryTradeOperation;
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using TPME.Log;

    public class OperationManager
    {
        public string AllCheck = Global.M_ResourceManager.GetString("TradeStr_MainFormF3_AllCheck");
        public string AllNotCheck = Global.M_ResourceManager.GetString("TradeStr_MainFormF3_AllNotCheck");
        public List<string> commodityList = new List<string>();
        public ConOrderOperation conOrderOperation = new ConOrderOperation();
        public RefreshQueryInfo CurrentSelectIndex;
        public string DoubleClickCancellation = Global.M_ResourceManager.GetString("TradeStr_MainFormF2_DoubleClickCancellation");
        public string ErrorTitle = Global.M_ResourceManager.GetString("TradeStr_MainForm_ErrorTitle");
        public int IdleOnMoudel;
        public int IdleRefreshButton;
        public string InputPageNum = Global.M_ResourceManager.GetString("TradeStr_MainForm_InputPageNum");
        public string InputRightPageNum = Global.M_ResourceManager.GetString("TradeStr_MainForm_InputRightPageNum");
        public List<string> myCommodityList = new List<string>();
        public List<string> myTransactionsList = new List<string>();
        private static volatile OperationManager operationManager;
        public OrderOperation orderOperation = new OrderOperation();
        public string PageNumError = Global.M_ResourceManager.GetString("TradeStr_MainForm_PageNumError");
        public string Prompt = Global.M_ResourceManager.GetString("TradeStr_MainForm_Prompt");
        public QueryAllOrderOperation queryAllOrderOperation = new QueryAllOrderOperation();
        public QueryCommDataOperation queryCommDataOperation = new QueryCommDataOperation();
        public QueryConOrderOperation queryConOrderOperation = new QueryConOrderOperation();
        public QueryHoldingDetailOperation queryHoldingDatailOperation = new QueryHoldingDetailOperation();
        public QueryHoldingOperation queryHoldingOperation = new QueryHoldingOperation();
        public QueryInitDataOperation queryInitDataOperation = new QueryInitDataOperation();
        public QueryPredelegateOperation queryPredelegateOperation = new QueryPredelegateOperation();
        public QuerySysTimeOperation querySysTimeOperation = new QuerySysTimeOperation();
        public FuturesTrade.Gnnt.BLL.Query.QueryTradeOperation.QueryTradeOperation queryTradeOperation = new FuturesTrade.Gnnt.BLL.Query.QueryTradeOperation.QueryTradeOperation();
        public QueryTradeOrderOperation queryTradeOrderOperation = new QueryTradeOrderOperation();
        public QueryUnOrderOperation queryUnOrderOperation = new QueryUnOrderOperation();
        public RevokeConOrderOperation revokeConOrderOperation = new RevokeConOrderOperation();
        public RevokeOrderOperation revokeOrderOperation = new RevokeOrderOperation();
        public string RevokeOrders = Global.M_ResourceManager.GetString("TradeStr_MainFormF2_RevokeOrders");
        public string RevokeOrdersMessge = Global.M_ResourceManager.GetString("TradeStr_MainFormF2_RevokeOrdersMessge");
        public SetHQTimerEnableCallBack SetHQTimerEnable;
        public ShowHoldingControlCallBack ShowHoldingCollect;
        public ShowHoldingControlCallBack ShowHoldingDetail;
        public string StrAll = Global.M_ResourceManager.GetString("TradeStr_RadioAllF3");
        public string StrBuy = Global.M_ResourceManager.GetString("TradeStr_RadioB");
        public string StrSale = Global.M_ResourceManager.GetString("TradeStr_RadioS");
        public SubmitConOrderOperation submitConOrderOperation = new SubmitConOrderOperation();
        public SubmitOrderOperation submitOrderOperation = new SubmitOrderOperation();
        public string SussceOrder = Global.M_ResourceManager.GetString("TradeStr_MainForm_SussceOrder");
        public T8OrderOperation t8OrderOperation = new T8OrderOperation();
        public string Total = Global.M_ResourceManager.GetString("TradeStr_MainFormF2_Total");
        public List<string> transactionsList = new List<string>();
        public TransferInfoCallBack TransferInfo;

        public event SetComboCommodityCallBack SetComboCommodityEvent;

        public event SetTransactionCallBack SetTransactionEvent;

        public OperationManager()
        {
            this.SetOperationDelegate();
        }

        public void DispostOperationManager()
        {
            if (operationManager != null)
            {
                operationManager = null;
            }
            GC.SuppressFinalize(this);
        }

        public void GetCommodityInfoList()
        {
            try
            {
                this.commodityList.Clear();
                this.myCommodityList.Clear();
                DataSet dataSetByXml = new XmlDataSet(Global.ConfigPath + Global.CommCodeXml).GetDataSetByXml();
                this.commodityList.Add(this.StrAll);
                this.myCommodityList.Add(this.StrAll);
                foreach (DataRow row in dataSetByXml.Tables[0].Rows)
                {
                    if ((bool)row["Flag"])
                    {
                        this.myCommodityList.Add(row["commodityCode"].ToString());
                    }
                    else
                    {
                        this.commodityList.Add(row["commodityCode"].ToString());
                    }
                }
                if (this.SetComboCommodityEvent != null)
                {
                    if (this.myCommodityList.Count <= 1)
                    {
                        this.SetComboCommodityEvent(this.commodityList);
                    }
                    else
                    {
                        this.SetComboCommodityEvent(this.myCommodityList);
                    }
                }
                Global.LoadFlag = false;
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, "获取商品信息错误：" + exception.Message);
            }
        }

        public static OperationManager GetInstance()
        {
            if (operationManager == null)
            {
                lock (typeof(OperationManager))
                {
                    if (operationManager == null)
                    {
                        try
                        {
                            operationManager = new OperationManager();
                        }
                        catch (Exception exception)
                        {
                            Logger.wirte(exception);
                        }
                    }
                }
            }
            return operationManager;
        }

        public void GetTransactionInfoList()
        {
            try
            {
                DataSet dataSetByXml = new XmlDataSet(Global.ConfigPath + Global.TrancCodeXml).GetDataSetByXml();
                this.transactionsList.Add(operationManager.StrAll);
                int num = 0;
                foreach (DataRow row in dataSetByXml.Tables[0].Rows)
                {
                    if ((bool)row["Flag"])
                    {
                        string str = row["TransactionsCode"].ToString();
                        if (!Global.FirmID.Equals(str) && (str.Length > 2))
                        {
                            this.myTransactionsList.Add(str);
                        }
                        num++;
                    }
                    else
                    {
                        string str2 = row["TransactionsCode"].ToString();
                        if (!Global.FirmID.Equals(str2) && (str2.Length > 2))
                        {
                            this.transactionsList.Add(str2);
                        }
                    }
                }
                if (this.SetTransactionEvent != null)
                {
                    this.SetTransactionEvent(this.transactionsList);
                }
            }
            catch (Exception exception)
            {
                Logger.wirte(MsgType.Error, exception.StackTrace + exception.Message);
            }
        }

        private void RefreshOrderInfo()
        {
            switch (this.CurrentSelectIndex)
            {
                case RefreshQueryInfo.UnTrade_TradeOrder:
                    this.queryUnOrderOperation.QueryUnOrderInfo();
                    return;

                case RefreshQueryInfo.AllOrder:
                case RefreshQueryInfo.AllOrder_HoldingCollect:
                    this.queryAllOrderOperation.QueryAllOrderInfo();
                    return;

                case RefreshQueryInfo.FundsInfo:
                    this.queryInitDataOperation.QueryFirmInfoThread();
                    return;
            }
        }

        public void SetAllQueryFlag(bool flag)
        {
            this.queryUnOrderOperation.SetQueryUnOrderFlag(flag);
            this.queryTradeOrderOperation.SetQueryUnOrderFlag(flag);
            this.queryAllOrderOperation.SetQueryUnOrderFlag(flag);
            this.queryTradeOperation.SetQueryUnOrderFlag(flag);
            this.queryHoldingOperation.SetQueryUnOrderFlag(flag);
            this.queryHoldingDatailOperation.SetQueryUnOrderFlag(flag);
            this.queryInitDataOperation.SetQueryUnOrderFlag(flag);
            this.queryConOrderOperation.SetQueryUnOrderFlag(flag);
        }

        public void SetIdleOnMoudel()
        {
            this.queryUnOrderOperation.IdleOnMoudel = this.IdleOnMoudel;
            this.queryUnOrderOperation.IdleRefreshButton = this.IdleRefreshButton;
            this.queryTradeOrderOperation.IdleOnMoudel = this.IdleOnMoudel;
            this.queryTradeOrderOperation.IdleRefreshButton = this.IdleRefreshButton;
            this.queryAllOrderOperation.IdleOnMoudel = this.IdleOnMoudel;
            this.queryAllOrderOperation.IdleRefreshButton = this.IdleRefreshButton;
            this.queryTradeOperation.IdleOnMoudel = this.IdleOnMoudel;
            this.queryTradeOperation.IdleRefreshButton = this.IdleRefreshButton;
            this.queryHoldingOperation.IdleOnMoudel = this.IdleOnMoudel;
            this.queryHoldingOperation.IdleRefreshButton = this.IdleRefreshButton;
            this.queryHoldingDatailOperation.IdleOnMoudel = this.IdleOnMoudel;
            this.queryHoldingDatailOperation.IdleRefreshButton = this.IdleRefreshButton;
            this.queryInitDataOperation.IdleOnMoudel = this.IdleOnMoudel;
            this.queryInitDataOperation.IdleRefreshButton = this.IdleRefreshButton;
            this.queryConOrderOperation.IdleOnMoudel = this.IdleOnMoudel;
            this.queryConOrderOperation.IdleRefreshButton = this.IdleRefreshButton;
        }

        private void SetOperationDelegate()
        {
            this.queryInitDataOperation.isPagingQueryCallBack = new QueryInitDataOperation.IsPagingQueryCallBack(this.SetPagingInfo);
            this.queryInitDataOperation.ComboCommodityLoad = new QueryInitDataOperation.ComboCommodityLoadCallBack(this.GetCommodityInfoList);
            this.queryInitDataOperation.TransactionLoad = new QueryInitDataOperation.TransactionLoadCallBack(this.GetTransactionInfoList);
            this.revokeOrderOperation.refreshOrder = new RevokeOrderOperation.refreshOrderCallBack(this.SetQueryFlag);
            this.revokeConOrderOperation.refreshConOrder = new RevokeConOrderOperation.refreshConOrderCallBack(this.SetQueryFlag);
            this.orderOperation.refreshHQ = new OrderOperation.RefreshHQCallBack(this.queryCommDataOperation.RefreshGN);
            this.submitOrderOperation.RefreshCurrentTab = new QueryOperation.RefreshCurrentTabCallBack(this.SetQueryFlag);
            this.querySysTimeOperation.RefreshCurrentTab = new QueryOperation.RefreshCurrentTabCallBack(this.SetQueryFlag);
            this.queryConOrderOperation.RefreshCurrentTab = new QueryOperation.RefreshCurrentTabCallBack(this.SetQueryFlag);
            this.queryCommDataOperation.UpdatePrice = new QueryCommDataOperation.UpdatePriceCallBack(this.orderOperation.UpdatePrice);
            this.submitOrderOperation.SubmitPredelegateInfo = new SubmitOrderOperation.SubmitPredelegateCallBack(this.queryPredelegateOperation.SubmitPredelegateInfo);
            this.queryPredelegateOperation.setMaxID = new QueryPredelegateOperation.SetMaxIDCallBack(this.submitOrderOperation.SetMaxID);
        }

        private void SetPagingInfo(bool isOrderNew, bool isTradeNew)
        {
            this.queryUnOrderOperation.SetUnOrderIsPaging(isOrderNew);
            this.queryAllOrderOperation.SetAllOrderIsPaging(isOrderNew);
            this.queryTradeOperation.SetTradeIsPaging(isTradeNew);
            this.queryTradeOrderOperation.SetTradeOrderIsPaging(isTradeNew);
        }

        private void SetQueryFlag(int mark, bool flag)
        {
            if (mark == 0)
            {
                this.queryUnOrderOperation.SetQueryUnOrderFlag(flag);
                this.queryAllOrderOperation.SetQueryUnOrderFlag(flag);
                this.queryInitDataOperation.SetQueryUnOrderFlag(flag);
                this.RefreshOrderInfo();
            }
            else if (mark == 1)
            {
                this.SetAllQueryFlag(flag);
                this.TabMainSelectIndexChanged();
            }
            else if (mark == 2)
            {
                this.queryConOrderOperation.SetQueryUnOrderFlag(flag);
                if (this.CurrentSelectIndex == RefreshQueryInfo.ConditionOrder)
                {
                    this.queryConOrderOperation.QueryConOrderInfoLoad();
                }
            }
            else if (mark == 3)
            {
                operationManager.queryPredelegateOperation.QueryPreDelegateLoad();
            }
        }

        public void SetRefreshTime(bool type)
        {
            operationManager.queryCommDataOperation.SetRefreshGNTime(type);
            operationManager.querySysTimeOperation.SetSysTimeRefreshTime(type);
        }

        public void ShowHolding(int flag)
        {
            if (flag == 0)
            {
                if (this.ShowHoldingCollect != null)
                {
                    this.ShowHoldingCollect();
                }
            }
            else if ((flag == 1) && (this.ShowHoldingDetail != null))
            {
                this.ShowHoldingDetail();
            }
        }

        public void TabMainSelectIndexChanged()
        {
            switch (this.CurrentSelectIndex)
            {
                case RefreshQueryInfo.UnTrade_TradeOrder:
                    this.queryUnOrderOperation.QueryUnOrderInfoLoad();
                    this.queryTradeOrderOperation.QueryTradeOrderInfoLoad();
                    break;

                case RefreshQueryInfo.AllOrder:
                    this.queryAllOrderOperation.QueryAllOrderInfoLoad();
                    break;

                case RefreshQueryInfo.AllTrade:
                    this.queryTradeOperation.QueryTradeInfoLoad();
                    break;

                case RefreshQueryInfo.HoldingDetail:
                    this.queryHoldingDatailOperation.QueryHoldingDetailInfoLoad();
                    break;

                case RefreshQueryInfo.FundsInfo:
                    this.queryInitDataOperation.QueryFirmInfoThread();
                    break;

                case RefreshQueryInfo.PreDelegates:
                    this.queryPredelegateOperation.QueryPreDelegateLoad();
                    break;

                case RefreshQueryInfo.ConditionOrder:
                    this.queryConOrderOperation.QueryConOrderInfoLoad();
                    break;

                case RefreshQueryInfo.AllOrder_HoldingCollect:
                    this.queryAllOrderOperation.QueryAllOrderInfoLoad();
                    this.queryHoldingOperation.QueryHoldingInfoLoad();
                    break;

                case RefreshQueryInfo.Holding_HoldingDetail:
                    this.queryHoldingOperation.QueryHoldingInfoLoad();
                    this.queryHoldingDatailOperation.QueryHoldingDetailInfoLoad();
                    break;
            }
            this.IdleOnMoudel = 0;
        }

        public delegate void SetComboCommodityCallBack(List<string> commodityIDList);

        public delegate void SetHQTimerEnableCallBack(bool enable);

        public delegate void SetTransactionCallBack(List<string> transactionLis);

        public delegate void ShowHoldingControlCallBack();

        public delegate void TransferInfoCallBack(string info, byte priceQtyFlag);
    }
}
