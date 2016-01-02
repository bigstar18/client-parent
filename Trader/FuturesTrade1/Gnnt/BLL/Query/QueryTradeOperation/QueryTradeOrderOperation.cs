namespace FuturesTrade.Gnnt.BLL.Query.QueryTradeOperation
{
    using FuturesTrade.Gnnt.BLL.Query;
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using TradeInterface.Gnnt.DataVO;

    public class QueryTradeOrderOperation : QueryOperation
    {
        private bool isShowPagingControl;
        private bool isTradeOrderNew;
        private byte queryCurrentPageDataFlag;
        private bool QueryPagingDataFirst = true;
        private bool QueryTradeOrderFlag = true;
        private bool refreshTradeOrderFlag = true;
        private Hashtable sumNamesHashtable = new Hashtable();
        private string[] sumNmaes = new string[] { "Time", "TransactionsCode", "Qty" };
        private int tradeOrderAllNum;
        private int tradeOrderAllPage;
        private int tradeOrderCurrentPage = 1;
        public TradeOrderFillCallBack TradeOrderFill;
        private string tradeOrderSort = " DESC";
        private string tradeOrderSortFld = "TradeNo";
        private string tradeOrderSql = " 1=1 ";

        public QueryTradeOrderOperation()
        {
            for (int i = 0; i < this.sumNmaes.Length; i++)
            {
                this.sumNamesHashtable.Add(this.sumNmaes[i], "");
            }
        }

        private TradeQueryPagingRequestVO FillTradeOrderQueryPagingReqVO(object o)
        {
            TradeQueryPagingRequestVO tvo = new TradeQueryPagingRequestVO
            {
                UserID = Global.UserID
            };
            if (o != null)
            {
                tvo.CurrentPagNum = this.tradeOrderCurrentPage;
            }
            tvo.IsDesc = base.GetDescOrAsc(this.tradeOrderSort);
            tvo.RecordCount = Global.PagNum;
            tvo.SortFld = this.tradeOrderSortFld;
            if (tvo.SortFld == "Price")
            {
                tvo.SortFld = "TR_P";
            }
            return tvo;
        }

        private TradeQueryRequestVO FillTradeOrderQueryReqVO()
        {
            return new TradeQueryRequestVO { UserID = Global.UserID };
        }

        public void QueryPageTradeOrderData(byte buttonMark, int num)
        {
            switch (buttonMark)
            {
                case 0:
                    this.tradeOrderCurrentPage = 1;
                    break;

                case 1:
                    this.tradeOrderCurrentPage--;
                    if (this.tradeOrderCurrentPage < 1)
                    {
                        this.tradeOrderCurrentPage = 1;
                    }
                    break;

                case 2:
                    this.tradeOrderCurrentPage++;
                    if (this.tradeOrderCurrentPage > this.tradeOrderAllPage)
                    {
                        this.tradeOrderCurrentPage = this.tradeOrderAllPage;
                    }
                    break;

                case 3:
                    this.tradeOrderCurrentPage = this.tradeOrderAllPage;
                    break;

                case 4:
                    this.tradeOrderCurrentPage = num;
                    break;
            }
            if (this.isTradeOrderNew)
            {
                this.QueryPagingTradeOrderInfoThread(this.queryCurrentPageDataFlag);
            }
            else
            {
                this.QueryTradeOrderMemoryInfo(null);
            }
        }

        private void QueryPagingTradeOrderInfoThread(object o)
        {
            WaitCallback callBack = new WaitCallback(this.QueryPingTradeOrderInfo);
            ThreadPool.QueueUserWorkItem(callBack, o);
        }

        private void QueryPagingTradeOrderMemoryInfo(TradeQueryPagingRequestVO tradeOrderQueryPagingRegVO)
        {
            DataSet set = base.serviceManage.CreateIQueryTrade(this.isTradeOrderNew).QueryTradeOrderDataSet(tradeOrderQueryPagingRegVO);
            this.tradeOrderAllNum = base.GetAllDataCount(set.Tables["Trade"], this.sumNmaes);
            this.TradeOrderSetPage();
            DataTable tradeOrderDataTable = base.GetDataTable(set.Tables["Trade"], this.tradeOrderSql, this.tradeOrderSortFld + this.tradeOrderSort, 1);
            if (this.TradeOrderFill != null)
            {
                this.TradeOrderFill(tradeOrderDataTable, this.isShowPagingControl);
            }
        }

        private void QueryPingTradeOrderInfo(object o)
        {
            if (this.refreshTradeOrderFlag)
            {
                this.refreshTradeOrderFlag = false;
                TradeQueryPagingRequestVO tradeOrderQueryPagingRegVO = this.FillTradeOrderQueryPagingReqVO(o);
                this.QueryPagingTradeOrderMemoryInfo(tradeOrderQueryPagingRegVO);
                this.refreshTradeOrderFlag = true;
            }
        }

        private void QueryTradeOrderInfo(object o)
        {
            if (this.refreshTradeOrderFlag)
            {
                this.refreshTradeOrderFlag = false;
                TradeQueryRequestVO tradeOrderQueryReqVO = this.FillTradeOrderQueryReqVO();
                this.QueryTradeOrderMemoryInfo(tradeOrderQueryReqVO);
                this.refreshTradeOrderFlag = true;
            }
        }

        public void QueryTradeOrderInfoLoad()
        {
            if (this.QueryTradeOrderFlag || base.IsOutRefreshTime())
            {
                base.ButtonRefreshFlag = 0;
                if (this.isTradeOrderNew)
                {
                    this.QueryPagingTradeOrderInfoThread(null);
                }
                else
                {
                    this.QueryTradeOrderInfoThread();
                }
                this.QueryTradeOrderFlag = false;
            }
        }

        private void QueryTradeOrderInfoThread()
        {
            WaitCallback callBack = new WaitCallback(this.QueryTradeOrderInfo);
            ThreadPool.QueueUserWorkItem(callBack);
        }

        private void QueryTradeOrderMemoryInfo(TradeQueryRequestVO tradeOrderQueryReqVO)
        {
            DataSet set = base.serviceManage.CreateIQueryTrade(this.isTradeOrderNew).QueryTradeOrderDataSet(tradeOrderQueryReqVO);
            this.tradeOrderAllNum = set.Tables["Trade"].Rows.Count;
            this.TradeOrderSetPage();
            DataTable tradeOrderDataTable = base.GetDataTable(set.Tables["Trade"], this.tradeOrderSql, this.tradeOrderSortFld + this.tradeOrderSort, this.tradeOrderCurrentPage);
            base.DataViewAddQueryDgUnTradeSum(tradeOrderDataTable.DefaultView, this.sumNmaes, this.sumNamesHashtable);
            if (this.TradeOrderFill != null)
            {
                this.TradeOrderFill(tradeOrderDataTable, this.isShowPagingControl);
            }
        }

        public void SetQueryUnOrderFlag(bool flag)
        {
            this.QueryTradeOrderFlag = flag;
        }

        public void SetTradeOrderIsPaging(bool isPagingQuery)
        {
            this.isTradeOrderNew = isPagingQuery;
        }

        public void TradeOrderDataGridViewSort(string tradeOrderSortName)
        {
            this.tradeOrderSortFld = tradeOrderSortName;
            if (this.tradeOrderSort == " ASC ")
            {
                this.tradeOrderSort = " Desc ";
            }
            else
            {
                this.tradeOrderSort = " ASC ";
            }
            if (this.isTradeOrderNew)
            {
                this.QueryPagingTradeOrderInfoThread(this.queryCurrentPageDataFlag);
            }
            else
            {
                this.QueryTradeOrderMemoryInfo(null);
            }
        }

        private void TradeOrderSetPage()
        {
            if (this.tradeOrderAllNum > Global.PagNum)
            {
                this.isShowPagingControl = true;
                if ((this.tradeOrderAllNum % Global.PagNum) == 0)
                {
                    this.tradeOrderAllPage = this.tradeOrderAllNum / Global.PagNum;
                }
                else
                {
                    this.tradeOrderAllPage = (this.tradeOrderAllNum / Global.PagNum) + 1;
                }
            }
            else
            {
                this.isShowPagingControl = false;
            }
        }

        public int TradeOrderAllPage
        {
            get
            {
                return this.tradeOrderAllPage;
            }
            set
            {
                this.tradeOrderAllPage = value;
            }
        }

        public int TradeOrderCurrentPage
        {
            get
            {
                return this.tradeOrderCurrentPage;
            }
            set
            {
                this.tradeOrderCurrentPage = value;
            }
        }

        public delegate void TradeOrderFillCallBack(DataTable tradeOrderDataTable, bool _isShowPagingControl);
    }
}
