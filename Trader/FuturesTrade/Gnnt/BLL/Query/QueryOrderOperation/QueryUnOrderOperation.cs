namespace FuturesTrade.Gnnt.BLL.Query.QueryOrderOperation
{
    using FuturesTrade.Gnnt.BLL.Query;
    using FuturesTrade.Gnnt.Library;
    using System;
    using System.Collections;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using TradeInterface.Gnnt.DataVO;

    public class QueryUnOrderOperation : QueryOperation
    {
        private bool isOrderNew;
        private bool isShowPagingControl;
        private byte queryCurrentPageDataFlag;
        private bool QueryPagingDataFirst = true;
        private bool QueryUnOrderFlag = true;
        private bool refreshUnOrderFlag = true;
        private Hashtable sumNamesHashtable = new Hashtable();
        private string[] sumNmaes = new string[] { "Time", "TransactionsCode", "Qty", "Balance" };
        private int unOrderAllNum;
        private int unOrderAllPage;
        private int unOrderCurrentPage = 1;
        public UnOrderFillCallBack UnOrderFill;
        private string unOrderSort = " DESC";
        private string unOrderSortFld = "OrderNo";
        private string unOrderSql = " 1=1 ";

        public QueryUnOrderOperation()
        {
            for (int i = 0; i < this.sumNmaes.Length; i++)
            {
                this.sumNamesHashtable.Add(this.sumNmaes[i], "");
            }
        }

        private OrderQueryPagingRequestVO FillOrderQueryPagingReqVO(object o)
        {
            OrderQueryPagingRequestVO tvo = new OrderQueryPagingRequestVO
            {
                UserID = Global.UserID
            };
            if (o != null)
            {
                tvo.CurrentPagNum = this.unOrderCurrentPage;
            }
            tvo.IsQueryAll = 1;
            tvo.IsDesc = base.GetDescOrAsc(this.unOrderSort);
            tvo.RecordCount = Global.PagNum;
            tvo.SortFld = this.unOrderSortFld;
            if (tvo.SortFld == "Price")
            {
                tvo.SortFld = "OR_P";
            }
            return tvo;
        }

        private OrderQueryRequestVO FillOrderQueryReqVO()
        {
            return new OrderQueryRequestVO { UserID = Global.UserID };
        }

        public void QueryPageUnOrderData(byte buttonMark, int num)
        {
            switch (buttonMark)
            {
                case 0:
                    this.unOrderCurrentPage = 1;
                    break;

                case 1:
                    this.unOrderCurrentPage--;
                    if (this.unOrderCurrentPage < 1)
                    {
                        this.unOrderCurrentPage = 1;
                    }
                    break;

                case 2:
                    this.unOrderCurrentPage++;
                    if (this.unOrderCurrentPage > this.unOrderAllPage)
                    {
                        this.unOrderCurrentPage = this.unOrderAllPage;
                    }
                    break;

                case 3:
                    this.unOrderCurrentPage = this.unOrderAllPage;
                    break;

                case 4:
                    this.unOrderCurrentPage = num;
                    break;
            }
            if (this.isOrderNew)
            {
                this.QueryPagingUnOrderInfoThread(this.queryCurrentPageDataFlag);
            }
            else
            {
                this.QueryUnOrderMemoryInfo(null);
            }
        }

        private void QueryPagingUnOrderInfoThread(object o)
        {
            WaitCallback callBack = new WaitCallback(this.QueryPingOrderInfo);
            ThreadPool.QueueUserWorkItem(callBack, o);
        }

        private void QueryPagingUnOrderMemoryInfo(OrderQueryPagingRequestVO orderQueryPagingRegVO)
        {
            DataSet set = base.serviceManage.CreateIQueryOrder(this.isOrderNew).QueryUnOrderDataSet(orderQueryPagingRegVO);
            this.unOrderAllNum = base.GetAllDataCount(set.Tables["Order"], this.sumNmaes);
            this.UnOrderSetPage();
            if (this.UnOrderFill != null)
            {
                this.UnOrderFill(set.Tables["Order"], this.isShowPagingControl);
            }
        }

        private void QueryPingOrderInfo(object o)
        {
            if (this.refreshUnOrderFlag)
            {
                this.refreshUnOrderFlag = false;
                OrderQueryPagingRequestVO orderQueryPagingRegVO = this.FillOrderQueryPagingReqVO(o);
                this.QueryPagingUnOrderMemoryInfo(orderQueryPagingRegVO);
                this.refreshUnOrderFlag = true;
            }
        }

        public void QueryUnOrderInfo()
        {
            if (this.isOrderNew)
            {
                this.QueryPagingUnOrderMemoryInfo(null);
            }
            else
            {
                this.QueryUnOrderMemoryInfo(null);
            }
        }

        private void QueryUnOrderInfo(object o)
        {
            if (this.refreshUnOrderFlag)
            {
                this.refreshUnOrderFlag = false;
                OrderQueryRequestVO orderQueryReqVO = this.FillOrderQueryReqVO();
                this.QueryUnOrderMemoryInfo(orderQueryReqVO);
                this.refreshUnOrderFlag = true;
            }
        }

        public void QueryUnOrderInfoLoad()
        {
            if (this.QueryUnOrderFlag || base.IsOutRefreshTime())
            {
                base.ButtonRefreshFlag = 0;
                if (this.isOrderNew)
                {
                    this.QueryPagingUnOrderInfoThread(null);
                }
                else
                {
                    this.QueryUnOrderInfoThread();
                }
                this.QueryUnOrderFlag = false;
            }
        }

        private void QueryUnOrderInfoThread()
        {
            WaitCallback callBack = new WaitCallback(this.QueryUnOrderInfo);
            ThreadPool.QueueUserWorkItem(callBack);
        }

        private void QueryUnOrderMemoryInfo(OrderQueryRequestVO orderQueryReqVO)
        {
            DataSet set = base.serviceManage.CreateIQueryOrder(this.isOrderNew).QueryUnOrderDataSet(orderQueryReqVO);
            this.unOrderAllNum = set.Tables["Order"].Rows.Count;
            this.UnOrderSetPage();
            DataTable unOrderDataTable = base.GetDataTable(set.Tables["Order"], this.unOrderSql, this.unOrderSortFld + this.unOrderSort, this.unOrderCurrentPage);
            base.DataViewAddQueryDgUnTradeSum(unOrderDataTable.DefaultView, this.sumNmaes, this.sumNamesHashtable);
            if (this.UnOrderFill != null)
            {
                this.UnOrderFill(unOrderDataTable, this.isShowPagingControl);
            }
        }

        public void SetQueryUnOrderFlag(bool flag)
        {
            this.QueryUnOrderFlag = flag;
        }

        public void SetUnOrderIsPaging(bool isPagingQuery)
        {
            this.isOrderNew = isPagingQuery;
        }

        public void UnOrderDataGridViewSort(string unOrderSortName)
        {
            this.unOrderSortFld = unOrderSortName;
            if (this.unOrderSort == " ASC ")
            {
                this.unOrderSort = " Desc ";
            }
            else
            {
                this.unOrderSort = " ASC ";
            }
            if (this.isOrderNew)
            {
                this.QueryPagingUnOrderInfoThread(this.queryCurrentPageDataFlag);
            }
            else
            {
                this.QueryUnOrderMemoryInfo(null);
            }
        }

        private void UnOrderSetPage()
        {
            if (this.unOrderAllNum > Global.PagNum)
            {
                this.isShowPagingControl = true;
                if ((this.unOrderAllNum % Global.PagNum) == 0)
                {
                    this.unOrderAllPage = this.unOrderAllNum / Global.PagNum;
                }
                else
                {
                    this.unOrderAllPage = (this.unOrderAllNum / Global.PagNum) + 1;
                }
            }
            else
            {
                this.isShowPagingControl = false;
            }
        }

        public int UnOrderAllPage
        {
            get
            {
                return this.unOrderAllPage;
            }
            set
            {
                this.unOrderAllPage = value;
            }
        }

        public int UnOrderCurrentPage
        {
            get
            {
                return this.unOrderCurrentPage;
            }
            set
            {
                this.unOrderCurrentPage = value;
            }
        }

        public delegate void UnOrderFillCallBack(DataTable unOrderDataTable, bool _isShowPagingControl);
    }
}
