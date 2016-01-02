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

    public class QueryAllOrderOperation : QueryOperation
    {
        private int allOrderAllNum;
        private int allOrderAllPage;
        private int allOrderCurrentPage = 1;
        public AllOrderFillCallBack AllOrderFill;
        private string allOrderSort = " DESC";
        private string allOrderSortFld = "OrderNo";
        private string allOrderSql = " 1=1 ";
        private short buySellType;
        private string commodityID = string.Empty;
        private bool isOrderNew;
        private bool isShowPagingControl;
        private short orderStatue;
        private bool QueryAllOrderFlag = true;
        private byte QueryCurrentDataFlag;
        private byte queryCurrentPageDataFlag;
        private bool QueryPagingDataFirst = true;
        private bool refreshAllOrderFlag = true;
        private Hashtable sumNamesHashtable = new Hashtable();
        private string[] sumNmaes = new string[] { "Time", "TransactionsCode", "Qty", "Balance" };

        public QueryAllOrderOperation()
        {
            for (int i = 0; i < this.sumNmaes.Length; i++)
            {
                this.sumNamesHashtable.Add(this.sumNmaes[i], "");
            }
        }

        public void AllOrderDataGridViewSort(string allOrderSortName)
        {
            this.allOrderSortFld = allOrderSortName;
            if (this.allOrderSort == " ASC ")
            {
                this.allOrderSort = " Desc ";
            }
            else
            {
                this.allOrderSort = " ASC ";
            }
            if (this.isOrderNew)
            {
                this.QueryPagingAllOrderInfoThread(this.queryCurrentPageDataFlag);
            }
            else
            {
                this.QueryAllOrderMemoryInfo(null);
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
                tvo.CurrentPagNum = this.allOrderCurrentPage;
            }
            tvo.IsQueryAll = 0;
            tvo.IsDesc = base.GetDescOrAsc(this.allOrderSort);
            tvo.RecordCount = Global.PagNum;
            tvo.SortFld = this.allOrderSortFld;
            if (tvo.SortFld == "Price")
            {
                tvo.SortFld = "OR_P";
            }
            tvo.Pri = this.commodityID;
            tvo.Type = this.buySellType;
            tvo.Sta = this.orderStatue;
            return tvo;
        }

        private OrderQueryRequestVO FillOrderQueryReqVO()
        {
            return new OrderQueryRequestVO { UserID = Global.UserID };
        }

        public void QueryAllOrderInfo()
        {
            if (this.isOrderNew)
            {
                this.QueryPagingOrderMemoryInfo(null);
            }
            else
            {
                this.QueryAllOrderMemoryInfo(null);
            }
        }

        private void QueryAllOrderInfo(object showOrder)
        {
            if (this.refreshAllOrderFlag)
            {
                this.refreshAllOrderFlag = false;
                OrderQueryRequestVO orderQueryReqVO = this.FillOrderQueryReqVO();
                this.QueryAllOrderMemoryInfo(orderQueryReqVO);
                this.refreshAllOrderFlag = true;
            }
        }

        public void QueryAllOrderInfoLoad()
        {
            if (this.QueryAllOrderFlag || base.IsOutRefreshTime())
            {
                base.ButtonRefreshFlag = 0;
                if (this.isOrderNew)
                {
                    this.QueryPagingAllOrderInfoThread(null);
                }
                else
                {
                    this.QueryAllOrderInfoThread();
                }
                this.QueryAllOrderFlag = false;
            }
        }

        private void QueryAllOrderInfoThread()
        {
            WaitCallback callBack = new WaitCallback(this.QueryAllOrderInfo);
            ThreadPool.QueueUserWorkItem(callBack);
        }

        private void QueryAllOrderMemoryInfo(OrderQueryRequestVO orderQueryReqVO)
        {
            DataSet set = base.serviceManage.CreateIQueryOrder(this.isOrderNew).QueryAllOrderDataSet(orderQueryReqVO);
            this.allOrderAllNum = set.Tables["Order"].Rows.Count;
            this.UnOrderSetPage();
            DataTable allOrderDataTable = base.GetDataTable(set.Tables["Order"], this.allOrderSql, this.allOrderSortFld + this.allOrderSort, this.allOrderCurrentPage);
            base.DataViewAddQueryDgUnTradeSum(allOrderDataTable.DefaultView, this.sumNmaes, this.sumNamesHashtable);
            if (this.AllOrderFill != null)
            {
                this.AllOrderFill(allOrderDataTable, this.isShowPagingControl);
            }
        }

        public void QueryPageAllOrderData(byte buttonMark, int num)
        {
            switch (buttonMark)
            {
                case 0:
                    this.allOrderCurrentPage = 1;
                    break;

                case 1:
                    this.allOrderCurrentPage--;
                    if (this.allOrderCurrentPage < 1)
                    {
                        this.allOrderCurrentPage = 1;
                    }
                    break;

                case 2:
                    this.allOrderCurrentPage++;
                    if (this.allOrderCurrentPage > this.allOrderAllPage)
                    {
                        this.allOrderCurrentPage = this.allOrderAllPage;
                    }
                    break;

                case 3:
                    this.allOrderCurrentPage = this.allOrderAllPage;
                    break;

                case 4:
                    this.allOrderCurrentPage = num;
                    break;
            }
            if (this.isOrderNew)
            {
                this.QueryPagingAllOrderInfoThread(this.queryCurrentPageDataFlag);
            }
            else
            {
                this.QueryAllOrderMemoryInfo(null);
            }
        }

        private void QueryPagingAllOrderInfoThread(object o)
        {
            WaitCallback callBack = new WaitCallback(this.QueryPingOrderInfo);
            ThreadPool.QueueUserWorkItem(callBack, o);
        }

        private void QueryPagingOrderMemoryInfo(OrderQueryPagingRequestVO OrderQueryPagingRegVO)
        {
            DataSet set = base.serviceManage.CreateIQueryOrder(this.isOrderNew).QueryAllOrderDataSet(OrderQueryPagingRegVO);
            this.allOrderAllNum = base.GetAllDataCount(set.Tables["Order"], this.sumNmaes);
            this.UnOrderSetPage();
            DataTable allOrderDataTable = base.GetDataTable(set.Tables["Order"], this.allOrderSql, this.allOrderSortFld + this.allOrderSort, 1);
            if (this.AllOrderFill != null)
            {
                this.AllOrderFill(allOrderDataTable, this.isShowPagingControl);
            }
        }

        private void QueryPingOrderInfo(object o)
        {
            if (this.refreshAllOrderFlag)
            {
                this.refreshAllOrderFlag = false;
                OrderQueryPagingRequestVO orderQueryPagingRegVO = this.FillOrderQueryPagingReqVO(o);
                this.QueryPagingOrderMemoryInfo(orderQueryPagingRegVO);
                this.refreshAllOrderFlag = true;
            }
        }

        public void ScreeningAllOrderData(string _commodityID, short _buySellType, short _orderStatue, string sql)
        {
            this.commodityID = _commodityID;
            this.orderStatue = _orderStatue;
            this.buySellType = _buySellType;
            this.allOrderSql = sql;
            this.allOrderCurrentPage = 1;
            if (this.isOrderNew)
            {
                this.QueryPagingAllOrderInfoThread(null);
            }
            else
            {
                this.QueryAllOrderMemoryInfo(null);
            }
        }

        public void SetAllOrderIsPaging(bool isPagingQuery)
        {
            this.isOrderNew = isPagingQuery;
        }

        public void SetQueryUnOrderFlag(bool flag)
        {
            this.QueryAllOrderFlag = flag;
        }

        private void UnOrderSetPage()
        {
            if (this.allOrderAllNum > Global.PagNum)
            {
                this.isShowPagingControl = true;
                if ((this.allOrderAllNum % Global.PagNum) == 0)
                {
                    this.allOrderAllPage = this.allOrderAllNum / Global.PagNum;
                }
                else
                {
                    this.allOrderAllPage = (this.allOrderAllNum / Global.PagNum) + 1;
                }
            }
            else
            {
                this.isShowPagingControl = false;
            }
        }

        public int AllOrderAllPage
        {
            get
            {
                return this.allOrderAllPage;
            }
            set
            {
                this.allOrderAllPage = value;
            }
        }

        public int AllOrderCurrentPage
        {
            get
            {
                return this.allOrderCurrentPage;
            }
            set
            {
                this.allOrderCurrentPage = value;
            }
        }

        public delegate void AllOrderFillCallBack(DataTable allOrderDataTable, bool _isShowPagingControl);
    }
}
